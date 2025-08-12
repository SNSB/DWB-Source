using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Export
{
    public partial class UserControlTransformationFilter : UserControl
    {
        private DiversityWorkbench.Export.TransformationFilter _Filter;
        private Export.UserControlTransformation _UserControlTransformation;

        public UserControlTransformationFilter(DiversityWorkbench.Export.TransformationFilter Filter, Export.UserControlTransformation U)//, iTransformationInterface iTransformation)
        {
            InitializeComponent();
            this._Filter = Filter;
            this._UserControlTransformation = U;
            this.initControl();
        }

        private void initControl()
        {
            try
            {
                this.comboBoxFilterOperator.Text = this._Filter.FilterOperator;
                this.textBoxFilter.Text = this._Filter.Filter;
                if (this._Filter.PositionOfFilterColumn != this._Filter.Transformation.FileColumn.Position)
                {
                    this.buttonFilterColumn.Text = this._Filter.PositionOfFilterColumn.ToString();
                    this.buttonFilterColumn.Image = null;
                }
                else
                {
                    this.buttonFilterColumn.Text = "";
                    this.buttonFilterColumn.Image = DiversityWorkbench.Properties.Resources.MarkColumn;
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void buttonFilterColumn_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.Form f = new Form();
                f.Text = "Select a file column for filtering";
                f.ShowIcon = false;
                f.StartPosition = FormStartPosition.CenterParent;
                System.Windows.Forms.Panel p = new Panel();
                p.Dock = DockStyle.Fill;
                p.AutoScroll = true;
                f.Controls.Add(p);
                System.Collections.Generic.List<int> LL = new List<int>();
                //System.Collections.Generic.List<double> LL = new List<double>();
                this._FileColumnsForFilter = new List<UserControlFileColumn>();
                foreach (System.Collections.Generic.KeyValuePair<int, Export.FileColumn> KV in Export.Exporter.FileColumnList)
                    LL.Add(KV.Key);
                //foreach (System.Collections.Generic.KeyValuePair<double, Export.FileColumn> KV in Export.Exporter.FileColumnList)
                //    LL.Add(KV.Key);
                for (int i = LL.Count - 1; i >= 0; i--)
                {
                    Export.UserControlFileColumn U = new UserControlFileColumn(Export.Exporter.FileColumnList[LL[i]], "");
                    U.buttonFilter.Click += new System.EventHandler(this.buttonFilter_Click);
                    if (Export.Exporter.FileColumnList[LL[i]].Position == this._Filter.Transformation.FileColumn.Position)
                    {
                        U.BackColor = System.Drawing.Color.Yellow;
                        //U.buttonFilter.Enabled = false;
                        //U.buttonFilter.Text = "";
                        //U.buttonFilter.Image = null;
                        //U.buttonFilter.FlatAppearance.BorderSize = 0;
                        //U.buttonFilter.FlatStyle = FlatStyle.Flat;
                    }
                    else if (Export.Exporter.FileColumnList[LL[i]].Position == this._Filter.PositionOfFilterColumn)
                        U.SetAsSelected(true);
                    this._FileColumnsForFilter.Add(U);
                    U.Dock = DockStyle.Left;
                    U.BringToFront();
                    p.Controls.Add(U);
                }
                int MaxWidth = (LL.Count * 164) + 16;
                if (MaxWidth > 1200) MaxWidth = 1200;
                f.Width = MaxWidth;
                f.Height = 200;
                f.ShowDialog();
                if (this._Filter.PositionOfFilterColumn != this._Filter.Transformation.FileColumn.Position)
                    this.initControl();
            }
            catch (System.Exception ex)
            {
            }

        }

        private System.Collections.Generic.List<Export.UserControlFileColumn> _FileColumnsForFilter;

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Export.UserControlFileColumn U in this._FileColumnsForFilter)
                {
                    if (U.FilterPosition() != this._Filter.Transformation.FileColumn.Position)
                        U.SetAsSelected(false);
                }
                System.Windows.Forms.Button B = (System.Windows.Forms.Button)sender;
                System.Windows.Forms.TableLayoutPanel T = (System.Windows.Forms.TableLayoutPanel)B.Parent;
                Export.UserControlFileColumn Uselected = (Export.UserControlFileColumn)T.Parent;
                Uselected.SetAsSelected(true);
                this._Filter.PositionOfFilterColumn = Uselected.FilterPosition();
            }
            catch (System.Exception ex)
            {
            }
        }


        private void comboBoxFilterOperator_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this._Filter.FilterOperator = this.comboBoxFilterOperator.SelectedItem.ToString();
            this.initControl();
        }

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            this._Filter.Filter = this.textBoxFilter.Text;
        }

        private void comboBoxFilterOperator_DropDown(object sender, EventArgs e)
        {
            try
            {
                if (this.comboBoxFilterOperator.Items.Count == 0)
                {
                    foreach (string s in this._Filter.FilterOperators)
                        this.comboBoxFilterOperator.Items.Add(s);
                }
            }
            catch (System.Exception ex) { }
        }

        private void buttonRemoveFilter_Click(object sender, EventArgs e)
        {
            try
            {
                this._UserControlTransformation.RemoveFilterCondition(this._Filter);
            }
            catch (System.Exception ex) { }
        }
    }
}
