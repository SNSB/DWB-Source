using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Reflection;

namespace DiversityWorkbench
{
    public class Functions
    {

        #region Database


        public static string ColumnDescription(string Table, string Column)
        {
            string Description = "";
            if (DiversityWorkbench.Settings.ConnectionString.Length == 0) return "";
            if (Table.IndexOf("_Main20") > 0)
                Table = Table.Substring(0, Table.IndexOf("_Main20"));
            if (Table.IndexOf("_Core") > 0)
                Table = Table.Substring(0, Table.IndexOf("_Core"));
            if (Table.IndexOf("_IsAccepted") > 0)
                Table = Table.Substring(0, Table.IndexOf("_IsAccepted"));
            Description = DiversityWorkbench.Forms.FormFunctions.getDescriptionCache(Table, Column);
            if (Description == null || Description.Length == 0)
            {
                Microsoft.Data.SqlClient.SqlConnection conn = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                string SQL = "SELECT max(CONVERT(nvarchar(MAX), [value])) " +
                    " FROM ::fn_listextendedproperty(NULL, 'user', 'dbo', 'table', '" + Table +
                    "', 'column', '" + Column + "') WHERE name =  'MS_Description'--[::fn_listextendedproperty_1]";
                Microsoft.Data.SqlClient.SqlCommand Com = new Microsoft.Data.SqlClient.SqlCommand(SQL, conn);
                try
                {
                    conn.Open();
                    Description = Com.ExecuteScalar().ToString();
                }
                catch
                {
                }
                finally
                {
                    conn.Close();
                }
            }
            return Description;
        }

        public static string TableDescription(string Table)
        {
            string Description = "";
            if (DiversityWorkbench.Settings.ConnectionString.Length == 0) return "";
            if (Table.IndexOf("_Main20") > 0)
                Table = Table.Substring(0, Table.IndexOf("_Main20"));
            if (Table.IndexOf("_Core") > 0)
                Table = Table.Substring(0, Table.IndexOf("_Core"));
            if (Table.IndexOf("_IsAccepted") > 0)
                Table = Table.Substring(0, Table.IndexOf("_IsAccepted"));
            if (Description == null || Description.Length == 0)
            {
                Microsoft.Data.SqlClient.SqlConnection conn = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                string SQL = "SELECT max(CONVERT(nvarchar(MAX), [value])) " +
                    " FROM ::fn_listextendedproperty(NULL, 'user', 'dbo', 'table', '" + Table +
                    "', default, NULL) WHERE name =  'MS_Description'--[::fn_listextendedproperty_1]";
                Microsoft.Data.SqlClient.SqlCommand Com = new Microsoft.Data.SqlClient.SqlCommand(SQL, conn);
                try
                {
                    conn.Open();
                    Description = Com.ExecuteScalar().ToString();
                }
                catch
                {
                }
                finally
                {
                    conn.Close();
                }
            }
            return Description;
        }

        #endregion

        public static string InstalledFramework()
        {
            string Version = "";
            RegistryKey rk = Registry.LocalMachine;
            try
            {
                RegistryKey rkSoftware = rk.OpenSubKey("Software");
                RegistryKey rkMicrosoft = rkSoftware.OpenSubKey("Microsoft");
                RegistryKey rkFramework = rkMicrosoft.OpenSubKey(".NetFramework");
                string[] Subkeys = rkFramework.GetSubKeyNames();
                string sPattern = "v\\d{1}[.]\\d{1}.";
                foreach (string s in Subkeys)
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(s, sPattern))
                        Version = s.Substring(1, 3);
                }
            }
            catch { }
            return Version;
        }

        public static MethodInfo GetEventInvoker(object obj, string eventName)
        {
            // --- Begin parameters checking code -----------------------------
            //Debug.Assert(obj != null);
            //Debug.Assert(!string.IsNullOrEmpty(eventName));
            // --- End parameters checking code -------------------------------

            // prepare current processing type
            Type currentType = obj.GetType();

            // try to get special event declaration
            while (true)
            {
                FieldInfo fieldInfo = currentType.GetField(eventName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.GetField);

                if (fieldInfo == null)
                {
                    if (currentType.BaseType != null)
                    {
                        // move deeper
                        currentType = currentType.BaseType;
                        continue;
                    }

                    //Debug.Fail(string.Format("Not found event named {0} in object type {1}", eventName, obj));
                    return null;
                }

                // found
                return ((MulticastDelegate)fieldInfo.GetValue(obj)).Method;
            }
        }


    }


}
