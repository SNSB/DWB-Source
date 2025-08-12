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
    public partial class UserControlXslAttribute : UserControl
    {

        #region Parameter
        
        private DiversityWorkbench.XslEditor.XslNode _XslNode;
        private string _Name;
        private string _Value;
        private DiversityWorkbench.XslEditor.XslAttribute _XslAttribute;
        
        #endregion        //private System.Collections.Generic.List<DiversityWorkbench.XslEditor.UserControlXslAttributeCondition> _AttributeConditions;

        #region Construction

        public UserControlXslAttribute(string Name, string Value, DiversityWorkbench.XslEditor.XslNode XslNode)
        {
            InitializeComponent();
            this.labelName.Text = Name;
            this._Name = Name;
            this._Value = Value;
            this.comboBoxValue.Text = Value;
            this._XslNode = XslNode;
            DiversityWorkbench.XslEditor.XslAttribute.AttributeType T = XslAttribute.AttributeType.match;
            if (Name == "test") T = XslAttribute.AttributeType.test;
            else if (Name == "select") T = XslAttribute.AttributeType.select;
            this._XslAttribute = new XslAttribute(T);
            //this._AttributeConditions = new List<UserControlXslAttributeCondition>();
            //this._AttributeConditions.Add(this.userControlXslAttributeCondition1);
            this.initControl();
        }
        
        #endregion

        #region Control
        
        private void initControl()
        {
            this.comboBoxValue.Items.Clear();
            //if (this._XslNode.ParentXslNode != null && this._XslAttribute.Type != XslAttribute.AttributeType.test)
            //{
            //    foreach (string s in DiversityWorkbench.XslEditor.XslEditor.XmlPathsFromRoot(this._XslNode.ParentXslNode.XmlNodePath))
            //        this.comboBoxValue.Items.Add(s);
            //}
            switch (this._XslAttribute.Type)
            {
                case XslAttribute.AttributeType.test:
                    this.comboBoxValue.Visible = false;
                    //this.userControlXslAttributeCondition1.Visible = true;
                    //this.userControlXslAttributeCondition1.Condition = this._Value;
                    this.buttonAddCondition.Visible = true;
                    this.buttonRemoveCondition.Visible = true;
                    this._XslAttribute.SetTestCondition(this._Value);
                    foreach (DiversityWorkbench.XslEditor.AttributeTestCondition A in this._XslAttribute.AttributeTestConditions)
                    {
                        this.AddCondition(A);
                    }
                    //this.AnalyseValue();
                    break;
                case XslAttribute.AttributeType.match:
                case XslAttribute.AttributeType.select:
                    this.comboBoxValue.Visible = true;
                    //this.userControlXslAttributeCondition1.Visible = false;
                    this.buttonAddCondition.Visible = false;
                    this.buttonRemoveCondition.Visible = false;
                    break;
                default:
                    break;
            }

            //switch (this._Name)
            //{
            //    case "test":
            //        this.comboBoxValue.Visible = false;
            //        this.userControlXslAttributeCondition1.Visible = true;
            //        this.userControlXslAttributeCondition1.Condition = this._Value;
            //        this.buttonAddCondition.Visible = true;
            //        this.AnalyseValue();
            //        break;
            //    case "select":
            //        this.comboBoxValue.Visible = true;
            //        this.userControlXslAttributeCondition1.Visible = false;
            //        this.buttonAddCondition.Visible = false;
            //        break;
            //    default:
            //        break;
            //}

        }

        private void comboBoxValue_Leave(object sender, EventArgs e)
        {
            if (this._XslNode.Attributes.ContainsKey(this._Name))
            {
                this._XslNode.Attributes[this._Name] = this.comboBoxValue.Text;
            }
            else if (this._XslNode.ContentParts() != null
                && this._XslNode.ContentParts().ContainsKey(this._Name))
            {
                this._XslNode.ContentParts()[this._Name] = this.comboBoxValue.Text;
                this._XslNode.Content = this._XslNode.ContentFromContentParts();
                this._XslNode.TreeNode.Text = this._XslNode.DisplayText;
            }
            if (this._XslNode.Name == null)
                this._XslNode.Content = this.comboBoxValue.Text;
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {

        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {

        }

        private void buttonAddCondition_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.XslEditor.AttributeTestCondition A = new AttributeTestCondition();
            A.BoolOperator = "and";
            this.AddCondition(A);
        }

        private void buttonRemoveCondition_Click(object sender, EventArgs e)
        {
            this.RemoveCondition();
        }

        private void AddCondition(DiversityWorkbench.XslEditor.AttributeTestCondition A)
        {
            try
            {
                DiversityWorkbench.XslEditor.UserControlXslAttributeCondition U = new UserControlXslAttributeCondition();
                U.AttributeTestCondition = A;
                U.XslNode = this._XslNode;
                this._XslAttribute.AddUserControlXslAttributeCondition(U);
                //this._AttributeConditions.Add(U);
                this.panelAttributeConditions.Controls.Add(U);
                U.BringToFront();
                U.Dock = DockStyle.Top;
                this.Height += 22;
            }
            catch (System.Exception ex) { }
        }

        private void RemoveCondition()
        {
            DiversityWorkbench.XslEditor.UserControlXslAttributeCondition U =
                (DiversityWorkbench.XslEditor.UserControlXslAttributeCondition)this.panelAttributeConditions.Controls[0];
            this._XslAttribute.RemoveUserControlXslAttributeCondition(U);
            //this._AttributeConditions.RemoveAt(this._AttributeConditions.Count - 1);
            this.panelAttributeConditions.Controls.Remove(U);
            this.Height -= 22;
        }

        private void comboBoxValue_DropDown(object sender, EventArgs e)
        {
            string Current = comboBoxValue.Text;
            System.Collections.Generic.List<string> L = new List<string>();
            if (this.comboBoxValue.DataSource == null)
            {
                switch (this._XslNode.Name.Trim())
                {
                    case "xsl:call-template":
                        break;
                    case "xsl:apply-template":
                        break;
                    case "xsl:if":
                        L = this._XslNode.XmlPathsBelowRoot;
                        break;
                    case "span":
                        switch (this._Name)
                        {
                            case "style":
                                L = this._XslNode.XslEditor.XslFonts;
                                break;
                        }
                        break;
                    case "td":
                        L = DiversityWorkbench.XslEditor.XslEditor.HtmlAttributes(this._Name.Trim());
                        break;
                    case "":
                        if (this._XslNode.Content.Length > 0 && this._Name.Length > 0)
                            L = DiversityWorkbench.XslEditor.XslEditor.HtmlAttributes(this._Name.Trim());
                        break;
                    default:
                        this.comboBoxValue.Text = Current;
                        break;
                }
            }
            //if (this.comboBoxValue.DataSource == null)
            //{
            //    switch (this._XslNode.Name.Trim())
            //    {
            //        case "xsl:call-template":
            //            break;
            //        case "xsl:apply-template":
            //            break;
            //        case "xsl:if":
            //            this.comboBoxValue.DataSource = this._XslNode.XmlPathsBelowRoot;
            //            break;
            //        case "span":
            //            switch (this._Name)
            //            {
            //                case "style":
            //                    this.comboBoxValue.DataSource = this._XslNode.XslEditor.XslFonts;
            //                    break;
            //            }
            //            break;
            //        case "td":
            //            this.comboBoxValue.DataSource = DiversityWorkbench.XslEditor.XslEditor.HtmlAttributes(this._Name.Trim());
            //            break;
            //        case "":
            //            if (this._XslNode.Content.Length > 0 && this._Name.Length > 0)
            //                this.comboBoxValue.DataSource = DiversityWorkbench.XslEditor.XslEditor.HtmlAttributes(this._Name.Trim());
            //            break;
            //        default:
            //            this.comboBoxValue.Text = Current;
            //            break;
            //    }
            //}
        }
        
        #endregion

    }
}
