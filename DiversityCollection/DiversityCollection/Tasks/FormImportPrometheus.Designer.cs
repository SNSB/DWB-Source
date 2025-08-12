namespace DiversityCollection.Tasks
{
    partial class FormImportPrometheus
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImportPrometheus));
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageGrafana = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelGrafana = new System.Windows.Forms.TableLayoutPanel();
            this.webView2Grafana = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.buttonGrafanaAdd = new System.Windows.Forms.Button();
            this.comboBoxGrafanaDashboards = new System.Windows.Forms.ComboBox();
            this.imageListTab = new System.Windows.Forms.ImageList(this.components);
            this.tabPagePrometheus = new System.Windows.Forms.TabPage();
            this.tabPageClimate = new System.Windows.Forms.TabPage();
            this.tabControlMain.SuspendLayout();
            this.tabPageGrafana.SuspendLayout();
            this.tableLayoutPanelGrafana.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webView2Grafana)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageGrafana);
            this.tabControlMain.Controls.Add(this.tabPagePrometheus);
            this.tabControlMain.Controls.Add(this.tabPageClimate);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.ImageList = this.imageListTab;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(800, 450);
            this.tabControlMain.TabIndex = 1;
            // 
            // tabPageGrafana
            // 
            this.tabPageGrafana.Controls.Add(this.tableLayoutPanelGrafana);
            this.tabPageGrafana.ImageIndex = 0;
            this.tabPageGrafana.Location = new System.Drawing.Point(4, 23);
            this.tabPageGrafana.Name = "tabPageGrafana";
            this.tabPageGrafana.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGrafana.Size = new System.Drawing.Size(792, 423);
            this.tabPageGrafana.TabIndex = 0;
            this.tabPageGrafana.Text = "Dashboards (Grafana)";
            this.tabPageGrafana.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelGrafana
            // 
            this.tableLayoutPanelGrafana.ColumnCount = 2;
            this.tableLayoutPanelGrafana.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelGrafana.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelGrafana.Controls.Add(this.webView2Grafana, 0, 1);
            this.tableLayoutPanelGrafana.Controls.Add(this.buttonGrafanaAdd, 0, 0);
            this.tableLayoutPanelGrafana.Controls.Add(this.comboBoxGrafanaDashboards, 1, 0);
            this.tableLayoutPanelGrafana.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelGrafana.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelGrafana.Name = "tableLayoutPanelGrafana";
            this.tableLayoutPanelGrafana.RowCount = 2;
            this.tableLayoutPanelGrafana.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelGrafana.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelGrafana.Size = new System.Drawing.Size(786, 417);
            this.tableLayoutPanelGrafana.TabIndex = 0;
            // 
            // webView2Grafana
            // 
            this.tableLayoutPanelGrafana.SetColumnSpan(this.webView2Grafana, 2);
            this.webView2Grafana.CreationProperties = null;
            this.webView2Grafana.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webView2Grafana.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webView2Grafana.Location = new System.Drawing.Point(3, 30);
            this.webView2Grafana.Name = "webView2Grafana";
            this.webView2Grafana.Size = new System.Drawing.Size(780, 384);
            this.webView2Grafana.TabIndex = 3;
            this.webView2Grafana.ZoomFactor = 1D;
            // 
            // buttonGrafanaAdd
            // 
            this.buttonGrafanaAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonGrafanaAdd.FlatAppearance.BorderSize = 0;
            this.buttonGrafanaAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGrafanaAdd.Image = global::DiversityCollection.Resource.Add1;
            this.buttonGrafanaAdd.Location = new System.Drawing.Point(0, 0);
            this.buttonGrafanaAdd.Margin = new System.Windows.Forms.Padding(0);
            this.buttonGrafanaAdd.Name = "buttonGrafanaAdd";
            this.buttonGrafanaAdd.Size = new System.Drawing.Size(20, 27);
            this.buttonGrafanaAdd.TabIndex = 4;
            this.buttonGrafanaAdd.UseVisualStyleBackColor = true;
            this.buttonGrafanaAdd.Click += new System.EventHandler(this.buttonGrafanaAdd_Click);
            // 
            // comboBoxGrafanaDashboards
            // 
            this.comboBoxGrafanaDashboards.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxGrafanaDashboards.FormattingEnabled = true;
            this.comboBoxGrafanaDashboards.Location = new System.Drawing.Point(23, 3);
            this.comboBoxGrafanaDashboards.Name = "comboBoxGrafanaDashboards";
            this.comboBoxGrafanaDashboards.Size = new System.Drawing.Size(760, 21);
            this.comboBoxGrafanaDashboards.TabIndex = 5;
            this.comboBoxGrafanaDashboards.SelectedIndexChanged += new System.EventHandler(this.comboBoxGrafanaDashboards_SelectedIndexChanged);
            // 
            // imageListTab
            // 
            this.imageListTab.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTab.ImageStream")));
            this.imageListTab.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTab.Images.SetKeyName(0, "Grafana.ico");
            this.imageListTab.Images.SetKeyName(1, "Prometheus.ico");
            this.imageListTab.Images.SetKeyName(2, "Sensor.ico");
            // 
            // tabPagePrometheus
            // 
            this.tabPagePrometheus.ImageIndex = 1;
            this.tabPagePrometheus.Location = new System.Drawing.Point(4, 23);
            this.tabPagePrometheus.Name = "tabPagePrometheus";
            this.tabPagePrometheus.Size = new System.Drawing.Size(792, 423);
            this.tabPagePrometheus.TabIndex = 1;
            this.tabPagePrometheus.Text = "Import (Prometheus)";
            this.tabPagePrometheus.UseVisualStyleBackColor = true;
            // 
            // tabPageClimate
            // 
            this.tabPageClimate.ImageIndex = 2;
            this.tabPageClimate.Location = new System.Drawing.Point(4, 23);
            this.tabPageClimate.Name = "tabPageClimate";
            this.tabPageClimate.Size = new System.Drawing.Size(792, 423);
            this.tabPageClimate.TabIndex = 2;
            this.tabPageClimate.Text = "Import (climate)";
            this.tabPageClimate.UseVisualStyleBackColor = true;
            // 
            // FormImportPrometheus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControlMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormImportPrometheus";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import sensor data";
            this.tabControlMain.ResumeLayout(false);
            this.tabPageGrafana.ResumeLayout(false);
            this.tableLayoutPanelGrafana.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.webView2Grafana)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageGrafana;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelGrafana;
        private System.Windows.Forms.ImageList imageListTab;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView2Grafana;
        private System.Windows.Forms.Button buttonGrafanaAdd;
        private System.Windows.Forms.ComboBox comboBoxGrafanaDashboards;
        private System.Windows.Forms.TabPage tabPagePrometheus;
        private System.Windows.Forms.TabPage tabPageClimate;
    }
}