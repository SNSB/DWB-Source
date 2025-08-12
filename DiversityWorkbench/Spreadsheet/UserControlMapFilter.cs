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
    public partial class UserControlMapFilter : UserControl
    {

        #region Parameter

        private MapFilter _MapFilter;
        //private System.Windows.Forms.Panel _ParentPanel;
        private Sheet _Sheet;
        private iMapSymbolForm _MapSymbolForm;

        #endregion

        public UserControlMapFilter(MapFilter Filter, iMapSymbolForm MapSymbolForm, Sheet Sheet)
        {
            InitializeComponent();
            this._MapFilter = Filter;
            this._MapSymbolForm = MapSymbolForm;
            this.groupBox.Text = this._MapFilter.FilterType().ToString();
            switch (this._MapFilter.FilterType())
            {
                case MapFilter.FilterTypes.Geography:
                    this.splitContainerDetails.Panel1Collapsed = false;
                    this.splitContainerDetails.Panel2Collapsed = true;
                    int i = 0;
                    foreach (System.Collections.Generic.KeyValuePair<string, MapFilter.GeographyFilterTypes> KV in this.GeographyFilterTypes())
                    {
                        this.comboBoxGeographyConversionType.Items.Add(KV.Key);
                        if (this._MapFilter.GeographyFilterType() == KV.Value)
                            this.comboBoxGeographyConversionType.SelectedIndex = i;
                        i++;
                    }
                    i = 0;
                    foreach (System.Collections.Generic.KeyValuePair<string, ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityGazetteer"].ServerConnections())
                    {
                        this.comboBoxGazetteer.Items.Add(KV.Value.DisplayText);
                        if (KV.Value.DisplayText == this._MapFilter.GeographyGazetteer())
                            this.comboBoxGazetteer.SelectedIndex = i;
                        i++;
                    }
                    this.Height = 66;
                    break;
                case MapFilter.FilterTypes.Color:
                    this.splitContainerDetails.Panel1Collapsed = true;
                    this.splitContainerDetails.Panel2Collapsed = false;
                    this.splitContainerColorSymbol.Panel1Collapsed = false;
                    this.splitContainerColorSymbol.Panel2Collapsed = true;
                    this.comboBoxColorFilterType.Items.Add(MapFilter.ColorFilterTypes.Maximum.ToString());
                    this.comboBoxColorFilterType.SelectedIndex = 0;
                    this.Height = 47;
                    break;
                case MapFilter.FilterTypes.Symbol:
                    this.splitContainerDetails.Panel1Collapsed = true;
                    this.splitContainerDetails.Panel2Collapsed = false;
                    this.splitContainerColorSymbol.Panel1Collapsed = true;
                    this.splitContainerColorSymbol.Panel2Collapsed = false;
                    this.initSymbols();
                    this.Height = 47;
                    break;
            }
            this.SetForwardType(this._MapFilter.ForwardType);
            this._Sheet = Sheet;
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            this._Sheet.MapFilterList.Remove(this._MapFilter.Position());
            this._MapSymbolForm.initFilterList();
            //this._ParentPanel.Controls.Remove(this);
            this.Dispose();
        }

        private void buttonForwardType_Click(object sender, EventArgs e)
        {
            if (this._MapFilter.ForwardType == MapFilter.ForwardTypes.Allways)
                this.SetForwardType(MapFilter.ForwardTypes.RestrictToSuccess);
            else
                this.SetForwardType(MapFilter.ForwardTypes.Allways);
        }

        private void SetForwardType(MapFilter.ForwardTypes Type)
        {
            this._MapFilter.ForwardType = Type;
            switch (this._MapFilter.ForwardType)
            {
                case MapFilter.ForwardTypes.Allways:
                    this.buttonForwardType.Image = this.imageListForwardType.Images[0];
                    this.toolTip.SetToolTip(this.buttonForwardType, "Execute following filter in any case");
                    break;
                case MapFilter.ForwardTypes.RestrictToSuccess:
                    this.buttonForwardType.Image = this.imageListForwardType.Images[1];
                    this.toolTip.SetToolTip(this.buttonForwardType, "Execute following filter only if current filter is successful");
                    break;
            }
        }

        #region Moving

        private void buttonUp_Click(object sender, EventArgs e)
        {

        }

        private void buttonDown_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Geography

        private void comboBoxGeographyConversionType_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        private System.Collections.Generic.Dictionary<string, MapFilter.GeographyFilterTypes> _GeographyFilterTypes;
        private System.Collections.Generic.Dictionary<string, MapFilter.GeographyFilterTypes> GeographyFilterTypes()
        {
            if (this._GeographyFilterTypes == null)
            {
                this._GeographyFilterTypes = new Dictionary<string, MapFilter.GeographyFilterTypes>();
                this._GeographyFilterTypes.Add("Quadrant", MapFilter.GeographyFilterTypes.Quadrant);
                this._GeographyFilterTypes.Add("Center of quadrant", MapFilter.GeographyFilterTypes.CenterOfQuadrant);
            }
            return this._GeographyFilterTypes;
        }

        private void comboBoxGazetteer_SelectedIndexChanged(object sender, EventArgs e)
        {
            this._MapFilter.setGeographyGazetteer(this.comboBoxGazetteer.SelectedItem.ToString());
            this.toolTip.SetToolTip(this.comboBoxGazetteer, this._MapFilter.GeographyGazetteer());
        }

        private void comboBoxGazetteer_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this._MapFilter.ResetGazetteerConnection();
        }

        #endregion

        #region Symbol

        private void buttonSymbolAdd_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.List<string> L = this._MapFilter.MissingSymbols();
            if (L.Count > 0)
            {
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(L, "Symbol value", "Please select the value that should be added next", true);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    this._MapFilter.AddSymbol(f.SelectedString);
                    this.initSymbols();
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No further value left");
            }
        }

        private void buttonSymbolValuesClear_Click(object sender, EventArgs e)
        {
            this._MapFilter.ClearSymbols();
            this.initSymbols();
        }

        private void initSymbols()
        {
            this.panelSymbols.Controls.Clear();
            foreach (System.Collections.Generic.KeyValuePair<string, int> KV in this._MapFilter.SymbolValueSequence())
            {
                System.Windows.Forms.Button B = new Button();
                B.Text = KV.Key;
                B.Width = 24 + KV.Key.Length * 4;
                B.FlatStyle = FlatStyle.Flat;
                B.Dock = DockStyle.Left;
                B.Margin = new Padding(0);
                B.Padding = new Padding(0);
                B.Click += new EventHandler(buttonSymbolRemove_Click);
                this.toolTip.SetToolTip(B, "Remove the entry " + KV.Key + " from the list");
                this.panelSymbols.Controls.Add(B);
                B.BringToFront();
            }
        }

        private void buttonSymbolRemove_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button B = (System.Windows.Forms.Button)sender;
            this._MapFilter.RemoveSymbol(B.Text);
            this.initSymbols();
        }

        #endregion

        #region Color

        private void comboBoxColorFilterType_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        #endregion

    }
}
