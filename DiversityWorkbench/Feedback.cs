//#define DEBUG

using DiversityWorkbench.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Windows;

namespace DiversityWorkbench
{
    public class Feedback
    {

        public static void SendFeedback(string Module, string Version)
        {
            // #34

            // Check if a connection to the central Feedback database is possible
            //bool ConnectionPossible = false;
            //string SQL = "SELECT COUNT(*) FROM DiversityWorkbenchFeedback.dbo.Feedback";
            //Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            //Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            try
            {
                SendMail(Version, "", "");

                //con.Open();
                //C.ExecuteNonQuery();
                //con.Close();
                //ConnectionPossible = true;
            }
            catch
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(new Exception("Sending e-mail failed"));
                //// no direct connection - try via IP
                //try
                //{
                //    if (con.State == System.Data.ConnectionState.Open) con.Close();
                //    con = new Microsoft.Data.SqlClient.SqlConnection(ConnectionStringFeedbackReading);
                //    C.Connection = con;
                //    con.Open();
                //    C.CommandText = "SELECT COUNT(*) FROM DiversityWorkbenchFeedback.dbo.Feedback";
                //    C.ExecuteNonQuery();
                //    con.Close();
                //    ConnectionPossible = true;
                //}
                //catch
                //{
                //    // no connection
                //    ConnectionPossible = false;
                //}
            }
            //if (!ConnectionPossible)
            //{
            //    System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo("mailto:weiss@bsm.mwn.de?subject=Feedback%20" + Module + "%20" + Version);
            //    info.UseShellExecute = true;
            //    System.Diagnostics.Process.Start(info);
            //}
            //else
            //{
            //    DiversityWorkbench.Forms.FormFeedback f = new DiversityWorkbench.Forms.FormFeedback(Module, Version, con);
            //    f.ShowDialog();
            //}
        }

