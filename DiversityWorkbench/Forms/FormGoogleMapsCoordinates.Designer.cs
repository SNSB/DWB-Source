namespace DiversityWorkbench.Forms
{
    partial class FormGoogleMapsCoordinates
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

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGoogleMapsCoordinates));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.buttonSetStartCoordinates = new System.Windows.Forms.Button();
            this.labelLocality = new System.Windows.Forms.Label();
            this.textBoxLocality = new System.Windows.Forms.TextBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.splitContainerBrowser = new System.Windows.Forms.SplitContainer();
            this.userControlWebView = new DiversityWorkbench.UserControls.UserControlWebView();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerBrowser)).BeginInit();
            this.splitContainerBrowser.Panel1.SuspendLayout();
            this.splitContainerBrowser.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 6;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.buttonSetStartCoordinates, 3, 1);
            this.tableLayoutPanel.Controls.Add(this.labelLocality, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.textBoxLocality, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonSearch, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonCancel, 4, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonOK, 5, 1);
            this.tableLayoutPanel.Controls.Add(this.splitContainerBrowser, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(801, 628);
            this.tableLayoutPanel.TabIndex = 2;
            // 
            // buttonSetStartCoordinates
            // 
            this.buttonSetStartCoordinates.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonSetStartCoordinates.Image = global::DiversityWorkbench.Properties.Resources.CoordinateCrossTransparent;
            this.buttonSetStartCoordinates.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSetStartCoordinates.Location = new System.Drawing.Point(511, 598);
            this.buttonSetStartCoordinates.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.buttonSetStartCoordinates.Name = "buttonSetStartCoordinates";
            this.buttonSetStartCoordinates.Size = new System.Drawing.Size(128, 27);
            this.buttonSetStartCoordinates.TabIndex = 2;
            this.buttonSetStartCoordinates.Text = "Set start coordinates";
            this.buttonSetStartCoordinates.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.buttonSetStartCoordinates, "Set the default start coordinates of the form to the current position");
            this.buttonSetStartCoordinates.UseVisualStyleBackColor = true;
            this.buttonSetStartCoordinates.Click += new System.EventHandler(this.buttonSetStartCoordinates_Click);
            // 
            // labelLocality
            // 
            this.labelLocality.AutoSize = true;
            this.labelLocality.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLocality.Location = new System.Drawing.Point(3, 603);
            this.labelLocality.Margin = new System.Windows.Forms.Padding(3, 8, 0, 0);
            this.labelLocality.Name = "labelLocality";
            this.labelLocality.Size = new System.Drawing.Size(46, 25);
            this.labelLocality.TabIndex = 3;
            this.labelLocality.Text = "Locality:";
            // 
            // textBoxLocality
            // 
            this.textBoxLocality.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLocality.Location = new System.Drawing.Point(49, 601);
            this.textBoxLocality.Margin = new System.Windows.Forms.Padding(0, 6, 0, 3);
            this.textBoxLocality.Name = "textBoxLocality";
            this.textBoxLocality.Size = new System.Drawing.Size(421, 20);
            this.textBoxLocality.TabIndex = 4;
            this.toolTip.SetToolTip(this.textBoxLocality, "Enter the name of the locality you want to find");
            // 
            // buttonSearch
            // 
            this.buttonSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSearch.Image = global::DiversityWorkbench.Properties.Resources.Find;
            this.buttonSearch.Location = new System.Drawing.Point(473, 598);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(25, 27);
            this.buttonSearch.TabIndex = 5;
            this.toolTip.SetToolTip(this.buttonSearch, "Search for the locality (uses Google Maps)");
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCancel.Location = new System.Drawing.Point(652, 598);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(60, 27);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.toolTip.SetToolTip(this.buttonCancel, "Close form without saving the corrdinates");
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOK.Image = global::DiversityWorkbench.Properties.Resources.Localisation;
            this.buttonOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonOK.Location = new System.Drawing.Point(718, 598);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(80, 27);
            this.buttonOK.TabIndex = 7;
            this.buttonOK.Text = "OK";
            this.toolTip.SetToolTip(this.buttonOK, "Set coordinates to the center of the current map");
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // splitContainerBrowser
            // 
            this.tableLayoutPanel.SetColumnSpan(this.splitContainerBrowser, 6);
            this.splitContainerBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerBrowser.Location = new System.Drawing.Point(3, 3);
            this.splitContainerBrowser.Name = "splitContainerBrowser";
            // 
            // splitContainerBrowser.Panel1
            // 
            this.splitContainerBrowser.Panel1.Controls.Add(this.userControlWebView);
            this.splitContainerBrowser.Panel2Collapsed = true;
            this.splitContainerBrowser.Size = new System.Drawing.Size(795, 589);
            this.splitContainerBrowser.SplitterDistance = 265;
            this.splitContainerBrowser.TabIndex = 8;
            // 
            // userControlWebView
            // 
            this.userControlWebView.AllowScripting = true;
            this.userControlWebView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlWebView.Location = new System.Drawing.Point(0, 0);
            this.userControlWebView.Margin = new System.Windows.Forms.Padding(0);
            this.userControlWebView.Name = "userControlWebView";
            this.userControlWebView.ScriptErrorsSuppressed = false;
            this.userControlWebView.Size = new System.Drawing.Size(795, 589);
            this.userControlWebView.TabIndex = 0;
            this.userControlWebView.Url = new System.Uri("about:blank", System.UriKind.Absolute);
            this.userControlWebView.MessageReceived += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs>(this.userControlWebView_MessageReceived);
            // 
            // FormGoogleMapsCoordinates
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(801, 628);
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormGoogleMapsCoordinates";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " Coordinates via Google Maps";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.splitContainerBrowser.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerBrowser)).EndInit();
            this.splitContainerBrowser.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button buttonSetStartCoordinates;
        private System.Windows.Forms.Label labelLocality;
        private System.Windows.Forms.TextBox textBoxLocality;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.SplitContainer splitContainerBrowser;
        private UserControls.UserControlWebView userControlWebView;
    }
}