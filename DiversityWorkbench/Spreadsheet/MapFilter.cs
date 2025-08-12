using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Spreadsheet
{
    public class MapFilter
    {

        #region Forward type

        public enum ForwardTypes { Allways, RestrictToSuccess }
        private ForwardTypes _ForwardType;

        public ForwardTypes ForwardType
        {
            get { return _ForwardType; }
            set { _ForwardType = value; }
        }
        
        #endregion

        #region Filter type

        public enum FilterTypes { Geography, Color, Symbol }
        private FilterTypes _FilterType;
        public FilterTypes FilterType() { return this._FilterType; }
        private int _Position;
        public void setPosition(int Position) { this._Position = Position; }
        public int Position() { return this._Position; }

        #endregion

        #region Detail types and values

        public enum ColorFilterTypes { Maximum }
        private ColorFilterTypes _ColorFilterType = ColorFilterTypes.Maximum;
        public ColorFilterTypes ColorFilterType() { return this._ColorFilterType; }

        public enum GeographyFilterTypes { Quadrant, CenterOfQuadrant }
        private GeographyFilterTypes _GeographyFilterType = GeographyFilterTypes.CenterOfQuadrant;
        public GeographyFilterTypes GeographyFilterType()
        {
            return this._GeographyFilterType;
        }
        public void setGeographyFilterType(GeographyFilterTypes Type) { this._GeographyFilterType = Type; }
        private string _GeographyGazetteer = "";
        public string GeographyGazetteer() { return this._GeographyGazetteer; }
        public void setGeographyGazetteer(string ServerConnectionDisplayText) { this._GeographyGazetteer = ServerConnectionDisplayText; }

        private DiversityWorkbench.ServerConnection _GazetteerConnection;
        public DiversityWorkbench.ServerConnection GazetteerConnection()
        {
            DiversityWorkbench.ServerConnection C = null;
            foreach (System.Collections.Generic.KeyValuePair<string, ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityGazetteer"].ServerConnections())
            {
                if (this._GeographyGazetteer == KV.Value.DisplayText)
                {
                    this._GazetteerConnection = KV.Value;
                    break;
                }
                C = KV.Value;
            }
            if (this._GazetteerConnection == null)
                this._GazetteerConnection = C;
            return this._GazetteerConnection;
        }
        public void ResetGazetteerConnection() { this._GazetteerConnection = null; }

        #endregion

        //private string _Table;
        //private string _Column;
        private Sheet _Sheet;

        #region Symbol

        private System.Data.DataTable _DtSymbolValues;
        private System.Data.DataTable DtSymbolValues()
        {
            if (this._DtSymbolValues == null)
            {
                this._DtSymbolValues = new System.Data.DataTable();
                this._Sheet.getDataDistinctContentForMapSymbols(ref this._DtSymbolValues);
            }
            return this._DtSymbolValues;
        }

        public System.Collections.Generic.List<string> MissingSymbols()
        {
            System.Collections.Generic.List<string> L = new List<string>();
            foreach (System.Data.DataRow R in this.DtSymbolValues().Rows)
            {
                if (!this.SymbolValueSequence().ContainsKey(R[0].ToString()))
                    L.Add(R[0].ToString());
            }
            return L;
        }
        
        private System.Collections.Generic.Dictionary<string, int> _SymbolValueSequence;
        public System.Collections.Generic.Dictionary<string, int> SymbolValueSequence()
        {
            if (this._SymbolValueSequence == null)
                this._SymbolValueSequence = new Dictionary<string, int>();
            return this._SymbolValueSequence;
        }

        public void AddSymbol(string SymbolValue)
        {
            this.SymbolValueSequence().Add(SymbolValue, this.SymbolValueSequence().Count);
        }

        public void ClearSymbols()
        {
            if (this._SymbolValueSequence != null)
                this._SymbolValueSequence.Clear();
        }

        public void RemoveSymbol(string Value)
        {
            this.SymbolValueSequence().Remove(Value);
        }

        private System.Collections.Generic.Dictionary<string, MapSymbol> _MapSymbols;

        private System.Collections.Generic.Dictionary<string, MapSymbol> MapSymbols()
        {
            if (this._MapSymbols == null)
            {
                this._MapSymbols = new Dictionary<string, MapSymbol>();
                foreach (System.Collections.Generic.KeyValuePair<string, int> KV in this._SymbolValueSequence)
                {
                    MapSymbol MS = new MapSymbol(KV.Key, 1, KV.Key);
                }
            }
            return this._MapSymbols;
        }

        private MapSymbol SymbolOfObject(WpfSamplingPlotPage.GeoObject O)
        {
            MapSymbol MS;
            switch (O.PointType)
            {
                case WpfSamplingPlotPage.PointSymbol.Circle:
                    if (O.FillTransparency == 0)
                    {
                    }
                    break;
                case WpfSamplingPlotPage.PointSymbol.Cross:
                    break;
                case WpfSamplingPlotPage.PointSymbol.Diamond:
                    break;
                case WpfSamplingPlotPage.PointSymbol.Square:
                    break;
                case WpfSamplingPlotPage.PointSymbol.X:
                    break;

            }
            MS = new MapSymbol("", 1, "");
            return MS;
        }
        
        #endregion

        #region Construction

        public MapFilter(FilterTypes Type, Sheet Sheet)
        {
            this._Sheet = Sheet;
            this._FilterType = Type;
            switch (this._FilterType)
            {
                case FilterTypes.Geography:
                    this._GeographyFilterType = GeographyFilterTypes.CenterOfQuadrant;
                    this._ForwardType = ForwardTypes.Allways;
                    break;
                case FilterTypes.Color:
                    this._ColorFilterType = ColorFilterTypes.Maximum;
                    this._ForwardType = ForwardTypes.RestrictToSuccess;
                    break;
                case FilterTypes.Symbol:
                    this._ForwardType = ForwardTypes.RestrictToSuccess;
                    break;
            }
        }
        
        #endregion

        public FilterTypes Type() { return this._FilterType; }

        private int GeographyObjectPosition(string Column, string TableAlias)
        {
            int i = -1;
            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
            {
                if (DC.Value.DataTable().Alias() == TableAlias &&
                    DC.Value.Column.Name == Column)
                {
                    i = DC.Key;
                    break;
                }
            }
            return i;
        }

    }
}
