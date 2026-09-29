using Microsoft.Web.WebView2.WinForms;
using System;
using System.IO;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormPrintPreview : Form
    {
        private WebView2 _webView;
        private string _previewUrl;
        private bool _autoOpenPrintPreview;


        public FormPrintPreview(string previewUrl, bool autoOpenPrintPreview)
        {
            _previewUrl = previewUrl;
            _autoOpenPrintPreview = autoOpenPrintPreview;

            this.Text = "Print Preview";
            this.Size = new System.Drawing.Size(800, 600);
            _webView = new WebView2
            {
                Dock = DockStyle.Fill
            };

            // CRITICAL FIX: Set CreationProperties with proper user data folder BEFORE adding to controls
            _webView.CreationProperties = new CoreWebView2CreationProperties
            {
                UserDataFolder = GetUserDataFolder()
            };

            this.Controls.Add(_webView);
            Load += async (sender, e) =>
            {
                try
                {
                    await _webView.EnsureCoreWebView2Async();
                    // THEN: Navigate to the URL
                    if (!string.IsNullOrEmpty(_previewUrl))
                    {
                        _webView.Source = new Uri(_previewUrl);
                    }
                    //_webView.Source = new Uri(previewUrl);


                    CreateToolbar();
                    this.Text = "Print";
                    this.Icon = global::DiversityWorkbench.Properties.Resources.Print1;
                    this.Size = new System.Drawing.Size(1000, 700);
                    this.StartPosition = FormStartPosition.CenterParent;

                    if (_autoOpenPrintPreview)
                    {
                        // Subscribe to NavigationCompleted to know when page is loaded
                        _webView.NavigationCompleted += (s, args) =>
                        {
                            if (args.IsSuccess)
                            {
                                // Wait a moment for the page to render
                                System.Threading.Tasks.Task.Delay(500).ContinueWith(_ =>
                                {
                                    this.Invoke(new Action(() => OpenPrintDialog()));
                                });
                            }
                        };
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile($"WebView2 access denied: {ex.Message}");
                    MessageBox.Show(
                        "Unable to initialize the print preview. Please check application permissions.\n\n",
                        "Access Denied",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    this.Close();
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile($"WebView2 initialization error: {ex.Message}");
                    MessageBox.Show(
                        "Unable to initialize the print preview.\n\nError: " + ex.Message,
                        "Initialization Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    this.Close();
                }
            };
        }
        private string GetUserDataFolder()
        {
            // Try multiple locations in order of preference
            var possiblePaths = new[]
            {
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DiversityWorkbench", "NETwebView"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DiversityWorkbench", "NETwebView"),
            Path.Combine(Path.GetTempPath(), "DiversityWorkbench", "NETwebView")
        };

            foreach (var path in possiblePaths)
            {
                try
                {
                    // Try to create the directory
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    // Test write permission
                    var testFile = Path.Combine(path, $"test_{Guid.NewGuid()}.tmp");
                    File.WriteAllText(testFile, "test");
                    File.Delete(testFile);

                    // Success - return this path
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile($"Using WebView2 user data folder: {path}");
                    return path;
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile($"Cannot use path {path}: {ex.Message}");
                }
            }

            // Last resort fallback
            var fallbackPath = Path.Combine(Path.GetTempPath(), "DiversityWorkbench", "NETwebView", Guid.NewGuid().ToString());
            Directory.CreateDirectory(fallbackPath);
            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile($"Using fallback WebView2 user data folder: {fallbackPath}");
            return fallbackPath;
        }

        private void OpenPrintDialog()
        {
            try
            {
                if (_webView?.CoreWebView2 != null)
                {
                    _webView.CoreWebView2.ExecuteScriptAsync("window.print();");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error printing: " + ex.Message);
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                MessageBox.Show("Print is temporarily unavailable.", "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void PrintButton_Click(object sender, EventArgs e)
        {
            try
            {
                OpenPrintDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening the print dialog: " + ex.Message, "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateToolbar()
        {
            ToolStrip toolStrip = new ToolStrip();
            toolStrip.Dock = DockStyle.Top;

            ToolStripButton printButton = new ToolStripButton("Print Preview");
            printButton.DisplayStyle = ToolStripItemDisplayStyle.Image | ToolStripItemDisplayStyle.Text;
            printButton.Image = global::DiversityWorkbench.ResourceWorkbench.Print;
            printButton.Click += PrintButton_Click;

            toolStrip.Items.Add(printButton);
            this.Controls.Add(toolStrip);
            this.Controls.SetChildIndex(toolStrip, 0);
        }

        //    private UserControlWebView userControlWebView;
        //    private bool _autoOpenPrintDialog = false;
        //    private bool _printDialogOpened = false;

        //    public FormPrintPreview(string htmlPath, bool autoOpenPrintDialog = false)
        //    {
        //        InitializeComponent();

        //        _autoOpenPrintDialog = autoOpenPrintDialog;

        //        // Initialize UserControlWebView
        //        userControlWebView = new UserControlWebView();
        //        userControlWebView.Dock = DockStyle.Fill;
        //        userControlWebView.NavigationCompleted += UserControlWebView_NavigationCompleted;
        //        this.Controls.Add(userControlWebView);


        //        // Set window properties
        //        this.Text = "Print Preview";
        //        this.Icon = global::DiversityWorkbench.Properties.Resources.Print1;
        //        this.Size = new System.Drawing.Size(1000, 700);
        //        this.StartPosition = FormStartPosition.CenterParent;

        //        // If auto-opening print, start invisible
        //        if (_autoOpenPrintDialog)
        //        {
        //            this.Visible = false;
        //        }
        //        // Load the HTML file
        //        if (!string.IsNullOrEmpty(htmlPath))
        //        {
        //            try
        //            {
        //                userControlWebView.Navigate(htmlPath);
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show("Error loading print preview: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            }
        //        }
        //    }

        //    private void UserControlWebView_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        //    {
        //        if (_autoOpenPrintDialog && !_printDialogOpened)
        //        {
        //            _printDialogOpened = true;

        //            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        //            timer.Interval = 300;
        //            timer.Tick += (s, args) =>
        //            {
        //                timer.Stop();
        //                timer.Dispose();
        //                OpenPrintDialog();
        //            };
        //            timer.Start();
        //        }
        //    }
        //    protected override void OnLoad(EventArgs e)
        //    {
        //        base.OnLoad(e);

        //        // Only create toolbar if NOT auto-opening print dialog
        //        if (!_autoOpenPrintDialog)
        //        {
        //            CreateToolbar();
        //        }
        //    }
        //    private void CreateToolbar()
        //    {
        //        ToolStrip toolStrip = new ToolStrip();
        //        toolStrip.Dock = DockStyle.Top;

        //        ToolStripButton printButton = new ToolStripButton("Print");
        //        printButton.DisplayStyle = ToolStripItemDisplayStyle.Image | ToolStripItemDisplayStyle.Text;
        //        printButton.Image = global::DiversityWorkbench.ResourceWorkbench.Print;
        //        printButton.Click += PrintButton_Click;

        //        toolStrip.Items.Add(printButton);
        //        this.Controls.Add(toolStrip);
        //        this.Controls.SetChildIndex(toolStrip, 0);
        //    }

        //    private void PrintButton_Click(object sender, EventArgs e)
        //    {
        //        OpenPrintDialog();
        //    }
        //    private void OpenPrintDialog()
        //    {
        //        try
        //        {
        //            userControlWebView.CallJavaScript("window.print();");
        //        }
        //        catch (Exception ex)
        //        {
        //            System.Diagnostics.Debug.WriteLine("Error printing: " + ex.Message);
        //            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //            MessageBox.Show("Print is temporarily unavailable.", "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //    }

        //    protected override void OnFormClosed(FormClosedEventArgs e)
        //    {
        //        base.OnFormClosed(e);
        //        userControlWebView?.Dispose();
        //    }

        //}
    }
}
