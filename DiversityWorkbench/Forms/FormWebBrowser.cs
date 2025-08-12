using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormWebBrowser : Form
    {
        #region Parameter
        private System.Collections.Generic.List<System.Uri> Websites;
        private int Position = 0;
        private System.Uri URI;
        private string _FormStartAddress = "";
        private string _StartAddress = "https://duckduckgo.com/";
        private string _SearchAddress = "https://duckduckgo.com/html?q=";

        public bool ShowExternal
        {
            get { return this.toolStripButtonSystemBrowser.Visible; }
            set { this.toolStripButtonSystemBrowser.Visible = value; }
        }

        #endregion

        #region Construction

        public FormWebBrowser()
        {
            InitializeComponent();
            try { this.userControlWebView.ScriptErrorsSuppressed = true; }
            catch { }
            this.Websites = new List<Uri>();
            string URL = this._StartAddress;// "http://www.google.com";
            this.textBoxURL.Text = URL;
            Navigate(URL);
        }

        public FormWebBrowser(string URL)
        {
            InitializeComponent();
            try { this.userControlWebView.ScriptErrorsSuppressed = true; }
            catch { }
            if (URL.Length == 0) URL = this._StartAddress;// "http://www.google.com";
            this.textBoxURL.Text = URL;
            if (!Navigate(URL))
                Navigate(_SearchAddress + URL);
            else
                _FormStartAddress = this.userControlWebView.Url.ToString();
            this.Websites = new List<Uri>();
        }

        /// <summary>
        /// Open a browser window with a given URL
        /// </summary>
        /// <param name="HTML">The URL to be shown</param>
        /// <param name="ShowStartPage">If the URL is missing, show http://www.google.com</param>
        public FormWebBrowser(string HTML, bool ShowStartPage)
        {
            InitializeComponent();
            try { this.userControlWebView.ScriptErrorsSuppressed = true; }
            catch { }
            if (HTML.Length == 0 && ShowStartPage)
            {
                HTML = this._StartAddress;// "http://www.google.com";
            }
            else
            {
                try
                {
                    this.userControlWebView.Navigate("about:blank");
                    string html = "<html><head><title>Test</title></head><body></body></html>";
                    this.userControlWebView.NavigateToDocument(html);

                    //if (this.webBrowser.Document == null)
                    //    this.webBrowser.Document.OpenNew(false);
                    //this.webBrowser.Document.Write("<html><head><title>Test</title></head><body></body></html>");

                    //this.webBrowser.Document.Body.InnerText = HTML;
                    //this.webBrowser.Refresh();

                    //string test = this.webBrowser.DocumentText;

                    //if (!HTML.StartsWith("<html><body>"))
                    //    HTML = "<HTML><body>" + HTML + "</body></HTML>";

                    //this.webBrowser.Navigate("about:blank");
                    //this.webBrowser.Document.OpenNew(false);
                    //this.webBrowser.Document.Write(HTML);
                    //this.webBrowser.Refresh();
                    //this.webBrowser.DocumentText  = 
                    //    "<html><body>Please enter your name:<br/>" +
                    //    "<input type='text' name='userName'/><br/>" +
                    //    "<a href='http://www.microsoft.com'>continue</a>" +
                    //    "</body></html>";
                    //System.IO.FileInfo HtmlFile = new System.IO.FileInfo(...Windows.Forms.Application.StartupPath + "\\Temp.html");
                    //System.Windows.Forms.HtmlDocument H = this.webBrowser.Document;
                    //H.Write(HTML);
                }
                catch (System.Exception ex) { }
                //System.IO.StreamWriter s = new System.IO.StreamWriter(HtmlFile.FullName, false);
                //s.Write(HTML);
                //this.webBrowser.DocumentText.BeginRead( = HtmlFile.FullName;
            }
            this.Websites = new List<Uri>();
        }

        #endregion

        #region Navigation

        public void AllowNavigation(bool DoAllow)
        {
            if (!DoAllow)
            {
                this.toolStrip.Visible = false;
                this.textBoxURL.Enabled = false;
            }
        }

        private void toolStripButtonBrowse_Click(object sender, EventArgs e)
        {
            Navigate(this.textBoxURL.Text);
        }

        private void textBoxURL_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Navigate(this.textBoxURL.Text);
            }
        }

        // Navigates to the given URL if it is valid.
        private bool Navigate(String address)
        {
            bool OK = true;
            address = DiversityWorkbench.Forms.FormFunctions.UrlText(address);
            this.toolStripButtonFile.Visible = address.Contains(":\\") || address.StartsWith("file:///");
            if (String.IsNullOrEmpty(address)) return false;
            if (address.Equals("about:blank")) return false;
            if (!address.StartsWith("http://")
                && !address.Contains(":\\")
                && !address.StartsWith("file:///")
                && !address.StartsWith("https://"))
                address = _SearchAddress + address; // "http://" + address;
            try
            {
                this.URI = new Uri(address);
                this.userControlWebView.Navigate(this.URI);
            }
            catch (System.UriFormatException)
            {
                OK = false;
            }
            return OK;
        }

        // Updates the URL in TextBoxAddress upon navigation.
        private void userControlWebView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            this.textBoxURL.Text = userControlWebView.Url.ToString();
            int i;
            bool OK = false;
            for (i = 0; i < this.Websites.Count; i++)
            {
                if (this.Websites[i].AbsoluteUri == userControlWebView.Url.ToString())
                {
                    this.Position = i;
                    OK = true;
                    break;
                }
            }
            if (!OK)
            {
                this.URI = new Uri(userControlWebView.Url.ToString());
                this.Websites.Add(this.URI);
                this.Position = i;
            }
            if (this.Position == 0) this.toolStripButtonBack.Enabled = false;
            else this.toolStripButtonBack.Enabled = true;
            if (this.Position == this.Websites.Count - 1) this.toolStripButtonForward.Enabled = false;
            else this.toolStripButtonForward.Enabled = true;
        }

        private void toolStripButtonScriptOn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to activate scripts?", "Activate scripts", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                return;

            this.toolStripButtonScriptOn.Visible = false;
            this.toolStripButtonScriptOff.Visible = true;
            this.userControlWebView.AllowScripting = true;
        }

        private void toolStripButtonScriptOff_Click(object sender, EventArgs e)
        {
            this.toolStripButtonScriptOn.Visible = true;
            this.toolStripButtonScriptOff.Visible = false;
            this.userControlWebView.AllowScripting = false;
        }

        private void toolStripButtonBack_Click(object sender, EventArgs e)
        {
            if (this.Position > 0)
            {
                this.Position--;
                this.Navigate(this.Websites[this.Position].AbsoluteUri);
            }
            else
            {
                this.userControlWebView.NavigateBack();
            }
        }

        private void toolStripButtonForward_Click(object sender, EventArgs e)
        {
            if (this.Position < this.Websites.Count - 1)
            {
                this.Position++;
                this.Navigate(this.Websites[this.Position].AbsoluteUri);
            }
            else
            {
                this.userControlWebView.NavigateForward();
            }
        }

        private void toolStripButtonHome_Click(object sender, EventArgs e)
        {
            string URL = this._FormStartAddress == "" ? this._StartAddress : this._FormStartAddress;// "http://www.google.com";
            this.textBoxURL.Text = URL;
            Navigate(URL);
        }

        private void toolStripButtonFile_Click(object sender, EventArgs e)
        {
            string URL = this.userControlWebView.Url.AbsolutePath;
            System.IO.FileInfo fi = new System.IO.FileInfo(URL);
            if (fi.Exists)
                this.openFileDialog.InitialDirectory = fi.DirectoryName;
            else
                this.openFileDialog.InitialDirectory = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule();// ...Windows.Forms.Application.StartupPath;
            if (this.openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                URL = this.openFileDialog.FileName;
                this.textBoxURL.Text = URL;
                Navigate(URL);
            }
        }

        private void toolStripButtonSystemBrowser_Click(object sender, EventArgs e)
        {
            if (this.userControlWebView.Url != null)
            {
                System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(this.userControlWebView.Url.ToString());
                info.UseShellExecute = true;
                System.Diagnostics.Process.Start(info);
            }
        }

        private void toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Navigate(this.textBoxURL.Text); //this.userControlWebView.NavigateToAddress(this._FormStartAddress == "" ? this._StartAddress : this._FormStartAddress);
        }

        #endregion

        #region Properties
        public string URL
        {
            get
            {
                return this.textBoxURL.Text;
            }
        }

        public bool AllowFile
        {
            get
            {
                return this.toolStripButtonFile.Visible;
            }
            set
            {
                this.toolStripButtonFile.Visible = value;
            }
        }
        #endregion

        #region Dialog
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.textBoxURL.Text = "";
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        #endregion

        #region Public functions

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        #endregion
    }
}