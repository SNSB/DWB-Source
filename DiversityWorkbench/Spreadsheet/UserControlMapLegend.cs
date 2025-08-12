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
    public partial class UserControlMapLegend : UserControl
    {
        public UserControlMapLegend(MapColor MapColor, string Description)
        {
            InitializeComponent();
            this.labelDescription.Text = Description;
            this.buttonSymbolOrColor.BackColor = MapColor.BrushColors()[MapColor.Brush];
        }

        public UserControlMapLegend(MapSymbol MapSymbol, string Description)
        {
            InitializeComponent();
            this.labelDescription.Text = Description;
            this.buttonSymbolOrColor.BackColor = System.Drawing.SystemColors.Control;
            this.buttonSymbolOrColor.Image = MapSymbols.SymbolDictionary()[MapSymbol.SymbolTitle].Image;
        }

    }
}
