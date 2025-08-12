using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Xml;

namespace DiversityWorkbench
{
    public class ScriptProcessor : IDisposable
    {
        #region Fields and public properties

        private bool _Disposed = false;
        private string _SqlConnectionString;
        private string _ScriptFile;
        private string _ScriptText;
        private StringReader _ScriptReader;
        private XmlWriterSettings _XmlSettings;
        private XmlWriter _XmlWriter;
        private string _XmlFileName;
        private StreamWriter _ProtocolWriter;
        //Fixed file names for protocol handling
        private static string _XsltStyle;
        private const string _DefaultXmlFile = "defaultSQL.xml";   //TODO genaue Einbindung nochmal überlegen
        //Values for control of summary output in protocol file
        private int _StepCount = 0;
        private int _StepExecuted = 0;
        private int _OccuredError = 0;
        private int _AcceptedError = 16;
        private string _Errors = string.Empty;

        private string _PostgresError = "";
        private bool _ForPostgres;

        public bool ForPostgres
        {
            get { return _ForPostgres; }
            set { _ForPostgres = value; }
        }

        public string Errors
        {
            get { return _Errors; }
        }

        public int AcceptedError
        {
            get { return _AcceptedError; }
            set { _AcceptedError = value; }
        }

        public static string XsltStyle
        {
            get { return _XsltStyle; }
            set { if (value != _XsltStyle) _XsltStyle = value; }
        }
        #endregion

