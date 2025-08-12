using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormGetPermit : Form
    {

        #region Parameter

        private DiversityCollection.TransactionPermit _Permit;
        private System.Windows.Forms.BindingSource _BindingSourceTransaction;
        private string _Type = "permit";

        #endregion

        #region Construction

        public FormGetPermit()
        {
            InitializeComponent();
            try
            {
                // online manual
                this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();
                string Table = "[Transaction]";
                this.initForm();
            }
            catch (System.Exception ex)
            {
            }
        }

        public FormGetPermit(string Type, System.Drawing.Image FormIcon, bool HideDetails = true)
        {
            InitializeComponent();
            try
            {
                // online manual
                this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();
                this._Type = Type;
                if (this._Type != "permit")
                   this.Text = this.Text.Replace("permit", this._Type);
                Bitmap theBitmap = new Bitmap(FormIcon, new Size(16, 16));
                IntPtr Hicon = theBitmap.GetHicon();// Get an Hicon for myBitmap.
                Icon newIcon = Icon.FromHandle(Hicon);// Create a new icon from the handle.
                this.Icon = newIcon;
                this.initForm();
                if (HideDetails)
                {
                    this.splitContainerMain.Panel2Collapsed = true;
                    this.Width = 200;
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        #region Form

        private void initForm()
        {
            try
            {
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                System.Data.DataSet DS = this.userControlPermit.DataSet();
                System.Windows.Forms.BindingSource BS = this.userControlPermit.BindingSource();
                System.Windows.Forms.TreeView TV = new TreeView();
                if (this._Permit == null)
                {
                    this._Permit = new TransactionPermit(ref DS, DS.Tables["Transaction"],
                        ref TV, this, this.userControlQueryList, this.splitContainerMain,
                        null, null, //this.imageListSpecimenList,
                        null, this.helpProvider, this.toolTip, ref BS,
                        this.userControlPermit.TabControl());
                }
                this._Permit.initForm();
                this._Permit.SetUserControlPermit(this.userControlPermit);
                this.userControlQueryList.setQueryRestriction("TransactionType = '" + this._Type + "'", "#");

                this._Permit.FormFunctions.setDescriptions();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
        #endregion

        #region Interface
        
        public int? SelectedTransactionID()
        {
            int ID = -1;
            if (this.userControlPermit.BindingSource().Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.userControlPermit.BindingSource().Current;
                if (!int.TryParse(R["TransactionID"].ToString(), out ID))
                    return null;
            }
            return ID;
        }

        public string SelectedTransaction()
        {
            string Trans = "";
            if (this.userControlPermit.BindingSource().Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.userControlPermit.BindingSource().Current;
                Trans = R["TransactionTitle"].ToString();
            }
            return Trans;
        }
        
        #endregion

    }
}
