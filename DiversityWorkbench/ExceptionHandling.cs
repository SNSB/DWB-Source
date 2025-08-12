using Microsoft.SqlServer.Management.XEvent;
using System;
using System.IO;

namespace DiversityWorkbench
{
	/// <summary>
	/// Zusammenfassung für ExceptionHandling.
	/// </summary>
	public class ExceptionHandling
	{
        private static string _ErrorLogFilePath = "";

        public static string ErrorLogFilePath
        {
            get 
            {
                if (DiversityWorkbench.ExceptionHandling._ErrorLogFilePath.Length == 0)
                {
                    try
                    {
                        DiversityWorkbench.ExceptionHandling._ErrorLogFilePath = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.ErrorLogFile();
                        System.IO.FileInfo f = new System.IO.FileInfo(DiversityWorkbench.ExceptionHandling._ErrorLogFilePath);
                        if (!f.Exists)
                        {
                            System.IO.StreamWriter sw;
                            sw = new System.IO.StreamWriter(DiversityWorkbench.ExceptionHandling._ErrorLogFilePath, false);
                            sw.Flush();
                            sw.Write("");
                            sw.Close();
                            sw.Dispose();
                        }
                        //f.Create();
                        System.Security.AccessControl.FileSecurity fSecurity = new System.Security.AccessControl.FileSecurity();// f.GetAccessControl();
                        fSecurity.AddAccessRule(new System.Security.AccessControl.FileSystemAccessRule(System.Environment.UserName,
                                                                    System.Security.AccessControl.FileSystemRights.FullControl,
                                                                    System.Security.AccessControl.AccessControlType.Allow));
                        System.IO.FileSystemAclExtensions.SetAccessControl(f, fSecurity);
                        System.IO.FileAttributes attribute;
                        attribute = (System.IO.FileAttributes)(f.Attributes - System.IO.FileAttributes.ReadOnly);
                        System.IO.File.SetAttributes(DiversityWorkbench.ExceptionHandling._ErrorLogFilePath, attribute);
                    }
                    catch (System.Exception ex) { }
                }
                return ExceptionHandling._ErrorLogFilePath; 
            }
            //set { ExceptionHandling._ErrorLogFile = value; }
        }

        public static string ErrorLogFile
        {
            get
            {
                string LogFile = "";
                string Line = "";
                bool Current = false;
                try
                {
                    string LogPath = DiversityWorkbench.ExceptionHandling.ErrorLogFilePath;// ...Windows.Forms.Application.StartupPath + "\\" + System.Windows.Forms.Application.ProductName.ToString() + "Error.log";
                    System.IO.StreamReader sr;
                    if (System.IO.File.Exists(LogPath))
                    {
                        sr = new System.IO.StreamReader(LogPath);
                        while (!sr.EndOfStream)
                        {
                            Line = sr.ReadLine();
                            if (Line.StartsWith("Date:"))
                            {
                                string Date = Line.Substring(Line.IndexOf("\t")).Trim();
                                System.DateTime d;
                                if (System.DateTime.TryParse(Date, out d))
                                {
                                    if (d.Date.ToString() == System.DateTime.Now.Date.ToString())
                                    {
                                        Current = true;
                                    }
                                }
                            }
                            if (Current)
                                LogFile += sr.ReadLine();
                        }
                    }
                }
                catch { }
                return LogFile;
            }
        }

        public static void WriteToErrorLogFile(System.Exception Exception)
        {
            WriteToErrorLogFile(Exception, "");
        }

