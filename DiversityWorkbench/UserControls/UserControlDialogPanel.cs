using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.UserControls
{
    public partial class UserControlDialogPanel : UserControl
    {
        #region Events
        public event EventHandler<EventArgs> ButtonClicked;

        protected virtual void OnButtonClicked(EventArgs e)
        {
            EventHandler<EventArgs> handler = ButtonClicked;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        #endregion

        public enum DisplayOption { Cancel_OK, No_Yes, CancelNo_Yes }
        private DisplayOption _DisplayOption = DisplayOption.Cancel_OK;

        public UserControlDialogPanel()
        {
            InitializeComponent();
        }

        public DisplayOption Display
        {
            set
            {
                this._DisplayOption = value;
                switch (this._DisplayOption)
                {
                    case DisplayOption.Cancel_OK:
                        break;
                    case DisplayOption.CancelNo_Yes:
                        this.buttonCancel.Visible = true;
                        this.buttonNo.Visible = true;
                        this.buttonOK.Visible = false;
                        this.buttonYes.Visible = true;
                        break;
                    case DisplayOption.No_Yes:
                        this.buttonCancel.Visible = false;
                        this.buttonNo.Visible = true;
                        this.buttonOK.Visible = false;
                        this.buttonYes.Visible = true;
                        break;
                }
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            OnButtonClicked(e);
            System.Windows.Forms.Form f = this.ParentForm;
            f.DialogResult = DialogResult.Cancel;
            f.Close();
        }

        public void buttonOK_Click(object sender, EventArgs e)
        {
            OnButtonClicked(e);
            System.Windows.Forms.Form f = this.ParentForm;
            f.DialogResult = DialogResult.OK;
            f.Close();
        }

        private void buttonNo_Click(object sender, EventArgs e)
        {
            OnButtonClicked(e);
            System.Windows.Forms.Form f = this.ParentForm;
            f.DialogResult = DialogResult.No;
            f.Close();
        }

        private void buttonYes_Click(object sender, EventArgs e)
        {
            OnButtonClicked(e);
            System.Windows.Forms.Form f = this.ParentForm;
            f.DialogResult = DialogResult.Yes;
            f.Close();
        }
    }
}
