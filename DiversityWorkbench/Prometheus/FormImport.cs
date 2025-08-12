using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityWorkbench.Prometheus
{
    public partial class FormImport : Form
    {

        private string grafanaUrl = "http://127.0.0.1:3000/d/9Ivy9bFnk/testprometheus?orgId=1";
        public FormImport()
        {
            InitializeComponent();
            this.textBoxGrafanaUrl.Text = grafanaUrl;
            this.setGrafanaUrl();
        }

        private void buttonGrafanaRefresh_Click(object sender, EventArgs e)
        {
            this.setGrafanaUrl();
        }

        private void setGrafanaUrl()
        {
            System.Uri U = new Uri(grafanaUrl);
        }

        private void textBoxGrafanaUrl_TextChanged(object sender, EventArgs e)
        {
            if (textBoxGrafanaUrl.Text != grafanaUrl)
                grafanaUrl = textBoxGrafanaUrl.Text;
        }
    }
}
