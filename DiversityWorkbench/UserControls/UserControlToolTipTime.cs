using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityWorkbench.UserControls
{
    public partial class UserControlToolTipTime : UserControl
    {
        #region Construction

        public UserControlToolTipTime()
        {
            InitializeComponent();
        }

        #endregion

        #region Interface

        public void initControl()
        {
            this.numericUpDownToolTipTime.Value = DiversityWorkbench.Settings.ToolTipAutoPopDelay / 1000;
            this.setToolTipTimeExplanation();
        }

        #endregion

        #region private

        private void numericUpDownToolTipTime_Click(object sender, EventArgs e)
        {
            switch (this.numericUpDownToolTipTime.Value)
            {
                case 0:
                    DiversityWorkbench.Settings.ToolTipAutoPopDelay = 1;
                    break;
                case 6:
                    DiversityWorkbench.Settings.ToolTipAutoPopDelay = 0;
                    break;
                default:
                    DiversityWorkbench.Settings.ToolTipAutoPopDelay = (int)this.numericUpDownToolTipTime.Value * 1000;
                    break;
            }
            this.setToolTipTimeExplanation();
        }

        private void setToolTipTimeExplanation()
        {
            switch (DiversityWorkbench.Settings.ToolTipAutoPopDelay)
            {
                case 0:
                    this.labelToolTipTimeExplained.Text = "no limit";
                    break;
                case 1:
                    this.labelToolTipTimeExplained.Text = "hide";
                    break;
                case 1000:
                    this.labelToolTipTimeExplained.Text = (DiversityWorkbench.Settings.ToolTipAutoPopDelay / 1000).ToString() + " second";
                    break;
                default:
                    this.labelToolTipTimeExplained.Text = (DiversityWorkbench.Settings.ToolTipAutoPopDelay / 1000).ToString() + " seconds";
                    break;
            }
        }

        #endregion
    }
}
