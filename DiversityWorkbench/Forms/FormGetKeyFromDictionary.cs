using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormGetKeyFromDictionary : Form
    {
        private int _Key;
        System.Collections.Generic.Dictionary<int, string> _Dict;
        public FormGetKeyFromDictionary(string Title, string Header, System.Collections.Generic.Dictionary<int, string> dict)
        {
            InitializeComponent();
            try
            {
                this.Text = Title;
                this.label.Text = Header;
                System.Collections.Generic.List<string> list = new List<string>();
                foreach (System.Collections.Generic.KeyValuePair<int, string> KV in dict)
                    list.Add(KV.Value);
                this.comboBox.DataSource = list;
                this.userControlDialogPanel.buttonOK.Enabled = false;
                _Dict = dict;
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            foreach (System.Collections.Generic.KeyValuePair<int, string> KV in _Dict)
            {
                if (KV.Value == comboBox.SelectedItem.ToString())
                {
                    _Key = KV.Key;
                    break;
                }
            }
            this.userControlDialogPanel.buttonOK.Enabled = true;
        }

        public int Key { get => _Key; }

    }
}
