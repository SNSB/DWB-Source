using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using System.Data;

namespace DiversityWorkbench
{
    public class Postgres
    {

        #region static objects and functions
        
        //public static void PostgresConnectionReset()
        //{
        //    DiversityWorkbench.Postgres._PostgresConnection = null;
        //}

        //private static Npgsql.NpgsqlConnection _PostgresConnection;
        //public static Npgsql.NpgsqlConnection PostgresConnection()
        //{
        //    if (DiversityWorkbench.Postgres._PostgresConnection == null)
        //    {
        //        if (DiversityWorkbench.Postgres.Server.Length > 0
        //            && DiversityWorkbench.Postgres.Port.ToString().Length > 0
        //            && DiversityWorkbench.Postgres.DatabaseName.Length > 0
        //            && DiversityWorkbench.Postgres.UserName.Length > 0
        //            && DiversityWorkbench.Postgres.Password.Length > 0)
        //        {
        //            try
        //            {
        //                NpgsqlConnectionStringBuilder NC = new NpgsqlConnectionStringBuilder();
        //                NC.Database = DiversityWorkbench.PostgresSettings.Default.Database;
        //                NC.Host = DiversityWorkbench.PostgresSettings.Default.Server;
        //                NC.Port = DiversityWorkbench.PostgresSettings.Default.Port;
        //                NC.UserName = DiversityWorkbench.PostgresSettings.Default.User;
        //                NC.Password = DiversityWorkbench.Postgres.Password;
        //                DiversityWorkbench.Postgres._PostgresConnection = new NpgsqlConnection(NC);
        //            }
        //            catch (System.Exception ex)
        //            {
        //            }
        //        }
        //    }
        //    return DiversityWorkbench.Postgres._PostgresConnection;
        //}

        //public static string Password = "";

        //public static string PostgresDatabaseConnectionString(string DatabaseName)
        //{
        //    if (DiversityWorkbench.Postgres.PostgresConnection() != null)
        //    {
        //        NpgsqlConnectionStringBuilder NC = new NpgsqlConnectionStringBuilder();
        //        NC.Database = DatabaseName;
        //        NC.Host = DiversityWorkbench.PostgresSettings.Default.Server;
        //        NC.Port = DiversityWorkbench.PostgresSettings.Default.Port;
        //        NC.UserName = DiversityWorkbench.PostgresSettings.Default.User;
        //        NC.Password = DiversityWorkbench.Postgres.Password;
        //        return NC.ConnectionString;
        //    }
        //    else return "";
        //}

        //public static bool PostgresExecuteSqlNonQuery(string SQL)
        //{
        //    bool OK = false;
        //    try
        //    {
        //        if (DiversityWorkbench.Postgres.PostgresConnection() != null)
        //        {
        //            Npgsql.NpgsqlCommand C = new NpgsqlCommand(SQL, DiversityWorkbench.Postgres.PostgresConnection());
        //            if (DiversityWorkbench.Postgres.PostgresConnection().State == ConnectionState.Closed)
        //                DiversityWorkbench.Postgres.PostgresConnection().Open();
        //            C.ExecuteNonQuery();
        //            C.Dispose();
        //        }
        //        OK = true;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return OK;
        //}

        //public static bool PostgresExecuteSqlNonQuery(string SQL, ref string Message)
        //{
        //    bool OK = false;
        //    try
        //    {
        //        if (DiversityWorkbench.Postgres.PostgresConnection() != null)
        //        {
        //            Npgsql.NpgsqlCommand C = new NpgsqlCommand(SQL, DiversityWorkbench.Postgres.PostgresConnection());
        //            if (DiversityWorkbench.Postgres.PostgresConnection().State == ConnectionState.Closed)
        //                DiversityWorkbench.Postgres.PostgresConnection().Open();
        //            C.ExecuteNonQuery();
        //            C.Dispose();
        //        }
        //        OK = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Message = ex.Message;
        //    }
        //    return OK;
        //}

        //public static string PostgresExecuteSqlSkalar(string SQL)
        //{
        //    string Result = "";
        //    try
        //    {
        //        if (DiversityWorkbench.Postgres.PostgresConnection() != null)
        //        {
        //            Npgsql.NpgsqlCommand C = new NpgsqlCommand(SQL, DiversityWorkbench.Postgres.PostgresConnection());
        //            if (DiversityWorkbench.Postgres.PostgresConnection().State == ConnectionState.Closed)
        //                DiversityWorkbench.Postgres.PostgresConnection().Open();
        //            Result = C.ExecuteScalar().ToString();
        //            C.Dispose();
        //        }
        //    }
        //    catch (System.Data.SqlClient.SqlException ex)
        //    {
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return Result;
        //}

        //public static string DatabaseName
        //{
        //    get
        //    {
        //        if (DiversityWorkbench.PostgresSettings.Default.Database.Length > 0)
        //            return DiversityWorkbench.PostgresSettings.Default.Database;
        //        else return "postgres";
        //    }
        //    set
        //    {
        //        DiversityWorkbench.PostgresSettings.Default.Database = value;
        //        DiversityWorkbench.PostgresSettings.Default.Save();
        //    }
        //}

        //public static string Server
        //{
        //    get { return DiversityWorkbench.PostgresSettings.Default.Server; }
        //    set
        //    {
        //        DiversityWorkbench.PostgresSettings.Default.Server = value;
        //        DiversityWorkbench.PostgresSettings.Default.Save();
        //    }
        //}

        //public static string UserName
        //{
        //    get { return DiversityWorkbench.PostgresSettings.Default.User; }
        //    set
        //    {
        //        DiversityWorkbench.PostgresSettings.Default.User = value;
        //        DiversityWorkbench.PostgresSettings.Default.Save();
        //    }
        //}

        //public static int Port
        //{
        //    get { return DiversityWorkbench.PostgresSettings.Default.Port; }
        //    set
        //    {
        //        DiversityWorkbench.PostgresSettings.Default.Port = value;
        //        DiversityWorkbench.PostgresSettings.Default.Save();
        //    }
        //}
        
        #endregion

    }
}
