using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityWorkbench.Import
{
    public class RecordSQL
    {
        private static bool _Record = false;
        public static bool Record { set => _Record = value; get => _Record; }

        private static string _LastRecordFile = "";

        public static string LastRecordFile { get => _LastRecordFile; }

        public static string SQLfile()
        {
            _LastRecordFile = Import.DefaultFileName() + "_SQL.sql";
            return _LastRecordFile;
        }

        private static int _Line = 0;

        public static void AddSQLStatement(string SQL, string Error = "")
        {
            System.IO.StreamWriter sw = streamWriter;
            WriteUseDatabase();
            if (Error.Length > 0)
                SQL += ";\r\n/*failed:\r\n" + Error + "\r\n*/";
            SQL += "\r\nGO\r\n";
            sw.WriteLine(SQL);
        }

        private static void WriteUseDatabase()
        {
            if (_Line == 0)
            {
                string DB = "";
                if (DiversityWorkbench.Settings.DatabaseName != null && DiversityWorkbench.Settings.DatabaseName.Length > 0) { DB = DiversityWorkbench.Settings.DatabaseName; }
                else { return; }
                string UseDB = "USE " + DB;
                _Line++;
                AddSQLStatement(UseDB);
            }
        }

        private static System.IO.StreamWriter _streamWriter;
        private static System.IO.StreamWriter streamWriter
        {
            get
            {
                if (_streamWriter == null)
                    _streamWriter = new System.IO.StreamWriter(SQLfile(), false, DiversityWorkbench.Import.Import.Encoding);
                return _streamWriter;
            }
        }

        //public static void Restart() 
        //{
        //    Finish();
        //    _streamWriter = null; 
        //}

        public static void Finish() 
        {
            if (_streamWriter != null)
            {
                _streamWriter.Close();
                _streamWriter.Dispose();
                _streamWriter= null;
            }
            _Line = 0;
        }

    }
}
