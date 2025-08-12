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
using System.Globalization;
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
    /// Interaktionslogik für Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        #region Constants

        // GPS baudrates for combo box
        private string[] GpsBaudrates = { "50", "110", "150", "300", "1200", "2400", "4800", "9600", "19200", "38400", "57600", "115200", "230400", "460800", "500000", "Demo mode" };

        #endregion // Constants

        #region Fields

        private WpfControl m_WpfControl = null;
        private SamplePoints m_Symbol = null;
        private PointSymbol m_PointSymbol = PointSymbol.Pin;
        private string m_GpsBaudrate = string.Empty;

        #endregion // Fields

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        /// <param name="wpfControl">The WPF control.</param>
        public Settings(WpfControl wpfControl)
        {
            // Get GIS Editor instance
            m_WpfControl = wpfControl;
            // Initialize
            InitializeComponent();

            // Set localization text
            labelShapeFileReadFormat.Content = WpfSamplingPlotPage.Properties.Resources.SettingsLabelShapeFileReadFormat;
            labelShapeFileFormats.Content = WpfSamplingPlotPage.Properties.Resources.SettingsLabelShapeFileFormats;
            radioButtonUTM.Content = WpfSamplingPlotPage.Properties.Resources.SettingsRadioButtonUTM;
            radioButtonShapeGeo.Content = WpfSamplingPlotPage.Properties.Resources.SettingsRadioButtonShapeGeo;
            labelUtmZone.Content = WpfSamplingPlotPage.Properties.Resources.SettingsLabelUtmZone;
            labelUtmHemi.Content = WpfSamplingPlotPage.Properties.Resources.SettingsLabelUtmHemi;
            checkBoxSplitShapes.Content = WpfSamplingPlotPage.Properties.Resources.SettingsCheckBoxSplitShapes;
            checkBoxShapeFileFormatArcView.Content = WpfSamplingPlotPage.Properties.Resources.SettingsCheckBoxShapeFileFormatArcView;
            checkBoxShapeFileFormatImport.Content = WpfSamplingPlotPage.Properties.Resources.SettingsCheckBoxShapeFileFormatImport;
            checkBoxShapeFileFormatSQL.Content = WpfSamplingPlotPage.Properties.Resources.SettingsCheckBoxShapeFileFormatSQL;
            labelImageCoordFileFormats.Content = WpfSamplingPlotPage.Properties.Resources.SettingsLabelImageCoordFileFormats;
            checkBoxImageFileFormatXml.Content = WpfSamplingPlotPage.Properties.Resources.SettingsCheckBoxImageFileFormatXml;
            checkBoxImageFileFormatGlopus.Content = WpfSamplingPlotPage.Properties.Resources.SettingsCheckBoxImageFileFormatGlopus;
            labelWorkingArea.Content = WpfSamplingPlotPage.Properties.Resources.SettingsLabelWorkingArea;
            checkBoxSaveWorkingArea.Content = WpfSamplingPlotPage.Properties.Resources.SettingsCheckBoxSaveWorkingArea;
            checkBoxAreaFrame.Content = WpfSamplingPlotPage.Properties.Resources.SettingsCheckBoxAreaFrame;
            labelFrame.Content = WpfSamplingPlotPage.Properties.Resources.SettingsLabelFrame;
            labelWidth.Content = WpfSamplingPlotPage.Properties.Resources.SettingsLabelWidth;
            labelHeight.Content = WpfSamplingPlotPage.Properties.Resources.SettingsLabelHeight;
            labelAltitude.Content = WpfSamplingPlotPage.Properties.Resources.SettingsLabelAltitude;
            checkBoxGetAltitude.Content = WpfSamplingPlotPage.Properties.Resources.SettingsCheckBoxGetAltitude;
            labelStrokeThickness.Content = WpfSamplingPlotPage.Properties.Resources.SettingsLabelStrokeThickness;
            labelPoints.Content = WpfSamplingPlotPage.Properties.Resources.SettingsLabelPoints;
            labelPointSymbol.Content = WpfSamplingPlotPage.Properties.Resources.SettingsLabelPointSymbol;
            labelSampleOffTransparency.Content = WpfSamplingPlotPage.Properties.Resources.SettingsLabelSampleOffTransparency;
            labelPointSymbolSize.Content = WpfSamplingPlotPage.Properties.Resources.SettingsLabelPointSymbolSize;
            labelGpsTrack.Content = WpfSamplingPlotPage.Properties.Resources.SettingsLabelGpsTrack;
            checkBoxGpsTrack.Content = WpfSamplingPlotPage.Properties.Resources.SettingsCheckBoxGpsTrack;
            labelGps.Content = WpfSamplingPlotPage.Properties.Resources.SettingsLabelGps;
            labelBaudrate.Content = WpfSamplingPlotPage.Properties.Resources.SettingsLabelBaudrate;
            labelViewer.Content = WpfSamplingPlotPage.Properties.Resources.SettingsLabelViewer;
            buttonCancel.Content = WpfSamplingPlotPage.Properties.Resources.ctrlButtonCancel;
            GISEditorSettings.Title = WpfSamplingPlotPage.Properties.Resources.SettingsTitle;

            // Preset checkboxes according to flags from GIS Editor object
            // Read ArcView Shape format
            radioButtonUTM.IsChecked = m_WpfControl.m_ReadArcViewShapeFormatUtm;
            radioButtonShapeGeo.IsChecked = !m_WpfControl.m_ReadArcViewShapeFormatUtm;
            textBoxUtmZone.Text = m_WpfControl.m_ReadArcViewShapeFormatUtmZone.ToString();
            textBoxUtmHemi.Text = m_WpfControl.m_ReadArcViewShapeFormatUtmHemi;
            textBoxUtmZone.IsEnabled = m_WpfControl.m_ReadArcViewShapeFormatUtm;
            textBoxUtmHemi.IsEnabled = m_WpfControl.m_ReadArcViewShapeFormatUtm;
            checkBoxSplitShapes.IsChecked = m_WpfControl.m_SplitShapes;
            // Write to SQL GeoObject file
            checkBoxShapeFileFormatSQL.IsChecked = m_WpfControl.m_WriteShapesToSqlFile;
            // Write to ArcView Shape files
            checkBoxShapeFileFormatArcView.IsChecked = m_WpfControl.m_WriteShapesToArcFile;
            // Write to ArcView Shape files
            checkBoxShapeFileFormatImport.IsChecked = m_WpfControl.m_WriteShapesToImportFile;
            // Write image coordinates to Glopus calibration files
            checkBoxImageFileFormatGlopus.IsChecked = m_WpfControl.m_WriteImageToKalFile;
            // Save working area
            checkBoxSaveWorkingArea.IsChecked = m_WpfControl.m_SaveWorkingArea;
            // Save working area frame
            checkBoxAreaFrame.IsChecked = m_WpfControl.m_AreaFrame = WpfSamplingPlotPage.Properties.Settings.Default.AreaFrame;
            /*
            if (!m_WpfControl.m_SaveWorkingArea)
            {
                // Disable the Frame controls
                checkBoxAreaFrame.IsEnabled = false;
                textBoxWidth.IsEnabled = false;
                textBoxHeight.IsEnabled = false;
            }
            */
            // Get altitude for coordinate points from Geonames server
            // --- CURRENTLY ENABLED ---
            checkBoxGetAltitude.IsChecked = m_WpfControl.m_GetAltitude;
            // Track GPS data as a line string if active
            checkBoxGpsTrack.IsChecked = m_WpfControl.m_TrackGpsData;
            // Init sliders
            // Set zoom slider region (factor 0.1 to 10)
            sliderStrokeThicknessPoint.Minimum = sliderStrokeThicknessLine.Minimum = sliderStrokeThicknessArea.Minimum = sliderPointSymbolSize.Minimum = 0.5;
            sliderStrokeThicknessPoint.Maximum = sliderStrokeThicknessLine.Maximum = sliderStrokeThicknessArea.Maximum = sliderPointSymbolSize.Maximum = 10.0;
            sliderStrokeThicknessPoint.Value = m_WpfControl.m_PointsStrokeThickness;
            sliderStrokeThicknessLine.Value = m_WpfControl.m_LineStrokeThickness;
            sliderStrokeThicknessArea.Value = m_WpfControl.m_PolygonStrokeThickness;
            sliderPointSymbolSize.Value = m_WpfControl.m_PointSymbolSize;
            // Set opacity slider region (opacity 0 to 100 %)
            sliderSampleOffTransparencySample.Minimum = 0.0;
            sliderSampleOffTransparencySample.Maximum = 255.0;
            sliderSampleOffTransparencySample.Value = m_WpfControl.m_SampleOffTransparency;
            labelSampleOffTransparencySampleValue.Content = (sliderSampleOffTransparencySample.Value * 100 / 255).ToString("F0", CultureInfo.InvariantCulture);

            // Set frame sizes
            textBoxWidth.Text = m_WpfControl.m_AreaFrameWidth.ToString("F0");
            textBoxHeight.Text = m_WpfControl.m_AreaFrameHeight.ToString("F0");
            // MapMode-Viewer
            radioButtonGoogle.IsChecked = (m_WpfControl.m_MapModeViewer == 0 ? true : false);
            radioButtonOSM.IsChecked = (m_WpfControl.m_MapModeViewer == 0 ? false : true);

            // Set tool tips
            SetToolTips();

            // Set current PointSymbol
            m_Symbol = new SamplePoints(canvasPointSymbols);
            m_Symbol.SamplePointSymbol = m_PointSymbol = wpfControl.m_PointSymbol;
            m_Symbol.SamplePointSymbolSize = wpfControl.m_PointSymbolSize;
            m_Symbol.SetSamplePoint(new Point(0, 0));

            // Fill combobox PointSymbols
            for (PointSymbol pointSymbol = PointSymbol.Pin; pointSymbol < PointSymbol.None; pointSymbol++)
            {
                // Create combobox item
                ComboBoxItem item = new ComboBoxItem();
                // Create item content
                item.Content = pointSymbol.ToString();
                item.DataContext = pointSymbol;
                // Add item to combobox
                comboBoxPointSymbols.Items.Add(item);
                // If item is current symbol, set selection to it
                if (wpfControl.m_PointSymbol == pointSymbol)
                {
                    comboBoxPointSymbols.SelectedItem = item;
                }
            }

            // Fill combobox Baudrates
            m_GpsBaudrate = wpfControl.m_GpsBaudrate;
            foreach (string baudrate in GpsBaudrates)
            {
                // Create combobox item
                ComboBoxItem item = new ComboBoxItem();
                // Create item content
                item.Content = baudrate;
                item.DataContext = baudrate;
                // Add item to combobox
                comboBoxGpsBaudrates.Items.Add(item);
                // If item is current symbol, set selection to it
                if (wpfControl.m_GpsBaudrate == baudrate)
                {
                    comboBoxGpsBaudrates.SelectedItem = item;
                }
            }

        }

        /// <summary>
        /// Sets the tool tips of all labels and controls of the settings.
        /// </summary>
        private void SetToolTips()
        {
            sliderStrokeThicknessPoint.ToolTip          = m_WpfControl.SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttSettingsSliderStrokeThicknessPoint);
            sliderStrokeThicknessLine.ToolTip           = m_WpfControl.SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttSettingsSliderStrokeThicknessLine);
            sliderStrokeThicknessArea.ToolTip           = m_WpfControl.SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttSettingsSliderStrokeThicknessArea);
            sliderPointSymbolSize.ToolTip               = m_WpfControl.SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttSettingsSliderPointSymbolSize);
            sliderSampleOffTransparencySample.ToolTip   = m_WpfControl.SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttSettingsSliderSampleOffTransparencySample);
            checkBoxShapeFileFormatSQL.ToolTip          = m_WpfControl.SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttCheckBoxShapeFileFormatSQL);
            checkBoxShapeFileFormatArcView.ToolTip      = m_WpfControl.SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttCheckBoxShapeFileFormatArcView);
            checkBoxShapeFileFormatImport.ToolTip       = m_WpfControl.SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttCheckBoxShapeFileFormatImport);
            checkBoxImageFileFormatGlopus.ToolTip       = m_WpfControl.SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttCheckBoxImageFileFormatGlopus);
            checkBoxGetAltitude.ToolTip                 = m_WpfControl.SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttCheckBoxGetAltitude);
            checkBoxImageFileFormatXml.ToolTip          = m_WpfControl.SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttCheckBoxImageFileFormatXml);
            checkBoxSaveWorkingArea.ToolTip             = m_WpfControl.SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttCheckBoxSaveWorkingArea);
            checkBoxAreaFrame.ToolTip                   = m_WpfControl.SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttCheckBoxAreaFrame);
            labelStrokeThicknessPointValue.ToolTip      = m_WpfControl.SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttLabelStrokeThicknessPointValue);
            labelStrokeThicknessLineValue.ToolTip       = m_WpfControl.SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttLabelStrokeThicknessLineValue);
            labelStrokeThicknessAreaValue.ToolTip       = m_WpfControl.SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttLabelStrokeThicknessAreaValue);
            canvasPointSymbols.ToolTip                  = m_WpfControl.SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttCanvasPointSymbols);
            comboBoxPointSymbols.ToolTip                = m_WpfControl.SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttComboBoxPointSymbols);
            comboBoxGpsBaudrates.ToolTip                = m_WpfControl.SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttComboBoxGpsBaudrates);
            labelPointSymbolSizeValue.ToolTip           = m_WpfControl.SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttLabelPointSymbolSizeValue);
            labelSampleOffTransparencySampleValue.ToolTip = m_WpfControl.SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttLabelSampleOffTransparencySampleValue);
            textBoxWidth.ToolTip                        = m_WpfControl.SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttTextBoxWidth);
            textBoxHeight.ToolTip                       = m_WpfControl.SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttTextBoxHeight);
            radioButtonGoogle.ToolTip                   = m_WpfControl.SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttSettingsRadioButtonGoogle);
            radioButtonOSM.ToolTip                      = m_WpfControl.SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttSettingsRadioButtonOsm);
        }

        #endregion // Construction

        #region Methods

        #region Event handlers

        /// <summary>
        /// Handles the Checked event of the radioButtonUTM control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void radioButtonUTM_Checked(object sender, RoutedEventArgs e)
        {
            m_WpfControl.m_ReadArcViewShapeFormatUtm = true;
            textBoxUtmZone.IsEnabled = m_WpfControl.m_ReadArcViewShapeFormatUtm;
            textBoxUtmHemi.IsEnabled = m_WpfControl.m_ReadArcViewShapeFormatUtm;
        }

        /// <summary>
        /// Handles the Unchecked event of the radioButtonUTM control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void radioButtonUTM_Unchecked(object sender, RoutedEventArgs e)
        {
            m_WpfControl.m_ReadArcViewShapeFormatUtm = false;
            textBoxUtmZone.IsEnabled = m_WpfControl.m_ReadArcViewShapeFormatUtm;
            textBoxUtmHemi.IsEnabled = m_WpfControl.m_ReadArcViewShapeFormatUtm;
        }

        /// <summary>
        /// Handles the TextChanged event of the textBoxUtmZone control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs"/> instance containing the event data.</param>
        private void textBoxUtmZone_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int zone = Convert.ToInt32(textBoxUtmZone.Text);
                if (zone >= 1 && zone <= 60)
                {
                    m_WpfControl.m_ReadArcViewShapeFormatUtmZone = zone;
                    textBoxUtmZone.Foreground = Brushes.Black;
                }
                else
                {
                    textBoxUtmZone.Foreground = Brushes.Red;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the textBoxUtmHemi control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs"/> instance containing the event data.</param>
        private void textBoxUtmHemi_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string hemi = textBoxUtmHemi.Text;
                if (hemi == "N" || hemi == "S")
                {
                    m_WpfControl.m_ReadArcViewShapeFormatUtmHemi = hemi;
                    textBoxUtmHemi.Foreground = Brushes.Black;
                }
                else
                {
                    textBoxUtmHemi.Foreground = Brushes.Red;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Handles the Checked event of the checkBoxShapeFileFormatSQL control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void checkBoxShapeFileFormatSQL_Checked(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Handles the Checked event of the checkBoxShapeFileFormatArcView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void checkBoxShapeFileFormatArcView_Checked(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Handles the Checked event of the checkBoxShapeFileFormatImport control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void checkBoxShapeFileFormatImport_Checked(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Handles the Checked event of the checkBoxImageFileFormatXml control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void checkBoxImageFileFormatXml_Checked(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Handles the Checked event of the checkBoxImageFileFormatGlopus control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void checkBoxImageFileFormatGlopus_Checked(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Handles the Checked event of the checkBoxSaveWorkingArea control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void checkBoxSaveWorkingArea_Checked(object sender, RoutedEventArgs e)
        {
            // Enable the Frame conrols
            // checkBoxAreaFrame.IsEnabled = true;
            // textBoxWidth.IsEnabled = true;
            // textBoxHeight.IsEnabled = true;
        }

        /// <summary>
        /// Handles the Unchecked event of the checkBoxSaveWorkingArea control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void checkBoxSaveWorkingArea_Unchecked(object sender, RoutedEventArgs e)
        {
            // Disable the Frame conrols
            // checkBoxAreaFrame.IsEnabled = false;
            // textBoxWidth.IsEnabled = false;
            // textBoxHeight.IsEnabled = false;
        }

        /// <summary>
        /// Handles the Checked event of the checkBoxGetAltitude control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void checkBoxGetAltitude_Checked(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Handles the ValueChanged event of the sliderStrokeThicknessPoint control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The RoutedPropertyChangedEventArgs instance containing the event data.</param>
        private void sliderStrokeThicknessPoint_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            labelStrokeThicknessPointValue.Content = sliderStrokeThicknessPoint.Value.ToString("F1", CultureInfo.InvariantCulture);
            RedrawPointSymbol();
        }

        /// <summary>
        /// Handles the MouseDoubleClick event of the sliderStrokeThicknessPoint control.
        /// Slider is set back to default value.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void sliderStrokeThicknessPoint_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            sliderStrokeThicknessPoint.Value = WpfControl.DefaultPointsStrokeThickness;
            RedrawPointSymbol();
        }

        /// <summary>
        /// Handles the ValueChanged event of the sliderStrokeThicknessLine control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The RoutedPropertyChangedEventArgs instance containing the event data.</param>
        private void sliderStrokeThicknessLine_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            labelStrokeThicknessLineValue.Content = sliderStrokeThicknessLine.Value.ToString("F1", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Handles the MouseDoubleClick event of the sliderStrokeThicknessLine control.
        /// Slider is set back to default value.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void sliderStrokeThicknessLine_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            sliderStrokeThicknessLine.Value = WpfControl.DefaultLineStrokeThickness;
        }

        /// <summary>
        /// Handles the ValueChanged event of the sliderStrokeThicknessArea control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The RoutedPropertyChangedEventArgs instance containing the event data.</param>
        private void sliderStrokeThicknessArea_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            labelStrokeThicknessAreaValue.Content = sliderStrokeThicknessArea.Value.ToString("F1", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Handles the MouseDoubleClick event of the sliderStrokeThicknessArea control.
        /// Slider is set back to default value.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void sliderStrokeThicknessArea_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            sliderStrokeThicknessArea.Value = WpfControl.DefaultPolygonStrokeThickness;
        }


        /// <summary>
        /// Handles the SelectionChanged event of the comboBoxPointSymbols control.
        /// Current PointSymbol is set according to the selection.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void comboBoxPointSymbols_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = comboBoxPointSymbols.SelectedItem as ComboBoxItem;
            if (item != null && item.DataContext != null) 
                m_PointSymbol = (PointSymbol)item.DataContext;
            RedrawPointSymbol();
        }
        
        /// <summary>
        /// Handles the ValueChanged event of the sliderPointSymbolSize control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The RoutedPropertyChangedEventArgs instance containing the event data.</param>
        private void sliderPointSymbolSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            labelPointSymbolSizeValue.Content = sliderPointSymbolSize.Value.ToString("F1", CultureInfo.InvariantCulture);
            RedrawPointSymbol();
        }
        
        /// <summary>
        /// Handles the MouseDoubleClick event of the sliderPointSymbolSize control.
        /// Slider is set back to default value.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void sliderPointSymbolSize_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            sliderPointSymbolSize.Value = WpfControl.DefaultPointSymbolSize;
        }

        /// <summary>
        /// Handles the ValueChanged event of the sliderSampleOffTransparencySample control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The RoutedPropertyChangedEventArgs instance containing the event data.</param>
        private void sliderSampleOffTransparencySample_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            labelSampleOffTransparencySampleValue.Content = (sliderSampleOffTransparencySample.Value * 100 / 255).ToString("F0", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Handles the MouseDoubleClick event of the sliderSampleOffTransparencySample control.
        /// Slider is set back to default value.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void sliderSampleOffTransparencySample_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            sliderSampleOffTransparencySample.Value = m_WpfControl.m_SampleOffTransparency;
        }


        /// <summary>
        /// Handles the TextChanged event of the textBoxWidth and textBoxHeight control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs"/> instance containing the event data.</param>
        private void textBoxWidth_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Check if value is in the range!!!
        }

        /// <summary>
        /// Handles the SelectionChanged event of the comboBoxGpsBaudrates control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void comboBoxGpsBaudrates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = comboBoxGpsBaudrates.SelectedItem as ComboBoxItem;
            if (item != null && item.DataContext != null)
                m_GpsBaudrate = (string)item.DataContext;
        }

        /// <summary>
        /// Handles the Click event of the buttonOK control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Set GIS Editor flags according to checkboxes of the Settings menu
                m_WpfControl.m_ReadArcViewShapeFormatUtm = WpfSamplingPlotPage.Properties.Settings.Default.ReadArcViewShapeFormatUtm = radioButtonUTM.IsChecked.Value;
                m_WpfControl.m_ReadArcViewShapeFormatUtmZone = WpfSamplingPlotPage.Properties.Settings.Default.UtmZone = Convert.ToInt32(textBoxUtmZone.Text);
                m_WpfControl.m_ReadArcViewShapeFormatUtmHemi = WpfSamplingPlotPage.Properties.Settings.Default.UtmHemi = textBoxUtmHemi.Text;
                m_WpfControl.m_SplitShapes = WpfSamplingPlotPage.Properties.Settings.Default.SplitShapes = checkBoxSplitShapes.IsChecked.Value;
                m_WpfControl.m_WriteShapesToSqlFile = WpfSamplingPlotPage.Properties.Settings.Default.WriteShapesToSqlFile = checkBoxShapeFileFormatSQL.IsChecked.Value;
                m_WpfControl.m_WriteShapesToArcFile = WpfSamplingPlotPage.Properties.Settings.Default.WriteShapesToArcFile = checkBoxShapeFileFormatArcView.IsChecked.Value;
                m_WpfControl.m_WriteShapesToImportFile = WpfSamplingPlotPage.Properties.Settings.Default.WriteShapesToImportFile = checkBoxShapeFileFormatImport.IsChecked.Value;
                m_WpfControl.m_WriteImageToKalFile = WpfSamplingPlotPage.Properties.Settings.Default.WriteImageToKalFile = checkBoxImageFileFormatGlopus.IsChecked.Value;
                m_WpfControl.m_SaveWorkingArea = WpfSamplingPlotPage.Properties.Settings.Default.SaveWorkingArea = checkBoxSaveWorkingArea.IsChecked.Value;
                m_WpfControl.m_TrackGpsData = WpfSamplingPlotPage.Properties.Settings.Default.GpsTrack = checkBoxGpsTrack.IsChecked.Value;
                m_WpfControl.m_GetAltitude = WpfSamplingPlotPage.Properties.Settings.Default.GetAltitude = checkBoxGetAltitude.IsChecked.Value;
                m_WpfControl.m_PolygonStrokeThickness = WpfSamplingPlotPage.Properties.Settings.Default.PolygonStrokeThickness = Convert.ToDouble(sliderStrokeThicknessArea.Value.ToString("F1"));
                m_WpfControl.m_LineStrokeThickness = WpfSamplingPlotPage.Properties.Settings.Default.LineStrokeThickness = Convert.ToDouble(sliderStrokeThicknessLine.Value.ToString("F1"));
                m_WpfControl.m_PointsStrokeThickness = WpfSamplingPlotPage.Properties.Settings.Default.PointsStrokeThickness = Convert.ToDouble(sliderStrokeThicknessPoint.Value.ToString("F1"));
                m_WpfControl.SetStrokeThickness();
                m_WpfControl.m_PointSymbol = m_PointSymbol;
                WpfSamplingPlotPage.Properties.Settings.Default.PointSymbol = (ushort)m_PointSymbol;
                m_WpfControl.m_PointSymbolSize = WpfSamplingPlotPage.Properties.Settings.Default.PointSymbolSize = Convert.ToDouble(sliderPointSymbolSize.Value.ToString("F1"));
                m_WpfControl.SetPointSymbol();
                m_WpfControl.m_SampleOffTransparency = WpfSamplingPlotPage.Properties.Settings.Default.SampleOffTransparency = Convert.ToByte(sliderSampleOffTransparencySample.Value);
                m_WpfControl.m_GpsBaudrate = WpfSamplingPlotPage.Properties.Settings.Default.Baudrate = m_GpsBaudrate;
                // Set capture frame
                m_WpfControl.m_AreaFrame = WpfSamplingPlotPage.Properties.Settings.Default.AreaFrame = checkBoxAreaFrame.IsChecked.Value;
                /*
                if (!m_WpfControl.m_SaveWorkingArea)
                    m_WpfControl.m_AreaFrame = false;
                */
                m_WpfControl.m_AreaFrameWidth = Math.Max(WpfSamplingPlotPage.Properties.Settings.Default.AreaFrameWidth = Convert.ToInt32(textBoxWidth.Text), CaptureFrame.MinFrameWidth);
                m_WpfControl.m_AreaFrameHeight = Math.Max(WpfSamplingPlotPage.Properties.Settings.Default.AreaFrameHeight = Convert.ToInt32(textBoxHeight.Text), CaptureFrame.MinFrameHeight);
                m_WpfControl.SetCaptureFrame();
                m_WpfControl.m_MapModeViewer = WpfSamplingPlotPage.Properties.Settings.Default.MapModeViewer = (radioButtonGoogle.IsChecked.Value ? 0 : 1);
                // Save settings
                WpfSamplingPlotPage.Properties.Settings.Default.Save();
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

        /// <summary>
        /// Handles the Click event of the buttonCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            // Discard settings and close window
            this.Close();
        }

        /// <summary>
        /// Handles the Click event of the buttonInfo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonInfo_Click(object sender, RoutedEventArgs e)
        {
            // Open Info dialog
            Info info = new Info();
            info.ShowDialog();
        }

        #endregion // Event handlers

        #region Helpers

        /// <summary>
        /// Redraws the point symbol for online adjust of the size and type.
        /// </summary>
        private void RedrawPointSymbol()
        {
            if (m_Symbol != null)
            {
                m_Symbol.ClearSamplePoint();
                m_Symbol.SamplePointSymbol = m_PointSymbol;
                m_Symbol.SamplePointSymbolSize = sliderPointSymbolSize.Value;
                m_Symbol.StrokeThickness = sliderStrokeThicknessPoint.Value;
                m_Symbol.SetSamplePoint(new Point(0, 0));
            }
        }

        #endregion // Helpers

        #endregion // Methods

    }
}
