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
    public partial class UserControlLabelColumn : UserControl
    {
        #region Parameter

        private DiversityCollection.LabelEditor.LabelColumn _LabelColumn;
        
        #endregion

        #region Construction

        public UserControlLabelColumn(ref DiversityCollection.LabelEditor.LabelColumn LC)
        {
            InitializeComponent();
            this._LabelColumn = LC;
            this.maskedTextBoxWidth.Text = LC.ColumnWidth.ToString();
        }
        
        #endregion

        #region Public propeties
        
        public int ColumnWidth
        {
            get { return this._LabelColumn.ColumnWidth; }
            set
            {
                this.Width = value;
                this._LabelColumn.ColumnWidth = value;
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