        public static void ShowErrorLog()
        {
            System.IO.FileInfo E = new System.IO.FileInfo(ErrorLogFilePath);
            if (E.Exists)
            {
                try
                {
                    System.IO.FileInfo fileObj = new System.IO.FileInfo(E.FullName);
                    fileObj.Attributes = System.IO.FileAttributes.ReadOnly;
                    System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(fileObj.FullName);
                    info.UseShellExecute = true;
                    System.Diagnostics.Process.Start(info);
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No ErrorLog present");
            }
        }

        /// <summary>
        /// Clear error log if present and start from new
        /// </summary>
        /// <param name="StartMessage">The first message</param>
        public static void InitErrorLogFile(string StartMessage)
        {
            try
            {
                EnsureAccess();

                string Logfile = DiversityWorkbench.ExceptionHandling.ErrorLogFilePath;
                System.IO.StreamWriter sw;
                if (System.IO.File.Exists(Logfile))
                    sw = new System.IO.StreamWriter(Logfile, false);
                else
                {
                    sw = new System.IO.StreamWriter(Logfile);
                }
                try
                {
                    sw.WriteLine(StartMessage);
                    sw.WriteLine();
                }
                catch
                {
                }
                finally
                {
                    sw.Close();
                }
            }
            catch(System.Exception ex) 
            { }
        }

        /// <summary>
        /// Reset Errorlog if ErrorLogResetAtStartup = true
        /// </summary>
        public static void InitErrorLogFile()
        {
            if (Settings.ErrorLogResetAtStartup)
            {
                try
                {
                    System.IO.FileInfo f = new System.IO.FileInfo(DiversityWorkbench.ExceptionHandling.ErrorLogFilePath);
                    if (f.Exists)
                    {
                        EnsureAccess();
                        System.IO.StreamWriter sw;
                        sw = new System.IO.StreamWriter(DiversityWorkbench.ExceptionHandling.ErrorLogFilePath, false);
                        sw.Flush();
                        sw.Write("");
                        sw.Close();
                        sw.Dispose();
                    }
                }
                catch (System.Exception ex)
                { }
            }
        }

        private static void EnsureAccess()
        {
            try
            {
                System.IO.FileAttributes attribute;
                System.IO.FileInfo f = new System.IO.FileInfo(DiversityWorkbench.ExceptionHandling._ErrorLogFilePath);
                attribute = (System.IO.FileAttributes)(f.Attributes - System.IO.FileAttributes.ReadOnly);
                System.IO.File.SetAttributes(DiversityWorkbench.ExceptionHandling._ErrorLogFilePath, attribute);
                attribute = (System.IO.FileAttributes)(f.Attributes - System.IO.FileAttributes.Hidden);
                System.IO.File.SetAttributes(DiversityWorkbench.ExceptionHandling._ErrorLogFilePath, attribute);
                System.IO.File.SetAttributes(DiversityWorkbench.ExceptionHandling._ErrorLogFilePath, System.IO.FileAttributes.Normal);
            }
            catch (System.Exception ex) { }
        }

        public static void WriteToErrorLogFile(System.Exception Exception, string Info)
        {
            try
            {
                string Logfile = DiversityWorkbench.ExceptionHandling.ErrorLogFilePath;// ...Windows.Forms.Application.StartupPath + "\\" + System.Windows.Forms.Application.ProductName.ToString() + "Error.log";
                EnsureAccess();
                System.IO.StreamWriter sw;
                if (System.IO.File.Exists(Logfile))
                    sw = new System.IO.StreamWriter(Logfile, true);
                else
                    sw = new System.IO.StreamWriter(Logfile);
                try
                {
                    // Markus 23.2.2021 - Message for debugging
                    string DebugMessage = Exception.Source + "\r\n" + Exception.TargetSite + "\r\n" + Exception.Message;

                    sw.WriteLine("User:\t" + System.Environment.UserName);
                    sw.WriteLine("Date:\t" + DateTimeIsoString());
                    //sw.WriteLine();
                    sw.WriteLine("Source:\t" + Exception.Source);
                    sw.WriteLine("Target:\t" + Exception.TargetSite);
                    sw.WriteLine("Message:\t" + Exception.Message);
                    if (Exception.StackTrace.Contains(";Password="))
                    {
                        string Message = Exception.StackTrace.ToString();
                        while (Message.Contains(";Password="))
                            Message = Message.Substring(0, Message.IndexOf(";Password=")) + Message.Substring(Message.IndexOf(";Password=") + 20);
                    }
                    else
                        sw.WriteLine("Stack:\t" + Exception.StackTrace.ToString());
                    // Markus 17.11.2020 - More details for SQL-Exceptions
                    if (Exception.GetType() == typeof(Microsoft.Data.SqlClient.SqlException))
                    {
                        Microsoft.Data.SqlClient.SqlException sqlException = (Microsoft.Data.SqlClient.SqlException)Exception;
                        foreach(Microsoft.Data.SqlClient.SqlError E in sqlException.Errors)
                        {
                            sw.WriteLine("Error:\t" + E.Message);
                            DebugMessage += "\r\n" + E.Message;
                        }
                    }
                    if (!string.IsNullOrEmpty(Info))
                        sw.WriteLine("Info:\t" + Info);
                    sw.WriteLine();
#if DEBUG
                    // Markus 23.2.2021 - Message for debugging
                    System.Windows.Forms.MessageBox.Show(DebugMessage);
#endif
                }
                catch
                {
                }
                finally
                {
                    sw.Close();
                }
            }
            catch (System.Exception ex) { }
        }

        public static void WriteToErrorLogFile(string Message, bool OmitMessagePrefix = false)
        {
            try
            {
                string Logfile = DiversityWorkbench.ExceptionHandling.ErrorLogFilePath;// ...Windows.Forms.Application.StartupPath + "\\" + System.Windows.Forms.Application.ProductName.ToString() + "Error.log";
                EnsureAccess();
                System.IO.StreamWriter sw;
                if (System.IO.File.Exists(Logfile))
                    sw = new System.IO.StreamWriter(Logfile, true);
                else
                    sw = new System.IO.StreamWriter(Logfile);
                try
                {
                    if (!OmitMessagePrefix)
                        Message = "Message:\t" + Message;
                    sw.WriteLine();
                    sw.WriteLine(Message);
                    //sw.WriteLine();
                }
                catch
                {
                }
                finally
                {
                    sw.Close();
                }
            }
            catch { }
        }

        public static void WriteToErrorLogFile(string Source, string Target, string Message, bool OmitUser = false, bool OmitMessagePrefix = false)
        {
            try
            {
                string Logfile = DiversityWorkbench.ExceptionHandling.ErrorLogFilePath;// ...Windows.Forms.Application.StartupPath + "\\" + System.Windows.Forms.Application.ProductName.ToString() + "Error.log";
                EnsureAccess();
                System.IO.StreamWriter sw;
                if (System.IO.File.Exists(Logfile))
                    sw = new System.IO.StreamWriter(Logfile, true);
                else
                    sw = new System.IO.StreamWriter(Logfile);
                try
                {
                    if (!OmitUser)
                        sw.WriteLine("User:\t" + System.Environment.UserName);
                    sw.WriteLine("Date:\t" + DateTimeIsoString());
                    //sw.WriteLine();
                    sw.WriteLine("Source:\t" + Source);
                    sw.WriteLine("Target:\t" + Target);
                    if (!OmitMessagePrefix)
                        Message = "Message:\t" + Message;
                    sw.WriteLine(Message);
                    sw.WriteLine();
                }
                catch
                {
                }
                finally
                {
                    sw.Close();
                }
            }
            catch { }
        }

        /// <summary>
        /// Notlösung - soll durch logging library ersetzt werden
        /// </summary>
        /// <param name="O"></param>
        /// <param name="e"></param>
        public static void WriteToErrorLogFile(object O, EventArgs e = null)
        {
            if (DiversityWorkbench.Settings.GenerateTraceFile)
            {
                System.Diagnostics.StackTrace stack = new System.Diagnostics.StackTrace();
                foreach(System.Diagnostics.StackFrame f in stack.GetFrames())
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(f.ToString(), "", "");
                }
            }
        }
        private static string DateTimeIsoString()
        {
            string IsoDate = System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"); //.Year.ToString() + "-";

            //if (System.DateTime.Now.Month < 10)
            //    IsoDate += "0";
            //IsoDate += System.DateTime.Now.Month.ToString() + "-";

            //if (System.DateTime.Now.Day < 10)
            //    IsoDate += "0";
            //IsoDate += System.DateTime.Now.Day.ToString() + "T";

            //if (System.DateTime.Now.Hour < 10)
            //    IsoDate += "0";
            //IsoDate += System.DateTime.Now.Hour.ToString() + ":";

            //if (System.DateTime.Now.Minute < 10)
            //    IsoDate += "0";
            //IsoDate += System.DateTime.Now.Minute.ToString() + ":";

            //if (System.DateTime.Now.Second < 10)
            //    IsoDate += "0";
            //IsoDate += System.DateTime.Now.Second.ToString();

            return IsoDate;
        }

