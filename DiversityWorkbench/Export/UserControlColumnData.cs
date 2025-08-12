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
    public partial class UserControlColumnData : UserControl
    {
        private Export.ColumnData _ColumnData;

        public UserControlColumnData(Export.ColumnData CD)
        {
            InitializeComponent();
            this._ColumnData = CD;
        }

        private void setControls()
        {
            this.labelSourceTable.Text = _ColumnData.TableColumn.Table.DisplayText;
            if (this._ColumnData.UnitValue != null && this._ColumnData.UnitValue.Length > 0)
                this.labelSourceColumnOrUnitvalue.Text = this._ColumnData.UnitValue;
            else
                this.labelSourceColumnOrUnitvalue.Text = this._ColumnData.TableColumn.DisplayText;
            this.textBoxPostfix.Text = this._ColumnData.Postfix;
            this.textBoxPrefix.Text = this._ColumnData.Prefix;
        }

    }
}
