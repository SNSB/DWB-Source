namespace DiversitySamplingPlots.UserControls
{
    partial class UserControlGeography
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
                if (_wpfControl != null)
                {
                    _wpfControl.Dispose();
                    _wpfControl = null;
                }
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
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButtonEditMode = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItemEditModeBrowser = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemEditModeGisNoEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemEditModeGIS = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.numericUpDownZoomLevel = new System.Windows.Forms.NumericUpDown();
            this.splitContainerMaps = new System.Windows.Forms.SplitContainer();
            this.panelWebbrowser = new System.Windows.Forms.Panel();
            this.panelForWpfControl = new System.Windows.Forms.Panel();
            this.elementHost = new System.Windows.Forms.Integration.ElementHost();
            this.checkBoxZoomLevel = new System.Windows.Forms.CheckBox();
            this.buttonReload = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.userControlWebView = new DiversityWorkbench.UserControls.UserControlWebView();
            this.tableLayoutPanelMain.SuspendLayout();
            this.toolStripMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownZoomLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMaps)).BeginInit();
            this.splitContainerMaps.Panel1.SuspendLayout();
            this.splitContainerMaps.Panel2.SuspendLayout();
            this.splitContainerMaps.SuspendLayout();
            this.panelWebbrowser.SuspendLayout();
            this.panelForWpfControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 4;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.toolStripMain, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.numericUpDownZoomLevel, 2, 0);
            this.tableLayoutPanelMain.Controls.Add(this.splitContainerMaps, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxZoomLevel, 1, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonReload, 3, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 2;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(421, 315);
            this.tableLayoutPanelMain.TabIndex = 2;
            // 
            // toolStripMain
            // 
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButtonEditMode,
            this.toolStripButtonSave});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(126, 25);
            this.toolStripMain.TabIndex = 0;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // toolStripDropDownButtonEditMode
            // 
            this.toolStripDropDownButtonEditMode.AutoSize = false;
            this.toolStripDropDownButtonEditMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemEditModeBrowser,
            this.toolStripMenuItemEditModeGisNoEdit,
            this.toolStripMenuItemEditModeGIS});
            this.toolStripDropDownButtonEditMode.Image = global::DiversitySamplingPlots.Properties.Resources.Lupe;
            this.toolStripDropDownButtonEditMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonEditMode.Name = "toolStripDropDownButtonEditMode";
            this.toolStripDropDownButtonEditMode.Size = new System.Drawing.Size(100, 22);
            this.toolStripDropDownButtonEditMode.Text = "Browser";
            // 
            // toolStripMenuItemEditModeBrowser
            // 
            this.toolStripMenuItemEditModeBrowser.Image = global::DiversitySamplingPlots.Properties.Resources.Lupe;
            this.toolStripMenuItemEditModeBrowser.Name = "toolStripMenuItemEditModeBrowser";
            this.toolStripMenuItemEditModeBrowser.Size = new System.Drawing.Size(127, 22);
            this.toolStripMenuItemEditModeBrowser.Text = "Browser";
            this.toolStripMenuItemEditModeBrowser.Click += new System.EventHandler(this.toolStripMenuItemEditModeBrowser_Click);
            // 
            // toolStripMenuItemEditModeGisNoEdit
            // 
            this.toolStripMenuItemEditModeGisNoEdit.Image = global::DiversitySamplingPlots.Properties.Resources.EditNo1;
            this.toolStripMenuItemEditModeGisNoEdit.Name = "toolStripMenuItemEditModeGisNoEdit";
            this.toolStripMenuItemEditModeGisNoEdit.Size = new System.Drawing.Size(127, 22);
            this.toolStripMenuItemEditModeGisNoEdit.Text = "GIS - View";
            this.toolStripMenuItemEditModeGisNoEdit.Click += new System.EventHandler(this.toolStripMenuItemEditModeGisNoEdit_Click);
            // 
            // toolStripMenuItemEditModeGIS
            // 
            this.toolStripMenuItemEditModeGIS.Image = global::DiversitySamplingPlots.Properties.Resources.Edit;
            this.toolStripMenuItemEditModeGIS.Name = "toolStripMenuItemEditModeGIS";
            this.toolStripMenuItemEditModeGIS.Size = new System.Drawing.Size(127, 22);
            this.toolStripMenuItemEditModeGIS.Text = "GIS - Edit";
            this.toolStripMenuItemEditModeGIS.Click += new System.EventHandler(this.toolStripMenuItemEditModeGIS_Click);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSave.Image = global::DiversitySamplingPlots.Properties.Resources.Save;
            this.toolStripButtonSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSave.Text = "Save the changes of the geographical objects";
            this.toolStripButtonSave.ToolTipText = "Save the changes of the geographical objects";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // numericUpDownZoomLevel
            // 
            this.numericUpDownZoomLevel.Dock = System.Windows.Forms.DockStyle.Left;
            this.numericUpDownZoomLevel.Location = new System.Drawing.Point(233, 3);
            this.numericUpDownZoomLevel.Maximum = new decimal(new int[] {
            19,
            0,
            0,
            0});
            this.numericUpDownZoomLevel.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDownZoomLevel.Name = "numericUpDownZoomLevel";
            this.numericUpDownZoomLevel.Size = new System.Drawing.Size(37, 20);
            this.numericUpDownZoomLevel.TabIndex = 3;
            this.numericUpDownZoomLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownZoomLevel.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // splitContainerMaps
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.splitContainerMaps, 4);
            this.splitContainerMaps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMaps.Location = new System.Drawing.Point(3, 29);
            this.splitContainerMaps.Name = "splitContainerMaps";
            // 
            // splitContainerMaps.Panel1
            // 
            this.splitContainerMaps.Panel1.Controls.Add(this.panelWebbrowser);
            // 
            // splitContainerMaps.Panel2
            // 
            this.splitContainerMaps.Panel2.BackColor = System.Drawing.SystemColors.Window;
            this.splitContainerMaps.Panel2.Controls.Add(this.panelForWpfControl);
            this.splitContainerMaps.Panel2.Padding = new System.Windows.Forms.Padding(3);
            this.splitContainerMaps.Size = new System.Drawing.Size(415, 283);
            this.splitContainerMaps.SplitterDistance = 208;
            this.splitContainerMaps.TabIndex = 5;
            // 
            // panelWebbrowser
            // 
            this.panelWebbrowser.Controls.Add(this.userControlWebView);
            this.panelWebbrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWebbrowser.Location = new System.Drawing.Point(0, 0);
            this.panelWebbrowser.Name = "panelWebbrowser";
            this.panelWebbrowser.Size = new System.Drawing.Size(208, 283);
            this.panelWebbrowser.TabIndex = 0;
            // 
            // panelForWpfControl
            // 
            this.panelForWpfControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelForWpfControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelForWpfControl.Controls.Add(this.elementHost);
            this.panelForWpfControl.Location = new System.Drawing.Point(3, 3);
            this.panelForWpfControl.Name = "panelForWpfControl";
            this.panelForWpfControl.Padding = new System.Windows.Forms.Padding(3);
            this.panelForWpfControl.Size = new System.Drawing.Size(197, 277);
            this.panelForWpfControl.TabIndex = 2;
            // 
            // elementHost
            // 
            this.elementHost.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.elementHost.Location = new System.Drawing.Point(3, 3);
            this.elementHost.Name = "elementHost";
            this.elementHost.Size = new System.Drawing.Size(187, 267);
            this.elementHost.TabIndex = 1;
            this.elementHost.Text = "elementHost";
            this.elementHost.Child = null;
            // 
            // checkBoxZoomLevel
            // 
            this.checkBoxZoomLevel.AutoSize = true;
            this.checkBoxZoomLevel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxZoomLevel.Location = new System.Drawing.Point(129, 3);
            this.checkBoxZoomLevel.Name = "checkBoxZoomLevel";
            this.checkBoxZoomLevel.Size = new System.Drawing.Size(98, 20);
            this.checkBoxZoomLevel.TabIndex = 7;
            this.checkBoxZoomLevel.Text = "Use zoom level";
            this.checkBoxZoomLevel.UseVisualStyleBackColor = true;
            // 
            // buttonReload
            // 
            this.buttonReload.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonReload.Image = global::DiversitySamplingPlots.Properties.Resources.Transfrom;
            this.buttonReload.Location = new System.Drawing.Point(274, 1);
            this.buttonReload.Margin = new System.Windows.Forms.Padding(1);
            this.buttonReload.Name = "buttonReload";
            this.buttonReload.Size = new System.Drawing.Size(37, 24);
            this.buttonReload.TabIndex = 8;
            this.buttonReload.UseVisualStyleBackColor = true;
            this.buttonReload.Click += new System.EventHandler(this.buttonReload_Click);
            // 
            // userControlWebView
            // 
            this.userControlWebView.AllowScripting = true;
            this.userControlWebView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlWebView.Location = new System.Drawing.Point(0, 0);
            this.userControlWebView.Margin = new System.Windows.Forms.Padding(0);
            this.userControlWebView.Name = "userControlWebView";
            this.userControlWebView.ScriptErrorsSuppressed = false;
            this.userControlWebView.Size = new System.Drawing.Size(208, 283);
            this.userControlWebView.TabIndex = 1;
            this.userControlWebView.Url = new System.Uri("about:blank", System.UriKind.Absolute);
            // 
            // UserControlGeography
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Name = "UserControlGeography";
            this.Size = new System.Drawing.Size(421, 315);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownZoomLevel)).EndInit();
            this.splitContainerMaps.Panel1.ResumeLayout(false);
            this.splitContainerMaps.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMaps)).EndInit();
            this.splitContainerMaps.ResumeLayout(false);
            this.panelWebbrowser.ResumeLayout(false);
            this.panelForWpfControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonEditMode;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemEditModeBrowser;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemEditModeGisNoEdit;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemEditModeGIS;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.NumericUpDown numericUpDownZoomLevel;
        private System.Windows.Forms.SplitContainer splitContainerMaps;
        private System.Windows.Forms.Panel panelWebbrowser;
        private System.Windows.Forms.Panel panelForWpfControl;
        private System.Windows.Forms.Integration.ElementHost elementHost;
        private System.Windows.Forms.CheckBox checkBoxZoomLevel;
        private System.Windows.Forms.Button buttonReload;
        private System.Windows.Forms.ToolTip toolTip;
        private DiversityWorkbench.UserControls.UserControlWebView userControlWebView;
    }
}
