using System;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;

namespace DiversityWorkbench.Forms
{
    public partial class FormPrintPreview : Form
    {
        private WebView2 _webView;

        public FormPrintPreview(string previewUrl, bool autoOpenPrintPreview)
        {
            this.Text = "Print Preview";
            this.Size = new System.Drawing.Size(800, 600);
            _webView = new WebView2
            {
                Dock = DockStyle.Fill
            };
            this.Controls.Add(_webView);
            Load += async (sender, e) =>
            {
                await _webView.EnsureCoreWebView2Async();
                _webView.Source = new Uri(previewUrl);

                CreateToolbar();
                this.Text = "Print";
                this.Icon = global::DiversityWorkbench.Properties.Resources.Print1;
                this.Size = new System.Drawing.Size(1000, 700);
                this.StartPosition = FormStartPosition.CenterParent;
                
                if (autoOpenPrintPreview)
                {
                    OpenPrintDialog();
                }
                
            };
        }
        private void OpenPrintDialog()
        {
            try
            {
                _webView?.CoreWebView2.ExecuteScriptAsync("window.print();");
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
