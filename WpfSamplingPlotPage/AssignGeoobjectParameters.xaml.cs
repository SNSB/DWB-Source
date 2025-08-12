using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfSamplingPlotPage
{
    /// <summary>
    /// Interaktionslogik für AssignGeoobjectParameters.xaml
    /// </summary>
    public partial class AssignGeoobjectParameters : Window
    {
        #region Constants

        // Carriage-Return Separator string
        internal const string StrCR = "\r\n";
        private const string StrTAB = "\t";
        private const int IndexGeography = 9;
        private const string StrLongitude = "longitude";
        private const string StrLatitude = "latitude";
        private string StrPolygonMTB = "POLYGON(({0} {1}, {2} {3}, {4} {5}, {6} {7}, {8} {9}))";
        private string StrCenterPointMTB = "POINT({0} {1})";

        #endregion
        
        #region Fields

        private WpfControl m_WpfControl = null;
        private string[] m_DefaultParameter = { "Id", "Text", "Red", "Yellow", "255", "64", "1.00", "Pin", "1.00", "POINT(longitude latitude)", "longitude", "latitude", "" };
        private string[] m_Parameter = { "Id", "Text", "Red", "Yellow", "255", "64", "1.00", "Pin", "1.00", "" };
        private string[] m_ParameterNames = { "Identifier:", "Display Text:", "Stroke Color:", "Fill Color:", "Stroke Transparency:", "Fill Transparency:", "Stroke Thickness:", "Point Type:", "Point SymbolSize:", "Geography Data:", "Longitude:", "Latitude:", "MTB/Q:" };
        private int[] m_Assignment = new int[13];
        private bool m_CenterPoint = false;
        private Label[] m_Labels = new Label[13];
        private CheckBox[] m_CheckBoxes = new CheckBox[13];
        private bool m_SkipCheckedEvent = false;
        private string m_MtbGeography = string.Empty;
        private string m_MtbGeographyPolygon = string.Empty;
        private string m_MtbGeographyPoint = string.Empty;
        private string m_Longitude = "longitude";
        string m_Latitude = "latitude";
        private bool m_SaveAsSingleSample = false;
        private int m_Line = 0;

        private List<string> m_Row = new List<string>();

        // Conversion Methods for geographic coordinates
        private GeoConversion.GeoCon m_GeoCon = new GeoConversion.GeoCon();

        #endregion // Fields

        #region Properties

        /// <summary>
        /// Return SaveAsSingleSample flag
        /// </summary>
        public bool SaveAsSingleSample
        {
            get { return m_SaveAsSingleSample; }
        }

        #endregion // Properties

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="wpfControl">Calling instance object.</param>
        /// <param name="row"></param>
        public AssignGeoobjectParameters(WpfControl wpfControl, List<string> row)
        {
            InitializeComponent();
            // Localize the detection window
            this.Title = WpfSamplingPlotPage.Properties.Resources.AssignWindowTitle;
            labelSelectInput.Content = WpfSamplingPlotPage.Properties.Resources.AssignLabelSelectInput;
            labelAssignParameter.Content = WpfSamplingPlotPage.Properties.Resources.AssignLabelAssignParameter;
            labelCurrentlyAssigned.Content = WpfSamplingPlotPage.Properties.Resources.AssignLabelCurrentlyAssigned;
            checkBoxSingleObject.Content = WpfSamplingPlotPage.Properties.Resources.AssignCheckBoxSingleObject;
            checkBoxCenterPoint.Content = WpfSamplingPlotPage.Properties.Resources.AssignCheckBoxCenterPoint;
            buttonAssignAll.Content = WpfSamplingPlotPage.Properties.Resources.AssignButtonAssignAll;
            buttonRemoveAll.Content = WpfSamplingPlotPage.Properties.Resources.AssignButtonRemoveAll;
            buttonAssignLast.Content = WpfSamplingPlotPage.Properties.Resources.AssignButtonAssignLast;
            buttonOK.Content = WpfSamplingPlotPage.Properties.Resources.ctrlButtonOK;
            buttonCancel.Content = WpfSamplingPlotPage.Properties.Resources.ctrlButtonCancel;

            // Assign input parameters
            m_WpfControl = wpfControl;
            foreach (string str in row)
            {
                m_Row.Add(str);
            }
            m_Line = 0;
            
            // Preset default parameter values
            m_DefaultParameter[2] = m_WpfControl.SetNameOfBrush(m_WpfControl.m_LastStrokeBrush);
            m_DefaultParameter[3] = m_WpfControl.SetNameOfBrush(m_WpfControl.m_LastFillBrush);
            m_DefaultParameter[4] = m_WpfControl.m_StrokeTransparency.ToString();
            m_DefaultParameter[5] = m_WpfControl.m_FillTransparency.ToString();
            m_DefaultParameter[6] = m_WpfControl.m_PointsStrokeThickness.ToString(CultureInfo.InvariantCulture);
            m_DefaultParameter[7] = m_WpfControl.m_PointSymbol.ToString();
            m_DefaultParameter[8] = m_WpfControl.m_PointSymbolSize.ToString(CultureInfo.InvariantCulture);
            m_DefaultParameter[IndexGeography] = string.Format(StrCenterPointMTB, m_Longitude, m_Latitude);
            for (int i = 0; i < IndexGeography; i++)
                m_Parameter[i] = m_DefaultParameter[i];

            // Init parameter checkboxes and labels
            for (int i = 0; i < m_ParameterNames.Length; i++)
            {
                // Checkboxes
                m_CheckBoxes[i] = new CheckBox();
                m_CheckBoxes[i].Checked += checkBox_Checked;
                m_CheckBoxes[i].Unchecked += checkBox_Unchecked;
                m_CheckBoxes[i].Height = 16;
                m_CheckBoxes[i].Content = m_ParameterNames[i];
                m_CheckBoxes[i].DataContext = i;
                if (i > IndexGeography)
                    m_CheckBoxes[i].Margin = new Thickness(15, 0, 0, 0);
                Parameters.Children.Add(m_CheckBoxes[i]);

                // Labels
                m_Labels[i] = new Label();
                m_Labels[i].Height = 16;
                m_Labels[i].Padding = new Thickness(0);
                m_Labels[i].Content = m_DefaultParameter[i];
                if (i == 9)
                    m_Labels[i].Foreground = Brushes.Red;
                else
                    m_Labels[i].Foreground = Brushes.LightGray;
                Assigned.Children.Add(m_Labels[i]);
            }

            // Assign last settings
            checkBoxCenterPoint.IsChecked = m_WpfControl.m_CenterPoint;

            // Init assigned input parameters
            for (int i = 0; i < m_Assignment.Length; i++)
            {
                m_Assignment[i] = -1;
            }

            int rbIndex = 0;
            // Set checkboxes with content of first input list
            foreach (string attribute in m_Row)
            {
                string str = attribute;
                RadioButton radioButton = new RadioButton();
                if (attribute.Length > 100)
                    str = attribute.Substring(0, 100) + "...";
                radioButton.Content = str;
                radioButton.DataContext = rbIndex;

                AssignFields.Children.Add(radioButton);
                if (rbIndex == 0)
                    radioButton.IsChecked = true;
                rbIndex++;
            }

            // Init checkbox Save As Single Sample
            SetSaveAsSingleSampleStatus();
        }

        #endregion // Constructor

        #region Event handlers
        
        /// <summary>
        /// Event handler for checked event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            // Ignore Checked event in case of assign all parameters
            if (m_SkipCheckedEvent)
                return;
            CheckBox checkbox = sender as CheckBox;
            m_SkipCheckedEvent = true;
            AssignParameter(checkbox);
            m_SkipCheckedEvent = false;
        }

        /// <summary>
        /// Event handler for unchecked event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Ignore Unchecked event in case of remove all parameters
            if (m_SkipCheckedEvent)
                return;
            CheckBox checkbox = sender as CheckBox;
            m_SkipCheckedEvent = true;
            RemoveParameter(checkbox);
            m_SkipCheckedEvent = false;
        }

        /// <summary>
        /// Event handler for Assign All click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_AssignAll_Click(object sender, RoutedEventArgs e)
        {
            m_SkipCheckedEvent = true;
            AssignAllParameters();
            m_SkipCheckedEvent = false;
        }

        /// <summary>
        /// Event handler for Remove All click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_RemoveAll_Click(object sender, RoutedEventArgs e)
        {
            m_SkipCheckedEvent = true;
            RemoveAllParameters(); 
            m_SkipCheckedEvent = false;
        }
        
        /// <summary>
        /// Event handler for Assign Last click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_AssignLast_Click(object sender, RoutedEventArgs e)
        {
            // Get last assignment settings
            for (int i = 0; i < m_Assignment.Length; i++)
            {
                m_Assignment[i] = m_WpfControl.m_Assignment[i];
            }

            // Set check boxes accordingly
            for (int i = 0; i < m_Assignment.Length; i++)
            {
                if (m_Assignment[i] < m_Row.Count)
                {
                    if (m_Assignment[i] == -1)
                        m_CheckBoxes[i].IsChecked = false;
                    else
                    {
                        // Find radiobutton by index parameter
                        foreach (RadioButton radiobutton in AssignFields.Children)
                        {
                            int rbIndex = Convert.ToInt32(radiobutton.DataContext);
                            if (rbIndex == m_Assignment[i])
                            {
                                radiobutton.IsChecked = true;
                                break;
                            }
                        }
                        // Assign checked radio button to check box
                        m_CheckBoxes[i].IsChecked = true;
                    }
                }

            }
        }

        private void checkBoxSingleObject_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkbox = sender as CheckBox;
            m_SaveAsSingleSample = true;
        }
        
        private void checkBoxSingleObject_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkbox = sender as CheckBox;
            m_SaveAsSingleSample = false;
        }

        private void checkBoxCenterPoint_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkbox = sender as CheckBox;
            if (m_MtbGeography != string.Empty)
                m_Labels[IndexGeography].Content = m_MtbGeographyPoint;
            m_CenterPoint = true;
        }

        private void checkBoxCenterPoint_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkbox = sender as CheckBox;
            if (m_MtbGeography != string.Empty)
                m_Labels[IndexGeography].Content = m_MtbGeographyPolygon;
            m_CenterPoint = false;
        }


        /// <summary>
        /// Event handler for OK click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_OK_Click(object sender, RoutedEventArgs e)
        {
            // Save last assignment settings
            // for (int i = 0; i < m_Assignment.Length; i++)
            // {
            //     m_WpfControl.m_Assignment[i] = m_Assignment[i];
            // }
            m_WpfControl.m_Assignment[0] = WpfSamplingPlotPage.Properties.Settings.Default.Assign0 = m_Assignment[0];
            m_WpfControl.m_Assignment[1] = WpfSamplingPlotPage.Properties.Settings.Default.Assign1 = m_Assignment[1];
            m_WpfControl.m_Assignment[2] = WpfSamplingPlotPage.Properties.Settings.Default.Assign2 = m_Assignment[2];
            m_WpfControl.m_Assignment[3] = WpfSamplingPlotPage.Properties.Settings.Default.Assign3 = m_Assignment[3];
            m_WpfControl.m_Assignment[4] = WpfSamplingPlotPage.Properties.Settings.Default.Assign4 = m_Assignment[4];
            m_WpfControl.m_Assignment[5] = WpfSamplingPlotPage.Properties.Settings.Default.Assign5 = m_Assignment[5];
            m_WpfControl.m_Assignment[6] = WpfSamplingPlotPage.Properties.Settings.Default.Assign6 = m_Assignment[6];
            m_WpfControl.m_Assignment[7] = WpfSamplingPlotPage.Properties.Settings.Default.Assign7 = m_Assignment[7];
            m_WpfControl.m_Assignment[8] = WpfSamplingPlotPage.Properties.Settings.Default.Assign8 = m_Assignment[8];
            m_WpfControl.m_Assignment[9] = WpfSamplingPlotPage.Properties.Settings.Default.Assign9 = m_Assignment[9];
            m_WpfControl.m_Assignment[10] = WpfSamplingPlotPage.Properties.Settings.Default.Assign10 = m_Assignment[10];
            m_WpfControl.m_Assignment[11] = WpfSamplingPlotPage.Properties.Settings.Default.Assign11 = m_Assignment[11];
            m_WpfControl.m_Assignment[12] = WpfSamplingPlotPage.Properties.Settings.Default.Assign12 = m_Assignment[12];
            m_WpfControl.m_CenterPoint = WpfSamplingPlotPage.Properties.Settings.Default.CenterPoint = m_CenterPoint;
            // Save settings
            WpfSamplingPlotPage.Properties.Settings.Default.Save();

            // Adjust flag if check is disabled
            if (checkBoxSingleObject.IsEnabled == false)
                m_SaveAsSingleSample = false;

            DialogResult = true;
            // Close the window
            this.Close();
        }

        /// <summary>
        /// vent handler for Cancel click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Close the window
            this.Close();
        }

        #endregion // Event handlers

        #region Methods

        /// <summary>
        /// Assign an input parameter (checkbox) to a selected Geo-Object field (radiobutton).
        /// </summary>
        /// <param name="checkbox">Checkbox.</param>
        private void AssignParameter(CheckBox checkbox)
        {
            // Find selected input parameter (radiobutton)
            foreach (RadioButton radiobutton in AssignFields.Children)
            {
                // If found, assign input value to Geo-Object parameter
                if (radiobutton.IsChecked.Value == true)
                {
                    int rbIndex = Convert.ToInt32(radiobutton.DataContext);
                    int cbIndex = Convert.ToInt32(checkbox.DataContext);
                    m_Assignment[cbIndex] = rbIndex;
                    string parameter = radiobutton.Content.ToString();
                    // Check if longitude or latitude
                    if (cbIndex >= IndexGeography)
                        HandleGeographySettings(cbIndex, parameter, true);
                    // Set parameter
                    m_Labels[cbIndex].Content = parameter;
                    m_Labels[cbIndex].Foreground = Brushes.Black;
                    CheckAssignedParameter(cbIndex, parameter);
                    break;
                }
            }
        }

        /// <summary>
        /// Assign all input parameters to the available Geo-Object fields and check checkboxes.
        /// </summary>
        private void AssignAllParameters()
        {

            foreach (RadioButton radiobutton in AssignFields.Children)
            {
                int rbIndex = Convert.ToInt32(radiobutton.DataContext);
                if (rbIndex < m_Parameter.Length)
                {
                    m_Assignment[rbIndex] = rbIndex;
                    m_CheckBoxes[rbIndex].IsChecked = true;
                    m_Labels[rbIndex].Content = radiobutton.Content;
                    m_Labels[rbIndex].Foreground = Brushes.Black;
                }
            }
            SetSaveAsSingleSampleStatus();
        }

        /// <summary>
        /// Check if assigned parameter is valid. If not, set label color to red.
        /// </summary>
        /// <param name="index">Parameter index.</param>
        /// <param name="parameter">Parameter string.</param>
        private bool CheckAssignedParameter(int index, string parameter)
        {
            bool invalidParameter = false;

            try
            {
                switch(index)
                {
                    // Identifier, Display Text: Any text
                    case 0:
                    case 1:
                        break;
                    // Stroke Color, Fill Color: Defined brush only
                    case 2:
                    case 3:
                        invalidParameter = true;
                        foreach (PropertyInfo prop in m_WpfControl.m_BrushesProps)
                        {
                            // check brush name
                            if (prop.Name == parameter)
                            {
                                invalidParameter = false;
                                break;
                            }
                        }
                        break;
                    // Stroke Transparency, Fill Transparency: Integer value from 0 to 255
                    case 4:
                    case 5:
                        int transparency = Convert.ToInt32(parameter);
                        if (transparency < 0 || transparency > 255)
                            invalidParameter = true;
                        break;
                    // Stroke Thickness: Double value > 0 (max. 10?)
                    case 6:
                        double thickness = Convert.ToDouble(parameter, CultureInfo.InvariantCulture);
                        if (thickness <= 0)
                            invalidParameter = true;
                        break;
                    // Point Type: Defined Enum value only
                    case 7:
                        invalidParameter = true;
                        for (PointSymbol pointSymbol = PointSymbol.Pin; pointSymbol < PointSymbol.None; pointSymbol++)
                        {
                            // check brush name
                            if (pointSymbol.ToString() == parameter)
                            {
                                invalidParameter = false;
                                break;
                            }
                        }
                        break;
                    // Point SymbolSize: Double value > 0 (max. 10?)
                    case 8:
                        double size = Convert.ToDouble(parameter, CultureInfo.InvariantCulture);
                        if (size <= 0)
                            invalidParameter = true;
                        break;
                    // Geography Data: Not checked yet
                    case 9:
                        break;
                    // Longitude: Double value from -180 to 180
                    case 10:
                        double lon = Convert.ToDouble(parameter, CultureInfo.InvariantCulture);
                        if (lon < -180 || lon > 180)
                            invalidParameter = true;
                        break;
                    // Latitude: Double value from -90 to 90
                    case 11:
                        double lat = Convert.ToDouble(parameter, CultureInfo.InvariantCulture);
                        if (lat < -180 || lat > 180)
                            invalidParameter = true;
                        break;
                    // TODO:  MTB/QQQ check !!!
                    case 12:
                        if (parameter.Length < 4)
                            invalidParameter = true;
                        for (int i = 0; i < parameter.Length; i++)
                        {
                            if (i >= 0 && i <= 3)
                                if (parameter[i] < '0' || parameter[i] > '9')
                                    invalidParameter = true;
                            if (i == 4)
                                if (parameter[i] != '/')
                                    invalidParameter = true;
                            if (i >= 5 && i <= 7)
                                if (parameter[i] < '1' || parameter[i] > '4')
                                    invalidParameter = true;
                            if (i > 7)
                                break;

                        }
                        break;
                }
            }
            catch
            {
                invalidParameter = true;
            }
            // Print label in red, if invalid
            if (invalidParameter)
            {
                m_Labels[index].Foreground = Brushes.Red;
            }
            return !invalidParameter;
        }

        /// <summary>
        /// Remove an input parameter (radiobutton) from the assigned Geo-Object field.
        /// </summary>
        /// <param name="checkbox"></param>
        private void RemoveParameter(CheckBox checkbox)
        {
            int cbIndex = Convert.ToInt32(checkbox.DataContext);
            m_Assignment[cbIndex] = -1;
            string parameter = m_DefaultParameter[cbIndex];
            // Set parameter
            m_Labels[cbIndex].Content = parameter;
            if (cbIndex == 9)
                m_Labels[cbIndex].Foreground = Brushes.Red;
            else
                m_Labels[cbIndex].Foreground = Brushes.LightGray;
            // Check if longitude or latitude
            if (cbIndex >= IndexGeography)
                HandleGeographySettings(cbIndex, parameter, false);
        }

        /// <summary>
        /// Remove all input parameters from the Geo-Object and uncheck checkboxes.
        /// </summary>
        private void RemoveAllParameters()
        {
            for (int i = 0; i < m_CheckBoxes.Length; i++)
            {
                m_Assignment[i] = -1;
                m_CheckBoxes[i].IsChecked = false;
                m_Labels[i].Content = m_DefaultParameter[i];
                if (i == 9)
                    m_Labels[i].Foreground = Brushes.Red;
                else
                    m_Labels[i].Foreground = Brushes.LightGray;
            }
        }


        /// <summary>
        /// Handles the interactions between assigning a Geography or integrating Longitude and Latitude
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="check">true if set, false if remove</param>
        private void HandleGeographySettings(int index, string parameter, bool check)
        {
            switch (index)
            {
                case IndexGeography:
                    if (check)
                    {
                        m_CheckBoxes[10].IsChecked = false;
                        m_Longitude = StrLongitude;
                        m_Labels[10].Content = StrLongitude;
                        m_Labels[10].Foreground = Brushes.LightGray;
                        m_CheckBoxes[11].IsChecked = false;
                        m_Latitude = StrLatitude;
                        m_Labels[11].Content = StrLatitude;
                        m_Labels[11].Foreground = Brushes.LightGray;
                        m_CheckBoxes[12].IsChecked = false;
                        m_MtbGeography = string.Empty;
                        m_Labels[12].Content = string.Empty;
                    }
                    break;
                case 10:
                    if (check)
                    {
                        m_CheckBoxes[IndexGeography].IsChecked = false;
                        m_Longitude = parameter;
                        m_Labels[IndexGeography].Content = string.Format(StrCenterPointMTB, m_Longitude, m_Latitude);
                    }
                    else
                    {
                        m_Longitude = StrLongitude;
                        m_Labels[IndexGeography].Content = string.Format(StrCenterPointMTB, m_Longitude, m_Latitude);
                    }
                    SetGeographyForeground();
                    break;
                case 11:
                    if (check)
                    {
                        m_CheckBoxes[IndexGeography].IsChecked = false;
                        m_Latitude = parameter;
                        m_Labels[IndexGeography].Content = string.Format(StrCenterPointMTB, m_Longitude, m_Latitude);
                    }
                    else
                    {
                        m_Latitude = StrLatitude;
                        m_Labels[IndexGeography].Content = string.Format(StrCenterPointMTB, m_Longitude, m_Latitude);
                    }
                    SetGeographyForeground();
                    break;
                case 12:
                    m_MtbGeography = string.Empty;
                    if (check)
                    {
                        if (ConvertMTBtoLonLat(parameter, ref m_MtbGeography))
                        {
                            m_CheckBoxes[10].IsChecked = false;
                            m_CheckBoxes[11].IsChecked = false;
                            m_CheckBoxes[IndexGeography].IsChecked = false;
                            // m_Labels[10].Content = m_Longitude;
                            // m_Labels[10].Foreground = Brushes.Black;
                            // m_Labels[11].Content = m_Latitude;
                            // m_Labels[11].Foreground = Brushes.Black;
                            if (m_MtbGeography != string.Empty)
                                m_Labels[IndexGeography].Content = m_MtbGeography;
                            SetGeographyForeground();
                        }
                    }
                    else
                    {
                        if (m_Longitude != StrLongitude)
                            m_CheckBoxes[10].IsChecked = true;
                        if (m_Latitude != StrLatitude)
                            m_CheckBoxes[11].IsChecked = true;
                        // m_Latitude = StrLatitude;
                        // m_Labels[10].Foreground = Brushes.LightGray;
                        // m_Labels[11].Foreground = Brushes.LightGray;
                        m_Labels[IndexGeography].Content = string.Format(StrCenterPointMTB, m_Longitude, m_Latitude);
                        SetGeographyForeground();
                    }
                    break;

            }
            SetSaveAsSingleSampleStatus();
        }

        /// <summary>
        /// Set the color for the geography parameter.
        /// </summary>
        private void SetGeographyForeground()
        {
            /* Check if valid parameter and set color (red = missing values).
            if (m_Longitude == StrLongitude || m_Latitude == StrLatitude)
                m_Labels[IndexGeography].Foreground = Brushes.Red;
            else
                m_Labels[IndexGeography].Foreground = Brushes.Black;
            */

            if ((m_Longitude != StrLongitude && m_Latitude != StrLatitude) || m_MtbGeography != string.Empty)
                m_Labels[IndexGeography].Foreground = Brushes.Black;
            else
                m_Labels[IndexGeography].Foreground = Brushes.Red;

        }

        /// <summary>
        /// Disable checkBoxSingleObject if longitude and latitude both are not set.
        /// </summary>
        private void SetSaveAsSingleSampleStatus()
        {
            if (m_Longitude == StrLongitude || m_Latitude == StrLatitude)
            {
                checkBoxSingleObject.IsEnabled = false;
            }
            else
            {
                checkBoxSingleObject.IsEnabled = true; 
            }
        }


        /// <summary>
        /// Returns the result sample string of the input parameters according to the assingment.
        /// </summary>
        /// <returns>Resulting samples string.</returns>
        public string GetResult(List<string> row)
        {
            // Resulting samples string
            string resultStr = string.Empty;

            // Check for consistency (all rows must have the same size)
            if (row.Count != m_Row.Count)
                return resultStr;

            // Init parameters string array with default values
            string[] parameters = m_Parameter;
            m_Longitude = StrLongitude;
            m_Latitude = StrLatitude;

            // Check for parameter assignments
            for (int i = 0; i < m_Assignment.Length; i++)
            {
                // Get associated row index from assignment
                int index = m_Assignment[i];

                // Assign values to parameters array
                if (index != -1 && i < parameters.Length)
                {
                    parameters[i] = row[index];
                }
                else if (index != -1 && i == 10)
                {
                    m_Longitude = row[index].Replace(",", ".");
                }
                else if (index != -1 && i == 11)
                {
                    m_Latitude = row[index].Replace(",", ".");
                }
                else if (index != -1 && i == 12)
                {
                    m_MtbGeography = string.Empty;
                    ConvertMTBtoLonLat(row[index], ref m_MtbGeography);
                }
            }

            // If assigned: Create POINT geography from longitude and latitude
            if (m_Longitude != StrLongitude && m_Latitude != StrLatitude)
            {

                // Convert Degrees-Minutes-Seconds to Decimal Degrees, if needed
                double lon = 0.0;
                double lat = 0.0;
                if (m_GeoCon.ConvertCoordinateToDecimal(m_Longitude, ref lon))
                    m_Longitude = lon.ToString("F8", CultureInfo.InvariantCulture);
                if (m_GeoCon.ConvertCoordinateToDecimal(m_Latitude, ref lat))
                    m_Latitude = lat.ToString("F8", CultureInfo.InvariantCulture);
                // ----------------------

                if (ConvertPGKtoGeo(ref m_Longitude, ref m_Latitude))
                    parameters[IndexGeography] = string.Format(StrCenterPointMTB, m_Longitude, m_Latitude);           
            }

            // If assigned: Create geography from longitude and latitude or MTB/Q entries
            if (m_MtbGeography != string.Empty)
            {
                parameters[IndexGeography] = m_MtbGeography;
            }

            // Replace commas with points for thickness
            parameters[6] = parameters[6].Replace(',', '.');
            parameters[8] = parameters[8].Replace(',', '.');

            // Check for consistency of the parameters
            if (!CheckConsistency(parameters))
                return string.Empty;


            // Assemble Geo-Object string
            if (!m_SaveAsSingleSample)
            {
                // Return complete Geo-Object string
                for (int i = 0; i < parameters.Length; i++)
                {
                    resultStr += parameters[i];
                    // if (i < parameters.Length - 1)
                        resultStr += StrCR;
                }
            }
            else
            {
                // First row: Return complete Geo-Object string until first coordinate (to be assembled)
                if (m_Line == 0)
                {
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        resultStr += parameters[i];
                        if (i < parameters.Length - 1)
                            resultStr += StrCR;
                        else
                        {
                            string tempResult = resultStr.Substring(0, resultStr.Length - 1) + ", ";
                            resultStr = tempResult;
                        }
                    }
                }
                // Just return next longitude and latitude coordinate string (to be assembled)
                else
                {
                    resultStr = m_Longitude + " " + m_Latitude + ", ";
                }
            }

            m_Line++;

            return resultStr;
        }


        private bool ConvertPGKtoGeo(ref string longitude, ref string latitude)
        {
            try
            {
                double lat = 0;
                double lon = 0;

                Point point = new Point();
                point.X = Convert.ToDouble(longitude, CultureInfo.InvariantCulture);
                point.Y = Convert.ToDouble(latitude, CultureInfo.InvariantCulture);

                // Assume Gauss-Krüger Format, if not in geographic coordinates range
                if (Math.Abs(point.X) > 180 || Math.Abs(point.Y) > 90)
                {
                    // Convert GK coords to geographic
                    if (!m_GeoCon.CoordPotsdamGkToGeo(point.X, point.Y, ref lon, ref lat))
                        return false;
                    // Convert Potsdam datum to WGS84
                    if (!m_GeoCon.DatumPotsdamToWgs84(lon, lat, ref lon, ref lat))
                        return false;
                    // GKtoGeo(point.X, point.Y, ref lon, ref lat);
                    longitude = lon.ToString("F10", CultureInfo.InvariantCulture);
                    latitude = lat.ToString("F10", CultureInfo.InvariantCulture);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Check if assignment is valid.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>true or false</returns>
        private bool CheckConsistency(string[] parameters)
        {
            try
            {
                for ( int i = 0; i < parameters.Length; i++ )
                {
                    if (!CheckAssignedParameter(i, parameters[i]))
                        return false;
                }
                if (m_Longitude != StrLongitude)
                    if (!CheckAssignedParameter(10, m_Longitude))
                        return false;
                if (m_Latitude != StrLatitude)
                    if (!CheckAssignedParameter(11, m_Latitude))
                        return false;
            }
            catch
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// Convert MTB code into longitude and latitude values (WGS84).
        /// </summary>
        /// <param name="mtb">input parameter MTB/Q code.</param>
        /// <param name="mtbGeography">output string parameter for geography derived from MTB/Q code.</param>
        /// <returns>True if success, else false</returns>
        private bool ConvertMTBtoLonLat(string mtb, ref string mtbGeography)
        {
            // Parse MTB input string
            if (mtb.Length < 4)
                return false;

            try
            {
                double latStart = 56.0;
                double lonStart = 5.5 + (10.0 / 60.0);

                double mtbYdifLat = 6.0 / 60.0;
                double mtbXdifLon = 10.0 / 60.0;

                // Get MTB numbers
                int mtbY = Convert.ToInt32(mtb.Substring(0, 2));
                int mtbX = Convert.ToInt32(mtb.Substring(2, 2));
                int mtbQ = 0;
                int mtbQQ = 0;
                int mtbQQQ = 0;

                if (mtb.Length >= 5 && mtb[4] != '/')
                    return false;

                // Get quadrant, if any
                if (mtb.Length >= 6)
                {
                    if (mtb[5] < '1' || mtb[5] > '4')
                        return false;
                    else
                        mtbQ = Convert.ToInt32(mtb.Substring(5, 1));
                }

                // Get 1/4 quarter quadrant, if any
                if (mtb.Length >= 7)
                {
                    if (mtb[6] < '1' || mtb[6] > '4')
                        return false;
                    else
                        mtbQQ = Convert.ToInt32(mtb.Substring(6, 1));
                }

                // Get 1/16 quadrant, if any
                if (mtb.Length >= 8)
                {
                    if (mtb[7] < '1' || mtb[7] > '4')
                        return false;
                    else
                        mtbQQQ = Convert.ToInt32(mtb.Substring(7, 1));
                }

                // Calculate longitude and latitude of MTB upper left corner
                double mtbLat = latStart - (mtbY * mtbYdifLat);
                double mtbLon = lonStart + (mtbX * mtbXdifLon);

                // Calculate center of MTB
                double mtbCenterLon = mtbLon + (mtbXdifLon / 2);
                double mtbCenterLat = mtbLat - (mtbYdifLat / 2);

                // Preset offsets for Qs
                double offsetLon = -(5.0 / 60.0);
                double offsetLat = (3.0 / 60.0);

                // string color = "Black";

                if (mtbQ != 0)
                {
                    switch (mtbQ)
                    {
                        case 1:
                            offsetLon = -(2.5 / 60.0);
                            offsetLat = (1.5 / 60.0);
                            break;
                        case 2:
                            offsetLon = (2.5 / 60.0);
                            offsetLat = (1.5 / 60.0);
                            break;
                        case 3:
                            offsetLon = -(2.5 / 60.0);
                            offsetLat = -(1.5 / 60.0);
                            break;
                        case 4:
                            offsetLon = (2.5 / 60.0);
                            offsetLat = -(1.5 / 60.0);
                            break;
                    }
                    // Calculate center of quadrant
                    mtbCenterLon += offsetLon;
                    mtbCenterLat += offsetLat;
                    // color = "Red";
                }

                if (mtbQQ != 0)
                {
                    switch (mtbQQ)
                    {
                        case 1:
                            offsetLon = -(1.25 / 60.0);
                            offsetLat = (0.75 / 60.0);
                            break;
                        case 2:
                            offsetLon = (1.25 / 60.0);
                            offsetLat = (0.75 / 60.0);
                            break;
                        case 3:
                            offsetLon = -(1.25 / 60.0);
                            offsetLat = -(0.75 / 60.0);
                            break;
                        case 4:
                            offsetLon = (1.25 / 60.0);
                            offsetLat = -(0.75 / 60.0);
                            break;
                    }
                    // Calculate center of 1/4 quadrant
                    mtbCenterLon += offsetLon;
                    mtbCenterLat += offsetLat;
                    // color = "Blue";
                }

                if (mtbQQQ != 0)
                {
                    switch (mtbQQQ)
                    {
                        case 1:
                            offsetLon = -(0.625 / 60.0);
                            offsetLat = (0.375 / 60.0);
                            break;
                        case 2:
                            offsetLon = (0.625 / 60.0);
                            offsetLat = (0.375 / 60.0);
                            break;
                        case 3:
                            offsetLon = -(0.625 / 60.0);
                            offsetLat = -(0.375 / 60.0);
                            break;
                        case 4:
                            offsetLon = (0.625 / 60.0);
                            offsetLat = -(0.375 / 60.0);
                            break;
                    }
                    // Calculate center of 1/16 quadrant
                    mtbCenterLon += offsetLon;
                    mtbCenterLat += offsetLat;
                    // color = "Green";
                }

                // MTB/QQQ corners
                double mtbUpperLeftLon = mtbCenterLon - offsetLon;
                double mtbUpperLeftLat = mtbCenterLat - offsetLat;
                double mtbUpperRightLon = mtbCenterLon + offsetLon;
                double mtbUpperRightLat = mtbCenterLat - offsetLat;
                double mtbLowerRightLon = mtbCenterLon + offsetLon;
                double mtbLowerRightLat = mtbCenterLat + offsetLat;
                double mtbLowerLeftLon = mtbCenterLon - offsetLon;
                double mtbLowerLeftLat = mtbCenterLat + offsetLat;
                // Convert Potsdam datum to WGS84
                // Center point
                if (!m_WpfControl.m_GeoCon.DatumPotsdamToWgs84(mtbCenterLon, mtbCenterLat, ref mtbCenterLon, ref mtbCenterLat))
                    return false;
                // Convert MTB/QQQ corners
                if (!m_WpfControl.m_GeoCon.DatumPotsdamToWgs84(mtbUpperLeftLon, mtbUpperLeftLat, ref mtbUpperLeftLon, ref mtbUpperLeftLat))
                    return false;
                if (!m_WpfControl.m_GeoCon.DatumPotsdamToWgs84(mtbUpperRightLon, mtbUpperRightLat, ref mtbUpperRightLon, ref mtbUpperRightLat))
                    return false;
                if (!m_WpfControl.m_GeoCon.DatumPotsdamToWgs84(mtbLowerRightLon, mtbLowerRightLat, ref mtbLowerRightLon, ref mtbLowerRightLat))
                    return false;
                if (!m_WpfControl.m_GeoCon.DatumPotsdamToWgs84(mtbLowerLeftLon, mtbLowerLeftLat, ref mtbLowerLeftLon, ref mtbLowerLeftLat))
                    return false;

                // Set geography strings for MTB as rectangle and as center point
                m_MtbGeographyPolygon = string.Format(StrPolygonMTB,
                        mtbUpperLeftLon.ToString("F8", CultureInfo.InvariantCulture), mtbUpperLeftLat.ToString("F8", CultureInfo.InvariantCulture),
                        mtbUpperRightLon.ToString("F8", CultureInfo.InvariantCulture), mtbUpperRightLat.ToString("F8", CultureInfo.InvariantCulture),
                        mtbLowerRightLon.ToString("F8", CultureInfo.InvariantCulture), mtbLowerRightLat.ToString("F8", CultureInfo.InvariantCulture),
                        mtbLowerLeftLon.ToString("F8", CultureInfo.InvariantCulture), mtbLowerLeftLat.ToString("F8", CultureInfo.InvariantCulture),
                        mtbUpperLeftLon.ToString("F8", CultureInfo.InvariantCulture), mtbUpperLeftLat.ToString("F8", CultureInfo.InvariantCulture));
                m_MtbGeographyPoint = string.Format(StrCenterPointMTB,
                        mtbCenterLon.ToString("F8", CultureInfo.InvariantCulture), mtbCenterLat.ToString("F8", CultureInfo.InvariantCulture));


                // Set output data
                if (checkBoxCenterPoint.IsChecked.Value)
                {   
                    // longitude = mtbCenterLon.ToString("F8", CultureInfo.InvariantCulture);
                    // latitude = mtbCenterLat.ToString("F8", CultureInfo.InvariantCulture);
                    mtbGeography = m_MtbGeographyPoint;
                    //     string.Format(StrCenterPointMTB, 
                    //     mtbCenterLon.ToString("F8", CultureInfo.InvariantCulture), mtbCenterLat.ToString("F8", CultureInfo.InvariantCulture));
                    // string outLine = mtb + StrTAB + color + StrTAB + mtbCenterLon.ToString("F8", CultureInfo.InvariantCulture) + StrTAB + mtbCenterLat.ToString("F8", CultureInfo.InvariantCulture);
                }
                else
                {
                    mtbGeography = m_MtbGeographyPolygon;
                    //     string.Format(StrPolygonMTB,
                    //     mtbUpperLeftLon.ToString("F8", CultureInfo.InvariantCulture), mtbUpperLeftLat.ToString("F8", CultureInfo.InvariantCulture),
                    //     mtbUpperRightLon.ToString("F8", CultureInfo.InvariantCulture), mtbUpperRightLat.ToString("F8", CultureInfo.InvariantCulture),
                    //     mtbLowerRightLon.ToString("F8", CultureInfo.InvariantCulture), mtbLowerRightLat.ToString("F8", CultureInfo.InvariantCulture),
                    //     mtbLowerLeftLon.ToString("F8", CultureInfo.InvariantCulture), mtbLowerLeftLat.ToString("F8", CultureInfo.InvariantCulture),
                    //     mtbUpperLeftLon.ToString("F8", CultureInfo.InvariantCulture), mtbUpperLeftLat.ToString("F8", CultureInfo.InvariantCulture));
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion // methods

    }
}
