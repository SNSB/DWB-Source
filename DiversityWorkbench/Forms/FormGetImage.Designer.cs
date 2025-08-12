namespace DiversityWorkbench.Forms
{
    partial class FormGetImage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGetImage));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.buttonOpenFile = new System.Windows.Forms.Button();
            this.textBoxImageFilePath = new System.Windows.Forms.TextBox();
            this.labelHeader = new System.Windows.Forms.Label();
            this.buttonOpenBrowser = new System.Windows.Forms.Button();
            this.buttonUpload = new System.Windows.Forms.Button();
            this.checkBoxRenameForMediaService = new System.Windows.Forms.CheckBox();
            this.labelSize = new System.Windows.Forms.Label();
            this.labelSizeInMB = new System.Windows.Forms.Label();
            this.labelUploadNotPossible = new System.Windows.Forms.Label();
            this.splitContainerResource = new System.Windows.Forms.SplitContainer();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.splitContainerNonImage = new System.Windows.Forms.SplitContainer();
            this.panelWebBrowser = new System.Windows.Forms.Panel();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.userControlMediaPlayer = new DiversityWorkbench.UserControls.UserControlMediaPlayer();
            this.labelNameForMediaService = new System.Windows.Forms.Label();
            this.labelDirectoryForMediaService = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.imageListMedia = new System.Windows.Forms.ImageList(this.components);
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.userControlXMLTreeEXIF = new DiversityWorkbench.UserControls.UserControlXMLTree();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerResource)).BeginInit();
            this.splitContainerResource.Panel1.SuspendLayout();
            this.splitContainerResource.Panel2.SuspendLayout();
            this.splitContainerResource.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerNonImage)).BeginInit();
            this.splitContainerNonImage.Panel1.SuspendLayout();
            this.splitContainerNonImage.Panel2.SuspendLayout();
            this.splitContainerNonImage.SuspendLayout();
            this.panelWebBrowser.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            resources.ApplyResources(this.tableLayoutPanelMain, "tableLayoutPanelMain");
            this.tableLayoutPanelMain.Controls.Add(this.buttonOpenFile, 3, 0);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxImageFilePath, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonOpenBrowser, 4, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonUpload, 5, 0);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxRenameForMediaService, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.labelSize, 4, 2);
            this.tableLayoutPanelMain.Controls.Add(this.labelSizeInMB, 4, 3);
            this.tableLayoutPanelMain.Controls.Add(this.labelUploadNotPossible, 0, 4);
            this.tableLayoutPanelMain.Controls.Add(this.splitContainerResource, 0, 5);
            this.tableLayoutPanelMain.Controls.Add(this.labelNameForMediaService, 3, 3);
            this.tableLayoutPanelMain.Controls.Add(this.labelDirectoryForMediaService, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.userControlXMLTreeEXIF, 0, 6);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            // 
            // buttonOpenFile
            // 
            resources.ApplyResources(this.buttonOpenFile, "buttonOpenFile");
            this.buttonOpenFile.Name = "buttonOpenFile";
            this.toolTip.SetToolTip(this.buttonOpenFile, resources.GetString("buttonOpenFile.ToolTip"));
            this.buttonOpenFile.Click += new System.EventHandler(this.buttonOpenFile_Click);
            // 
            // textBoxImageFilePath
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxImageFilePath, 6);
            resources.ApplyResources(this.textBoxImageFilePath, "textBoxImageFilePath");
            this.textBoxImageFilePath.Name = "textBoxImageFilePath";
            this.textBoxImageFilePath.TextChanged += new System.EventHandler(this.textBoxImageFilePath_TextChanged);
            // 
            // labelHeader
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.labelHeader, 3);
            resources.ApplyResources(this.labelHeader, "labelHeader");
            this.labelHeader.Name = "labelHeader";
            // 
            // buttonOpenBrowser
            // 
            this.buttonOpenBrowser.Image = global::DiversityWorkbench.ResourceWorkbench.Browse;
            resources.ApplyResources(this.buttonOpenBrowser, "buttonOpenBrowser");
            this.buttonOpenBrowser.Name = "buttonOpenBrowser";
            this.toolTip.SetToolTip(this.buttonOpenBrowser, resources.GetString("buttonOpenBrowser.ToolTip"));
            this.buttonOpenBrowser.UseVisualStyleBackColor = true;
            this.buttonOpenBrowser.Click += new System.EventHandler(this.buttonOpenBrowser_Click);
            // 
            // buttonUpload
            // 
            resources.ApplyResources(this.buttonUpload, "buttonUpload");
            this.buttonUpload.Image = global::DiversityWorkbench.Properties.Resources.ImageUpload;
            this.buttonUpload.Name = "buttonUpload";
            this.toolTip.SetToolTip(this.buttonUpload, resources.GetString("buttonUpload.ToolTip"));
            this.buttonUpload.UseVisualStyleBackColor = true;
            this.buttonUpload.Click += new System.EventHandler(this.buttonUpload_Click);
            // 
            // checkBoxRenameForMediaService
            // 
            resources.ApplyResources(this.checkBoxRenameForMediaService, "checkBoxRenameForMediaService");
            this.tableLayoutPanelMain.SetColumnSpan(this.checkBoxRenameForMediaService, 4);
            this.checkBoxRenameForMediaService.Name = "checkBoxRenameForMediaService";
            this.checkBoxRenameForMediaService.UseVisualStyleBackColor = true;
            this.checkBoxRenameForMediaService.CheckedChanged += new System.EventHandler(this.checkBoxRenameForMediaService_CheckedChanged);
            // 
            // labelSize
            // 
            resources.ApplyResources(this.labelSize, "labelSize");
            this.tableLayoutPanelMain.SetColumnSpan(this.labelSize, 2);
            this.labelSize.Name = "labelSize";
            // 
            // labelSizeInMB
            // 
            resources.ApplyResources(this.labelSizeInMB, "labelSizeInMB");
            this.tableLayoutPanelMain.SetColumnSpan(this.labelSizeInMB, 2);
            this.labelSizeInMB.Name = "labelSizeInMB";
            // 
            // labelUploadNotPossible
            // 
            resources.ApplyResources(this.labelUploadNotPossible, "labelUploadNotPossible");
            this.tableLayoutPanelMain.SetColumnSpan(this.labelUploadNotPossible, 6);
            this.labelUploadNotPossible.Name = "labelUploadNotPossible";
            // 
            // splitContainerResource
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.splitContainerResource, 6);
            resources.ApplyResources(this.splitContainerResource, "splitContainerResource");
            this.splitContainerResource.Name = "splitContainerResource";
            // 
            // splitContainerResource.Panel1
            // 
            this.splitContainerResource.Panel1.Controls.Add(this.pictureBox);
            // 
            // splitContainerResource.Panel2
            // 
            this.splitContainerResource.Panel2.Controls.Add(this.splitContainerNonImage);
            // 
            // pictureBox
            // 
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.pictureBox, "pictureBox");
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.TabStop = false;
            // 
            // splitContainerNonImage
            // 
            resources.ApplyResources(this.splitContainerNonImage, "splitContainerNonImage");
            this.splitContainerNonImage.Name = "splitContainerNonImage";
            // 
            // splitContainerNonImage.Panel1
            // 
            this.splitContainerNonImage.Panel1.Controls.Add(this.panelWebBrowser);
            // 
            // splitContainerNonImage.Panel2
            // 
            this.splitContainerNonImage.Panel2.Controls.Add(this.userControlMediaPlayer);
            // 
            // panelWebBrowser
            // 
            this.panelWebBrowser.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelWebBrowser.Controls.Add(this.webBrowser);
            resources.ApplyResources(this.panelWebBrowser, "panelWebBrowser");
            this.panelWebBrowser.Name = "panelWebBrowser";
            // 
            // webBrowser
            // 
            resources.ApplyResources(this.webBrowser, "webBrowser");
            this.webBrowser.Name = "webBrowser";
            // 
            // userControlMediaPlayer
            // 
            resources.ApplyResources(this.userControlMediaPlayer, "userControlMediaPlayer");
            this.userControlMediaPlayer.File = null;
            this.userControlMediaPlayer.Name = "userControlMediaPlayer";
            // 
            // labelNameForMediaService
            // 
            resources.ApplyResources(this.labelNameForMediaService, "labelNameForMediaService");
            this.labelNameForMediaService.Name = "labelNameForMediaService";
            // 
            // labelDirectoryForMediaService
            // 
            resources.ApplyResources(this.labelDirectoryForMediaService, "labelDirectoryForMediaService");
            this.tableLayoutPanelMain.SetColumnSpan(this.labelDirectoryForMediaService, 3);
            this.labelDirectoryForMediaService.Name = "labelDirectoryForMediaService";
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // imageListMedia
            // 
            this.imageListMedia.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMedia.ImageStream")));
            this.imageListMedia.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListMedia.Images.SetKeyName(0, "Audio.gif");
            this.imageListMedia.Images.SetKeyName(1, "Video.gif");
            this.imageListMedia.Images.SetKeyName(2, "Image.gif");
            this.imageListMedia.Images.SetKeyName(3, "Unknown.gif");
            // 
            // userControlDialogPanel
            // 
            resources.ApplyResources(this.userControlDialogPanel, "userControlDialogPanel");
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            // 
            // userControlXMLTreeEXIF
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.userControlXMLTreeEXIF, 6);
            resources.ApplyResources(this.userControlXMLTreeEXIF, "userControlXMLTreeEXIF");
            this.userControlXMLTreeEXIF.Name = "userControlXMLTreeEXIF";
            this.userControlXMLTreeEXIF.XML = "";
            // 
            // FormGetImage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Controls.Add(this.userControlDialogPanel);
            this.Name = "FormGetImage";
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.splitContainerResource.Panel1.ResumeLayout(false);
            this.splitContainerResource.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerResource)).EndInit();
            this.splitContainerResource.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.splitContainerNonImage.Panel1.ResumeLayout(false);
            this.splitContainerNonImage.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerNonImage)).EndInit();
            this.splitContainerNonImage.ResumeLayout(false);
            this.panelWebBrowser.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.TextBox textBoxImageFilePath;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button buttonOpenFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonOpenBrowser;
        private System.Windows.Forms.Button buttonUpload;
        private System.Windows.Forms.CheckBox checkBoxRenameForMediaService;
        private System.Windows.Forms.Label labelSize;
        private System.Windows.Forms.Label labelSizeInMB;
        private System.Windows.Forms.Label labelUploadNotPossible;
        private System.Windows.Forms.SplitContainer splitContainerResource;
        private System.Windows.Forms.Panel panelWebBrowser;
        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.SplitContainer splitContainerNonImage;
        private DiversityWorkbench.UserControls.UserControlMediaPlayer userControlMediaPlayer;
        private System.Windows.Forms.ImageList imageListMedia;
        private System.Windows.Forms.Label labelNameForMediaService;
        private System.Windows.Forms.Label labelDirectoryForMediaService;
        private System.Windows.Forms.HelpProvider helpProvider;
        private UserControls.UserControlXMLTree userControlXMLTreeEXIF;
    }
}