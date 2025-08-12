using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.LabelEditor
{
    public partial class UserControlLabelRow : UserControl
    {
        #region Parameter

        private DiversityCollection.LabelEditor.LabelRow _LabelRow;
        
        #endregion

        #region Construction

        public UserControlLabelRow(ref DiversityCollection.LabelEditor.LabelRow LabelRow)
        {
            InitializeComponent();
            this.maskedTextBoxHeight.Text = LabelRow.RowHeight.ToString();
            this._LabelRow = LabelRow;
            this.Height = this._LabelRow.RowHeight;
        }
        
        #endregion

        #region Public propertries
        
        public int RowHeight
        {
            get { return this._LabelRow.RowHeight; }
            set
            {
                this._LabelRow.RowHeight = value;
                this.Height = value;
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