        /// <summary>
        /// Old version for sendding and editing feedbach
        /// </summary>
        /// <param name="Module">The name of the module</param>
        /// <param name="Version">The version of the client software</param>
        /// <param name="Query">Optional: The query to retrieve the current data</param>
        /// <param name="ID">Optional: The ID of the current dataset</param>
        public static void SendFeedback(string Module, string Version, string Query, string ID)
        {
            try
            {
                // #34

                SendMail(Version, Query, ID);
                //DiversityWorkbench.Forms.FormFeedback f = new DiversityWorkbench.Forms.FormFeedback(Module, Version, Query, ID);
                //f.ShowDialog();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public static void FeedbackHistory(string Module)
        {
            // #34
            return;

            // Check if a connection to the central Feedback database is possible
            bool ConnectionPossible = false;
            string SQL = "SELECT COUNT(*) FROM DiversityWorkbenchFeedback.dbo.Feedback";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            try
            {
                con.Open();
                C.ExecuteNonQuery();
                con.Close();
                ConnectionPossible = true;
            }
            catch
            {
                // no direct connection - try via IP
                try
                {
                    if (con.State == System.Data.ConnectionState.Open) con.Close();
                    con = new Microsoft.Data.SqlClient.SqlConnection(ConnectionStringFeedbackReading);
                    C.Connection = con;
                    con.Open();
                    C.CommandText = "SELECT COUNT(*) FROM DiversityWorkbenchFeedback.dbo.Feedback";
                    C.ExecuteNonQuery();
                    con.Close();
                    ConnectionPossible = true;
                }
                catch
                {
                    // no connection
                    ConnectionPossible = false;
                }
            }
            if (!ConnectionPossible)
            {
                System.Windows.Forms.MessageBox.Show("No connection to the feedback database possible");
            }
            else
            {
                DiversityWorkbench.Forms.FormFeedback f = new DiversityWorkbench.Forms.FormFeedback(Module);
                f.ShowDialog();
            }
        }

        /// <summary>
        /// Editing the feedback
        /// </summary>
        public static void EditFeedback()
        {
            // #34
            return;

            try
            {
                // Check if a connection to the central Feedback database is possible
                if (EditFeedbackPossible())
                {
                    DiversityWorkbench.Forms.FormFeedback f = new DiversityWorkbench.Forms.FormFeedback("", "", "", DiversityWorkbench.Forms.FormFeedback.FormState.Editing);
                    f.ShowDialog();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("No connection to the feedback database possible");
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// Editing the feedback possible
        /// </summary>
        /// <returns>true if editing is possible</returns>
        public static bool EditFeedbackPossible()
        {
            // #34
            return false;

            // Check if a connection to the central Feedback database is possible
            bool ConnectionPossible = false;
            string SQL = "SELECT COUNT(*) FROM DiversityWorkbenchFeedback.dbo.Feedback";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(ConnectionStringFeedbackEditing);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            try
            {
                con.Open();
                C.ExecuteNonQuery();
                con.Close();
                ConnectionPossible = true;
            }
            catch (System.Exception) { }
            return ConnectionPossible;
        }

        /// <summary>
        /// Sending feedback - new version restricted to sending feedback
        /// </summary>
        /// <param name="Version">Version of the software. Retrieval: System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString()</param>
        /// <param name="Query">The Query used to search for the current data if available</param>
        /// <param name="ID">The ID of the current dataset if available</param>
        public static void SendFeedback(string Version, string Query, string ID)
        {
            try
            {
                // #34

                SendMail(Version, Query, ID);
                //DiversityWorkbench.Forms.FormFeedback f = new DiversityWorkbench.Forms.FormFeedback(Version, Query, ID, DiversityWorkbench.Forms.FormFeedback.FormState.Reporting);
                //f.ShowDialog();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        private static string _SupportMailAccount = "dwb-support@snsb.de";
        /// <summary>
        /// Sending feedback via mail
        /// https://github.com/SNSB/DiversityCollection/issues/34
        /// </summary>
        /// <param name="Version">The version of the client</param>
        /// <param name="Query">Any query parameters if available</param>
        /// <param name="ID">The ID if available</param>
        public static void SendMail(string Version, string Query, string ID)
        {
            // #252
            if (!IsMailClientConfigured())
            {
                MessageBox.Show("No mail client configured. Please send an email with your feedback.", "No mail client", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // #34
            System.Globalization.CultureInfo cultureInfo = System.Globalization.CultureInfo.InstalledUICulture;
            string subject = " ";
            string sDatabase = "";
            string sUser = "";
            string sMessage = "";
            string sQuery = "";

            switch (cultureInfo.Name)
            {
                case "de_DE":
                    subject = "Rückmeldung ";
                    sDatabase = "Datenbank";
                    sUser = "Benutzer";
                    sMessage = "Bitte hier das Anliegen und Bilder einfügen:";
                    sQuery = "Suche";
                    break;
                default:
                    subject = "Feedback ";
                    sDatabase = "Database";
                    sUser = "User";
                    sMessage = "Please enter your subject including screenshots here:";
                    sQuery = "Query";
                    break;
            }

            if (DiversityWorkbench.Settings.ModuleName != null && DiversityWorkbench.Settings.ModuleName.Length > 0)
            {
                subject += DiversityWorkbench.Settings.ModuleName;
            }

            string body = sMessage + "\r\n\r\n\r\n";
            if (Version != null && Version.Length > 0)
            {
                body += "Version: " + Version + "\r\n";
            }
            if (DiversityWorkbench.Settings.DatabaseName != null && DiversityWorkbench.Settings.DatabaseName.Length > 0)
            {
                body += "\r\n" + sDatabase + ": " + DiversityWorkbench.Settings.DatabaseName + "\r\n";
                if (DiversityWorkbench.Settings.DatabaseUser != null && DiversityWorkbench.Settings.DatabaseUser.Length > 0 
                    && DiversityWorkbench.Settings.Password != null && DiversityWorkbench.Settings.Password.Length > 0)
                    body += sUser + ": " + DiversityWorkbench.Settings.DatabaseUser + "\r\n";
                else if (DiversityWorkbench.Settings.CurrentUserName().Length > 0)
                    body += sUser + ": " + DiversityWorkbench.Settings.CurrentUserName() + "\r\n";
            }
            if (Query != null && Query != "")
            {
                body += "\r\n" + sQuery + ":\r\n" + Query + "\r\n";
            }
            if (ID != null && ID != "")
            {
                body += "\r\nID: " + ID + "\r\n";
            }
            if(ErrorLog().Length > 0)
            {
                body += "\r\nError log:\r\n" + ErrorLog() + "\r\n";
            }
            subject = Uri.EscapeDataString(subject);
            body = Uri.EscapeDataString(body);
            // #252
            try
            {
                System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo("mailto:" + _SupportMailAccount + "?subject=" + subject + "&body=" + body);
                info.UseShellExecute = true;
                System.Diagnostics.Process.Start(info);
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        /// <summary>
        /// Sending feedback via mail
        /// https://github.com/SNSB/DiversityCollection/issues/34
        /// </summary>
        /// <param name="Subject">Subject of the email</param>
        /// <param name="Body">Body of the email</param>
        public static void SendMail(string Subject, string Body)
        {
            string subject = Uri.EscapeDataString(Subject);
            string body = Uri.EscapeDataString(Body);
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo("mailto:" + _SupportMailAccount + "?subject=" + subject +"&body=" + body);
            info.UseShellExecute = true;
            System.Diagnostics.Process.Start(info);
        }

        //public static void SendFeedback(System.Windows.Forms.Form fParent, string Version, string Query, string ID)
        //{
        //    try
        //    {
        //        DiversityWorkbench.Forms.FormFeedback f = new DiversityWorkbench.Forms.FormFeedback(Version, Query, ID, DiversityWorkbench.Forms.FormFeedback.FormState.Reporting);
        //        f.ShowDialog();
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        public static void FeedbackHistory()
        {
            //#34
            return;

            try
            {
                DiversityWorkbench.Forms.FormFeedback f = new DiversityWorkbench.Forms.FormFeedback("", "", "", DiversityWorkbench.Forms.FormFeedback.FormState.History);
                f.ShowDialog();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        public static readonly string ConnectionStringFeedbackReading = global::DiversityWorkbench.Properties.Settings.Default.DiversityWorkbenchFeedbackConnectionString;
        public static readonly string ConnectionStringFeedbackEditing = global::DiversityWorkbench.Properties.Settings.Default.DiversityWorkbenchFeedbackWriteConnectionString;

        private static System.IO.FileInfo _ErrorLog;
        private static string ErrorLog(string Module = "", int MaxLines = 150)
        {
            string Log = "";
            try
            {
                if (_ErrorLog == null)
                {
                    if (Module.Length == 0)
                        Module = DiversityWorkbench.Settings.ModuleName;
                    _ErrorLog = new System.IO.FileInfo(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.ErrorLogFile(Module));
                }
                if (_ErrorLog != null && _ErrorLog.Exists)
                {
                    int Lines = 0;
                    System.IO.StreamReader sr = new System.IO.StreamReader(_ErrorLog.FullName); ;
                    using (sr)
                    {
                        string line = "";
                        while ((line = sr.ReadLine()) != null && Lines < MaxLines)
                        {
                            if (line.StartsWith("Date:"))
                            {
                                string DateInLine = line.Substring(6).Trim();
                                System.DateTime D;
                                if (System.DateTime.TryParse(DateInLine, out D))
                                {
                                    if (D.ToShortDateString() == System.DateTime.Now.ToShortDateString())
                                    {
                                        Log += line + "\r\n";
                                        Lines++;
                                        break;
                                    }
                                }
                            }
                        }
                        while ((line = sr.ReadLine()) != null && Lines < MaxLines)
                        {
                            Log += line + "\r\n";
                            Lines++;
                        }
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return Log;
        }

        /// <summary>
        /// Check if a mail client is configured #252
        /// </summary>
        /// <returns>If client is configured</returns>
        public static bool IsMailClientConfigured()
        {
            try
            {
                var psi = new ProcessStartInfo("mailto:test@example.com")
                {
                    UseShellExecute = true
                };
                Process.Start(psi);
                return true;
            }
            catch (Exception ex)
            {
                // You can log ex.Message for diagnostics
                return false;
            }
        }
    }
}
