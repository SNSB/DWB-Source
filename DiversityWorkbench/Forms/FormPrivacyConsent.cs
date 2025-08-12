using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormPrivacyConsent : Form
    {
        public FormPrivacyConsent()
        {
            InitializeComponent();
            string SQL = "SELECT dbo.PrivacyConsentInfo()";
            string Site = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            if (Site.Length == 0)
                Site = "http://diversityworkbench.net/Portal/Default_Agreement_on_Processing_of_Personal_Data_in_DWB_Software";
            this.linkLabelUrlInfo.Text = Site;
            //if (Site.Length > 0)
            //{
            //    this.linkLabelUrlInfo.Text = Site;
            //}
            //else
            //{
            //    this.linkLabelUrlInfo.Visible = false;
            //    this.labelUrlInfo.Visible = false;
            //}
        }

        private Database.PrivacyConsent _PrivacyConsent = Database.PrivacyConsent.undecided;

        //private UserSettings.PrivacyConsent PrivacyConsentState()
        //{
        //    UserSettings.PrivacyConsent Consent = UserSettings.PrivacyConsent.undecided;
        //    string SQL = "select case when PrivacyConsent = 1 then 'consented' else case when PrivacyConsent is null then 'undecided' else 'rejected' end end " +
        //        "from UserProxy where LoginName = SUSER_SNAME()";
        //    string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
        //    if (Result == UserSettings.PrivacyConsent.consented.ToString())
        //        Consent = UserSettings.PrivacyConsent.consented;
        //    else if (Result == UserSettings.PrivacyConsent.rejected.ToString())
        //        Consent = UserSettings.PrivacyConsent.rejected;
        //    return Consent;
        //}

        private void SetPrivacyConsentState(bool Consented)
        {
            DiversityWorkbench.Database.SetPrivacyConsentState(Consented);
            //string SQL = "UPDATE U SET PrivacyConsent = ";
            //if (Consented) SQL += "1";
            //else SQL += "0";
            //SQL += " from UserProxy U where LoginName = SUSER_SNAME()";
            //DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
        }

        //private bool ColumnPrivacyConsentDoesExist()
        //{
        //    bool Exists = false;
        //    string SQL = "select count(*) " +
        //        "from INFORMATION_SCHEMA.COLUMNS C " +
        //        "where C.TABLE_NAME = 'UserProxy' and C.COLUMN_NAME = 'PrivacyConsent'";
        //    string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
        //    if (Result == "1")
        //        Exists = true;
        //    return Exists;
        //}

        private void buttonConsent_Click(object sender, EventArgs e)
        {
            this.SetPrivacyConsentState(true);
            this._PrivacyConsent = Database.PrivacyConsent.consented;
            this.Close();
        }

        private void buttonReject_Click(object sender, EventArgs e)
        {
            this.SetPrivacyConsentState(false);
            this._PrivacyConsent = Database.PrivacyConsent.rejected;
            this.Close();
        }

        public Database.PrivacyConsent PrivacyConsent() { return this._PrivacyConsent; }

        private void linkLabelUrlInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new FormWebBrowser(this.linkLabelUrlInfo.Text);
            f.ShowDialog();
        }

    }
}
