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

    //public struct WebResource
    //{
    //    public System.Uri Uri;
    //    public string Title;
    //    public string CopyRight;
    //}
    public partial class FormWebview : Form
    {

        //private System.Collections.Generic.List<System.Uri> _uris;
        private int _Position;
        private System.Collections.Generic.List<Tasks.Resource> _webResources;
        public FormWebview(System.Uri uri, string Title)
        {
            InitializeComponent();
            this.Text = Title;
            this.webView2.Source = uri;
            this.linkLabel.Text = uri.ToString();
        }

        //public FormWebview(System.Collections.Generic.List< System.Uri> uris, string Title)
        //{
        //    InitializeComponent();
        //    this.Text = Title;
        //    this._uris = uris;
        //    this.webView2.Source = uris[0];
        //    this.linkLabel.Text = uris[0].ToString();
        //}

        public FormWebview(System.Collections.Generic.List<Tasks.Resource> webResources)
        {
            InitializeComponent();
            this._webResources = webResources;
            this._Position = 0;
            this.setPosition(_Position);
            //this.Text = webResources[0].Title;
            //this.webView2.Source = webResources[0].Uri;
            //this.linkLabel.Text = webResources[0].Uri.ToString();
            //this.linkLabelCopyright.Text = webResources[0].CopyRight;
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            this.setPosition(this._Position + 1);
            //if (this._uris != null && this._uris.Count > _Position + 1)
            //{
            //    _Position++;
            //    this.webView2.Source = _uris[_Position];
            //    this.linkLabel.Text = _uris[_Position].ToString();
            //    this.buttonPrevious.Visible = true;
            //    if (this._uris.Count == _Position + 1)
            //        this.buttonNext.Visible = false;
            //}
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            this.setPosition(this._Position - 1);
            //if (this._uris != null && _Position > 0)
            //{
            //    _Position--;
            //    this.webView2.Source = _uris[_Position];
            //    this.linkLabel.Text = _uris[_Position].ToString();
            //    this.buttonNext.Visible = true;
            //    if (_Position == 0)
            //        this.buttonPrevious.Visible = false;
            //}
        }

        private void setPosition(int i)
        {
            try
            {
                if (this._webResources != null)
                {
                    if (i > this._webResources.Count + 1 || i < 0)
                        return;
                    this._Position = i;
                    this.Text = _webResources[i].Title;
                    if (_webResources[i].Notes != null && _webResources[i].Notes.Length > 0)
                        this.Text += " " + _webResources[i].Notes;
                    this.webView2.Source = _webResources[i].Uri;
                    this.linkLabel.Text = _webResources[i].Uri.ToString();
                    this.linkLabelCopyright.Text = _webResources[i].CopyRight;
                    this.buttonNext.Visible = _Position < this._webResources.Count - 1;
                    this.buttonPrevious.Visible = _Position > 0;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

        }
    }
}
