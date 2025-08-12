using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench
{
    public class UserSettings
    {
        public enum SettingGroups { DiversityMobile, ModuleSource };

        /// <summary>
        /// Setting a value
        /// </summary>
        /// <param name="Setting">The node list underneath the root node Settings</param>
        /// <param name="SettingValue">The value of the setting node</param>
        public void SaveSetting(System.Collections.Generic.List<string> Setting, string SettingValue, string Attribute)
        {
            try
            {
                // space is not allowed
                this.AdjustSettings(ref Setting);

                // check if settings contain anything
                string SQL = "SELECT CASE WHEN u.Settings IS NULL THEN 0 ELSE 1 END AS HasSettings " +
                    "FROM UserProxy AS u " +
                    "WHERE LoginName = USER_NAME()";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                string Result = C.ExecuteScalar()?.ToString();
                if (Result == "0")
                {
                    C.CommandText = "UPDATE    U " +
                        "SET              Settings = '<Settings> <" + Setting[0] + "> </" + Setting[0] + ">  </Settings>' " +
                        "FROM         UserProxy AS U " +
                        "WHERE     (LoginName = USER_NAME())";
                    C.ExecuteNonQuery();
                }

                // check if settings have basic nodes 
                C.CommandText = "SELECT     Settings.exist('/Settings') AS Node " +
                    "FROM         UserProxy AS U " +
                    "WHERE     (LoginName = USER_NAME())";
                Result = C.ExecuteScalar()?.ToString();
                if (Result != "True")
                {
                    C.CommandText = "UPDATE    U " +
                        "SET              Settings = '<Settings> <" + Setting[0] + "> </" + Setting[0] + ">  </Settings>' " +
                        "FROM         UserProxy AS U " +
                        "WHERE     (LoginName = USER_NAME())";
                    C.ExecuteNonQuery();
                }

                // check if node exists
                string SettingNode = "";
                for (int iSN = 0; iSN < Setting.Count ; iSN++)
                {
                    SettingNode += "/" + Setting[iSN];
                    C.CommandText = "SELECT Settings.exist('/Settings" + SettingNode +
                        "') AS Node FROM UserProxy AS U " +
                        "WHERE (LoginName = USER_NAME())";
                    Result = C.ExecuteScalar()?.ToString();
                    if (Result != "True")
                    {
                        // inserting the superior nodes if missing
                        for (int i = iSN; i < Setting.Count - 1; i++)
                        {
                            C.CommandText = "DECLARE @Setting xml; " +
                                "SET @Setting = (SELECT Settings " +
                                "FROM UserProxy AS U " +
                                "WHERE LoginName = USER_NAME());" +
                                "set @Setting.modify('" +
                                "insert <" + Setting[i] + "></" + Setting[i] + ">   " +
                                "as first       " +
                                "into (/Settings";
                            for (int ii = 0; ii < iSN; ii++)
                            {
                                C.CommandText += "/" + Setting[ii];
                            }
                            C.CommandText += ")[1]'); update U set u.Settings = @Setting " +
                                "FROM [dbo].[UserProxy] U " +
                                "WHERE LoginName = USER_NAME();";
                            C.ExecuteNonQuery();
                            break;
                        }

                        // Last node containing the value
                        // Only if attribute is missing is SettingValue inculded
                        C.CommandText = "DECLARE @Setting xml; " +
                            "SET @Setting = (SELECT Settings " +
                            "FROM UserProxy AS U " +
                            "WHERE LoginName = USER_NAME());" +
                            "set @Setting.modify('" +
                            "insert <" + Setting.Last() + ">";
                        if (Attribute.Length == 0)
                            C.CommandText += SettingValue;
                        C.CommandText += "</" + Setting.Last() + ">   " +
                            "as first       " +
                            "into (/Settings";
                        for (int i = 0; i < Setting.Count - 1; i++)
                        {
                            C.CommandText += "/" + Setting[i];
                        }
                        C.CommandText += ")[1]'); update U set u.Settings = @Setting " +
                            "FROM [dbo].[UserProxy] U " +
                            "WHERE LoginName = USER_NAME();";
                        C.ExecuteNonQuery();
                    }
                    else if (iSN == Setting.Count - 1)
                    {
                        if (Attribute.Length > 0)
                            C.CommandText = "EXEC dbo.SetXmlAttribute 'UserProxy', 'Settings', '/Settings" + SettingNode + "', '" + SettingValue + "', '" + Attribute + "', 'LoginName = USER_NAME()'";
                        else
                        {
                            if (SettingValue.Length > 0)
                                C.CommandText = "EXEC dbo.SetXmlValue 'UserProxy', 'Settings', '/Settings" + SettingNode + "', '" + SettingValue + "', 'LoginName = USER_NAME()'";
                            else
                                C.CommandText = "EXEC dbo.DeleteXmlNode 'UserProxy', 'Settings', '/Settings" + SettingNode + "', 'LoginName = USER_NAME()'";
                        }
                        C.ExecuteNonQuery();
                    }
                }
                con.Close();
                con.Dispose();
                C.Dispose();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

        }

        /// <summary>
        /// Setting a value
        /// </summary>
        /// <param name="Setting">The node list underneath the root node Settings</param>
        /// <param name="SettingValue">The value of the setting node</param>
        public void DeleteSettingAttribute(System.Collections.Generic.List<string> Setting, string Attribute)
        {
            try
            {
                // space is not allowed
                this.AdjustSettings(ref Setting);

                // check if settings contain anything
                string SQL = "SELECT CASE WHEN u.Settings IS NULL THEN 0 ELSE 1 END AS HasSettings " +
                    "FROM UserProxy AS u " +
                    "WHERE LoginName = USER_NAME()";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                string Result = C.ExecuteScalar()?.ToString();
                if (Result == "0")
                {
                    C.CommandText = "UPDATE    U " +
                        "SET              Settings = '<Settings> <" + Setting[0] + "> </" + Setting[0] + ">  </Settings>' " +
                        "FROM         UserProxy AS U " +
                        "WHERE     (LoginName = USER_NAME())";
                    C.ExecuteNonQuery();
                }

                // check if settings have basic nodes 
                C.CommandText = "SELECT     Settings.exist('/Settings') AS Node " +
                    "FROM         UserProxy AS U " +
                    "WHERE     (LoginName = USER_NAME())";
                Result = C.ExecuteScalar()?.ToString();
                if (Result != "True")
                {
                    C.CommandText = "UPDATE    U " +
                        "SET              Settings = '<Settings> <" + Setting[0] + "> </" + Setting[0] + ">  </Settings>' " +
                        "FROM         UserProxy AS U " +
                        "WHERE     (LoginName = USER_NAME())";
                    C.ExecuteNonQuery();
                }

                // check if node exists
                string SettingNode = "";
                for (int iSN = 0; iSN < Setting.Count; iSN++)
                {
                    SettingNode += "/" + Setting[iSN];
                    C.CommandText = "SELECT Settings.exist('/Settings" + SettingNode +
                        "') AS Node FROM UserProxy AS U " +
                        "WHERE (LoginName = USER_NAME())";
                    Result = C.ExecuteScalar()?.ToString();
                    if (Result != "True")
                    {
                        // inserting the superior nodes if missing
                        for (int i = iSN; i < Setting.Count - 1; i++)
                        {
                            C.CommandText = "DECLARE @Setting xml; " +
                                "SET @Setting = (SELECT Settings " +
                                "FROM UserProxy AS U " +
                                "WHERE LoginName = USER_NAME());" +
                                "set @Setting.modify('" +
                                "insert <" + Setting[i] + "></" + Setting[i] + ">   " +
                                "as first       " +
                                "into (/Settings";
                            for (int ii = 0; ii < iSN; ii++)
                            {
                                C.CommandText += "/" + Setting[ii];
                            }
                            C.CommandText += ")[1]'); update U set u.Settings = @Setting " +
                                "FROM [dbo].[UserProxy] U " +
                                "WHERE LoginName = USER_NAME();";
                            C.ExecuteNonQuery();
                            break;
                        }

                        // Last node containing the value
                        // Only if attribute is missing is SettingValue inculded
                        C.CommandText = "DECLARE @Setting xml; " +
                            "SET @Setting = (SELECT Settings " +
                            "FROM UserProxy AS U " +
                            "WHERE LoginName = USER_NAME());" +
                            "set @Setting.modify('" +
                            "insert <" + Setting.Last() + ">";
                        C.CommandText += "</" + Setting.Last() + ">   " +
                            "as first       " +
                            "into (/Settings";
                        for (int i = 0; i < Setting.Count - 1; i++)
                        {
                            C.CommandText += "/" + Setting[i];
                        }
                        C.CommandText += ")[1]'); update U set u.Settings = @Setting " +
                            "FROM [dbo].[UserProxy] U " +
                            "WHERE LoginName = USER_NAME();";
                        C.ExecuteNonQuery();
                    }
                    else if (iSN == Setting.Count - 1)
                    {
                            C.CommandText = "EXEC dbo.DeleteXmlAttribute 'UserProxy', 'Settings', '/Settings" + SettingNode + "', '" + Attribute + "', 'LoginName = USER_NAME()'";
                        C.ExecuteNonQuery();
                    }
                }
                con.Close();
                con.Dispose();
                C.Dispose();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

        }

        /// <summary>
        /// Setting a value
        /// </summary>
        /// <param name="Setting">The node list underneath the root node Settings</param>
        public void DeleteSettingNode(System.Collections.Generic.List<string> Setting)
        {
            try
            {
                // space is not allowed
                this.AdjustSettings(ref Setting);

                // check if settings contain anything
                string SQL = "SELECT CASE WHEN u.Settings IS NULL THEN 0 ELSE 1 END AS HasSettings " +
                    "FROM UserProxy AS u " +
                    "WHERE LoginName = USER_NAME()";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                string Result = C.ExecuteScalar()?.ToString();
                if (Result == "0")
                {
                    C.CommandText = "UPDATE    U " +
                        "SET              Settings = '<Settings> <" + Setting[0] + "> </" + Setting[0] + ">  </Settings>' " +
                        "FROM         UserProxy AS U " +
                        "WHERE     (LoginName = USER_NAME())";
                    C.ExecuteNonQuery();
                }

                // check if settings have basic nodes 
                C.CommandText = "SELECT     Settings.exist('/Settings') AS Node " +
                    "FROM         UserProxy AS U " +
                    "WHERE     (LoginName = USER_NAME())";
                Result = C.ExecuteScalar()?.ToString();
                if (Result != "True")
                {
                    C.CommandText = "UPDATE    U " +
                        "SET              Settings = '<Settings> <" + Setting[0] + "> </" + Setting[0] + ">  </Settings>' " +
                        "FROM         UserProxy AS U " +
                        "WHERE     (LoginName = USER_NAME())";
                    C.ExecuteNonQuery();
                }

                // check if node exists
                string SettingNode = "";
                for (int iSN = 0; iSN < Setting.Count; iSN++)
                {
                    SettingNode += "/" + Setting[iSN];
                    C.CommandText = "SELECT Settings.exist('/Settings" + SettingNode +
                        "') AS Node FROM UserProxy AS U " +
                        "WHERE (LoginName = USER_NAME())";
                    Result = C.ExecuteScalar()?.ToString();
                    if (Result != "True")
                    {
                        // inserting the superior nodes if missing
                        for (int i = iSN; i < Setting.Count - 1; i++)
                        {
                            C.CommandText = "DECLARE @Setting xml; " +
                                "SET @Setting = (SELECT Settings " +
                                "FROM UserProxy AS U " +
                                "WHERE LoginName = USER_NAME());" +
                                "set @Setting.modify('" +
                                "insert <" + Setting[i] + "></" + Setting[i] + ">   " +
                                "as first       " +
                                "into (/Settings";
                            for (int ii = 0; ii < iSN; ii++)
                            {
                                C.CommandText += "/" + Setting[ii];
                            }
                            C.CommandText += ")[1]'); update U set u.Settings = @Setting " +
                                "FROM [dbo].[UserProxy] U " +
                                "WHERE LoginName = USER_NAME();";
                            C.ExecuteNonQuery();
                            break;
                        }

                        // Last node containing the value
                        // Only if attribute is missing is SettingValue inculded
                        C.CommandText = "DECLARE @Setting xml; " +
                            "SET @Setting = (SELECT Settings " +
                            "FROM UserProxy AS U " +
                            "WHERE LoginName = USER_NAME());" +
                            "set @Setting.modify('" +
                            "insert <" + Setting.Last() + ">";
                        C.CommandText += "</" + Setting.Last() + ">   " +
                            "as first       " +
                            "into (/Settings";
                        for (int i = 0; i < Setting.Count - 1; i++)
                        {
                            C.CommandText += "/" + Setting[i];
                        }
                        C.CommandText += ")[1]'); update U set u.Settings = @Setting " +
                            "FROM [dbo].[UserProxy] U " +
                            "WHERE LoginName = USER_NAME();";
                        C.ExecuteNonQuery();
                    }
                    else if (iSN == Setting.Count - 1)
                    {
                        //C.CommandText = "EXEC dbo.DeleteXmlAttribute 'UserProxy', 'Settings', '/Settings" + SettingNode + "', '" + Attribute + "', 'LoginName = USER_NAME()'";
                        C.ExecuteNonQuery();
                    }
                }
                con.Close();
                con.Dispose();
                C.Dispose();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

        }

        /// <summary>
        /// Getting a value from the settings
        /// </summary>
        /// <param name="Settings">The node list underneath the root node Settings</param>
        /// <returns>The value of the settings (trimmed as empty values must be replaced by ' ' in order to enable later changes)</returns>
        public string GetSetting(System.Collections.Generic.List<string> Settings)
        {
            // space is not allowed
            this.AdjustSettings(ref Settings);

            string Setting = "";
            string SQL = "DECLARE @Setting xml; " +
                "DECLARE @Result nvarchar(max); " +
                "SET @Setting = (SELECT Settings FROM UserProxy AS U WHERE LoginName = USER_NAME()); " +
                "SET @Result =  @Setting.value('(/Settings";
            foreach (string S in Settings)
                SQL += "/" + S;
            SQL += ")[1]', 'nvarchar(max)' ) " +
                "SELECT @Result";
            string s = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true);
            if (s != null)
                Setting = s;
            return Setting.Trim();
        }

        public string GetSetting(System.Collections.Generic.List<string> Settings, string Attribute)
        {
            // space is not allowed
            this.AdjustSettings(ref Settings);

            string Setting = "";
            string SQL = "SET ANSI_NULLS ON; DECLARE @Setting xml; " +
                "DECLARE @Result nvarchar(max); " +
                "SET @Setting = (SELECT Settings FROM UserProxy AS U WHERE LoginName = USER_NAME()); " +
                "SET @Result =  @Setting.value('(/Settings";
            foreach (string S in Settings)
                SQL += "/" + S;
            SQL += "/@" + Attribute + ")[1]', 'nvarchar(max)' ) " +
                "SELECT @Result";
            Setting = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true);
            return Setting.Trim();
        }

        /// <summary>
        /// Remove space from every part with the setting list and replace it by a predefined value
        /// </summary>
        /// <param name="Settings">The list of strings that should be checked for space characters</param>
        /// <param name="Replacement">Character set for replacement, default = _</param>
        private void AdjustSettings(ref System.Collections.Generic.List<string> Settings, char Replacement = '_')
        {
            for (int i = 0; i < Settings.Count; i++)
            {
                if (Settings[i].IndexOf(' ') > -1)
                    Settings[i] = Settings[i].Replace(' ', Replacement);
            }
        }
        public System.Collections.Generic.Dictionary<string, string> GetSettingOptions(System.Collections.Generic.List<string> Settings, DiversityWorkbench.WorkbenchUnit WorkbenchUnit)
        {
            System.Collections.Generic.Dictionary<string, string> DD = new Dictionary<string, string>();
            //string Setting = "";
            //if (WorkbenchUnit != null && WorkbenchUnit.Q ServerConnection.WebserviceOptions().Count > 0)
            //{
            //    foreach (DiversityWorkbench.WebserviceQueryOption O in Webservice.WebserviceOptions())
            //    {
            //        string SQL = "SET ANSI_NULLS ON; DECLARE @Setting xml; " +
            //            "DECLARE @Result nvarchar(max); " +
            //            "SET @Setting = (SELECT Settings FROM UserProxy AS U WHERE LoginName = USER_NAME()); " +
            //            "SET @Result =  @Setting.value('(/Settings";
            //        foreach (string S in Settings)
            //            SQL += "/" + S;
            //        SQL += "/@" + O.Name() + ")[1]', 'nvarchar(max)' ) " +
            //            "SELECT @Result";
            //        Setting = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            //        DD.Add(O.Name(), Setting);
            //    }
            //}
            return DD;
        }

        /// <summary>
        /// Setting a attribute value
        /// </summary>
        /// <param name="Setting">The node list underneath the root node Settings</param>
        /// <param name="Attribute">The attribute of the setting node</param>
        /// <param name="AttributeValue">The value of the attribute</param>
        //public void SaveSetting(System.Collections.Generic.List<string> Setting, string Attribute, string AttributeValue)
        //{
        //    try
        //    {
        //        // check if settings contain anything
        //        string SQL = "SELECT CASE WHEN u.Settings IS NULL THEN 0 ELSE 1 END AS HasSettings " +
        //            "FROM UserProxy AS u " +
        //            "WHERE LoginName = USER_NAME()";
        //        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
        //        Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
        //        con.Open();
        //        string Result = C.ExecuteScalar()?.ToString();
        //        if (Result == "0")
        //        {
        //            C.CommandText = "UPDATE    U " +
        //                "SET              Settings = '<Settings> <" + Setting[0] + "> </" + Setting[0] + ">  </Settings>' " +
        //                "FROM         UserProxy AS U " +
        //                "WHERE     (LoginName = USER_NAME())";
        //            C.ExecuteNonQuery();
        //        }

        //        // check if settings have basic nodes 
        //        C.CommandText = "SELECT     Settings.exist('/Settings') AS Node " +
        //            "FROM         UserProxy AS U " +
        //            "WHERE     (LoginName = USER_NAME())";
        //        Result = C.ExecuteScalar()?.ToString();
        //        if (Result != "True")
        //        {
        //            C.CommandText = "UPDATE    U " +
        //                "SET              Settings = '<Settings> <" + Setting[0] + "> </" + Setting[0] + ">  </Settings>' " +
        //                "FROM         UserProxy AS U " +
        //                "WHERE     (LoginName = USER_NAME())";
        //        }

        //        // check if node exists
        //        string SettingNode = "";
        //        for (int iSN = 0; iSN < Setting.Count; iSN++)
        //        {
        //            SettingNode += "/" + Setting[iSN];
        //            C.CommandText = "SELECT Settings.exist('/Settings" + SettingNode +
        //                "') AS Node FROM UserProxy AS U " +
        //                "WHERE (LoginName = USER_NAME())";
        //            Result = C.ExecuteScalar()?.ToString();
        //            if (Result != "True")
        //            {
        //                // inserting the superior nodes if missing
        //                for (int i = iSN; i < Setting.Count - 1; i++)
        //                {
        //                    C.CommandText = "DECLARE @Setting xml; " +
        //                        "SET @Setting = (SELECT Settings " +
        //                        "FROM UserProxy AS U " +
        //                        "WHERE LoginName = USER_NAME());" +
        //                        "set @Setting.modify('" +
        //                        "insert <" + Setting[i] + "></" + Setting[i] + ">   " +
        //                        "as first       " +
        //                        "into (/Settings";
        //                    for (int ii = 0; ii < iSN; ii++)
        //                    {
        //                        C.CommandText += "/" + Setting[ii];
        //                    }
        //                    C.CommandText += ")[1]'); update U set u.Settings = @Setting " +
        //                        "FROM [dbo].[UserProxy] U " +
        //                        "WHERE LoginName = USER_NAME();";
        //                    C.ExecuteNonQuery();
        //                }

        //                // Last node containing the attribute and value
        //                C.CommandText = "DECLARE @Setting xml; " +
        //                    "SET @Setting = (SELECT Settings " +
        //                    "FROM UserProxy AS U " +
        //                    "WHERE LoginName = USER_NAME());" +
        //                    "set @Setting.modify('" +
        //                    "insert <" + Setting.Last() + "[@" + Attribute + "=" + AttributeValue + "]></" + Setting.Last() + ">   " +
        //                    "as first       " +
        //                    "into (/Settings";
        //                for (int i = 0; i < Setting.Count - 1; i++)
        //                {
        //                    C.CommandText += "/" + Setting[i];
        //                }
        //                C.CommandText += ")[1]'); update U set u.Settings = @Setting " +
        //                    "FROM [dbo].[UserProxy] U " +
        //                    "WHERE LoginName = USER_NAME();";
        //                C.ExecuteNonQuery();
        //            }
        //            else if (iSN == Setting.Count - 1)
        //            {
        //                C.CommandText = "EXEC dbo.SetXmlAttributeValue 'UserProxy', 'Settings', '/Settings" + SettingNode + "', '" + Attribute + "', '" + AttributeValue + "', 'LoginName = USER_NAME()'";
        //                //C.CommandText = "EXEC dbo.SetXmlValue 'UserProxy', 'Settings', '/Settings" + SettingNode + "/@" + Attribute + "', '" + AttributeValue + "', 'LoginName = USER_NAME()'";
        //                C.ExecuteNonQuery();
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }

        //}

        /// <summary>
        /// Getting an attribute value from the settings
        /// </summary>
        /// <param name="Settings">The node list underneath the root node Settings</param>
        /// <param name="Attribute">The attribute of the setting node</param>
        /// <returns>The value of the settings attribute</returns>
        //public string GetSetting(System.Collections.Generic.List<string> Settings, string Attribute)
        //{
        //    string Setting = "";
        //    string SQL = "DECLARE @Setting xml; " +
        //        "DECLARE @Result nvarchar(max); " +
        //        "SET @Setting = (SELECT Settings FROM UserProxy AS U WHERE LoginName = USER_NAME()); " +
        //        "SET @Result =  @Setting.value('(/Settings";
        //    foreach (string S in Settings)
        //        SQL += "/" + S;
        //    SQL += "/@" + Attribute;
        //    SQL += ")[1]', 'nvarchar(max)' ) " +
        //        "SELECT @Result";
        //    Setting = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
        //    return Setting.Trim();
        //}

    }
}
