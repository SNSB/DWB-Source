using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.UserControls
{
    public partial class UserControl_Transaction : UserControl__Data
    {
        #region Construction

        public UserControl_Transaction(
            iMainForm MainForm,
            System.Windows.Forms.BindingSource Source,
            string HelpNamespace)
            : base(MainForm, Source, HelpNamespace)
        {
            InitializeComponent();
            this._HelpNamespace = HelpNamespace;
            this.initControl();
            this.FormFunctions.addEditOnDoubleClickToTextboxes();
            this.FormFunctions.setDescriptions(this);
        }
        #endregion

        #region Interface

        public override void SetPosition(int Position)
        {
            base.SetPosition(Position);
            if (this.groupBoxTransaction.Text.Length == 0) this.groupBoxTransaction.Text = "Transaction";
            this.groupBoxTransaction.ForeColor = System.Drawing.Color.Brown;
            this.pictureBoxTransaction.Image = DiversityCollection.Specimen.getImage(Specimen.OverviewImage.Transaction);
            if (this.groupBoxTransaction.Text.Length == 0)
            {
                System.Collections.Generic.Dictionary<string, string> dict = DiversityWorkbench.Entity.EntityInformation("CollTransactionType_Enum.Code.loan");
                this.groupBoxTransaction.Text = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, dict);
            }
            bool IsManager = this.FormFunctions.getObjectPermissions("ManagerCollectionList", DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Select);
            bool IsManagerOfCurrentTransaction = false;
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                string SQL = "SELECT TransactionID FROM [TransactionList] where TransactionID = " + R["TransactionID"].ToString();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                System.Data.DataTable dt = new DataTable();
                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                    IsManagerOfCurrentTransaction = true;
                int TransationID = int.Parse(R["TransactionID"].ToString());
                string TransactionType = DiversityCollection.LookupTable.TransactionType(TransationID);
                this.checkBoxIsOnLoan.Visible = false;
                this.groupBoxTransaction.Text = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, DiversityWorkbench.Entity.EntityInformation("CollTransactionType_Enum.Code." + TransactionType.ToLower()));
                if (this.groupBoxTransaction.Text.Length == 0) this.groupBoxTransaction.Text = "Transaction";
                this.groupBoxTransaction.ForeColor = System.Drawing.Color.Brown;
                this.pictureBoxTransaction.Image = DiversityCollection.Specimen.ImageForTransactionType(TransactionType);

                if (this.groupBoxTransaction.Text.Length == 0)
                {
                    System.Collections.Generic.Dictionary<string, string> dict = DiversityWorkbench.Entity.EntityInformation("CollTransactionType_Enum.Code.loan");
                    this.groupBoxTransaction.Text = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, dict);
                }
            }
            catch 
            {
                this.groupBoxTransaction.ForeColor = System.Drawing.Color.Brown;
            }
            if (IsManager)
            {
                this.buttonOpenTransaction.Enabled = IsManagerOfCurrentTransaction;
            }
            else
            {
                this.buttonOpenTransaction.Enabled = false;
            }
            this.setTransactionTitle();
        }

        public System.Windows.Forms.TableLayoutPanel TableLayoutPanelTransaction
        {
            get { return this.tableLayoutPanelTransaction; }
        }

        #endregion

        #region Control

        private void initControl()
        {
            this.checkBoxIsOnLoan.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this._Source, "IsOnLoan", true));
            this.textBoxTransactionAccessionNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "AccessionNumber", true));

            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this, true);

            DiversityWorkbench.Entity.setEntity(this, this.toolTip);

            this.CheckIfClientIsUpToDate();
        }

        private void buttonOpenTransaction_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                int ID = int.Parse(R["TransactionID"].ToString());
                DiversityCollection.Forms.FormTransaction f = new Forms.FormTransaction(ID);
                System.Windows.Forms.Control C = this.Parent;
                while (C.Parent != null && C.Parent.GetType() != typeof(DiversityCollection.Forms.FormCollection))
                    C = C.Parent;
                if (C.GetType().BaseType == typeof(System.Windows.Forms.Form))
                {
                    f.Width = C.Width - 20;
                    f.Height = C.Height - 20;
                }
                this.Cursor = System.Windows.Forms.Cursors.Default;
                f.ShowDialog();
                if (f.ChangeToSpecimen && f.CollectionSpecimenID != this._iMainForm.ID_Specimen())
                {
                    this._iMainForm.setSpecimen(f.CollectionSpecimenID);
                    return;
                }
                if (f.DialogResult == DialogResult.OK)
                {
                    DiversityCollection.LookupTable.ResetTransaction();
                    R["TransactionID"] = f.ID;
                    this._iMainForm.setSpecimen();
                    this.setTransactionTitle();
                }
                DiversityCollection.LookupTable.ResetTransaction();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setTransactionTitle()
        {
            this.textBoxTransaction.ForeColor = System.Drawing.Color.Brown;
            if (this._Source.Current != null)
            {
                System.Data.DataRowView RV = (System.Data.DataRowView)this._Source.Current;
                int TransactionID = 0;
                int.TryParse(RV["TransactionID"].ToString(), out TransactionID);
                this.textBoxTransaction.Text = DiversityCollection.LookupTable.TransactionTitle(TransactionID);
            }
        }

        #endregion

    }
}
