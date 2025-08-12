using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Spreadsheet
{
    public partial class UserControlSetMapSymbol : UserControl
    {
        //private double _SymbolSize = 1.0;
        //private string _Value = "";
        private MapSymbol _MapSymbol;
        private Sheet _Sheet;

        #region Construction

        public UserControlSetMapSymbol(ref Sheet Sheet, ref MapSymbol MapSymbol)
        {
            InitializeComponent();
            this.fillSymbolList();
            this._MapSymbol = MapSymbol;
            this.toolStripLabelValue.Text = this._MapSymbol.Value;
            this.toolStripTextBoxSize.Text = this._MapSymbol.SymbolSize.ToString();
            this._Sheet = Sheet;
            foreach (System.Windows.Forms.ToolStripDropDownItem DD in this.toolStripDropDownButtonSymbol.DropDownItems)
            {
                if (DD.Text == this._MapSymbol.SymbolTitle)
                {
                    this.toolStripMenuItemSymbol_Click(DD, null);
                }
            }
        }
        
        #endregion

        private void fillSymbolList()
        {

            this.toolStripDropDownButtonSymbol.DropDownItems.Clear();

            foreach (System.Collections.Generic.KeyValuePair<string, MapSymbol> KV in MapSymbols.SymbolDictionary())
            {
                string Title = KV.Key;
                ToolStripItem T = this.toolStripDropDownButtonSymbol.DropDownItems.Add(Title);
                T.Tag = KV.Value;
                T.Image = KV.Value.Image;
                T.Image.Tag = Title;
                T.Text = Title;
                T.DisplayStyle = ToolStripItemDisplayStyle.Image;
            }
            foreach (System.Windows.Forms.ToolStripDropDownItem DD in this.toolStripDropDownButtonSymbol.DropDownItems)
            {
                DD.DisplayStyle = ToolStripItemDisplayStyle.Image;
                DD.Click += new System.EventHandler(this.toolStripMenuItemSymbol_Click);
                DD.AutoSize = false;
                DD.Width = 30;
            }

            //this.toolStripMenuItemSymbol_Click(TcircleFilled, null);
            this.toolStripMenuItemSymbol_Click(this.toolStripDropDownButtonSymbol.DropDownItems[0], null);

        }

        private void toolStripMenuItemSymbol_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.ToolStripMenuItem D = (System.Windows.Forms.ToolStripMenuItem)sender;

                this.toolStripDropDownButtonSymbol.Text = D.Text;
                this.toolStripDropDownButtonSymbol.Tag = D.Tag;
                this.toolStripDropDownButtonSymbol.Image = D.Image;
                this.toolStripDropDownButtonSymbol.Image.Tag = D.Image.Tag;
                this.toolStripDropDownButtonSymbol.DisplayStyle = D.DisplayStyle;

                //MapSymbols MS = new MapSymbols();
                if (MapSymbols.SymbolDictionary().ContainsKey(D.Text) && this._MapSymbol != null)
                {
                    this._MapSymbol.Symbol = MapSymbols.SymbolDictionary()[D.Text].Symbol;// MS.getSymbol(D.Text);
                    this._MapSymbol.SymbolFilled = MapSymbols.SymbolDictionary()[D.Text].SymbolFilled;
                    this._MapSymbol.SymbolTitle = MapSymbols.SymbolDictionary()[D.Text].SymbolTitle;
                    if (this._MapSymbol.Value != null)
                    {
                        this._Sheet.MapSymbols()[this._MapSymbol.Value].Image = D.Image;
                        if (this._MapSymbol.Value.Length == 0)
                        {
                            this._Sheet.MapSymbolForMissing = this._MapSymbol;
                        }
                    }
                    else
                    {
                        this._Sheet.MapSymbolForMissing = this._MapSymbol;
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripTextBoxSize_TextChanged(object sender, EventArgs e)
        {
            double D;
            if (double.TryParse(this.toolStripTextBoxSize.Text, out D))
            {
                this._MapSymbol.SymbolSize = D;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(this.toolStripTextBoxSize.Text + " is not a numeric value");
                this.toolStripTextBoxSize.Text = this._MapSymbol.SymbolSize.ToString();
            }
        }

        public WpfSamplingPlotPage.PointSymbol SelectedSymbol()
        {
            return (WpfSamplingPlotPage.PointSymbol)this.toolStripDropDownButtonSymbol.Tag;
        }

        public void SizeEnabled(bool IsEnabled)
        {
            this.toolStripTextBoxSize.Enabled = IsEnabled;
        }

        public string Value() { return this._MapSymbol.Value; }
        public double SymbolSize() { return this._MapSymbol.SymbolSize; }
        public string SQL() { return this._Sheet.GeographySymbolTableAlias + "." + this._Sheet.GeographySymbolColumn + " = '" + this._MapSymbol.Value + "' "; }
        public void setSymbolSize(double Size) { this.toolStripTextBoxSize.Text = Size.ToString(); }

        private void toolStripButtonDown_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("available in upcoming version");
        }

        private void toolStripButtonUp_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("available in upcoming version");
        }

    }
}