        #region Event log
        
		public ExceptionHandling()
		{
            try
            {
                // Create the source, if it does not already exist.
                if (!System.Diagnostics.EventLog.SourceExists("DiversityWorkbench"))
                {
                    System.Diagnostics.EventLog.CreateEventSource("DiversityWorkbench", "ErrorLog");
                }

                // Create an EventLog instance and assign its source.
                System.Diagnostics.EventLog myLog = new System.Diagnostics.EventLog();
                myLog.Source = "DiversityWorkbench";

                // Write an informational entry to the event log.    
                myLog.WriteEntry("Writing to event log.");
            }
            catch (System.Exception ex)
            {
            }
		}

        public static void WriteToEventLog(string Sender, string Function, System.Collections.Hashtable Parameter, string Message, string WorkbenchModule)
        {
            try
            {
                string Msg = "Message:\t" + Message + "\r\n\r\n";
                if (Parameter.Count > 0)
                    Msg += "Param.:\tValue:\r\n";
                System.Collections.IDictionaryEnumerator Enumerator = Parameter.GetEnumerator();
                while (Enumerator.MoveNext())
                    Msg += Enumerator.Key + ":\t" + Enumerator.Value + "\r\n";
                DiversityWorkbench.ExceptionHandling.WriteToEventLog(Sender, Function, Msg, WorkbenchModule);
            }
            catch (System.Exception ex)
            {
            }
        }

