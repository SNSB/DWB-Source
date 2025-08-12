using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Spreadsheet
{
    public partial class FormUserSettings : Form
    {

        #region Parameter

        private string _Title = "";
        private string _Info = "";
        private bool _AllowReset = true;
        
        #endregion

        #region Construction

        public FormUserSettings(string Title = "", string Info = "", bool AllowReset = true)
        {
            InitializeComponent();
            this._Info = Info;
            this._AllowReset = AllowReset;
            this._Title = Title;
            this.initForm();
        }

        public FormUserSettings(System.Collections.Generic.List<string> Settings) : this()
        {
            this.userControlUserSettings.MarkSettings(Settings);
        }

        public FormUserSettings(System.Collections.Generic.List<string> Settings, string Title = "", string Info = "", bool AllowReset = true)
            : this(Title, Info, AllowReset)
        {
            this.userControlUserSettings.MarkSettings(Settings);
        }

        #endregion

        #region Form
        
        private void initForm()
        {
            if (!this._AllowReset)
                this.userControlUserSettings.AllowReset(this._AllowReset);
            if (this._Info.Length > 0)
            {
                this.labelInfo.Text = this._Info;
                this.labelInfo.Visible = true;
            }
            if (this._Title.Length > 0)
                this.Text = this._Title;
        }

        #endregion

        #region Interfache

        public bool HasBeenReset() { return this.userControlUserSettings.HasBeenReset(); }
        
        #endregion

    }
}
