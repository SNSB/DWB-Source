namespace DiversityCollection.Tasks
{
    partial class IPM
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IPM));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.imageListPests = new System.Windows.Forms.ImageList(this.components);
            this._Chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this._Chart)).BeginInit();
            // 
            // imageListPests
            // 
            this.imageListPests.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListPests.ImageStream")));
            this.imageListPests.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListPests.Images.SetKeyName(0, "Tinea_pellionella.png");
            this.imageListPests.Images.SetKeyName(1, "Trogoderma_angustum.png");
            this.imageListPests.Images.SetKeyName(2, "Xestobium_rufovillosum.png");
            this.imageListPests.Images.SetKeyName(3, "Berlinkaefer.png");
            this.imageListPests.Images.SetKeyName(4, "Nagekaefer.png");
            this.imageListPests.Images.SetKeyName(5, "Pelzmotte.png");
            this.imageListPests.Images.SetKeyName(6, "Speckkaefer.png");
            this.imageListPests.Images.SetKeyName(7, "Wollkrautbluetenkaefer.png");
            // 
            // _Chart
            // 
            chartArea1.Name = "ChartArea1";
            this._Chart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this._Chart.Legends.Add(legend1);
            this._Chart.Location = new System.Drawing.Point(0, 0);
            this._Chart.Name = "_Chart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this._Chart.Series.Add(series1);
            this._Chart.Size = new System.Drawing.Size(300, 300);
            this._Chart.TabIndex = 0;
            this._Chart.Text = "chart1";
            ((System.ComponentModel.ISupportInitialize)(this._Chart)).EndInit();

        }

        #endregion

        private System.Windows.Forms.ImageList imageListPests;
        private System.Windows.Forms.DataVisualization.Charting.Chart _Chart;
    }
}