        public static bool WriteToEventLog(string Sender, string Function, string Message, string WorkbenchModule)
        {
            try
            {
                if (!System.Diagnostics.EventLog.SourceExists(WorkbenchModule))
                    System.Diagnostics.EventLog.CreateEventSource(WorkbenchModule, "DiversityWorkbench");
                System.Diagnostics.EventLog EL = new System.Diagnostics.EventLog("DiversityWorkbench");
                EL.Source = WorkbenchModule;
                string Msg = "Sender:\t" + Sender.ToString() + "\r\n"
                    + "Function:\t" + Function + "\r\n"
                    + "Message:\t" + Message + "\r\n\r\n";
                EL.WriteEntry(Msg);
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        public static bool WriteToEventLog(string Source, string Message)
        {
            try
            {
                System.Diagnostics.EventLog EL = new System.Diagnostics.EventLog("Application");
                EL.Source = Source;
                EL.WriteEntry(Message);
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        public static bool WriteToLog(string Sender, string Function, string Message, string WorkbenchModule)
        {
            try
            {
                bool OK = true;
                OK = DiversityWorkbench.ExceptionHandling.WriteToEventLog(Sender, Function, Message, WorkbenchModule);
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(Sender, Function, Message);
                return OK;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region showErrorToUser
        public static void ShowErrorMessage(string message)
        {
            System.Windows.Forms.MessageBox.Show(message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        }

        public static bool ShowConfirmation(string message)
        {
            var result = System.Windows.Forms.MessageBox.Show(message, "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);
            return result == System.Windows.Forms.DialogResult.Yes;
        }

        #endregion
    }
}
