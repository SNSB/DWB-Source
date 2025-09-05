using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityCollection.Tasks
{
    public class Prometheus
    {

        #region Checking valid

        public static bool IsValidIP(string IP)
        {
            System.Text.RegularExpressions.Regex ip = new System.Text.RegularExpressions.Regex("^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$");
            return ip.IsMatch(IP);
        }

        public static bool IsValidPort(string Port)
        {
            int i;
            return int.TryParse(Port, out i) && IsValidPort(i);
        }

        public static bool IsValidPort(int Port)
        {
            return Port >= 1024 && Port <= 65535;
        }

        public static bool IsValidLink(string Link, ref string Error)
        {
            bool IsValid = false;
            Link += "/api/v1/label/job/values";
            System.Collections.Generic.Dictionary<int, string> Dict = ReadDataFromJson(Link, ref Error, true);
            if (Dict.Count > 0)
                IsValid = true;
            return IsValid;
        }

        #endregion

        #region Jobs - veraltet

        public static System.Collections.Generic.List<string> Jobs()
        {
            System.Collections.Generic.List<string> list = new List<string>();
            string Error = "";
            string URI = PrometheusServer + "/api/v1/label/job/values";
            System.Collections.Generic.Dictionary<int, string> dict = ReadDataFromJson(URI, ref Error);
            foreach(System.Collections.Generic.KeyValuePair<int, string> KV in dict)
            {
                if (KV.Value != "prometheus")
                    list.Add(KV.Value);
            }
            return list;
        }

        public static System.Collections.Generic.List<string> Metrics(string Job)
        {
            System.Collections.Generic.List<string> list = new List<string>();
            Job = System.Net.WebUtility.UrlEncode("\"" + Job + "\"");
            string URL = Settings.Default.PrometheusServer + ":" + Settings.Default.PrometheusPort.ToString() + "/api/v1/series?match[]={job=" + Job + "}";
            string Error = "";
            System.Collections.Generic.Dictionary<int, string> dict = ReadDataFromJson(URL, ref Error, false, "metric");
            foreach (System.Collections.Generic.KeyValuePair<int, string> KV in dict)
            {
                if (KV.Value != "prometheus")
                    list.Add(KV.Value);
            }
            return list;
        }

        #endregion

        #region Links in Settings
        public static void AddLink(string IP, int Port)
        {
            if (IsValidIP(IP) && IsValidPort(Port))
            {
                string PrometheusApiLink = IP + ":" + Port.ToString();// + "/api/v1/";
                string Error = "";
                if (IsValidLink(PrometheusApiLink, ref Error))
                    AddLink(PrometheusApiLink);
            }
        }

        private static void AddLink(string Link)
        {
            if (Settings.Default.PrometheusApiLinks == null)
                Settings.Default.PrometheusApiLinks = new System.Collections.Specialized.StringCollection();
            if (!Settings.Default.PrometheusApiLinks.Contains(Link))
                Settings.Default.PrometheusApiLinks.Add(Link);
        }

        #endregion

        #region SensorMetric

        public static System.Collections.Generic.List<string> Sensors(string DomainFilter, string SensorFilter = "")
        {
            string URI = PrometheusServer + "/api/v1/label/__name__/values";
            getSensorMetric(URI, DomainFilter, SensorFilter);
            return SensorList(DomainFilter);
        }

        private static System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>>> _SensorMetric;

        private static System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>>> getSensorMetric(string URI, string DomainFilter, string SensorFilter = "")
        {
            string Error = "";
            try
            {
                System.Collections.Generic.Dictionary<int, string> dict = ReadDataFromJson(URI, ref Error);
                foreach (System.Collections.Generic.KeyValuePair<int, string> KV in dict)
                {
                    if (KV.Value != null)
                    {
                        string[] vv = KV.Value.Split(new char[] { '_' });
                        if (DomainFilter.Length > 0 && vv[0] != DomainFilter) continue;
                        if (vv.Length > 3)
                        {
                            string Server = PrometheusServer;
                            string Domain = vv[0];
                            string Sensor = vv[1];
                            if (SensorFilter.Length > 0 && Sensor != SensorFilter) continue;
                            string Metric = vv[2];
                            if (vv.Length > 2)
                            {
                                for (int i = 3; i < vv.Length; i++)
                                {
                                    Metric += "_" + vv[i];
                                }
                            }
                            if (_SensorMetric == null)
                                _SensorMetric = new System.Collections.Generic.Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>();

                            if (_SensorMetric.ContainsKey(Server))
                            {
                                if (_SensorMetric[Server].ContainsKey(Domain))
                                {
                                    if (_SensorMetric[Server][Domain].ContainsKey(Sensor))
                                    {
                                        if (!_SensorMetric[Server][Domain][Sensor].Contains(Metric))
                                        {
                                            _SensorMetric[Server][Domain][Sensor].Add(Metric);
                                        }
                                    }
                                    else // No Sensor
                                    {
                                        System.Collections.Generic.List<string> ll = new List<string>();
                                        ll.Add(Metric);
                                        _SensorMetric[Server][Domain].Add(Sensor, ll);
                                    }
                                }
                                else // No Domain
                                {
                                    System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> ss = new Dictionary<string, List<string>>();
                                    System.Collections.Generic.List<string> ll = new List<string>();
                                    ll.Add(Metric);
                                    ss.Add(Sensor, ll);
                                    _SensorMetric[Server].Add(Domain, ss);
                                }
                            }
                            else // No Server
                            {
                                Dictionary<string, Dictionary<string, List<string>>> dd = new Dictionary<string, Dictionary<string, List<string>>>();
                                System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> ss = new Dictionary<string, List<string>>();
                                System.Collections.Generic.List<string> ll = new List<string>();
                                ll.Add(Metric);
                                ss.Add(Sensor, ll);
                                dd.Add(Domain, ss);
                                _SensorMetric.Add(Server, dd);
                            }
                        }
                    }
                }
            }
            catch(System.Exception ex){ DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return _SensorMetric;
        }

        public static System.Collections.Generic.List<string> Domains()
        {
            System.Collections.Generic.List<string> dd = new List<string>();
            if (_SensorMetric != null)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>>> KV in _SensorMetric)
                {
                    if (KV.Key == PrometheusServer)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>> kv in KV.Value)
                        {
                            dd.Add(kv.Key);
                        }
                    }
                }
            }
            dd.Sort();
            return dd;
        }

        public static System.Collections.Generic.List<string> SensorList(string Domain)
        {
            System.Collections.Generic.List<string> list = new List<string>();
            if (Domain.Length > 0)
            {
                if (_SensorMetric.ContainsKey(PrometheusServer) && _SensorMetric[PrometheusServer].ContainsKey(Domain))
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<string>> KV in _SensorMetric[PrometheusServer][Domain])
                    {
                        list.Add(KV.Key);
                    }
                }
            }
            list.Sort();
            return list;
        }

        public static System.Collections.Generic.List<string> SensorMetrcList(string Domain, string Sensor)
        {
            System.Collections.Generic.List<string> list = new List<string>();
            if (_SensorMetric.ContainsKey(PrometheusServer))
            {
                if (_SensorMetric[PrometheusServer].ContainsKey(Domain))
                {
                    if (_SensorMetric[PrometheusServer][Domain].ContainsKey(Sensor))
                    {
                        foreach (string m in _SensorMetric[PrometheusServer][Domain][Sensor])
                        {
                            list.Add(m);
                        }
                    }
                }
            }
            list.Sort();
            return list;
        }

        #endregion

        #region PrometheusMetric

        private static string _Domain;

        private static string Domain { get { return _Domain; } }

        public static void Init(string Domain, string Sensor = "")
        {
            _Domain = Domain;
            string URI = PrometheusServer + "/api/v1/label/__name__/values";
            getPromentheusMetrics(URI, Domain);
        }

        public static System.Collections.Generic.List<string> PrometheusSensors(string Domain)
        {
            System.Collections.Generic.List<string> list = new List<string>();
            if (Domain.Length > 0)
            {
                if (_PrometheusMetrics.ContainsKey(PrometheusServer) && _PrometheusMetrics[PrometheusServer].ContainsKey(Domain))
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.SortedDictionary<string, PrometheusMetric>> KV in _PrometheusMetrics[PrometheusServer][Domain])
                    {
                        list.Add(KV.Key);
                    }
                }
            }
            list.Sort();
            return list;
        }

        public static System.Collections.Generic.List<string> DomainList()
        {
            System.Collections.Generic.List<string> dd = new List<string>();
            if (_PrometheusMetrics != null)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.SortedDictionary<string, PrometheusMetric>>>> KV in _PrometheusMetrics)
                {
                    if (KV.Key == PrometheusServer)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.SortedDictionary<string, PrometheusMetric>>> kv in KV.Value)
                        {
                            dd.Add(kv.Key);
                        }
                    }
                }
            }
            dd.Sort();
            return dd;
        }

        private static System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.SortedDictionary<string, PrometheusMetric>>>> _PrometheusMetrics;

        private static System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.SortedDictionary<string, PrometheusMetric>>>> getPromentheusMetrics(string URI, string Domain, string Sensor = "")
        {
            string Error = "";
            try
            {
                System.Collections.Generic.Dictionary<int, string> dict = ReadDataFromJson(URI, ref Error);
                foreach (System.Collections.Generic.KeyValuePair<int, string> KV in dict)
                {
                    if (KV.Value != null)
                    {
                        PrometheusMetric prometheusMetric = new PrometheusMetric(KV.Value);

                        if (!prometheusMetric.IsValid)
                            continue;

                        if (prometheusMetric.Domain != Domain)
                            continue;

                        if (_PrometheusMetrics == null)
                            _PrometheusMetrics = new System.Collections.Generic.Dictionary<string, Dictionary<string, Dictionary<string, SortedDictionary<string, PrometheusMetric>>>>();

                        if (_PrometheusMetrics.ContainsKey(prometheusMetric.Server))
                        {
                            if (_PrometheusMetrics[prometheusMetric.Server].ContainsKey(prometheusMetric.Domain))
                            {
                                if (_PrometheusMetrics[prometheusMetric.Server][prometheusMetric.Domain].ContainsKey(prometheusMetric.SensorDisplayText))
                                {
                                    if (!_PrometheusMetrics[prometheusMetric.Server][prometheusMetric.Domain][prometheusMetric.SensorDisplayText].ContainsKey(prometheusMetric.PrometheusIdentifier))
                                    {
                                        _PrometheusMetrics[prometheusMetric.Server][prometheusMetric.Domain][prometheusMetric.SensorDisplayText].Add(prometheusMetric.PrometheusIdentifier, prometheusMetric);
                                    }
                                }
                                else // No Sensor
                                {
                                    System.Collections.Generic.SortedDictionary<string, PrometheusMetric> ll = new SortedDictionary<string, PrometheusMetric>();
                                    ll.Add(prometheusMetric.PrometheusIdentifier, prometheusMetric);
                                    _PrometheusMetrics[prometheusMetric.Server][prometheusMetric.Domain].Add(prometheusMetric.SensorDisplayText, ll);
                                }
                            }
                            else // No Domain
                            {
                                System.Collections.Generic.Dictionary<string, System.Collections.Generic.SortedDictionary<string, PrometheusMetric>> ss = new Dictionary<string, SortedDictionary<string, PrometheusMetric>>();
                                System.Collections.Generic.SortedDictionary<string, PrometheusMetric> ll = new SortedDictionary<string, PrometheusMetric>();
                                ll.Add(prometheusMetric.PrometheusIdentifier, prometheusMetric);
                                ss.Add(prometheusMetric.SensorDisplayText, ll);
                                _PrometheusMetrics[prometheusMetric.Server].Add(prometheusMetric.Domain, ss);
                            }
                        }
                        else // No Server
                        {
                            Dictionary<string, Dictionary<string, SortedDictionary<string, PrometheusMetric>>> dd = new Dictionary<string, Dictionary<string, SortedDictionary<string, PrometheusMetric>>>();
                            System.Collections.Generic.Dictionary<string, System.Collections.Generic.SortedDictionary<string, PrometheusMetric>> ss = new Dictionary<string, SortedDictionary<string, PrometheusMetric>>();
                            System.Collections.Generic.SortedDictionary<string, PrometheusMetric> ll = new SortedDictionary<string, PrometheusMetric>();
                            ll.Add(prometheusMetric.PrometheusIdentifier, prometheusMetric);
                            ss.Add(prometheusMetric.SensorDisplayText, ll);
                            dd.Add(prometheusMetric.Domain, ss);
                            _PrometheusMetrics.Add(prometheusMetric.Server, dd);
                        }
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return _PrometheusMetrics;
        }

        public static void SensorMetricReset() { _SensorMetric = null; }

        public static System.Collections.Generic.List<string> Server()
        {
            System.Collections.Generic.List<string> dd = new List<string>();
            if (_PrometheusMetrics != null)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.SortedDictionary<string, PrometheusMetric>>>> KV in _PrometheusMetrics)
                {
                    dd.Add(KV.Key);
                }
            }
            dd.Sort();
            return dd;
        }

        public static System.Collections.Generic.SortedDictionary<string, PrometheusMetric> PrometheusMetricList(string Domain, string Sensor)
        {
            System.Collections.Generic.SortedDictionary<string, PrometheusMetric> dict = new SortedDictionary<string, PrometheusMetric>();
            if (Domain.Length > 0)
            { 
                if (_PrometheusMetrics.ContainsKey(PrometheusServer) && _PrometheusMetrics[PrometheusServer].ContainsKey(Domain) && _PrometheusMetrics[PrometheusServer][Domain].ContainsKey(Sensor))
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, PrometheusMetric> KV in _PrometheusMetrics[PrometheusServer][Domain][Sensor])
                    {
                        dict.Add(KV.Key, KV.Value);
                    }
                }
            }
            return dict;
        }

        public static System.Collections.Generic.SortedDictionary<string, PrometheusMetric> PrometheusMetricList(string Sensor)
        {
            System.Collections.Generic.SortedDictionary<string, PrometheusMetric> dict = new SortedDictionary<string, PrometheusMetric>();
            if (Domain.Length > 0)
            {
                if (_PrometheusMetrics.ContainsKey(PrometheusServer) && _PrometheusMetrics[PrometheusServer].ContainsKey(Domain) && _PrometheusMetrics[PrometheusServer][Domain].ContainsKey(Sensor))
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, PrometheusMetric> KV in _PrometheusMetrics[PrometheusServer][Domain][Sensor])
                    {
                        dict.Add(KV.Key, KV.Value);
                    }
                }
            }
            return dict;
        }

        #endregion

        #region Common

        public static string PrometheusServer { get { return Settings.Default.PrometheusServer + ":" + Settings.Default.PrometheusPort.ToString(); } }

        public static System.Collections.Generic.List<string> _Statics;
        public static System.Collections.Generic.List<string> Statics()
        {
            if (_Statics == null)
            {
                _Statics = new List<string>();
                string SQL = "SELECT Code FROM CollTaskMetricAggregation_Enum";
                System.Data.DataTable dt = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                foreach(System.Data.DataRow R in dt.Rows)
                {
                    _Statics.Add(R[0].ToString());
                }
            }
            return _Statics;
        }

        #endregion


        #region Json

        public static System.Collections.Generic.Dictionary<int, float> ReadMetricFromJson(string URI)
        {
            System.Collections.Generic.Dictionary<int, float> Values = new Dictionary<int, float>();
            System.Object[] TT = null;
            try
            {
                string URL = URI;
                if (!URL.StartsWith("http"))
                    URL = "http://" + URL;
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(URL);
                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();

                string Response = "";
                System.IO.Stream receiveStream = response.GetResponseStream();
                Encoding encode = System.Text.Encoding.UTF8; //.GetEncoding("utf-8"); #253
                System.IO.StreamReader readStream = new System.IO.StreamReader(receiveStream, encode);
                Char[] read = new Char[256];
                int count = readStream.Read(read, 0, 256);
                while (count > 0)
                {
                    // Dumps the 256 characters on a string and displays the string to the console.
                    String str = new String(read, 0, count);
                    Response += str;
                    count = readStream.Read(read, 0, 256);
                }
                System.Collections.Generic.Dictionary<string, object> O = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, Object>>(Response);
                
                    System.Collections.Generic.Dictionary<string, Object> Results = (System.Collections.Generic.Dictionary<string, Object>)O;
                    if (Results.ContainsKey("data"))
                    {
                        if (Results["data"].GetType() ==
                            typeof(System.Collections.Generic.Dictionary<System.String, System.Object>))
                        {
                            System.Collections.Generic.Dictionary<System.String, System.Object> RR =
                                (System.Collections.Generic.Dictionary<System.String, System.Object>)Results["data"];
                            if (RR.ContainsKey("result"))
                            {
                                System.Object[] rr = (System.Object[])RR["result"];
                                foreach (System.Object o in rr)
                                {
                                    if (o.GetType() ==
                                        typeof(System.Collections.Generic.Dictionary<System.String, System.Object>))
                                    {
                                        System.Collections.Generic.Dictionary<System.String, System.Object> oo =
                                            (System.Collections.Generic.Dictionary<System.String, System.Object>)o;
                                        if (oo.ContainsKey("values"))
                                        {
                                            System.Object[] vv = (System.Object[])oo["values"];
                                            for (int i = 0; i < vv.Length; i++)
                                            {
                                                System.Object[] v = (System.Object[])vv[i];
                                                Values.Add(i, float.Parse(v[1].ToString()));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Values;
        }

        public static System.Collections.Generic.Dictionary<string, float> ReadMetricFromJson(string URI, int Rounding)
        {
            System.Collections.Generic.Dictionary<string, float> Values = new Dictionary<string, float>();
            System.Object[] TT = null;
            try
            {
                string URL = URI;
                if (!URL.StartsWith("http"))
                    URL = "http://" + URL;
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(URL);
                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();

                string Response = "";
                System.IO.Stream receiveStream = response.GetResponseStream();
                Encoding encode = System.Text.Encoding.UTF8; //.GetEncoding("utf-8"); // #253
                System.IO.StreamReader readStream = new System.IO.StreamReader(receiveStream, encode);
                Char[] read = new Char[256];
                int count = readStream.Read(read, 0, 256);
                while (count > 0)
                {
                    // Dumps the 256 characters on a string and displays the string to the console.
                    String str = new String(read, 0, count);
                    Response += str;
                    count = readStream.Read(read, 0, 256);
                }
               
                System.Collections.Generic.Dictionary<string, object> O = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, Object>>(Response);
                    System.Collections.Generic.Dictionary<string, Object> Results = (System.Collections.Generic.Dictionary<string, Object>)O;
                    if (Results.ContainsKey("data"))
                    {
                        if (Results["data"].GetType() ==
                            typeof(System.Collections.Generic.Dictionary<System.String, System.Object>))
                        {
                            System.Collections.Generic.Dictionary<System.String, System.Object> RR =
                                (System.Collections.Generic.Dictionary<System.String, System.Object>)Results["data"];
                            if (RR.ContainsKey("result"))
                            {
                                System.Object[] rr = (System.Object[])RR["result"];
                                foreach (System.Object o in rr)
                                {
                                    if (o.GetType() ==
                                        typeof(System.Collections.Generic.Dictionary<System.String, System.Object>))
                                    {
                                        System.Collections.Generic.Dictionary<System.String, System.Object> oo =
                                            (System.Collections.Generic.Dictionary<System.String, System.Object>)o;
                                        if (oo.ContainsKey("values"))
                                        {
                                            System.Object[] vv = (System.Object[])oo["values"];
                                            for (int i = 0; i < vv.Length; i++)
                                            {
                                                System.Object[] v = (System.Object[])vv[i];
                                                System.DateTime dateTime =
                                                    UnixTimeStampToDateTime(double.Parse(v[0].ToString()));
                                                string Key = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
                                                float value = (float)System.Math.Round(decimal.Parse(v[1].ToString()),
                                                    Rounding);
                                                if (!Values.ContainsKey(Key))
                                                    Values.Add(Key, value);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Values;
        }

        //public System.Collections.Generic.Dictionary<int, float> ReadMetricFromJson(string )

        private static System.Collections.Generic.Dictionary<int, string> ReadDataFromJson(string URI, ref string Error, bool SingleObject = false, string Target = "")
        {
            System.Collections.Generic.Dictionary<int, string> Data = new Dictionary<int, string>();
            System.Object[] TT = null;
            try
            {
                string URL = URI;
                if (!URL.StartsWith("http"))
                    URL = "http://" + URL;
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(URL);
                if (SingleObject)
                    request.Timeout = 1000;
                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();

                string Response = "";
                System.IO.Stream receiveStream = response.GetResponseStream();
                Encoding encode = System.Text.Encoding.UTF8; //.GetEncoding("utf-8"); #253
                System.IO.StreamReader readStream = new System.IO.StreamReader(receiveStream, encode);
                Char[] read = new Char[256];
                int count = readStream.Read(read, 0, 256);
                while (count > 0)
                {
                    // Dumps the 256 characters on a string and displays the string to the console.
                    String str = new String(read, 0, count);
                    Response += str;
                    count = readStream.Read(read, 0, 256);
                }

                System.Collections.Generic.Dictionary<string, object> O =
                    System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, Object>>(Response);
                System.Collections.Generic.Dictionary<string, Object> Results =
                    (System.Collections.Generic.Dictionary<string, Object>)O;
                if (Results.ContainsKey("data"))
                {
                    if (Results["data"].GetType() ==
                        typeof(System.Collections.Generic.Dictionary<System.String, System.Object>))
                    {
                        System.Collections.Generic.Dictionary<System.String, System.Object> RR =
                            (System.Collections.Generic.Dictionary<System.String, System.Object>)Results["data"];
                        if (RR.ContainsKey("result"))
                        {
                            System.Object[] rr = (System.Object[])RR["result"];
                            foreach (System.Object o in rr)
                            {
                                if (o.GetType() ==
                                    typeof(System.Collections.Generic.Dictionary<System.String, System.Object>))
                                {
                                    System.Collections.Generic.Dictionary<System.String, System.Object> oo =
                                        (System.Collections.Generic.Dictionary<System.String, System.Object>)o;
                                    if (oo.ContainsKey("values"))
                                    {
                                        System.Object[] vv = (System.Object[])oo["values"];
                                        for (int i = 0; i < vv.Length; i++)
                                        {
                                            System.Object[] v = (System.Object[])vv[i];
                                            Data.Add(i, v[1].ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (Results["data"].GetType() == typeof(System.Object[]))
                    {
                        System.Object[] oo = (System.Object[])Results["data"];
                        foreach (System.Object o in oo)
                        {
                            if (o.GetType() == typeof(string))
                            {
                                Data.Add(Data.Count, o.ToString());
                            }
                            else if (o.GetType() ==
                                     typeof(System.Collections.Generic.Dictionary<System.String, System.Object>))
                            {
                                System.Collections.Generic.Dictionary<System.String, System.Object> OO =
                                    (System.Collections.Generic.Dictionary<System.String, System.Object>)o;
                                foreach (System.Collections.Generic.KeyValuePair<System.String, System.Object> KV in OO)
                                {
                                    if (Target.Length > 0 && KV.Key == Target)
                                    {
                                        if (!Data.ContainsValue(KV.Value.ToString()))
                                            Data.Add(Data.Count, KV.Value.ToString());
                                    }
                                }
                            }
                            else
                            {
                                string Test = o.GetType().ToString();
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                Error = "Failed to retrieve data from " + URI + ":\r\n" + ex.Message;
            }

            return Data;
        }

        #endregion

        #region PrometheusApis

        private static System.Collections.Generic.List<string> _PrometheusApis;

        public static System.Collections.Generic.List<string> PrometheusApis()
        {
            if (DiversityCollection.Tasks.Settings.Default.PrometheusApiLinks == null)
                DiversityCollection.Tasks.Settings.Default.PrometheusApiLinks = new System.Collections.Specialized.StringCollection();
            if (_PrometheusApis == null || (_PrometheusApis != null && _PrometheusApis.Count == 0))
            {
                _PrometheusApis = new List<string>();
                foreach (string A in DiversityCollection.Tasks.Settings.Default.PrometheusApiLinks)
                {
                    _PrometheusApis.Add(A);
                }
                _PrometheusApis.Sort();
            }
            return _PrometheusApis;
        }

        public static bool PrometheusApisAdd(string Api)
        {
            bool OK = false;
            try
            {
                if (!PrometheusApis().Contains(Api))
                {
                    _PrometheusApis.Add(Api);
                    _PrometheusApis.Sort();
                    OK = true;
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return OK;
        }

        public static bool PrometheusApisRemove(string Api)
        {
            bool OK = false;
            try
            {
                if (PrometheusApis().Contains(Api))
                {
                    _PrometheusApis.Remove(Api);
                    _PrometheusApis.Sort();
                    OK = true;
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return OK;
        }


        public static bool PrometheusApisSave()
        {
            bool OK = false;
            try
            {
                //DiversityCollection.Tasks.Settings.Default.PrometheusApiLinks.Clear();
                System.Collections.Generic.List<string> toRemove = new List<string>();
                foreach (string PA in DiversityCollection.Tasks.Settings.Default.PrometheusApiLinks)
                {
                    if (PA.StartsWith(PrometheusServer))
                    {
                        toRemove.Add(PA);
                        //DiversityCollection.Tasks.Settings.Default.PrometheusApiLinks.Remove(PA);
                    }
                }
                foreach(string PA in toRemove)
                {
                    if(DiversityCollection.Tasks.Settings.Default.PrometheusApiLinks.Contains(PA))
                        DiversityCollection.Tasks.Settings.Default.PrometheusApiLinks.Remove(PA);
                }
                PrometheusApis().Sort();
                foreach (string A in PrometheusApis())
                {
                    if (A != null)
                    DiversityCollection.Tasks.Settings.Default.PrometheusApiLinks.Add(PrometheusServer + "_" + A);
                }
                DiversityCollection.Tasks.Settings.Default.Save();
                OK = true;
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return OK;
        }

        #endregion

        #region Auxillary
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        public static string DiplayTextSensor(string Sensor)
        {
            string DisplayText = Sensor;
            if (DisplayText.Length == 12)
            {
                bool IsMac = true;
                foreach(Char c in DisplayText.ToUpper())
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
                    for(int i = 0; i < DisplayText.Length; i++)
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

        public static string DiplayTextMetric(string Metric)
        {
            string DisplayText = Metric.Replace('_', ' ');
            DisplayText = DisplayText.Substring(0, 1).ToUpper() + DisplayText.Substring(1);
            return DisplayText;
        }

        public enum MetricType { Battery, Humidity, Temperature }

        public static MetricType SensorMetricType(string Metric)
        {
            MetricType metricType = MetricType.Battery;
            if (Metric.ToLower().IndexOf("temperatur") > -1)
                metricType = MetricType.Temperature;
            else if (Metric.ToLower().IndexOf("humidity") > -1)
                metricType = MetricType.Humidity;
            return metricType;
        }

        #endregion

    }
}
