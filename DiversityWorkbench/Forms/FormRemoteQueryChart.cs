using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace DiversityWorkbench.Forms
{
    // Must inherit Control, not Component, in order to have Handle
    [DefaultEvent("ClipboardChanged")]

    public partial class FormRemoteQueryChart : Form
    {
        #region Parameter

        private DiversityWorkbench.Chart _Chart;

        /// <summary>
        /// List of files that will be deleted when form is closed
        /// </summary>
        private System.Collections.Generic.List<string> _CreatedHtmlFiles;

        #endregion

        #region Construction

        public FormRemoteQueryChart(DiversityWorkbench.Chart Chart)//, string BaseURL, string Header = "")
        {
            if (Chart == null)
            {
                System.Windows.Forms.MessageBox.Show("Failed to create chart");
                this.Close();
                return;
            }
            InitializeComponent();
            this.labelHeader.Text = "Please select an item from ";
            if (Chart.Title().Length > 0)
                this.labelHeader.Text += Chart.Title();
            else
                this.labelHeader.Text += "the chart";
            this._Chart = Chart;
            this.Width = this._Chart.WindowWidth() - 4;
            this.Height = this._Chart.WindowHeight() - 4;
            //this._BaseURL = BaseURL;
#if xDEBUG
            this.initHtmlGrid();
#else
            this.initHtmlTable();
#endif
        }

        #endregion

        #region Form
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            this.dataGridView.Rows.Clear();
            this.initHtmlTable();
        }


        /// <summary>
        /// returning the content of the clipboard and delete created html files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormRemoteQueryChart_FormClosing(object sender, FormClosingEventArgs e)
        {
            Object returnObject = null;
            if (Clipboard.ContainsText())
            {
                returnObject = System.Windows.Forms.Clipboard.GetText();
                int i;
                if (int.TryParse(returnObject.ToString(), out i))
                    this._SeletedID = i;
            }
            if (this._CreatedHtmlFiles != null)
            {
                foreach (string F in this._CreatedHtmlFiles)
                {
                    try
                    {
                        System.IO.FileInfo f = new System.IO.FileInfo(F);
                        if (f.Exists)
                            f.Delete();
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
            }
        }

        #endregion

        #region public interface

        public string SelectedURL()
        {
            if (this._SeletedID != null && this._Chart.BaseURL.Length > 0)// this._BaseURL != null)
                return this._Chart.BaseURL + this._SeletedID.ToString();
            else
                return "";
        }

        private int? _SeletedID;
        public int? SelectedID()
        {
            return this._SeletedID;
        }

        //public System.Collections.Generic.Dictionary<string, string> UnitValues()
        //{
        //    System.Collections.Generic.Dictionary<string, string> Dict = new Dictionary<string, string>();
        //    if (SelectedURL().Length > 0)
        //    {
        //        DiversityWorkbench.ServerConnection SC = DiversityWorkbench.WorkbenchUnit.getServerConnectionFromURI(SelectedURL());
        //        DiversityWorkbench.IWorkbenchUnit workbenchUnit = null;
        //        switch(SC.ModuleName)
        //        {
        //            case "DiversityTaxonNames":
        //                DiversityWorkbench.TaxonName taxonName = new TaxonName(SC);
        //                workbenchUnit = taxonName;
        //                break;
        //            case "DiversityScientificTerms":
        //                DiversityWorkbench.ScientificTerm scientificTerm = new ScientificTerm(SC);
        //                workbenchUnit = scientificTerm;
        //                break;
        //        }
        //        if (workbenchUnit != null)
        //        {
        //            int ID;
        //            if (int.TryParse(DiversityWorkbench.WorkbenchUnit.getIDFromURI(SelectedURL()), out ID))
        //            {
        //                Dict = workbenchUnit.UnitValues(ID);
        //            }
        //        }
        //    }
        //    return Dict;
        //}

        //public string DisplayText
        //{
        //    get
        //    {
        //        // Toni 20180821: Supply empty string, not null (-> Return might cause exception at caller site)
        //        if (this._DisplayText == null)
        //            this._DisplayText = "";
        //        if (this._DatabaseService.IsWebservice && this.treeViewUnit.Nodes.Count > 0)
        //        {
        //            string Display = this.treeViewUnit.Nodes[0].Text.Trim();
        //            if (Display.Length > 0 && (this._DisplayText == null || Display.Length > this._DisplayText.Trim().Length && Display.StartsWith(this._DisplayText)))
        //                this._DisplayText = Display;
        //        }
        //        else if (this._DisplayText.Length == 0 && !this._DatabaseService.IsWebservice) // Toni 20180821: Read query list only if database was called
        //        {
        //            try
        //            {
        //                System.Data.DataRowView R = (System.Data.DataRowView)this.userControlQueryList.listBoxQueryResult.SelectedItem;
        //                this._DisplayText = R["Display"].ToString();
        //            }
        //            catch (System.Exception ex) { }
        //        }
        //        return this._DisplayText.Trim();
        //    }
        //}

        #endregion

        #region HTML

        #region Clipboard

        private IntPtr _nextClipboardViewer;
        private void getClipboard()
        {
        }

        private void MainClipboardMonitor_ClipboardChanged(object sender, ClipboardChangedEventArgs e)
        {
            string clipboardText = string.Empty;

            try
            {
                clipboardText = e.DataObject.GetData(System.Windows.Forms.DataFormats.UnicodeText).ToString();
            }
            catch (Exception ex)
            {
                clipboardText = "Exception Occured while capturing clipboard: " + ex.ToString();
            }
            finally
            {
                //WriteToLog(clipboardText);
            }
        }

        private event EventHandler<ClipboardChangedEventArgs> ClipboardChanged;

        #endregion

        /*
         * https://github.com/jn2guru/Damn-Simple-Clipboard-Capture/blob/master/DamnSimpleClipboardCapture/ClipboardMonitor.cs
         * */

        private string Html5FileName(System.Collections.Generic.List<int> ParentKeys, bool ForBacklink = false)
        {
            string FileName = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Query) + DiversityWorkbench.Settings.DatabaseName + "_Chart";
            if (this._Chart.IsSection) FileName += "Section";
            //else
            {
                FileName += "Select";
                if (ParentKeys != null)
                {
                    if (ForBacklink)
                    {
                        if (ParentKeys.Count > 2)
                        {
                            FileName += "_" + ParentKeys[ParentKeys.Count - 3].ToString();
                            if (ParentKeys.Count > 1)
                                FileName += "_" + ParentKeys[ParentKeys.Count - 2].ToString();
                        }
                    }
                    else
                    {
                        if (ParentKeys.Count > 1)
                            FileName += "_" + ParentKeys[ParentKeys.Count - 2].ToString();
                        if (ParentKeys.Count > 0)
                            FileName += "_" + ParentKeys[ParentKeys.Count - 1].ToString();
                    }
                }
            }
            FileName += ".html";
            if (this._CreatedHtmlFiles == null)
                this._CreatedHtmlFiles = new List<string>();
            this._CreatedHtmlFiles.Add(FileName);
            return FileName;
        }


        #region Table

        private string initHtmlTable(System.Collections.Generic.List<int> ParentKeys = null)
        {
            string FileName = "";
            try
            {
                FileName = this.Html5FileName(ParentKeys);

                System.Text.StringBuilder builder = new StringBuilder();
                System.IO.StringWriter stringWriter = new System.IO.StringWriter(builder);
                using (System.Web.UI.HtmlTextWriter HWriter = new System.Web.UI.HtmlTextWriter(stringWriter))
                {
                    HWriter.WriteLine("<!DOCTYPE html>");
                    HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Html);
                    HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Head);
                    HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Style);
                    HWriter.WriteLine("table, th, td { border: 0px solid black; border-collapse: collapse; font-family: Arial; text-align: left; } ");
                    HWriter.WriteLine("th, td {padding: 5px; vertical-align: top;}");
                    HWriter.WriteLine("td:hover {border: 3px solid black;}");
                    HWriter.WriteLine("div.left {float: left; clear: both;}");
                    HWriter.RenderEndTag(); // Style
                    HWriter.RenderEndTag(); // Head
                    HWriter.WriteLine();
                    HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Body);

                    // Title
                    HWriter.AddStyleAttribute("font-family", "Arial");
                    HWriter.AddStyleAttribute("font-weight", "bold");
                    HWriter.AddStyleAttribute("font-size", "20px");
                    HWriter.AddStyleAttribute("text-decoration", "underline");
                    HWriter.RenderBeginTag("span");
                    HWriter.WriteEncodedText(this._Chart.Title());
                    if (this._Chart.Section().Length > 0)
                        HWriter.WriteEncodedText(" - " + this._Chart.Section());
                    HWriter.RenderEndTag();

                    // Table
                    HWriter.AddStyleAttribute("width", "100%");
                    HWriter.AddStyleAttribute("margin", "0px");
                    HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Table);
                    // Table head
                    this.Html5writeTableHeader(HWriter);
                    // Table body
                    HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Tbody);
                    HWriter.WriteLine();
                    // data header
                    this.Html5writeTableTop(HWriter, ParentKeys);
                    if (ParentKeys != null && ParentKeys.Count > 0)
                    {
                        int Key = ParentKeys.Last();
                        if (this._Chart.getChartValue(Key).ChildrenIDs != null)
                        {
                            for (int i = 0; i < this._Chart.getChartValue(Key).ChildrenIDs.Count; i++)
                            {
                                ChartValue cCV = this._Chart.getChartValue(this._Chart.getChartValue(Key).ChildrenIDs[i]);
                                this.Html5writeTableRow(HWriter, ParentKeys, cCV, true);
                                //this.Html5writeTableRow(HWriter, cCV, ParentKeys, true);
                            }
                        }
                        HWriter.WriteLine();
                    }
                    else
                    {
                        foreach (System.Collections.Generic.KeyValuePair<int, ChartValue> CV in this._Chart.BaseChartValues())
                        {
                            this.Html5writeTableRow(HWriter, ParentKeys, CV.Value, true);
                            //this.Html5writeTableRow(HWriter, CV.Value, ParentKeys, true);
                            HWriter.WriteLine();
                        }
                    }
                    HWriter.RenderEndTag(); // Tbody
                    HWriter.RenderEndTag(); // Table
                    HWriter.WriteLine();

                    // Skript
                    this.Html5writeFunction(HWriter);

                    HWriter.WriteLine();
                    HWriter.RenderEndTag(); // Body
                    HWriter.RenderEndTag(); // Html
                }

                System.IO.File.WriteAllText(FileName, stringWriter.ToString());
                System.Uri uTest = new Uri(FileName);
                this.webBrowser.Url = uTest;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            return FileName;
        }


        private void Html5writeTableHeader(System.Web.UI.HtmlTextWriter HWriter)
        {
            try
            {
                HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Thead);
                HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Tr);
                int iColCount = this._Chart.MaxColumnCount;

                for (int i = 0; i < this._Chart.MaxColumnsPerPage(); i++)
                {
                    HWriter.AddAttribute("visible", "false");
                    HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Th);
                    HWriter.RenderEndTag(); // Th
                    HWriter.WriteLine();
                }
                HWriter.RenderEndTag(); // Tr
                HWriter.WriteLine();
                HWriter.RenderEndTag(); // Thead
                HWriter.WriteLine();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void Html5writeTableTop(System.Web.UI.HtmlTextWriter HWriter, System.Collections.Generic.List<int> ParentKeys)
        {
            try
            {
                //HWriter.WriteLine();
                HWriter.AddAttribute("visible", "false");
                HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Tr);
                HWriter.WriteLine();
                HWriter.AddAttribute("colspan", this._Chart.MaxColumnCount.ToString());
                HWriter.AddAttribute("align", "right");
                HWriter.AddStyleAttribute("border-color", "white");
                HWriter.AddStyleAttribute("border-width", "0px");
                HWriter.AddAttribute("visibility", "hidden");
                HWriter.AddStyleAttribute("font-size", "x-small");
                HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Td);
                HWriter.WriteLine();
                HWriter.Write("ID: ");
                HWriter.AddAttribute("id", "ID");
                HWriter.AddAttribute("readonly", "readonly");
                HWriter.AddStyleAttribute("font-size", "xx-small");
                HWriter.AddStyleAttribute("height", "10px");
                HWriter.AddStyleAttribute("width", "60px");
                HWriter.AddStyleAttribute("text-align", "left");
                HWriter.AddStyleAttribute("border-color", "silver");
                HWriter.AddStyleAttribute("border-width", "1px");
                HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Textarea);
                HWriter.WriteLine();
                HWriter.RenderEndTag(); // Textarea
                HWriter.WriteLine();
                HWriter.RenderEndTag();//Td
                HWriter.WriteLine();
                HWriter.RenderEndTag();//Tr
                HWriter.WriteLine();

                if (ParentKeys != null && ParentKeys.Count > 0)
                {
                    // Backlink
                    int ParentID = -1;
                    if (ParentKeys.Count > 1)
                        ParentID = ParentKeys[ParentKeys.Count - 2];
                    else
                        ParentID = ParentKeys[0];
                    ChartValue bCV = this._Chart.getChartValue(ParentID);// ParentKeys.First());// this._Chart.getChartGroupBase(ParentKeys.First());
                    HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Tr); // Row
                    HWriter.WriteLine();
                    HWriter.AddStyleAttribute("border-width", "0px");
                    HWriter.AddStyleAttribute("border-color", "white");
                    HWriter.AddStyleAttribute("font-weight", "bold");
                    HWriter.AddAttribute("colspan", this._Chart.MaxColumnCount.ToString());
                    HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Td);
                    HWriter.WriteLine();
                    HWriter.AddStyleAttribute("font-size", "20px");
                    HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Span);
                    HWriter.WriteLine();
                    HWriter.Write("&larr;&nbsp;");
                    HWriter.WriteLine();
                    HWriter.RenderEndTag();// Span
                    string ParentWebsite = this.Html5FileName(ParentKeys, true);
                    HWriter.AddAttribute("href", ParentWebsite);
                    HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.A);
                    if (ParentKeys.Count > 1)
                        HWriter.WriteEncodedText(bCV.DisplayText);// "< " + "");// pCV.DisplayText);
                    else
                        HWriter.WriteEncodedText(this._Chart.Title());
                    HWriter.WriteLine();
                    HWriter.RenderEndTag();// A
                    HWriter.WriteLine();
                    HWriter.RenderEndTag();// Td
                    HWriter.WriteLine();
                    HWriter.RenderEndTag(); // Row
                    HWriter.WriteLine();

                    // Parent value
                    HWriter.WriteLine();
                    HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Tr); // Row
                    ChartValue pCV = this._Chart.getChartValue(ParentKeys.Last());
                    this.Html5writeTableField(HWriter, pCV, 1, ParentKeys, true);
                    HWriter.WriteLine();
                    HWriter.RenderEndTag(); // Row

                    // empty row underneath header
                    HWriter.AddStyleAttribute("height", "6px");
                    HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Tr); // Row
                    HWriter.AddStyleAttribute("border-width", "0px");
                    HWriter.AddStyleAttribute("border-color", "white");
                    HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Td);
                    HWriter.RenderEndTag();// Td
                    HWriter.RenderEndTag(); // Row
                }
                //HWriter.WriteLine();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        private void Html5writeTableRow(System.Web.UI.HtmlTextWriter HWriter, ChartValue CV,
            System.Collections.Generic.List<int> ParentKeys, bool NewRow = false)
        {
            System.Collections.Generic.List<ChartValue> NextRows = new List<ChartValue>();
            try
            {
                if (NewRow)
                    HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Tr);
                int Position = CV.ColumnPosition;
                if (Position >= this._Chart.MaxColumnCount)
                    Position = Position % this._Chart.MaxColumnCount;

                for (int i = 0; i < this._Chart.MaxColumnCount; i++)
                {
                    if (i > Position + 1)
                        continue;
                    if (Position < i)
                    {
                        bool IsGroup = this._Chart.Groups.Contains(CV.ID); // CV.IsGroup;
                        if (CV.ChildrenIDs != null && CV.ChildrenIDs.Count > 0 && !IsGroup)
                        {
                            for (int c = 0; c < CV.ChildrenIDs.Count; c++)
                            {
                                ChartValue cv = this._Chart.getChartValue(CV.ChildrenIDs[c]);
                                if (c == 0)
                                {
                                    this.Html5writeTableRow(HWriter, cv, ParentKeys, false);
                                    if (NewRow)
                                    {
                                        continue;
                                    }
                                }
                                else
                                {
                                    NextRows.Add(cv);
                                }
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (Position == i)
                    {
                        this.Html5writeTableField(HWriter, CV, i, ParentKeys);
                    }
                }
                if (NewRow)
                {
                    HWriter.RenderEndTag(); // Tr
                    HWriter.WriteLine();
                }
                foreach (ChartValue NextRow in NextRows)
                {
                    this.Html5writeTableRow(HWriter, NextRow, ParentKeys, true);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        private System.Collections.Generic.List<ChartValue> Html5writeTableRow(
            System.Web.UI.HtmlTextWriter HWriter,
            System.Collections.Generic.List<int> ParentKeys,
            ChartValue CV,
            bool NewRow)
        {
            System.Collections.Generic.List<ChartValue> NextRows = new List<ChartValue>();
            try
            {
                if (NewRow)
                    HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Tr);
                int Position = CV.ColumnPosition;
                if (Position >= this._Chart.MaxColumnCount)
                    Position = Position % this._Chart.MaxColumnCount;

                for (int i = 0; i < this._Chart.MaxColumnCount; i++)
                {
                    if (i > Position + 1)
                        continue;
                    if (Position < i)
                    {
                        if (CV.DisplayText == null)
                            continue;
                        bool IsGroup = this._Chart.Groups.Contains(CV.ID); // CV.IsGroup;
                        if (CV.ChildrenIDs != null && CV.ChildrenIDs.Count > 0 && !IsGroup)
                        {
                            for (int c = 0; c < CV.ChildrenIDs.Count; c++)
                            {
                                ChartValue cv = this._Chart.getChartValue(CV.ChildrenIDs[c]);
                                if (c == 0)
                                {
                                    System.Collections.Generic.List<ChartValue> nn = this.Html5writeTableRow(HWriter, ParentKeys, cv, false);
                                    foreach (ChartValue n in nn)
                                        NextRows.Add(n);
                                    if (NewRow)
                                    {
                                        continue;
                                    }
                                    else
                                    {

                                    }
                                }
                                else
                                {
                                    NextRows.Add(cv);
                                }
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (Position == i)
                    {
                        this.Html5writeTableField(HWriter, CV, i, ParentKeys);
                    }
                }
                if (NewRow)
                {
                    HWriter.RenderEndTag(); // Tr
                    HWriter.WriteLine();
                    foreach (ChartValue NextRow in NextRows)
                    {
                        this.Html5writeTableRow(HWriter, ParentKeys, NextRow, true);
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return NextRows;
        }


        private void Html5writeTableField(System.Web.UI.HtmlTextWriter HWriter, ChartValue CV, int ColumnPosition,
            System.Collections.Generic.List<int> ParentKeys, bool IsHeader = false)
        {
            try
            {
                if (CV.DisplayText == null || CV.ID == 0)
                    return;
                int Size = 20;
                if (ColumnPosition == 0)
                    Size = 20;
                else if (ColumnPosition == 1)
                    Size = 18;
                else if (ColumnPosition == 2)
                    Size = 16;
                else if (ColumnPosition < 6)
                    Size = 16 - ColumnPosition;
                else
                    Size = 10;
                bool IsGroup = this._Chart.Groups.Contains(CV.ID);//false;
                HWriter.WriteLine();
                if (IsHeader)
                    HWriter.AddAttribute("colspan", (this._Chart.MaxColumnCount.ToString()));//.MaxColumnCountGroup(CV.ID)).ToString());
                else if (CV.ChildrenIDs == null && ColumnPosition < this._Chart.MaxColumnCount - 1)
                    HWriter.AddAttribute("colspan", (this._Chart.MaxColumnCount - ColumnPosition).ToString());
                else
                {
                    if ((CV.ChildrenIDs == null || CV.ChildrenIDs.Count == 0) && ColumnPosition < this._Chart.MaxColumnCount - 1)
                        HWriter.AddAttribute("colspan", (this._Chart.MaxColumnCount - ColumnPosition).ToString());
                }
                int Rows = this._Chart.MaxRowCount(CV.ID);
                if (Rows > 1 && !IsHeader && !IsGroup)
                {
                    HWriter.AddAttribute("rowspan", Rows.ToString());
                }
                if (CV.Title != null && CV.Title.Length > 0)
                {
                    string Title = System.Web.HttpUtility.HtmlEncode(CV.Title);
                    HWriter.AddAttribute("title", Title, false);
                }
                if (IsGroup && !IsHeader)
                {
                    HWriter.AddStyleAttribute("border-width", "1px");
                    HWriter.AddStyleAttribute("border-color", this._Chart.ColorCode(CV.ChartColor));
                    HWriter.AddStyleAttribute("background-color", "white");
                }
                else if (IsHeader || !IsGroup)
                {
                    if (this._Chart.RepresentationID(CV.ID) != -1)
                        HWriter.AddAttribute("onclick", "function_ID(" + this._Chart.RepresentationID(CV.ID).ToString() + ")");
                    HWriter.AddStyleAttribute("background-color", this._Chart.ColorCode(CV.ChartColor));
                }
                HWriter.AddStyleAttribute("font-size", Size.ToString() + "px");
                if (CV.RowCount > 1)
                    HWriter.AddStyleAttribute("vertical-align", "top");
                HWriter.AddStyleAttribute("color", System.Drawing.ColorTranslator.ToHtml(CV.ForeColor));
                if (CV.ForeColor != System.Drawing.Color.White &&
                    CV.ForeColor != System.Drawing.Color.Black &&
                    CV.ForeColor.Name != "0" &&
                    !IsGroup)
                    HWriter.AddStyleAttribute("font-weight", "bold");
                HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Td);
                if (IsGroup && !IsHeader)
                {
                    if (ParentKeys == null)
                    {
                        ParentKeys = new List<int>();
                        // get Parent of current dataset
                        ChartValue Parent = this._Chart.getChartParent(CV.ID);
                        if (Parent.ID > -1)
                        {
                            ParentKeys.Add(Parent.ID);
                        }
                    }
                    ParentKeys.Add(CV.ID);
                    HWriter.WriteLine();
                    string Website = this.initHtmlTable(ParentKeys);
                    HWriter.AddAttribute("href", Website);
                    //HWriter.AddStyleAttribute("font-weight", "bold");
                    HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.A);
                    HWriter.WriteEncodedText(CV.DisplayText);
                    HWriter.RenderEndTag();
                    HWriter.WriteLine();
                }
                else
                {
                    // getting the number of depending rows
                    int MaxRows = this._Chart.MaxRowCount(CV.ID);
                    if (false)
                    {
                        // wrap if there are depending data
                        bool wrapText = CV.ChildrenIDs != null && CV.ChildrenIDs.Count > 0 && MaxRows > 1;

                        // with an image
                        if (false)//CV.ImagePath != null && CV.ImagePath.Length > 0)
                        {
                            // wrap if there are no depending data but an image
                            //if (!wrapText && CV.ImagePath != null && CV.ImagePath.Length > 0)
                            //    wrapText = true;

                            // position of image
                            bool imageRight = CV.ChildrenIDs == null;

                            if (imageRight)
                            {
                                HWriter.WriteAttribute("class", "left");
                                HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
                                //this.Html5writeTableFieldText(HWriter, CV.DisplayText, wrapText);
                                HWriter.RenderEndTag();
                                HWriter.WriteAttribute("class", "right");
                                HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
                                this.Html5writeTableImage(HWriter, CV);
                                HWriter.RenderEndTag();
                            }
                        }
                        else // No image
                        {

                        }

                    }

                    // Wrap text if there are depending data or an image
                    bool WrapText = false;
                    int MaxLength = this._Chart.MaxDisplayTextWordLength(CV.ColumnPosition);
                    if (CV.DisplayText.Length > MaxLength)
                    {
                        if ((CV.Images != null && CV.Images.Count > 0) &&
                           (CV.ChildrenIDs == null || CV.ChildrenIDs.Count == 0))
                            WrapText = false;// true;
                        else if (CV.ChildrenIDs != null && CV.ChildrenIDs.Count > 0 && MaxRows > 1)
                        {
                            if (CV.Images == null)
                                WrapText = true;
                            if (CV.Images != null && CV.Images.Count == 0)
                                WrapText = true;
                        }
                        //if ((CV.ImagePath == null || CV.ImagePath.Length == 0) &&
                        //    CV.ChildrenIDs != null && CV.ChildrenIDs.Count > 0 && MaxRows > 1)
                        //    WrapText = true;
                        //else if ((CV.ImagePath != null && CV.ImagePath.Length > 0) &&
                        //    (CV.ChildrenIDs == null || CV.ChildrenIDs.Count == 0))
                        //    WrapText = false;// true;
                    }
                    HWriter.WriteLine();
                    HWriter.AddAttribute("class", "left");
                    HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
                    int TextLines = this.Html5writeTableFieldText(HWriter, CV.DisplayText, WrapText, MaxLength);
                    HWriter.RenderEndTag();
                    HWriter.WriteLine();
                    if (CV.Images != null && CV.Images.Count > 0)// CV.ImagePath != null && CV.ImagePath.Length > 0)
                    {
                        bool PlaceImageUnderneathText = CV.ChildrenIDs != null && CV.ChildrenIDs.Count > 0;
                        if (PlaceImageUnderneathText)
                            HWriter.Write("<br/>");
                        HWriter.WriteLine();
                        HWriter.AddAttribute("class", "right");
                        HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
                        this.Html5writeTableImage(HWriter, CV);
                        HWriter.RenderEndTag();
                        HWriter.WriteLine();
                    }
                    HWriter.RenderEndTag(); // Td
                    HWriter.WriteLine();
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private int Html5writeTableFieldText(System.Web.UI.HtmlTextWriter HWriter, string Text, bool wrap, int MaxLength)
        {
            int iLines = 0;
            if (wrap)
            {
                System.Collections.Generic.List<string> Words = this._Chart.DisplayTextSplitted(Text);
                //System.Collections.Generic.List<string> HtmlLines = new List<string>();
                string HtmlText = "";
                for (int i = 0; i < Words.Count; i++)// (string W in Words)
                {
                    HtmlText += Words[i];
                    string NextWord = "";
                    if (i < Words.Count - 1)
                        NextWord = Words[i + 1].Trim();
                    if (HtmlText.Length + NextWord.Length >= MaxLength)
                    {
                        if (i > 0 && iLines > 0)
                            HWriter.WriteBreak();
                        else if (HtmlText.StartsWith(" "))
                            HWriter.Write("&nbsp;");
                        HWriter.WriteEncodedText(HtmlText.Trim());
                        HtmlText = "";
                        iLines++;
                    }
                    else
                    {
                        if (i == Words.Count - 1)
                        {
                            if (i > 0)
                                HWriter.WriteBreak();
                            HWriter.WriteEncodedText(HtmlText.Trim());
                        }
                        else
                        {
                            if (HtmlText.Length > 0)
                                HtmlText += " ";
                            //HtmlText += Words[i];
                        }
                    }
                    //if (HtmlText.Length > 0)
                    //    HWriter.WriteEncodedText(HtmlText);
                    //if (i > 0) HWriter.WriteBreak();
                    //HWriter.WriteEncodedText(Words[i]);
                    //iLines++;
                }
                //if (HtmlText.Trim().Length > 0)
                //    HWriter.WriteEncodedText(HtmlText.Trim());
                //foreach (string W in Words)
                //{
                //    if (iLines > 0) HWriter.WriteBreak();
                //    HWriter.WriteEncodedText(W);
                //    iLines++;
                //}
            }
            else
            {
                if (Text.StartsWith(" "))
                    HWriter.Write("&nbsp;");
                HWriter.WriteEncodedText(Text);
                iLines++;
            }
            return iLines;
        }

        private void Html5writeTableImage(System.Web.UI.HtmlTextWriter HWriter, ChartValue CV)
        {
            HWriter.WriteLine();
            foreach (ChartImage CI in CV.Images)
            {
                HWriter.AddAttribute("src", CI.Source);
                if (CI.Alt != null && CI.Alt.Length > 0)
                {
                    HWriter.AddAttribute("alt", CI.Alt);
                }
                else
                {
                    if (CV.Title != null && CV.Title.Length > 0)
                        HWriter.AddAttribute("alt", CV.Title);
                    else
                        HWriter.AddAttribute("alt", CV.DisplayText);
                }
                if (CI.Width > 0)
                {
                    HWriter.AddAttribute("width", CI.Width.ToString());
                    if (CI.Height > 0)
                        HWriter.AddAttribute("height", CI.Height.ToString());
                }
                HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Img);
                HWriter.RenderEndTag();
                HWriter.WriteLine();
            }
            //HWriter.AddAttribute("src", CV.ImagePath);
            //if (CV.Title != null && CV.Title.Length > 0)
            //    HWriter.AddAttribute("alt", CV.Title);
            //else
            //    HWriter.AddAttribute("alt", CV.DisplayText);
            //if (CV.ImageWidth > 0)
            //{
            //    HWriter.AddAttribute("width", CV.ImageWidth.ToString());
            //    if (CV.ImageHeigth > 0)
            //        HWriter.AddAttribute("height", CV.ImageHeigth.ToString());
            //}
            //else
            //{

            //}
            //HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Img);
            //HWriter.RenderEndTag();
            //HWriter.WriteLine(); 
        }



        private void Html5writeFunction(System.Web.UI.HtmlTextWriter HWriter)
        {
            try
            {
                HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Script);
                HWriter.WriteLine("function function_ID(ID) { ");
                HWriter.WriteLine("var IDText = document.getElementById(\"ID\");");
                HWriter.WriteLine("IDText.value = ID; ");
                HWriter.WriteLine("IDText.select(); ");
                HWriter.WriteLine("document.execCommand(\"copy\"); }");
                HWriter.RenderEndTag(); // Script

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        private int ChildrenCountLines(ChartValue CV)
        {
            int i = 0;
            if (CV.ChildrenIDs != null && CV.ChildrenIDs.Count > 0)
            {
                i += CV.ChildrenIDs.Count - 1;
                foreach (int c in CV.ChildrenIDs)
                {
                    ChartValue ccv = this._Chart.getChartValue(c);
                    i += this.ChildrenCountLines(ccv);
                }
            }
            return i;
        }
        #endregion

        #endregion

    }
}
