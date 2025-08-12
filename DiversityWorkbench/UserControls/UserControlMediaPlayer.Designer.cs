namespace DiversityWorkbench.UserControls
{
    partial class UserControlMediaPlayer
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlMediaPlayer));
            this.buttonPlay = new System.Windows.Forms.Button();
            this.textBoxFile = new System.Windows.Forms.TextBox();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.buttonExternalBrowser = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.labelMediumState = new System.Windows.Forms.Label();
            this.elementHostMedia = new System.Windows.Forms.Integration.ElementHost();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonPlay
            // 
            resources.ApplyResources(this.buttonPlay, "buttonPlay");
            this.buttonPlay.FlatAppearance.BorderSize = 0;
            this.buttonPlay.Image = global::DiversityWorkbench.Properties.Resources.ArrowNext;
            this.buttonPlay.Name = "buttonPlay";
            this.helpProvider.SetShowHelp(this.buttonPlay, ((bool)(resources.GetObject("buttonPlay.ShowHelp"))));
            this.toolTip.SetToolTip(this.buttonPlay, resources.GetString("buttonPlay.ToolTip"));
            this.buttonPlay.UseVisualStyleBackColor = true;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // textBoxFile
            // 
            this.textBoxFile.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxFile, 5);
            resources.ApplyResources(this.textBoxFile, "textBoxFile");
            this.textBoxFile.Name = "textBoxFile";
            this.textBoxFile.ReadOnly = true;
            this.helpProvider.SetShowHelp(this.textBoxFile, ((bool)(resources.GetObject("textBoxFile.ShowHelp"))));
            // 
            // tableLayoutPanelMain
            // 
            resources.ApplyResources(this.tableLayoutPanelMain, "tableLayoutPanelMain");
            this.tableLayoutPanelMain.Controls.Add(this.buttonExternalBrowser, 3, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonStop, 1, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonPlay, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxFile, 0, 4);
            this.tableLayoutPanelMain.Controls.Add(this.pictureBox, 4, 0);
            this.tableLayoutPanelMain.Controls.Add(this.labelMediumState, 2, 0);
            this.tableLayoutPanelMain.Controls.Add(this.elementHostMedia, 0, 3);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.helpProvider.SetShowHelp(this.tableLayoutPanelMain, ((bool)(resources.GetObject("tableLayoutPanelMain.ShowHelp"))));
            // 
            // buttonExternalBrowser
            // 
            resources.ApplyResources(this.buttonExternalBrowser, "buttonExternalBrowser");
            this.buttonExternalBrowser.FlatAppearance.BorderSize = 0;
            this.buttonExternalBrowser.Image = global::DiversityWorkbench.Properties.Resources.ExternerBrowserSmall;
            this.buttonExternalBrowser.Name = "buttonExternalBrowser";
            this.helpProvider.SetShowHelp(this.buttonExternalBrowser, ((bool)(resources.GetObject("buttonExternalBrowser.ShowHelp"))));
            this.toolTip.SetToolTip(this.buttonExternalBrowser, resources.GetString("buttonExternalBrowser.ToolTip"));
            this.buttonExternalBrowser.UseVisualStyleBackColor = true;
            this.buttonExternalBrowser.Click += new System.EventHandler(this.buttonExternalBrowser_Click);
            // 
            // buttonStop
            // 
            resources.ApplyResources(this.buttonStop, "buttonStop");
            this.buttonStop.FlatAppearance.BorderSize = 0;
            this.buttonStop.Image = global::DiversityWorkbench.Properties.Resources.Stop;
            this.buttonStop.Name = "buttonStop";
            this.helpProvider.SetShowHelp(this.buttonStop, ((bool)(resources.GetObject("buttonStop.ShowHelp"))));
            this.toolTip.SetToolTip(this.buttonStop, resources.GetString("buttonStop.ToolTip"));
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // pictureBox
            // 
            resources.ApplyResources(this.pictureBox, "pictureBox");
            this.pictureBox.Name = "pictureBox";
            this.helpProvider.SetShowHelp(this.pictureBox, ((bool)(resources.GetObject("pictureBox.ShowHelp"))));
            this.pictureBox.TabStop = false;
            // 
            // labelMediumState
            // 
            resources.ApplyResources(this.labelMediumState, "labelMediumState");
            this.labelMediumState.Name = "labelMediumState";
            this.helpProvider.SetShowHelp(this.labelMediumState, ((bool)(resources.GetObject("labelMediumState.ShowHelp"))));
            // 
            // elementHostMedia
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.elementHostMedia, 5);
            resources.ApplyResources(this.elementHostMedia, "elementHostMedia");
            this.elementHostMedia.Name = "elementHostMedia";
            this.elementHostMedia.Child = null;
            // 
            // UserControlMediaPlayer
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.helpProvider.SetHelpKeyword(this, resources.GetString("$this.HelpKeyword"));
            this.helpProvider.SetHelpNavigator(this, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("$this.HelpNavigator"))));
            this.Name = "UserControlMediaPlayer";
            this.helpProvider.SetShowHelp(this, ((bool)(resources.GetObject("$this.ShowHelp"))));
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonPlay;
        private System.Windows.Forms.TextBox textBoxFile;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Label labelMediumState;
        private System.Windows.Forms.Button buttonExternalBrowser;
        private System.Windows.Forms.Integration.ElementHost elementHostMedia;
    }
}
