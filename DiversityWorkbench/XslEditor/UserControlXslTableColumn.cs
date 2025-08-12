using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.XslEditor
{
    public partial class UserControlXslTableColumn : UserControl
    {
        #region Parameter

        private DiversityWorkbench.XslEditor.XslTableColumn _XslTableColumn;
        private double _Zoom = 1;
        
        #endregion

        #region Construction

        public UserControlXslTableColumn(ref DiversityWorkbench.XslEditor.XslTableColumn TC)
        {
            InitializeComponent();
            this._XslTableColumn = TC;
            this.maskedTextBoxWidth.Text = TC.ColumnWidth.ToString();
        }
        
        #endregion

        #region Public propeties
        
        public int ColumnWidth
        {
            get { return this._XslTableColumn.ColumnWidth; }
            set
            {
                this.Width = (int)(Math.Round(value * this._Zoom, 0));
                this._XslTableColumn.ColumnWidth = value;
            }
        }

        public double ZoomFactor
        {
            set
            {
                this._Zoom = value;
                this.Width = (int)(Math.Round(this._XslTableColumn.ColumnWidth * this._Zoom, 0));
            }
        }
        
        #endregion

        #region User control events etc

        private void maskedTextBoxWidth_TextChanged(object sender, EventArgs e)
        {
            this.ColumnWidth = int.Parse(this.maskedTextBoxWidth.Text);
        }

        #endregion

    }
}
