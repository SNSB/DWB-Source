using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Import
{
    public partial class UserControlTransformationFilter : UserControl
    {
        private DiversityWorkbench.Import.TransformationFilter _Filter;
        private DiversityWorkbench.Import.iTransformationInterface _iTransformation;

        public UserControlTransformationFilter(DiversityWorkbench.Import.TransformationFilter Filter, iTransformationInterface iTransformation)
        {
            InitializeComponent();
            this._Filter = Filter;
            this._iTransformation = iTransformation;
            this.initControl();
        }

        private void initControl()
        {
            try
            {
                if (this.comboBoxFilterOperator.Items.Count == 0)
                {
                    this.comboBoxFilterOperator.Items.Add("=");
                    this.comboBoxFilterOperator.Items.Add("≠");
                }
                this.comboBoxFilterOperator.Text = this._Filter.FilterOperator;
                this.textBoxFilter.Text = this._Filter.Filter;
                if (this._Filter.FilterColumn != (int)this._Filter.Transformation.iDataColumn.PositionInFile())
                {
                    this.buttonFilterColumn.Text = this._Filter.FilterColumn.ToString();
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
                DiversityWorkbench.Import.FormFileDataGrid f = new FormFileDataGrid(this._iTransformation.DataGridCopy(50), this._Filter.FilterColumn);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK && f.SelectedFileColumn() != null)
                {
                    int i = (int)f.SelectedFileColumn();
                    this._Filter.FilterColumn = i;
                    this.initControl();
                }
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
                this._iTransformation.RemoveFilterConditin(this._Filter);
                //this._Filter.Transformation.FilterConditions.Remove(this._Filter);
                //this._iTransformation.initControl();
            }
            catch (System.Exception ex) { }
        }
    }
}
