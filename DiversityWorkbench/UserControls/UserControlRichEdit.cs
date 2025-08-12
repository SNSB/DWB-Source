using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityWorkbench.UserControls
{
    public partial class UserControlRichEdit : UserControl
    {
        #region Events
        public event EventHandler EditTextChanged;

        protected virtual void OnEditTextChanged(EventArgs e)
        {
            EventHandler handler = EditTextChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler EditTextDoubleClick;

        protected virtual void OnEditTextDoubleClick(EventArgs e)
        {
            EventHandler handler = EditTextDoubleClick;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        #endregion

        #region Parameter
        private Color _WarnColor = Color.Yellow;
        private Color _ErrorColor = Color.LightCoral;
        private Color _A = Color.Green;
        private Color _C = Color.Blue;
        private Color _G = Color.Black;
        private Color _T = Color.Red;
        private Color _AmbColor = Color.Gray;
        private Color _GapColor = Color.Silver;
        //private Color _BackColor = SystemColors.Window;
        //private Color _ForeColor = SystemColors.WindowText;
        private Font _FontBold;
        private Font _FontItalic;
        private bool _UseDefaultBackColor = true;

        private string _SequenceType = "";
        private int _SymbolLength = 0;
        private int _DisplayLength = 1;
        private string _GapSymbol = "";
        private StringBuilder _SB;
        private bool _EnableAmbiguity = true;
        private Dictionary<string, string> _Symbols;
        private Dictionary<string, string> _AmbSymbols;
        private int _LastCursor = 0;
        private bool _Init;
        private string _EditText = "";
        private string _OriginalEditText = "";

        public string EditText
        {
            get
            {
                //if (_SymbolLength < 1)
                //    _EditText = rtf2Html(this.richTextBox.Rtf);
                return _EditText;
            }
            set
            {
                if (_SymbolLength < 1)
                {
                    this.richTextBox.SuspendLayout();
                    this.richTextBox.Enabled = false;
                    this.richTextBox.Rtf = "";
                    this.richTextBox.Rtf = html2Rtf(value == null ? "" : value);
                    this.richTextBox.Enabled = true;
                    this.richTextBox.ResumeLayout();
                }
                _EditText = value == null ? "" : formattedString(value);
                if (_OriginalEditText == "")
                    _OriginalEditText = _EditText; // Save original text for reload
                setEditText();
            }
        }

        public bool ReadOnly
        {
            get { return this.richTextBox.ReadOnly; }
            set
            {
                this.richTextBox.ReadOnly = value;
                if (_UseDefaultBackColor)
                    this.richTextBox.BackColor = (value ? SystemColors.Control : SystemColors.Window);
                this.toolStripLabelInsert.Visible = !ReadOnly && _SymbolLength >= 1;
                this.toolStripComboBoxSymbol.Visible = !ReadOnly && _SymbolLength >= 1;
                this.toolStripButtonInsert.Visible = !ReadOnly && _SymbolLength >= 1;
                this.toolStripButtonBold.Visible = !ReadOnly && _SymbolLength < 1;
                this.toolStripButtonItalic.Visible = !ReadOnly && _SymbolLength < 1;
                this.toolStripButtonUnderline.Visible = !ReadOnly && _SymbolLength < 1;
                this.toolStripSeparator1.Visible = !ReadOnly;
                this.toolStripButtonWeb.Visible = !ReadOnly && _SymbolLength < 1;
                this.toolStripDropDownButtonDatabase.Visible = !ReadOnly && _SymbolLength < 1;
                this.toolStripSeparator2.Visible = !ReadOnly;
                this.toolStripButtonReload.Visible = !ReadOnly && _SymbolLength < 1;
            }
        }

        public bool ShowMenu
        {
            get { return this.toolStrip.Visible; }
            set { this.toolStrip.Visible = value; }
        }

        public bool ShowStatus
        {
            get { return this.statusStrip.Visible; }
            set { this.statusStrip.Visible = value; }
        }
        #endregion

        #region Construction
        public UserControlRichEdit()
        {
            InitializeComponent();
            if (_UseDefaultBackColor)
                this.richTextBox.BackColor = (this.richTextBox.ReadOnly ? SystemColors.Control : SystemColors.Window);
            _SB = new StringBuilder();
            _FontBold = new Font(this.richTextBox.Font, FontStyle.Bold);
            _FontItalic = new Font(this.richTextBox.Font, FontStyle.Italic);
            this.toolStrip.Visible = false;
            _Init = false;
        }
        #endregion

        #region Public
        /// <summary>
        /// Set up the control for rich text editing
        /// </summary>
        public void SetControl()
        {
            _Init = false;
            _SequenceType = "";
            _SymbolLength = 0;
            _DisplayLength = 1;
            _GapSymbol = "";
            _EnableAmbiguity = false;
            _Symbols = new Dictionary<string, string>();
            _AmbSymbols = new Dictionary<string, string>();
            //_ForeColor = this.richTextBox.ForeColor;

            if (this.richTextBox.ContextMenuStrip != this.ContextMenuStrip)
                this.richTextBox.ContextMenuStrip = this.ContextMenuStrip;

            this.toolStrip.Visible = false;
            this.toolStripComboBoxSymbol.Items.Clear();
            this.toolStripLabelInsert.Visible = false;
            this.toolStripComboBoxSymbol.Visible = false;
            this.toolStripButtonInsert.Visible = false;
            this.toolStripButtonBold.Visible = true;
            this.toolStripButtonItalic.Visible = true;
            this.toolStripButtonUnderline.Visible = true;
            this.toolStripSeparator1.Visible = true;
            this.toolStripButtonWeb.Visible = true;
            this.toolStripDropDownButtonDatabase.Visible = true;
            this.toolStripSeparator2.Visible = true;
            this.toolStripButtonReload.Visible = true;
            this.toolStripButtonScanRtf.Visible = false;
            this.toolStripStatusLabelType.Visible = false;
            this.toolStripStatusLabelGap.Visible = false;
            this.toolStripStatusLabelGapSym.Visible = false;
            this.toolStripStatusLabelLen.Visible = false;
            this.toolStripStatusLabelLength.Visible = false;

            _Init = true;
            setEditText();
        }

        /// <summary>
        /// Set up control for molecular sequence editing
        /// </summary>
        /// <param name="sequenceType">'Nucleotide' or 'Protein'</param>
        /// <param name="symbolLength">1 or 3 for protein sequences, 1 for nucleotide sequences</param>
        /// <param name="gapSymbol">A string with symbolLength representing a gap of unspecified length. Do not mix up with 'unknown' symbols! NULL if no gap symbol is supported.</param>
        /// <param name="enableAbiguity">'true' if ambiguity symbols are allowed.</param>
        /// <param name="symbols">A dictionary of the symbols and their long text.</param>
        /// <param name="ambiguitySymbols">A dictionary of the ambiguity symbols and their long text.</param>
        public void SetControl(string sequenceType, int symbolLength, string gapSymbol, bool enableAbiguity, Dictionary<string, string> symbols, Dictionary<string, string> ambiguitySymbols)
        {
            _Init = false;
            _SequenceType = sequenceType;
            _SymbolLength = symbolLength;
            _DisplayLength = _SymbolLength;
            _GapSymbol = gapSymbol != null ? gapSymbol.Trim() : "";
            _EnableAmbiguity = enableAbiguity;
            _Symbols = symbols;
            _AmbSymbols = ambiguitySymbols;
            //_ForeColor = this.richTextBox.ForeColor;

            // Build combo box
            this.toolStripComboBoxSymbol.Items.Clear();
            this.toolStripComboBoxSymbol.Items.Add("");
            foreach (var item in _Symbols)
                this.toolStripComboBoxSymbol.Items.Add(item.Value);
            if (_EnableAmbiguity)
                foreach (var item in _AmbSymbols)
                    this.toolStripComboBoxSymbol.Items.Add(item.Value);
            if (_GapSymbol != "")
                this.toolStripComboBoxSymbol.Items.Add("Gap symbol");
            this.toolStripComboBoxSymbol.SelectedIndex = 0;
            this.toolStripButtonInsert.Enabled = false;

            // Initialize controls
            this.toolStripLabelInsert.Visible = true;
            this.toolStripComboBoxSymbol.Visible = true;
            this.toolStripButtonInsert.Visible = true;
            this.toolStripButtonBold.Visible = false;
            this.toolStripButtonItalic.Visible = false;
            this.toolStripButtonUnderline.Visible = false;
            this.toolStripSeparator1.Visible = false;
            this.toolStripButtonWeb.Visible = false;
            this.toolStripDropDownButtonDatabase.Visible = false;
            this.toolStripSeparator2.Visible = false;
            this.toolStripButtonReload.Visible = false;
            this.toolStripButtonScanRtf.Visible = false;
            this.toolStripStatusLabelLen.Visible = false;
            this.toolStripStatusLabelLength.Visible = false;
            this.toolStripStatusLabelGap.Visible = false;
            this.toolStripStatusLabelGapSym.Visible = false;
            this.toolStripStatusLabelType.Text = "";
            this.toolStripStatusLabelPosition.Text = "";
            this.toolStripStatusLabelDisplay.Text = "";
            this.toolStripStatusLabelDisplay.ToolTipText = "";

            if (_UseDefaultBackColor)
                this.toolStripStatusLabelDisplay.BackColor = this.toolStripStatusLabelGap.BackColor;

            this.toolStripStatusLabelGapSym.Text = _GapSymbol;
            if (_GapSymbol != "")
            {
                this.toolStripStatusLabelGap.Visible = true;
                this.toolStripStatusLabelGapSym.Visible = true;
            }

            switch (_SequenceType)
            {
                case "Nucleotide":
                    _SymbolLength = 1;
                    this.toolStripStatusLabelType.Visible = true;
                    this.toolStripStatusLabelLen.Visible = true;
                    this.toolStripStatusLabelLength.Visible = true;
                    this.toolStripStatusLabelType.Text = WorkbenchMessages.Nucleotide;
                    this.toolStripStatusLabelLength.Text = _SymbolLength.ToString();
                    break;
                case "Protein":
                    this.toolStripStatusLabelType.Visible = true;
                    this.toolStripStatusLabelLen.Visible = true;
                    this.toolStripStatusLabelLength.Visible = true;
                    this.toolStripStatusLabelType.Text = WorkbenchMessages.Protein;
                    this.toolStripStatusLabelLength.Text = _SymbolLength.ToString();
                    break;
                default:
                    _SymbolLength = 0;
                    break;
            }
            if (this.richTextBox.ContextMenuStrip != this.ContextMenuStrip)
                this.richTextBox.ContextMenuStrip = this.ContextMenuStrip;

            _Init = true;
            setEditText();
        }
        #endregion

        #region Private
        private void setEditText()
        {
            if (_SymbolLength < 1)
            {
                bool showRtf = _EditText.Contains("\\b{}") || _EditText.Contains("\\i{}") || _EditText.Contains("\\ul{}");
                this.toolStripButtonScanRtf.Visible = showRtf;
                if (this.richTextBox.ReadOnly && showRtf)
                    this.toolStripButtonReload.Visible = true;
            }
            _LastCursor = _EditText.Length;
            formatDisplayText();
            richTextBox_SelectionChanged(null, null);
        }

        private string formattedString(string text)
        {
            return DiversityWorkbench.MolecularSequence.FormattedSequence(text, _SymbolLength, _GapSymbol, _Symbols, _AmbSymbols, _SB);
        }

        private void formatDisplayText()
        {
            bool oldInit = _Init;
            _Init = false;
            this.richTextBox.SuspendLayout();
            this.richTextBox.Visible = false;

            if (_SymbolLength > 0)
            {
                // Format text in default manner
                this.richTextBox.Text = _EditText;
                this.richTextBox.SelectAll();
                this.richTextBox.SelectionBackColor = this.richTextBox.BackColor;
                this.richTextBox.SelectionColor = this.richTextBox.ForeColor;
                this.richTextBox.SelectionFont = _FontBold; // this.richTextBox.Font;

                // Step throug the symbols
                for (int i = 0; i < this.richTextBox.Text.Length; i += _SymbolLength)
                {
                    // Extract actual symbol
                    string sym = this.richTextBox.Text.Substring(i, (this.richTextBox.Text.Length - i >= _SymbolLength ? _SymbolLength : this.richTextBox.Text.Length - i));

                    // Symbol is known
                    if (_Symbols.ContainsKey(sym.ToUpper()))
                    {
                        // Symbol is known
                        if (_SequenceType == "Nucleotide")
                        {
                            switch (sym)
                            {
                                case "A": // Adenine
                                    setSymbolColor(i, sym.Length, _A);
                                    break;
                                case "C": // Cytosine
                                    setSymbolColor(i, sym.Length, _C);
                                    break;
                                case "G": // Guanine
                                    setSymbolColor(i, sym.Length, _G);
                                    break;
                                case "T": // Thymine
                                case "U": // Uracil
                                    setSymbolColor(i, sym.Length, _T);
                                    break;
                                default:
                                    setSymbolColor(i, sym.Length, this.richTextBox.ForeColor, _ErrorColor);
                                    break;
                            }
                        }
                    }
                    else if (_AmbSymbols.ContainsKey(sym.ToUpper()))
                    {
                        if (_EnableAmbiguity) // Ambiguity symbols allowed
                            setSymbolColor(i, sym.Length, _AmbColor);
                        else // Ambiguity symbols not allowed
                            setSymbolColor(i, sym.Length, _AmbColor, _WarnColor);
                    }
                    else if (_GapSymbol.ToUpper() == sym.ToUpper())
                    {
                        // Gap Symbol
                        setSymbolColor(i, sym.Length, _GapColor);
                    }
                    else
                    {
                        // Unknown symbol
                        setSymbolColor(i, sym.Length, this.richTextBox.ForeColor, _ErrorColor);
                    }
                }
            }
            this.richTextBox.Visible = true;
            this.richTextBox.ResumeLayout();
            _Init = oldInit;
            this.richTextBox.Select(_LastCursor, 0);
        }

        private void setSymbolColor(int s, int l, Color c)
        {
            setSymbolColor(s, l, c, this.BackColor);
        }

        private void setSymbolColor(int s, int l, Color c, Color b)
        {
            this.richTextBox.Select(s, l);
            this.richTextBox.SelectionColor = c;
            this.richTextBox.SelectionBackColor = b;
        }

        private string html2Rtf(string html)
        {
            this.richTextBoxCvt.Text = "";
            string raw = html.Replace("<br/>", "<br />").Replace("<br>", "<br />");
            while (raw.IndexOf("<br /> ") > -1)
                raw = raw.Replace("<br /> ", "<br />");
            raw = raw.Replace("<br />", "\n").Replace("\r", "").Replace("<p>", "\n").Replace("</p>", "\n");

            this.richTextBoxCvt.Text = raw;
            raw = this.richTextBoxCvt.Rtf.Replace("</b>", "\\b0 ").Replace("<b>", "\\b ").Replace("</i>", "\\i0 ").Replace("<i>", "\\i ").Replace("</u>", "\\ulnone ").Replace("<u>", "\\ul ");
            this.richTextBoxCvt.Rtf = raw.Replace("</strong>", "\\b0 ").Replace("<strong>", "\\b ").Replace("</em>", "\\i0 ").Replace("<em>", "\\i ");
            return this.richTextBoxCvt.Rtf;
        }

        private string rtf2Html(string rtf)
        {
            this.richTextBoxCvt.Text = "";
            string[] splitter = { "\\\\" };
            string[] cvt = rtf.Split(splitter, StringSplitOptions.None);
            string raw = "";
            for (int i = 0; i < cvt.Length; i++)
            {
                cvt[i] = cvt[i].Replace("\\b0 ", "</b>").Replace("\\b ", "<b>").Replace("\\i0 ", "</i>").Replace("\\i ", "<i>").Replace("\\ulnone ", "</u>").Replace("\\ul0 ", "</u>").Replace("\\ul ", "<u>");
                cvt[i] = cvt[i].Replace("\\b0", "</b>").Replace("\\b", "<b>").Replace("\\i0", "</i>").Replace("\\i", "<i>").Replace("\\ulnone", "</u>").Replace("\\ul", "<u>");
                cvt[i] = cvt[i].Replace("<u>0\\cf0", "\\ul0\\cf0"); // Special handling of embedded links!!!
                raw += (i > 0 ? "\\\\" : "") + cvt[i];
            }
            this.richTextBoxCvt.Rtf = raw;
            return this.richTextBoxCvt.Text.Replace("\n", "\r\n");
        }

        private int getTextIndex(int cursor)
        {
            int stopIdx = _EditText.Length;
            int startIdx = 0;

            while (startIdx < stopIdx)
            {
                startIdx = _EditText.IndexOf('<', startIdx);
                if (startIdx < 0 || cursor <= startIdx)
                    break;
                int endIdx = _EditText.IndexOf('>', startIdx);
                if (endIdx < 0)
                    break;
                string tag = _EditText.Substring(startIdx, endIdx - startIdx + 1);
                switch (tag)
                {
                    case "<b>":
                    case "<i>":
                    case "<u>":
                    case "</b>":
                    case "</i>":
                    case "</u>":
                    case "<em>":
                    case "</em>":
                    case "<br>":
                    case "<br/>":
                    case "<br />":
                    case "<strong>":
                    case "</strong>":
                        startIdx += tag.Length;
                        cursor += tag.Length;
                        break;
                    default:
                        startIdx += endIdx - startIdx + 1;
                        break;
                }
            }
            string checkCRLF = _EditText.Replace("\r\n", "\n");
            if (checkCRLF.Length > cursor)
                checkCRLF = checkCRLF.Remove(cursor);
            while (checkCRLF.Contains("\n"))
            {
                checkCRLF = checkCRLF.Remove(checkCRLF.LastIndexOf("\n"));
                cursor++;
            }
            return cursor;
        }

        private void insertLinkAtCursor(string text)
        {
            int cursor = getTextIndex(this.richTextBox.SelectionStart);
            string pre = "";
            string post = "";
            if (cursor > 0 && !char.IsWhiteSpace(_EditText[cursor - 1]))
                pre = " ";
            if (cursor < _EditText.Length && !char.IsWhiteSpace(_EditText[cursor]))
                post = " ";
            EditText = (cursor < _EditText.Length ? _EditText.Remove(cursor) + pre + text + post + _EditText.Substring(cursor) : _EditText + pre + text);
        }
        #endregion

        #region Events
        private void richTextBox_TextChanged(object sender, EventArgs e)
        {
            if (_Init && this.richTextBox.Enabled)
            {
                if (_SymbolLength > 0)
                {
                    _EditText = formattedString(this.richTextBox.Text);
                    formatDisplayText();
                }
                else
                {
                    _EditText = rtf2Html(this.richTextBox.Rtf);
                    this.toolStripButtonScanRtf.Visible = _EditText.Contains("\\b{}") || _EditText.Contains("\\i{}") || _EditText.Contains("\\ul{}");
                }
                OnEditTextChanged(e);
                this.richTextBox.Focus();
            }
        }

        private void richTextBox_SelectionChanged(object sender, EventArgs e)
        {
            if (_Init)
            {
                _LastCursor = this.richTextBox.SelectionStart;
                this.toolStripStatusLabelPosition.Text = (_LastCursor / _DisplayLength + 1).ToString();
                this.toolStripStatusLabelDisplay.BackColor = this.toolStripStatusLabelGap.BackColor;
                this.toolStripStatusLabelDisplay.ToolTipText = "";
                if (_SymbolLength > 0)
                {
                    string symbol = "";
                    string SYMBOL = "";
                    if ((_LastCursor - (_LastCursor % _DisplayLength) + _DisplayLength) <= this.richTextBox.Text.Length)
                    {
                        symbol = this.richTextBox.Text.Substring(_LastCursor - (_LastCursor % _DisplayLength), _DisplayLength);
                        SYMBOL = symbol.ToUpper();
                    }

                    if (_Symbols.ContainsKey(SYMBOL))
                        this.toolStripStatusLabelDisplay.Text = _Symbols[SYMBOL];
                    else if (_AmbSymbols.ContainsKey(SYMBOL))
                    {
                        this.toolStripStatusLabelDisplay.Text = _AmbSymbols[SYMBOL];
                        if (!_EnableAmbiguity)
                        {
                            this.toolStripStatusLabelDisplay.BackColor = _WarnColor;
                            this.toolStripStatusLabelDisplay.ToolTipText = WorkbenchMessages.NoAmbiguitySymbols;
                        }
                    }
                    else if (SYMBOL == "")
                        this.toolStripStatusLabelDisplay.Text = "";
                    else if (this.toolStripStatusLabelGapSym.Text.ToUpper() == SYMBOL)
                        this.toolStripStatusLabelDisplay.Text = "Gap symbol";
                    else
                    {
                        this.toolStripStatusLabelDisplay.Text = string.Format(WorkbenchMessages.IllegalSymbol, symbol);
                        this.toolStripStatusLabelDisplay.BackColor = _ErrorColor;
                    }
                }
                else
                {
                    this.toolStripButtonBold.BackColor = SystemColors.Control;
                    this.toolStripButtonItalic.BackColor = SystemColors.Control;
                    this.toolStripButtonUnderline.BackColor = SystemColors.Control;

                    if (this.richTextBox.SelectionFont != null)
                    {
                        if (this.richTextBox.SelectionFont.Bold) this.toolStripButtonBold.BackColor = SystemColors.ControlDark;
                        if (this.richTextBox.SelectionFont.Italic) this.toolStripButtonItalic.BackColor = SystemColors.ControlDark;
                        if (this.richTextBox.SelectionFont.Underline) this.toolStripButtonUnderline.BackColor = SystemColors.ControlDark;
                    }

                }
            }
        }

        private void richTextBox_DoubleClick(object sender, EventArgs e)
        {
            OnEditTextDoubleClick(e);
        }

        private void toolStripComboBoxSymbol_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_Init)
            {
                this.toolStripButtonInsert.Enabled = (this.toolStripComboBoxSymbol.SelectedItem.ToString() != "");
            }
        }

        private void toolStripButtonInsert_Click(object sender, EventArgs e)
        {
            if (_Init && this.toolStripComboBoxSymbol.SelectedItem != null)
            {
                string sym = "";
                if (this.toolStripComboBoxSymbol.SelectedItem.ToString() == "Gap symbol")
                {
                    sym = _GapSymbol;
                }
                else if (_Symbols.ContainsValue(this.toolStripComboBoxSymbol.SelectedItem.ToString()))
                {
                    foreach (var item in _Symbols)
                        if (item.Value == this.toolStripComboBoxSymbol.SelectedItem.ToString())
                        {
                            sym = item.Key;
                            break;
                        }
                }
                else if (_AmbSymbols.ContainsValue(this.toolStripComboBoxSymbol.SelectedItem.ToString()))
                {
                    foreach (var item in _AmbSymbols)
                        if (item.Value == this.toolStripComboBoxSymbol.SelectedItem.ToString())
                        {
                            sym = item.Key;
                            break;
                        }
                }

                if (sym != "")
                {
                    int cursor = _LastCursor - _LastCursor % _DisplayLength;
                    string text = cursor < _EditText.Length ? _EditText.Remove(cursor) + sym + _EditText.Substring(cursor) : _EditText + sym;
                    _EditText = formattedString(text);
                    _LastCursor = cursor + _SymbolLength;
                    formatDisplayText();
                }
                //this.toolStripComboBoxSymbol.SelectedIndex = 0;
                //this.toolStripButtonInsert.Enabled = false;
                this.richTextBox.Focus();
            }
        }

        private void toolStripComboBoxSymbol_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_Init)
            {
                if (e.KeyChar == '\r')
                    toolStripButtonInsert_Click(sender, e);
            }
        }

        private void richTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_Init && _SymbolLength > 0)
            {
                if (e.KeyChar == '\t')
                {
                    if (this.richTextBox.SelectedText == "")
                        _LastCursor = _LastCursor - _LastCursor % _DisplayLength;
                    else
                        _LastCursor = _LastCursor - _LastCursor % _DisplayLength + _SymbolLength;
                    if (_LastCursor >= this.richTextBox.Text.Length)
                        _LastCursor = 0;
                    this.richTextBox.Select(_LastCursor, _SymbolLength);
                    e.Handled = true;
                }
            }
        }

        private void addStyleFlag(FontStyle style)
        {
            int selectionStart = this.richTextBox.SelectionStart;
            int selectionLength = this.richTextBox.SelectionLength;
            this.richTextBox.SuspendLayout();
            this.richTextBox.Enabled = false;
            this.richTextBox.Select(selectionStart, 1);
            int startIdx = selectionStart;
            Font of = this.richTextBox.SelectionFont;
            Font nf = new Font(of, of.Style | style);

            for (int i = selectionStart + 1; i < selectionStart + selectionLength; i++)
            {
                this.richTextBox.SelectionStart = i;
                if (this.richTextBox.SelectionFont.Bold != of.Bold || this.richTextBox.SelectionFont.Italic != of.Bold || this.richTextBox.SelectionFont.Underline != of.Underline)
                {
                    this.richTextBox.Select(startIdx, i - startIdx);
                    this.richTextBox.SelectionFont = nf;

                    this.richTextBox.Select(i, 1);
                    startIdx = i;
                    of = this.richTextBox.SelectionFont;
                    nf = new Font(of, this.richTextBox.SelectionFont.Style | style);
                }
            }
            this.richTextBox.Select(startIdx, selectionStart + selectionLength - startIdx);
            this.richTextBox.SelectionFont = nf;
            this.richTextBox.Enabled = true;
            this.richTextBox.Select(selectionStart, selectionLength);
            this.richTextBox.ResumeLayout();
            this.richTextBox.Focus();
        }

        private void removeStyleFlag(FontStyle style)
        {
            int selectionStart = this.richTextBox.SelectionStart;
            int selectionLength = this.richTextBox.SelectionLength;
            this.richTextBox.SuspendLayout();
            this.richTextBox.Enabled = false;
            this.richTextBox.Select(selectionStart, 1);
            int startIdx = selectionStart;
            Font of = this.richTextBox.SelectionFont;
            Font nf = new Font(of, of.Style & ~style);

            for (int i = selectionStart + 1; i < selectionStart + selectionLength; i++)
            {
                this.richTextBox.SelectionStart = i;
                if (this.richTextBox.SelectionFont.Bold != of.Bold || this.richTextBox.SelectionFont.Italic != of.Bold || this.richTextBox.SelectionFont.Underline != of.Underline)
                {
                    this.richTextBox.Select(startIdx, i - startIdx);
                    this.richTextBox.SelectionFont = nf;

                    this.richTextBox.Select(i, 1);
                    startIdx = i;
                    of = this.richTextBox.SelectionFont;
                    nf = new Font(of, this.richTextBox.SelectionFont.Style & ~style);
                }
            }
            this.richTextBox.Select(startIdx, selectionStart + selectionLength - startIdx);
            this.richTextBox.SelectionFont = nf;
            this.richTextBox.Enabled = true;
            this.richTextBox.Select(selectionStart, selectionLength);
            this.richTextBox.ResumeLayout();
            this.richTextBox.Focus();
        }

        private void toolStripButtonBold_Click(object sender, EventArgs e)
        {
            if (_Init)
            {
                if (this.toolStripButtonBold.BackColor == SystemColors.Control)
                {
                    addStyleFlag(FontStyle.Bold);
                    this.toolStripButtonBold.BackColor = SystemColors.ControlDark;
                }
                else
                {
                    removeStyleFlag(FontStyle.Bold);
                    this.toolStripButtonBold.BackColor = SystemColors.Control;
                }
                richTextBox_TextChanged(sender, e);
            }
        }

        private void toolStripButtonItalic_Click(object sender, EventArgs e)
        {
            if (_Init)
            {
                if (this.toolStripButtonItalic.BackColor == SystemColors.Control)
                {
                    addStyleFlag(FontStyle.Italic);
                    this.toolStripButtonItalic.BackColor = SystemColors.ControlDark;
                }
                else
                {
                    removeStyleFlag(FontStyle.Italic);
                    this.toolStripButtonItalic.BackColor = SystemColors.Control;
                }
                richTextBox_TextChanged(sender, e);
            }
        }

        private void toolStripButtonUnderline_Click(object sender, EventArgs e)
        {
            if (_Init)
            {
                if (this.toolStripButtonUnderline.BackColor == SystemColors.Control)
                {
                    addStyleFlag(FontStyle.Underline);
                    this.toolStripButtonUnderline.BackColor = SystemColors.ControlDark;
                }
                else
                {
                    removeStyleFlag(FontStyle.Underline);
                    this.toolStripButtonUnderline.BackColor = SystemColors.Control;
                }
                richTextBox_TextChanged(sender, e);
            }
        }

        private void toolStripButtonReload_Click(object sender, EventArgs e)
        {
            if (_Init && _SymbolLength < 1)
            {
                EditText = _OriginalEditText;
            }
        }

        private void toolStripButtonScanRtf_Click(object sender, EventArgs e)
        {
            if (_Init && _SymbolLength < 1)
            {
                string text = _EditText.Replace("\\b0{}", "</b>").Replace("\\i0{}", "</i>").Replace("\\ulnone{}", "</u>").Replace("\\ul0{}", "</u>").Replace("\\b{}", "<b>").Replace("\\i{}", "<i>").Replace("\\ul{}", "<u>");
                if (_EditText != text)
                {
                    this.richTextBox.SuspendLayout();
                    this.richTextBox.Enabled = false;
                    this.richTextBox.Rtf = "";
                    this.richTextBox.Rtf = html2Rtf(text);
                    this.richTextBox.Enabled = true;
                    this.richTextBox.ResumeLayout();
                    _EditText = formattedString(text);
                    setEditText();
                }
                else
                    this.toolStripButtonScanRtf.Visible = false;
            }
        }

        private void richTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            if (_Init)
            {
                try
                {
                    string linkText = e.LinkText.ToLower();
                    if (!linkText.Contains("/agent") &&
                        !linkText.Contains("/collection") &&
                        !linkText.Contains("/description") &&
                        !linkText.Contains("/exsiccat") &&
                        !linkText.Contains("/gazetteer") &&
                        !linkText.Contains("/project") &&
                        !linkText.Contains("/reference") &&
                        !linkText.Contains("/samplingplot") &&
                        !linkText.Contains("/scientificterm") &&
                        !linkText.Contains("/taxonname"))
                    {
                        DiversityWorkbench.Forms.FormWebBrowser fw = new DiversityWorkbench.Forms.FormWebBrowser(e.LinkText);
                        fw.ShowExternal = true;
                        fw.ShowDialog();
                    }
                    else
                    {
                        string service = DiversityWorkbench.WorkbenchUnit.getServiceNameFromURI(e.LinkText);
                        DiversityWorkbench.IWorkbenchUnit wu = null;
                        switch (service)
                        {
                            case "DiversityAgents":
                                wu = (DiversityWorkbench.IWorkbenchUnit)(new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection));
                                break;
                            case "DiversityCollection":
                                wu = (DiversityWorkbench.IWorkbenchUnit)(new DiversityWorkbench.CollectionSpecimen(DiversityWorkbench.Settings.ServerConnection));
                                break;
                            case "DiversityDescriptions":
                                wu = (DiversityWorkbench.IWorkbenchUnit)(new DiversityWorkbench.Description(DiversityWorkbench.Settings.ServerConnection));
                                break;
                            case "DiversityExsiccatae":
                                wu = (DiversityWorkbench.IWorkbenchUnit)(new DiversityWorkbench.Exsiccate(DiversityWorkbench.Settings.ServerConnection));
                                break;
                            case "DiversityGazetteer":
                                wu = (DiversityWorkbench.IWorkbenchUnit)(new DiversityWorkbench.Gazetteer(DiversityWorkbench.Settings.ServerConnection));
                                break;
                            case "DiversityProjects":
                                wu = (DiversityWorkbench.IWorkbenchUnit)(new DiversityWorkbench.Project(DiversityWorkbench.Settings.ServerConnection));
                                break;
                            case "DiversityReferences":
                                wu = (DiversityWorkbench.IWorkbenchUnit)(new DiversityWorkbench.Reference(DiversityWorkbench.Settings.ServerConnection));
                                break;
                            case "DiversitySamplingPlots":
                                wu = (DiversityWorkbench.IWorkbenchUnit)(new DiversityWorkbench.SamplingPlot(DiversityWorkbench.Settings.ServerConnection));
                                break;
                            case "DiversityScientificTerms":
                                wu = (DiversityWorkbench.IWorkbenchUnit)(new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection));
                                break;
                            case "DiversityTaxonNames":
                                wu = (DiversityWorkbench.IWorkbenchUnit)(new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection));
                                break;
                            default:
                                DiversityWorkbench.Forms.FormWebBrowser fw = new DiversityWorkbench.Forms.FormWebBrowser(e.LinkText);
                                fw.ShowDialog();
                                return;
                        }
                        DiversityWorkbench.Forms.FormRemoteQuery fq = new DiversityWorkbench.Forms.FormRemoteQuery(e.LinkText, wu);
                        fq.ShowDialog();
                    }
                }
                catch (Exception)
                { }
            }
        }

        private void dAgentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_Init)
            {
                try
                {
                    DiversityWorkbench.IWorkbenchUnit wu = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                    DiversityWorkbench.Forms.FormRemoteQuery rq = new DiversityWorkbench.Forms.FormRemoteQuery(wu);
                    if (rq.ShowDialog() == DialogResult.OK)
                        insertLinkAtCursor(rq.URI);
                    this.richTextBox.Focus();
                }
                catch (Exception)
                { }
            }
        }

        private void dCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_Init)
            {
                try
                {
                    DiversityWorkbench.IWorkbenchUnit wu = new DiversityWorkbench.CollectionSpecimen(DiversityWorkbench.Settings.ServerConnection);
                    DiversityWorkbench.Forms.FormRemoteQuery rq = new DiversityWorkbench.Forms.FormRemoteQuery(null, wu);
                    if (rq.ShowDialog() == DialogResult.OK)
                        insertLinkAtCursor(rq.URI);
                    this.richTextBox.Focus();
                }
                catch (Exception)
                { }
            }
        }

        private void dDescriptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_Init)
            {
                try
                {
                    DiversityWorkbench.IWorkbenchUnit wu = new DiversityWorkbench.Description(DiversityWorkbench.Settings.ServerConnection);
                    DiversityWorkbench.Forms.FormRemoteQuery rq = new DiversityWorkbench.Forms.FormRemoteQuery(null, wu);
                    if (rq.ShowDialog() == DialogResult.OK)
                        insertLinkAtCursor(rq.URI);
                    this.richTextBox.Focus();
                }
                catch (Exception)
                { }
            }
        }

        private void dExsciccataeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_Init)
            {
                try
                {
                    DiversityWorkbench.IWorkbenchUnit wu = new DiversityWorkbench.Exsiccate(DiversityWorkbench.Settings.ServerConnection);
                    DiversityWorkbench.Forms.FormRemoteQuery rq = new DiversityWorkbench.Forms.FormRemoteQuery(null, wu);
                    if (rq.ShowDialog() == DialogResult.OK)
                        insertLinkAtCursor(rq.URI);
                    this.richTextBox.Focus();
                }
                catch (Exception)
                { }
            }
        }

        private void dGazetteersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_Init)
            {
                try
                {
                    DiversityWorkbench.IWorkbenchUnit wu = new DiversityWorkbench.Gazetteer(DiversityWorkbench.Settings.ServerConnection);
                    DiversityWorkbench.Forms.FormRemoteQuery rq = new DiversityWorkbench.Forms.FormRemoteQuery(null, wu);
                    if (rq.ShowDialog() == DialogResult.OK)
                        insertLinkAtCursor(rq.URI);
                    this.richTextBox.Focus();
                }
                catch (Exception)
                { }
            }
        }

        private void dProjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_Init)
            {
                try
                {
                    DiversityWorkbench.IWorkbenchUnit wu = new DiversityWorkbench.Project(DiversityWorkbench.Settings.ServerConnection);
                    DiversityWorkbench.Forms.FormRemoteQuery rq = new DiversityWorkbench.Forms.FormRemoteQuery(null, wu);
                    if (rq.ShowDialog() == DialogResult.OK)
                        insertLinkAtCursor(rq.URI);
                    this.richTextBox.Focus();
                }
                catch (Exception)
                { }
            }
        }

        private void dReferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_Init)
            {
                try
                {
                    DiversityWorkbench.IWorkbenchUnit wu = new DiversityWorkbench.Reference(DiversityWorkbench.Settings.ServerConnection);
                    DiversityWorkbench.Forms.FormRemoteQuery rq = new DiversityWorkbench.Forms.FormRemoteQuery(null, wu);
                    if (rq.ShowDialog() == DialogResult.OK)
                        insertLinkAtCursor(rq.URI);
                    this.richTextBox.Focus();
                }
                catch (Exception)
                { }
            }
        }

        private void dSamplingPlotsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_Init)
            {
                try
                {
                    DiversityWorkbench.IWorkbenchUnit wu = new DiversityWorkbench.SamplingPlot(DiversityWorkbench.Settings.ServerConnection);
                    DiversityWorkbench.Forms.FormRemoteQuery rq = new DiversityWorkbench.Forms.FormRemoteQuery(null, wu);
                    if (rq.ShowDialog() == DialogResult.OK)
                        insertLinkAtCursor(rq.URI);
                    this.richTextBox.Focus();
                }
                catch (Exception)
                { }
            }
        }

        private void dScientificTermsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_Init)
            {
                try
                {
                    DiversityWorkbench.IWorkbenchUnit wu = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
                    DiversityWorkbench.Forms.FormRemoteQuery rq = new DiversityWorkbench.Forms.FormRemoteQuery(null, wu);
                    if (rq.ShowDialog() == DialogResult.OK)
                        insertLinkAtCursor(rq.URI);
                    this.richTextBox.Focus();
                }
                catch (Exception)
                { }
            }
        }

        private void dTaxonNamesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_Init)
            {
                try
                {
                    DiversityWorkbench.IWorkbenchUnit wu = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
                    DiversityWorkbench.Forms.FormRemoteQuery rq = new DiversityWorkbench.Forms.FormRemoteQuery(null, wu);
                    if (rq.ShowDialog() == DialogResult.OK)
                        insertLinkAtCursor(rq.URI);
                    this.richTextBox.Focus();
                }
                catch (Exception)
                { }
            }
        }

        private void toolStripButtonWeb_Click(object sender, EventArgs e)
        {
            if (_Init)
            {
                try
                {
                    DiversityWorkbench.Forms.FormWebBrowser fw = new DiversityWorkbench.Forms.FormWebBrowser();
                    if (fw.ShowDialog() == DialogResult.OK)
                        insertLinkAtCursor(fw.URL);
                    this.richTextBox.Focus();
                }
                catch (Exception)
                { }
            }
        }

        private void UserControlRichEdit_BackColorChanged(object sender, EventArgs e)
        {
            this.richTextBox.BackColor = this.BackColor;
            this.statusStrip.BackColor = this.BackColor;
            _UseDefaultBackColor = false;
            setEditText();
        }
        #endregion
    }
}
