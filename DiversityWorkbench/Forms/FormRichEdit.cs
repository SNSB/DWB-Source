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
    public partial class FormRichEdit : Form
    {
        #region Parameter
        public bool ShowMenu
        {
            get { return this.userControlRichEdit.ShowMenu; }
            set { this.userControlRichEdit.ShowMenu = value; }
        }

        public bool ShowStatus
        {
            get { return this.userControlRichEdit.ShowStatus; }
            set { this.userControlRichEdit.ShowStatus = value; }
        }
        #endregion

        #region Construction
        /// <summary>
        /// Editor for rich text
        /// </summary>
        /// <param name="Title">Window title</param>
        /// <param name="TextToEdit">Text to be edited</param>
        /// <param name="ReadOnly">'true' if text editin is not allowed</param>
        public FormRichEdit(string Title, string TextToEdit, bool ReadOnly = false)
        {
            InitializeComponent();
            this.Text = Title;
            this.userControlRichEdit.SetControl();
            this.userControlRichEdit.ReadOnly = ReadOnly;
            this.userControlRichEdit.EditText = TextToEdit;
            this.userControlRichEdit.ShowMenu = !ReadOnly;
            this.userControlRichEdit.ShowStatus = false;
            if (ReadOnly)
            {
                this.userControlDialogPanel.Visible = false;
                if (Title.StartsWith("Edit "))
                    this.Text = Title.Substring(5);
            }
        }

        /// <summary>
        /// Editor for molecular sequence texts
        /// </summary>
        /// <param name="Title">Window title</param>
        /// <param name="TextToEdit">Text to be edited</param>
        /// <param name="SequenceType">'Nucleotie' or 'Protein'</param>
        /// <param name="SymbolLength">1 or 3 for protein sequences, 1 for nucleotide sequences</param>
        /// <param name="GapSymbol">>A string with symbolLength representing a gap of unspecified length. Do not mix up with 'unknown' symbols! NULL if no gap symbol is supported.</param>
        /// <param name="EnableAbiguity">'true' if ambiguity symbols are allowed.</param>
        /// <param name="Symbols">A dictionary of the symbols and their long text.</param>
        /// <param name="AmbiguitySymbols">A dictionary of the ambiguity symbols and their long text.</param>
        /// <param name="ReadOnly">'true' if text editin is not allowed</param>
        public FormRichEdit(string Title, string TextToEdit, string SequenceType, int SymbolLength, string GapSymbol, bool EnableAbiguity, Dictionary<string, string> Symbols, Dictionary<string, string> AmbiguitySymbols, bool ReadOnly = false)
            : this(Title, TextToEdit, ReadOnly)
        {
            this.userControlRichEdit.SetControl(SequenceType, SymbolLength, GapSymbol, EnableAbiguity, Symbols, AmbiguitySymbols);
            this.userControlRichEdit.ShowStatus = true;
            this.Icon = Properties.Resources.DscrSequWr;
        }
        #endregion

        #region Public
        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void SetHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        /// <summary>
        /// Set the back color of the edit window
        /// </summary>
        /// <param name="backColor">Back color</param>
        public void SetBackColor(Color backColor)
        {
            this.userControlRichEdit.BackColor = backColor;
        }
        #endregion

        #region Properties
        public string EditedText { get { return this.userControlRichEdit.EditText; } }
        #endregion

        #region Events
        private void FormRichEdit_Shown(object sender, EventArgs e)
        {
            this.userControlRichEdit.Focus();
        }
        #endregion
    }
}
