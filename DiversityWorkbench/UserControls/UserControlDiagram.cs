using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace DiversityWorkbench.UserControls
{
    public partial class UserControlDiagram : UserControl
    {
        #region Events
        public event EventHandler Style3DChanged;

        protected virtual void OnStyle3DChanged(EventArgs e)
        {
            EventHandler handler = Style3DChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        #endregion

        #region Parameter
        // Constants for chart variation
        private const int _RotVal = 5;
        private const int _IncVal = 5;
        private const int _PerVal = 5;
        private const double _SclIni = 1.3;
        private const double _SclVal = .1;

        // Max y-value for scaling
        private double _SclMax;

        private string _DiagramPath = "Diagram";
        public string DiagramPath
        {
            get { return _DiagramPath; }
            set { _DiagramPath = value; }
        }

        private string _DefaultFileName = "Diagram";
        public string DefaultFileName
        {
            get { return _DefaultFileName; }
            set { _DefaultFileName = value; }
        }

        public bool Style3D
        {
            get
            {
                //if (this.chartDiagram.ChartAreas.Count > 0)
                //    return this.chartDiagram.ChartAreas[0].Area3DStyle.Enable3D;
                //else
                    return true;
            }
            set
            {
                //if (this.chartDiagram.ChartAreas.Count > 0 && this.chartDiagram.ChartAreas[0].Area3DStyle.Enable3D != value)
                //{
                //    this.chartDiagram.ChartAreas[0].Area3DStyle.Enable3D = value;
                //    this.toolStripSeparatorPosition.Visible = value;
                //    this.toolStripButtonLeft.Visible = value;
                //    this.toolStripButtonRight.Visible = value;
                //    this.toolStripButtonUp.Visible = value;
                //    this.toolStripButtonDown.Visible = value;
                //    this.toolStripButtonRect.Visible = value;
                //    this.toolStripButtonUpper.Visible = value;
                //    this.toolStripSeparatorPerspective.Visible = value;
                //    this.toolStripButtonPerspectiveDec.Visible = value;
                //    this.toolStripButtonPerspectiveInc.Visible = value;
                //    this.toolStripButton3D.BackColor = value ? SystemColors.ControlLightLight : SystemColors.Control;
                //    OnStyle3DChanged(new EventArgs());
                //}
            }
        }

        public bool DiagramHidden
        {
            get
            {
                //if (this.chartDiagram.ChartAreas.Count > 0)
                //    return !this.chartDiagram.ChartAreas[0].Visible;
                //else
                    return true;
            }
            set
            {
                //if (this.chartDiagram.ChartAreas.Count > 0)
                //    this.chartDiagram.ChartAreas[0].Visible = !value;
            }
        }

        public bool ShowStyleButton
        {
            get { return this.toolStripButton3D.Visible; }
            set
            {
                this.toolStripButton3D.Visible = value;
                this.toolStripSeparator3D.Visible = value;
            }
        }

        public bool ShowDataRowButton
        {
            get { return this.toolStripButtonSaveData.Visible; }
            set { this.toolStripButtonSaveData.Visible = value; }
        }

        private string DataDirectory
        {
            get { return DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule(); }
        }

        // Cultures for export selection
        private System.Globalization.CultureInfo _SelectedCulture;
        private Dictionary<string, System.Globalization.CultureInfo> _CultureList;
        private Dictionary<string, System.Globalization.CultureInfo> CultureList
        {
            get
            {
                if (_CultureList == null)
                {
                    _CultureList = new Dictionary<string, System.Globalization.CultureInfo>();
                    System.Globalization.CultureInfo[] cultures = System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.InstalledWin32Cultures);
                    foreach (System.Globalization.CultureInfo item in cultures.OrderBy(x => x.DisplayName))
                    {
                        if (!item.IsNeutralCulture)
                        {
                            if (!_CultureList.ContainsKey(item.DisplayName))
                                _CultureList.Add(item.DisplayName, item);
                        }
                    }
                }
                return _CultureList;
            }
        }
        #endregion

        #region Construction
        public UserControlDiagram()
        {
            InitializeComponent();

            // Set color palette
            //System.Windows.Forms.DataVisualization.Charting.ChartColorPalette actPal = this.chartDiagram.Palette;
            int selIdx = 0;
            foreach (System.Windows.Forms.DataVisualization.Charting.ChartColorPalette item in Enum.GetValues(typeof(System.Windows.Forms.DataVisualization.Charting.ChartColorPalette)))
            {
                this.toolStripComboBoxPalette.Items.Add(item.ToString());
                //if (item == actPal)
                //    selIdx = this.toolStripComboBoxPalette.Items.Count - 1;
            }
            this.toolStripComboBoxPalette.SelectedIndex = selIdx;
            this._SelectedCulture = System.Globalization.CultureInfo.CurrentCulture;
        }
        #endregion

        #region Public
        /// <summary>
        /// Set the chart data
        /// In xAxis there are the labels of the diagram's x-axis for integer based values. If for a certain x-value no
        /// label shall be displayed, provide an empty string. In ySeries there are severeal series of y-values and their
        /// series name. The y-values are provided in a one- or zero-based of double values. If zeroBased is 'true' the
        /// first y-value is assumed to belong to x-value '0', otherwise to x-value '1'.
        /// </summary>
        /// <param name="ySeries">Series of y-values</param>
        /// <param name="xAxis">Labels of x-axis, null for no labels</param>
        /// <param name="zeroBased">'true' if the first y-value is for x-value '0', default 'false'</param>
        public void SetChart(Dictionary<string, List<double>> ySeries, Dictionary<int, string> xAxis, bool zeroBased = false)
        {
            //try
            //{
            //    // Clear series data
            //    this.chartDiagram.ChartAreas[0].Visible = false;

            //    foreach (System.Windows.Forms.DataVisualization.Charting.Series item in this.chartDiagram.Series)
            //        item.Dispose();
            //    this.chartDiagram.Series.Clear();

            //    foreach (System.Windows.Forms.DataVisualization.Charting.CustomLabel item in this.chartDiagram.ChartAreas[0].AxisX.CustomLabels)
            //        item.Dispose();
            //    this.chartDiagram.ChartAreas[0].AxisX.CustomLabels.Clear();

            //    // Insert series data
            //    double max = 0.0;
            //    foreach (KeyValuePair<string, List<double>> yItems in ySeries)
            //    {
            //        System.Windows.Forms.DataVisualization.Charting.Series series = new System.Windows.Forms.DataVisualization.Charting.Series(yItems.Key, 1); //yItems.Value.Count);

            //        // Insert chart values
            //        int x = zeroBased ? 0 : 1;
            //        foreach (double item in yItems.Value)
            //        {
            //            series.Points.AddXY(x++, item);
            //            if (max < item)
            //                max = item;
            //        }
            //        this.chartDiagram.Series.Add(series);
            //    }
            //    // Calculate scaling factor
            //    _SclMax = max;
            //    this.chartDiagram.ChartAreas[0].AxisY.Maximum = _SclIni * _SclMax;

            //    // Set legend for x-values
            //    if (xAxis != null)
            //    {
            //        foreach (KeyValuePair<int, string> xValue in xAxis)
            //        {
            //            if (xValue.Value != "")
            //            {
            //                // Insert custom label
            //                System.Windows.Forms.DataVisualization.Charting.CustomLabel customLabel = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
            //                customLabel.FromPosition = xValue.Key;
            //                customLabel.ToPosition = xValue.Key;
            //                customLabel.Text = xValue.Value;
            //                this.chartDiagram.ChartAreas[0].AxisX.CustomLabels.Add(customLabel);
            //            }
            //        }
            //    }
            //    this.chartDiagram.ChartAreas[0].AxisX.LabelAutoFitStyle = System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.WordWrap;
            //}
            //catch (Exception ex)
            //{
            //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            //}
            //if (this.chartDiagram.ChartAreas.Count > 0)
            //    this.chartDiagram.ChartAreas[0].Visible = true;
        }

        /// <summary>
        /// Set title of the main chart area
        /// </summary>
        /// <param name="Title">Title</param>
        public void SetTitle(string Title)
        {
            //if (this.chartDiagram.Titles.Count > 0)
            //    this.chartDiagram.Titles[0].Text = Title;
        }
        #endregion

        #region Events
        private void toolStripComboBoxPalette_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.chartDiagram.Palette = (System.Windows.Forms.DataVisualization.Charting.ChartColorPalette)Enum.Parse(typeof(System.Windows.Forms.DataVisualization.Charting.ChartColorPalette), this.toolStripComboBoxPalette.SelectedItem.ToString());
        }

        private void toolStripButton3D_Click(object sender, EventArgs e)
        {
            Style3D = !Style3D;
        }

        private void toolStripButtonLeft_Click(object sender, EventArgs e)
        {
            //if (this.chartDiagram.ChartAreas.Count > 0)
            //{
            //    if (this.chartDiagram.ChartAreas[0].Area3DStyle.Rotation + _RotVal <= 180)
            //        this.chartDiagram.ChartAreas[0].Area3DStyle.Rotation += _RotVal;
            //}
        }

        private void toolStripButtonRight_Click(object sender, EventArgs e)
        {
            //if (this.chartDiagram.ChartAreas.Count > 0)
            //{
            //    if (this.chartDiagram.ChartAreas[0].Area3DStyle.Rotation - _RotVal >= -180)
            //        this.chartDiagram.ChartAreas[0].Area3DStyle.Rotation -= _RotVal;
            //}
        }

        private void toolStripButtonUp_Click(object sender, EventArgs e)
        {
            //if (this.chartDiagram.ChartAreas.Count > 0)
            //{
            //    if (this.chartDiagram.ChartAreas[0].Area3DStyle.Inclination - _IncVal >= -90)
            //        this.chartDiagram.ChartAreas[0].Area3DStyle.Inclination -= _IncVal;
            //}
        }

        private void toolStripButtonDown_Click(object sender, EventArgs e)
        {
            //if (this.chartDiagram.ChartAreas.Count > 0)
            //{
            //    if (this.chartDiagram.ChartAreas[0].Area3DStyle.Inclination + _IncVal <= 90)
            //        this.chartDiagram.ChartAreas[0].Area3DStyle.Inclination += _IncVal;
            //}
        }

        private void toolStripButtonRect_Click(object sender, EventArgs e)
        {
            //if (this.chartDiagram.ChartAreas.Count > 0)
            //{
            //    this.chartDiagram.ChartAreas[0].Area3DStyle.Inclination = 0;
            //    this.chartDiagram.ChartAreas[0].Area3DStyle.Rotation = 0;
            //}
        }

        private void toolStripButtonUpper_Click(object sender, EventArgs e)
        {
            //if (this.chartDiagram.ChartAreas.Count > 0)
            //{
            //    this.chartDiagram.ChartAreas[0].Area3DStyle.Inclination = 30;
            //    this.chartDiagram.ChartAreas[0].Area3DStyle.Rotation = 45;
            //}
        }

        private void toolStripButtonPerspectiveInc_Click(object sender, EventArgs e)
        {
            //if (this.chartDiagram.ChartAreas.Count > 0)
            //{
            //    if (this.chartDiagram.ChartAreas[0].Area3DStyle.Perspective + _PerVal <= 100)
            //        this.chartDiagram.ChartAreas[0].Area3DStyle.Perspective += _PerVal;
            //}
        }

        private void toolStripButtonPerspectiveDec_Click(object sender, EventArgs e)
        {
            //if (this.chartDiagram.ChartAreas.Count > 0)
            //{
            //    if (this.chartDiagram.ChartAreas[0].Area3DStyle.Perspective - _PerVal >= 0)
            //        this.chartDiagram.ChartAreas[0].Area3DStyle.Perspective -= _PerVal;
            //}

        }

        private void toolStripButtonHeightInc_Click(object sender, EventArgs e)
        {
            //if (this.chartDiagram.ChartAreas.Count > 0)
            //{
            //    if (chartDiagram.ChartAreas[0].AxisY.Maximum > (_SclVal * _SclMax))
            //        chartDiagram.ChartAreas[0].AxisY.Maximum -= (_SclVal * _SclMax);
            //}
        }

        private void toolStripButtonHeightDec_Click(object sender, EventArgs e)
        {
            //if (this.chartDiagram.ChartAreas.Count > 0)
            //{
            //    chartDiagram.ChartAreas[0].AxisY.Maximum += (_SclVal * _SclMax);
            //}
        }

        private void toolStripButtonSaveData_Click(object sender, EventArgs e)
        {
            try
            {
                //if (this.chartDiagram.Series.Count < 1)
                {
                    MessageBox.Show("No data available");
                    return;
                }

                //// Select culture
                //DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(CultureList.Keys.ToList(), "Select language", "Select export language for number format", true);
                //f.PresetSelection(_SelectedCulture.DisplayName);
                //if (f.ShowDialog() == DialogResult.OK)
                //{
                //    _SelectedCulture = CultureList[f.SelectedString];
                //}

                //// Set file save dialog
                //string path = "\\" + _DiagramPath.Trim().Trim('\\');
                //System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(DataDirectory + path);
                //if (!di.Exists)
                //    di.Create();
                //this.saveFileDialog.InitialDirectory = di.FullName;
                //this.saveFileDialog.FileName = _DefaultFileName + "Data";
                //this.saveFileDialog.DefaultExt = "txt";
                //this.saveFileDialog.Filter = "Text file|*.txt";
                //if (this.saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //{
                //    System.IO.FileInfo fi = new System.IO.FileInfo(this.saveFileDialog.FileName);
                //    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(fi.FullName, false, Encoding.UTF8))
                //    {
                //        // Write title row
                //        string line = "X-Value\t";
                //        for (int seriesIdx = 0; seriesIdx < this.chartDiagram.Series.Count; seriesIdx++)
                //            line += this.chartDiagram.Series[seriesIdx].Name + "\t";
                //        line += "Label";
                //        writer.WriteLine(line);

                //        // Write data rows
                //        for (int xIdx = 0; xIdx < this.chartDiagram.Series[0].Points.Count; xIdx++)
                //        {
                //            line = this.chartDiagram.Series[0].Points[xIdx].XValue.ToString(_SelectedCulture) + "\t";
                //            for (int seriesIdx = 0; seriesIdx < this.chartDiagram.Series.Count; seriesIdx++)
                //                line += this.chartDiagram.Series[seriesIdx].Points[xIdx].YValues[0].ToString(_SelectedCulture) + "\t";
                //            if (this.chartDiagram.ChartAreas[0].AxisX.CustomLabels.Count > xIdx)
                //                line += this.chartDiagram.ChartAreas[0].AxisX.CustomLabels[xIdx].Text;
                //            writer.WriteLine(line);
                //        }
                //        writer.Flush();
                //        writer.Close();
                //        writer.Dispose();
                //    }
                //}
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Set file save dialog
                string path = "\\" + _DiagramPath.Trim().Trim('\\');
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(DataDirectory + path);
                if (!di.Exists)
                    di.Create();
                this.saveFileDialog.InitialDirectory = di.FullName;
                this.saveFileDialog.FileName = _DefaultFileName;
                this.saveFileDialog.DefaultExt = "png";
                this.saveFileDialog.Filter = "PNG image|*.png|GIF image|*.gif|JPEG image|*.jpg";
                if (this.saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    System.Windows.Forms.DataVisualization.Charting.ChartImageFormat fileFormat;
                    System.IO.FileInfo fi = new System.IO.FileInfo(this.saveFileDialog.FileName);
                    switch (fi.Extension.ToLower())
                    {
                        case ".png":
                            fileFormat = System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png;
                            break;
                        case ".gif":
                            fileFormat = System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Gif;
                            break;
                        case ".jpg":
                        case ".jpeg":
                            fileFormat = System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Jpeg;
                            break;
                        default:
                            fileFormat = System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png;
                            break;
                    }
                    //this.chartDiagram.SaveImage(fi.FullName, fileFormat);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        #endregion
    }
}
