using DiversityWorkbench.DwbManual;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Spreadsheet
{
    public partial class FormMapLegend : Form
    {
        private Sheet _Sheet;

        public FormMapLegend(Sheet Sheet)
        {
            InitializeComponent();
            this._Sheet = Sheet;
            this.initForm();
        }

        public FormMapLegend(
            string SymbolHeader,
            System.Collections.Generic.Dictionary<string, MapSymbol> MapSymbols,
            string ColorHeader,
            System.Collections.Generic.List<MapColor> MapColors)
        {
            InitializeComponent();
            this.initForm(SymbolHeader, MapSymbols, ColorHeader, MapColors);
        }

        private void initForm()
        {
            try
            {
                this.moveToolStripMenuItem.Enabled = false;
                this.Text = "";
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.Height = (int)(80 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                DataColumn DCsymbol = this._Sheet.DataTables()[this._Sheet.GeographySymbolTableAlias].DataColumns()[this._Sheet.GeographySymbolColumn];
                this.groupBoxSymbol.Text = DCsymbol.DisplayText;
                int s = 0;
                System.Collections.Generic.Dictionary<string, string> Legend = new Dictionary<string, string>();
                System.Collections.Generic.Dictionary<string, MapSymbol> SymbolsForLegend = new Dictionary<string, MapSymbol>();
                foreach (System.Collections.Generic.KeyValuePair<string, MapSymbol> KV in this._Sheet.MapSymbols())
                {
                    if (!SymbolsForLegend.ContainsKey(KV.Value.SymbolTitle))
                        SymbolsForLegend.Add(KV.Value.SymbolTitle, KV.Value);
                    if (Legend.ContainsKey(KV.Value.SymbolTitle))
                    {
                        Legend[KV.Value.SymbolTitle] += ", " + KV.Key;
                    }
                    else
                    {
                        Legend.Add(KV.Value.SymbolTitle, KV.Key);
                    }
                }

                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Legend)
                {
                    DiversityWorkbench.Spreadsheet.UserControlMapLegend U = new UserControlMapLegend(SymbolsForLegend[KV.Key], KV.Value);
                    U.Dock = DockStyle.Top;
                    this.panelSymbol.Controls.Add(U);
                    U.BringToFront();
                    this.Height += (int)(22 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                    s++;
                }
                if (this._Sheet.GeographyColorColumn.Length > 0 && this._Sheet.GeographyColorTableAlias.Length > 0)
                {
                    DataColumn DCcolor = this._Sheet.DataTables()[this._Sheet.GeographyColorTableAlias].DataColumns()[this._Sheet.GeographyColorColumn];
                    this.groupBoxColor.Text = DCcolor.DisplayText;
                    int c = 1;
                    foreach (MapColor MC in this._Sheet.MapColors())
                    {
                        string Description = MC.Operator + " " + MC.LowerValue;
                        if (MC.Operator == "-")
                            Description = MC.LowerValue + " - " + MC.UpperValue;
                        DiversityWorkbench.Spreadsheet.UserControlMapLegend U = new UserControlMapLegend(MC, Description);
                        U.Dock = DockStyle.Top;
                        this.panelColor.Controls.Add(U);
                        U.BringToFront();
                        this.Height += (int)(22 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                        c++;
                    }
                    this.splitContainer.SplitterDistance = this.splitContainer.Height - (c * (int)(22 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor));
                    this.splitContainer.SplitterWidth = 1;
                }
                else
                {
                    this.splitContainer.Panel2Collapsed = true;
                    this.Height -= (int)(20 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void initForm(string SymbolHeader,
            System.Collections.Generic.Dictionary<string, MapSymbol> MapSymbols,
            string ColorHeader,
            System.Collections.Generic.List<MapColor> MapColors)
        {
            try
            {
                this.moveToolStripMenuItem.Enabled = false;
                this.Text = "";
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.Height = (int)(80 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                this.groupBoxSymbol.Text = SymbolHeader;
                int s = 0;
                System.Collections.Generic.Dictionary<string, string> Legend = new Dictionary<string, string>();
                System.Collections.Generic.Dictionary<string, MapSymbol> SymbolsForLegend = new Dictionary<string, MapSymbol>();
                foreach (System.Collections.Generic.KeyValuePair<string, MapSymbol> KV in MapSymbols)
                {
                    if (!SymbolsForLegend.ContainsKey(KV.Value.SymbolTitle))
                        SymbolsForLegend.Add(KV.Value.SymbolTitle, KV.Value);
                    if (Legend.ContainsKey(KV.Value.SymbolTitle))
                    {
                        Legend[KV.Value.SymbolTitle] += ", " + KV.Key;
                    }
                    else
                    {
                        Legend.Add(KV.Value.SymbolTitle, KV.Key);
                    }
                }

                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Legend)
                {
                    DiversityWorkbench.Spreadsheet.UserControlMapLegend U = new UserControlMapLegend(SymbolsForLegend[KV.Key], KV.Value);
                    U.Dock = DockStyle.Top;
                    this.panelSymbol.Controls.Add(U);
                    U.BringToFront();
                    this.Height += (int)(23 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                    s++;
                }
                if (ColorHeader.Length > 0)
                {
                    this.groupBoxColor.Text = ColorHeader;
                    int c = 1;
                    foreach (MapColor MC in MapColors)
                    {
                        string Description = MC.Operator + " " + MC.LowerValue;
                        if (MC.Operator == "-")
                            Description = MC.LowerValue + " - " + MC.UpperValue;
                        DiversityWorkbench.Spreadsheet.UserControlMapLegend U = new UserControlMapLegend(MC, Description);
                        U.Dock = DockStyle.Top;
                        this.panelColor.Controls.Add(U);
                        U.BringToFront();
                        this.Height += (int)(22 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                        c++;
                    }
                    this.splitContainer.SplitterDistance = this.splitContainer.Height - (c * (int)(22 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor));
                    this.splitContainer.SplitterWidth = 1;
                }
                else
                {
                    this.splitContainer.Panel2Collapsed = true;
                    this.Height -= (int)(20 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void moveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.moveToolStripMenuItem.Enabled = false;
            this.fixPositionToolStripMenuItem.Enabled = true;
            //this.Height += 30;
            this.ControlBox = true;
        }

        private void fixPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.fixPositionToolStripMenuItem.Enabled = false;
            this.moveToolStripMenuItem.Enabled = true;
            //this.Height -= 30;
            this.ControlBox = false;
        }

        #region Manual

        /// <summary>
        /// #35
        /// setting the keyword for the help provider
        /// </summary>
        public void setKeyword(string Keyword)
        {
            this.helpProvider.SetHelpKeyword(this, Keyword);
        }


        /// <summary>
        /// Adding event deletates to form and controls
        /// </summary>
        /// <returns></returns>
        private async System.Threading.Tasks.Task InitManual()
        {
            try
            {

                DiversityWorkbench.DwbManual.Hugo manual = new Hugo(this.helpProvider, this);
                if (manual != null)
                {
                    await manual.addKeyDownF1ToForm();
                }
            }
            catch (Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        /// <summary>
        /// ensure that init is only done once
        /// </summary>
        private bool _InitManualDone = false;


        /// <summary>
        /// KeyDown of the form adding event deletates to form and controls within the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Form_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (!_InitManualDone)
                {
                    await this.InitManual();
                    _InitManualDone = true;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        #endregion
    }
}
