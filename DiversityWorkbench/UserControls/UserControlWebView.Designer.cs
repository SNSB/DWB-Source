
namespace DiversityWorkbench.UserControls
{
    partial class UserControlWebView
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            webView2 = new Microsoft.Web.WebView2.WinForms.WebView2();
            ((System.ComponentModel.ISupportInitialize)webView2).BeginInit();
            SuspendLayout();
            // 
            // webView
            // 
            webView2.AllowExternalDrop = true;
            webView2.BackColor = System.Drawing.SystemColors.Window;
            webView2.CreationProperties = null;
            webView2.DefaultBackgroundColor = System.Drawing.Color.White;
            webView2.Dock = System.Windows.Forms.DockStyle.Fill;
            webView2.Location = new System.Drawing.Point(0, 0);
            webView2.Margin = new System.Windows.Forms.Padding(0);
            webView2.Name = "webView2";
            webView2.Size = new System.Drawing.Size(404, 173);
            webView2.TabIndex = 0;
            webView2.ZoomFactor = 1D;
            webView2.NavigationStarting += WebView2NavigationStarting;
            webView2.NavigationCompleted += WebView2NavigationCompleted;
            webView2.WebMessageReceived += WebView2WebMessageReceived;
            // 
            // UserControlWebView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(webView2);
            Margin = new System.Windows.Forms.Padding(0);
            Name = "UserControlWebView";
            Size = new System.Drawing.Size(404, 173);
            ((System.ComponentModel.ISupportInitialize)webView2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 webView2;
    }
}
