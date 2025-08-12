using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.XslEditor
{
    public partial class UserControlXslAttributeCondition : UserControl
    {
        #region Parameter

        private bool _IsFirstPart = true;
        private DiversityWorkbench.XslEditor.AttributeTestCondition _AttributeTestCondition;
        private DiversityWorkbench.XslEditor.XslNode _XslNode;
        
        #endregion

        #region Construction
        
        public UserControlXslAttributeCondition()
        {
            InitializeComponent();
            this.initControl();
        }
        
        #endregion

        #region Control
        
        public void initControl()
        {
            this.comboBoxBoolOperator.Visible = false;
            this.buttonRemove.Visible = false;
            this.buttonBracketClose.Text = "";
            this.buttonBracketOpen.Text = "";
        }

        private void buttonBracketOpen_Click(object sender, EventArgs e)
        {
            if (this.buttonBracketOpen.Text == "")
                this.buttonBracketOpen.Text = "(";
            else this.buttonBracketOpen.Text = "";
        }

        private void buttonBracketClose_Click(object sender, EventArgs e)
        {
            if (this.buttonBracketClose.Text == "")
                this.buttonBracketClose.Text = ")";
            else this.buttonBracketClose.Text = "";
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {

        }
        
        private void comboBoxValue_DropDown(object sender, EventArgs e)
        {
            if (this.comboBoxValue.DataSource == null)
            {
                this.comboBoxValue.DataSource = this.XslNode.XmlPathBelowRootForValue;
            }
        }

        private void comboBoxCompareTo_DropDown(object sender, EventArgs e)
        {
            if (this.comboBoxCompareTo.DataSource == null)
                this.comboBoxCompareTo.DataSource = this.XslNode.XmlPathBelowRootForCompareTo;
        }

        private void UserControlXslAttributeCondition_Leave(object sender, EventArgs e)
        {
            System.Collections.Generic.Dictionary<string, string> ChangedAttributes = new Dictionary<string, string>();
            foreach(System.Collections.Generic.KeyValuePair<string, string> KV in this._XslNode.Attributes)
            {
                if (KV.Value == this._AttributeTestCondition.Condition)
                {
                    double Test;
                    string Condition = this.comboBoxBoolOperator.Text + this.buttonBracketOpen.Text + this.comboBoxValue.Text + this.comboBoxOperator.Text;
                    if (!double.TryParse(this.comboBoxCompareTo.Text, out Test)
                        && !this.comboBoxCompareTo.Text.StartsWith("'"))
                        Condition += "'";
                    Condition += this.comboBoxCompareTo.Text;
                    if (!double.TryParse(this.comboBoxCompareTo.Text, out Test)
                        && !this.comboBoxCompareTo.Text.EndsWith("'"))
                        Condition += "'";
                    Condition += this.buttonBracketClose.Text;
                    ChangedAttributes.Add(KV.Key, Condition);
                }
            }
            if (ChangedAttributes.Count > 0)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in ChangedAttributes)
                {
                    this._XslNode.Attributes[KV.Key] = KV.Value;
                }
            }
            //this._XslNode.Attributes[this._AttributeTestCondition.Condition]
        }

        #endregion

        #region Properties
        
        public DiversityWorkbench.XslEditor.AttributeTestCondition AttributeTestCondition
        {
            get
            {
                string Cond = "";
                if (this.comboBoxBoolOperator.Visible && this.comboBoxBoolOperator.Text.Length > 0)
                    Cond += this.comboBoxBoolOperator.Text + " ";
                Cond += this.buttonBracketOpen.Text + " "
                    + this.comboBoxValue.Text + " ";
                switch (this.comboBoxOperator.Text)
                {
                    case "<":
                        Cond += "&gt; ";
                        break;
                    case ">":
                        Cond += "&lt; ";
                        break;
                    case "≠":
                        Cond += "!= ";
                        break;
                    default:
                        Cond += this.comboBoxOperator.Text + " ";
                        break;
                }
                Cond += this.comboBoxCompareTo.Text + " "
                    + this.buttonBracketClose.Text;
                if (Cond.Replace("(", "").Replace(")", "").Trim().Length == 0)
                    Cond = "";
                return _AttributeTestCondition;
            }
            set
            {
                _AttributeTestCondition = value;
                if (_AttributeTestCondition.BoolOperator != null && _AttributeTestCondition.BoolOperator.Length > 0)
                {
                    this.comboBoxBoolOperator.Text = _AttributeTestCondition.BoolOperator;
                    this.comboBoxBoolOperator.Visible = true;
                }
                bool OperatorFound = false;
                if (_AttributeTestCondition.Condition != null && _AttributeTestCondition.Condition.Length > 0)
                {
                    foreach (string s in Operators)
                    {
                        if (_AttributeTestCondition.Condition.IndexOf(s) > -1)
                        {
                            if (s == "!=")
                                this.comboBoxOperator.Text = "≠";
                            else
                                this.comboBoxOperator.Text = s.Trim();
                            this.comboBoxValue.Text = _AttributeTestCondition.Condition.Substring(0, _AttributeTestCondition.Condition.IndexOf(s)).Trim();
                            this.comboBoxCompareTo.Text = _AttributeTestCondition.Condition.Substring(_AttributeTestCondition.Condition.IndexOf(s) + s.Length).Trim();
                            OperatorFound = true;
                            break;
                        }
                    }
                    if (!OperatorFound)
                    {
                        this.comboBoxValue.Text = _AttributeTestCondition.Condition;
                    }
                }
                this.comboBoxBoolOperator.Text = value.BoolOperator;
            }
        }
        
        public DiversityWorkbench.XslEditor.XslNode XslNode 
        {
            get 
            {
                if (this._XslNode == null)
                {
                    this._XslNode = new XslNode();
                }
                return this._XslNode; 
            } 
            set { this._XslNode = value; } 
        }

        private System.Collections.Generic.List<string> _Operators;
        private System.Collections.Generic.List<string> Operators
        {
            get
            {
                if (this._Operators == null)
                {
                    this._Operators = new List<string>();
                    this._Operators.Add("!=");
                    this._Operators.Add("≠");
                    this._Operators.Add("=");
                    this._Operators.Add("<");
                    this._Operators.Add(">");
                }
                return this._Operators;
            }
        }

        public bool IsFirstPart
        {
            set 
            { 
                this._IsFirstPart = value;
                this.comboBoxBoolOperator.Visible = value;
                //this.buttonRemove.Visible = value;
            }
        }

        #endregion

    }
}
