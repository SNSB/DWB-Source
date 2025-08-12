using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaktionslogik für SampleDetectionParameters.xaml
    /// </summary>
    public partial class SampleDetectionParameters : Window
    {

        #region Public enumerations

        /// <summary>
        /// Enumeration of the Editor Modes.
        /// </summary>
        public enum AdjustParam
        {
            /// <summary>
            /// Red color minimum value
            /// </summary>
            RedMin,
            /// <summary>
            /// Green color minimum value
            /// </summary>
            GreenMin,
            /// <summary>
            /// Blue color minimum value
            /// </summary>
            BlueMin,
            /// <summary>
            /// Grey color minimum value
            /// </summary>
            GreyMin,
            /// <summary>
            /// Red color maximum value
            /// </summary>
            RedMax,
            /// <summary>
            /// Green color maximum value
            /// </summary>
            GreenMax,
            /// <summary>
            /// Blue color maximum value
            /// </summary>
            BlueMax,
            /// <summary>
            /// Grey color maximum value
            /// </summary>
            GreyMax
        };

        #endregion // Public enumerations

        #region Fields

        private Color m_ColorMin = new Color();
        private Color m_ColorMax = new Color();
        private WpfControl m_WpfControl = null;
        private int m_PointDistMin = 0;
        private string m_ValidNumber = string.Empty;

        #endregion // Fields

        #region Construction

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="wpfControl">Calling instance object.</param>
        public SampleDetectionParameters(WpfControl wpfControl)
        {
            // Get GIS Editor instance
            m_WpfControl = wpfControl;

            InitializeComponent();

            // Localize the detection window
            labelAdjust.Content = WpfSamplingPlotPage.Properties.Resources.DetectionLabelAdjust;
            labelRed.Content = WpfSamplingPlotPage.Properties.Resources.DetectionLabelRed;
            labelGreen.Content = WpfSamplingPlotPage.Properties.Resources.DetectionLabelGreen;
            labelBlue.Content = WpfSamplingPlotPage.Properties.Resources.DetectionLabelBlue;
            labelGrey.Content = WpfSamplingPlotPage.Properties.Resources.DetectionLabelGrey;
            labelMinDistance.Content = WpfSamplingPlotPage.Properties.Resources.DetectionLabelMinDistance;
            checkBoxSeparateSamples.Content = WpfSamplingPlotPage.Properties.Resources.DetectionCheckBoxSeparateSamples;
            labelID.Content = WpfSamplingPlotPage.Properties.Resources.DetectionLabelId;
            labelNumber.Content = WpfSamplingPlotPage.Properties.Resources.DetectionLabelNumber;
            buttonCancel.Content = WpfSamplingPlotPage.Properties.Resources.ctrlButtonCancel;

            // Set Parameters
            sliderGreyMax.Value = 255;
            sliderGreyMax.Value = m_WpfControl.m_LimitGreyMax;
            sliderGreyMin.Value = m_WpfControl.m_LimitGreyMin;
            sliderRedMin.Value = m_WpfControl.m_LimitRedMin;
            sliderGreenMin.Value = m_WpfControl.m_LimitGreenMin;
            sliderBlueMin.Value = m_WpfControl.m_LimitBlueMin;
            sliderRedMax.Value = m_WpfControl.m_LimitRedMax;
            sliderGreenMax.Value = m_WpfControl.m_LimitGreenMax;
            sliderBlueMax.Value = m_WpfControl.m_LimitBlueMax;
            m_PointDistMin = m_WpfControl.m_PointDistanceMin;
            checkBoxSeparateSamples.IsChecked = m_WpfControl.m_SeparateSamples;
            textBoxMinDistance.Text = m_PointDistMin.ToString();
            textBoxID.Text = m_WpfControl.m_IdText;
            textBoxNumber.Text = m_ValidNumber = m_WpfControl.m_IdNumber;

        }

        #endregion // Construction

        #region Methods

        private void AdjustParams(AdjustParam colorParam)
        {
            switch (colorParam)
            {
                case AdjustParam.RedMin:
                    labelRedMinValue.Content = sliderRedMin.Value.ToString("F0");
                    if (sliderRedMin.Value > sliderRedMax.Value)
                        sliderRedMax.Value = sliderRedMin.Value;
                    m_ColorMin.R = Convert.ToByte(sliderRedMin.Value);
                    break;

                case AdjustParam.GreenMin:
                    labelGreenMinValue.Content = sliderGreenMin.Value.ToString("F0");
                    if (sliderGreenMin.Value > sliderGreenMax.Value)
                        sliderGreenMax.Value = sliderGreenMin.Value;
                    m_ColorMin.G = Convert.ToByte(sliderGreenMin.Value);
                    break;

                case AdjustParam.BlueMin:
                    labelBlueMinValue.Content = sliderBlueMin.Value.ToString("F0");
                    if (sliderBlueMin.Value > sliderBlueMax.Value)
                        sliderBlueMax.Value = sliderBlueMin.Value;
                    m_ColorMin.B = Convert.ToByte(sliderBlueMin.Value);
                    break;

                case AdjustParam.GreyMin:
                    sliderRedMin.Value = sliderGreyMin.Value;
                    sliderGreenMin.Value = sliderGreyMin.Value;
                    sliderBlueMin.Value = sliderGreyMin.Value;
                    if (sliderGreyMin.Value > sliderGreyMax.Value)
                        sliderGreyMax.Value = sliderGreyMin.Value;
                    break;

                case AdjustParam.RedMax:
                    labelRedMaxValue.Content = sliderRedMax.Value.ToString("F0");
                    if (sliderRedMax.Value < sliderRedMin.Value)
                        sliderRedMin.Value = sliderRedMax.Value;
                    m_ColorMax.R = Convert.ToByte(sliderRedMax.Value);
                    break;

                case AdjustParam.GreenMax:
                    labelGreenMaxValue.Content = sliderGreenMax.Value.ToString("F0");
                    if (sliderGreenMax.Value < sliderGreenMin.Value)
                        sliderGreenMin.Value = sliderGreenMax.Value;
                    m_ColorMax.G = Convert.ToByte(sliderGreenMax.Value);
                    break;

                case AdjustParam.BlueMax:
                    labelBlueMaxValue.Content = sliderBlueMax.Value.ToString("F0");
                    if (sliderBlueMax.Value < sliderBlueMin.Value)
                        sliderBlueMin.Value = sliderBlueMax.Value;
                    m_ColorMax.B = Convert.ToByte(sliderBlueMax.Value);
                    break;

                case AdjustParam.GreyMax:
                    sliderRedMax.Value = sliderGreyMax.Value;
                    sliderGreenMax.Value = sliderGreyMax.Value;
                    sliderBlueMax.Value = sliderGreyMax.Value;
                    if (sliderGreyMax.Value < sliderGreyMin.Value)
                        sliderGreyMin.Value = sliderGreyMax.Value;
                    break;

                default:
                    break;
            }

            // Adjust Color boxes
            m_ColorMin.A = 255;
            m_ColorMax.A = 255;
            labelColorMin.Background = new SolidColorBrush(m_ColorMin);
            labelColorMax.Background = new SolidColorBrush(m_ColorMax);
            labelColorSlide.Background = new LinearGradientBrush(m_ColorMin, m_ColorMax, 0);
           
        }

        #endregion // Methods

        #region Event handlers

        private void sliderRedMin_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AdjustParams(AdjustParam.RedMin);
        }

        private void sliderGreenMin_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AdjustParams(AdjustParam.GreenMin);
        }

        private void sliderBlueMin_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AdjustParams(AdjustParam.BlueMin);
        }

        private void sliderGreyMin_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AdjustParams(AdjustParam.GreyMin);
        }

        private void sliderRedMax_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AdjustParams(AdjustParam.RedMax);
        }

        private void sliderGreenMax_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AdjustParams(AdjustParam.GreenMax);
        }

        private void sliderBlueMax_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AdjustParams(AdjustParam.BlueMax);
        }

        private void sliderGreyMax_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AdjustParams(AdjustParam.GreyMax);
        }

        private void textBoxNumeric_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBoxNumeric = sender as TextBox;
            try
            {
                int test = Convert.ToInt32(textBoxNumeric.Text);
                textBoxNumeric.Background = Brushes.White;
                if (textBoxNumeric == textBoxMinDistance)
                    m_PointDistMin = Convert.ToInt32(textBoxNumeric.Text);
                if (textBoxNumeric == textBoxNumber)
                    m_ValidNumber = textBoxNumeric.Text;
            }
            catch
            {
                textBoxNumeric.Background = Brushes.Pink;
            }
        }


        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Set GIS Editor flags according to values of the Detection menu
                m_WpfControl.m_LimitRedMin = WpfSamplingPlotPage.Properties.Settings.Default.LimitRedMin = m_ColorMin.R;
                m_WpfControl.m_LimitGreenMin = WpfSamplingPlotPage.Properties.Settings.Default.LimitGreenMin = m_ColorMin.G;
                m_WpfControl.m_LimitBlueMin = WpfSamplingPlotPage.Properties.Settings.Default.LimitBlueMin = m_ColorMin.B;
                m_WpfControl.m_LimitGreyMin = WpfSamplingPlotPage.Properties.Settings.Default.LimitGreyMin = sliderGreyMin.Value;
                m_WpfControl.m_LimitRedMax = WpfSamplingPlotPage.Properties.Settings.Default.LimitRedMax = m_ColorMax.R;
                m_WpfControl.m_LimitGreenMax = WpfSamplingPlotPage.Properties.Settings.Default.LimitGreenMax = m_ColorMax.G;
                m_WpfControl.m_LimitBlueMax = WpfSamplingPlotPage.Properties.Settings.Default.LimitBlueMax = m_ColorMax.B;
                m_WpfControl.m_LimitGreyMax = WpfSamplingPlotPage.Properties.Settings.Default.LimitGreyMax = sliderGreyMax.Value;
                m_WpfControl.m_PointDistanceMin = WpfSamplingPlotPage.Properties.Settings.Default.PointDistanceMin = m_PointDistMin;
                m_WpfControl.m_SeparateSamples = WpfSamplingPlotPage.Properties.Settings.Default.SeparateSamples = checkBoxSeparateSamples.IsChecked.Value;
                m_WpfControl.m_IdText = WpfSamplingPlotPage.Properties.Settings.Default.IdText = textBoxID.Text;
                if (textBoxNumber.Background == Brushes.Pink)
                    textBoxNumber.Text = m_ValidNumber;
                m_WpfControl.m_IdNumber = WpfSamplingPlotPage.Properties.Settings.Default.IdNumber = textBoxNumber.Text;
                // Save settings
                WpfSamplingPlotPage.Properties.Settings.Default.Save();
                DialogResult = true;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
            finally
            {
                // Close the window
                this.Close();
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            // Close the window
            this.Close();
        }

        #endregion // Event handlers

    }
}
