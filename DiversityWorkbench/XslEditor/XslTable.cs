using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.XslEditor
{
    public class XslTable
    {
        #region Parameter

        private System.Collections.Generic.List<DiversityWorkbench.XslEditor.XslTableRow> _XslTableRows;
        private System.Collections.Generic.List<DiversityWorkbench.XslEditor.XslTableColumn> _XslTableColumns;
        private DiversityWorkbench.XslEditor.XslNode _XslNode;

        #endregion

        #region Construction
        
        public XslTable(DiversityWorkbench.XslEditor.XslNode XslNode)
        {
            this._XslNode = XslNode;
        }
        
        #endregion

        #region Public properties

        public System.Collections.Generic.List<DiversityWorkbench.XslEditor.XslTableColumn> TableColums
        {
            get
            {
                if (this._XslTableColumns == null)
                {
                    this._XslTableColumns = new List<DiversityWorkbench.XslEditor.XslTableColumn>();
                    if (this._XslNode.Name == "table" && this._XslNode.XslNodeType == "html" && this._XslNode.XslNodes.Count > 0)
                    {
                        foreach (DiversityWorkbench.XslEditor.XslNode N in this._XslNode.XslNodes)
                        {
                            if (N.Name == "tr" && N.XslNodeType == "html" && N.XslNodes.Count > 0)
                            {
                                foreach (DiversityWorkbench.XslEditor.XslNode NC in N.XslNodes)
                                {
                                    if ((NC.Name == "th" || NC.Name == "td") && NC.XslNodeType == "html")
                                    {
                                        int Width = 20;
                                        if (NC.Attributes.Count > 0)
                                        {
                                            if (NC.Attributes.ContainsKey("width"))
                                                int.TryParse(NC.Attributes["width"], out Width);
                                        }
                                        DiversityWorkbench.XslEditor.XslTableColumn C = new XslTableColumn(NC);
                                        this._XslTableColumns.Add(C);
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
                return _XslTableColumns;
            }
        }

        public System.Collections.Generic.List<DiversityWorkbench.XslEditor.XslTableRow> TableRows
        {
            get
            {
                if (this._XslTableRows == null)
                {
                    this._XslTableRows = new List<DiversityWorkbench.XslEditor.XslTableRow>();
                    if (this._XslNode.Name == "table" && this._XslNode.XslNodeType == "html" && this._XslNode.XslNodes.Count > 0)
                    {
                        foreach (DiversityWorkbench.XslEditor.XslNode N in this._XslNode.XslNodes)
                        {
                            if (N.Name == "tr" && N.XslNodeType == "html")
                            {
                                int Height = 20;
                                if (N.Attributes.Count > 0)
                                {
                                    if (N.Attributes.ContainsKey("height"))
                                        int.TryParse(N.Attributes["height"], out Height);
                                }
                                DiversityWorkbench.XslEditor.XslTableRow C = new XslTableRow(N);
                                this._XslTableRows.Add(C);
                            }
                        }
                    }
                }
                return _XslTableRows;
            }
        }

        public int TableWidth
        {
            get
            {
                int i = 0;
                foreach (DiversityWorkbench.XslEditor.XslTableColumn C in this.TableColums)
                {
                    i = i + C.ColumnWidth;
                }
                return i;
            }
        }
        
        public int TableHeight
        {
            get
            {
                int i = 0;
                foreach (DiversityWorkbench.XslEditor.XslTableRow R in this.TableRows)
                {
                    i = i + R.RowHeight;
                }
                return i;
            }
        }

        public int ColumnCount { get { return this.TableColums.Count; } }

        public int RowCount { get { return this.TableRows.Count; } }

        public DiversityWorkbench.XslEditor.XslTableRow GetTableRow()
        {
            System.Collections.Generic.Dictionary<string, string> Att = new Dictionary<string, string>();
            Att.Add("height", "20");
            DiversityWorkbench.XslEditor.XslNode N = new XslNode("html", "tr", Att);
            this._XslNode.XslNodes.Add(N);
            DiversityWorkbench.XslEditor.XslTableRow R = new XslTableRow(N);
            this._XslTableRows = null;
            return R;
        }

        public void RemoveRow()
        {
        }

        public DiversityWorkbench.XslEditor.XslTableColumn GetTableColumn()
        {
            System.Collections.Generic.Dictionary<string, string> Att = new Dictionary<string, string>();
            Att.Add("width", "20");
            DiversityWorkbench.XslEditor.XslNode Nnew = new XslNode("html", "th", Att);
            foreach (DiversityWorkbench.XslEditor.XslNode NR in this._XslNode.XslNodes)
            {
                if (NR.Name == "tr" && NR.XslNodeType == "html" && NR.XslNodes.Count > 0)
                {
                    foreach (DiversityWorkbench.XslEditor.XslNode NC in NR.XslNodes)
                    {
                        if ((NC.Name == "th" || NC.Name == "td") && NC.XslNodeType == "html")
                        {
                            NR.XslNodes.Add(Nnew);
                            break;
                        }
                    }
                    break;
                }
            }
            this._XslTableColumns = null;
            DiversityWorkbench.XslEditor.XslTableColumn C = new XslTableColumn(Nnew);
            return C;
        }

        public void RemoveColumn()
        {
        }

        #endregion

    }
}
