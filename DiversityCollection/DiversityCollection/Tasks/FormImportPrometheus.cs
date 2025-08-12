using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;

namespace DiversityCollection.Tasks
{

    public partial class FormImportPrometheus : Form
    {

        private string grafanaUrl = "";// http://127.0.0.1:3000/d/9Ivy9bFnk/testprometheus?orgId=1";
        private bool _FirstTrialForGrafana = true;

        public FormImportPrometheus()
        {
            InitializeComponent();
            this.initGrafanaDashboards();
            this.setGrafanaUrl();
        }

        private void initGrafanaDashboards()
        {
            if (DiversityCollection.Tasks.Settings.Default.GrafanaDashboards != null && DiversityCollection.Tasks.Settings.Default.GrafanaDashboards.Count > 0)
            {
                this.comboBoxGrafanaDashboards.Items.Clear();
                foreach (string D in DiversityCollection.Tasks.Settings.Default.GrafanaDashboards)
                    this.comboBoxGrafanaDashboards.Items.Add(D);
            }

        }

        private void setGrafanaUrl()
        {
            if (grafanaUrl.Length == 0)
                return;
            try
            {
                System.Uri U = new Uri(grafanaUrl);
                this.webView2Grafana.EnsureCoreWebView2Async();
                this.webView2Grafana.Source = U;
                if (this.webView2Grafana != null && this.webView2Grafana.CoreWebView2 != null)
                    this.webView2Grafana.CoreWebView2.Navigate(grafanaUrl);
                else
                {
                    if (!this._FirstTrialForGrafana)
                        System.Windows.Forms.MessageBox.Show("You may need to install the runtime of the edge browser:\r\nhttps://developer.microsoft.com/en-us/microsoft-edge/webview2");
                    else
                        _FirstTrialForGrafana = false;
                }
                //this.webView2Grafana.Source = U;

            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void comboBoxGrafanaDashboards_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.grafanaUrl = this.comboBoxGrafanaDashboards.SelectedItem.ToString();
            this.setGrafanaUrl();
        }

        private void buttonGrafanaAdd_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Grafana dashboard", "Please enter the URL of a grafana dashboard", "");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK && f.String.Length > 0)
            {
                try
                {
                    System.Uri U = new Uri(f.String);
                    if (DiversityCollection.Tasks.Settings.Default.GrafanaDashboards == null)
                        DiversityCollection.Tasks.Settings.Default.GrafanaDashboards = new System.Collections.Specialized.StringCollection();
                    if (!DiversityCollection.Tasks.Settings.Default.GrafanaDashboards.Contains(f.String))
                    {
                        DiversityCollection.Tasks.Settings.Default.GrafanaDashboards.Add(f.String);
                        DiversityCollection.Tasks.Settings.Default.Save();
                    }
                    this.initGrafanaDashboards();
                }
                catch(System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

    }
}
