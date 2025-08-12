using DiversityWorkbench.DwbManual;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.CacheDatabase
{
    public partial class FormSetFilter : Form
    {
        #region Parameter
        private string _WhereClause = "";
        private string _Object = "";
        private bool _ForPostgres = false;
        private string _Schema = "";
        private ObjectType _ObjectType;

        public enum ObjectType { Table, View }
        #endregion

        #region Construction
        public FormSetFilter(string Object, ObjectType Type, string Schema, string WhereClause, bool ForPostgres)
        {
            InitializeComponent();
            this._WhereClause = WhereClause;
            this._Object = Object;
            this._ForPostgres = ForPostgres;
            this._Schema = Schema;
            this._ObjectType = Type;
            this.initForm();
        }
        #endregion

        #region Form & Controls Events
        private void initForm()
        {
            this.labelFilter.Text = this._WhereClause;
                System.Data.DataTable dt = new DataTable();
                string SQL = "select column_name from information_schema.columns P " +
                    "where 1 = 1 ";
            //if (this._ForPostgres)
            //{
            //    SQL += " and P.table_type = '";
            //    switch (this._ObjectType)
            //    {
            //        case ObjectType.Table:
            //            SQL += "BASE TABLE";
            //            break;
            //        case ObjectType.View:
            //            SQL += "VIEW";
            //            break;
            //    }
            //    SQL += "' ";
            //}
            SQL += " and P.Table_Name = '" + this._Object + "' and P.table_schema = '" + this._Schema + "'";
            SQL += " order by column_name";
            if (this._ForPostgres)
            {
                Npgsql.NpgsqlDataAdapter ad = new Npgsql.NpgsqlDataAdapter(SQL, DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString());
                ad.Fill(dt);
            }
            else
            {
                string Message = "";
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref Message);
            }
            this.comboBoxColumn.DataSource = dt;
            this.comboBoxColumn.DisplayMember = "column_name";
            this.comboBoxColumn.ValueMember = "column_name";
        }

        private void buttonClearFilter_Click(object sender, EventArgs e)
        {
            this._WhereClause = "";
            this.labelFilter.Text = this._WhereClause;
        }

        private void buttonAddFilter_Click(object sender, EventArgs e)
        {
            if (this._WhereClause.Length == 0)
                this._WhereClause += "WHERE ";
            else this._WhereClause += " AND ";
            if (this._ForPostgres)
                this._WhereClause += "\"" + this.comboBoxColumn.SelectedValue.ToString() + "\"";
            else
                this._WhereClause += this.comboBoxColumn.SelectedValue.ToString() + " ";
            switch (this.comboBoxOperator.Text)
            {
                case "=":
                case "<":
                case ">":
                    this._WhereClause += this.comboBoxOperator.Text;
                    break;
                case "~":
                    this._WhereClause += "LIKE";
                    break;
            }
            this._WhereClause += " '" + this.textBoxValue.Text.Replace("'", "''") + "'";
            this.labelFilter.Text = this._WhereClause;
            this.buttonAddFilter.BackColor = System.Drawing.SystemColors.Control;
        }

        public string WhereClause()
        {
            if (this.DialogResult == System.Windows.Forms.DialogResult.OK)
                return this._WhereClause;
            else return "";
        }

        private void textBoxValue_TextChanged(object sender, EventArgs e)
        {
            if (this.textBoxValue.Text.Length > 0)
                this.buttonAddFilter.BackColor = System.Drawing.Color.Red;
        }

        #endregion

        #region Manual

        /// <summary>
        /// Adding event deletates to form and controls
        /// </summary>
        /// <returns></returns>
        private async System.Threading.Tasks.Task InitManual()
        {
            try
            {

                DiversityWorkbench.DwbManual.Hugo manual = new Hugo(this.helpProvider, this);
                if (manual != null)
                {
                    await manual.addKeyDownF1ToForm();
                }
            }
            catch (Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        /// <summary>
        /// ensure that init is only done once
        /// </summary>
        private bool _InitManualDone = false;


        /// <summary>
        /// KeyDown of the form adding event deletates to form and controls within the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Form_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (!_InitManualDone)
                {
                    await this.InitManual();
                    _InitManualDone = true;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        #endregion
    }
}
