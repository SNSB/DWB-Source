using DiversityWorkbench.DwbManual;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormExportToCSV : Form
    {
        private string _colSeparator;
        private string _rowSeparator;
        private string _bcpVersion = "";
        private string _databaseVersion = "";
        System.Diagnostics.ProcessStartInfo _bcpInfo;

        #region Construction

        public FormExportToCSV()
        {
            InitializeComponent();
            if (bulkCopyAvailable())
            {
                this.comboBoxColumn.SelectedIndex = 0;
                this.comboBoxRow.SelectedIndex = 0; ;
                string outPath = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Export) + DiversityWorkbench.Settings.DatabaseName;
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(outPath);
                if (!di.Exists) di.Create();
                this.textBoxDirectory.Text = outPath;
                this.folderBrowserDialog.SelectedPath = outPath;
                fillTables();
            }
            else
            {
                MessageBox.Show("Bulk copy tool is not available.\r\n\r\nPlease check your SQL server installation!", "Bulk copy not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.buttonStart.Enabled = false; // Toni 20140122: Disable start button if no bulk copy available
            }
        }

        public FormExportToCSV(string HelpNameSpace)
            : this()
        {
            this.helpProvider.HelpNamespace = HelpNameSpace;
        }

        #endregion

        #region Public interfaces
        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
            this.helpProvider.SetHelpKeyword(this, KeyWord);
        }

        #endregion

        #region private events
        private bool bulkCopyAvailable()
        {
            try
            {
                // prepare start parameter for bulk copy tool
                _bcpInfo = new System.Diagnostics.ProcessStartInfo("bcp.exe");
                _bcpInfo.UseShellExecute = false;
                _bcpInfo.CreateNoWindow = true;
                _bcpInfo.RedirectStandardOutput = true;
                _bcpInfo.Arguments = "-v";

                using (System.Diagnostics.Process bcp = System.Diagnostics.Process.Start(_bcpInfo))
                {
                    _bcpVersion = bcp.StandardOutput.ReadToEnd();
                    if (bcp.WaitForExit(1000))
                        if (bcp.ExitCode == 0 && _bcpVersion.Contains("Microsoft SQL Server"))
                        {
                            _bcpVersion = _bcpVersion.Substring(_bcpVersion.LastIndexOf(' ')).Trim();
                            return true;
                        }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return false;
        }

        private void fillTables()
        {
            try
            {
                // get tables
                string SQL = string.Format("SELECT [TABLE_NAME]\r\n" +
                                           "FROM [{0}].[INFORMATION_SCHEMA].[TABLES]\r\n" +
                                           "WHERE [TABLE_TYPE] = 'BASE TABLE' AND [TABLE_SCHEMA] = 'dbo'\r\n" +
                                           "ORDER BY [TABLE_NAME]", DiversityWorkbench.Settings.ServerConnection.DatabaseName);

                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ServerConnection.ConnectionString);
                a.Fill(dt);

                // include tables in list box
                System.Windows.Forms.TreeNode node;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string t = dt.Rows[i]["TABLE_NAME"].ToString().ToUpper();
                    if (t != "SYSDIAGRAMS" && t != "DTPROPERTIES")
                    {
                        node = new TreeNode(dt.Rows[i]["TABLE_NAME"].ToString());
                        node.Checked = true;
                        if (t.EndsWith("_LOG") || t.Contains("_LOG_"))
                            if (!this.checkBoxLog.Checked) node.Checked = false;
                        if (t.EndsWith("_ENUM"))
                            if (!checkBoxEnum.Checked) node.Checked = false;
                        this.treeViewTables.Nodes.Add(node);
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private string xml2html(string xmlFile)
        {
            string outFile;
            // convert xml to html
            try
            {
                System.Xml.Xsl.XslCompiledTransform xslt = new System.Xml.Xsl.XslCompiledTransform();

                // load default style sheet from resources
                System.IO.StringReader xsltReader = new System.IO.StringReader(DiversityWorkbench.Properties.Resources.DatabaseExportSummary);
                System.Xml.XmlReader xml = System.Xml.XmlReader.Create(xsltReader);
                xslt.Load(xml);

                // Load the file to transform
                System.Xml.XPath.XPathDocument doc = new System.Xml.XPath.XPathDocument(xmlFile);

                // The output file:
                outFile = xmlFile.Substring(0, xmlFile.LastIndexOf('.')) + ".htm";

                // Create the writer.             
                System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(outFile, xslt.OutputSettings);

                // Transform the file and send the output to the console.
                xslt.Transform(doc, writer);
                writer.Close();
            }
            catch (Exception ex)
            {
                outFile = xmlFile;
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return outFile;
        }

        private void treeViewTables_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                MessageBox.Show("Errors for table " + e.Node.Text + ":\r\n" + e.Node.Tag, "Export error");
            }
        }

        private void buttonAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.treeViewTables.Nodes.Count; i++)
                this.treeViewTables.Nodes[i].Checked = true;
        }

        private void buttonNone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.treeViewTables.Nodes.Count; i++)
                this.treeViewTables.Nodes[i].Checked = false;
        }

        private void checkBoxEnum_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < this.treeViewTables.Nodes.Count; i++)
            {
                string t = this.treeViewTables.Nodes[i].Text.ToUpper();
                if (t.EndsWith("_ENUM"))
                    this.treeViewTables.Nodes[i].Checked = this.checkBoxEnum.Checked;
            }
        }

        private void checkBoxLog_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < this.treeViewTables.Nodes.Count; i++)
            {
                string t = this.treeViewTables.Nodes[i].Text.ToUpper();
                if (t.EndsWith("_LOG") || t.Contains("_LOG_"))
                    this.treeViewTables.Nodes[i].Checked = this.checkBoxLog.Checked;
            }
        }

        private void checkBoxSchema_CheckedChanged(object sender, EventArgs e)
        {
            this.checkBoxXML.Enabled = this.checkBoxSchema.Checked;
        }

        private void buttonDirectory_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBoxDirectory.Text = this.folderBrowserDialog.SelectedPath;
            }
        }

        private void comboBoxColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxColumn.SelectedIndex)
            {
                case 0: _colSeparator = null;
                    break;
                case 1: _colSeparator = ",";
                    break;
                case 2: _colSeparator = ";";
                    break;
                default:
                    break;
            }
        }

        private void comboBoxRow_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxRow.SelectedIndex)
            {
                case 0: _rowSeparator = null;
                    break;
                case 1: _rowSeparator = "\\0";
                    break;
                default:
                    break;
            }
        }

        private void textBoxResult_TextChanged(object sender, EventArgs e)
        {
            if (textBoxResult.Text == string.Empty)
            {
                this.buttonStart.Enabled = true;
                this.textBoxResult.Cursor = Cursors.Default;
            }
            else
            {
                this.buttonStart.Enabled = false;
                this.textBoxResult.Cursor = Cursors.Hand;
            }
        }

        private void textBoxResult_Click(object sender, EventArgs e)
        {
            if (textBoxResult.Text != string.Empty)
            {
                if (textBoxResult.Text.EndsWith(".xml", StringComparison.InvariantCultureIgnoreCase))
                    textBoxResult.Text = xml2html(textBoxResult.Text);
                FormWebBrowser html = new FormWebBrowser(textBoxResult.Text);
                html.ShowDialog();
            }
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name, System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString());
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            this.treeViewTables.Enabled = false;
            this.groupBoxOptions.Enabled = false;

            string cmdPar = string.Empty;
            string outFile = string.Empty;
            int errorCount = 0;
            int checkedItems = 0;
            string errorToolTip = "Click here to show errors";
            Color AllOk = Color.LightGreen;
            Color NotOk = Color.LightCoral;
            Color DataOk = Color.Yellow;

            this.Cursor = this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.groupBoxResult.Visible = false;
            this.groupBoxProgress.Visible = true;
            this.Update();

            // count checked items
            foreach (TreeNode item in this.treeViewTables.Nodes)
                if (item.Checked) checkedItems++;

            System.DateTime startTime = System.DateTime.Now;
            string xmlFile = this.textBoxDirectory.Text + "\\DbExportInfo.xml";
            System.Xml.XmlWriterSettings xmlSettings = new System.Xml.XmlWriterSettings();
            xmlSettings.Indent = true;
            xmlSettings.IndentChars = "  ";
            System.Xml.XmlWriter xmlWriter = System.Xml.XmlWriter.Create(xmlFile, xmlSettings);
            xmlWriter.WriteStartElement("ExportInfo");
            xmlWriter.WriteAttributeString("Database", Settings.ServerConnection.DatabaseName);

            try
            {
                // get SQL server version
                using (Microsoft.Data.SqlClient.SqlConnection dbc = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ServerConnection.ConnectionString))
                {
                    dbc.Open();
                    _databaseVersion = dbc.ServerVersion;
                    dbc.Close();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                xmlWriter.WriteStartElement("Error");
                xmlWriter.WriteCData(ex.Message);
                xmlWriter.WriteEndElement(); // Error
                errorCount++;
            }

            // write additional data
            xmlWriter.WriteAttributeString("MSSQLVersion", _databaseVersion);
            xmlWriter.WriteAttributeString("ExportPath", this.textBoxDirectory.Text);
            xmlWriter.WriteAttributeString("BCPVersion", _bcpVersion);

            // prepare start parameter for bulk copy tool
            _bcpInfo.WorkingDirectory = textBoxDirectory.Text;

            if (checkBoxSchema.Checked)
            {
                // section for Schema
                this.groupBoxProgress.Text = "Exporting schema";
                this.groupBoxProgress.Update();
                this.progressBar.Maximum = checkedItems;
                this.progressBar.Value = 0;

                xmlWriter.WriteStartElement("Schema");

                foreach (TreeNode item in this.treeViewTables.Nodes)
                {
                    if (item.Checked)
                    {
                        // actualize progress bar
                        this.progressBar.PerformStep();
                        Application.DoEvents();

                        // store table name
                        string table = item.Text.Trim();

                        // create base parameter string for schema file export
                        if (checkBoxXML.Checked)
                        {
                            outFile = table + ".xml";
                            cmdPar = string.Format("[{0}].[dbo].[{1}] format nul -w -x -f\"{2}\"", Settings.ServerConnection.DatabaseName, table, outFile);
                        }
                        else
                        {
                            outFile = table + ".txt";
                            cmdPar = string.Format("[{0}].[dbo].[{1}] format nul -w -f\"{2}\"", Settings.ServerConnection.DatabaseName, table, outFile);
                        }

                        // append parameter for separators
                        if (_colSeparator != null) cmdPar += " -t" + _colSeparator;
                        if (_rowSeparator != null) cmdPar += " -r" + _rowSeparator;

                        // append paramter for daabase access
                        if (Settings.ServerConnection.IsTrustedConnection)
                            cmdPar += string.Format(" -T -S\"{0}\",{1}", Settings.ServerConnection.DatabaseServer, Settings.ServerConnection.DatabaseServerPort);
                        else
                            cmdPar += string.Format(" -S\"{0}\",{1} -U{2} -P{3}",
                                Settings.ServerConnection.DatabaseServer, Settings.ServerConnection.DatabaseServerPort, Settings.ServerConnection.DatabaseUser, Settings.ServerConnection.DatabasePassword);

                        // start bcp.exe
                        _bcpInfo.Arguments = cmdPar;
                        try
                        {
                            using (System.Diagnostics.Process bcp = System.Diagnostics.Process.Start(_bcpInfo))
                            {
                                string output = bcp.StandardOutput.ReadToEnd();
                                if (bcp.WaitForExit(60000))
                                {
                                    if (bcp.ExitCode == 0)
                                        xmlWriter.WriteElementString("File", outFile);
                                    else
                                    {
                                        item.BackColor = DataOk;
                                        item.ToolTipText = errorToolTip;
                                        item.Tag = "\r\nSchema export error:\r\n" + output;
                                        xmlWriter.WriteStartElement("Error");
                                        xmlWriter.WriteAttributeString("Table", table);
                                        if (output != string.Empty) xmlWriter.WriteCData(output);
                                        else xmlWriter.WriteCData("No further information available");
                                        xmlWriter.WriteEndElement(); // Error
                                        errorCount++;
                                    }
                                }
                                else
                                {
                                    item.BackColor = DataOk;
                                    item.ToolTipText = errorToolTip;
                                    item.Tag = "\r\nSchema export error:\r\nTimeout";
                                    xmlWriter.WriteStartElement("Error");
                                    xmlWriter.WriteAttributeString("Table", table);
                                    xmlWriter.WriteCData("Timeout");
                                    xmlWriter.WriteEndElement(); // Error
                                    errorCount++;

                                    if (MessageBox.Show("Timeout during schema export of table " + table + ".\r\n\r\nAbort processing?", "Timeout", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == System.Windows.Forms.DialogResult.Yes)
                                        break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                            item.BackColor = DataOk;
                            item.ToolTipText = errorToolTip;
                            item.Tag = "\r\nSchema export error:\r\n" + ex.Message;
                            xmlWriter.WriteStartElement("Error");
                            xmlWriter.WriteAttributeString("Table", table);
                            xmlWriter.WriteCData(ex.Message);
                            xmlWriter.WriteEndElement(); // Error
                            errorCount++;
                        }
                        this.treeViewTables.Update();
                        Application.DoEvents();
                    }
                }
                xmlWriter.WriteEndElement(); // Schema
            }

            // section for data
            this.groupBoxProgress.Text = "Exporting data";
            this.groupBoxProgress.Update();
            this.progressBar.Maximum = checkedItems;
            this.progressBar.Value = 0;

            xmlWriter.WriteStartElement("Data");
            foreach (TreeNode item in this.treeViewTables.Nodes)
            {
                if (item.Checked)
                {
                    // actualize progress bar
                    this.progressBar.PerformStep();
                    Application.DoEvents();

                    // store table name
                    string table = item.Text.Trim();

                    // create base parameter string for table export
                    outFile = table + ".csv";
                    cmdPar = string.Format("{0}.dbo.{1} out \"{2}\" -w -q", Settings.ServerConnection.DatabaseName, table, outFile);

                    // append parameter for separators
                    if (_colSeparator != null) cmdPar += " -t" + _colSeparator;
                    if (_rowSeparator != null) cmdPar += " -r" + _rowSeparator;

                    // append paramter for daabase access
                    if (Settings.ServerConnection.IsTrustedConnection)
                        cmdPar += string.Format(" -T -S{0},{1}", Settings.ServerConnection.DatabaseServer, Settings.ServerConnection.DatabaseServerPort);
                    else
                        cmdPar += string.Format(" -S{0},{1} -U{2} -P{3}",
                            Settings.ServerConnection.DatabaseServer, Settings.ServerConnection.DatabaseServerPort, Settings.ServerConnection.DatabaseUser, Settings.ServerConnection.DatabasePassword);

                    // start bcp.exe
                    _bcpInfo.Arguments = cmdPar;
                    try
                    {
                        using (System.Diagnostics.Process bcp = System.Diagnostics.Process.Start(_bcpInfo))
                        {
                            string output = bcp.StandardOutput.ReadToEnd();
                            if (bcp.WaitForExit(120000))
                            {
                                if (bcp.ExitCode == 0)
                                {
                                    if (item.BackColor != DataOk)
                                        item.BackColor = AllOk;
                                    xmlWriter.WriteElementString("File", outFile);
                                }
                                else
                                {
                                    item.BackColor = NotOk;
                                    item.ToolTipText = errorToolTip;
                                    item.Tag += "\r\nData export error:\r\n" + output;
                                    xmlWriter.WriteStartElement("Error");
                                    xmlWriter.WriteAttributeString("Table", table);
                                    if (output != string.Empty) xmlWriter.WriteCData(output);
                                    else xmlWriter.WriteCData("No further information available");
                                    xmlWriter.WriteEndElement(); // Error
                                    errorCount++;
                                }
                            }
                            else
                            {
                                item.BackColor = NotOk;
                                item.Tag += "\r\nData export error:\r\nTimeout";
                                xmlWriter.WriteStartElement("Error");
                                xmlWriter.WriteAttributeString("Table", table);
                                xmlWriter.WriteCData("Timeout");
                                xmlWriter.WriteEndElement(); // Error
                                errorCount++;

                                if (MessageBox.Show("Timeout during export of table " + table + ".\r\n\r\nAbort processing?", "Timeout", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == System.Windows.Forms.DialogResult.Yes)
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        item.BackColor = NotOk;
                        item.ToolTipText = errorToolTip;
                        item.Tag += "\r\nData export error:\r\n" + ex.Message;
                        xmlWriter.WriteStartElement("Error");
                        xmlWriter.WriteAttributeString("Table", table);
                        xmlWriter.WriteCData(ex.Message);
                        xmlWriter.WriteEndElement(); // Error
                        errorCount++;
                    }
                    this.treeViewTables.Update();
                    Application.DoEvents();
                }
            }
            xmlWriter.WriteEndElement(); // Data

            // write summary information
            System.DateTime endTime = DateTime.Now;
            xmlWriter.WriteStartElement("Summary");
            xmlWriter.WriteElementString("Start", startTime.ToUniversalTime().ToString());
            //xmlWriter.WriteElementString("StartTime", startTime.ToShortTimeString());
            xmlWriter.WriteElementString("End", endTime.ToUniversalTime().ToString());
            //xmlWriter.WriteElementString("EndTime", endTime.ToShortTimeString());
            xmlWriter.WriteElementString("Errors", errorCount.ToString());
            xmlWriter.WriteEndElement(); // Summary
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();

            // convert xml to html
            this.textBoxResult.Text = xml2html(xmlFile);
            this.Cursor = this.Cursor = System.Windows.Forms.Cursors.Default;
            this.groupBoxResult.Visible = true;
            this.groupBoxProgress.Visible = false;
            this.Update();

            // set tool tip for results
            this.treeViewTables.ShowNodeToolTips = true;

            this.treeViewTables.Enabled = true;
            this.groupBoxOptions.Enabled = true;
        }

        #endregion

        #region Manual

        /// <summary>
        /// Adding event deletates to form and controls
        /// </summary>
        /// <returns></returns>
        private async System.Threading.Tasks.Task InitManual()
        {
            try
            {

                DiversityWorkbench.DwbManual.Hugo manual = new Hugo(this.helpProvider, this);
                if (manual != null)
                {
                    await manual.addKeyDownF1ToForm();
                }
            }
            catch (Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        /// <summary>
        /// ensure that init is only done once
        /// </summary>
        private bool _InitManualDone = false;


        /// <summary>
        /// KeyDown of the form adding event deletates to form and controls within the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Form_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (!_InitManualDone)
                {
                    await this.InitManual();
                    _InitManualDone = true;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        #endregion
    }
}
