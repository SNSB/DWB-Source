using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormLabelEditor : Form
    {
        /*
         * Aus XML das fuer Export erzeugt wird Hierarchie und daraus TreeView generiert
         * damit erhaelt man alle Felder und deren Ansteuerung
         * Diese koennen ausgewaehlt und mit Format versehen werden
         * dazu muss die Funktion im XmlExport entsprechend erweitert werden: 
         * ForLabelEditor bool
         *  falls ja, werden keine Inhalte abgefragt oder geschrieben sondern der Name des Feldes
         *  
         * Zusaetzlich zu den Feldern sollen Templates auswaehlbar sein. 
         * 
         * Falls sich ein include findet, muss auch die eingeschlossene Datei analysisert werden. Diese wird gelesen, aber nicht editiert
         *  
         * Entweder wird eine Schema neu erzeugt oder ein vorhandenes wird editiert. Die Schemata muessen sich dazu an Vorgaben halten
         * 
         * Es wird nur eine Tabelle fuer das Label angelegt in die dann alles eingepasst wird
         * 
         * xsl:variable nur fuer Fonts, der Rest entfaellt
         * 
         * Header entfallen
         * 
         * */

        #region Parameter

        private DiversityCollection.LabelEditor.Label _Label;
        private string _XmlFile;
        private System.Collections.Generic.List<DiversityCollection.LabelEditor.UserControlLabelColumn> _ColumnControls;
        private System.Collections.Generic.List<DiversityCollection.LabelEditor.UserControlLabelRow> _RowControls;
        
        #endregion

        #region Public properties

        public System.Collections.Generic.List<DiversityCollection.LabelEditor.UserControlLabelColumn> ColumnControls
        {
            get
            {
                if (this._ColumnControls == null)
                    this._ColumnControls = new List<LabelEditor.UserControlLabelColumn>();
                return _ColumnControls;
            }
        }

        public System.Collections.Generic.List<DiversityCollection.LabelEditor.UserControlLabelRow> RowControls
        {
            get
            {
                if (this._RowControls == null)
                    this._RowControls = new List<LabelEditor.UserControlLabelRow>();
                return _RowControls;
            }
        }

        public DiversityCollection.LabelEditor.Label Label
        {
            get
            {
                if (this._Label == null) 
                    this._Label = new LabelEditor.Label();
                return this._Label;
            }
        }
        
        #endregion

        #region Construction

        public FormLabelEditor(System.Collections.Generic.List<int> IDs)
        {
            InitializeComponent();
        }

        /// <summary>
        /// Editing the schemas for xml files
        /// </summary>
        /// <param name="XMLfile">The path and name of an xml file for processing with the schemata</param>
        public FormLabelEditor(string XMLfile)
        {
            InitializeComponent();
            this._XmlFile = XMLfile;
        }

        #endregion

        #region XSLT-File analysis
        
        private void toolStripButtonOpenSchemaFile_Click(object sender, EventArgs e)
        {
            string Path = System.Windows.Forms.Application.StartupPath + "\\LabelPrinting\\Schemas";
            if (!System.IO.File.Exists(Path))
            {
                System.IO.Directory.CreateDirectory(Path);
            }
            if (this.textBoxSchemaFile.Text.Length > 0)
            {
                try
                {
                    System.IO.FileInfo FI = new System.IO.FileInfo(this.textBoxSchemaFile.Text);
                    if (FI.Exists)
                        Path = FI.DirectoryName;
                }
                catch { }
            }
            this.openFileDialogLabelSchema = new OpenFileDialog();
            this.openFileDialogLabelSchema.RestoreDirectory = true;
            this.openFileDialogLabelSchema.Multiselect = false;
            this.openFileDialogLabelSchema.InitialDirectory = Path;
            this.openFileDialogLabelSchema.Filter = "XSLT Files|*.xslt";
            try
            {
                this.openFileDialogLabelSchema.ShowDialog();
                if (this.openFileDialogLabelSchema.FileName.Length > 0)
                {
                    this.textBoxSchemaFile.Tag = this.openFileDialogLabelSchema.FileName;
                    System.IO.FileInfo f = new System.IO.FileInfo(this.openFileDialogLabelSchema.FileName);
                    this.textBoxSchemaFile.Text = f.FullName;
                    this._Label = null;
                    this.Label.AnalyseXSLTfile(f.FullName);
                    this.DrawLabelColumnsAndRows();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        //private void AnalyseXSLTfile()
        //{
        //    if (this.textBoxSchemaFile.Text.Length == 0)
        //        return;
        //    System.Xml.XmlTextReader R = new System.Xml.XmlTextReader(this.textBoxSchemaFile.Text);
        //    try
        //    {
        //        while (R.Read())
        //        {
        //            if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
        //                continue;
        //            if (R.Name == "ImportSchedule"
        //                || R.Name == "xml"
        //                || R.Name == "xsl:stylesheet"
        //                || R.Name == "xsl:output")
        //                continue;
        //            if (R.Name == "xsl:include")
        //            {
        //                this.ReadInclude(ref R);
        //            }
        //            else if (R.NodeType == System.Xml.XmlNodeType.Element && R.Name == "xsl:variable")
        //            {
        //                this.ReadVariable(ref R);
        //            }
        //            else if (R.NodeType == System.Xml.XmlNodeType.Element && R.Name == "xsl:template")
        //            {
        //                this.ReadXslTemplate(ref R);
        //            }
        //            else
        //            {
                                
        //            }
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //    }
        //    finally
        //    {
        //        R.ResetState();
        //        R.Close();
        //    }
        //}

        //private void ReadVariable(ref System.Xml.XmlTextReader R)
        //{
        //    try
        //    {
        //        string VarialbleName = R.GetAttribute(0);
        //        R.Read();
        //        string[] AttributList = R.Value.ToString().Split(new char[] { ';' });
        //        System.Collections.Generic.List<string> Att = new List<string>();
        //        foreach (string s in AttributList)
        //        {
        //            Att.Add(s.Trim());
        //        }
        //        this.Label.XslVariables.Add(VarialbleName, Att);
        //        return;
        //        //while (R.Read())
        //        //{
        //        //    if (R.NodeType == System.Xml.XmlNodeType.EndElement)
        //        //        return;
        //        //}
        //    }
        //    catch (System.Exception ex) { }
        //}

        //private void ReadXslTemplate(ref System.Xml.XmlTextReader R)
        //{
        //    try
        //    {
        //        string TemplateName = R.GetAttribute(0);
        //        System.Collections.Generic.Dictionary<string, string> Att = new Dictionary<string, string>();
        //        for (int i = 0; i > R.AttributeCount; i++)
        //        {
        //            R.MoveToAttribute(i);
        //            Att.Add(R.Name, R.Value);
        //            if (R.Name == "name" || R.Name == "match")
        //                TemplateName = R.Name;
        //        }
        //        DiversityCollection.LabelEditor.LabelXslTemplate T = new LabelEditor.LabelXslTemplate(Att);
        //        this.Label.XslTemplates.Add(TemplateName, T);
        //        while (R.Read())
        //        {
        //            if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
        //                continue;
        //            if (R.NodeType == System.Xml.XmlNodeType.EndElement && R.Name == "xsl:template")
        //                return;
        //            if (R.NodeType == System.Xml.XmlNodeType.Element)
        //                this.ReadXslNode(ref R, ref T);
        //        }
        //    }
        //    catch (System.Exception ex) { }
        //}

        //private void ReadXslNode(ref System.Xml.XmlTextReader R, ref DiversityCollection.LabelEditor.LabelXslTemplate XslTemplate)
        //{
        //    try
        //    {
        //        System.Collections.Generic.List<string> Att = new List<string>();
        //        for (int i = 0; i < R.AttributeCount; i++)
        //        {
        //            Att.Add(R.GetAttribute(i));
        //        }
        //        if (R.AttributeCount > 0)
        //        {
        //            int Width = int.Parse(R.GetAttribute(0).ToString());
        //            DiversityCollection.LabelEditor.LabelColumn LC = new LabelEditor.LabelColumn(Width);
        //            this.Label.LabelColums.Add(LC);
        //        }
        //        while (R.Read())
        //        {
        //            if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
        //                continue;
        //            if (R.Name == "th" && R.NodeType == System.Xml.XmlNodeType.Element)
        //            {
        //                Width = int.Parse(R.GetAttribute(0).ToString());
        //                DiversityCollection.LabelEditor.LabelColumn C = new LabelEditor.LabelColumn(Width);
        //                this.Label.LabelColums.Add(C);
        //            }
        //            if (R.Name == "td" && R.NodeType == System.Xml.XmlNodeType.Element)
        //                this.ReadTableData(ref R);
        //            if (R.Name == "tr" && R.NodeType == System.Xml.XmlNodeType.EndElement)
        //                return;
        //        }
        //    }
        //    catch (System.Exception ex) { }
        //}


        //private void ReadTemplateLabel(ref System.Xml.XmlTextReader R)
        //{
        //    try
        //    {
        //        string TemplateName = R.GetAttribute(0);
        //        //string InnerXML = R.ReadInnerXml();
        //        if (!this.Label.XslTemplates.ContainsKey(TemplateName))
        //            this.Label.XslTemplates.Add(TemplateName, "");
        //        while (R.Read())
        //        {
        //            if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
        //                continue;
        //            if (R.Name == "template" && R.NodeType == System.Xml.XmlNodeType.EndElement)
        //                return;
        //            if (R.Name == "table" && R.NodeType == System.Xml.XmlNodeType.Element)
        //            {
        //                //System.Collections.Generic.List<string> Att = new List<string>();
        //                System.Collections.Generic.Dictionary<string, string> Attributes = new Dictionary<string,string>();
        //                for (int i = 0; i < R.AttributeCount; i++)
        //                {
        //                    //Att.Add(R.GetAttribute(i));
        //                    R.MoveToAttribute(i);
        //                    Attributes.Add(R.Name, R.Value);
        //                }
        //            }
        //            if (R.Name == "tr" && R.NodeType == System.Xml.XmlNodeType.Element)
        //            {
        //                this.ReadTableRow(ref R);
        //            }
        //        }
        //    }
        //    catch (System.Exception ex) { }
        //}

        //private void ReadTableRow(ref System.Xml.XmlTextReader R)
        //{
        //    try
        //    {
        //        System.Collections.Generic.List<string> Att = new List<string>();
        //        for (int i = 0; i < R.AttributeCount; i++)
        //        {
        //            Att.Add(R.GetAttribute(i));
        //        }
        //        if (R.AttributeCount > 0)
        //        {
        //            int Width = int.Parse(R.GetAttribute(0).ToString());
        //            DiversityCollection.LabelEditor.LabelColumn LC = new LabelEditor.LabelColumn(Width);
        //            this.Label.LabelColums.Add(LC);
        //        }
        //        while (R.Read())
        //        {
        //            if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
        //                continue;
        //            if (R.Name == "th" && R.NodeType == System.Xml.XmlNodeType.Element)
        //            {
        //                Width = int.Parse(R.GetAttribute(0).ToString());
        //                DiversityCollection.LabelEditor.LabelColumn C = new LabelEditor.LabelColumn(Width);
        //                this.Label.LabelColums.Add(C);
        //            }
        //            if (R.Name == "td" && R.NodeType == System.Xml.XmlNodeType.Element)
        //                this.ReadTableData(ref R);
        //            if (R.Name == "tr" && R.NodeType == System.Xml.XmlNodeType.EndElement)
        //                return;
        //        }
        //    }
        //    catch (System.Exception ex) { }
        //}



        //private void ReadTableColumns(ref System.Xml.XmlTextReader R)
        //{
        //    try
        //    {
        //        int Width = int.Parse(R.GetAttribute(0).ToString());
        //        DiversityCollection.LabelEditor.LabelColumn LC = new LabelEditor.LabelColumn(Width);
        //        this.Label.LabelColums.Add(LC);
        //        while (R.Read())
        //        {
        //            if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
        //                continue;
        //            if (R.Name == "th" && R.NodeType == System.Xml.XmlNodeType.Element)
        //            {
        //                Width = int.Parse(R.GetAttribute(0).ToString());
        //                DiversityCollection.LabelEditor.LabelColumn C = new LabelEditor.LabelColumn(Width);
        //                this.Label.LabelColums.Add(C);
        //            }
        //            if (R.Name == "tr" && R.NodeType == System.Xml.XmlNodeType.EndElement)
        //                return;
        //        }
        //    }
        //    catch (System.Exception ex) { }
        //}

        //private void ReadTableData(ref System.Xml.XmlTextReader R)
        //{
        //    try
        //    {
        //        int Width = int.Parse(R.GetAttribute(0).ToString());
        //        DiversityCollection.LabelEditor.LabelColumn LC = new LabelEditor.LabelColumn(Width);
        //        this.Label.LabelColums.Add(LC);
        //        while (R.Read())
        //        {
        //            if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
        //                continue;
        //            if (R.Name == "th" && R.NodeType == System.Xml.XmlNodeType.Element)
        //            {
        //                Width = int.Parse(R.GetAttribute(0).ToString());
        //                DiversityCollection.LabelEditor.LabelColumn C = new LabelEditor.LabelColumn(Width);
        //                this.Label.LabelColums.Add(C);
        //            }
        //            if (R.Name == "tr" && R.NodeType == System.Xml.XmlNodeType.EndElement)
        //                return;
        //        }
        //    }
        //    catch (System.Exception ex) { }
        //}


        //private void ReadInclude(ref System.Xml.XmlTextReader R)
        //{
        //    try
        //    {
        //        string IncludedFile = R.GetAttribute(0);
        //        this.Label.XslIncludes.Add(IncludedFile);
        //        return;
        //        //while (R.Read())
        //        //{
        //        //    if (R.NodeType == System.Xml.XmlNodeType.EndElement)
        //        //        return;
        //        //}
        //    }
        //    catch (System.Exception ex) { }
        //}

        #endregion

        #region Rows & Columns

        private int LabelWidth
        {
            get
            {
                return this.Label.LabelWidth;
            }
        }

        private int LabelHeight
        {
            get
            {
                return this.Label.LabelHeight;
            }
        }

        private void toolStripButtonRowAdd_Click(object sender, EventArgs e)
        {
            DiversityCollection.LabelEditor.LabelRow LR = new LabelEditor.LabelRow(20);
            this.Label.LabelRows.Add(LR);
            this.DrawLabelColumnsAndRows();
            //DiversityCollection.LabelEditor.UserControlLabelRow U = new LabelEditor.UserControlLabelRow(ref LR);
            //U.buttonOptions.Click += new System.EventHandler(this.setRowOptions);
            //this.panelRows.Controls.Add(U);
            //this.RowControls.Add(U);
            //U.Dock = DockStyle.Top;
        }

        private void toolStripButtonColumnAdd_Click(object sender, EventArgs e)
        {
            DiversityCollection.LabelEditor.LabelColumn LC = new LabelEditor.LabelColumn(20);
            this.Label.LabelColums.Add(LC);
            this.DrawLabelColumnsAndRows();
            //DiversityCollection.LabelEditor.UserControlLabelColumn U = new LabelEditor.UserControlLabelColumn(ref LC);
            //this.panelColumns.Controls.Add(U);
            //this.ColumnControls.Add(U);
            //U.Dock = DockStyle.Left;
        }

        private void toolStripButtonColumnRemove_Click(object sender, EventArgs e)
        {
            this.panelColumns.Controls.Remove(this.ColumnControls[0]);
            this.ColumnControls.RemoveAt(0);
            this.Label.LabelColums.RemoveAt(0);
            this.DrawLabelColumnsAndRows();
        }

        private void toolStripButtonRowRemove_Click(object sender, EventArgs e)
        {
            this.panelRows.Controls.Remove(this.RowControls[0]);
            this.RowControls.RemoveAt(0);
            this.Label.LabelRows.RemoveAt(0);
            this.DrawLabelColumnsAndRows();
        }

        private void DrawLabelColumnsAndRows()
        {
            this.tableLayoutPanelRowColumnEditor.Controls.Clear();

            this.tableLayoutPanelRowColumnEditor.ColumnStyles.Clear();
            this.tableLayoutPanelRowColumnEditor.ColumnCount = this.Label.ColumnCount;
            this.tableLayoutPanelRowColumnEditor.Width = this.LabelWidth;
            this.panelColumns.Controls.Clear();
            for (int i = 0; i < this.Label.LabelColums.Count; i++)
            {
                System.Single S = (System.Single)this.Label.LabelColums[i].ColumnWidth;
                this.tableLayoutPanelRowColumnEditor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, S));
            } 
            for (int i = this.Label.LabelColums.Count - 1; i > -1; i--)
            {
                DiversityCollection.LabelEditor.LabelColumn LC = this.Label.LabelColums[i];
                DiversityCollection.LabelEditor.UserControlLabelColumn U = new LabelEditor.UserControlLabelColumn(ref LC);
                this.panelColumns.Controls.Add(U);
                this.ColumnControls.Add(U);
                U.Dock = DockStyle.Left;
            }

            this.tableLayoutPanelRowColumnEditor.RowStyles.Clear();
            this.tableLayoutPanelRowColumnEditor.RowCount = this.Label.RowCount;
            this.tableLayoutPanelRowColumnEditor.Height = this.LabelHeight;
            this.panelRows.Controls.Clear();
            for (int i = 0; i < this.Label.LabelRows.Count;  i++)
            {
                System.Single S = (System.Single)this.Label.LabelRows[i].RowHeight;
                this.tableLayoutPanelRowColumnEditor.RowStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, S));
            }
            int Offset = 0;
            for (int i = this.Label.LabelRows.Count - 1; i > -1; i--)
            {
                System.Single S = (System.Single)this.Label.LabelRows[i].RowHeight;
                this.tableLayoutPanelRowColumnEditor.RowStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, S));
                DiversityCollection.LabelEditor.LabelRow LR = this.Label.LabelRows[i];
                DiversityCollection.LabelEditor.UserControlLabelRow U = new LabelEditor.UserControlLabelRow(ref LR);
                U.buttonOptions.Click += new System.EventHandler(this.setRowOptions);
                this.panelRows.Controls.Add(U);
                this.RowControls.Add(U);
                U.Dock = DockStyle.Top;
                if (LR.RowHeight < U.MinimumSize.Height)
                    Offset += U.MinimumSize.Height - LR.RowHeight;
            }
            //if (Offset > 0)
            {
                System.Windows.Forms.Padding P = new Padding(0, Offset, 0, 0);
                this.tableLayoutPanelRowColumnEditor.Margin = P;
            }
        }

        private void setRowOptions(object sender, EventArgs e)
        {
            System.Windows.Forms.Button B = (System.Windows.Forms.Button)sender;
            System.Windows.Forms.Panel P = (System.Windows.Forms.Panel)B.Parent;
            DiversityCollection.LabelEditor.UserControlLabelRow U = (DiversityCollection.LabelEditor.UserControlLabelRow)P.Parent;
            this.tabControlDesigner.SelectedTab = this.tabPageRows;
            this.labelRowOptionsAttributes.Text = U.RowHeight.ToString();
        }

        #endregion

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            this.Label.AnalyseXSLTfile(this.openFileDialogLabelSchema.FileName);
            this.DrawLabelColumnsAndRows();
        }

    }
}
