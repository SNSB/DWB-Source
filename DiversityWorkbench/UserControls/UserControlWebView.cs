using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        // Lock object for thread-safe folder initialization (needed for multiple instances in one form sharing the userfolder)
        private static readonly object _folderLock = new object();
        // share the user data folder across all instances of UserControlWebView in the same process, save it to environment, so also wpfsamlingplot can use it
        private const string WebViewUserDataFolderEnvironmentVariable = "DIVERSITYWORKBENCH_WEBVIEW2_USER_DATA_FOLDER";
        #endregion

        #region Parameter
        private bool _NavigationCompleted = false;

        private bool _isInitialized = false;
        private Task _initializationTask = null;
        private readonly SemaphoreSlim _initLock = new SemaphoreSlim(1, 1);
        private Queue<Action> _pendingNavigations = new Queue<Action>();

        private bool _AllowScripting = false;
        public bool AllowScripting
        {
            get => _AllowScripting;
            set
            {
                _AllowScripting = value;
                if (this.webView2?.CoreWebView2 != null)
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
            get => this.webView2?.Source;
            set
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => this.Navigate(value)));
                }
                else
                {
                    this.Navigate(value);
                }
            }
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
                    _creationProperties.UserDataFolder = GetUserDataFolder();
                    //DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile($"WebView2 UserDataFolder: {_creationProperties.UserDataFolder}");
                }
                return _creationProperties;
            }
            set
            {
                _creationProperties = value;
            }
        }

        private string GetUserDataFolder()
        {
            // Use lock to ensure thread-safe initialization within the same process
            lock (_folderLock)
            {
                // Reuse
                if (_UserDataFolder != null)
                    return _UserDataFolder;

                var environmentFolder = Environment.GetEnvironmentVariable(WebViewUserDataFolderEnvironmentVariable);
                if (!string.IsNullOrWhiteSpace(environmentFolder))
                {
                    _UserDataFolder = environmentFolder;
                    return _UserDataFolder;
                }

                _UserDataFolder = CreateUserDataFolder();

                Environment.SetEnvironmentVariable(
                    WebViewUserDataFolderEnvironmentVariable,
                    _UserDataFolder,
                    EnvironmentVariableTarget.Process);

                return _UserDataFolder;
            }
        }
        private string CreateUserDataFolder()
        {
            var processId = System.Diagnostics.Process.GetCurrentProcess().Id;
            var processName = GetSafeFolderName(System.Diagnostics.Process.GetCurrentProcess().ProcessName);

            var possiblePaths = new[]
            {
         Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DiversityWorkbench", "NETWebView", processName, $"Session_{processId}"),
         Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DiversityWorkbench", "NETWebView", processName, $"Session_{processId}"),
         Path.Combine(Path.GetTempPath(), "DiversityWorkbench", "NETWebView", processName, $"Session_{processId}")
    };

            foreach (var path in possiblePaths)
            {
                try
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var testFile = Path.Combine(path, "test.tmp");
                    File.WriteAllText(testFile, "test");
                    File.Delete(testFile);

                    return path;
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile($"Failed to use path {path}: {ex.Message}");
                }
            }

            var fallbackPath = Path.Combine(Path.GetTempPath(), "DiversityWorkbench", "NETWebView", processName, "Retry_" + Guid.NewGuid().ToString());
            try
            {
                Directory.CreateDirectory(fallbackPath);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile($"Failed to create fallback folder: {ex.Message}");
            }

            return fallbackPath;
        }
        private string GetSafeFolderName(string folderName)
        {
            if (string.IsNullOrWhiteSpace(folderName))
                folderName = "Application";

            foreach (var invalidChar in Path.GetInvalidFileNameChars())
            {
                folderName = folderName.Replace(invalidChar, '_');
            }

            return folderName;
        }

        #region Construction
        public UserControlWebView()
        {
            // Initialize control
            InitializeComponent();
            webView2.CreationProperties = CreationProperties;

            // Handle the form/control load to initialize WebView2
            //this.Load += UserControlWebView_Load;
        }

        //private async void UserControlWebView_Load(object sender, EventArgs e)
        //{
        //    if (!DesignMode && !_isInitialized)
        //    {
        //        await EnsureInitializedAsync();
        //    }

        //}

        public UserControlWebView(CoreWebView2CreationProperties creationProperties = null)
        {
            this.CreationProperties = creationProperties;
            // Initialize control
            InitializeComponent();
            //this.Load += UserControlWebView_Load;
        }

        private async Task EnsureInitializedAsync()
        {
            if (_isInitialized)
                return;

            // Check if already initializing BEFORE acquiring lock
            Task existingTask = null;
            await _initLock.WaitAsync();
            try
            {
                if (_isInitialized) // Double-check after acquiring lock
                    return;

                // Check if already initializing
                if (_initializationTask != null)
                {
                    existingTask = _initializationTask;
                }
                else
                {
                    _initializationTask = InitializeAsync();
                }
            }
            finally
            {
                _initLock.Release();
            }

            // Wait outside the lock to avoid blocking other calls
            if (existingTask != null)
            {
                await existingTask;
            }
            else
            {
                await _initializationTask;

                // Only set initialized and process navigations if we were the one initializing
                await _initLock.WaitAsync();
                try
                {
                    if (!_isInitialized)
                    {
                        _isInitialized = true;
                        ProcessPendingNavigations();
                    }
                }
                finally
                {
                    _initLock.Release();
                }
            }
        }
        public async Task InitializeAsync()
        {
            try
            {
                // DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile($"Starting WebView2 initialization. UserDataFolder: {CreationProperties.UserDataFolder}");

                await webView2.EnsureCoreWebView2Async();

                // DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("WebView2 initialization completed successfully");
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile($"WebView2 COM Exception during initialization. HRESULT: 0x{ex.HResult:X8}, Message: {ex.Message}, UserDataFolder: {CreationProperties.UserDataFolder}");

                // Try with a different user data folder
                try
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("Retrying with alternative user data folder...");
                    var alternativePath = Path.Combine(Path.GetTempPath(), "DiversityWorkbench", "NETwebView_Retry_" + Guid.NewGuid().ToString());
                    Directory.CreateDirectory(alternativePath);
                    _creationProperties.UserDataFolder = alternativePath;
                    webView2.CreationProperties = _creationProperties;

                    await Task.Delay(500); // Small delay before retry
                    await webView2.EnsureCoreWebView2Async();

                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("WebView2 initialization succeeded with alternative folder");
                }
                catch (Exception retryEx)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile($"WebView2 retry failed: {retryEx.Message}");
                    throw new InvalidOperationException($"Unable to initialize web browser after retry. Original error: {ex.Message}", ex);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile($"WebView2 initialization failed with access denied: {ex.Message}. UserDataFolder: {CreationProperties.UserDataFolder}");
                throw new InvalidOperationException("Unable to initialize web browser. Please check application permissions.", ex);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile($"WebView2 initialization failed: {ex.Message}");
                throw;
            }
        }

        private void ProcessPendingNavigations()
        {
            while (_pendingNavigations.Count > 0)
            {
                var action = _pendingNavigations.Dequeue();
                try
                {
                    // Ensure webView2 is initialized before invoking
                    if (webView2 != null && webView2.CoreWebView2 != null)
                    {
                        action?.Invoke();
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile($"Error processing pending navigation: {ex.Message}");
                }
            }
        }

        #endregion

        #region Public
        public void Navigate(Uri Url)
        {
            if (Url == null)
                Url = new Uri("about:blank");
            if (_isInitialized && webView2?.CoreWebView2 != null)
            {
                // Already initialized, navigate immediately
                this.webView2.Source = Url;
            }
            else
            {
                // Capture the URL in the lambda to avoid closure issues
                Uri capturedUrl = Url;
                // Queue the navigation and ensure initialization
                _pendingNavigations.Enqueue(() =>
                {
                    if (webView2?.CoreWebView2 != null)
                    {
                        this.webView2.Source = capturedUrl;
                    }
                });
                _ = EnsureInitializedAsync(); // Fire and forget
            }
        }

        public void Navigate(string address)
        {
            if (string.IsNullOrEmpty(address))
                address = "about:blank";

            Uri url;
            try
            {
                // Try to parse as URI first
                if (Uri.TryCreate(address, UriKind.Absolute, out Uri parsedUri))
                {
                    url = parsedUri;
                }
                // Check if it's a local file path
                else if (File.Exists(address))
                {
                    url = new Uri(Path.GetFullPath(address));
                }
                else
                {
                    // Try to get full path and check again
                    try
                    {
                        var fullPath = Path.GetFullPath(address);
                        if (File.Exists(fullPath))
                        {
                            url = new Uri(fullPath);
                        }
                        else
                        {
                            // Fallback: try to create URI anyway
                            url = new Uri(address);
                        }
                    }
                    catch
                    {
                        url = new Uri(address);
                    }
                }
            }
            catch (UriFormatException)
            {
                url = new Uri("about:blank");
            }

            this.Navigate(url);
        }

        public void NavigateToDocument(string html)
        {
            if (_isInitialized && webView2?.CoreWebView2 != null)
            {
                this.webView2.CoreWebView2.NavigateToString(html);
            }
            else
            {
                _pendingNavigations.Enqueue(() =>
                {
                    if (webView2?.CoreWebView2 != null)
                    {
                        this.webView2.CoreWebView2.NavigateToString(html);
                    }
                });
                _ = EnsureInitializedAsync(); // Fire and forget
            }
        }

        public void NavigateBack()
        {
            if (_isInitialized && webView2?.CoreWebView2 != null)
            {
                this.webView2.GoBack();
            }
        }

        public void NavigateForward()
        {
            if (_isInitialized && webView2?.CoreWebView2 != null)
            {
                this.webView2.GoForward();
            }
        }

        public Task<string> ExecuteScriptAsync(string javaScript)
        {
            if (_NavigationCompleted && _isInitialized && webView2?.CoreWebView2 != null)
                return this.webView2.ExecuteScriptAsync(javaScript);
            else
                return Task.FromResult<string>(null);
        }

        public async void CallJavaScript(string javaScript)
        {
            if (!_isInitialized)
                await EnsureInitializedAsync();

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
