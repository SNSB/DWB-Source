using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WpfSamplingPlotPage;

namespace DiversityCollection.DistributionMap
{
    public partial class FormDistributionMap : Form, InterfaceDistributionMap
    {
        #region Parameter and properties

        private System.Windows.Forms.Integration.ElementHost elementHost;
        /// <summary>
        /// the WPF control for the GIS operations
        /// </summary>
        private WpfSamplingPlotPage.WpfControl _wpfControl;
        public WpfSamplingPlotPage.WpfControl WpfControl
        {
            get
            {
                if (this._wpfControl == null)
                {
                    _wpfControl = new WpfSamplingPlotPage.WpfControl(DiversityWorkbench.Settings.Language);
                    this._wpfControl.WpfShowRefMapDelButton(false);
                    //this.elementHost.Child = _wpfControl;
                    this.elementHost.Child = _wpfControl.WpfGetCanvas(this.splitContainerMain.Panel1);
                }
                return _wpfControl;
            }
            set { _wpfControl = value; }
        }

        private System.Collections.Generic.List<Setting> _Settings;
        
        #endregion

        #region Construction, Form
        
        public FormDistributionMap()
        {
            InitializeComponent();
            this.initForm();
        }

        private void initForm()
        {
            this.elementHost = new System.Windows.Forms.Integration.ElementHost();
            this.elementHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost.Location = new System.Drawing.Point(0, 0);
            this.elementHost.Name = "elementHost";
            this.elementHost.Size = new System.Drawing.Size(631, 627);
            this.elementHost.TabIndex = 0;
            this.elementHost.Text = "elementHost";
            //this.elementHost.Child = this._wpfControl;
            this.splitContainerMain.Panel1.Controls.Add(this.elementHost);

            this.WpfControl.WpfShowRefMapDelButton(false);
            //this.elementHost.Child = _wpfControl.WpfGetCanvas(this.splitContainerMain.Panel1);
            this.WpfControl.WpfClearAllSamples(true);

            this._Settings = new List<Setting>();
        }
        
        #endregion

        public void RemoveSetting(Setting Setting)
        {
            this._Settings.Remove(Setting);
            this.initSettingControls();
        }

        //private void initSettings()
        //{
        //    this.panelSettings.Controls.Clear();
        //    foreach (Setting S in this._Settings)
        //        this.AddSetting(S);
        //}

        private void initSettingControls()
        {
            this.panelSettings.Controls.Clear();
            foreach (Setting S in this._Settings)
                this.AddSettingControl(S);
        }



        private void toolStripButtonGenerateMap_Click(object sender, EventArgs e)
        {

            string SQL = "SELECT " + DiversityCollection.Spreadsheet.Target.TableAlias("CollectionEventLocalisation") + ".Geography.ToString(), " +
                DiversityCollection.Spreadsheet.Target.TableAlias("IdentificationUnit") + ".IdentificationUnitID ";
            string Message = "";
            System.Collections.Generic.List<GeoObject> GeoObjects = new List<GeoObject>();
            foreach (Setting S in this._Settings)
            {
                string sql = SQL + S.FromWhereClause;
                System.Data.DataTable DT = new DataTable();
                DiversityWorkbench.FormFunctions.SqlFillTable(sql, ref DT, ref Message);
                foreach (System.Data.DataRow R in DT.Rows)
                {
                    GeoObject GO = new GeoObject();
                    GO.PointSymbolSize = S.SymbolSize;
                    GO.PointType = S.PointSymbol;
                    GO.GeometryData = R[0].ToString();
                    GO.Identifier = R[1].ToString();
                    GO.DisplayText = R[1].ToString();
                    GO.StrokeTransparency = S.SymbolTransparency;
                    GO.FillTransparency = S.SymbolTransparency;
                    GO.FillBrush = S.Brush;
                    GO.StrokeBrush = S.Brush;
                    GeoObjects.Add(GO);
                }
            }
            this.WpfControl.WpfClearAllSamples(true);
            this.WpfControl.WpfSetMapAndGeoObjects(GeoObjects);
            this.WpfControl.WpfAddSample();
        }

        private void toolStripButtonLoadSettings_Click(object sender, EventArgs e)
        {
            this.openFileDialog = new OpenFileDialog();
            this.openFileDialog.RestoreDirectory = true;
            this.openFileDialog.InitialDirectory = Setting.SettingsDirectory();
            this.openFileDialog.Filter = "xml files (*.xml)|*.xml";
            this.openFileDialog.ShowDialog();
            if (this.openFileDialog.FileName.Length > 0)
            {
                this._Settings = Setting.ReadSettings(this.openFileDialog.FileName);
                this.initSettingControls();
            }
        }

        private void toolStripButtonAddSetting_Click(object sender, EventArgs e)
        {
            DiversityCollection.DistributionMap.Setting S = new Setting();
            this.AddSetting(S);
            this.initSettingControls();
        }

        private void AddSetting(Setting S)
        {
            if (this._Settings == null)
                this._Settings = new List<Setting>();
            this._Settings.Add(S);
        }

        private void AddSettingControl(Setting S)
        {
            DiversityCollection.DistributionMap.UserControlSetting U = new UserControlSetting(S, this);
            this.panelSettings.Controls.Add(U);
            U.Dock = DockStyle.Top;
            U.BringToFront();
        }

        private void toolStripButtonSaveSettings_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.FormGetString f = new DiversityWorkbench.FormGetString("Save settings", "Please enter the title of the settings", "DistributionMap");
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.String.Length > 0)
            {
                Setting.WriteDistributionMapSettings(f.String, this._Settings);
            }
        }

    }
}
