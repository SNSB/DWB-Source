using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.IO;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace DiversityWorkbench.UserControls
{
    public partial class UserControlWebView : UserControl
    {
        #region Events
        public event EventHandler<CoreWebView2NavigationStartingEventArgs> NavigationStarting;

        protected virtual void OnNavigationStarting(CoreWebView2NavigationStartingEventArgs e)
        {
            EventHandler<CoreWebView2NavigationStartingEventArgs> handler = NavigationStarting;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public event EventHandler<CoreWebView2NavigationCompletedEventArgs> NavigationCompleted;

        protected virtual void OnNavigationCompleted(CoreWebView2NavigationCompletedEventArgs e)
        {
            EventHandler<CoreWebView2NavigationCompletedEventArgs> handler = NavigationCompleted;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<CoreWebView2WebMessageReceivedEventArgs> MessageReceived;

        protected virtual void OnMessageReceived(CoreWebView2WebMessageReceivedEventArgs e)
        {
            EventHandler<CoreWebView2WebMessageReceivedEventArgs> handler = MessageReceived;
            {
                if (handler != null)
                {
                    handler(this, e);
                }
            }
        }
        #endregion

        #region Static
        private static string _UserDataFolder = null;
        #endregion

        #region Parameter
        private bool _NavigationCompleted = false;

        private bool _AllowScripting = false;
        public bool AllowScripting
        {
            get => _AllowScripting;
            set
            {
                _AllowScripting = value;
                if (this.webView2.CoreWebView2 != null)
                    this.webView2.Reload();
            }
        }

        private bool _ScriptErrorsSuppressed = false;
        public bool ScriptErrorsSuppressed
        {
            get => _ScriptErrorsSuppressed;
            set
            {
                _ScriptErrorsSuppressed = value;
                if (_ScriptErrorsSuppressed)
                    _AllowScripting = false;
            }
        }
        public Uri Url
        {
            get => this.webView2.Source;
            set => this.Navigate(value);
        }
        #endregion


        private CoreWebView2CreationProperties _creationProperties = null;
        public CoreWebView2CreationProperties CreationProperties
        {
            get
            {
                if (_creationProperties == null)
                {
                    _creationProperties = new Microsoft.Web.WebView2.WinForms.CoreWebView2CreationProperties();
                    _creationProperties.UserDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\DiversityWorkbench\NETwebView";
                }
                return _creationProperties;
            }
            set
            {
                _creationProperties = value;
            }
        }

        #region Construction
        public UserControlWebView()
        {
            // Initialize control
            InitializeComponent();
            webView2.CreationProperties = CreationProperties;
            ////// Initialize webView2 control
            ////InitializeWebView();
            // this.Load += UserControlWebView_Load;
        }

        public UserControlWebView(CoreWebView2CreationProperties creationProperties = null)
        {
            this.CreationProperties = creationProperties;
            // Initialize control
            InitializeComponent();
        }
        //private async void UserControlWebView_Load(object sender, EventArgs e)
        //{
        //    if (_isInitialized)
        //        return;
        //    _isInitialized = true;
        //    await InitializeWebView();
        //}


        //private async Task InitializeWebView(string url = null)
        //{

        //#if DEBUG
        //            // Start of form designer crashes if async init is done, therefore do not perform this in debug mode
        //            return;
        //#endif
        //    // Set the user data location folder to temp folder
        //    if (webView2.CoreWebView2 != null)
        //    {
        //        // webView is already initialized, skip reinitialization
        //        return;
        //    }
        //    if (_UserDataFolder == null)
        //        _UserDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\DiversityWorkbench\NETwebView";

        //    var env = await CoreWebView2Environment.CreateAsync(null, _UserDataFolder);
        //    await webView2.EnsureCoreWebView2Async(env);
        //    if (!string.IsNullOrEmpty(url))
        //    {
        //        webView2.Source = new Uri(url);
        //    }
        //}
        #endregion

        #region Public
        public void Navigate(Uri Url)
        {
            if (Url == null)
                Url = new Uri("about:blank");
            this.webView2.Source = Url;
        }

        public void Navigate(string address)
        {
            if (address == null)
                address = "about:blank";
            Uri url = new Uri(address);
            this.Navigate(url);
        }

        public void NavigateToDocument(string html)
        {
            this.webView2.CoreWebView2.NavigateToString(html);
        }

        public void NavigateBack()
        {
            this.webView2.GoBack();
        }

        public void NavigateForward()
        {
            this.webView2.GoForward();
        }

        public Task<string> ExecuteScriptAsync(string javaScript)
        {
            if (_NavigationCompleted)
                return this.webView2.ExecuteScriptAsync(javaScript);
            else return null;
        }

        public async void CallJavaScript(string javaScript)
        {
            if (_NavigationCompleted)
                await this.webView2.ExecuteScriptAsync(javaScript);
        }
        #endregion

        #region Event handler
        private void WebView2NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            _NavigationCompleted = false;
            if (this.webView2.CoreWebView2 != null)
                this.webView2.CoreWebView2.Settings.IsScriptEnabled = AllowScripting;
            OnNavigationStarting(e);
        }

        private void WebView2NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            _NavigationCompleted = true;
            OnNavigationCompleted(e);
        }

        private void WebView2WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            OnMessageReceived(e);
        }
        #endregion
    }
}
