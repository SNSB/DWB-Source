using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.DistributionMap
{
    public partial class UserControlSetting : UserControl
    {

        #region Parameter

        private DiversityCollection.DistributionMap.Setting _Setting;
        private InterfaceDistributionMap _InterfaceDistributionMap;
        
        #endregion

        #region Construction and init and control

        public UserControlSetting(DiversityCollection.DistributionMap.Setting Setting, InterfaceDistributionMap iDistributionMap)
        {
            InitializeComponent();
            this._Setting = Setting;
            this._InterfaceDistributionMap = iDistributionMap;
            this.InitControl();
        }
        
        private void InitControl()
        {
            this.initSymbols();
            this.fillColorList();

            this.setControl();
        }

        private void setControl()
        {
            this.textBoxTitle.Text = this._Setting.Title;
            this.toolStripButtonSetColor.BackColor = this._Setting.SettingColor;
            this.toolStripTextBoxTransparency.Text = this._Setting.SymbolTransparency.ToString();
            this.toolStripTextBoxSize.Text = this._Setting.SymbolSize.ToString();
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            this._InterfaceDistributionMap.RemoveSetting(this._Setting);
        }

        #endregion

        private void toolStripButtonSetRestriction_Click(object sender, EventArgs e)
        {
            this._Setting.FromWhereClause = DiversityCollection.DistributionMap.DistributionMap.FromWhereClause();
            string DisplayText = this._Setting.FromWhereClause.Substring(this._Setting.FromWhereClause.IndexOf(" WHERE "));
            DisplayText = DisplayText.Substring(DisplayText.IndexOf(".") + 1);
            System.Collections.Generic.List<string> Display = new List<string>();
            while (DisplayText.IndexOf(" AND ") > -1)
            {
                Display.Add(DisplayText.Substring(0, DisplayText.IndexOf(" AND ")));
                DisplayText = DisplayText.Substring(DisplayText.IndexOf(" AND ") + " AND ".Length);
                DisplayText = DisplayText.Substring(DisplayText.IndexOf(".") + 1);
            }
            //Display.Add(DisplayText); contains last added restriction "Geography IS NULL "
            this._Setting.Title = "";
            foreach (string D in Display)
            {
                if (this._Setting.Title.Length > 0)
                    this._Setting.Title += "; ";
                this._Setting.Title += D;
            }
            this.InitControl();
            return;

            DiversityCollection.FormCollectionSpecimen f = new FormCollectionSpecimen(-1, true, true, FormCollectionSpecimen.ViewMode.QueryMode, "DistributionMap");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                DiversityWorkbench.UserControlQueryList QL = new DiversityWorkbench.UserControlQueryList();
                QL.RememberQueryConditionSettings_ReadFromFile("DistributionMap");
                this._Setting.FromWhereClause = QL.WhereClauseOptimized();
            }
        }

        //private void toolStripButtonSetColor_Click(object sender, EventArgs e)
        //{
        //    this.colorDialog = new ColorDialog();
        //    this.colorDialog.AnyColor = false;
        //    this.colorDialog.AllowFullOpen = false;
        //    this.colorDialog.SolidColorOnly = true;
        //    this.colorDialog.ShowDialog();
        //    this._Setting.SettingColor = this.colorDialog.Color;
        //    this.setControl();
        //}

        #region Color

        private void fillColorList()
        {
            ToolStripItem Tred = this.toolStripDropDownButtonColor.DropDownItems.Add(System.Windows.Media.Brushes.Red.ToString());
            Tred.BackColor = System.Drawing.Color.Red;
            Tred.Tag = System.Windows.Media.Brushes.Red;

            ToolStripItem Torange = this.toolStripDropDownButtonColor.DropDownItems.Add(System.Windows.Media.Brushes.Orange.ToString());
            Torange.BackColor = System.Drawing.Color.Orange;
            Torange.Tag = System.Windows.Media.Brushes.Orange;

            ToolStripItem Tyellow = this.toolStripDropDownButtonColor.DropDownItems.Add(System.Windows.Media.Brushes.Yellow.ToString());
            Tyellow.BackColor = System.Drawing.Color.Yellow;
            Tyellow.Tag = System.Windows.Media.Brushes.Yellow;

            ToolStripItem Tgreen = this.toolStripDropDownButtonColor.DropDownItems.Add(System.Windows.Media.Brushes.Green.ToString());
            Tgreen.BackColor = System.Drawing.Color.Green;
            Tgreen.Tag = System.Windows.Media.Brushes.Green;

            ToolStripItem Tblue = this.toolStripDropDownButtonColor.DropDownItems.Add(System.Windows.Media.Brushes.Blue.ToString());
            Tblue.BackColor = System.Drawing.Color.Blue;
            Tblue.Tag = System.Windows.Media.Brushes.Blue;

            ToolStripItem Tviolet = this.toolStripDropDownButtonColor.DropDownItems.Add(System.Windows.Media.Brushes.Violet.ToString());
            Tviolet.BackColor = System.Drawing.Color.Violet;
            Tviolet.Tag = System.Windows.Media.Brushes.Violet;

            ToolStripItem Tbrown = this.toolStripDropDownButtonColor.DropDownItems.Add(System.Windows.Media.Brushes.Brown.ToString());
            Tbrown.BackColor = System.Drawing.Color.Brown;
            Tbrown.Tag = System.Windows.Media.Brushes.Brown;

            ToolStripItem Tblack = this.toolStripDropDownButtonColor.DropDownItems.Add(System.Windows.Media.Brushes.Black.ToString());
            Tblack.BackColor = System.Drawing.Color.Black;
            Tblack.Tag = System.Windows.Media.Brushes.Black;

            ToolStripItem Tgray = this.toolStripDropDownButtonColor.DropDownItems.Add(System.Windows.Media.Brushes.Gray.ToString());
            Tgray.BackColor = System.Drawing.Color.Gray;
            Tgray.Tag = System.Windows.Media.Brushes.Gray;

            ToolStripItem Twhite = this.toolStripDropDownButtonColor.DropDownItems.Add(System.Windows.Media.Brushes.White.ToString());
            Twhite.BackColor = System.Drawing.Color.White;
            Twhite.Tag = System.Windows.Media.Brushes.White;

            foreach (System.Windows.Forms.ToolStripDropDownItem DD in this.toolStripDropDownButtonColor.DropDownItems)
            {
                DD.DisplayStyle = ToolStripItemDisplayStyle.Image;
                DD.Width = 30;
                DD.Click += new System.EventHandler(this.toolStripMenuItemColor_Click);
                DD.AutoSize = false;
            }

            this.toolStripMenuItemColor_Click(Tred, null);

        }

        private void toolStripMenuItemColor_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolStripMenuItem D = (System.Windows.Forms.ToolStripMenuItem)sender;

            this.toolStripDropDownButtonColor.BackColor = D.BackColor;
            this.toolStripDropDownButtonColor.Text = "     ";
            this.toolStripDropDownButtonColor.Tag = D.Tag;
            this._Setting.setColorForSymbol(D.BackColor.Name);
        }
        
        //public System.Windows.Media.Brush Brush
        //{
        //    get
        //    {
        //        System.Windows.Media.Brush b = System.Windows.Media.Brushes.White;
        //        try
        //        {
        //            if (this.toolStripDropDownButtonColor.Tag != null)
        //                b = (System.Windows.Media.Brush)this.toolStripDropDownButtonColor.Tag;
        //        }
        //        catch { }
        //        return b;
        //    }
        //}

        #endregion

        private void toolStripTextBoxSize_Leave(object sender, EventArgs e)
        {
            int i;
            if (int.TryParse(this.toolStripTextBoxSize.Text, out i))
            {
                this._Setting.SymbolSize = i;
                this.setControl();
            }
        }

        private void textBoxTitle_Leave(object sender, EventArgs e)
        {
            this._Setting.Title = this.textBoxTitle.Text;
        }

        private void toolStripTextBoxTransparency_Leave(object sender, EventArgs e)
        {
            byte b;
            if (byte.TryParse(this.toolStripTextBoxTransparency.Text, out b))
            {
                this._Setting.SymbolTransparency = b;
                this.setControl();
            }
        }

        #region Symbol

        private System.Collections.Generic.Dictionary<Setting.Symbol, System.Drawing.Image> _Symbols;

        private void initSymbols()
        {
            this._Symbols = new Dictionary<Setting.Symbol, Image>();

            this._Symbols.Add(Setting.Symbol.Circle, this.imageListSymbols.Images[7]);
            this._Symbols.Add(Setting.Symbol.CircleFilled, this.imageListSymbols.Images[8]);

            this._Symbols.Add(Setting.Symbol.Square, this.imageListSymbols.Images[11]);
            this._Symbols.Add(Setting.Symbol.SquareFilled, this.imageListSymbols.Images[12]);

            this._Symbols.Add(Setting.Symbol.Diamond, this.imageListSymbols.Images[0]);
            this._Symbols.Add(Setting.Symbol.DiamondFilled, this.imageListSymbols.Images[1]);

            //this._Symbols.Add(Setting.Symbol.TriangleUp, this.imageListSymbols.Images[2]);
            //this._Symbols.Add(Setting.Symbol.TriangleUpFilled, this.imageListSymbols.Images[5]);

            //this._Symbols.Add(Setting.Symbol.TriangleDown, this.imageListSymbols.Images[3]);
            //this._Symbols.Add(Setting.Symbol.TriangleDownFilled, this.imageListSymbols.Images[4]);

            this._Symbols.Add(Setting.Symbol.Cross, this.imageListSymbols.Images[9]);
            this._Symbols.Add(Setting.Symbol.X, this.imageListSymbols.Images[13]);
            //this._Symbols.Add(Setting.Symbol.Minus, this.imageListSymbols.Images[10]);
            //this._Symbols.Add(Setting.Symbol.Questionmark, this.imageListSymbols.Images[6]);

            foreach (System.Collections.Generic.KeyValuePair<Setting.Symbol, System.Drawing.Image> KV in this._Symbols)
            {
                System.Windows.Forms.ToolStripMenuItem MI = new ToolStripMenuItem(KV.Key.ToString(), KV.Value, SymbolToolStripMenuItem_Click);
                MI.Tag = KV.Key;
                this.toolStripDropDownButtonSymbol.DropDownItems.Add(MI);
            }
        }

        private void SymbolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolStripMenuItem MI = (System.Windows.Forms.ToolStripMenuItem)sender;
            this._Setting.setSymbol((Setting.Symbol)MI.Tag);
            this.toolStripDropDownButtonSymbol.Image = MI.Image;

        }
        
        #endregion

    }
}
