using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Spreadsheet
{
    public class MapSymbol
    {
        private string _Value;
        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        private string _SymbolTitle;
        public string SymbolTitle
        {
            get { return _SymbolTitle; }
            set { _SymbolTitle = value; }
        }

        private bool _IsExcluded;
        public bool IsExcluded
        {
            get { return _IsExcluded; }
            set { _IsExcluded = value; }
        }

        private int _LegendPosition = 1;
        public int LegendPosition
        {
            get { return _LegendPosition; }
            set { _LegendPosition = value; }
        }

        private WpfSamplingPlotPage.PointSymbol _Symbol;
        public WpfSamplingPlotPage.PointSymbol Symbol
        {
            get { return _Symbol; }
            set { _Symbol = value; }
        }

        private bool _SymbolFilled = false;
        public bool SymbolFilled
        {
            get { return _SymbolFilled; }
            set { _SymbolFilled = value; }
        }

        private double _SymbolSize;
        public double SymbolSize
        {
            get { return _SymbolSize; }
            set { _SymbolSize = value; }
        }

        private System.Drawing.Image _Image;
        public System.Drawing.Image Image
        {
            get { return _Image; }
            set { _Image = value; }
        }

        public MapSymbol(string Title, WpfSamplingPlotPage.PointSymbol Symbol, bool IsFilled, System.Drawing.Image Image)
        {
            this._Symbol = Symbol;
            this._SymbolTitle = Title;
            this._SymbolFilled = IsFilled;
            this._SymbolSize = 1;
            this._Image = Image;
        }

        public MapSymbol(string Value, double SymbolSize, string SymbolTitle)
        {
            MapSymbol MS = MapSymbols.SymbolDictionary()[SymbolTitle];
            this._Symbol = MS.Symbol;
            this._Value = Value;
            this._SymbolTitle = SymbolTitle;
            this._SymbolFilled = MS.SymbolFilled;
            this._SymbolSize = SymbolSize;
            this._Image = MS.Image;
        }

    }
}