        #region Ctors
        /// <summary>
        /// Create a ScriptProcessor with SQL connection
        /// </summary>
        /// <param name="sqlConnectionString">SQL connection string</param>
        public ScriptProcessor(string sqlConnectionString)
        {
            this._SqlConnectionString = sqlConnectionString;
            _XmlSettings = new XmlWriterSettings();
            _XmlSettings.Indent = true;
            _XmlSettings.IndentChars = "  ";
        }
        /// <summary>
        /// Create a ScriptProcessor with SQL connection and SQL script
        /// </summary>
        /// <param name="sqlConnectionString">SQL connection string</param>
        /// <param name="sqlScriptFile">SQL script file info</param>
        public ScriptProcessor(string sqlConnectionString, FileInfo sqlScriptFile)
            : this(sqlConnectionString)
        {
            ReadScriptFile(sqlScriptFile);
        }
        /// <summary>
        /// Create a ScriptProcessor with SQL connection, SQL script and XML protocol file
        /// </summary>
        /// <param name="sqlConnectionString">SQL connection string</param>
        /// <param name="sqlScriptFile">SQL script file info</param>
        /// <param name="xmlFile">XML protocol file info</param>
        public ScriptProcessor(string sqlConnectionString, FileInfo sqlScriptFile, FileInfo xmlFile)
            : this(sqlConnectionString, sqlScriptFile)
        {
            SetXmlFile(xmlFile);
        }
        /// <summary>
        /// Implement IDisposable
        /// </summary>
        public void Dispose()
        {
            if (!_Disposed)
            {
                if (_ProtocolWriter != null)
                {
                    _ProtocolWriter.Close();
                    _ProtocolWriter.Dispose();
                    _ProtocolWriter = null;
                }
                Close();
                GC.SuppressFinalize(this);
                _Disposed = true;
            }
        }
        /// <summary>
        /// Destructor
        /// </summary>
        ~ScriptProcessor()
        {
            Dispose();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Read SQL script file to private sriptText field and start the scriptReader.
        /// The SQL file info is stored in private scriptFile field.
        /// </summary>
        /// <param name="file">SQL script file info</param>
        private void ReadScriptFile(FileInfo sqlFile)
        {
            if (_ScriptReader != null)
            {
                _ScriptReader.Close();
                _ScriptReader.Dispose();
            }
            if (sqlFile.Exists)
            {
                _ScriptFile = sqlFile.FullName;
                StreamReader strRead = sqlFile.OpenText();
                _ScriptText = strRead.ReadToEnd();
                strRead.Close();
                strRead.Dispose();
                _ScriptReader = new StringReader(_ScriptText);
            }
            else
            {
                _ScriptFile = null;
                _ScriptReader = null;
            }
            if (_ProtocolWriter != null)
            {
                _ProtocolWriter.WriteLine(">>>SQL script file opened<<<");
                _ProtocolWriter.WriteLine(sqlFile.FullName);
                _ProtocolWriter.WriteLine();
            }
        }

        private void WriteXmlSummary()
        {
            if (_XmlWriter != null)
            {
                _XmlWriter.WriteStartElement("Summary");
                if (_OccuredError > _AcceptedError)
                    _XmlWriter.WriteAttributeString("State", "Aborted");
                else
                    _XmlWriter.WriteAttributeString("State", "Executed");
                _XmlWriter.WriteStartElement("Info");
                _XmlWriter.WriteAttributeString("Executed", _StepExecuted.ToString());
                _XmlWriter.WriteAttributeString("Total", _StepCount.ToString());
                _XmlWriter.WriteAttributeString("MaxClass", _OccuredError.ToString());
                _XmlWriter.WriteEndElement(); //Info
                _XmlWriter.WriteEndElement(); //Summary
            }
        }
        #endregion

        #region Public set and auxiliary methods
        /// <summary>
        /// Closes actual XML protocol file an opens a new one
        /// </summary>
        /// <param name="xmlFile">XML protocol file info</param>
        public void SetXmlFile(FileInfo xmlFile)
        {
            if (_XmlWriter != null)
            {
                WriteXmlSummary();
                _XmlWriter.WriteEndDocument();
                _XmlWriter.Close();
            }
            _XmlFileName = xmlFile.FullName;
            _XmlWriter = XmlWriter.Create(xmlFile.FullName, _XmlSettings);
            _XmlWriter.WriteStartElement("Script");
            if (_ScriptFile != null)
            {
                _XmlWriter.WriteElementString("ScriptFile", _ScriptFile);
            }

            //Initialize values for script protocol
            _StepCount = 0;
            _StepExecuted = 0;
            _OccuredError = 0;

            if (_ProtocolWriter != null)
            {
                _ProtocolWriter.WriteLine(">>>XML Protocol file opened<<<");
                _ProtocolWriter.WriteLine(xmlFile.FullName);
                _ProtocolWriter.WriteLine();
            }
        }

        /// <summary>
        /// Closes actual protocol file an opens a new one
        /// </summary>
        /// <param name="protocolFile">Protocol file info</param>
        public void SetProtocolFile(FileInfo protocolFile)
        {
            if (_ProtocolWriter != null)
            {
                _ProtocolWriter.WriteLine(">>>New protocol file: {0}<<<", protocolFile.FullName);
                _ProtocolWriter.Close();
                _ProtocolWriter.Dispose();
            }
            _ProtocolWriter = new StreamWriter(protocolFile.FullName);
        }

        /// <summary>
        /// Closes the actual protocol file
        /// </summary>
        public void CloseProtocolFile()
        {
            if (_ProtocolWriter != null)
            {
                _ProtocolWriter.Close();
                _ProtocolWriter.Dispose();
                _ProtocolWriter = null;
            }
        }

        /// <summary>
        /// Sets the connection string
        /// </summary>
        /// <param name="connectionString">New connection string</param>
        public void SetConnectionString(string connectionString)
        {
            _SqlConnectionString = connectionString;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Opens processing of a new script file
        /// </summary>
        /// <param name="sqlScriptFile">SQL script file info</param>
        /// <returns>SQL script text</returns>
        public string Open(FileInfo sqlScriptFile)
        {
            _Errors = string.Empty;
            ReadScriptFile(sqlScriptFile);
            return _ScriptText;
        }

        /// <summary>
        /// Opens processing of a new script file and XML protocol file
        /// </summary>
        /// <param name="sqlScriptFile">SQL script file info</param>
        /// <param name="xmlFile">XML protocol file info</param>
        /// <returns>SQL script text</returns>
        public string Open(FileInfo sqlScriptFile, FileInfo xmlFile)
        {
            _Errors = string.Empty;
            ReadScriptFile(sqlScriptFile);
            SetXmlFile(xmlFile);
            return _ScriptText;
        }

        /// <summary>
        /// Opens processing of a new script text
        /// </summary>
        /// <param name="sqlScriptText">SQL script text</param>
        /// <param name="sqlScriptName">SQL script name</param>
        public void Open(string sqlScriptText, string sqlScriptName)
        {
            _Errors = string.Empty;
            if (_ScriptReader != null)
            {
                _ScriptReader.Close();
                _ScriptReader.Dispose();
            }
            
            _ScriptText = sqlScriptText;
            _ScriptReader = new StringReader(_ScriptText);
            _ScriptFile = sqlScriptName;
            
            if (_ProtocolWriter != null)
            {
                _ProtocolWriter.WriteLine(">>>SQL script text opened<<<");
                _ProtocolWriter.WriteLine();
            }
        }

        /// <summary>
        /// Opens processing of a new script text and XML protocol file
        /// </summary>
        /// <param name="sqlScriptText">SQL script text</param>
        /// <param name="sqlScriptName">SQL script name</param>
        /// <param name="xmlFile">XML protocol file info</param>
        public void Open(string sqlScriptText, string sqlScriptName, FileInfo xmlFile)
        {
            Open(sqlScriptText, sqlScriptName);
            SetXmlFile(xmlFile);
        }

        /// <summary>
        /// Closes actual script text, XML and text protocol files
        /// </summary>
        public void Close()
        {
            if (_ScriptReader != null)
            {
                _ScriptReader.Close();
                _ScriptReader.Dispose();
                _ScriptReader = null;
                _ScriptText = null;
                _ScriptFile = null;
            }
            if (_XmlWriter != null)
            {
                WriteXmlSummary();
                _XmlWriter.WriteEndDocument();
                _XmlWriter.Close();
                _XmlWriter = null;
            }
        }

        /// <summary>
        /// Get next command step from SQL script text
        /// Comentary lines starting with '--' and empty
        /// GO commands will be ignored
        /// </summary>
        /// <param name="processedLength">Processed text length, 0 if end is reached</param>
        /// <returns>SQL command text</returns>
        public string NextStep(out int processedLength)
        {
            bool comment = false;
            processedLength = 0;

            if (_ScriptReader == null)
            {
                return string.Empty;
            }

            StringBuilder strBld = new StringBuilder();

            while (_ScriptReader.Peek() > 0)
            {
                string textLine = _ScriptReader.ReadLine();
                processedLength += textLine.Length + 2;

                //if (textLine.Contains("--")) Removed Toni 13/09/03 problem with text strings containing '--'
                //    textLine = textLine.Substring(0, textLine.IndexOf("--"));

                string compLine = textLine.Trim(' ', '\t');

                if (!comment)
                {
                    if (ForPostgres && compLine.StartsWith("--##", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (strBld.Length > 0)
                        {
                            return strBld.ToString();
                        }
                    }
                    else if (compLine.StartsWith("GO", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (strBld.Length > 0)
                        {
                            return strBld.ToString();
                        }
                    }
                    else if (compLine.StartsWith("/*") && !compLine.EndsWith("*/"))
                    {
                        comment = true;
                    }
                }
                else if (compLine.EndsWith("*/"))
                {
                    comment = false;
                }
                strBld.AppendLine(textLine);
            }
            return strBld.ToString();
        }

        /// <summary>
        /// Send command step to the SQL database and evaluates result
        /// </summary>
        /// <param name="textBlock">SQL command Text</param>
        /// <param name="resultText">Result Text</param>
        /// <returns>0 if o.k, > 0 highest occured error</returns>
        public int ExecStep(string textBlock, out string resultText)
        {
            if (textBlock.Length <= 0)
            {
                resultText = string.Empty;
                return 0;
            }

            SqlConnection con;
            SqlCommand cmdSQL;
            con = new SqlConnection(_SqlConnectionString);
            _StepCount++;
            cmdSQL = new SqlCommand(textBlock, con);
            cmdSQL.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
            StringBuilder strBld = new StringBuilder();
            int errCode = 0;

            if (_XmlWriter != null)
            {
                _XmlWriter.WriteStartElement("Step");
                _XmlWriter.WriteStartElement("Command");
                _XmlWriter.WriteCData(textBlock);
                _XmlWriter.WriteEndElement(); //Command
            }
            if (_ProtocolWriter != null)
            {
                _ProtocolWriter.WriteLine(">>>Command<<<");
                _ProtocolWriter.Write(textBlock);
            }

            try
            {
                if (!ForPostgres && con.ConnectionString.Length > 0)
                    con.Open();
            }
            catch (Exception e)
            {
                errCode = 20;
                _Errors += e.Message + "\r\nSQL-Statement:\r\nOpen Connection\r\n\r\n";
                strBld.AppendLine(">>>Connection error<<<");
                strBld.AppendLine(e.Message);
                
                if (_XmlWriter != null)
                {
                    _XmlWriter.WriteStartElement("Error");
                    _XmlWriter.WriteStartElement("Info");
                    _XmlWriter.WriteAttributeString("Class", "20");
                    _XmlWriter.WriteAttributeString("Step", _StepCount.ToString());
                    _XmlWriter.WriteEndElement(); //Info
                    _XmlWriter.WriteStartElement("Message");
                    _XmlWriter.WriteCData(e.Message);
                    _XmlWriter.WriteEndElement(); //Message
                    _XmlWriter.WriteEndElement(); //Error
                    _XmlWriter.WriteEndElement(); //Step
                    _XmlWriter.Flush();
                }
            }

            if (errCode == 0)
            {
                try
                {
                    //int resSQL = cmdSQL.ExecuteNonQuery();
                    //boxResult.Text += ">>>Rückgabewert: " + resSQL.ToString() + "\r\n";
                    SqlDataReader drSQL = cmdSQL.ExecuteReader();
                    strBld.AppendLine(">>>SQL Executed<<<");
                    _StepExecuted++;

                    do
                    {
                        int line = 0;
                        List<int> colWidth = new List<int>();
                        List<List<String>> colRows = new List<List<String>>();

                        while (drSQL.Read())
                        {
                            if (line == 0)
                            {
                                if (drSQL.RecordsAffected >= 0)
                                {
                                    strBld.AppendFormat("{0} Rows affected\r\n", drSQL.RecordsAffected.ToString());
                                }

                                for (int row = 0; row < drSQL.FieldCount; row++)
                                {
                                    colWidth.Add(drSQL.GetName(row).Length);
                                    colRows.Add(new List<string>());
                                }
                            }

                            for (int row = 0; row < drSQL.FieldCount; row++)
                            {
                                colRows[row].Add(drSQL[row].ToString());
                                if (colWidth[row] < colRows[row][line].Length)
                                {
                                    colWidth[row] = colRows[row][line].Length;
                                }
                            }
                            //prepare next line
                            line++;
                        }
                        //output table
                        strBld.AppendLine();
                        for (int c = 0; c < drSQL.FieldCount; c++)
                        {
                            strBld.Append(drSQL.GetName(c).PadRight(colWidth[c] + 1));
                        }
                        strBld.AppendLine();

                        for (int r = 0; r < line; r++)
                        {
                            for (int c = 0; c < drSQL.FieldCount; c++)
                            {
                                strBld.Append(colRows[c][r].PadRight(colWidth[c] + 1));
                            }
                            strBld.AppendLine();
                        }

                        //output to XML
                        if (_XmlWriter != null)
                        {
                            _XmlWriter.WriteStartElement("Result");
                            _XmlWriter.WriteAttributeString("RecordsAffected", drSQL.RecordsAffected.ToString());

                            if (drSQL.FieldCount > 0)
                            {
                                _XmlWriter.WriteStartElement("Table");

                                for (int r = 0; r < line; r++)
                                {
                                    _XmlWriter.WriteStartElement("Row");
                                    for (int c = drSQL.FieldCount - 1; c >= 0; c--)
                                    {
                                        if (colRows[c][r] != string.Empty)
                                        {
                                            try
                                            {
                                                _XmlWriter.WriteAttributeString((drSQL.GetName(c) == string.Empty) ? "NULL" : drSQL.GetName(c), colRows[c][r]);
                                            }
                                            catch (Exception ex)
                                            {
                                                _XmlWriter.WriteComment(ex.Message);
                                            }
                                        }
                                    }
                                    _XmlWriter.WriteEndElement(); //Row
                                }
                                _XmlWriter.WriteEndElement(); //Table
                            }
                            _XmlWriter.WriteEndElement(); //Result
                            _XmlWriter.Flush();
                        }

                        //release auxilliary collections
                        colWidth.Clear();
                        colWidth = null;
                        for (int c = 0; c < drSQL.FieldCount; c++)
                        {
                            colRows[c].Clear();
                            colRows[c] = null;
                        }
                        colRows.Clear();
                        colRows = null;
                    } while (drSQL.NextResult());

                    drSQL.Close();

                    if (_XmlWriter != null)
                    {
                        _XmlWriter.WriteEndElement(); //Step
                        _XmlWriter.Flush();
                    }
                }
                catch (SqlException ex)
                {
                    _Errors += ex.Message + "\r\nSQL-Statement:\r\n" + textBlock + "\r\n\r\n";
                    strBld.AppendLine(">>>SQL Error<<<");
                    foreach (SqlError item in ex.Errors)
                    {
                        strBld.AppendFormat("Number {0}, Level {1}, State {2}, Line {3}\r\n", item.Number, item.Class, item.State, item.LineNumber);
                        strBld.AppendLine(item.Message.ToString());

                        if (_XmlWriter != null)
                        {
                            _XmlWriter.WriteStartElement("Error");
                            _XmlWriter.WriteStartElement("Info");
                            _XmlWriter.WriteAttributeString("Line", item.LineNumber.ToString());
                            _XmlWriter.WriteAttributeString("Procedure", item.Procedure.ToString());
                            _XmlWriter.WriteAttributeString("Number", item.Number.ToString());
                            _XmlWriter.WriteAttributeString("Class", item.Class.ToString());
                            _XmlWriter.WriteAttributeString("State", item.State.ToString());
                            _XmlWriter.WriteAttributeString("Source", item.Source.ToString());
                            _XmlWriter.WriteAttributeString("Server", item.Server.ToString());
                            _XmlWriter.WriteAttributeString("Step", _StepCount.ToString());
                            _XmlWriter.WriteEndElement(); //Info
                            _XmlWriter.WriteStartElement("Message");
                            _XmlWriter.WriteCData(item.Message.ToString());
                            _XmlWriter.WriteEndElement(); //Message
                            _XmlWriter.WriteEndElement(); //Error
                        }

                        if (item.Class > errCode)
                        {
                            errCode = item.Class;
                        }
                    }

                    if (_XmlWriter != null)
                    {
                        _XmlWriter.WriteEndElement(); //Step
                        _XmlWriter.Flush();
                    }
                }
                con.Close();
            }

            if (_ProtocolWriter != null)
            {
                _ProtocolWriter.WriteLine(strBld.ToString());
                _ProtocolWriter.Flush();
            }

            if (errCode > _OccuredError)
            {
                _OccuredError = errCode;
            }

            resultText = strBld.ToString();
            return errCode;
        }

        /// <summary>
        /// Send command step to the SQL database and evaluates result
        /// </summary>
        /// <param name="textBlock">SQL command Text</param>
        /// <param name="resultText">Result Text</param>
        /// <returns>0 if o.k, > 0 highest occured error</returns>
        public int ExecStepPostgres(string textBlock, out string resultText)
        {
            if (textBlock.Length <= 0)
            {
                resultText = string.Empty;
                return 0;
            }

            Npgsql.NpgsqlConnection con;
            Npgsql.NpgsqlCommand cmdSQL;
            con = new Npgsql.NpgsqlConnection(DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString(DiversityWorkbench.Settings.TimeoutDatabase));// .Postgres.PostgresConnection().ConnectionString);
            cmdSQL = new Npgsql.NpgsqlCommand(textBlock, con);
            cmdSQL.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
            _StepCount++;
            StringBuilder strBld = new StringBuilder();
            string errCode = "";

            if (_XmlWriter != null)
            {
                _XmlWriter.WriteStartElement("Step");
                _XmlWriter.WriteStartElement("Command");
                _XmlWriter.WriteCData(textBlock);
                _XmlWriter.WriteEndElement(); //Command
            }
            if (_ProtocolWriter != null)
            {
                _ProtocolWriter.WriteLine(">>>Command<<<");
                _ProtocolWriter.Write(textBlock);
            }

            try
            {
                con.Open();
            }
            catch (Exception e)
            {
                errCode = "ERROR";
                _Errors += e.Message + "\r\nSQL-Statement:\r\nOpen Connection\r\n\r\n";
                strBld.AppendLine(">>>Connection error<<<");
                strBld.AppendLine(e.Message);

                if (_XmlWriter != null)
                {
                    _XmlWriter.WriteStartElement("Error");
                    _XmlWriter.WriteStartElement("Info");
                    _XmlWriter.WriteAttributeString("Class", "20");
                    _XmlWriter.WriteAttributeString("Step", _StepCount.ToString());
                    _XmlWriter.WriteEndElement(); //Info
                    _XmlWriter.WriteStartElement("Message");
                    _XmlWriter.WriteCData(e.Message);
                    _XmlWriter.WriteEndElement(); //Message
                    _XmlWriter.WriteEndElement(); //Error
                    _XmlWriter.WriteEndElement(); //Step
                    _XmlWriter.Flush();
                }
            }

            if (errCode == "")
            {
                try
                {
                    string Test = cmdSQL.CommandText;
                    //int resSQL = cmdSQL.ExecuteNonQuery();
                    //boxResult.Text += ">>>Rückgabewert: " + resSQL.ToString() + "\r\n";

                    // neuer Ansatz nach Update auf 5.0.3
                    cmdSQL.ExecuteNonQuery();
                    _StepExecuted++;
                    if (_XmlWriter != null)
                    {
                        _XmlWriter.WriteStartElement("SQL");
                        _XmlWriter.WriteString(cmdSQL.CommandText);
                        _XmlWriter.WriteEndElement();
                        _XmlWriter.Flush();
                    }

                    if (false)
                    {

                        Npgsql.NpgsqlDataReader drSQL = cmdSQL.ExecuteReader();

                        strBld.AppendLine(">>>SQL Executed<<<");
                        _StepExecuted++;

                        do
                        {
                            int line = 0;
                            List<int> colWidth = new List<int>();
                            List<List<String>> colRows = new List<List<String>>();

                            while (drSQL.Read())
                            {
                                if (line == 0)
                                {
                                    if (drSQL.RecordsAffected >= 0)
                                    {
                                        strBld.AppendFormat("{0} Rows affected\r\n", drSQL.RecordsAffected.ToString());
                                    }

                                    for (int row = 0; row < drSQL.FieldCount; row++)
                                    {
                                        colWidth.Add(drSQL.GetName(row).Length);
                                        colRows.Add(new List<string>());
                                    }
                                }

                                for (int row = 0; row < drSQL.FieldCount; row++)
                                {
                                    colRows[row].Add(drSQL[row].ToString());
                                    if (colWidth[row] < colRows[row][line].Length)
                                    {
                                        colWidth[row] = colRows[row][line].Length;
                                    }
                                }
                                //prepare next line
                                line++;
                            }
                            //output table
                            strBld.AppendLine();
                            for (int c = 0; c < drSQL.FieldCount; c++)
                            {
                                try
                                {
                                    strBld.Append(drSQL.GetName(c).PadRight(colWidth[c] + 1));
                                }
                                catch (System.Exception ex)
                                {
                                }
                            }
                            strBld.AppendLine();

                            for (int r = 0; r < line; r++)
                            {
                                for (int c = 0; c < drSQL.FieldCount; c++)
                                {
                                    strBld.Append(colRows[c][r].PadRight(colWidth[c] + 1));
                                }
                                strBld.AppendLine();
                            }

                            //output to XML
                            if (_XmlWriter != null)
                            {
                                _XmlWriter.WriteStartElement("Result");
                                _XmlWriter.WriteAttributeString("RecordsAffected", drSQL.RecordsAffected.ToString());

                                if (drSQL.FieldCount > 0)
                                {
                                    _XmlWriter.WriteStartElement("Table");

                                    for (int r = 0; r < line; r++)
                                    {
                                        _XmlWriter.WriteStartElement("Row");
                                        for (int c = drSQL.FieldCount - 1; c >= 0; c--)
                                        {
                                            if (colRows[c][r] != string.Empty)
                                            {
                                                try
                                                {
                                                    _XmlWriter.WriteAttributeString((drSQL.GetName(c) == string.Empty) ? "NULL" : drSQL.GetName(c), colRows[c][r]);
                                                }
                                                catch (Exception ex)
                                                {
                                                    _XmlWriter.WriteComment(ex.Message);
                                                }
                                            }
                                        }
                                        _XmlWriter.WriteEndElement(); //Row
                                    }
                                    _XmlWriter.WriteEndElement(); //Table
                                }
                                _XmlWriter.WriteEndElement(); //Result
                                _XmlWriter.Flush();
                            }

                            //release auxilliary collections
                            colWidth.Clear();
                            colWidth = null;
                            for (int c = 0; c < drSQL.FieldCount; c++)
                            {
                                try
                                {
                                    colRows[c].Clear();
                                    colRows[c] = null;
                                }
                                catch (System.Exception ex)
                                {
                                }
                            }
                            colRows.Clear();
                            colRows = null;
                        } while (drSQL.NextResult());

                        drSQL.Close();

                        if (_XmlWriter != null)
                        {
                            _XmlWriter.WriteEndElement(); //Step
                            _XmlWriter.Flush();
                        }
                    }

                }
                //catch (SqlException ex)
                //{
                //    _Errors += ex.Message + "\r\nSQL-Statement:\r\n" + textBlock + "\r\n\r\n";
                //    strBld.AppendLine(">>>SQL Error<<<");
                //    foreach (SqlError item in ex.Errors)
                //    {
                //        strBld.AppendFormat("Number {0}, Level {1}, State {2}, Line {3}\r\n", item.Number, item.Class, item.State, item.LineNumber);
                //        strBld.AppendLine(item.Message.ToString());

                //        if (_XmlWriter != null)
                //        {
                //            _XmlWriter.WriteStartElement("Error");
                //            _XmlWriter.WriteStartElement("Info");
                //            _XmlWriter.WriteAttributeString("Line", item.LineNumber.ToString());
                //            _XmlWriter.WriteAttributeString("Procedure", item.Procedure.ToString());
                //            _XmlWriter.WriteAttributeString("Number", item.Number.ToString());
                //            _XmlWriter.WriteAttributeString("Class", item.Class.ToString());
                //            _XmlWriter.WriteAttributeString("State", item.State.ToString());
                //            _XmlWriter.WriteAttributeString("Source", item.Source.ToString());
                //            _XmlWriter.WriteAttributeString("Server", item.Server.ToString());
                //            _XmlWriter.WriteAttributeString("Step", _StepCount.ToString());
                //            _XmlWriter.WriteEndElement(); //Info
                //            _XmlWriter.WriteStartElement("Message");
                //            _XmlWriter.WriteCData(item.Message.ToString());
                //            _XmlWriter.WriteEndElement(); //Message
                //            _XmlWriter.WriteEndElement(); //Error
                //        }

                //        if (item.Class > errCode)
                //        {
                //            errCode = item.Class;
                //        }
                //    }

                //    if (_XmlWriter != null)
                //    {
                //        _XmlWriter.WriteEndElement(); //Step
                //        _XmlWriter.Flush();
                //    }
                //}
                catch(Npgsql.NpgsqlException ex)
                {
                    _Errors += ex.Message + "\r\nSQL-Statement:\r\n" + textBlock + "\r\n\r\n";
                    strBld.AppendLine(">>>SQL Error<<<");
                    // replacement for code below (new version of npgsql) - did not work - conflict of npgsql version
                    strBld.AppendLine(ex.Message.ToString());
                    if (_XmlWriter != null)
                    {
                        _XmlWriter.WriteStartElement("Error");
                        _XmlWriter.WriteStartElement("Info");
                        _XmlWriter.WriteElementString("Stack", ex.StackTrace);
                    }


                    // Old npgsql verison NpgsqlError does not exist any longer - restored (see above)
                    //foreach (Npgsql.NpgsqlError item in ex.Errors)
                    //{
                    //    strBld.AppendFormat("Position {0}, Code {1}, Severity {2}, Line {3}\r\n", item.Position, item.Code, item.Severity, item.Line);
                    //    strBld.AppendLine(item.Message.ToString());

                    //    if (_XmlWriter != null)
                    //    {
                    //        _XmlWriter.WriteStartElement("Error");
                    //        _XmlWriter.WriteStartElement("Info");
                    //        _XmlWriter.WriteAttributeString("Line", item.Line.ToString());
                    //        _XmlWriter.WriteAttributeString("Routine", item.Routine.ToString());
                    //        _XmlWriter.WriteAttributeString("Position", item.Position.ToString());
                    //        _XmlWriter.WriteAttributeString("Code", item.Code.ToString());
                    //        _XmlWriter.WriteAttributeString("Severity", item.Severity.ToString());
                    //        _XmlWriter.WriteAttributeString("SchemaName", item.SchemaName.ToString());
                    //        _XmlWriter.WriteAttributeString("Message", item.Message.ToString());
                    //        _XmlWriter.WriteAttributeString("Step", _StepCount.ToString());
                    //        _XmlWriter.WriteEndElement(); //Info
                    //        _XmlWriter.WriteStartElement("Message");
                    //        _XmlWriter.WriteCData(item.Message.ToString());
                    //        _XmlWriter.WriteEndElement(); //Message
                    //        _XmlWriter.WriteEndElement(); //Error
                    //    }

                    //    if (item.Severity != errCode)
                    //    {
                    //        errCode = item.Severity;
                    //    }
                    //}

                    if (_XmlWriter != null)
                    {
                        _XmlWriter.WriteEndElement(); //Step
                        _XmlWriter.Flush();
                    }
                }
                con.Close();
            }

            if (_ProtocolWriter != null)
            {
                _ProtocolWriter.WriteLine(strBld.ToString());
                _ProtocolWriter.Flush();
            }

            if (errCode != _PostgresError)
            {
                _PostgresError = errCode;
            }

            //if (errCode != _OccuredError)
            //{
            //    _OccuredError = errCode;
            //}

            resultText = strBld.ToString();
            if (errCode != "")
                return 20;
            //return errCode;
            return 0;
        }

        /// <summary>
        /// Runs opened SQL script
        /// </summary>
        /// <returns>0 if o.k, > 0 highest occured error</returns>
        /// After execution the files (script and xml) will be closed!
        public int Run()
        {
            if (_ScriptReader == null)
            {
                return 0;
            }

            int errCode = 0;
            int textLength;
            string textBlock;
            string resultText;

            if (_XmlWriter == null)
            {
                SetXmlFile(new FileInfo(_DefaultXmlFile));
            }

            while ((textBlock = NextStep(out textLength)) != string.Empty)
            {
                errCode = ExecStep(textBlock, out resultText);
                if (_AcceptedError < errCode)
                {
                    break;
                }
            }

            if (_ProtocolWriter != null)
            {
                if (errCode > _AcceptedError)
                    _ProtocolWriter.WriteLine(">>>SQL Script aborted<<<");
                else
                    _ProtocolWriter.WriteLine(">>>SQL Seript executed<<<");
                _ProtocolWriter.WriteLine(">>>SQL Error class max. {0}<<<", _OccuredError);
                _ProtocolWriter.WriteLine();
                _ProtocolWriter.Flush();
            }
            return errCode;
        }

        /// <summary>
        /// Convert XML protocol file to HTML output
        /// </summary>
        /// <param name="xmlFile">XML protocol file info</param>
        /// <returns>HTML output file name</returns>
        public string ConvertResult()
        {
            System.IO.FileInfo xmlFile;

            if (_XmlFileName != null)
            {
                xmlFile = new FileInfo(_XmlFileName);
            }
            else
            {
                return string.Empty;
            }

            if (!xmlFile.Exists)
            {
                return string.Empty;
            }

            return ConvertResult(xmlFile);
        }

        #endregion

        #region Static methods
        /// <summary>
        /// Convert XML protocol file to HTML output
        /// </summary>
        /// <param name="xmlFile">XML protocol file info</param>
        /// <returns>HTML output file name</returns>
        public static string ConvertResult(FileInfo xmlFile)
        {
            if (xmlFile == null)
            {
                xmlFile = new FileInfo(_DefaultXmlFile);
            }

            if (!xmlFile.Exists)
            {
                return string.Empty;
            }

            try
            {
                bool loadXmlResource = true;
                System.Xml.Xsl.XslCompiledTransform xslt = new System.Xml.Xsl.XslCompiledTransform();

                // check if XML style sheet file has been specified
                if (_XsltStyle != null)
                {
                    FileInfo xsltStyleSheet = new FileInfo(_XsltStyle);
                    if (xsltStyleSheet.Exists)
                    {
                        xslt.Load(xsltStyleSheet.FullName);
                        loadXmlResource = false;
                    }
                }
                
                if (loadXmlResource)
                {
                    // load default style sheet from resources
                    StringReader xsltReader = new StringReader(DiversityWorkbench.Properties.Resources.DatabaseUpdateSummary);
                    System.Xml.XmlReader xml = System.Xml.XmlReader.Create(xsltReader);
                    xslt.Load(xml);
                }
                
                // Load the file to transform.
                System.Xml.XPath.XPathDocument doc = new System.Xml.XPath.XPathDocument(xmlFile.FullName);

                // The output file:
                string outputFile = xmlFile.FullName.Substring(0, xmlFile.FullName.Length
                    - xmlFile.Extension.Length) + ".htm";

                // Create the writer.             
                System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(outputFile, xslt.OutputSettings);

                // Transform the file and send the output to the console.
                xslt.Transform(doc, writer);
                writer.Close();
                return outputFile;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        #endregion
    }
}
