using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.UserControls
{
    public partial class UserControlSpecimenList : UserControl
    {
        #region Parameter
        private string _DataTable;
        private string _DisplayColumn;
        private string _ValueColumn;
        private string[] _PkColumns;
        private System.Data.DataTable _dtList;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapter;
        private bool _ChangeToSpecimen = false;
        private int _CollectionSpecimenID;
        
        #endregion

        #region Construction
        public UserControlSpecimenList()
        {
            InitializeComponent();
        }
        
        #endregion

        #region Properties

        private Microsoft.Data.SqlClient.SqlDataAdapter SqlDataAdapter
        {
            get
            {
                if (this._SqlDataAdapter == null) this._SqlDataAdapter = new Microsoft.Data.SqlClient.SqlDataAdapter("", DiversityWorkbench.Settings.ConnectionString);
                return _SqlDataAdapter;
            }
        }

        private System.Data.DataTable DtList
        {
            get
            {
                if (this._dtList == null) this._dtList = new DataTable();
                return _dtList;
            }
            //set { _dtList = value; }
        }

        public int CollectionSpecimenID
        {
            get
            {
                int ID = -1;
                if (this.listBoxSpecimenList.SelectedIndex > -1)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxSpecimenList.SelectedItem;
                    foreach (System.Data.DataColumn C in R.DataView.Table.Columns)
                    {
                        if (C.ColumnName == "CollectionSpecimenID" || C.ColumnName == "ID")
                        {
                            ID = int.Parse(R[C.ColumnName].ToString());
                            break;
                        }
                    }
                }
                return ID;
            }
        }

        public int SpecimenCount
        {
            get
            {
                int i = 0;
                if (this._dtList != null)
                    i = this._dtList.Rows.Count;
                return i;
            }
        }

        public bool ChangeToSpecimen { get { return this._ChangeToSpecimen; } }

        public bool AllowTransfer
        {
            set
            {
                this.toolStripButtonTransfer.Visible = value;
            }
        }
        
        #endregion

        #region Specimen list

        public void setSpecimenList(string SQL, string Datatable, string DisplayColumn, string ValueColumn, string[] PkColumns)
        {
            this._DataTable = Datatable;
            this._ValueColumn = ValueColumn;
            this._DisplayColumn = DisplayColumn;
            this._PkColumns = new string[PkColumns.Length];
            for (int i = 0; i < PkColumns.Length; i++)
            {
                this._PkColumns[i] = PkColumns[i];
            }
            this.DtList.Clear();
            this.SqlDataAdapter.SelectCommand.CommandText = SQL;
            this.SqlDataAdapter.Fill(this.DtList);
            this.listBoxSpecimenList.DataSource = this.DtList;
            this.listBoxSpecimenList.DisplayMember = this._DisplayColumn;
            this.listBoxSpecimenList.ValueMember = this._ValueColumn;
        }

        public void setSpecimenList(string SQL)
        {
            //this._DataTable = Datatable;
            this._ValueColumn = "ID";
            this._DisplayColumn = "DisplayText";
            this._PkColumns = new string[1];
            this._PkColumns[0] = "ID";
            this.DtList.Clear();
            this.SqlDataAdapter.SelectCommand.CommandText = SQL;
            this.SqlDataAdapter.Fill(this.DtList);
            this.listBoxSpecimenList.DataSource = this.DtList;
            this.listBoxSpecimenList.DisplayMember = this._DisplayColumn;
            this.listBoxSpecimenList.ValueMember = this._ValueColumn;
        }

        private void setSpecimenList()
        {
            this.SqlDataAdapter.Fill(this.DtList);
            this.listBoxSpecimenList.DataSource = this.DtList;
            this.listBoxSpecimenList.DisplayMember = this._DisplayColumn;
            this.listBoxSpecimenList.ValueMember = this._ValueColumn;
            int i = this._dtList.Rows.Count;
            if (i > 0)
                this.groupBoxSpecimenList.Text = "Specimen: " + i.ToString();
            else
                this.groupBoxSpecimenList.Text = "Specimen";
        }

        public System.Collections.Generic.List<int> SelectedSpecimenIDs
        {
            get
            {
                System.Collections.Generic.List<int> IDs = new List<int>();
                foreach (System.Object o in this.listBoxSpecimenList.SelectedItems)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)o;
                    int i;
                    if (int.TryParse(R["ID"].ToString(), out i))
                        IDs.Add(i);
                }
                return IDs;
            }
        }

        #endregion

        #region toolStrip

        private void toolStripButtonFind_Click(object sender, EventArgs e)
        {
            if (this.listBoxSpecimenList.SelectedIndex > -1)
            {
                this._ChangeToSpecimen = true;
                try
                {
                    System.Windows.Forms.Form F = (System.Windows.Forms.Form)this.Parent.Parent.Parent.Parent.Parent.Parent.Parent;
                    F.Close();
                }
                catch (System.Exception ex) { }
            }
            else
                System.Windows.Forms.MessageBox.Show("Please select a specimen");
        }
        
        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                System.Collections.Generic.Stack<int> ii = new Stack<int>();
                if (this.listBoxSpecimenList.SelectedIndices.Count > 0 && this.DtList.Rows.Count > 0)
                {
                    foreach (System.Object o in this.listBoxSpecimenList.SelectedIndices)
                    {
                        int i = System.Int32.Parse(o.ToString());
                        ii.Push(i);
                    }
                    foreach (int i in ii)
                    {
                        System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxSpecimenList.Items[i];
                        this.UpdateForRemoval(rv);
                        rv.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void UpdateForRemoval(System.Data.DataRowView RV)
        {
            string SQL = "UPDATE " + this._DataTable + " SET " + this._ValueColumn + " = NULL WHERE 1 = 1 ";
            foreach (string S in this._PkColumns)
            {
                foreach (System.Data.DataColumn C in RV.DataView.Table.Columns)
                {
                    if (C.ColumnName == S)
                    {
                        SQL += " AND " + S + " = '" + RV[C.ColumnName].ToString() + "'";
                        break;
                    }
                }
            }
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand COM = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            con.Open();
            try
            {
                COM.ExecuteNonQuery();
            }
            catch 
            {
            } 
            con.Close();
        }

        #endregion

    }
}
