using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.UserControls
{
    public partial class UserControlResource : UserControl
    {
        #region local types
        private enum MediumState
        {
            none,
            stopped,
            playing,
            paused
        }

        private enum WebRequestResult
        {
            none,
            success,
            tooBig,
            errorNoAccess,
            errorSslNotPossible,
            errorUnspecified
        }

        private delegate void upbdateControlDelegate(WebRequestResult webResult, string requestUri, int resourceSize, bool ignoreSize);

        private class WebRequestState
        {
            // This class stores the state of the web request
            public upbdateControlDelegate resourceDelegate;
            public string requestUri;
            public bool ignoreSize;
            public int resourceSize;
            public System.Net.WebRequest request;
            public System.Net.WebResponse response;
            public WebRequestResult result;

            public WebRequestState()
            {
                resourceDelegate = null;
                requestUri = "";
                ignoreSize = false;
                resourceSize = -1;
                request = null;
                response = null;
                result = WebRequestResult.none;
            }
        }
        #endregion

        #region Parameter
        string _NoAccessPath = "";

		private WpfControls.Media.UserControlMediaPlayer userControlPlayer;

        private DiversityWorkbench.UserControls.UserControlImage _uci;
        private DiversityWorkbench.UserControls.UserControlImage uci
        {
            get
            {
                if (_uci == null)
                    _uci = new UserControlImage();
                return _uci;
            }
        }

        private int _Timeout = 0;
        public int Timeout
        {
            get { return _Timeout; }
            set
            {
                if (value >= 0)
                    _Timeout = value;
            }
        }

        private bool _BackgroundLoading = true;
        public bool BackgroundLoading
        {
            get { return _BackgroundLoading; }
            set { _BackgroundLoading = value; }
        }

        private DiversityWorkbench.Forms.FormFunctions.Medium _MediumType;
        public DiversityWorkbench.Forms.FormFunctions.Medium MediumType
        {
            get { return _MediumType; }
            set
            {
                _MediumType = value;
                this.labelMediumType.Text = _MediumType.ToString();
                StopPlayback();
                switch (this.MediumType)
                {
                    case DiversityWorkbench.Forms.FormFunctions.Medium.Unknown:
                        this.splitContainerResource.Panel1Collapsed = true;
                        this.splitContainerResource.Panel2Collapsed = false;
                        this.splitContainerMedia.Panel1Collapsed = false;
                        this.splitContainerMedia.Panel2Collapsed = true;
                        break;
                    case DiversityWorkbench.Forms.FormFunctions.Medium.Audio:
                    case DiversityWorkbench.Forms.FormFunctions.Medium.Video:
                        this.splitContainerResource.Panel1Collapsed = true;
                        this.splitContainerResource.Panel2Collapsed = false;
                        this.splitContainerMedia.Panel1Collapsed = true;
                        this.splitContainerMedia.Panel2Collapsed = false;
                        break;
                    default:
                        this.splitContainerResource.Panel1Collapsed = false;
                        this.splitContainerResource.Panel2Collapsed = true;
                        this.pictureBox.BackColor = SystemColors.Control;
                        break;
                }
            }
        }

        public string ResourcePath
        {
            get
            {
                return this.textBoxResourcePath.Text;
            }
            set
            {
                this.textBoxResourcePath.Text = value;
                this.buttonOpenResource.Enabled = value != null && value.Length > 0 && !value.ToLower().StartsWith("color://#");
                loadResource();
            }
        }

        public void SetPathVisibility(bool visible)
        {
            this.tableLayoutPanelPath.Visible = visible;
        }
        #endregion

        #region Construction
        public UserControlResource()
        {
            InitializeComponent();

            // Child-Property cannot be set in form editor for SDK-like projekts, perform basic init here
            this.userControlPlayer = new WpfControls.Media.UserControlMediaPlayer();
            this.elementHostMedia.Child = userControlPlayer;

            // Set event handler
			
            this.userControlPlayer.MediaPlayer.MediaEnded += mediaElementPlayer_MediaEnded;

            this._MediumType = DiversityWorkbench.Forms.FormFunctions.Medium.Image;
            try { this.userControlWebView.ScriptErrorsSuppressed = true; }
            catch { }
            this.buttonLoadBig.Visible = false;
            this.buttonOpenResource.Enabled = false;
        }
        #endregion

        #region Public
        public event EventHandler WebRequestTimedOut;
        protected virtual void OnWebRequestTimedOut(EventArgs e)
        {
            if (WebRequestTimedOut != null)
                WebRequestTimedOut(this, e);
        }

        public event EventHandler WebRequestEnded;
        protected virtual void OnWebRequestEnded(EventArgs e)
        {
            if (WebRequestEnded != null)
                WebRequestEnded(this, e);
        }
        #endregion

        #region Static
        private static void RespCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                // Evaluate asynchronous result
                WebRequestState reqState = (WebRequestState)asynchronousResult.AsyncState;

                try
                {
                    // Get asynchronous response
                    System.Net.WebRequest webRequest = reqState.request;
                    reqState.response = webRequest.EndGetResponse(asynchronousResult);
                    long lengthOfUri = reqState.response.ContentLength;
                    reqState.resourceSize = (int)(lengthOfUri / 1000);
                    reqState.result = !reqState.ignoreSize && reqState.resourceSize > DiversityWorkbench.Settings.MaximalImageSizeInKb ? WebRequestResult.tooBig : WebRequestResult.success;
                }
                catch (System.Net.WebException web)
                {
                    if (web.Status == System.Net.WebExceptionStatus.SecureChannelFailure)
                        reqState.result = WebRequestResult.errorSslNotPossible;
                    else
                        reqState.result = WebRequestResult.errorNoAccess;
                }
                catch (Exception)
                {
                    reqState.result = WebRequestResult.errorUnspecified;
                }

                // Update control 
                if (reqState.resourceDelegate != null)
                    reqState.resourceDelegate(reqState.result, reqState.requestUri, reqState.resourceSize, reqState.ignoreSize);
                reqState.resourceDelegate = null;

                // Close request
                if (reqState.response != null)
                    reqState.response.Close();
                reqState.request.Abort();
            }
            catch (Exception)
            { }
        }

        private static void TimeoutCallback(object state, bool timedOut)
        {
            if (timedOut)
            {
                // Evaluate asynchronous result
                WebRequestState reqState = state as WebRequestState;
                if (reqState != null)
                {
                    // Close request
                    reqState.request.Abort();
                }
            }
        }
        #endregion

        #region Private
        void mediaElementPlayer_MediaEnded(object sender, System.Windows.RoutedEventArgs e)
        {
            StopPlayback();
        }
        private void PlayMedia()
        {
            this.userControlPlayer.MediaPlayer.Play();
            this.buttonPlay.Image = DiversityWorkbench.Properties.Resources.Pause.ToBitmap();
            this.labelMediumState.Text = MediumState.playing.ToString();
        }
        private void TogglePause()
        {
            if (this.labelMediumState.Text == MediumState.playing.ToString())
            {
                this.userControlPlayer.MediaPlayer.Pause();
                this.buttonPlay.Image = DiversityWorkbench.Properties.Resources.ArrowNext;
                this.labelMediumState.Text = MediumState.paused.ToString();
            }
            else
            {
                PlayMedia();
            }
        }
        private void StopPlayback()
        {
            if (this.labelMediumState.Text != MediumState.stopped.ToString())
            {
                this.buttonPlay.Image = DiversityWorkbench.Properties.Resources.ArrowNext;
                this.labelMediumState.Text = MediumState.stopped.ToString();
                this.userControlPlayer.MediaPlayer.Stop();
            }
        }

        private void loadResource(bool ignoreSize = false)
        {
            Cursor saveCursor = this.Cursor;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            // Show start image
            this._NoAccessPath = "";
            this.buttonLoadBig.Visible = false;
            this.pictureBox.Image = null;
            this.userControlWebView.Url = null;
            this.userControlPlayer.MediaPlayer.Source = null;
            this.MediumType = DiversityWorkbench.Forms.FormFunctions.Medium.Image;
            this.pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
            this.pictureBox.Image = (System.Drawing.Bitmap)DiversityWorkbench.Properties.Resources.ResourceManager.GetObject("wait_animation");
            this.labelMediumState.Text = MediumState.stopped.ToString();
            this.splitContainerResource.SuspendLayout();

            // Load media data
            bool OK = true;
            string Path = this.textBoxResourcePath.Text.Replace("/", "\\");

            if (Path == "")
            {
                MediumType = DiversityWorkbench.Forms.FormFunctions.Medium.Image;
                this.pictureBox.Image = null;
            }
            else
            {
                try
                {
                    if ((Path.ToLower().StartsWith("http:") || Path.ToLower().StartsWith("https:")) && DiversityWorkbench.Forms.FormFunctions.MediaType(this.textBoxResourcePath.Text) != DiversityWorkbench.Forms.FormFunctions.Medium.Ignore)
                    {
                        Application.DoEvents(); // Show default image during data loading
                        System.Net.WebRequest webReq = System.Net.WebRequest.Create(this.textBoxResourcePath.Text);
                        webReq.Method = "HEAD"; // Read only header data
                        webReq.Proxy = null;
                        webReq.Timeout = _Timeout > 0 ? _Timeout : DiversityWorkbench.WorkbenchSettings.Default.TimeoutWeb;

                        if (_BackgroundLoading)
                        {
                            this.buttonOpenResource.Enabled = false;
                            WebRequestState reqState = new WebRequestState();
                            reqState.resourceDelegate = this.updateControl;
                            reqState.requestUri = this.textBoxResourcePath.Text;
                            reqState.ignoreSize = ignoreSize;
                            reqState.request = webReq;

                            // Start the asynchronous call for response.
                            IAsyncResult asyncResult = (IAsyncResult)reqState.request.BeginGetResponse(new AsyncCallback(RespCallback), reqState);

                            if (asyncResult != null)
                            {
                                // Set timout supervision - timout field in request does not work for async request
                                System.Threading.RegisteredWaitHandle handle = System.Threading.ThreadPool.RegisterWaitForSingleObject(
                                      asyncResult.AsyncWaitHandle,
                                      new System.Threading.WaitOrTimerCallback(TimeoutCallback),
                                      reqState,
                                      _Timeout > 0 ? _Timeout : DiversityWorkbench.WorkbenchSettings.Default.TimeoutWeb,  // timeout
                                      true);
                                if (handle == null)
                                    OK = false;
                            }
                            else
                                OK = false;
                        }
                        else
                        {
                            int resourceSize = -1;
                            System.Net.WebResponse webResponse = null;
                            WebRequestResult webResult = WebRequestResult.none;
                            try
                            {
                                // Get synchronous response
                                webResponse = webReq.GetResponse();
                                long lengthOfUri = webResponse.ContentLength;
                                resourceSize = (int)(lengthOfUri / 1000);
                                webResult = !ignoreSize && resourceSize > DiversityWorkbench.Settings.MaximalImageSizeInKb ? WebRequestResult.tooBig : WebRequestResult.success;
                            }
                            catch (System.Net.WebException)
                            {
                                webResult = WebRequestResult.errorNoAccess;
                            }
                            catch (Exception)
                            {
                                webResult = WebRequestResult.errorUnspecified;
                            }

                            // Close request
                            if (webResponse != null)
                                webResponse.Close();
                            webReq.Abort();

                            // Update control 
                            updateControl(webResult, this.textBoxResourcePath.Text, resourceSize, ignoreSize);
                        }
                    }
                    else if (Path.ToLower().StartsWith("color:\\\\#"))
                    {
                        // Treat color as image and set back color
                        this.MediumType = DiversityWorkbench.Forms.FormFunctions.Medium.Image;
                        int idx = "color:\\\\#".Length;
                        int r = Convert.ToInt32(Path.Substring(idx, 2), 16);
                        idx += 2;
                        int g = Convert.ToInt32(Path.Substring(idx, 2), 16);
                        idx += 2;
                        int b = Convert.ToInt32(Path.Substring(idx, 2), 16);

                        Color back = Color.FromArgb(r, g, b);
                        this.pictureBox.Image = null;
                        this.pictureBox.BackColor = back;
                    }
                    else
                    {
                        System.IO.FileInfo File = new System.IO.FileInfo(Path.StartsWith("file:\\\\\\") ? Path.Substring("file:\\\\\\".Length) : Path);
                        if (File.Exists)
                        {
                            this.MediumType = DiversityWorkbench.Forms.FormFunctions.MediaType(this.textBoxResourcePath.Text);
                            switch (this.MediumType)
                            {
                                case DiversityWorkbench.Forms.FormFunctions.Medium.Unknown:
                                    // try to open medium in web browser
                                    this.userControlWebView.Navigate(this.textBoxResourcePath.Text);
                                    break;
                                case DiversityWorkbench.Forms.FormFunctions.Medium.Image:
                                    this.pictureBox.SizeMode = PictureBoxSizeMode.Zoom; // Standard setting
                                    this.pictureBox.Load(this.textBoxResourcePath.Text);
                                    break;
                                case DiversityWorkbench.Forms.FormFunctions.Medium.Audio:
                                case DiversityWorkbench.Forms.FormFunctions.Medium.Video:
                                    this.userControlPlayer.MediaPlayer.Source = new Uri(this.textBoxResourcePath.Text);
                                    break;
                                default:
                                    this.pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
                                    this.pictureBox.Image = (System.Drawing.Bitmap)uci.DefaultIconUnknown;
                                    this.pictureBox.BackColor = SystemColors.Window;
                                    this.userControlWebView.Url = null;
                                    break;
                            }
                        }
                        else
                        {
                            this.pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
                            this.pictureBox.Image = (System.Drawing.Bitmap)uci.DefaultIconWrongPath;
                            this.MediumType = DiversityWorkbench.Forms.FormFunctions.Medium.Image;
                        }
                    }
                }
                catch (System.Exception)
                {
                    OK = false;
                }
                finally
                {
                    if (!OK)
                    {
                        this.textBoxResourcePath.Text = string.Empty;
                        this.pictureBox.Image = null;
                        MediumType = DiversityWorkbench.Forms.FormFunctions.Medium.Image;
                    }
                }
            }
            this.splitContainerResource.ResumeLayout();
            this.Cursor = saveCursor;
        }

        private void updateControl(WebRequestResult requestResult, string requestUri, int resourceSize, bool ignoreSize)
        {
            if (this.InvokeRequired)
            {
                // Method called by other thread: Use invoke!
                upbdateControlDelegate del = new upbdateControlDelegate(updateControl);
                this.Invoke(del, new object[] { requestResult, requestUri, resourceSize, ignoreSize });
            }
            else
            {
                try
                {
                    // Check requested uri
                    if (this.textBoxResourcePath.Text != requestUri)
                        return;

                    // Check response
                    if (requestResult == WebRequestResult.tooBig)
                    {
                        // Resource is too big
                        this.buttonLoadBig.Visible = true;

                        // Update button tool tip
                        string toolTipText = this.toolTip.GetToolTip(this.buttonLoadBig);
                        int idx = toolTipText.IndexOf(" (");
                        if (idx > -1)
                            toolTipText = toolTipText.Remove(idx);
                        toolTipText += string.Format(" ({0} KB)", resourceSize);
                        this.toolTip.SetToolTip(this.buttonLoadBig, toolTipText);

                        // Set image
                        this.pictureBox.SizeMode = PictureBoxSizeMode.Zoom; // Standard setting
                        this.pictureBox.Image = (System.Drawing.Bitmap)uci.DefaultIconTooBig;
                        this.MediumType = DiversityWorkbench.Forms.FormFunctions.Medium.Image;
                    }
                    else if (requestResult == WebRequestResult.success)
                    {
                        // Success, load resource
                        this.MediumType = DiversityWorkbench.Forms.FormFunctions.MediaType(this.textBoxResourcePath.Text);
                        switch (this._MediumType)
                        {
                            case DiversityWorkbench.Forms.FormFunctions.Medium.Unknown:
                                // try to open medium in web browser
                                this.userControlWebView.Navigate(this.textBoxResourcePath.Text);
                                break;
                            case DiversityWorkbench.Forms.FormFunctions.Medium.Image:
                                // open image window
                                string message = "";
                                this.pictureBox.Image = DiversityWorkbench.Forms.FormFunctions.BitmapFromWeb(this.textBoxResourcePath.Text, ref message, ignoreSize);
                                if (message == "")
                                    this.pictureBox.SizeMode = PictureBoxSizeMode.Zoom; // Standard setting
                                else
                                {
                                    // Loading failed: Send link to browser
                                    this.MediumType = DiversityWorkbench.Forms.FormFunctions.Medium.Unknown;
                                    this.userControlWebView.Url = new Uri(this.textBoxResourcePath.Text);
                                    //this.pictureBox.Image = (System.Drawing.Bitmap)uci.DefaultIconWrongPath;
                                }
                                break;
                            case DiversityWorkbench.Forms.FormFunctions.Medium.Audio:
                            case DiversityWorkbench.Forms.FormFunctions.Medium.Video:
                                // open media window
                                this.userControlPlayer.MediaPlayer.Source = new Uri(this.textBoxResourcePath.Text);
                                break;
                            default:
                                this.userControlWebView.Url = null;
                                break;
                        }
                    }
                    else if (requestResult == WebRequestResult.errorNoAccess)
                    {
                        // Access failed, set "broken" image
                        this._NoAccessPath = this.textBoxResourcePath.Text;
                        this.pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
                        this.pictureBox.Image = (System.Drawing.Bitmap)uci.DefaultIconWrongPath;
                        this.MediumType = DiversityWorkbench.Forms.FormFunctions.Medium.Image;
                        OnWebRequestTimedOut(EventArgs.Empty);
                    }
                    else if (requestResult == WebRequestResult.errorSslNotPossible)
                    {
                        // Send link to browser...
                        // Toni 20200715: Since some time Wiki links like
                        // https://upload.wikimedia.org/wikipedia/commons/thumb/d/d7/Eisenhut_blau.JPG/500px-Eisenhut_blau.JPG
                        // return an error concerning the ssl connection. Anyway, the browser can show the picture..
                        this.MediumType = DiversityWorkbench.Forms.FormFunctions.Medium.Unknown;
                        this.userControlWebView.Url = new Uri(this.textBoxResourcePath.Text);
                    }
                    else
                    {
                        // Unknown problem, clear image
                        this.textBoxResourcePath.Text = string.Empty;
                        this.pictureBox.Image = null;
                        MediumType = DiversityWorkbench.Forms.FormFunctions.Medium.Image;
                    }
                }
                catch (System.Exception ex)
                {
                    this.textBoxResourcePath.Text = string.Empty;
                    this.pictureBox.Image = null;
                    MediumType = DiversityWorkbench.Forms.FormFunctions.Medium.Image;
                }

                // Inform application about end of loading
                OnWebRequestEnded(EventArgs.Empty);

                // Set open button state
                this.buttonOpenResource.Enabled = this.textBoxResourcePath.Text != null && this.textBoxResourcePath.Text.Length > 0 && !this.textBoxResourcePath.Text.ToLower().StartsWith("color://#");
            }
        }
        #endregion

        #region Events
        private void buttonPlay_Click(object sender, EventArgs e)
        {
            if (this.labelMediumState.Text == MediumState.stopped.ToString())
            {
                PlayMedia();
            }
            else
            {
                TogglePause();
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            StopPlayback();
        }

        private void buttonLoadBig_Click(object sender, EventArgs e)
        {
            loadResource(true);
        }

        private void buttonOpenResource_Click(object sender, EventArgs e)
        {
            if (this.textBoxResourcePath.Text.Length == 0 || this.textBoxResourcePath.Text.ToLower().StartsWith("color://#")) return;
            StopPlayback();
            if (this._NoAccessPath != "")
            {
                // Open system browser
                System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(this._NoAccessPath);
                info.UseShellExecute = true;
                System.Diagnostics.Process.Start(info);
            }
            else
            {
                // Open internal form
                DiversityWorkbench.Forms.FormImage f = new DiversityWorkbench.Forms.FormImage(this.textBoxResourcePath.Text);
                f.Show();
            }
        }

        private void pictureBox_DoubleClick(object sender, EventArgs e)
        {
            this.buttonOpenResource_Click(null, null);
        }
        #endregion
    }
}
