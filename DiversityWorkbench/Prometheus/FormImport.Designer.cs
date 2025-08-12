namespace DiversityWorkbench.Prometheus
{
    partial class FormImport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImport));
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageGrafana = new System.Windows.Forms.TabPage();
            this.imageListTab = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanelGrafana = new System.Windows.Forms.TableLayoutPanel();
            this.buttonGrafanaRefresh = new System.Windows.Forms.Button();
            this.textBoxGrafanaUrl = new System.Windows.Forms.TextBox();
            this.tabControlMain.SuspendLayout();
            this.tabPageGrafana.SuspendLayout();
            this.tableLayoutPanelGrafana.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageGrafana);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.ImageList = this.imageListTab;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(722, 450);
            this.tabControlMain.TabIndex = 0;
            // 
            // tabPageGrafana
            // 
            this.tabPageGrafana.Controls.Add(this.tableLayoutPanelGrafana);
            this.tabPageGrafana.ImageIndex = 0;
            this.tabPageGrafana.Location = new System.Drawing.Point(4, 23);
            this.tabPageGrafana.Name = "tabPageGrafana";
            this.tabPageGrafana.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGrafana.Size = new System.Drawing.Size(714, 423);
            this.tabPageGrafana.TabIndex = 0;
            this.tabPageGrafana.Text = "Grafana";
            this.tabPageGrafana.UseVisualStyleBackColor = true;
            // 
            // imageListTab
            // 
            this.imageListTab.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTab.ImageStream")));
            this.imageListTab.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTab.Images.SetKeyName(0, "Grafana.ico");
            this.imageListTab.Images.SetKeyName(1, "Prometheus.ico");
            // 
            // tableLayoutPanelGrafana
            // 
            this.tableLayoutPanelGrafana.ColumnCount = 2;
            this.tableLayoutPanelGrafana.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelGrafana.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelGrafana.Controls.Add(this.buttonGrafanaRefresh, 0, 0);
            this.tableLayoutPanelGrafana.Controls.Add(this.textBoxGrafanaUrl, 1, 0);
            this.tableLayoutPanelGrafana.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelGrafana.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelGrafana.Name = "tableLayoutPanelGrafana";
            this.tableLayoutPanelGrafana.RowCount = 2;
            this.tableLayoutPanelGrafana.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelGrafana.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelGrafana.Size = new System.Drawing.Size(708, 417);
            this.tableLayoutPanelGrafana.TabIndex = 0;
            // 
            // buttonGrafanaRefresh
            // 
            this.buttonGrafanaRefresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonGrafanaRefresh.FlatAppearance.BorderSize = 0;
            this.buttonGrafanaRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGrafanaRefresh.Image = global::DiversityWorkbench.Properties.Resources.Transfrom;
            this.buttonGrafanaRefresh.Location = new System.Drawing.Point(0, 0);
            this.buttonGrafanaRefresh.Margin = new System.Windows.Forms.Padding(0);
            this.buttonGrafanaRefresh.Name = "buttonGrafanaRefresh";
            this.buttonGrafanaRefresh.Size = new System.Drawing.Size(23, 26);
            this.buttonGrafanaRefresh.TabIndex = 1;
            this.buttonGrafanaRefresh.UseVisualStyleBackColor = true;
            this.buttonGrafanaRefresh.Click += new System.EventHandler(this.buttonGrafanaRefresh_Click);
            // 
            // textBoxGrafanaUrl
            // 
            this.textBoxGrafanaUrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxGrafanaUrl.Location = new System.Drawing.Point(26, 3);
            this.textBoxGrafanaUrl.Name = "textBoxGrafanaUrl";
            this.textBoxGrafanaUrl.Size = new System.Drawing.Size(679, 20);
            this.textBoxGrafanaUrl.TabIndex = 2;
            this.textBoxGrafanaUrl.TextChanged += new System.EventHandler(this.textBoxGrafanaUrl_TextChanged);
            // 
            // FormImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(722, 450);
            this.Controls.Add(this.tabControlMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormImport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import";
            this.tabControlMain.ResumeLayout(false);
            this.tabPageGrafana.ResumeLayout(false);
            this.tableLayoutPanelGrafana.ResumeLayout(false);
            this.tableLayoutPanelGrafana.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageGrafana;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelGrafana;
        private System.Windows.Forms.Button buttonGrafanaRefresh;
        private System.Windows.Forms.TextBox textBoxGrafanaUrl;
        private System.Windows.Forms.ImageList imageListTab;
    }
}