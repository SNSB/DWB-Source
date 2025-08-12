namespace DiversityWorkbench.UserControls
{
    partial class UserControlGoogleMaps
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
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.userControlWebView = new DiversityWorkbench.UserControls.UserControlWebView();
            this.SuspendLayout();
            // 
            // webBrowser
            // 
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Left;
            this.webBrowser.Location = new System.Drawing.Point(0, 0);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(72, 214);
            this.webBrowser.TabIndex = 0;
            // 
            // userControlWebView
            // 
            this.userControlWebView.AllowScripting = false;
            this.userControlWebView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlWebView.Location = new System.Drawing.Point(72, 0);
            this.userControlWebView.Margin = new System.Windows.Forms.Padding(0);
            this.userControlWebView.Name = "userControlWebView";
            this.userControlWebView.ScriptErrorsSuppressed = false;
            this.userControlWebView.Size = new System.Drawing.Size(245, 214);
            this.userControlWebView.TabIndex = 1;
            this.userControlWebView.Url = new System.Uri("about:blank", System.UriKind.Absolute);
            this.userControlWebView.MessageReceived += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs>(this.userControlWebView_MessageReceived);
            // 
            // UserControlGoogleMaps
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.userControlWebView);
            this.Controls.Add(this.webBrowser);
            this.Name = "UserControlGoogleMaps";
            this.Size = new System.Drawing.Size(317, 214);
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.WebBrowser webBrowser;
        private UserControls.UserControlWebView userControlWebView;
    }
}
