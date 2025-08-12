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
    public partial class UserControlQueryOrderColumn : UserControl
    {
        #region Parameter

        private QueryDisplayColumn _QueryDisplayColumn;
        private int _Width;
        private string _Sorting = "ASC";
        UserControlQueryList _userControlQueryList;

        #endregion

        #region Construction

        public UserControlQueryOrderColumn(QueryDisplayColumn DisplayColumn, UserControlQueryList userControlQueryList, int Width = 10)
        {
            InitializeComponent();
            this.labelDisplayText.Text = DisplayColumn.DisplayText;
            this._QueryDisplayColumn = DisplayColumn;
            this.maskedTextBoxWidth.Text = Width.ToString();
            this._userControlQueryList = userControlQueryList;
            this.setSpacer(userControlQueryList);
        }

        #endregion

        #region Interface

        public int Width
        {
            get { return _Width; }
            set
            {
                if (value < 0)
                {
                    this.buttonSwitchDescAsc_Click(null, null);
                    value = value * -1;
                }
                this.maskedTextBoxWidth.Text = value.ToString();
            }
        }

        public string Sorting { get { return _Sorting; } }

        public void setSpacer(UserControlQueryList userControlQueryList)
        {
            if (DiversityWorkbench.Settings.QueryOptimizedByDefault)
                this.panelOptimizingSpacer.Visible = false;
            else
                this.panelOptimizingSpacer.Visible = userControlQueryList.Optimizing_AllowedForQueryList;
            if (DiversityWorkbench.Settings.QueryRememberQuerySettingsByDefault)
                this.panelRemeberQuerySettingsSpacer.Visible = false;
            else
                this.panelRemeberQuerySettingsSpacer.Visible = userControlQueryList.RememberQuerySettingsAvailable();
            this.panelSpacer1.Width = userControlQueryList.ManyOrderByColumns_Spacer1Width() - this.buttonUp.Width;// - this.buttonDown.Width;
        }

        #endregion

        #region private events

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            this._userControlQueryList.ManyOrderByColumns_RemoveColumn(this._QueryDisplayColumn.DisplayText);
        }

        private void maskedTextBoxWidth_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(this.maskedTextBoxWidth.Text, out _Width);
            // Markus 29.3.23 - Avoiding entry 0
            if (_Width == 0)
            {
                _Width = 1;
                this.maskedTextBoxWidth.Text = "1";
            }
        }

        private void pictureBoxWidth_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._userControlQueryList.ListOfIDs.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Nothing selected");
                    return;
                }
                string IDs = "";
                foreach (int ID in this._userControlQueryList.ListOfIDs)
                {
                    if (IDs.Length > 0) IDs += ", ";
                    IDs += ID.ToString();
                }
                string SQL = "SELECT MAX(LEN([" + this._QueryDisplayColumn.OrderColumn + "])) FROM [" + this._QueryDisplayColumn.TableName + "] WHERE " + this._QueryDisplayColumn.IdentityColumn + " IN (" + IDs + ")";
                int Max;
                if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out Max))
                {
                    if (Max > 99) Max = 99;
                    this.maskedTextBoxWidth.Text = Max.ToString();
                }
                // Markus 29.3.23 - setting 1 in case of NULL
                else this.maskedTextBoxWidth.Text = "1";
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void buttonSwitchDescAsc_Click(object sender, EventArgs e)
        {
            if (this._Sorting == "ASC")
            {
                this._Sorting = "DESC";
                this.buttonSwitchDescAsc.Image = DiversityWorkbench.Properties.Resources.ArrowUpSmall;
                this.toolTip.SetToolTip(this.buttonSwitchDescAsc, "Sorting is decending: Z -> A. Click to change to ascending");
            }
            else
            {
                this._Sorting = "ASC";
                this.buttonSwitchDescAsc.Image = DiversityWorkbench.Properties.Resources.ArrowDownSmall;
                this.toolTip.SetToolTip(this.buttonSwitchDescAsc, "Sorting is ascending: A -> Z. Click to change to decending");
            }
        }

        #endregion

        #region Sequence

        private void buttonUp_Click(object sender, EventArgs e)
        {
            this._userControlQueryList.ManyOrderByColumns_ChangePosition(this._QueryDisplayColumn.DisplayText, true);
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            this._userControlQueryList.ManyOrderByColumns_ChangePosition(this._QueryDisplayColumn.DisplayText, false);
        }

        #endregion

    }
}
