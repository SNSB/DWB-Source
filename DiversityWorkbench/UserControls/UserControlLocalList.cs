using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.UserControls
{
    public enum LocalListState { TextOnly, SelectedFromList, CanSelect };
    public partial class UserControlLocalList : UserControl
    {
        #region Construction
        public UserControlLocalList()
        {
            InitializeComponent();
        }
        #endregion

        public DiversityWorkbench.UserControls.LocalListState State
        {
            set
            {
                switch (value)
                {
                    // if only the text field should be accessible
                    case DiversityWorkbench.UserControls.LocalListState.TextOnly:
                        this.comboBoxLocalValues.Visible = false;
                        this.buttonDeleteValue.Visible = false;
                        this.textBoxValue.Visible = true;
                        this.textBoxValue.ReadOnly = false;
                        this.textBoxValue.BackColor = System.Drawing.SystemColors.Window;
                        this.textBoxValue.BorderStyle = BorderStyle.Fixed3D;
                        //this.textBoxValue.Dock = DockStyle.Fill;
                        break;
                    // if ID is set
                    case DiversityWorkbench.UserControls.LocalListState.SelectedFromList:
                        this.comboBoxLocalValues.Visible = false;
                        this.buttonDeleteValue.Visible = true;
                        this.textBoxValue.Visible = true;
                        this.textBoxValue.ReadOnly = true;
                        this.textBoxValue.BackColor = System.Drawing.SystemColors.Info;
                        this.textBoxValue.BorderStyle = BorderStyle.FixedSingle;
                        //this.textBoxValue.Dock = DockStyle.Fill;
                        break;
                    // if ID is not set
                    case DiversityWorkbench.UserControls.LocalListState.CanSelect:
                        this.textBoxValue.ReadOnly = false;
                        this.textBoxValue.BackColor = System.Drawing.SystemColors.Window;
                        this.textBoxValue.BorderStyle = BorderStyle.Fixed3D;
                        this.comboBoxLocalValues.Visible = true;
                        this.buttonDeleteValue.Visible = false;
                        this.textBoxValue.Visible = true;
                        break;
                    default:
                        goto case DiversityWorkbench.UserControls.LocalListState.CanSelect;
                }
            }
        }

    }
}
