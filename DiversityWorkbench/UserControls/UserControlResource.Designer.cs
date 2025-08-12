namespace DiversityWorkbench.UserControls
{
    partial class UserControlResource
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
            this.tableLayoutPanelPath = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxResourcePath = new System.Windows.Forms.TextBox();
            this.buttonOpenResource = new System.Windows.Forms.Button();
            this.buttonLoadBig = new System.Windows.Forms.Button();
            this.splitContainerResource = new System.Windows.Forms.SplitContainer();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.splitContainerMedia = new System.Windows.Forms.SplitContainer();
            this.userControlWebView = new DiversityWorkbench.UserControls.UserControlWebView();
            this.tableLayoutPanelMedium = new System.Windows.Forms.TableLayoutPanel();
            this.buttonPlay = new System.Windows.Forms.Button();
            this.elementHostMedia = new System.Windows.Forms.Integration.ElementHost();
            this.labelMediumType = new System.Windows.Forms.Label();
            this.labelMediumState = new System.Windows.Forms.Label();
            this.buttonStop = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanelPath.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerResource)).BeginInit();
            this.splitContainerResource.Panel1.SuspendLayout();
            this.splitContainerResource.Panel2.SuspendLayout();
            this.splitContainerResource.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMedia)).BeginInit();
            this.splitContainerMedia.Panel1.SuspendLayout();
            this.splitContainerMedia.Panel2.SuspendLayout();
            this.splitContainerMedia.SuspendLayout();
            this.tableLayoutPanelMedium.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelPath
            // 
            this.tableLayoutPanelPath.ColumnCount = 3;
            this.tableLayoutPanelPath.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPath.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPath.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPath.Controls.Add(this.textBoxResourcePath, 0, 0);
            this.tableLayoutPanelPath.Controls.Add(this.buttonOpenResource, 2, 0);
            this.tableLayoutPanelPath.Controls.Add(this.buttonLoadBig, 1, 0);
            this.tableLayoutPanelPath.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanelPath.Location = new System.Drawing.Point(0, 148);
            this.tableLayoutPanelPath.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelPath.Name = "tableLayoutPanelPath";
            this.tableLayoutPanelPath.RowCount = 1;
            this.tableLayoutPanelPath.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPath.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanelPath.Size = new System.Drawing.Size(356, 23);
            this.tableLayoutPanelPath.TabIndex = 0;
            // 
            // textBoxResourcePath
            // 
            this.textBoxResourcePath.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBoxResourcePath.Location = new System.Drawing.Point(2, 1);
            this.textBoxResourcePath.Margin = new System.Windows.Forms.Padding(2, 1, 1, 2);
            this.textBoxResourcePath.Name = "textBoxResourcePath";
            this.textBoxResourcePath.ReadOnly = true;
            this.textBoxResourcePath.Size = new System.Drawing.Size(307, 20);
            this.textBoxResourcePath.TabIndex = 0;
            // 
            // buttonOpenResource
            // 
            this.buttonOpenResource.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonOpenResource.Image = global::DiversityWorkbench.Properties.Resources.NewForm;
            this.buttonOpenResource.Location = new System.Drawing.Point(333, 0);
            this.buttonOpenResource.Margin = new System.Windows.Forms.Padding(0);
            this.buttonOpenResource.Name = "buttonOpenResource";
            this.buttonOpenResource.Size = new System.Drawing.Size(23, 23);
            this.buttonOpenResource.TabIndex = 1;
            this.toolTip.SetToolTip(this.buttonOpenResource, "Open image in a separate window");
            this.buttonOpenResource.UseVisualStyleBackColor = true;
            this.buttonOpenResource.Click += new System.EventHandler(this.buttonOpenResource_Click);
            // 
            // buttonLoadBig
            // 
            this.buttonLoadBig.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonLoadBig.Image = global::DiversityWorkbench.Properties.Resources.Replace;
            this.buttonLoadBig.Location = new System.Drawing.Point(310, 0);
            this.buttonLoadBig.Margin = new System.Windows.Forms.Padding(0);
            this.buttonLoadBig.Name = "buttonLoadBig";
            this.buttonLoadBig.Size = new System.Drawing.Size(23, 23);
            this.buttonLoadBig.TabIndex = 3;
            this.toolTip.SetToolTip(this.buttonLoadBig, "Click to load big resource");
            this.buttonLoadBig.UseVisualStyleBackColor = true;
            this.buttonLoadBig.Click += new System.EventHandler(this.buttonLoadBig_Click);
            // 
            // splitContainerResource
            // 
            this.splitContainerResource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerResource.Location = new System.Drawing.Point(0, 0);
            this.splitContainerResource.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainerResource.Name = "splitContainerResource";
            // 
            // splitContainerResource.Panel1
            // 
            this.splitContainerResource.Panel1.Controls.Add(this.pictureBox);
            // 
            // splitContainerResource.Panel2
            // 
            this.splitContainerResource.Panel2.Controls.Add(this.splitContainerMedia);
            this.splitContainerResource.Size = new System.Drawing.Size(356, 148);
            this.splitContainerResource.SplitterDistance = 156;
            this.splitContainerResource.TabIndex = 2;
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(156, 148);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.pictureBox.DoubleClick += new System.EventHandler(this.pictureBox_DoubleClick);
            // 
            // splitContainerMedia
            // 
            this.splitContainerMedia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMedia.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMedia.Name = "splitContainerMedia";
            this.splitContainerMedia.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMedia.Panel1
            // 
            this.splitContainerMedia.Panel1.Controls.Add(this.userControlWebView);
            // 
            // splitContainerMedia.Panel2
            // 
            this.splitContainerMedia.Panel2.Controls.Add(this.tableLayoutPanelMedium);
            this.splitContainerMedia.Size = new System.Drawing.Size(196, 148);
            this.splitContainerMedia.SplitterDistance = 73;
            this.splitContainerMedia.TabIndex = 1;
            // 
            // userControlWebView
            // 
            this.userControlWebView.AllowScripting = true;
            this.userControlWebView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlWebView.Location = new System.Drawing.Point(0, 0);
            this.userControlWebView.Margin = new System.Windows.Forms.Padding(0);
            this.userControlWebView.Name = "userControlWebView";
            this.userControlWebView.ScriptErrorsSuppressed = false;
            this.userControlWebView.Size = new System.Drawing.Size(196, 73);
            this.userControlWebView.TabIndex = 0;
            this.userControlWebView.Url = new System.Uri("about:blank", System.UriKind.Absolute);
            // 
            // tableLayoutPanelMedium
            // 
            this.tableLayoutPanelMedium.ColumnCount = 4;
            this.tableLayoutPanelMedium.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelMedium.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelMedium.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanelMedium.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tableLayoutPanelMedium.Controls.Add(this.buttonPlay, 0, 2);
            this.tableLayoutPanelMedium.Controls.Add(this.elementHostMedia, 0, 0);
            this.tableLayoutPanelMedium.Controls.Add(this.labelMediumType, 2, 2);
            this.tableLayoutPanelMedium.Controls.Add(this.labelMediumState, 3, 2);
            this.tableLayoutPanelMedium.Controls.Add(this.buttonStop, 1, 2);
            this.tableLayoutPanelMedium.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMedium.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMedium.Name = "tableLayoutPanelMedium";
            this.tableLayoutPanelMedium.RowCount = 3;
            this.tableLayoutPanelMedium.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMedium.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMedium.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelMedium.Size = new System.Drawing.Size(196, 71);
            this.tableLayoutPanelMedium.TabIndex = 0;
            // 
            // buttonPlay
            // 
            this.buttonPlay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPlay.Image = global::DiversityWorkbench.Properties.Resources.ArrowNext;
            this.buttonPlay.Location = new System.Drawing.Point(3, 43);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(24, 25);
            this.buttonPlay.TabIndex = 1;
            this.toolTip.SetToolTip(this.buttonPlay, "Play the sound or video");
            this.buttonPlay.UseVisualStyleBackColor = true;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // elementHostMedia
            // 
            this.elementHostMedia.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.tableLayoutPanelMedium.SetColumnSpan(this.elementHostMedia, 4);
            this.elementHostMedia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHostMedia.Location = new System.Drawing.Point(3, 3);
            this.elementHostMedia.Name = "elementHostMedia";
            this.tableLayoutPanelMedium.SetRowSpan(this.elementHostMedia, 2);
            this.elementHostMedia.Size = new System.Drawing.Size(190, 34);
            this.elementHostMedia.TabIndex = 1;
            this.elementHostMedia.Text = "elementHost";
            this.elementHostMedia.Child = null;
            // 
            // labelMediumType
            // 
            this.labelMediumType.AutoSize = true;
            this.labelMediumType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMediumType.Location = new System.Drawing.Point(63, 40);
            this.labelMediumType.Name = "labelMediumType";
            this.labelMediumType.Size = new System.Drawing.Size(55, 31);
            this.labelMediumType.TabIndex = 2;
            this.labelMediumType.Text = "Type";
            this.labelMediumType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelMediumState
            // 
            this.labelMediumState.AutoSize = true;
            this.labelMediumState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMediumState.Location = new System.Drawing.Point(124, 40);
            this.labelMediumState.Name = "labelMediumState";
            this.labelMediumState.Size = new System.Drawing.Size(69, 31);
            this.labelMediumState.TabIndex = 3;
            this.labelMediumState.Text = "State";
            this.labelMediumState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonStop
            // 
            this.buttonStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonStop.Image = global::DiversityWorkbench.Properties.Resources.Stop;
            this.buttonStop.Location = new System.Drawing.Point(33, 43);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(24, 25);
            this.buttonStop.TabIndex = 2;
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // UserControlResource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerResource);
            this.Controls.Add(this.tableLayoutPanelPath);
            this.Name = "UserControlResource";
            this.Size = new System.Drawing.Size(356, 171);
            this.tableLayoutPanelPath.ResumeLayout(false);
            this.tableLayoutPanelPath.PerformLayout();
            this.splitContainerResource.Panel1.ResumeLayout(false);
            this.splitContainerResource.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerResource)).EndInit();
            this.splitContainerResource.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.splitContainerMedia.Panel1.ResumeLayout(false);
            this.splitContainerMedia.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMedia)).EndInit();
            this.splitContainerMedia.ResumeLayout(false);
            this.tableLayoutPanelMedium.ResumeLayout(false);
            this.tableLayoutPanelMedium.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPath;
        private System.Windows.Forms.TextBox textBoxResourcePath;
        private System.Windows.Forms.Button buttonOpenResource;
        private System.Windows.Forms.SplitContainer splitContainerResource;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMedium;
        private System.Windows.Forms.Button buttonPlay;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.SplitContainer splitContainerMedia;
        private System.Windows.Forms.Integration.ElementHost elementHostMedia;
        private System.Windows.Forms.Label labelMediumType;
        private System.Windows.Forms.Label labelMediumState;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonLoadBig;
        private UserControlWebView userControlWebView;
    }
}
