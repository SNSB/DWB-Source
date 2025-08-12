using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Spreadsheet
{
    public partial class MapSymbols : Component
    {
        public MapSymbols()
        {
            InitializeComponent();
        }

        public MapSymbols(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        private static System.Collections.Generic.Dictionary<string, MapSymbol> _SymbolDictionary;
        public static System.Collections.Generic.Dictionary<string, MapSymbol> SymbolDictionary()
        {
            if (_SymbolDictionary == null)
            {
                MapSymbols MS = new MapSymbols();
                _SymbolDictionary = new Dictionary<string, MapSymbol>();
                AddSymbol("Circle", WpfSamplingPlotPage.PointSymbol.Circle, false, MS.imageListSymbol.Images[0], 0);
                AddSymbol("Circle filled", WpfSamplingPlotPage.PointSymbol.Circle, true, MS.imageListSymbol.Images[1], 1);
                AddSymbol("Diamond", WpfSamplingPlotPage.PointSymbol.Diamond, false, MS.imageListSymbol.Images[2], 2);
                AddSymbol("Diamond filled", WpfSamplingPlotPage.PointSymbol.Diamond, true, MS.imageListSymbol.Images[3], 3);
                AddSymbol("Square", WpfSamplingPlotPage.PointSymbol.Square, false, MS.imageListSymbol.Images[4], 4);
                AddSymbol("Square filled", WpfSamplingPlotPage.PointSymbol.Square, true, MS.imageListSymbol.Images[5], 5);
                AddSymbol("Cone", WpfSamplingPlotPage.PointSymbol.Cone, false, MS.imageListSymbol.Images[6], 6);
                AddSymbol("Cone filled", WpfSamplingPlotPage.PointSymbol.Cone, true, MS.imageListSymbol.Images[7], 7);
                AddSymbol("Pyramid", WpfSamplingPlotPage.PointSymbol.Pyramid, false, MS.imageListSymbol.Images[8], 8);
                AddSymbol("Pyramid filled", WpfSamplingPlotPage.PointSymbol.Pyramid, true, MS.imageListSymbol.Images[9], 9);
                AddSymbol("Cross", WpfSamplingPlotPage.PointSymbol.Cross, false, MS.imageListSymbol.Images[10], 10);
                AddSymbol("X", WpfSamplingPlotPage.PointSymbol.X, false, MS.imageListSymbol.Images[11], 11);
                AddSymbol("Minus", WpfSamplingPlotPage.PointSymbol.Minus, false, MS.imageListSymbol.Images[12], 12);
                AddSymbol("Questionmark", WpfSamplingPlotPage.PointSymbol.Questionmark, false, MS.imageListSymbol.Images[13], 13);
                AddSymbol("Hide", WpfSamplingPlotPage.PointSymbol.None, false, MS.imageListSymbol.Images[14], 14);
                AddSymbol("Circlepoint", WpfSamplingPlotPage.PointSymbol.Circlepoint, false, MS.imageListSymbol.Images[15], 15);
                AddSymbol("Square small", WpfSamplingPlotPage.PointSymbol.SquareSmall, true, MS.imageListSymbol.Images[16], 16);
            }
            return _SymbolDictionary;
        }

        public static System.Collections.Generic.Dictionary<string, MapSymbol> SymbolDictionarySorted()
        {
            System.Collections.Generic.SortedDictionary<int, string> Dict = new SortedDictionary<int, string>();
            foreach (System.Collections.Generic.KeyValuePair<string, MapSymbol> KV in SymbolDictionary())
            {
                if (!Dict.ContainsKey(KV.Value.LegendPosition))
                    Dict.Add(KV.Value.LegendPosition, KV.Key);
                else 
                {
                    int iPos = KV.Value.LegendPosition;
                    while (Dict.ContainsKey(iPos))
                        iPos++;
                    Dict.Add(iPos, KV.Key);
                }
            }
            System.Collections.Generic.Dictionary<string, MapSymbol> Symbols = new Dictionary<string, MapSymbol>();
            foreach(System.Collections.Generic.KeyValuePair<int, string> KV in Dict)
            {
                Symbols.Add(KV.Value, SymbolDictionary()[KV.Value]);
            }
            return Symbols;
        }

        private static void AddSymbol(string Title, WpfSamplingPlotPage.PointSymbol Symbol, bool IsFilled, System.Drawing.Image Image, int? Position = null)
        {
            MapSymbol MS = new MapSymbol(Title, Symbol, IsFilled, Image);
            // Setting the Position
            if (Position != null)
                MS.LegendPosition = (int)Position;
            else
            {
                System.Collections.Generic.List<int> Positions = new List<int>();
                foreach(System.Collections.Generic.KeyValuePair<string, MapSymbol> KV in _SymbolDictionary)
                {
                    Positions.Add(KV.Value.LegendPosition);
                }
                int iPos = 1;
                while (Positions.Contains(iPos))
                    iPos++;
                MS.LegendPosition = iPos;
            }
            _SymbolDictionary.Add(Title, MS);
        }

        private static System.Collections.Generic.Dictionary<WpfSamplingPlotPage.PointSymbol, double> _SymbolScalingFactors;
        public static System.Collections.Generic.Dictionary<WpfSamplingPlotPage.PointSymbol, double> SymbolScalingFactors()
        {
            if (_SymbolScalingFactors == null)
            {
                _SymbolScalingFactors = new Dictionary<WpfSamplingPlotPage.PointSymbol, double>();
                _SymbolScalingFactors.Add(WpfSamplingPlotPage.PointSymbol.Circle, 0.87);
                _SymbolScalingFactors.Add(WpfSamplingPlotPage.PointSymbol.Circlepoint, 0.87);
                _SymbolScalingFactors.Add(WpfSamplingPlotPage.PointSymbol.Diamond, 0.75);
                _SymbolScalingFactors.Add(WpfSamplingPlotPage.PointSymbol.Cone, 0.75);
                _SymbolScalingFactors.Add(WpfSamplingPlotPage.PointSymbol.Pyramid, 0.75);
                _SymbolScalingFactors.Add(WpfSamplingPlotPage.PointSymbol.Square, 1.0);
                _SymbolScalingFactors.Add(WpfSamplingPlotPage.PointSymbol.Cross, 0.92);
                _SymbolScalingFactors.Add(WpfSamplingPlotPage.PointSymbol.Minus, 0.92);
                _SymbolScalingFactors.Add(WpfSamplingPlotPage.PointSymbol.X, 1.1);
                _SymbolScalingFactors.Add(WpfSamplingPlotPage.PointSymbol.Questionmark, 0.92);
            }
            return _SymbolScalingFactors;
        }

    }
}
