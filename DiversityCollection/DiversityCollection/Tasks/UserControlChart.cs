using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace DiversityCollection.Tasks
{
    public partial class UserControlChart : UserControl
    {
        #region Parameter

        private int _ID;
        private bool _ForCollection = true;

        private int? _CollectionID;
        private System.Collections.Generic.List<int> _CollectionIDs;
        private int? _CollectionTaskID;
        private string _ChartTitle;
        private int _MainWindowWidth;
        private int _MainWindowHeight;

        #endregion

        #region Construction

        public UserControlChart()
        {
            try
            {
                InitializeComponent();
                this.chartTask.Series.Clear();
                if (this.Parent != null)
                {
                    System.Object o = this.Parent;
                    while (o.GetType() == typeof(System.Windows.Forms.Control))
                    {

                    }
                }
#if !DEBUG
            this.tabControlSettings.TabPages.Remove(this.tabPageSettings);
#endif
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

#endregion

#region Interface

        public void InitChart(int ID, bool ForCollection = true)
        {
            _ID = ID;
            _ForCollection = ForCollection;
            IPM.ChartInit(ID, ForCollection);
            this.setMetricList();
            this.setPestList();
        }

        public int CollectionID
        {
            set
            {
                this._CollectionID = value;
                //this.setMetricImport();
                this._CollectionIDs = IPM.CollectionIDs(value);
                this._CollectionTaskID = null;
                this._ID = value;
                this._ForCollection = true;
                this.setPestList();
                this.labelCollection.Text = this.ChartTitles(_ID)[0];
            }
        }

        public int CollectionTaskID
        {
            set
            {
                this._CollectionTaskID = value;
                this._ID = value;
                this._ForCollection = false;
                this.setPestList();
                System.Collections.Generic.List<string> Titles = this.ChartTitles(_ID);
                this.labelCollection.Text = Titles[0];
                if (Titles.Count > 1)
                    this.labelCollection.Text += " - " + Titles[1];
            }
        }

        public void SetMainWindowSize(int Width, int Height)
        {
            this._MainWindowHeight = Height;
            this._MainWindowWidth = Width;
        }

#endregion

#region Chart

        private System.Data.DataTable _dataTableChart;
        private System.Collections.Generic.Dictionary<string, string> _Series;
        private System.Collections.Generic.Dictionary<string, string> _Spalten;

        private System.Windows.Forms.DataVisualization.Charting.SeriesChartType ChartType
        {
            get
            {
                if (this.radioButtonBubble.Checked)
                    return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
                else
                    return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            }
        }

        private string CollectionIDs()
        {
            string SQL = "";
            if (_CollectionIDs == null)
            {
                return "";
            }
            foreach(int ID in this._CollectionIDs)
            {
                if (SQL.Length > 0)
                    SQL += ", ";
                SQL += ID.ToString();
            }
            return SQL;
        }

        private void setMetricList()
        {
            this.checkedListBoxMetrics.Items.Clear();
            System.Collections.Generic.Dictionary<Metric, bool> Metrics = IPM.MetricsSelected();
            foreach (System.Collections.Generic.KeyValuePair<Metric, bool> P in Metrics)
            {
                this.checkedListBoxMetrics.Items.Add(P.Key.DisplayText(), P.Value);
            }
            this.buttonPrometheus.Visible = Metrics.Count == 1 && !this._ForCollection;
        }
        private void setPestList()
        {
            this.checkedListBoxPests.Items.Clear();
            System.Collections.Generic.Dictionary<string, bool> Pests = IPM.PestsSelected();
            foreach(System.Collections.Generic.KeyValuePair<string, bool> P in Pests)
            {
                this.checkedListBoxPests.Items.Add(P.Key, P.Value);
            }
        }

        private void buttonCreateChart_Click(object sender, EventArgs e)
        {
            this.CreateChart();
//#if !DEBUG
//            System.Windows.Forms.MessageBox.Show("Available in upcoming version");
//            return;
//#endif

        }

        private void CreateChart()
        {
            try
            {
                if (this._CollectionID == null && this._CollectionTaskID == null)
                {
                    System.Windows.Forms.MessageBox.Show("Please select an entry");
                    return;
                }
                // Adding the titles
                this.chartTask.Titles.Clear();
                int ID = -1;
                if (_CollectionID != null)
                ID = (int)_CollectionID;
                System.Collections.Generic.List<string> TT = this.ChartTitles(ID);
                foreach (string T in TT)
                {
                    this.chartTask.Titles.Add(T);
                }

                _dataTableChart = new DataTable();
                _Spalten = new Dictionary<string, string>();
                System.Collections.Generic.List<string> Metrics = new List<string>();
                System.Collections.Generic.List<string> Cleaning = new List<string>();
                System.Collections.Generic.List<string> Beneficials = new List<string>();
                IPM.ChartGetData(ref _dataTableChart, ref _Spalten, ref Metrics, ref Cleaning, ref Beneficials);
                //this.GetChartData(ref dataTable, ref Spalten);

                this.chartTask.DataSource = _dataTableChart;
                this.chartTask.Series.Clear();
                System.Collections.Generic.Dictionary<string, string> Series = new Dictionary<string, string>();
                System.Collections.Generic.Dictionary<string, string> CleaningPoints = new Dictionary<string, string>();
                System.Collections.Generic.Dictionary<string, string> BeneficialPoints = new Dictionary<string, string>();
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in _Spalten)
                {
                    if (Metrics.Contains(KV.Key))
                    {
                        Series.Add(KV.Key, KV.Value);
                    }
                    else if (Cleaning.Contains(KV.Key))
                        CleaningPoints.Add(KV.Key, KV.Value);
                    else if (Beneficials.Contains(KV.Key))
                        BeneficialPoints.Add(KV.Key, KV.Value);
                    else
                    {
                        if (KV.Key.IndexOf("|") == -1)
                        {
                            if (this.ChartType == System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble)
                            {
                                //Series.Add(KV.Key, KV.Value + "a, " + KV.Value + "b");
                                Series.Add(KV.Key, KV.Value + ", " + KV.Value);
                            }
                            else
                                Series.Add(KV.Key, KV.Value);
                        }
                    }
                }
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Series)
                {
                    this.chartTask.Series.Add(KV.Key);
                    this.chartTask.Series[KV.Key].XValueMember = "Zeitpunkt";
                    if (Metrics.Contains(KV.Key))
                    {
                        this.chartTask.Series[KV.Key].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
                    }
                    else
                    {
                        this.chartTask.Series[KV.Key].ChartType = this.ChartType;
                        if (this.ChartType == System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble)
                        {
                            //int i = this.chartTask.Series[KV.Key].Points.AddXY(1, 1, 0);
                            //this.chartTask.Series[KV.Key].Points[i].Color = Color.Transparent;
                            //this.chartTask.Series[KV.Key]["BubbleScaleMin"] = "2";
                        }
                    }
                    this.chartTask.Series[KV.Key].YValueMembers = KV.Value;
                }
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in CleaningPoints)
                {
                    this.chartTask.Series.Add(KV.Key);
                    this.chartTask.Series[KV.Key].XValueMember = "Zeitpunkt";
                    if (Cleaning.Contains(KV.Key))
                    {
                        this.chartTask.Series[KV.Key].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                        string Path = Folder.Report(Folder.ReportFolder.TaskImg) + "Cleaning.ico";
                        System.IO.FileInfo FI = new System.IO.FileInfo(Path);
                        if (FI.Exists)
                            this.chartTask.Series[KV.Key].MarkerImage = Path;
                        else { }
                        if (CleaningPoints.Count > 1)
                        {
                            this.chartTask.Series[KV.Key].Label = KV.Key.Substring(0, 2) + ".";
                            this.chartTask.Series[KV.Key].LabelToolTip = KV.Key;
                        }
                    }
                    this.chartTask.Series[KV.Key].YValueMembers = KV.Value;
                }
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in BeneficialPoints)
                {
                    this.chartTask.Series.Add(KV.Key);
                    this.chartTask.Series[KV.Key].XValueMember = "Zeitpunkt";
                    if (Beneficials.Contains(KV.Key))
                    {
                        this.chartTask.Series[KV.Key].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                        string Path = Folder.Report(Folder.ReportFolder.TaskImg) + "Animal.ico";
                        System.IO.FileInfo FI = new System.IO.FileInfo(Path);
                        if (FI.Exists)
                            this.chartTask.Series[KV.Key].MarkerImage = Path;
                        else { }
                        if (BeneficialPoints.Count > 1)
                        {
                            this.chartTask.Series[KV.Key].Label = KV.Key.Substring(0, 2) + ".";
                            this.chartTask.Series[KV.Key].LabelToolTip = KV.Key;
                        }
                    }
                    this.chartTask.Series[KV.Key].YValueMembers = KV.Value;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private System.Collections.Generic.List<string> ChartTitles(int ID)
        {
            System.Collections.Generic.List<string> Titles = new List<string>();
            string SQL = "";
            if (this._ForCollection)
                SQL = "SELECT C.DisplayText FROM [dbo].[CollectionHierarchyAll]() C WHERE C.CollectionID = " + _ID.ToString(); 
            else
                SQL = "SELECT C.DisplayText FROM [dbo].[CollectionHierarchyAll]() C INNER JOIN CollectionTask T ON T.CollectionID = C.CollectionID AND T.CollectionTaskID = " + _ID.ToString();
            System.Data.DataTable dt = new DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
            if (!this._ForCollection)
            {
                SQL = "SELECT C.TaskDisplayText AS DisplayText FROM [dbo].[CollectionTaskHierarchyAll]() C WHERE C.CollectionTaskID = " + _ID.ToString();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
            }
            foreach (System.Data.DataRow R in dt.Rows)
                Titles.Add(R[0].ToString());
            return Titles;
        }

        private System.Collections.Generic.List<string> Metrics = new List<string>();

        //private void GetChartData(ref System.Data.DataTable dataTable, ref System.Collections.Generic.Dictionary<string, string> Spalten)
        //{
        //    try
        //    {
        //        Spalten.Clear();

        //        // getting the metric
        //        Metrics = new List<string>();
        //        string SQL = "";
        //        if (this._CollectionTaskID != null)
        //        {
        //            SQL = "SELECT T.MetricDescription " +
        //                "FROM CollectionTask AS T " +
        //                " WHERE T.CollectionTaskID IN (" + this._CollectionTaskID + ") " +
        //                " AND T.MetricDescription <> '' AND (T.DisplayOrder > 0) " +
        //                "GROUP BY T.MetricDescription, T.DisplayOrder ORDER BY T.MetricDescription";
        //        }
        //        else if (this.CollectionIDs().Length > 0)
        //        {
        //            SQL = "SELECT T.MetricDescription " +
        //                "FROM CollectionTask AS T " +
        //                " WHERE T.CollectionID IN (" + this.CollectionIDs() + ") " + 
        //                " AND T.MetricDescription <> '' AND (T.DisplayOrder > 0) " +
        //                "GROUP BY T.MetricDescription, T.DisplayOrder ORDER BY T.MetricDescription";
        //        }
        //        if (SQL.Length == 0)
        //            return;

        //        System.Data.DataTable dataTableMetrics = new DataTable();
        //        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTableMetrics);
        //        int ii = 0;
        //        for (int i = 0; i < dataTableMetrics.Rows.Count; i++)
        //        {
        //            string series = dataTableMetrics.Rows[i][0].ToString();
        //            Spalten.Add(series, "Wert_" + (i + 1).ToString());
        //            Metrics.Add(series);
        //            ii++;
        //        }

        //        // getting the series
        //        if (this._CollectionTaskID != null)
        //        {
        //            SQL = "SELECT T.DisplayText " +
        //            "FROM CollectionTask AS T " +
        //            " WHERE (T.CollectionTaskID = " + this._CollectionTaskID + " OR T.CollectionTaskParentID = " + this._CollectionTaskID + ") " +
        //            " AND T.DisplayText <> '' AND(T.DisplayOrder > 0) AND(NOT(T.TaskStart IS NULL)) AND(NOT(T.NumberValue IS NULL))" +
        //            "GROUP BY T.DisplayText, T.DisplayOrder ORDER BY T.DisplayText";
        //        }
        //        else
        //        {
        //            SQL = "SELECT T.DisplayText " +
        //                "FROM CollectionTask AS T " +
        //                " WHERE T.CollectionID IN (" + this.CollectionIDs() + ") " +
        //                " AND T.DisplayText <> '' AND(T.DisplayOrder > 0) AND (NOT(T.TaskStart IS NULL)) AND (NOT(T.NumberValue IS NULL))" +
        //                "GROUP BY T.DisplayText, T.DisplayOrder ORDER BY T.DisplayText";
        //        }
        //        System.Data.DataTable dataTableSeries = new DataTable();
        //        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTableSeries);
        //        for (int i = 0; i < dataTableSeries.Rows.Count; i++)
        //        {
        //            string series = dataTableSeries.Rows[i][0].ToString();
        //            Spalten.Add(series, "Wert_" + (ii + 1).ToString());
        //            Spalten.Add(series + "|a", "Wert_" + (ii + 1).ToString() + "a");
        //            Spalten.Add(series + "|b", "Wert_" + (ii + 1).ToString() + "b");
        //            ii++;
        //        }

        //        string Restriction = "";
        //        if (this._CollectionTaskID != null)
        //            Restriction = " (T.CollectionTaskID = " + this._CollectionTaskID.ToString() + " OR T.CollectionTaskParentID = " + this._CollectionTaskID + ") ";
        //        else
        //            Restriction = " C.CollectionID IN (" + this.CollectionIDs() + ") ";
        //        //// getting start and end
        //        SQL = "/*no value*/ declare @NoValue int; Set @NoValue = NULL; " +
        //            "/*bubble*/ declare @F int; Set @F = " + this.numericUpDownBubble.Value.ToString() + "; " + 
        //            "/*getting start and end*/ " +
        //            "declare @start date; " +
        //            "declare @end date; " +
        //            "set @start = (SELECT cast(min(CONVERT(varchar(10), T.TaskStart, 120)) as date) AS TaskStart " +
        //            "FROM [dbo].[Collection] C INNER JOIN dbo.CollectionTask T ON " + Restriction + " AND C.CollectionID = T.CollectionID AND C.Type = 'Trap' AND T.DisplayText <> '') ; " +
        //            "set @end = (SELECT cast(max(CONVERT(varchar(10), T.TaskStart, 120)) as date) AS TaskStart " +
        //            "FROM [dbo].[Collection] C INNER JOIN dbo.CollectionTask T ON " + Restriction + " AND C.CollectionID = T.CollectionID AND C.Type = 'Trap' AND T.DisplayText <> ''); ";
        //        // metric start and end
        //        SQL += "/*metric start and end*/ " +
        //            "declare @MetricStart date; " +
        //            "declare @MetricEnd date; " +
        //            "set @MetricStart = (SELECT cast(min(CONVERT(varchar(10), M.MetricDate, 120))  as date) AS TaskStart " +
        //            "FROM [dbo].[Collection] C INNER JOIN dbo.CollectionTask T ON " + Restriction + " AND C.CollectionID = T.CollectionID AND C.Type = 'Sensor' INNER JOIN CollectionTaskMetric AS M ON M.CollectionTaskID = T.CollectionTaskID) ; " +
        //            "set @MetricEnd = (SELECT cast(max(CONVERT(varchar(10), M.MetricDate, 120)) as date) AS TaskStart " +
        //            "FROM [dbo].[Collection] C INNER JOIN dbo.CollectionTask T ON " + Restriction + " AND C.CollectionID = T.CollectionID AND C.Type = 'Sensor' INNER JOIN CollectionTaskMetric AS M ON M.CollectionTaskID = T.CollectionTaskID); ";
        //        // setting the wider range
        //        SQL += "/*setting the wider range*/ " +
        //            "if (@start IS NULL OR (NOT @MetricStart IS NULL AND @start > @MetricStart)) " +
        //            "begin set @start = @MetricStart end; " +
        //            "if (@end IS NULL OR (NOT @MetricEnd IS NULL AND @end < @MetricEnd)) " +
        //            "begin set @end = @MetricEnd end; ";
        //        // inserting start
        //        SQL += "/*declare table containing data*/ " +
        //            "declare @Werte table(Zeitpunkt date ";
        //        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Spalten)
        //            SQL += ", " + KV.Value + " float";
        //        SQL += "); /*inserting start*/ insert into @Werte(Zeitpunkt";
        //        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Spalten)
        //            SQL += ", " + KV.Value;
        //        SQL += ") Values(@start";
        //        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Spalten)
        //            SQL += ", @NoValue";
        //        SQL += "); ";

        //        // filling the table with months
        //        SQL += "while (@end > (select max(Zeitpunkt) from @Werte)) " +
        //            "begin " +
        //            "insert into @Werte(Zeitpunkt";
        //        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Spalten)
        //            SQL += ", " + KV.Value;
        //        SQL += ") Values((select DateAdd(";
        //        if (dataTableMetrics.Rows.Count == 0)
        //            SQL += "month";
        //        else
        //            SQL += "day";
        //        SQL += ", 1, max(Zeitpunkt)) from @Werte) ";
        //        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Spalten)
        //            SQL += ", @NoValue";
        //        SQL += ") " +
        //            "end; ";

        //        SQL += "/*getting number values*/";
        //        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Spalten)
        //        {
        //            if (Metrics.Contains(KV.Key))
        //                continue;
        //            // getting the values
        //            string DisplayText = KV.Key;
        //            string UpDown = "";
        //            if (KV.Key.IndexOf("|") > 0)
        //            {
        //                DisplayText = KV.Key.Substring(0, KV.Key.IndexOf("|"));
        //                UpDown = KV.Key.Substring(KV.Key.IndexOf("|")+1);
        //            }
        //            SQL += "/* getting " + KV.Value + " */  declare @" + KV.Value + " table(Tag date, Wert float); " +
        //                "insert into @" + KV.Value + " (Tag, Wert) " +
        //                "SELECT cast(min(CONVERT(varchar(10), C.TaskStart, 120)) as date), ";
        //            switch(UpDown)
        //            {
        //                case "":
        //                case "a":
        //                    SQL += " sum(C.NumberValue) ";
        //                    break;
        //                case "b":
        //                    SQL += " sum(C.NumberValue)*@F ";
        //                    break;
        //            }
        //            SQL += "FROM CollectionTask AS C " +
        //                "WHERE(C.DisplayText = N'" + DisplayText + "') AND (NOT(C.TaskStart IS NULL)) AND (NOT(C.NumberValue IS NULL)) AND (C.DisplayOrder > 0) " +
        //                "GROUP BY C.DisplayText, CONVERT(varchar(10), C.TaskStart, 120); ";
        //            // transfer values in main table
        //            SQL += "update W set " + KV.Value + " = W1.Wert " +
        //                "from @Werte W inner " +
        //                "join @" + KV.Value + " W1 on cast(CONVERT(varchar(10), W.Zeitpunkt, 120) as date) = W1.Tag; ";
        //        }
        //        SQL += "/*getting metrics*/ ";
        //        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Spalten)
        //        {
        //            if (!Metrics.Contains(KV.Key))
        //                continue;
        //            // getting the values
        //            SQL += "/* getting " + KV.Value + " */  declare @" + KV.Value + " table(Tag date, Wert float); " +
        //                "insert into @" + KV.Value + " (Tag, Wert) " +
        //                "SELECT cast(min(CONVERT(varchar(10), M.MetricDate, 120)) as date), avg(M.MetricValue) " +
        //                "FROM CollectionTask C INNER JOIN CollectionTaskMetric AS M ON C.CollectionTaskID = M.CollectionTaskID AND (C.MetricDescription = N'" + KV.Key + "') AND (NOT(M.MetricDate IS NULL)) AND (NOT(M.MetricValue IS NULL)) AND (C.DisplayOrder > 0) " +
        //                "GROUP BY C.MetricDescription, CONVERT(varchar(10), M.MetricDate, 120); ";
        //            // transfer values in main table
        //            SQL += "update W set " + KV.Value + " = W1.Wert " +
        //                "from @Werte W inner " +
        //                "join @" + KV.Value + " W1 on W.Zeitpunkt = W1.Tag; ";
        //        }
        //        SQL += "; select CONVERT(varchar(10), Zeitpunkt, 120) AS Zeitpunkt ";
        //        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Spalten)
        //            SQL += ", " + KV.Value;
        //        SQL += " from @Werte";
        //        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTable);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        private void buttonOpenChart_Click(object sender, EventArgs e)
        {
            try
            {
                string Chart = "ChartAreaExtern";
                System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea = new System.Windows.Forms.DataVisualization.Charting.ChartArea(Chart);
                Tasks.FormChart f = new FormChart(this.chartTask, chartArea, this.chartTask.Titles[0].Text);
                try
                {
                    if (this.Parent != null)
                    {
                        System.Windows.Forms.Control C = (System.Windows.Forms.Control)this.Parent;
                        while (C.Parent != null)
                        {
                            C = (System.Windows.Forms.Control)C.Parent;
                        }
                        this._MainWindowHeight = C.Height;
                        this._MainWindowWidth = C.Width;
                    }
                    if (this._MainWindowHeight == 0)
                    {
                        this._MainWindowHeight = this.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Height;
                        this._MainWindowWidth = this.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Width;
                    }
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                f.Height = this._MainWindowHeight - 20;
                f.Width = this._MainWindowWidth - 20;
                f.ShowDialog();
                this.CreateChart();
                //if (this.chartTask.ChartAreas.Contains(chartArea))
                //    this.chartTask.ChartAreas[Chart].Dispose();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        //private string DateFromColumn(string Column, string Grouping = "", bool ForMetric = false)
        //{
        //    string SQL = "CAST(" + Grouping + "(CONVERT(";
        //    if (ForMetric)
        //    {
        //        if (this.radioButtonMetricGroupingDay.Checked)
        //        {

        //        }
        //        else
        //        {

        //        }
        //    }
        //    else
        //    {
        //        if (this.radioButtonNumberGroupingDay.Checked)
        //        {

        //        }
        //        else
        //        {

        //        }
        //    }
        //    return SQL;
        //}

#region Export

        private string _TableExportFile = "";

        private string ChartTitle()
        {
            string Title = "";
            foreach (string T in this.ChartTitles(this._ID))
            {
                if (Title.Length > 0)
                    Title += "_";
                Title += T.Replace(" ", "_").Replace("|", "_").Replace(".", "_");
            }
            while (Title.IndexOf("__") > -1)
                Title = Title.Replace("__", "_");
            return Title;
        }
        private void buttonExport_Click(object sender, EventArgs e)
        {
            if (_dataTableChart != null && _dataTableChart.Rows.Count > 0)
            {
                System.IO.StreamWriter sw;
                DateTime expTim = DateTime.Now;
                this._TableExportFile = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.FolderType.Export) + this.ChartTitle() + "_Export_" + expTim.ToString("yyyyMMdd_hhmmss") + ".txt";
                try
                {
                    if (System.IO.File.Exists(this._TableExportFile))
                        sw = new System.IO.StreamWriter(this._TableExportFile, true, System.Text.Encoding.UTF8);
                    else
                        sw = new System.IO.StreamWriter(this._TableExportFile, false, System.Text.Encoding.UTF8);
                }
                catch (System.Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                    return;
                }
                try
                {
                    sw.WriteLine(this.ChartTitle() + " export for chart");
                    sw.WriteLine();
                    sw.WriteLine("User:\t" + System.Environment.UserName);
                    sw.Write("Date:\t");
                    sw.WriteLine(expTim);
                    sw.WriteLine();
                    foreach (System.Data.DataColumn C in this._dataTableChart.Columns)
                    {
                        string value = C.ColumnName;
                        if (_Spalten.ContainsValue(value))
                        {
                            foreach(System.Collections.Generic.KeyValuePair<string, string> KV in _Spalten)
                            {
                                if (KV.Value == value)
                                {
                                    value = KV.Key;
                                    break;
                                }
                            }
                        }
                        sw.Write(value + "\t");
                    }
                    sw.WriteLine();
                    this.progressBar.Visible = true;
                    this.progressBar.Value = 0;
                    this.progressBar.Maximum = this._dataTableChart.Rows.Count;

                    // Process data grid view rows to preserve order
                    foreach (System.Data.DataRow R in this._dataTableChart.Rows)
                    {
                        foreach (System.Data.DataColumn C in this._dataTableChart.Columns)
                        {
                            string value = R[C.ColumnName].ToString();
                            sw.Write(value.ToString() + "\t");
                        }
                        sw.WriteLine();
                        if (this.progressBar.Value < this.progressBar.Maximum)
                            this.progressBar.Value++;
                    }
                    Application.DoEvents();
                    System.Windows.Forms.MessageBox.Show("Data were exported to " + this._TableExportFile);
                    this.progressBar.Visible = false;
                }
                catch
                {
                }
                finally
                {
                    sw.Close();
                }

            }
        }

        private void buttonExportOpen_Click(object sender, EventArgs e)
        {
            if (this._TableExportFile.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("So far nothing has been exported");
                return;
            }
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = this._TableExportFile,
                    UseShellExecute = true
                });
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void buttonSaveImage_Click(object sender, EventArgs e)
        {
            string path = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.FolderType.Export) + this.ChartTitle() + "_Export_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".png";
            this.chartTask.SaveImage(path, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
            try
            {
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

#endregion

        private void buttonPrometheus_Click(object sender, EventArgs e)
        {
            if (!this._ForCollection)
            {
                if (IPM.MetricsSelected().Count == 1)
                {
                    DiversityCollection.Tasks.Metric metric = IPM.MetricsSelected().First().Key;
                    string SQL = "SELECT MetricSource FROM CollectionTask WHERE CollectionTaskID = " + this._ID.ToString();
                    string Source = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    string Title = "Import for " + metric.Description + " [" + metric.Unit + "]";
                    string Unit = metric.Unit;
                    FormPrometheus f = new FormPrometheus(Source, this._ID, Unit, Title);
                    f.ShowDialog();
                    if (Source != f.Souce())
                    {
                        SQL = "UPDATE C SET MetricSource = '" + f.Souce() + "' FROM CollectionTask C WHERE C.CollectionTaskID = " + this._ID.ToString();
                        DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                    }
                }
            }
            //System.Data.DataRowView R = (System.Data.DataRowView)this.collectionTaskBindingSource.Current;
            //string Source = R["MetricSource"].ToString();
            //int CollectionTaskID = int.Parse(R["CollectionTaskID"].ToString());
            //string Title = "Import for " + R["MetricDescription"].ToString() + " [" + R["MetricUnit"].ToString() + "]";
            //string Unit = R["MetricUnit"].ToString();
            //FormPrometheus f = new FormPrometheus(Source, CollectionTaskID, Unit, Title);
            //f.ShowDialog();
            //if (Source != f.Souce())
            //{
            //    R.BeginEdit();
            //    R["MetricSource"] = f.Souce();
            //    R.EndEdit();
            //}
        }

#endregion

        //#region Metric import

        //private void setMetricImport()
        //{
        //    string SQL = "SELECT COUNT(*) FROM CollectionTask C INNER JOIN Task T ON T.TaskID = C.TaskID AND T.Type = 'Sensor' WHERE C.CollectionID = " + this._CollectionID.ToString();
        //    int Count;
        //    if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out Count) && Count > 0)
        //        this.buttonImportMetric.Visible = true;
        //    else
        //        this.buttonImportMetric.Visible = false;
        //}
        //private void buttonImportMetric_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string SQL = "SELECT CollectionTaskID, C.MetricDescription FROM CollectionTask C INNER JOIN Task T ON T.TaskID = C.TaskID AND T.Type = 'Sensor' WHERE C.CollectionID = " + this._CollectionID.ToString();
        //        System.Data.DataTable dt = new DataTable();
        //        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
        //        int? ID = null;
        //        if (dt.Rows.Count > 0)
        //        {
        //            if (dt.Rows.Count > 0)
        //            {
        //                if (dt.Rows.Count == 1)
        //                {
        //                    ID = int.Parse(dt.Rows[0][0].ToString());
        //                }
        //                else
        //                {
        //                    DiversityWorkbench.Forms.FormGetStringFromList fT = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "MetricDescription", "CollectionTaskID", "Task", "Please select a task");
        //                    fT.ShowDialog();
        //                    if (fT.DialogResult == DialogResult.OK)
        //                    {
        //                        ID = int.Parse(fT.SelectedValue);
        //                    }
        //                }
        //            }
        //            else
        //                ID = int.Parse(dt.Rows[0][0].ToString());
        //            if (ID != null)
        //            {
        //                SQL = "SELECT MetricSource, MetricUnit, MetricDescription FROM CollectionTask AS C WHERE CollectionTaskID = " + ID.ToString();
        //                dt = new DataTable();
        //                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
        //                FormPrometheus f = new FormPrometheus(dt.Rows[0][0].ToString(), (int)ID, dt.Rows[0][1].ToString(), dt.Rows[0][2].ToString());
        //                f.ShowDialog();
        //                if (f.DialogResult == DialogResult.OK)
        //                {

        //                }
        //            }
        //        }
        //        else
        //        {
        //            System.Windows.Forms.MessageBox.Show("No sensors found");
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //#endregion

    }
}
