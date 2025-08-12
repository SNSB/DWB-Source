using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.PostgreSQL
{
    public class Schema
    {
        #region Parameter

        protected DiversityWorkbench.PostgreSQL.Database _Database;

        public DiversityWorkbench.PostgreSQL.Database Database
        {
            get { return _Database; }
            //set { _Database = value; }
        }

        protected string _Name;

        public string Name
        {
            get { return _Name; }
        }
        
        #endregion

        #region Construction

        protected Schema()
        { }

        public Schema(string Name, DiversityWorkbench.PostgreSQL.Database Database)
        {
            this._Database = Database;
            this._Name = Name;
        }
        
        #endregion

        #region Properties
        
        private System.Collections.Generic.Dictionary<string, DiversityWorkbench.PostgreSQL.Table> _Tables;

        public System.Collections.Generic.Dictionary<string, DiversityWorkbench.PostgreSQL.Table> Tables
        {
            get
            {
                if (this._Tables == null)
                {
                    this._Tables = new Dictionary<string, Table>();
                    string SQL = "SELECT table_name FROM information_schema.tables WHERE table_schema='" + this._Name + "' ORDER BY table_name";
                    System.Data.DataTable dt = new System.Data.DataTable();
                    Npgsql.NpgsqlDataAdapter ad = new Npgsql.NpgsqlDataAdapter(SQL, DiversityWorkbench.PostgreSQL.Connection.ConnectionString(this._Database.Name));// DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString());
                    ad.Fill(dt);
                    foreach(System.Data.DataRow R in dt.Rows)
                    {
                        DiversityWorkbench.PostgreSQL.Table T = new Table(R[0].ToString(), this);
                        this._Tables.Add(T.Name, T);
                    }
                    ad.Dispose();
                }
                return _Tables;
            }
        }

        private System.Collections.Generic.Dictionary<string, DiversityWorkbench.PostgreSQL.View> _Views;

        public System.Collections.Generic.Dictionary<string, DiversityWorkbench.PostgreSQL.View> Views
        {
            get
            {
                if (this._Views == null)
                {
                    this._Views = new Dictionary<string, View>();
                }
                if (this._Views.Count == 0)
                {
                    string SQL = "SELECT table_name FROM information_schema.Views WHERE table_schema='" + this._Name + "' ORDER BY table_name";
                    System.Data.DataTable dt = new System.Data.DataTable();
                    Npgsql.NpgsqlDataAdapter ad = new Npgsql.NpgsqlDataAdapter(SQL, DiversityWorkbench.PostgreSQL.Connection.ConnectionString(this._Database.Name));// 
                    ad.Fill(dt);
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        DiversityWorkbench.PostgreSQL.View V = new View(R[0].ToString(), this);
                        this._Views.Add(V.Name, V);
                    }
                    ad.Dispose();
                }
                return _Views;
            }
        }

        #endregion

        //#region Special functinallity for cache database

        //private int? _ProjectID;

        //public int? ProjectID
        //{
        //    get
        //    {
        //        if (_ProjectID != null)
        //            return _ProjectID;
        //        string SQL = "select \"" + this.Name + "\".ProjectID()";
        //        int ID;
        //        if (int.TryParse(DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL, DiversityWorkbench.PostgreSQL.Connection.ConnectionString(this._Database.Name)), out ID))
        //        {
        //            _ProjectID = ID;
        //            return _ProjectID;
        //        }
        //        else return null;
        //    }
        //    //set { _ProjectID = value; }
        //}
        
        //#endregion

    }
}
