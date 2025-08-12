namespace DiversityWorkbench.UserControls
{
    partial class UserControlImage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlImage));
            this.splitContainerMedia = new System.Windows.Forms.SplitContainer();
            this.splitContainerImage = new System.Windows.Forms.SplitContainer();
            this.panelImage = new System.Windows.Forms.Panel();
            this.labelErrorMessage = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanelButtons = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxImagePath = new System.Windows.Forms.TextBox();
            this.toolStripImage = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonZoomOut = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonZoomIn = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonZoomSector = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton100 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonZoomAdapt = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonFlipHorizontal = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonFlipVertical = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRotateRight = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRotateLeft = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAutorotate = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonOpenInNewForm = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMarkArea = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonForceCanonicalPathAndQuery = new System.Windows.Forms.ToolStripButton();
            this.numericUpDownZoom = new System.Windows.Forms.NumericUpDown();
            this.labelZoom = new System.Windows.Forms.Label();
            this.labelMaxSize = new System.Windows.Forms.Label();
            this.numericUpDownMaxSize = new System.Windows.Forms.NumericUpDown();
            this.buttonZoomView = new System.Windows.Forms.Button();
            this.labelImagePath = new System.Windows.Forms.Label();
            this.panelForWpfControl = new System.Windows.Forms.Panel();
            this.toolStripImageEditor = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonImageEditorAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonImageEditorSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonImageEditorDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripTextBoxImageEditorFilename = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabelImageEditorUrl = new System.Windows.Forms.ToolStripLabel();
            this.splitContainerNoImage = new System.Windows.Forms.SplitContainer();
            this.userControlMediaPlayer = new UserControlMediaPlayer();
            this.webBrowserNoMedia = new DiversityWorkbench.UserControls.UserControlWebView();
            this.tableLayoutPanelMediaNoImage = new System.Windows.Forms.TableLayoutPanel();
            this.buttonScript = new System.Windows.Forms.Button();
            this.buttonMediaNoImageDownload = new System.Windows.Forms.Button();
            this.textBoxMediaNoImageURL = new System.Windows.Forms.TextBox();
            this.labelMediaNoImageURL = new System.Windows.Forms.Label();
            this.buttonMediaNoImageOpenInNewWindow = new System.Windows.Forms.Button();
            this.buttonExternalBrowser = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.imageListMediaTypes = new System.Windows.Forms.ImageList(this.components);
            this.imageListErrors = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMedia)).BeginInit();
            this.splitContainerMedia.Panel1.SuspendLayout();
            this.splitContainerMedia.Panel2.SuspendLayout();
            this.splitContainerMedia.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerImage)).BeginInit();
            this.splitContainerImage.Panel1.SuspendLayout();
            this.splitContainerImage.Panel2.SuspendLayout();
            this.splitContainerImage.SuspendLayout();
            this.panelImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.tableLayoutPanelButtons.SuspendLayout();
            this.toolStripImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxSize)).BeginInit();
            this.toolStripImageEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerNoImage)).BeginInit();
            this.splitContainerNoImage.Panel1.SuspendLayout();
            this.splitContainerNoImage.Panel2.SuspendLayout();
            this.splitContainerNoImage.SuspendLayout();
            this.tableLayoutPanelMediaNoImage.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerMedia
            // 
            resources.ApplyResources(this.splitContainerMedia, "splitContainerMedia");
            this.splitContainerMedia.Name = "splitContainerMedia";
            // 
            // splitContainerMedia.Panel1
            // 
            this.splitContainerMedia.Panel1.Controls.Add(this.splitContainerImage);
            resources.ApplyResources(this.splitContainerMedia.Panel1, "splitContainerMedia.Panel1");
            this.helpProvider.SetShowHelp(this.splitContainerMedia.Panel1, ((bool)(resources.GetObject("splitContainerMedia.Panel1.ShowHelp"))));
            // 
            // splitContainerMedia.Panel2
            // 
            this.splitContainerMedia.Panel2.Controls.Add(this.splitContainerNoImage);
            resources.ApplyResources(this.splitContainerMedia.Panel2, "splitContainerMedia.Panel2");
            this.helpProvider.SetShowHelp(this.splitContainerMedia.Panel2, ((bool)(resources.GetObject("splitContainerMedia.Panel2.ShowHelp"))));
            this.helpProvider.SetShowHelp(this.splitContainerMedia, ((bool)(resources.GetObject("splitContainerMedia.ShowHelp"))));
            // 
            // splitContainerImage
            // 
            resources.ApplyResources(this.splitContainerImage, "splitContainerImage");
            this.splitContainerImage.Name = "splitContainerImage";
            // 
            // splitContainerImage.Panel1
            // 
            this.splitContainerImage.Panel1.Controls.Add(this.panelImage);
            this.splitContainerImage.Panel1.Controls.Add(this.tableLayoutPanelButtons);
            this.helpProvider.SetShowHelp(this.splitContainerImage.Panel1, ((bool)(resources.GetObject("splitContainerImage.Panel1.ShowHelp"))));
            // 
            // splitContainerImage.Panel2
            // 
            this.splitContainerImage.Panel2.Controls.Add(this.panelForWpfControl);
            this.splitContainerImage.Panel2.Controls.Add(this.toolStripImageEditor);
            this.helpProvider.SetShowHelp(this.splitContainerImage.Panel2, ((bool)(resources.GetObject("splitContainerImage.Panel2.ShowHelp"))));
            this.splitContainerImage.Panel2Collapsed = true;
            this.helpProvider.SetShowHelp(this.splitContainerImage, ((bool)(resources.GetObject("splitContainerImage.ShowHelp"))));
            // 
            // panelImage
            // 
            resources.ApplyResources(this.panelImage, "panelImage");
            this.panelImage.Controls.Add(this.labelErrorMessage);
            this.panelImage.Controls.Add(this.pictureBox);
            this.panelImage.Name = "panelImage";
            this.helpProvider.SetShowHelp(this.panelImage, ((bool)(resources.GetObject("panelImage.ShowHelp"))));
            // 
            // labelErrorMessage
            // 
            resources.ApplyResources(this.labelErrorMessage, "labelErrorMessage");
            this.labelErrorMessage.Name = "labelErrorMessage";
            this.helpProvider.SetShowHelp(this.labelErrorMessage, ((bool)(resources.GetObject("labelErrorMessage.ShowHelp"))));
            // 
            // pictureBox
            // 
            resources.ApplyResources(this.pictureBox, "pictureBox");
            this.pictureBox.Name = "pictureBox";
            this.helpProvider.SetShowHelp(this.pictureBox, ((bool)(resources.GetObject("pictureBox.ShowHelp"))));
            this.pictureBox.TabStop = false;
            this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_Paint);
            this.pictureBox.DoubleClick += new System.EventHandler(this.pictureBox_DoubleClick);
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseDown);
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
            this.pictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseUp);
            // 
            // tableLayoutPanelButtons
            // 
            resources.ApplyResources(this.tableLayoutPanelButtons, "tableLayoutPanelButtons");
            this.tableLayoutPanelButtons.Controls.Add(this.textBoxImagePath, 6, 0);
            this.tableLayoutPanelButtons.Controls.Add(this.toolStripImage, 0, 0);
            this.tableLayoutPanelButtons.Controls.Add(this.numericUpDownZoom, 4, 0);
            this.tableLayoutPanelButtons.Controls.Add(this.labelZoom, 3, 0);
            this.tableLayoutPanelButtons.Controls.Add(this.labelMaxSize, 1, 0);
            this.tableLayoutPanelButtons.Controls.Add(this.numericUpDownMaxSize, 2, 0);
            this.tableLayoutPanelButtons.Controls.Add(this.buttonZoomView, 7, 0);
            this.tableLayoutPanelButtons.Controls.Add(this.labelImagePath, 5, 0);
            this.tableLayoutPanelButtons.Name = "tableLayoutPanelButtons";
            this.helpProvider.SetShowHelp(this.tableLayoutPanelButtons, ((bool)(resources.GetObject("tableLayoutPanelButtons.ShowHelp"))));
            // 
            // textBoxImagePath
            // 
            resources.ApplyResources(this.textBoxImagePath, "textBoxImagePath");
            this.textBoxImagePath.Name = "textBoxImagePath";
            this.textBoxImagePath.ReadOnly = true;
            this.helpProvider.SetShowHelp(this.textBoxImagePath, ((bool)(resources.GetObject("textBoxImagePath.ShowHelp"))));
            // 
            // toolStripImage
            // 
            this.toolStripImage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonZoomOut,
            this.toolStripButtonZoomIn,
            this.toolStripButtonZoomSector,
            this.toolStripButton100,
            this.toolStripButtonZoomAdapt,
            this.toolStripButtonFlipHorizontal,
            this.toolStripButtonFlipVertical,
            this.toolStripButtonRotateRight,
            this.toolStripButtonRotateLeft,
            this.toolStripButtonAutorotate,
            this.toolStripButtonOpenInNewForm,
            this.toolStripButtonMarkArea,
            this.toolStripButtonForceCanonicalPathAndQuery});
            this.toolStripImage.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            resources.ApplyResources(this.toolStripImage, "toolStripImage");
            this.toolStripImage.Name = "toolStripImage";
            this.helpProvider.SetShowHelp(this.toolStripImage, ((bool)(resources.GetObject("toolStripImage.ShowHelp"))));
            // 
            // toolStripButtonZoomOut
            // 
            this.toolStripButtonZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonZoomOut, "toolStripButtonZoomOut");
            this.toolStripButtonZoomOut.Name = "toolStripButtonZoomOut";
            this.toolStripButtonZoomOut.Click += new System.EventHandler(this.toolStripButtonZoomOut_Click);
            // 
            // toolStripButtonZoomIn
            // 
            this.toolStripButtonZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonZoomIn, "toolStripButtonZoomIn");
            this.toolStripButtonZoomIn.Name = "toolStripButtonZoomIn";
            this.toolStripButtonZoomIn.Click += new System.EventHandler(this.toolStripButtonZoomIn_Click);
            // 
            // toolStripButtonZoomSector
            // 
            this.toolStripButtonZoomSector.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonZoomSector, "toolStripButtonZoomSector");
            this.toolStripButtonZoomSector.Name = "toolStripButtonZoomSector";
            this.toolStripButtonZoomSector.Click += new System.EventHandler(this.toolStripButtonZoomSector_Click);
            // 
            // toolStripButton100
            // 
            this.toolStripButton100.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton100.Image = global::DiversityWorkbench.Properties.Resources.Zoom100;
            resources.ApplyResources(this.toolStripButton100, "toolStripButton100");
            this.toolStripButton100.Name = "toolStripButton100";
            this.toolStripButton100.Click += new System.EventHandler(this.toolStripButton100_Click);
            // 
            // toolStripButtonZoomAdapt
            // 
            this.toolStripButtonZoomAdapt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonZoomAdapt.Image = global::DiversityWorkbench.Properties.Resources.ZoomAdapt;
            resources.ApplyResources(this.toolStripButtonZoomAdapt, "toolStripButtonZoomAdapt");
            this.toolStripButtonZoomAdapt.Name = "toolStripButtonZoomAdapt";
            this.toolStripButtonZoomAdapt.Click += new System.EventHandler(this.toolStripButtonZoomAdapt_Click);
            // 
            // toolStripButtonFlipHorizontal
            // 
            this.toolStripButtonFlipHorizontal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFlipHorizontal.Image = global::DiversityWorkbench.Properties.Resources.FlipHorizontal;
            resources.ApplyResources(this.toolStripButtonFlipHorizontal, "toolStripButtonFlipHorizontal");
            this.toolStripButtonFlipHorizontal.Name = "toolStripButtonFlipHorizontal";
            this.toolStripButtonFlipHorizontal.Click += new System.EventHandler(this.toolStripButtonFlipHorizontal_Click);
            // 
            // toolStripButtonFlipVertical
            // 
            this.toolStripButtonFlipVertical.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFlipVertical.Image = global::DiversityWorkbench.Properties.Resources.FlipVertical;
            resources.ApplyResources(this.toolStripButtonFlipVertical, "toolStripButtonFlipVertical");
            this.toolStripButtonFlipVertical.Name = "toolStripButtonFlipVertical";
            this.toolStripButtonFlipVertical.Click += new System.EventHandler(this.toolStripButtonFlipVertical_Click);
            // 
            // toolStripButtonRotateRight
            // 
            this.toolStripButtonRotateRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRotateRight.Image = global::DiversityWorkbench.Properties.Resources.RotateRight;
            resources.ApplyResources(this.toolStripButtonRotateRight, "toolStripButtonRotateRight");
            this.toolStripButtonRotateRight.Name = "toolStripButtonRotateRight";
            this.toolStripButtonRotateRight.Click += new System.EventHandler(this.toolStripButtonRotateRight_Click);
            // 
            // toolStripButtonRotateLeft
            // 
            this.toolStripButtonRotateLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRotateLeft.Image = global::DiversityWorkbench.Properties.Resources.RotateLeft;
            resources.ApplyResources(this.toolStripButtonRotateLeft, "toolStripButtonRotateLeft");
            this.toolStripButtonRotateLeft.Name = "toolStripButtonRotateLeft";
            this.toolStripButtonRotateLeft.Click += new System.EventHandler(this.toolStripButtonRotateLeft_Click);
            // 
            // toolStripButtonAutorotate
            // 
            this.toolStripButtonAutorotate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAutorotate.Image = global::DiversityWorkbench.Properties.Resources.Transfrom;
            resources.ApplyResources(this.toolStripButtonAutorotate, "toolStripButtonAutorotate");
            this.toolStripButtonAutorotate.Name = "toolStripButtonAutorotate";
            this.toolStripButtonAutorotate.Click += new System.EventHandler(this.toolStripButtonAutorotate_Click);
            // 
            // toolStripButtonOpenInNewForm
            // 
            this.toolStripButtonOpenInNewForm.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonOpenInNewForm.Image = global::DiversityWorkbench.Properties.Resources.NewForm;
            resources.ApplyResources(this.toolStripButtonOpenInNewForm, "toolStripButtonOpenInNewForm");
            this.toolStripButtonOpenInNewForm.Name = "toolStripButtonOpenInNewForm";
            this.toolStripButtonOpenInNewForm.Click += new System.EventHandler(this.toolStripButtonOpenInNewForm_Click);
            // 
            // toolStripButtonMarkArea
            // 
            this.toolStripButtonMarkArea.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMarkArea.Image = global::DiversityWorkbench.Properties.Resources.ImageArea;
            resources.ApplyResources(this.toolStripButtonMarkArea, "toolStripButtonMarkArea");
            this.toolStripButtonMarkArea.Name = "toolStripButtonMarkArea";
            this.toolStripButtonMarkArea.Click += new System.EventHandler(this.toolStripButtonMarkArea_Click);
            // 
            // toolStripButtonForceCanonicalPathAndQuery
            // 
            this.toolStripButtonForceCanonicalPathAndQuery.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.toolStripButtonForceCanonicalPathAndQuery, "toolStripButtonForceCanonicalPathAndQuery");
            this.toolStripButtonForceCanonicalPathAndQuery.Name = "toolStripButtonForceCanonicalPathAndQuery";
            this.toolStripButtonForceCanonicalPathAndQuery.Click += new System.EventHandler(this.toolStripButtonForceCanonicalPathAndQuery_Click);
            // 
            // numericUpDownZoom
            // 
            resources.ApplyResources(this.numericUpDownZoom, "numericUpDownZoom");
            this.numericUpDownZoom.Increment = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numericUpDownZoom.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.numericUpDownZoom.Minimum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numericUpDownZoom.Name = "numericUpDownZoom";
            this.helpProvider.SetShowHelp(this.numericUpDownZoom, ((bool)(resources.GetObject("numericUpDownZoom.ShowHelp"))));
            this.numericUpDownZoom.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownZoom.ValueChanged += new System.EventHandler(this.numericUpDownZoom_ValueChanged);
            // 
            // labelZoom
            // 
            resources.ApplyResources(this.labelZoom, "labelZoom");
            this.labelZoom.Name = "labelZoom";
            this.helpProvider.SetShowHelp(this.labelZoom, ((bool)(resources.GetObject("labelZoom.ShowHelp"))));
            // 
            // labelMaxSize
            // 
            resources.ApplyResources(this.labelMaxSize, "labelMaxSize");
            this.labelMaxSize.Name = "labelMaxSize";
            this.helpProvider.SetShowHelp(this.labelMaxSize, ((bool)(resources.GetObject("labelMaxSize.ShowHelp"))));
            // 
            // numericUpDownMaxSize
            // 
            this.numericUpDownMaxSize.DecimalPlaces = 1;
            resources.ApplyResources(this.numericUpDownMaxSize, "numericUpDownMaxSize");
            this.numericUpDownMaxSize.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDownMaxSize.Name = "numericUpDownMaxSize";
            this.helpProvider.SetShowHelp(this.numericUpDownMaxSize, ((bool)(resources.GetObject("numericUpDownMaxSize.ShowHelp"))));
            this.toolTip.SetToolTip(this.numericUpDownMaxSize, resources.GetString("numericUpDownMaxSize.ToolTip"));
            this.numericUpDownMaxSize.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDownMaxSize.ValueChanged += new System.EventHandler(this.numericUpDownMaxSize_ValueChanged);
            // 
            // buttonZoomView
            // 
            resources.ApplyResources(this.buttonZoomView, "buttonZoomView");
            this.buttonZoomView.Image = global::DiversityWorkbench.Properties.Resources.Zoom;
            this.buttonZoomView.Name = "buttonZoomView";
            this.helpProvider.SetShowHelp(this.buttonZoomView, ((bool)(resources.GetObject("buttonZoomView.ShowHelp"))));
            this.toolTip.SetToolTip(this.buttonZoomView, resources.GetString("buttonZoomView.ToolTip"));
            this.buttonZoomView.UseVisualStyleBackColor = true;
            this.buttonZoomView.Click += new System.EventHandler(this.buttonZoomView_Click);
            // 
            // labelImagePath
            // 
            resources.ApplyResources(this.labelImagePath, "labelImagePath");
            this.labelImagePath.Name = "labelImagePath";
            this.helpProvider.SetShowHelp(this.labelImagePath, ((bool)(resources.GetObject("labelImagePath.ShowHelp"))));
            // 
            // panelForWpfControl
            // 
            resources.ApplyResources(this.panelForWpfControl, "panelForWpfControl");
            this.panelForWpfControl.Name = "panelForWpfControl";
            this.helpProvider.SetShowHelp(this.panelForWpfControl, ((bool)(resources.GetObject("panelForWpfControl.ShowHelp"))));
            // 
            // toolStripImageEditor
            // 
            resources.ApplyResources(this.toolStripImageEditor, "toolStripImageEditor");
            this.toolStripImageEditor.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripImageEditor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonImageEditorAdd,
            this.toolStripButtonImageEditorSave,
            this.toolStripButtonImageEditorDelete,
            this.toolStripTextBoxImageEditorFilename,
            this.toolStripLabelImageEditorUrl});
            this.toolStripImageEditor.Name = "toolStripImageEditor";
            this.helpProvider.SetShowHelp(this.toolStripImageEditor, ((bool)(resources.GetObject("toolStripImageEditor.ShowHelp"))));
            this.toolStripImageEditor.SizeChanged += new System.EventHandler(this.toolStripImageEditor_SizeChanged);
            // 
            // toolStripButtonImageEditorAdd
            // 
            this.toolStripButtonImageEditorAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonImageEditorAdd.Image = global::DiversityWorkbench.Properties.Resources.Add;
            resources.ApplyResources(this.toolStripButtonImageEditorAdd, "toolStripButtonImageEditorAdd");
            this.toolStripButtonImageEditorAdd.Name = "toolStripButtonImageEditorAdd";
            this.toolStripButtonImageEditorAdd.Click += new System.EventHandler(this.toolStripButtonImageEditorAdd_Click);
            // 
            // toolStripButtonImageEditorSave
            // 
            this.toolStripButtonImageEditorSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonImageEditorSave.Image = global::DiversityWorkbench.Properties.Resources.Save;
            resources.ApplyResources(this.toolStripButtonImageEditorSave, "toolStripButtonImageEditorSave");
            this.toolStripButtonImageEditorSave.Name = "toolStripButtonImageEditorSave";
            this.toolStripButtonImageEditorSave.Click += new System.EventHandler(this.toolStripButtonImageEditorSave_Click);
            // 
            // toolStripButtonImageEditorDelete
            // 
            this.toolStripButtonImageEditorDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonImageEditorDelete.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            resources.ApplyResources(this.toolStripButtonImageEditorDelete, "toolStripButtonImageEditorDelete");
            this.toolStripButtonImageEditorDelete.Name = "toolStripButtonImageEditorDelete";
            this.toolStripButtonImageEditorDelete.Click += new System.EventHandler(this.toolStripButtonImageEditorDelete_Click);
            // 
            // toolStripTextBoxImageEditorFilename
            // 
            this.toolStripTextBoxImageEditorFilename.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            resources.ApplyResources(this.toolStripTextBoxImageEditorFilename, "toolStripTextBoxImageEditorFilename");
            this.toolStripTextBoxImageEditorFilename.Name = "toolStripTextBoxImageEditorFilename";
            this.toolStripTextBoxImageEditorFilename.ReadOnly = true;
            // 
            // toolStripLabelImageEditorUrl
            // 
            this.toolStripLabelImageEditorUrl.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabelImageEditorUrl.Name = "toolStripLabelImageEditorUrl";
            resources.ApplyResources(this.toolStripLabelImageEditorUrl, "toolStripLabelImageEditorUrl");
            // 
            // splitContainerNoImage
            // 
            resources.ApplyResources(this.splitContainerNoImage, "splitContainerNoImage");
            this.splitContainerNoImage.Name = "splitContainerNoImage";
            // 
            // splitContainerNoImage.Panel1
            // 
            this.splitContainerNoImage.Panel1.Controls.Add(this.userControlMediaPlayer);
            this.helpProvider.SetShowHelp(this.splitContainerNoImage.Panel1, ((bool)(resources.GetObject("splitContainerNoImage.Panel1.ShowHelp"))));
            // 
            // splitContainerNoImage.Panel2
            // 
            this.splitContainerNoImage.Panel2.Controls.Add(this.webBrowserNoMedia);
            this.splitContainerNoImage.Panel2.Controls.Add(this.tableLayoutPanelMediaNoImage);
            this.helpProvider.SetShowHelp(this.splitContainerNoImage.Panel2, ((bool)(resources.GetObject("splitContainerNoImage.Panel2.ShowHelp"))));
            this.helpProvider.SetShowHelp(this.splitContainerNoImage, ((bool)(resources.GetObject("splitContainerNoImage.ShowHelp"))));
            // 
            // userControlMediaPlayer
            // 
            resources.ApplyResources(this.userControlMediaPlayer, "userControlMediaPlayer");
            this.userControlMediaPlayer.File = null;
            this.userControlMediaPlayer.Name = "userControlMediaPlayer";
            this.helpProvider.SetShowHelp(this.userControlMediaPlayer, ((bool)(resources.GetObject("userControlMediaPlayer.ShowHelp"))));
            // 
            // webBrowserNoMedia
            // 
            this.webBrowserNoMedia.AllowScripting = false;
            resources.ApplyResources(this.webBrowserNoMedia, "webBrowserNoMedia");
            this.webBrowserNoMedia.Name = "webBrowserNoMedia";
            this.webBrowserNoMedia.ScriptErrorsSuppressed = true;
            this.helpProvider.SetShowHelp(this.webBrowserNoMedia, ((bool)(resources.GetObject("webBrowserNoMedia.ShowHelp"))));
            this.webBrowserNoMedia.Url = new System.Uri("about:blank", System.UriKind.Absolute);
            // 
            // tableLayoutPanelMediaNoImage
            // 
            resources.ApplyResources(this.tableLayoutPanelMediaNoImage, "tableLayoutPanelMediaNoImage");
            this.tableLayoutPanelMediaNoImage.Controls.Add(this.buttonScript, 1, 0);
            this.tableLayoutPanelMediaNoImage.Controls.Add(this.buttonMediaNoImageDownload, 4, 0);
            this.tableLayoutPanelMediaNoImage.Controls.Add(this.textBoxMediaNoImageURL, 1, 0);
            this.tableLayoutPanelMediaNoImage.Controls.Add(this.labelMediaNoImageURL, 0, 0);
            this.tableLayoutPanelMediaNoImage.Controls.Add(this.buttonMediaNoImageOpenInNewWindow, 5, 0);
            this.tableLayoutPanelMediaNoImage.Controls.Add(this.buttonExternalBrowser, 3, 0);
            this.tableLayoutPanelMediaNoImage.Name = "tableLayoutPanelMediaNoImage";
            this.helpProvider.SetShowHelp(this.tableLayoutPanelMediaNoImage, ((bool)(resources.GetObject("tableLayoutPanelMediaNoImage.ShowHelp"))));
            // 
            // buttonScript
            // 
            resources.ApplyResources(this.buttonScript, "buttonScript");
            this.buttonScript.Image = global::DiversityWorkbench.Properties.Resources.JSno;
            this.buttonScript.Name = "buttonScript";
            this.helpProvider.SetShowHelp(this.buttonScript, ((bool)(resources.GetObject("buttonScript.ShowHelp"))));
            this.toolTip.SetToolTip(this.buttonScript, resources.GetString("buttonScript.ToolTip"));
            this.buttonScript.UseVisualStyleBackColor = true;
            this.buttonScript.Click += new System.EventHandler(this.buttonScript_Click);
            // 
            // buttonMediaNoImageDownload
            // 
            resources.ApplyResources(this.buttonMediaNoImageDownload, "buttonMediaNoImageDownload");
            this.buttonMediaNoImageDownload.Image = global::DiversityWorkbench.Properties.Resources.SaveSmall;
            this.buttonMediaNoImageDownload.Name = "buttonMediaNoImageDownload";
            this.helpProvider.SetShowHelp(this.buttonMediaNoImageDownload, ((bool)(resources.GetObject("buttonMediaNoImageDownload.ShowHelp"))));
            this.toolTip.SetToolTip(this.buttonMediaNoImageDownload, resources.GetString("buttonMediaNoImageDownload.ToolTip"));
            this.buttonMediaNoImageDownload.UseVisualStyleBackColor = true;
            this.buttonMediaNoImageDownload.Click += new System.EventHandler(this.buttonMediaNoImageDownload_Click);
            // 
            // textBoxMediaNoImageURL
            // 
            this.textBoxMediaNoImageURL.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.textBoxMediaNoImageURL, "textBoxMediaNoImageURL");
            this.textBoxMediaNoImageURL.Name = "textBoxMediaNoImageURL";
            this.textBoxMediaNoImageURL.ReadOnly = true;
            this.helpProvider.SetShowHelp(this.textBoxMediaNoImageURL, ((bool)(resources.GetObject("textBoxMediaNoImageURL.ShowHelp"))));
            // 
            // labelMediaNoImageURL
            // 
            resources.ApplyResources(this.labelMediaNoImageURL, "labelMediaNoImageURL");
            this.labelMediaNoImageURL.Name = "labelMediaNoImageURL";
            this.helpProvider.SetShowHelp(this.labelMediaNoImageURL, ((bool)(resources.GetObject("labelMediaNoImageURL.ShowHelp"))));
            // 
            // buttonMediaNoImageOpenInNewWindow
            // 
            resources.ApplyResources(this.buttonMediaNoImageOpenInNewWindow, "buttonMediaNoImageOpenInNewWindow");
            this.buttonMediaNoImageOpenInNewWindow.Image = global::DiversityWorkbench.Properties.Resources.NewForm;
            this.buttonMediaNoImageOpenInNewWindow.Name = "buttonMediaNoImageOpenInNewWindow";
            this.helpProvider.SetShowHelp(this.buttonMediaNoImageOpenInNewWindow, ((bool)(resources.GetObject("buttonMediaNoImageOpenInNewWindow.ShowHelp"))));
            this.toolTip.SetToolTip(this.buttonMediaNoImageOpenInNewWindow, resources.GetString("buttonMediaNoImageOpenInNewWindow.ToolTip"));
            this.buttonMediaNoImageOpenInNewWindow.UseVisualStyleBackColor = true;
            this.buttonMediaNoImageOpenInNewWindow.Click += new System.EventHandler(this.buttonMediaNoImageOpenInNewWindow_Click);
            // 
            // buttonExternalBrowser
            // 
            resources.ApplyResources(this.buttonExternalBrowser, "buttonExternalBrowser");
            this.buttonExternalBrowser.Image = global::DiversityWorkbench.Properties.Resources.ExternerBrowserSmallSmall;
            this.buttonExternalBrowser.Name = "buttonExternalBrowser";
            this.helpProvider.SetShowHelp(this.buttonExternalBrowser, ((bool)(resources.GetObject("buttonExternalBrowser.ShowHelp"))));
            this.toolTip.SetToolTip(this.buttonExternalBrowser, resources.GetString("buttonExternalBrowser.ToolTip"));
            this.buttonExternalBrowser.UseVisualStyleBackColor = true;
            this.buttonExternalBrowser.Click += new System.EventHandler(this.buttonExternalBrowser_Click);
            // 
            // imageListMediaTypes
            // 
            this.imageListMediaTypes.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMediaTypes.ImageStream")));
            this.imageListMediaTypes.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListMediaTypes.Images.SetKeyName(0, "Audio.gif");
            this.imageListMediaTypes.Images.SetKeyName(1, "Video.gif");
            this.imageListMediaTypes.Images.SetKeyName(2, "Image.gif");
            this.imageListMediaTypes.Images.SetKeyName(3, "Unknown.gif");
            // 
            // imageListErrors
            // 
            this.imageListErrors.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListErrors.ImageStream")));
            this.imageListErrors.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListErrors.Images.SetKeyName(0, "NotFound.ico");
            this.imageListErrors.Images.SetKeyName(1, "TooBig.ICO");
            this.imageListErrors.Images.SetKeyName(2, "WrongPath.ico");
            // 
            // UserControlImage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerMedia);
            this.Name = "UserControlImage";
            this.helpProvider.SetShowHelp(this, ((bool)(resources.GetObject("$this.ShowHelp"))));
            this.splitContainerMedia.Panel1.ResumeLayout(false);
            this.splitContainerMedia.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMedia)).EndInit();
            this.splitContainerMedia.ResumeLayout(false);
            this.splitContainerImage.Panel1.ResumeLayout(false);
            this.splitContainerImage.Panel2.ResumeLayout(false);
            this.splitContainerImage.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerImage)).EndInit();
            this.splitContainerImage.ResumeLayout(false);
            this.panelImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.tableLayoutPanelButtons.ResumeLayout(false);
            this.tableLayoutPanelButtons.PerformLayout();
            this.toolStripImage.ResumeLayout(false);
            this.toolStripImage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxSize)).EndInit();
            this.toolStripImageEditor.ResumeLayout(false);
            this.toolStripImageEditor.PerformLayout();
            this.splitContainerNoImage.Panel1.ResumeLayout(false);
            this.splitContainerNoImage.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerNoImage)).EndInit();
            this.splitContainerNoImage.ResumeLayout(false);
            this.tableLayoutPanelMediaNoImage.ResumeLayout(false);
            this.tableLayoutPanelMediaNoImage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelImage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelButtons;
        private System.Windows.Forms.ToolStrip toolStripImage;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomOut;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomIn;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomSector;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomAdapt;
        private System.Windows.Forms.ToolStripButton toolStripButtonFlipHorizontal;
        private System.Windows.Forms.ToolStripButton toolStripButtonFlipVertical;
        private System.Windows.Forms.ToolStripButton toolStripButtonRotateRight;
        private System.Windows.Forms.ToolStripButton toolStripButtonRotateLeft;
        private System.Windows.Forms.NumericUpDown numericUpDownZoom;
        private System.Windows.Forms.Label labelZoom;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.TextBox textBoxImagePath;
        private System.Windows.Forms.ToolStripButton toolStripButton100;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpenInNewForm;
        private System.Windows.Forms.Label labelMaxSize;
        private System.Windows.Forms.NumericUpDown numericUpDownMaxSize;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelErrorMessage;
        private System.Windows.Forms.SplitContainer splitContainerMedia;
        private DiversityWorkbench.UserControls.UserControlMediaPlayer userControlMediaPlayer;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.SplitContainer splitContainerNoImage;
        private System.Windows.Forms.TextBox textBoxMediaNoImageURL;
        private System.Windows.Forms.Button buttonMediaNoImageDownload;
        private System.Windows.Forms.Label labelMediaNoImageURL;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMediaNoImage;
        private System.Windows.Forms.ImageList imageListMediaTypes;
        private System.Windows.Forms.Button buttonZoomView;
        private System.Windows.Forms.ToolStripButton toolStripButtonMarkArea;
        private System.Windows.Forms.Button buttonMediaNoImageOpenInNewWindow;
        private System.Windows.Forms.ImageList imageListErrors;
        private System.Windows.Forms.SplitContainer splitContainerImage;
        private System.Windows.Forms.ToolStrip toolStripImageEditor;
        private System.Windows.Forms.ToolStripButton toolStripButtonImageEditorAdd;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxImageEditorFilename;
        private System.Windows.Forms.ToolStripLabel toolStripLabelImageEditorUrl;
        private System.Windows.Forms.Panel panelForWpfControl;
        private System.Windows.Forms.ToolStripButton toolStripButtonImageEditorSave;
        private System.Windows.Forms.ToolStripButton toolStripButtonImageEditorDelete;
        private System.Windows.Forms.ToolStripButton toolStripButtonForceCanonicalPathAndQuery;
        private System.Windows.Forms.Label labelImagePath;
        private System.Windows.Forms.Button buttonExternalBrowser;
        private System.Windows.Forms.ToolStripButton toolStripButtonAutorotate;
        private UserControls.UserControlWebView webBrowserNoMedia;
        private System.Windows.Forms.Button buttonScript;
    }
}
