namespace DiversityWorkbench.Forms
{
    partial class FormWebBrowser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWebBrowser));
            this.panelURL = new System.Windows.Forms.Panel();
            this.textBoxURL = new System.Windows.Forms.TextBox();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonBack = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonForward = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonBrowse = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonHome = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonFile = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSystemBrowser = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonScriptOn = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonScriptOff = new System.Windows.Forms.ToolStripButton();
            this.panelDialog = new System.Windows.Forms.Panel();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.userControlWebView = new DiversityWorkbench.UserControls.UserControlWebView();
            this.panelURL.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.panelDialog.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelURL
            // 
            this.panelURL.Controls.Add(this.textBoxURL);
            this.panelURL.Controls.Add(this.toolStrip);
            this.panelURL.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelURL.Location = new System.Drawing.Point(0, 0);
            this.panelURL.Name = "panelURL";
            this.panelURL.Size = new System.Drawing.Size(767, 23);
            this.panelURL.TabIndex = 1;
            // 
            // textBoxURL
            // 
            this.textBoxURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxURL.Location = new System.Drawing.Point(116, 0);
            this.textBoxURL.Name = "textBoxURL";
            this.textBoxURL.Size = new System.Drawing.Size(651, 20);
            this.textBoxURL.TabIndex = 0;
            this.textBoxURL.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxURL_KeyDown);
            // 
            // toolStrip
            // 
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonBack,
            this.toolStripButtonForward,
            this.toolStripButtonBrowse,
            this.toolStripButtonHome,
            this.toolStripButtonFile,
            this.toolStripButtonSystemBrowser,
            this.toolStripButtonScriptOn,
            this.toolStripButtonScriptOff});
            this.toolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(116, 23);
            this.toolStrip.TabIndex = 2;
            this.toolStrip.Text = "toolStrip1";
            this.toolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip_ItemClicked);
            // 
            // toolStripButtonBack
            // 
            this.toolStripButtonBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonBack.Image = global::DiversityWorkbench.ResourceWorkbench.ArrowBackward;
            this.toolStripButtonBack.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonBack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonBack.Name = "toolStripButtonBack";
            this.toolStripButtonBack.Size = new System.Drawing.Size(23, 16);
            this.toolStripButtonBack.Text = "go to previous URL";
            this.toolStripButtonBack.Click += new System.EventHandler(this.toolStripButtonBack_Click);
            // 
            // toolStripButtonForward
            // 
            this.toolStripButtonForward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonForward.Image = global::DiversityWorkbench.ResourceWorkbench.ArrowForward;
            this.toolStripButtonForward.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonForward.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonForward.Name = "toolStripButtonForward";
            this.toolStripButtonForward.Size = new System.Drawing.Size(23, 16);
            this.toolStripButtonForward.Text = "go to next URL";
            this.toolStripButtonForward.Click += new System.EventHandler(this.toolStripButtonForward_Click);
            // 
            // toolStripButtonBrowse
            // 
            this.toolStripButtonBrowse.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonBrowse.Image = global::DiversityWorkbench.ResourceWorkbench.Browse;
            this.toolStripButtonBrowse.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonBrowse.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonBrowse.Name = "toolStripButtonBrowse";
            this.toolStripButtonBrowse.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonBrowse.Text = "Find URL";
            this.toolStripButtonBrowse.Click += new System.EventHandler(this.toolStripButtonBrowse_Click);
            // 
            // toolStripButtonHome
            // 
            this.toolStripButtonHome.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonHome.Image = global::DiversityWorkbench.Properties.Resources.Home;
            this.toolStripButtonHome.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonHome.Name = "toolStripButtonHome";
            this.toolStripButtonHome.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonHome.Text = "Home";
            this.toolStripButtonHome.Click += new System.EventHandler(this.toolStripButtonHome_Click);
            // 
            // toolStripButtonFile
            // 
            this.toolStripButtonFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFile.Image = global::DiversityWorkbench.Properties.Resources.OpenFolder;
            this.toolStripButtonFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFile.Name = "toolStripButtonFile";
            this.toolStripButtonFile.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonFile.Text = "Open file";
            this.toolStripButtonFile.Visible = false;
            this.toolStripButtonFile.Click += new System.EventHandler(this.toolStripButtonFile_Click);
            // 
            // toolStripButtonSystemBrowser
            // 
            this.toolStripButtonSystemBrowser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSystemBrowser.Image = global::DiversityWorkbench.Properties.Resources.ExternerBrowser;
            this.toolStripButtonSystemBrowser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSystemBrowser.Name = "toolStripButtonSystemBrowser";
            this.toolStripButtonSystemBrowser.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonSystemBrowser.Text = "Open external browser";
            this.toolStripButtonSystemBrowser.Visible = false;
            this.toolStripButtonSystemBrowser.Click += new System.EventHandler(this.toolStripButtonSystemBrowser_Click);
            // 
            // toolStripButtonScriptOn
            // 
            this.toolStripButtonScriptOn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonScriptOn.Image = global::DiversityWorkbench.Properties.Resources.JSno;
            this.toolStripButtonScriptOn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonScriptOn.Name = "toolStripButtonScriptOn";
            this.toolStripButtonScriptOn.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonScriptOn.Text = "Activate scripts";
            this.toolStripButtonScriptOn.Click += new System.EventHandler(this.toolStripButtonScriptOn_Click);
            // 
            // toolStripButtonScriptOff
            // 
            this.toolStripButtonScriptOff.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonScriptOff.Image = global::DiversityWorkbench.Properties.Resources.JS;
            this.toolStripButtonScriptOff.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonScriptOff.Name = "toolStripButtonScriptOff";
            this.toolStripButtonScriptOff.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonScriptOff.Text = "Deactivate scripts";
            this.toolStripButtonScriptOff.Visible = false;
            this.toolStripButtonScriptOff.Click += new System.EventHandler(this.toolStripButtonScriptOff_Click);
            // 
            // panelDialog
            // 
            this.panelDialog.Controls.Add(this.buttonOK);
            this.panelDialog.Controls.Add(this.buttonCancel);
            this.panelDialog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelDialog.Location = new System.Drawing.Point(0, 513);
            this.panelDialog.Margin = new System.Windows.Forms.Padding(0);
            this.panelDialog.Name = "panelDialog";
            this.panelDialog.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.panelDialog.Size = new System.Drawing.Size(767, 25);
            this.panelDialog.TabIndex = 2;
            // 
            // buttonOK
            // 
            this.buttonOK.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonOK.Location = new System.Drawing.Point(692, 2);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonCancel.Location = new System.Drawing.Point(0, 2);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "all Files|*.*";
            this.openFileDialog.Title = "Open file";
            // 
            // userControlWebView
            // 
            this.userControlWebView.AllowScripting = true;
            this.userControlWebView.BackColor = System.Drawing.SystemColors.Window;
            this.userControlWebView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlWebView.Location = new System.Drawing.Point(0, 23);
            this.userControlWebView.Margin = new System.Windows.Forms.Padding(0);
            this.userControlWebView.Name = "userControlWebView";
            this.userControlWebView.ScriptErrorsSuppressed = false;
            this.userControlWebView.Size = new System.Drawing.Size(767, 490);
            this.userControlWebView.TabIndex = 3;
            this.userControlWebView.Url = new System.Uri("about:blank", System.UriKind.Absolute);
            this.userControlWebView.NavigationCompleted += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs>(this.userControlWebView_NavigationCompleted);
            // 
            // FormWebBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 538);
            this.Controls.Add(this.userControlWebView);
            this.Controls.Add(this.panelDialog);
            this.Controls.Add(this.panelURL);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormWebBrowser";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "... Browser";
            this.panelURL.ResumeLayout(false);
            this.panelURL.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.panelDialog.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelURL;
        private System.Windows.Forms.TextBox textBoxURL;
        private System.Windows.Forms.Panel panelDialog;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButtonBack;
        private System.Windows.Forms.ToolStripButton toolStripButtonForward;
        private System.Windows.Forms.ToolStripButton toolStripButtonBrowse;
        private System.Windows.Forms.ToolStripButton toolStripButtonHome;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.ToolStripButton toolStripButtonFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripButton toolStripButtonSystemBrowser;
        private UserControls.UserControlWebView userControlWebView;
        private System.Windows.Forms.ToolStripButton toolStripButtonScriptOn;
        private System.Windows.Forms.ToolStripButton toolStripButtonScriptOff;
    }
}