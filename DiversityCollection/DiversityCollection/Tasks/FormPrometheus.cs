using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityCollection.Tasks
{
    public partial class FormPrometheus : Form
    {

        #region Parameter

        private string _ImportSource = "";
        private int _CollectionTaskID;
        private string _Unit = "";
        private string _Sensor = "";

        public enum State { Search, GetSensor, Import, Alert, MetricSelection }
        private State _State = State.Import;

        #endregion

        #region Construction

        public FormPrometheus()
        {
            InitializeComponent();
#if !DEBUGx
            this.tabControl.TabPages.Remove(this.tabPageAlerting);
            this.tabControl.TabPages.Remove(this.tabPageConfiguration);
            this.tabControl.TabPages.Remove(this.tabPageSearch);
            this.tabControl.TabPages.Remove(this.tabPageStarting);
#endif
        }

        public FormPrometheus(State state, string Sensor = "")
        {
            InitializeComponent();
            this._State = state;
            this._Sensor = Sensor;
            this.initForm();
        }


        public FormPrometheus(string Source, int CollectionTaskID, string Unit = "", string Title = "")
        {
            InitializeComponent();
            this._State = State.Import;
            this.initForm();
            if (Source.Length > 0)
            {
                //Souce();
            }
            this.textBoxImportSource.Text = Source;
            this._CollectionTaskID = CollectionTaskID;
            //this.initForImport();
            if (Title.Length > 0)
                this.Text = Title;
            _Unit = Unit;
            if (Source.Trim().Length == 0)
                this.textBoxImportSource.BackColor = System.Drawing.Color.Pink;
        }

        public FormPrometheus(int CollectionTaskID)
        {
            InitializeComponent();
            try
            {
                string SQL = "SELECT MetricSource, MetricUnit, MetricDescription FROM CollectionTask AS C WHERE CollectionTaskID = " + CollectionTaskID.ToString();
                System.Data.DataTable dt = new DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                if (dt.Rows.Count == 1)
                {
                    this.textBoxImportSource.Text = dt.Rows[0][0].ToString();
                    this._CollectionTaskID = CollectionTaskID;
                    this.initForImport();
                    this.Text = dt.Rows[0][2].ToString();
                    _Unit = dt.Rows[0][1].ToString();
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion
                
        #region Form

        private void initForm()
        {
            this.tabControl.TabPages.Clear();
            switch(this._State)
            {
                case State.Import:
                    if (!this.tabControl.TabPages.Contains(this.tabPageImport))
                        this.tabControl.TabPages.Add(this.tabPageImport);
                    this.initImportUnits();
                    this.initImportPrometheusApiSource();
                    break;
                case State.Search:
                    if (!this.tabControl.TabPages.Contains(this.tabPageSearch))
                        this.tabControl.TabPages.Add(this.tabPageSearch);
                    this.initSearchStatistics();
                    this.initSearchPrometheus();
                    this.initSearchPrometheusSensors();
                    //this.initSearchPrometheusJobs();
                    break;
                case State.Alert:
                    if (!this.tabControl.TabPages.Contains(this.tabPageAlerting))
                        this.tabControl.TabPages.Add(this.tabPageAlerting);
                    break;
                case State.MetricSelection:
                    this.initMetricSelection();
                    goto case State.GetSensor;
                case State.GetSensor:
                    if (!this.tabControl.TabPages.Contains(this.tabPageSensors))
                        this.tabControl.TabPages.Add(this.tabPageSensors);
                    this.initSensors();
                    break;
            }
            this.setSize();
        }

        private void setSize()
        {
            switch (this._State)
            {
                case State.Import:
                    this.Height = 500;
                    this.Width = 500;
                    break;
                case State.Search:
                    this.Height = 200;
                    this.Width = 300;
                    break;
                case State.GetSensor:
                    this.Width = 300;
                    break;
                case State.MetricSelection:
                    this.Height = 200;
                    this.Width = 300;
                    break;
                default:
                    this.Height = 500;
                    this.Width = 500;
                    break;
            }
        }

        private void FormPrometheus_FormClosing(object sender, FormClosingEventArgs e)
        {
            Prometheus.PrometheusApisSave();
        }

        #endregion

        #region Get Sensor

        private string _SensorDomain = "IPM";

        private void initSensors()
        {
            this.textBoxSensorIP.Text = Settings.Default.PrometheusServer;
            this.maskedTextBoxSensorPort.Text = Settings.Default.PrometheusPort.ToString();
            // init Prometheus
            Prometheus.Init(_SensorDomain);
            // Filter
            this.SensorInitFilter();
            // Tree
            this.SensorBuildTree();
            // Statistics
            this.SensorInitStatitics();
        }

        private void SensorInitFilter()
        {
            string Domain = _SensorDomain;
            int i = 0;
            this.comboBoxSensorDomainFilter.Items.Clear();
            foreach (string D in Prometheus.DomainList())
            {
                this.comboBoxSensorDomainFilter.Items.Add(D);
                if (D == Domain)
                    i = this.comboBoxSensorDomainFilter.Items.Count - 1;
            }
            if (this.comboBoxSensorDomainFilter.Items.Count > 0)
                this.comboBoxSensorDomainFilter.SelectedIndex = i;
        }

        private void buttonSensorSearch_Click(object sender, EventArgs e)
        {
            this.SensorBuildTree(this.textBoxSensorSensorFilter.Text);
        }

        private void SensorBuildTree(string Filter = "")
        {
            this.treeViewSensor.Nodes.Clear();
            System.Collections.Generic.SortedDictionary<string, System.Windows.Forms.TreeNode> sensors = new SortedDictionary<string, System.Windows.Forms.TreeNode>();
            foreach (string Server in Prometheus.Server())
            {
                System.Windows.Forms.TreeNode server = new TreeNode(Server, 0, 0);
                this.treeViewSensor.Nodes.Add(server);
                if (Server == Prometheus.PrometheusServer)
                {
                    foreach (string Domain in Prometheus.DomainList())
                    {
                        System.Windows.Forms.TreeNode domain = new TreeNode(Domain, 1, 1);
                        //domain.Tag = Domain;
                        server.Nodes.Add(domain);
                        foreach(string Sensor in Prometheus.PrometheusSensors(Domain))
                        {
                            if (Filter.Length > 0)
                            {
                                string[] ff = Filter.ToUpper().Split(new char[] { '*' });
                                bool Match = true;
                                for(int i = 0; i < ff.Length; i++)
                                {
                                    if (ff[i].Length > 0)
                                    {
                                        if (i == 0 && !Sensor.StartsWith(ff[i]))
                                        { Match = false; break; }
                                        if (i == ff.Length - 1 && !Sensor.EndsWith(ff[i]))
                                        { Match = false; break; }
                                        if (Sensor.IndexOf(ff[i]) == -1)
                                        { Match = false; break; }
                                    }
                                }
                                if (!Match)
                                    continue;
                            }
                            System.Windows.Forms.TreeNode sensor = new TreeNode(Sensor, 2, 2);
                            sensor.Tag = Sensor;
                            sensors.Add(Sensor, sensor);
                            //domain.Nodes.Add(sensor);
                            foreach(System.Collections.Generic.KeyValuePair<string, PrometheusMetric> Metric in Prometheus.PrometheusMetricList(Domain, Sensor))
                            {
                                int imageIndex = 3;
                                switch(Metric.Value.Type)
                                {
                                    case PrometheusMetric.MetricType.Humidity:
                                        imageIndex = 4;
                                        break;
                                    case PrometheusMetric.MetricType.Temperature:
                                        imageIndex = 5;
                                        break;
                                }
                                System.Windows.Forms.TreeNode metric = new TreeNode(Metric.Value.MetricDisplayText, imageIndex, imageIndex);
                                metric.Tag = Metric.Value;
                                sensor.Nodes.Add(metric);
                            }
                        }
                        foreach(System.Collections.Generic.KeyValuePair<string, System.Windows.Forms.TreeNode> KV in sensors)
                        {
                            domain.Nodes.Add(KV.Value);
                        }
                        domain.Expand();
                    }
                    server.Expand();
                }
            }
            //treeViewSensor.ExpandAll();
        }

        private void treeViewSensor_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.buttonSensorOK.Enabled = this.treeViewSensor.SelectedNode.Tag != null 
                && (this.treeViewSensor.SelectedNode.Tag.GetType() == typeof(PrometheusMetric) || this.treeViewSensor.SelectedNode.Tag.GetType() == typeof(string));
        }

        private void SensorInitStatitics()
        {
            foreach (string s in Prometheus.Statics())
            {
                this.comboBoxSensorStatistics.Items.Add(s);
            }
            this.comboBoxSensorStatistics.SelectedIndex = 0;
            this.comboBoxSensorStatistics.SelectedItem = this.comboBoxSensorStatistics.Items[0];

        }

        private void buttonSensorOK_Click(object sender, EventArgs e)
        {
            if (this.treeViewSensor.SelectedNode.Tag != null)
            {
                if (this.treeViewSensor.SelectedNode.Tag.GetType() == typeof(PrometheusMetric))
                {
                    PrometheusMetric prometheusMetric = (PrometheusMetric)this.treeViewSensor.SelectedNode.Tag;
                    prometheusMetric.setStatistics(this.comboBoxSensorStatistics.SelectedItem.ToString());
                    _Api = prometheusMetric.Server + "/api/v1/query_range?query=" + prometheusMetric.PrometheusIdentifier + "&" + prometheusMetric.Statistics+ "(" + prometheusMetric.Metric + ")";
                }
                else if (this.treeViewSensor.SelectedNode.Tag.GetType() == typeof(string))
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, PrometheusMetric> KV in getSensorMetric())
                        KV.Value.setStatistics(this.comboBoxSensorStatistics.SelectedItem.ToString());
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public PrometheusMetric getPrometheusMetric()
        {
            PrometheusMetric prometheusMetric = (PrometheusMetric)this.treeViewSensor.Tag;
            prometheusMetric.setStatistics(this.comboBoxSensorStatistics.SelectedItem.ToString());
            return prometheusMetric;
        }

        public System.Collections.Generic.SortedDictionary<string, PrometheusMetric> getSensorMetric()
        {
            System.Collections.Generic.SortedDictionary<string, PrometheusMetric> dict = new SortedDictionary<string, PrometheusMetric>();
            if (this.treeViewSensor.SelectedNode.Tag != null && this.treeViewSensor.SelectedNode.Tag.GetType() == typeof(string))
            {
                string Sensor = this.treeViewSensor.SelectedNode.Tag.ToString();
                dict = Prometheus.PrometheusMetricList(Sensor);
            }
            return dict;
        }

        #endregion

        #region Metric selection

        private void initMetricSelection()
        {
            this.textBoxSensorIP.Enabled = false;
            this.maskedTextBoxSensorPort.Enabled = false;
            this.comboBoxSensorDomainFilter.Enabled = false;
            this.textBoxSensorSensorFilter.Enabled = false;
            this.buttonSensorSearch.Visible = false;
            // tree
            this.treeViewSensor.CheckBoxes = true;
        }

        private void MetricInitChecklist()
        {

        }

        #endregion

        #region Search

        private void initSearchStatistics()
        {
            foreach (string s in this.ImportStatistics())
            {
                this.comboBoxSearchStatistics.Items.Add(s);
            }
            this.comboBoxSearchStatistics.SelectedIndex = 0;
            this.comboBoxSearchStatistics.SelectedItem = this.comboBoxSearchStatistics.Items[0];
        }

        private void initSearchPrometheus()
        {
            this.textBoxSearchIP.Text = Settings.Default.PrometheusServer;
            this.textBoxSearchPort.Text = Settings.Default.PrometheusPort.ToString();
        }

        public void initForSearch()
        {
            this.Height = 200;
            this.Width = 300;

            this.tabControl.Visible = true;

            this.tabControl.TabPages.Remove(this.tabPageConfiguration);
            this.tabControl.TabPages.Remove(this.tabPageStarting);
            this.tabControl.TabPages.Remove(this.tabPageAlerting);
            this.tabControl.TabPages.Remove(this.tabPageImport);

            if (!this.tabControl.TabPages.Contains(this.tabPageSearch))
                this.tabControl.TabPages.Add(this.tabPageSearch);

            this.initSearchStatistics();
            //foreach (string s in this.ImportStatistics())
            //{
            //    this.comboBoxSearchStatistics.Items.Add(s);
            //}
            //this.comboBoxSearchStatistics.SelectedIndex = 0;
            //this.comboBoxSearchStatistics.SelectedItem = this.comboBoxSearchStatistics.Items[0];
            this.initSearchPrometheus();
            //this.textBoxSearchIP.Text = Settings.Default.PrometheusServer;
            //this.textBoxSearchPort.Text = Settings.Default.PrometheusPort.ToString();
        }


        private void textBoxSearchIP_Leave(object sender, EventArgs e)
        {
            if (Prometheus.IsValidIP(this.textBoxSearchIP.Text) || (this.textBoxSearchIP.Text.StartsWith("http") && this.textBoxSearchIP.Text.IndexOf("://") > -1 && this.textBoxSearchIP.Text.IndexOf(".") > -1))
            {
                Settings.Default.PrometheusServer = this.textBoxSearchIP.Text;
                Settings.Default.Save();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(this.textBoxSearchIP.Text + " is not a valid server or IP-Adress");
                this.textBoxSearchIP.Text = " . . . ";
            }
            this.initSearchPrometheusSensors();
        }

        private void textBoxSearchPort_Leave(object sender, EventArgs e)
        {
            if (Prometheus.IsValidPort(this.textBoxSearchPort.Text))
            {
                Settings.Default.PrometheusPort = int.Parse(this.textBoxSearchPort.Text);
                Settings.Default.Save();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(this.textBoxSearchPort.Text + " is not a valid Port");
                this.textBoxSearchPort.Text = Settings.Default.PrometheusPort.ToString();
            }
            this.initSearchPrometheusSensors();
        }

        private void initSearchPrometheusJobs()
        {
            string Error = "";
            if (Settings.Default.PrometheusServer.Length > 0 && Settings.Default.PrometheusPort.ToString().Length > 0 && Prometheus.IsValidLink(Settings.Default.PrometheusServer + ":" + Settings.Default.PrometheusPort.ToString(), ref Error))
            {
                this.comboBoxSearchSensor.Items.Clear();
                foreach (string S in Prometheus.Jobs())
                    this.comboBoxSearchSensor.Items.Add(S);
                this.tableLayoutPanelSearchSensor.Enabled = true;
            }
            else
            {
                this.tableLayoutPanelSearchSensor.Enabled = false;
            }
            if (Error.Length > 0)
            {
                System.Windows.Forms.MessageBox.Show(Error);
            }
        }

        private void initSearchPrometheusSensors()
        {
            string Error = "";
            if (Settings.Default.PrometheusServer.Length > 0 && Settings.Default.PrometheusPort.ToString().Length > 0 && Prometheus.IsValidLink(Settings.Default.PrometheusServer + ":" + Settings.Default.PrometheusPort.ToString(), ref Error))
            {
                this.comboBoxSearchSensor.Items.Clear();
                foreach (string S in Prometheus.Sensors(this.comboBoxSearchFilter.Text))
                    this.comboBoxSearchSensor.Items.Add(S);
                //foreach (System.Collections.Generic.KeyValuePair<string, PrometheusMetric> KV in Prometheus.PrometheusMetrics(this.comboBoxSearchFilter.Text))
                //    this.comboBoxSearchSensor.Items.Add(KV.Value.DisplayText);
                this.tableLayoutPanelSearchSensor.Enabled = true;
                this.SetFilter();
                //this.comboBoxSearchFilter.Items.Clear();
                //foreach (string D in Prometheus.Domains()) this.comboBoxSearchFilter.Items.Add(D);
            }
            else
            {
                this.tableLayoutPanelSearchSensor.Enabled = false;
            }
            if (Error.Length > 0)
            {
                System.Windows.Forms.MessageBox.Show(Error);
            }
        }

        private void SetFilter()
        {
            string Filter = this.comboBoxSearchFilter.Text;
            int i = 0;
            this.comboBoxSearchFilter.Items.Clear();
            foreach (string D in Prometheus.Domains())
            {
                this.comboBoxSearchFilter.Items.Add(D);
                if (D == Filter)
                    i = this.comboBoxSearchFilter.Items.Count - 1;
            }
            this.comboBoxSearchFilter.SelectedIndex = i;
        }

        private void comboBoxSearchJob_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.comboBoxSearchMetric.Items.Clear();
            foreach (string m in Prometheus.Metrics(this.comboBoxSearchSensor.SelectedItem.ToString()))
                this.comboBoxSearchMetric.Items.Add(m);
        }

        private void comboBoxSearchSensor_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.comboBoxSearchMetric.Items.Clear();
            foreach(string m in Prometheus.SensorMetrcList(this.comboBoxSearchFilter.Text, this.comboBoxSearchSensor.SelectedItem.ToString()))
            {
                this.comboBoxSearchMetric.Items.Add(m);
            }
            //foreach (string m in Prometheus.Metrics(this.comboBoxSearchSensor.SelectedItem.ToString()))
            //    this.comboBoxSearchMetric.Items.Add(m);
        }

        private void comboBoxSearchMetric_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.buttonSearchOK.Enabled = true;
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            this.initSearchPrometheusSensors();
        }

        private void buttonSearchOK_Click(object sender, EventArgs e)
        {
            if (this.comboBoxSearchStatistics.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select the statitics");
                return;
            }
            if (this.comboBoxSearchMetric.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a metric");
                return;
            }
            string Metric = this.comboBoxSearchFilter.Text + "_" + this.comboBoxSearchSensor.SelectedItem.ToString() + "_" + this.comboBoxSearchMetric.SelectedItem.ToString();
            _Api = Settings.Default.PrometheusServer + ":" + Settings.Default.PrometheusPort.ToString() + "/api/v1/query_range?query="+ Metric + "&" + this.comboBoxSearchStatistics.SelectedItem.ToString() + "(" + this.comboBoxSearchMetric.SelectedItem.ToString() + ")";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private string _Api;
        public string Api()
        {
            return this._Api;
        }

        #endregion

        #region Import

        #region Control events

        private void buttonImportSearchSource_Click(object sender, EventArgs e)
        {
            State state = State.Search;
#if DEBUG
            state = State.GetSensor;
#endif
            FormPrometheus f = new FormPrometheus(state);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                Prometheus.PrometheusApisAdd(f.Api());
                this.initImportPrometheusApiSource();
            }
        }

        private void buttonImportPrometheusApiLinkRemove_Click(object sender, EventArgs e)
        {
            Prometheus.PrometheusApisRemove(this.comboBoxImportPrometheusApiLinks.SelectedItem.ToString());
            this.initImportPrometheusApiSource();
            this.ImportSetSource();
        }

        private void comboBoxImportPrometheusApiLinks_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.ImportSetSource();
        }

        private void buttonImportReadFromPrometheus_Click(object sender, EventArgs e)
        {
            if (this._ImportSource.Length == 0)
                return;
            if (this.comboBoxImportRangeUnit.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a unit");
                return;
            }
            System.DateTime End = System.DateTime.Now;
            System.DateTime Start = this.dateTimePickerImportStart.Value;
            if (this.radioButtonImportDuration.Checked)
            {
                System.TimeSpan timeSpan = new TimeSpan();
                string Duration = this.ImportUnits()[this.comboBoxImportRangeUnit.SelectedItem.ToString()];
                int Intervall = (int)this.numericUpDownImportRange.Value;
                switch (Duration)
                {
                    case "y":
                        timeSpan = new TimeSpan(Intervall * 365, 0, 0, 0);
                        break;
                    case "w":
                        timeSpan = new TimeSpan(Intervall * 7, 0, 0, 0);
                        break;
                    case "d":
                        timeSpan = new TimeSpan(Intervall, 0, 0, 0);
                        break;
                    case "h":
                        timeSpan = new TimeSpan(Intervall, 0, 0);
                        break;
                }
                Start = System.DateTime.Now.Subtract(timeSpan);
            }

            //string Query = this._ImportSource + "[" + this.numericUpDownImportRange.Value.ToString() + this.ImportUnits()[this.comboBoxImportRangeUnit.SelectedItem.ToString()] + ":" + this.numericUpDownImportResolution.Value.ToString() + this.ImportUnits()[this.comboBoxImportResolutionUnit.SelectedItem.ToString()] + "]";

            string Query = this._ImportSource + "&start=" + Start.ToString("yyyy-MM-dd") + "T00:00:00.000Z&end=" + End.ToString("yyyy-MM-dd") + "T23:59:59.999Z&step=" + this.numericUpDownImportResolution.Value.ToString() + this.ImportUnits()[this.comboBoxImportResolutionUnit.SelectedItem.ToString()];

            //System.Collections.Generic.Dictionary<int, float> Values = Tasks.Prometheus.ReadMetricFromJson(Query);// this.ReadJson(Query);

            System.Collections.Generic.Dictionary<string, float> Values = Tasks.Prometheus.ReadMetricFromJson(Query, (int)this.numericUpDownImportRounding.Value);// this.ReadJson(Query);

            if (_DtImport == null)
            {
                _DtImport = new DataTable();
                System.Data.DataColumn dataColumnDate = new DataColumn("Date", System.Type.GetType("System.String"));
                _DtImport.Columns.Add(dataColumnDate);
                string Value = this._Unit;
                if (Value.Length == 0)
                    Value = "Value";
                //_DtImport.Columns.Add(new DataColumn(Value, System.Type.GetType("System.Double")));
                _DtImport.Columns.Add(new DataColumn(Value, System.Type.GetType("System.Single")));
                this.dataGridViewImport.Columns.Clear();
            }
            else
                _DtImport.Clear();
            System.DateTime dateTimeStart = System.DateTime.Now;
            foreach(System.Collections.Generic.KeyValuePair<string, float> KV in Values)
            {
                System.Data.DataRow R = _DtImport.NewRow();
                R[0] = KV.Key;// dateTime.ToString("yyyy-MM-dd HH:mm:ss");
                R[1] = KV.Value;
                _DtImport.Rows.Add(R);
            }
            this.dataGridViewImport.DataSource = _DtImport;
            this.setImportPrometheusURI();
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            try
            {
                int Count = 0;
                if (_DtImport != null && _DtImport.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow R in this._DtImport.Rows)
                    {
                        string SQL = "INSERT INTO CollectionTaskMetric (CollectionTaskID, MetricDate, MetricValue) " +
                        "SELECT " + this._CollectionTaskID.ToString() + ", CONVERT(DATETIME, '" + R[0].ToString() + "', 102), " + R[1].ToString() + " " +
                        "WHERE CONVERT(DATETIME, '" + R[0].ToString() + "', 102) > (SELECT MAX(MetricDate) FROM CollectionTaskMetric WHERE CollectionTaskID = " + this._CollectionTaskID.ToString() + ") " +
                        " OR (SELECT COUNT(*) FROM CollectionTaskMetric WHERE CollectionTaskID = " + this._CollectionTaskID.ToString() + ") = 0";
                        if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                            Count++;
                        //System.DateTime dateTime;
                        //if (DateTime.TryParse(R[0].ToString(), out dateTime))
                        //{
                        //    string SQL = "INSERT INTO CollectionTaskMetric (CollectionTaskID, MetricDate, MetricValue) " +
                        //    "SELECT " + this._CollectionTaskID.ToString() + ", CONVERT(DATETIME, '" + dateTime.ToString("yyyy-MM-dd HH:mm:ss") + "', 102), " + R[1].ToString() + " " +
                        //    "WHERE CAST('" + R[0].ToString() + "' as datetime) > (SELECT MAX(MetricDate) FROM CollectionTaskMetric WHERE CollectionTaskID = " + this._CollectionTaskID.ToString() + ") " +
                        //    " OR (SELECT COUNT(*) FROM CollectionTaskMetric WHERE CollectionTaskID = " + this._CollectionTaskID.ToString() + ") = 0";
                        //    if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                        //        Count++;
                        //}
                    }
                }
                System.Windows.Forms.MessageBox.Show(Count.ToString() + " values imported");
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void textBoxImportSource_TextChanged(object sender, EventArgs e)
        {
            if (this.ImportValidSource(this.textBoxImportSource.Text))
            {
                this._ImportSource = this.textBoxImportSource.Text;
                this.tableLayoutPanelImport.Enabled = true;
                this.textBoxImportSource.BackColor = System.Drawing.Color.White;
                this.setImportPrometheusURI();
            }
            else
            {
                this.textBoxImportSource.BackColor = System.Drawing.Color.Pink;
                this.tableLayoutPanelImport.Enabled = false;
            }
        }

        #endregion

        private void initForImport()
        {
            this.Height = 500;
            this.Width = 500;
            try
            {
                this.tabControl.TabPages.Remove(this.tabPageConfiguration);
                this.tabControl.TabPages.Remove(this.tabPageStarting);
                this.tabControl.TabPages.Remove(this.tabPageAlerting);
                this.tabControl.TabPages.Remove(this.tabPageSearch);

                this.initImportUnits();
                //foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.ImportUnits())
                //{
                //    this.comboBoxImportRangeUnit.Items.Add(KV.Key);
                //    this.comboBoxImportResolutionUnit.Items.Add(KV.Key);
                //}
                //this.comboBoxImportRangeUnit.SelectedIndex = 3;
                //this.comboBoxImportResolutionUnit.SelectedIndex = 1;
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }


        private void ImportSetSource()
        {
            try
            {
                if (comboBoxImportPrometheusApiLinks.SelectedItem == null && comboBoxImportPrometheusApiLinks.Items.Count > 0)
                    comboBoxImportPrometheusApiLinks.SelectedIndex = 0;
                if (comboBoxImportPrometheusApiLinks.SelectedItem != null && this.ImportValidSource(comboBoxImportPrometheusApiLinks.SelectedItem.ToString()))
                {
                    this._ImportSource = comboBoxImportPrometheusApiLinks.SelectedItem.ToString();
                    this.tableLayoutPanelImport.Enabled = true;
                    this.comboBoxImportPrometheusApiLinks.BackColor = System.Drawing.Color.White;
                    this.setImportPrometheusURI();
                }
                else
                {
                    this.comboBoxImportPrometheusApiLinks.BackColor = System.Drawing.Color.Pink;
                    this.tableLayoutPanelImport.Enabled = false;
                }
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }


        private void initImportPrometheusApiSource()
        {
            this.comboBoxImportPrometheusApiLinks.Items.Clear();
            foreach (string A in Prometheus.PrometheusApis())
            {
                if (A != null)
                    this.comboBoxImportPrometheusApiLinks.Items.Add(A);
            }
            this.ImportSetSource();
        }

        private void initImportUnits()
        {
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.ImportUnits())
            {
                this.comboBoxImportRangeUnit.Items.Add(KV.Key);
                this.comboBoxImportResolutionUnit.Items.Add(KV.Key);
            }
            this.comboBoxImportRangeUnit.SelectedIndex = 3;
            this.comboBoxImportResolutionUnit.SelectedIndex = 1;
        }

        private System.Collections.Generic.Dictionary<string, string> _ImportUnits;
        private System.Collections.Generic.Dictionary<string, string> ImportUnits()
        {
            if (_ImportUnits == null)
            {
                _ImportUnits = new Dictionary<string, string>();
                _ImportUnits.Add("hours", "h");
                _ImportUnits.Add("days", "d");
                _ImportUnits.Add("weeks", "w");
                _ImportUnits.Add("years", "y");
            }
            return _ImportUnits;
        }

        private System.Collections.Generic.List<string> _ImportStatistics;
        private System.Collections.Generic.List<string> ImportStatistics()
        {
            if (_ImportStatistics == null)
            {
                _ImportStatistics = new List<string>();
                _ImportStatistics.Add("avg");
                _ImportStatistics.Add("max");
                _ImportStatistics.Add("min");
                _ImportStatistics.Add("sum");
            }
            return _ImportStatistics;
        }

        private System.Data.DataTable _DtImport;

        private bool ImportValidSource(string Source)
        {
            bool OK = false;
            if (Source.Length > 0 && Source.LastIndexOf(":") > 6)
            {
                string[] sColon = Source.Split(new char[] { ':' });
                string[] sSlash = sColon[1].Split(new char[] { '/' });
                string sPort = sSlash[0];
                int Port;
                if (int.TryParse(sPort, out Port))
                {
                    if (Port > 1023 && Port < 49152)
                        OK = true;
                }
            }
            return OK;
        }

        private void setImportPrometheusURI()
        {
            if (this._ImportSource.Length > 0 && this._ImportSource.LastIndexOf(":") > 6)
            {
                try
                {
                    string URI = this._ImportSource;
                    if (URI.IndexOf("/api/v1") > -1)
                    {
                        URI = this._ImportSource.Substring(0, _ImportSource.IndexOf("/api/v1"));
                    }
                    if (_ImportSource.IndexOf('(') > -1)
                    {
                        URI += "/graph?g0.expr=";
                        URI += this._ImportSource.Substring(_ImportSource.IndexOf("(") + 1);
                        string Range = "1"; 
                        string Unit = "d";
                        if(this.comboBoxImportRangeUnit.SelectedItem != null)
                        {
                            Range = this.numericUpDownImportRange.Value.ToString();
                            Unit = this.ImportUnits()[this.comboBoxImportRangeUnit.SelectedItem.ToString()];
                        }
                        URI = "http://" + URI.Substring(0, URI.IndexOf(")")) + "&g0.range_input=" + Range + Unit;
                    }
                    System.Uri u = new Uri(URI);
                    this.webView2Prometheus.Source = u;
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please enter the server and the port\r\n<server>:<port>");
            }
        }

        //private System.Collections.Generic.Dictionary<int, float> ReadJson(string URI)
        //{
        //    System.Collections.Generic.Dictionary<int, float> Values = new Dictionary<int, float>();
        //    System.Object[] TT = null;
        //    try
        //    {
        //        string URL = URI;
        //        System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(URL);
        //        System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();

        //        string Response = "";
        //        System.IO.Stream receiveStream = response.GetResponseStream();
        //        Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
        //        System.IO.StreamReader readStream = new System.IO.StreamReader(receiveStream, encode);
        //        Char[] read = new Char[256];
        //        int count = readStream.Read(read, 0, 256);
        //        while (count > 0)
        //        {
        //            // Dumps the 256 characters on a string and displays the string to the console.
        //            String str = new String(read, 0, count);
        //            Response += str;
        //            count = readStream.Read(read, 0, 256);
        //        }
        //        System.Web.Script.Serialization.JavaScriptSerializer JSS = new System.Web.Script.Serialization.JavaScriptSerializer();
        //        System.Object O = JSS.DeserializeObject(Response);
        //        if (O.GetType() == typeof(System.Collections.Generic.Dictionary<string, Object>))
        //        {
        //            System.Collections.Generic.Dictionary<string, Object> Results = (System.Collections.Generic.Dictionary<string, Object>)O;
        //            if (Results.ContainsKey("data"))
        //            {
        //                if (Results["data"].GetType() == typeof(System.Collections.Generic.Dictionary<System.String, System.Object>))
        //                {
        //                    System.Collections.Generic.Dictionary<System.String, System.Object> RR = (System.Collections.Generic.Dictionary<System.String, System.Object>)Results["data"];
        //                    if (RR.ContainsKey("result"))
        //                    {
        //                        System.Object[] rr = (System.Object[])RR["result"];
        //                        foreach(System.Object o in rr)
        //                        { 
        //                            if (o.GetType() == typeof(System.Collections.Generic.Dictionary<System.String, System.Object>))
        //                            {
        //                                System.Collections.Generic.Dictionary<System.String, System.Object> oo = (System.Collections.Generic.Dictionary<System.String, System.Object>)o;
        //                                if (oo.ContainsKey("values"))
        //                                {
        //                                    System.Object[] vv = (System.Object[])oo["values"];
        //                                    for(int i = 0; i < vv.Length; i++)
        //                                    {
        //                                        System.Object[] v = (System.Object[])vv[i];
        //                                        Values.Add(i, float.Parse(v[1].ToString()));
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    return Values;
        //}

        private void buttonSearchSource_Click(object sender, EventArgs e)
        {
            FormPrometheus f = new FormPrometheus(State.Search);
            //FormPrometheus f = new FormPrometheus();
            //f.initForSearch();
            //f.initPrometheusSearch();
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                Prometheus.PrometheusApisAdd(f.Api());
                this.initImportPrometheusApiSource();
                //this.textBoxImportSource.Text = f.Api();
                //string SQL = "UPDATE C SET MetricSource = '" + f.Api() + "' FROM CollectionTask AS C WHERE CollectionTaskID = " + _CollectionTaskID.ToString();
                //bool OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            }
        }

#endregion

#region Interface

        public string Souce() { return this._ImportSource; }

#endregion

#region YAML
        private void buttonYamlOpen_Click(object sender, EventArgs e)
        {
            this.openFileDialogYaml = new OpenFileDialog();
            this.openFileDialogYaml.RestoreDirectory = true;
            this.openFileDialogYaml.Multiselect = false;
            if (this.textBoxYamlFile.Text.Length > 0)
            {
                System.IO.FileInfo FI = new System.IO.FileInfo(this.textBoxYamlFile.Text);
                this.openFileDialogYaml.InitialDirectory = FI.DirectoryName;
            }
            //else if (Settings.Default.PrometheusApiLink.Length > 0)
            //{
            //    System.IO.DirectoryInfo D = new System.IO.DirectoryInfo(Settings.Default.PrometheusApiLink);
            //    this.openFileDialogYaml.InitialDirectory = D.FullName;
            //}
            this.openFileDialogYaml.Filter = "Yaml Files|*.yml";
            try
            {
                this.openFileDialogYaml.ShowDialog();
                if (this.openFileDialogYaml.FileName.Length > 0)
                {
                    System.IO.FileInfo f = new System.IO.FileInfo(this.openFileDialogYaml.FileName);
                    this.textBoxYamlFile.Text = f.FullName;
                    string Directory = f.DirectoryName;
                    //if (Settings.Default.PrometheusApiLink != Directory)
                    //{
                    //    Settings.Default.PrometheusApiLink = Directory;
                    //    Settings.Default.Save();
                    //}
                    this.SetYamlFile();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void SetYamlFile()
        {

        }

        private void buttonYamlSave_Click(object sender, EventArgs e)
        {

        }

        #endregion

        //#region PrometheusApis

        //private System.Collections.Generic.List<string> _PrometheusApis;

        //private System.Collections.Generic.List<string> PrometheusApis()
        //{
        //    if (DiversityCollection.Tasks.Settings.Default.PrometheusApiLinks == null)
        //        DiversityCollection.Tasks.Settings.Default.PrometheusApiLinks = new System.Collections.Specialized.StringCollection();
        //    if (this._PrometheusApis == null)
        //    {
        //        _PrometheusApis = new List<string>();
        //        foreach(string A in DiversityCollection.Tasks.Settings.Default.PrometheusApiLinks)
        //        {
        //            _PrometheusApis.Add(A);
        //        }
        //        _PrometheusApis.Sort();
        //    }
        //    return _PrometheusApis;
        //}

        //private bool PrometheusApisAdd(string Api)
        //{
        //    bool OK = false;
        //    try
        //    {
        //        if (!PrometheusApis().Contains(Api))
        //        {
        //            _PrometheusApis.Add(Api);
        //            _PrometheusApis.Sort();
        //            OK = true;
        //        }
        //    }
        //    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        //    return OK;
        //}

        //private bool PrometheusApisRemove(string Api)
        //{
        //    bool OK = false;
        //    try
        //    {
        //        if (PrometheusApis().Contains(Api))
        //        {
        //            _PrometheusApis.Remove(Api);
        //            _PrometheusApis.Sort();
        //            OK = true;
        //        }
        //    }
        //    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        //    return OK;
        //}


        //private bool PrometheusApisSave()
        //{
        //    bool OK = false;
        //    try
        //    {
        //        DiversityCollection.Tasks.Settings.Default.PrometheusApiLinks.Clear();
        //        _PrometheusApis.Sort();
        //        foreach (string A in this.PrometheusApis())
        //            DiversityCollection.Tasks.Settings.Default.PrometheusApiLinks.Add(A);
        //        DiversityCollection.Tasks.Settings.Default.Save();
        //        OK = true;
        //    }
        //    catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        //    return OK;
        //}

        //#endregion

        #region Alerting

        private void initAlerting()
        {

        }


        private void buttonAlertAdd_Click(object sender, EventArgs e)
        {

        }

        private void buttonAlertDelete_Click(object sender, EventArgs e)
        {

        }

        #endregion

    }
}
