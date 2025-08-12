using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench
{
    public class TraceFileGeneration
    {
        private static string _TraceFilePath = "";

        public static string TraceFilePath
        {
            get
            {
                if (DiversityWorkbench.TraceFileGeneration._TraceFilePath.Length == 0)
                {
                    try
                    {
                        DiversityWorkbench.TraceFileGeneration._TraceFilePath = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule() + "\\" + System.Windows.Forms.Application.ProductName.ToString() + "Trace.txt";
                    }
                    catch { }
                }
                return TraceFileGeneration._TraceFilePath;
            }
        }

        //public static string TraceFile
        //{
        //    get
        //    {
        //        string TraceFile = "";
        //        string Line = "";
        //        bool Current = false;
        //        try
        //        {
        //            string TracePath = DiversityWorkbench.TraceFileGeneration.TraceFilePath;
        //            System.IO.StreamReader sr;
        //            if (System.IO.File.Exists(TracePath))
        //            {
        //                sr = new System.IO.StreamReader(TracePath);
        //                while (!sr.EndOfStream)
        //                {
        //                    Line = sr.ReadLine();
        //                    if (Line.StartsWith("Date:"))
        //                    {
        //                        string Date = Line.Substring(Line.IndexOf("\t")).Trim();
        //                        System.DateTime d;
        //                        if (System.DateTime.TryParse(Date, out d))
        //                        {
        //                            if (d.Date.ToString() == System.DateTime.Now.Date.ToString())
        //                            {
        //                                Current = true;
        //                            }
        //                        }
        //                    }
        //                    if (Current)
        //                        TraceFile += sr.ReadLine();
        //                }
        //            }
        //        }
        //        catch { }
        //        return TraceFile;
        //    }
        //}

        public static void WriteToTraceFile(System.Windows.Forms.Form Form, string Function, string Message)
        {
            TraceFileGeneration.WriteToTraceFile("Form", Form.Name, Function, Message);
        }

        public static void WriteToTraceFile(System.Windows.Forms.UserControl Control, string Function, string Message)
        {
            TraceFileGeneration.WriteToTraceFile("Control", Control.Name, Function, Message);
        }

        public static void WriteToTraceFile(string Name, string Function, string Message)
        {
            TraceFileGeneration.WriteToTraceFile("Class", Name, Function, Message);
        }

        private static void WriteToTraceFile(string Type, string Object, string Function, string Message)
        {
            try
            {
                string Tracefile = DiversityWorkbench.TraceFileGeneration.TraceFilePath;// ...Windows.Forms.Application.StartupPath + "\\" + ...Windows.Forms.Application.ProductName.ToString() + "Error.log";
                System.IO.StreamWriter sw;
                if (System.IO.File.Exists(Tracefile))
                {
                    sw = new System.IO.StreamWriter(Tracefile, true);
                }
                else
                {
                    sw = new System.IO.StreamWriter(Tracefile);
                    sw.WriteLine("User:\t" + System.Environment.UserName);
                    sw.Write("Date:\t");
                    sw.WriteLine(DateTime.Now);
                    sw.WriteLine();
                }
                try
                {
                    sw.WriteLine(Type + ":\t" + Object);
                    sw.WriteLine("Function:\t" + Function);
                    sw.WriteLine("Message:\t" + Message);
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

    }
}
