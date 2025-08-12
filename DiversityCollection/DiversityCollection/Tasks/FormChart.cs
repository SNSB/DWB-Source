using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityCollection.Tasks
{
    public partial class FormChart : Form
    {
        private System.Windows.Forms.DataVisualization.Charting.ChartArea _chartArea;

        public FormChart(System.Windows.Forms.DataVisualization.Charting.Chart Chart, System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea, string Title = "")
        {
            InitializeComponent();
            try
            {
                this.chart.ChartAreas.Clear();
                this.chart.ChartAreas.Add(chartArea);

                this.chart.Series.Clear();

                this.chart.Palette = Chart.Palette;
                foreach (System.Windows.Forms.DataVisualization.Charting.Title T in Chart.Titles)
                {
                    System.Windows.Forms.DataVisualization.Charting.Title title = new System.Windows.Forms.DataVisualization.Charting.Title(T.Text);
                    this.chart.Titles.Add(title);
                }
                foreach (System.Windows.Forms.DataVisualization.Charting.Series S in Chart.Series)
                {
                    System.Windows.Forms.DataVisualization.Charting.Series series = new System.Windows.Forms.DataVisualization.Charting.Series(S.Name);
                    series.ChartType = S.ChartType;
                    if (S.Label != null)
                    {
                        series.Label = S.Label.ToString();
                    }
                    if (S.LabelToolTip != null)
                    {
                        series.LabelToolTip = S.LabelToolTip.ToString();
                    }
                    if (S.MarkerImage != null)
                    {
                        series.MarkerImage = S.MarkerImage.ToString();
                    }
                    foreach (System.Windows.Forms.DataVisualization.Charting.DataPoint P in S.Points)
                    {
                        series.Points.Add(P);
                    }
                    //foreach (System.Windows.Forms.DataVisualization.Charting.DataPoint P in S.Points)
                    //{
                    //    System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint = new System.Windows.Forms.DataVisualization.Charting.DataPoint(P.XValue, P.YValues);
                    //    if (P.XValue == 0)
                    //    {
                    //        series.MarkerImage = "";
                    //        series.Label = "";
                    //        dataPoint.MarkerImage = null;
                    //    }
                    //    else
                    //    {
                    //    }
                    //    series.Points.Add(dataPoint);
                    //}
                    this.chart.Series.Add(series);
                }
                if (Title.Length > 0)
                    this.Text = Title;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private string ChartTitle()
        {
            string Title = "";
            foreach (System.Windows.Forms.DataVisualization.Charting.Title T in this.chart.Titles)
            {
                if (Title.Length > 0)
                    Title += "_";
                Title += T.Text.Replace(" ", "_").Replace("|", "_").Replace(".", "_");
            }
            while (Title.IndexOf("__") > -1)
                Title = Title.Replace("__", "_");
            return Title;
        }


        private void buttonSave_Click(object sender, EventArgs e)
        {
            string path = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.FolderType.Export) + this.ChartTitle() + "_Export_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".png";
            this.chart.SaveImage(path, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
            try
            {
                // System.Diagnostics.Process.Start(Path);
                Process.Start(new ProcessStartInfo
                {
                    FileName = path,
                    UseShellExecute = true
                });
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void FormChart_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.chart.ChartAreas.Clear();
        }
    }
}
