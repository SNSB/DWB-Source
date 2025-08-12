using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public class TextBoxMultilineAutocomplete
    {
        #region Parameter

        private ListBox _ListBox;
        private TextBox _TextBox;
        private AutoCompleteStringCollection _AutoCompleteStringCollection;
        private System.Collections.Generic.List<string> _List;
        private System.Collections.Generic.List<System.Windows.Forms.Control> _Controls;
        private List<string> _localList;


        #endregion

        #region Construction

        public TextBoxMultilineAutocomplete(ref Control ParentControl, ref TextBox textBox, AutoCompleteStringCollection autoCompleteStringCollection)
        {
            this._TextBox = textBox;
            this._ListBox = new ListBox();
            this._localList = new List<string>();
            //this._Controls.Add(_ListBox);
            this._TextBox.Controls.Add(_ListBox);
            _ListBox.Hide();
            this._AutoCompleteStringCollection = autoCompleteStringCollection;
            textBox.KeyUp += textBox_KeyUp;
            _List = new List<string>();
            System.Collections.IEnumerator enumerator = autoCompleteStringCollection.GetEnumerator();
            while (enumerator.MoveNext())
            {
                string Current = enumerator.Current.ToString();
                if (!_List.Contains(Current))
                _List.Add(Current);
            }
        }

        #endregion

        #region Events

        private void textBox_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if ((_TextBox.Text.Length > 3 && _localList.Count < 10) || _TextBox.Text.Length == 0)
                return;
            var x = _TextBox.Left;
            var y = _TextBox.Top + _TextBox.Height;
            var width = _TextBox.Width + 20;
            const int height = 40;

            _ListBox.SetBounds(x, y, width, height);
            _ListBox.KeyDown += listBox_SelectedIndexChanged;

            _localList = _List.Where(z => z.StartsWith(_TextBox.Text)).ToList();
            if (_localList.Any() && !string.IsNullOrEmpty(_TextBox.Text))
            {
                _ListBox.DataSource = _localList;
                _ListBox.Show();
                _ListBox.Focus();
            }
        }

        private void listBox_SelectedIndexChanged(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyValue == (decimal)System.Windows.Forms.Keys.Enter)
            {
                this._TextBox.Text = ((System.Windows.Forms.ListBox)sender).SelectedItem.ToString();
                _ListBox.Hide();
            }
        }

        #endregion

    }


}
