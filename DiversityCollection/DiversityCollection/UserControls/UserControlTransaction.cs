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
    public partial class UserControlTransaction : UserControl
    {
        private System.Data.SqlClient.SqlDataAdapter _SqlDataAdapterTransaction;


        public UserControlTransaction()
        {
            InitializeComponent();
        }

        public void setTransaction(int TransactionID)
        {
            try
            {
                string SQL = "SELECT " + DiversityCollection.Transaction.SqlFieldsPermit + " FROM TransactionPermit " +
                    "WHERE TransactionID = " + TransactionID.ToString();
                this._SqlDataAdapterTransaction = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                this._SqlDataAdapterTransaction.Fill(this.dataSetTransaction.Transaction);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

    }
}
