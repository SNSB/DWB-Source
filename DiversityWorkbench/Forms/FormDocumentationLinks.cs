using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormDocumentationLinks : Form
    {
        private System.IO.FileInfo _fileInfo = null;
        private System.Collections.Generic.Dictionary<string, string> _Links;
        private string _LinkMarker;
        public FormDocumentationLinks(System.IO.FileInfo fileInfo)
        {
            InitializeComponent();
            _fileInfo = fileInfo;
            this.textBoxLink.Text = _fileInfo.Name;
            this.initGrid();
        }

        private void initGrid()
        {
            System.Collections.Generic.Queue<string> Lines = HugoLinksFileContent(_fileInfo);
            _Links = new Dictionary<string, string>();
            try
            {
                while (Lines.Count > 0)
                {
                    string Line = Lines.Dequeue();
                    string Link = GetLinkFromLine(Line);
                    if (Link.Length > 0)
                    {
                        if (!_Links.ContainsKey(Line))
                            _Links.Add(Line, Link);
                    }
                }
                if (_Links.Count > 0)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, string> Link in _Links)
                    {
                        this.dataGridView.Rows.Add(Link.Key, Link.Value);
                    }
                }
                else
                    System.Windows.Forms.MessageBox.Show("No links for a marker in file detected");
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        private System.Collections.Generic.Queue<string> HugoLinksFileContent(System.IO.FileInfo file)
        {
            System.Collections.Generic.Queue<string> Lines = new Queue<string>();
            using (StreamReader sr = file.OpenText())
            {
                while (!sr.EndOfStream)
                {
                    Lines.Enqueue(sr.ReadLine());
                }
            }
            return Lines;
        }

        private string GetLinkFromLine(string line)
        {
            line = line.Replace(" ", "-").ToLower();
            string Link = new string(line.Where(c => char.IsLetterOrDigit(c) || c == ' ' || c == '-').ToArray());
            if (Link.StartsWith("-"))
                Link = Link.Substring(1);
            if (Link.Length > 0 && line.StartsWith("#") && line.IndexOf("#-") > -1)
            {
                //Link = line.Substring(line.IndexOf(" ")).Trim();
                //Link = Link.Replace(" ", "-");
                //Link = Link.Replace("*", "");
                //Link = Link.Replace(":", "");
                Link = "#" + Link;
            }
            else Link = "";
            return Link;
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            _LinkMarker = this.dataGridView.Rows[dataGridView.SelectedCells[0].RowIndex].Cells[1].Value.ToString();
        }

        public string LinkMarker {  get { return _LinkMarker; }  }

        public System.Collections.Generic.Dictionary<string, string> Links { get { return _Links; } }

        private void FormDocumentationLinks_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK && this._LinkMarker.Length == 0) 
            {
                System.Windows.Forms.MessageBox.Show("Please select a link in the table");
                e.Cancel = true;
            }
        }
    }
}
