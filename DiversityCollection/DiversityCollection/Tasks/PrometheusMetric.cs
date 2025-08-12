using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityCollection.Tasks
{
    public class PrometheusMetric
    {
        #region Properties

        private string _PrometheusIdentifier;
        private string _Server;
        private string _Domain;
        private string _Sensor;
        private string _Metric;
        private string _Units;
        private string _Statistics;

        #endregion

        #region Construction

        public PrometheusMetric(string PrometheusIdentifier)
        {
            _Server = Prometheus.PrometheusServer;
            _PrometheusIdentifier = PrometheusIdentifier;
            string[] vv = PrometheusIdentifier.Split(new char[] { '_' });
            this._Domain = vv[0];
            if (vv.Length > 1)
            {
                this._Sensor = vv[1];
                if (vv.Length > 2)
                {
                    this._Metric = vv[2];
                    if (vv.Length > 3)
                        this._Units = vv[3];
                }
            }
        }

        public PrometheusMetric(string Server, string Domain, string Sensor, string Metric)
        {
            this._Server = Server;
            this._Domain = Domain;
            this._Sensor = Sensor;
            this._Metric = Metric;
        }

        public PrometheusMetric(PrometheusMetric prometheusMetric)
        {
            this._Server = prometheusMetric._Server;
            this._Domain = prometheusMetric._Domain;
            this._Sensor = prometheusMetric._Sensor;
            this._Metric = prometheusMetric._Metric;
            this._Units = prometheusMetric._Units;
        }

        #endregion

        #region Interface

        public void setStatistics(string Stat)
        {
            if (Prometheus.Statics().Contains(Stat))
                this._Statistics = Stat;
        }

        public string Statistics { get { return _Statistics; } }

        public string Metric { get { return _Metric + "_" + _Units; } }


        public string MetricIdentifier { get { return Metric; } }

        public string API
        {
            get
            {
                if (_Server.Length > 0 && _Domain.Length > 0 && _Sensor.Length > 0 && _Metric.Length > 0 && _Units.Length > 0 && _Statistics.Length > 0)
                    return this._Server + "/api/v1/query_range?query=" + this._Domain + "_" + this._Sensor + "_" + this._Metric + "_" + this._Units + "&" + this._Statistics + "(" + this._Metric + "_" + this._Units + ")";
                else return "";
            }
        }

        public string PrometheusIdentifier { get { return _PrometheusIdentifier; } }

        public string DisplayText
        {
            get
            {
                if (_Server.Length > 0 && _Domain.Length > 0 && _Sensor.Length > 0 && _Metric.Length > 0 && _Units.Length > 0)
                    return this._Domain + " " + this.SensorDisplayText + " " + this.MetricDisplayText + " " + this.MetricUnitsDisplayText;
                return "";
            }
        }

        public bool IsValid
        {
            get
            {
                return 
                    _Server != null &&_Server.Length > 0 && 
                    _Domain != null && _Domain.Length > 0 &&
                    _Sensor != null && _Sensor.Length > 0 &&
                    _Metric != null && _Metric.Length > 0 &&
                    _Units != null && _Units.Length > 0;
            }
        }

        public string Server { get { return this._Server; } }

        public string Domain { get { return this._Domain; } }

        public string SensorDisplayText
        {
            get
            {
                string DisplayText = _Sensor;
                if (DisplayText.Length == 12)
                {
                    bool IsMac = true;
                    foreach (Char c in DisplayText.ToUpper())
                    {
                        if (c != '0' && c != '1' && c != '2' && c != '3' && c != '4' && c != '5' && c != '6' && c != '7' && c != '8' && c != '9' && c != 'A' && c != 'B' && c != 'C' && c != 'D' && c != 'E' && c != 'F')
                        {
                            IsMac = false;
                            break;
                        }
                    }
                    if (IsMac)
                    {
                        DisplayText = DisplayText.ToUpper();
                        string MAC = "";
                        for (int i = 0; i < DisplayText.Length; i++)
                        {
                            MAC += DisplayText[i];
                            if ((i + 1) % 2 == 0 && i < DisplayText.Length - 1)
                                MAC += ":";
                        }
                        DisplayText = MAC;
                    }
                }
                return DisplayText;
            }
        }

        public string MetricDisplayText
        {
            get
            {
                string DisplayText = "";
                if (_Metric != null)
                {
                    DisplayText = _Metric;
                    DisplayText = DisplayText.Substring(0, 1).ToUpper() + DisplayText.Substring(1) + " " + MetricUnitsDisplayText;
                }
                return DisplayText;
            }
        }

        public enum MetricType { Battery, Humidity, Temperature, None }

        public MetricType Type
        {
            get
            {
                MetricType metricType = MetricType.None;
                if (_Metric != null)
                {
                    if (_Metric.ToLower().IndexOf("temperatur") > -1)
                        metricType = MetricType.Temperature;
                    else if (_Metric.ToLower().IndexOf("humidity") > -1)
                        metricType = MetricType.Humidity;
                    else if (_Metric.ToLower().IndexOf("battery") > -1)
                        metricType = MetricType.Battery;
                }
                return metricType;

            }
        }


        public string MetricUnitsDisplayText
        {
            get
            {
                string DisplayText = _Units;
                switch(_Units.ToLower())
                {
                    case "ratio":
                        DisplayText = "%";
                        break;
                    case "celsius":
                        DisplayText = "°C";
                        break;
                    case "volts":
                        DisplayText = "V";
                        break;
                }
                return DisplayText;
            }
        }

        #endregion

    }
}
