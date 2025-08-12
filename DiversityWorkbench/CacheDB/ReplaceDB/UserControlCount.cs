using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityWorkbench.CacheDB.ReplaceDB
{
    public partial class UserControlCount : UserControl
    {

        private ReplaceCount _Count;
        private Replacement _Replacement;

        public UserControlCount(ReplaceCount Count, Replacement replacement, bool IsHeader = false)
        {
            InitializeComponent();
            this._Count = Count;
            this._Replacement = replacement;
            this.initControl(IsHeader);
        }

        private void initControl(bool IsHeader)
        {
            if (IsHeader)
            {
                this.Height = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(34);
                this.labelHeaderNew.Visible = true;
                this.labelHeaderOld.Visible = true;
                this.labelHeaderSource.Visible = true;
                this.labelHeader.Visible = true;
            }
            else
            {
                this.Height = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(18);
                this.labelHeaderNew.Visible = false;
                this.labelHeaderOld.Visible = false;
                this.labelHeaderSource.Visible = false;
                this.labelHeader.Visible = false;
            }
            this.labelSource.Text = this._Count.SourceName;
            if (this._Count.CountNew == null)
                this.labelNew.Text = "-";
            else
                this.labelNew.Text = this._Count.CountNew.ToString();
            if (this._Count.CountOld == null)
                this.labelOld.Text = "-";
            else
                this.labelOld.Text = this._Count.CountOld.ToString();
            if (this._Count.Status != Replacement.Status._None)
                this.buttonState.Image = this._Replacement.ImageStatus(this._Count.Status);
            this.toolTip.SetToolTip(this.buttonState, this._Replacement.StatusDescription(this._Count.Status));
            if ((int)this._Count.Status > (int)Replacement.Status.DataCountDifferent)
                this.BackColor = System.Drawing.Color.Pink;
            else
                this.BackColor = System.Drawing.SystemColors.Control;

        }
    }
}
