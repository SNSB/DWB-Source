using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DiversityWorkbench
{
    public class LinkedServer
    {

        #region Parameter

        //private System.Collections.Generic.Dictionary<string, string> _DatabaseBaseURLs;
        //private System.Collections.Generic.List<string> _DatabaseList;
        private string _ServerName;
        //private string _UserName;
        //private string _Password;
        private bool _ServerIsValid = true;

        #endregion

        #region Construction

        public LinkedServer(string ServerName)
        {
            this._ServerName = ServerName;
        }

        #endregion

        #region Properties and functions

        private System.Collections.Generic.Dictionary<string, DiversityWorkbench.LinkedServerDatabase> _Databases;
        public System.Collections.Generic.Dictionary<string, DiversityWorkbench.LinkedServerDatabase> Databases(string Module = "", bool Requery = false)
        {
            // Toni 15.11.22 Wenn Server nicht merh existiert, nach dem ersten abgelehnten Zugriff deaktivieren!
            if (this._ServerIsValid && (this._Databases == null || this._Databases.Count == 0 || Requery)) // Markus 14.10.22 - Liste war leer - auch dann neu fuellen
            {
                this._Databases = new Dictionary<string, LinkedServerDatabase>();
                string Restriction = "Diversity";
                if (Module.Length > 0) Restriction = Module + "";
                string SQL = "select d.name from [" + this._ServerName + "].master.dbo.sysdatabases d where d.name like '" + Restriction + "%'";
                System.Data.DataTable dt = new DataTable();
                // Markus 12.4.23: Connection timeout statt Query timeout verwenden
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString + ";Connection Timeout=" + DiversityWorkbench.Settings.TimeoutConnection.ToString() + ";");
                ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutConnection;
                try
                {
                    ad.Fill(dt);
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        SQL = "select TOP 1 BaseURL from [" + this._ServerName + "]." + R[0].ToString() + ".dbo.ViewBaseURL";
                        string Message = "";
                        string BaseURL = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message, false, true);
                        if (BaseURL.Length > 0 && Message.Length == 0)
                        {
                            SQL = "select TOP 1 DiversityWorkbenchModule from [" + this._ServerName + "]." + R[0].ToString() + ".dbo.ViewDiversityWorkbenchModule";
                            Message = "";
                            string DiversityWorkbenchModule = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message, false, true);
                            if (DiversityWorkbenchModule.Length > 0 && Message.Length == 0)
                            {
                                DiversityWorkbench.LinkedServerDatabase LSD = new LinkedServerDatabase(this, R[0].ToString());
                                LSD.BaseURL = BaseURL;
                                LSD.DiversityWorkbenchModule = DiversityWorkbenchModule;
                                if (!this._Databases.ContainsKey(LSD.DatabaseName))
                                    this._Databases.Add(LSD.DatabaseName, LSD);
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    this._ServerIsValid = false;
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            return this._Databases;
        }

        public System.Collections.Generic.Dictionary<string, DiversityWorkbench.LinkedServerDatabase> Databases(string Module)
        {
            System.Collections.Generic.Dictionary<string, DiversityWorkbench.LinkedServerDatabase> DBs = new Dictionary<string, LinkedServerDatabase>();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.LinkedServerDatabase> KV in this.Databases())
            {
                if (KV.Value.DiversityWorkbenchModule == Module)
                    DBs.Add(KV.Key, KV.Value);
            }
            return DBs;
        }

        public System.Collections.Generic.List<string> DatabaseList(string Module)
        {
            System.Collections.Generic.List<string> List = new List<string>();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.LinkedServerDatabase> KV in this.Databases())
            {
                if (KV.Value.DiversityWorkbenchModule == Module)
                    List.Add(KV.Key);
            }
            return List;
        }


        //public System.Collections.Generic.List<string> DatabaseList()
        //{
        //    if (this._DatabaseList == null)
        //    {
        //        string SQL = "select d.name from [" + this._ServerName + "].master.dbo.sysdatabases d where d.name like 'Diversity%'";
        //        System.Data.DataTable dt = new DataTable();
        //        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
        //        ad.Fill(dt);
        //        this._DatabaseList = new List<string>();
        //        foreach (System.Data.DataRow R in dt.Rows)
        //        {
        //            SQL = "select TOP 1 BaseURL from [" + this._ServerName + "]." + R[0].ToString() + ".dbo.ViewBaseURL";
        //            string Message = "";
        //            string BaseURL = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message);
        //            if (BaseURL.Length > 0 && Message.Length == 0)
        //                this._DatabaseList.Add(R[0].ToString());
        //        }
        //    }
        //    return this._DatabaseList;
        //}

        //public System.Collections.Generic.List<string> DatabaseList(string Module)
        //{
        //    System.Collections.Generic.List<string> List = new List<string>();
        //    string SQL = "select d.name from [" + this._ServerName + "].master.dbo.sysdatabases d where d.name like '" + Module + "%'";
        //    System.Data.DataTable dt = new DataTable();
        //    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
        //    ad.Fill(dt);
        //    foreach (System.Data.DataRow R in dt.Rows)
        //    {
        //        SQL = "select TOP 1 DiversityWorkbenchModule from [" + this._ServerName + "]." + R[0].ToString() + ".dbo.ViewDiversityWorkbenchModule";
        //        string Message = "";
        //        string DiversityWorkbenchModule = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message);
        //        if (DiversityWorkbenchModule == Module && Message.Length == 0)
        //            List.Add(R[0].ToString());
        //    }
        //    return List;
        //}



        //public string BaseURL(string Database)
        //{
        //    string User = "";
        //    string Password = "";
        //    string Port = "";
        //    string Server = "";
        //    bool IsTrusted = true;
        //    if (this._DatabaseBaseURLs == null)
        //    {
        //        this._DatabaseBaseURLs = new Dictionary<string, string>();
        //        foreach (string D in this.DatabaseList())
        //        {
        //            string BaseURL = "";
        //            DiversityWorkbench.ServerConnection SC = new ServerConnection();
        //            if (this._ServerName.Length > 0)
        //            {
        //                try
        //                {
        //                    string[] SS = this._ServerName.Split(new char[] { ',' });
        //                    SC.DatabaseServer = SS[0];
        //                    SC.DatabaseServerPort = int.Parse(SS[1]);
        //                    SC.DatabaseName = D;
        //                    if (DiversityWorkbench.Settings.IsTrustedConnection)
        //                    {
        //                        SC.IsTrustedConnection = true;
        //                    }
        //                    else
        //                    {
        //                        SC.DatabasePassword = DiversityWorkbench.Settings.Password;
        //                        SC.DatabaseUser = DiversityWorkbench.Settings.DatabaseUser;
        //                    }
        //                    if (SC.ConnectionIsValid)
        //                    {
        //                        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(SC.ConnectionString);
        //                        Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand("SELECT dbo.BaseURL()", con);
        //                        con.Open();
        //                        BaseURL = C.ExecuteScalar().ToString();
        //                        con.Close();
        //                    }
        //                }
        //                catch (System.Exception ex) { }
        //            }
        //            this._DatabaseBaseURLs.Add(D, BaseURL);
        //        }
        //    }
        //    if (this._DatabaseBaseURLs[Database] != null && this._DatabaseBaseURLs[Database].Length == 0)
        //    {
        //        DiversityWorkbench.ServerConnection sc = new ServerConnection();
        //        sc.DatabaseName = Database;
        //        sc.IsTrustedConnection = IsTrusted;
        //        if (Server.Length > 0)
        //            sc.DatabaseServer = Server;
        //        else
        //        {
        //            string[] SS = this._ServerName.Split(new char[] { ',' });
        //            sc.DatabaseServer = SS[0];
        //            if (Port.Length > 0)
        //                sc.DatabaseServerPort = int.Parse(Port);
        //            else
        //                sc.DatabaseServerPort = int.Parse(SS[1]);
        //        }
        //        if (Password.Length > 0)
        //            sc.DatabasePassword = Password;
        //        if (User.Length > 0)
        //            sc.DatabaseUser = User;
        //        foreach(System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.WorkbenchUnit> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList())
        //        {
        //            if (Database.StartsWith(KV.Key))
        //            {
        //                sc.ModuleName = KV.Key;
        //                break;
        //            }
        //        }
        //        DiversityWorkbench.Forms.FormConnectToDatabase f = new Forms.FormConnectToDatabase(sc, true);
        //        f.ShowDialog();
        //        if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
        //        {
        //            this._DatabaseBaseURLs[Database] = f.ServerConnection.BaseURL;
        //            User = f.User;
        //            Port = f.Port.ToString();
        //            Server = f.Server;
        //            Password = f.Password;
        //            IsTrusted = f.IsTrusted;
        //        }
        //    }
        //    return this._DatabaseBaseURLs[Database];
        //}

        #endregion

        #region static functions

        private static System.Collections.Generic.Dictionary<string, LinkedServer> _LinkedServers;

        public static System.Collections.Generic.Dictionary<string, LinkedServer> LinkedServers()
        {
            if (LinkedServer._LinkedServers == null)
            {
                LinkedServer._LinkedServers = new Dictionary<string, LinkedServer>();
                string SQL = "select s.name from sys.servers s where s.is_linked = 1";
                System.Data.DataTable dtServer = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dtServer);
                foreach (System.Data.DataRow R in dtServer.Rows)
                {
                    LinkedServer LS = new LinkedServer(R[0].ToString());
                    LinkedServer._LinkedServers.Add(R[0].ToString(), LS);
                }
            }
            return LinkedServer._LinkedServers;
        }

        public static void ResetLinkedServer()
        {
            DiversityWorkbench.LinkedServer._LinkedServers = null;
        }

        public static bool AddLinkedServer(string Server, string UserName, string Password)
        {
            bool OK = false;
            string message = "";
            if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery("USE [master]; EXEC master.dbo.sp_addlinkedserver @server = N'" + Server + "', @srvproduct=N'SQL Server'", ref message))
            {
                if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery("USE [master]; EXEC master.dbo.sp_addlinkedsrvlogin @rmtsrvname=N'" + Server + "',@useself=N'False',@locallogin=NULL,@rmtuser=N'" + UserName + "',@rmtpassword='" + Password + "'"))
                {
                    if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery("USE [master]; EXEC master.dbo.sp_serveroption @server=N'" + Server + "', @optname=N'collation compatible', @optvalue=N'false'"))
                    {
                        if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery("USE [master]; EXEC master.dbo.sp_serveroption @server=N'" + Server + "', @optname=N'data access', @optvalue=N'true'"))
                        {
                            if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery("USE [master]; EXEC master.dbo.sp_serveroption @server=N'" + Server + "', @optname=N'dist', @optvalue=N'false'"))
                            {
                                if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery("USE [master]; EXEC master.dbo.sp_serveroption @server=N'" + Server + "', @optname=N'pub', @optvalue=N'false'"))
                                {
                                    System.Windows.Forms.MessageBox.Show("Linked server " + Server + " created");
                                    OK = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    message = "Failed to create user " + Server + (message == "" ? "" : "\r\n\r\n" + message);
                    System.Windows.Forms.MessageBox.Show(message);
                }
            }
            else
            {
                message = "Failed to create server " + Server + (message == "" ? "" : "\r\n\r\n" + message);
                System.Windows.Forms.MessageBox.Show(message);
            }
            if (OK)
            {
                LinkedServer LS = new LinkedServer(Server);
                try
                {
                    LinkedServer.LinkedServers().Add(Server, LS);
                }
                catch (System.Exception ex) { }
            }
            return OK;
        }

        public static bool DropLinkedServer(string Server)
        {
            string message = "";
            string SQL = "USE [master]; EXEC master.dbo.sp_dropserver @server=N'" + Server + "', @droplogins='droplogins'";
            if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref message))
                return true;
            else
            {
                message = "Failed to delete server " + Server + (message == "" ? "" : "\r\n\r\n" + message);
                System.Windows.Forms.MessageBox.Show(message);
            }
            return false;
        }

        #endregion

    }
}
