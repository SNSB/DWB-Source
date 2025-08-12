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
    public partial class UserControlXslTableRow : UserControl
    {
        #region Parameter

        private DiversityWorkbench.XslEditor.XslTableRow _XslTableRow;
        private double _Zoom = 1;
        
        #endregion

        #region Construction

        public UserControlXslTableRow(ref DiversityWorkbench.XslEditor.XslTableRow TableRow)
        {
            InitializeComponent();
            this.maskedTextBoxHeight.Text = TableRow.RowHeight.ToString();
            this._XslTableRow = TableRow;
            this.Height = this._XslTableRow.RowHeight;
        }

        #endregion

        #region Public propertries
        
        public int RowHeight
        {
            get { return this._XslTableRow.RowHeight; }
            set
            {
                this._XslTableRow.RowHeight = value;
                this.Height = (int)(Math.Round(value * this._Zoom, 0));
            }
        }

        public double ZoomFactor
        {
            set
            {
                this._Zoom = value;
                this.Height = (int)(Math.Round(this._XslTableRow.RowHeight * this._Zoom, 0));
            }
        }

        #endregion

        #region Usercontrol events etc.
        
        private void maskedTextBoxHeight_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.RowHeight = int.Parse(this.maskedTextBoxHeight.Text);
            }
            catch { }
        }
        
        private void buttonOptions_Click(object sender, EventArgs e)
        {

        }

        #endregion


    }
}
