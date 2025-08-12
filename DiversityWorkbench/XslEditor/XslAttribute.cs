using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.XslEditor
{

    public struct AttributeTestCondition
    {
        public string BoolOperator;
        public string Condition;
    }

    public class XslAttribute
    {

        #region Parameter & Properties
        
        public enum AttributeType { match, test, select };

        private System.Collections.Generic.List<AttributeTestCondition> _AttributeTestConditions;
        public System.Collections.Generic.List<AttributeTestCondition> AttributeTestConditions
        {
            get
            {
                if (this._AttributeTestConditions == null)
                    this._AttributeTestConditions = new List<AttributeTestCondition>();
                return _AttributeTestConditions;
            }
            //set { _AttributeTestConditions = value; }
        }

        private System.Collections.Generic.List<DiversityWorkbench.XslEditor.UserControlXslAttributeCondition> _UserControlAttributeConditions;
        public System.Collections.Generic.List<DiversityWorkbench.XslEditor.UserControlXslAttributeCondition> UserControlAttributeConditions
        {
            get
            {
                if (this._UserControlAttributeConditions == null)
                    this._UserControlAttributeConditions = new List<UserControlXslAttributeCondition>();
                return _UserControlAttributeConditions;
            }
            //set { _AttributeConditions = value; }
        }

        private AttributeType _Type;
        public AttributeType Type { get { return this._Type; } }

        private System.Collections.Generic.Dictionary<string, string> _BoolOperators;

        public System.Collections.Generic.Dictionary<string, string> BoolOperators
        {
            get 
            {
                if (this._BoolOperators == null)
                {
                    this._BoolOperators = new Dictionary<string, string>();
                    this._BoolOperators.Add(" and not ", "and not");
                    this._BoolOperators.Add(" and not(", "and not");
                    this._BoolOperators.Add(")and not ", "and not");
                    this._BoolOperators.Add(")and not(", "and not");

                    this._BoolOperators.Add(" or not ", "or not");
                    this._BoolOperators.Add(" or not(", "or not");
                    this._BoolOperators.Add(")or not ", "or not");
                    this._BoolOperators.Add(")or not(", "or not");

                    this._BoolOperators.Add(" and ", "and");
                    this._BoolOperators.Add(" and(", "and");
                    this._BoolOperators.Add(")and ", "and");
                    this._BoolOperators.Add(")and(", "and");

                    this._BoolOperators.Add(" or ", "or");
                    this._BoolOperators.Add(" or(", "or");
                    this._BoolOperators.Add(")or ", "or");
                    this._BoolOperators.Add(")or(", "or");
                }
                return _BoolOperators; 
            }
        }
        
        #endregion

        #region Construction

        public XslAttribute(AttributeType Type)
        {
            this._Type = Type;
        }
        
        #endregion

        public void AddUserControlXslAttributeCondition(DiversityWorkbench.XslEditor.UserControlXslAttributeCondition U)
        {
            if (this._UserControlAttributeConditions == null)
                this._UserControlAttributeConditions = new List<UserControlXslAttributeCondition>();
            this._UserControlAttributeConditions.Add(U);
        }

        public void RemoveUserControlXslAttributeCondition(DiversityWorkbench.XslEditor.UserControlXslAttributeCondition U)
        {
            if (this._UserControlAttributeConditions != null && this._UserControlAttributeConditions.Contains(U))
                this._UserControlAttributeConditions.Remove(U);
        }

        public void SetTestCondition(string Condition)
        {
            string Cond = Condition;
            AttributeTestCondition Astart = new AttributeTestCondition();
            Astart.BoolOperator = "";
            Astart.Condition = Cond;
            this.CutOfRestCondition(ref Astart.Condition);
            this.AttributeTestConditions.Add(Astart);


            for (int i = 0; i < Cond.Length; i++)
            {
                //bool NextTestStartFound = false;
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in BoolOperators)
                {
                    if (Cond.Substring(i).StartsWith(KV.Key))
                    {
                        AttributeTestCondition A = new AttributeTestCondition();
                        A.BoolOperator = KV.Value;

                        // cutting of the rest if another bool operator follows
                        string Acondition = Cond.Substring(i + KV.Key.Length - 1);
                        if (!KV.Key.EndsWith("("))
                            Acondition = Acondition.Substring(1);
                        A.Condition = Acondition;
                        this.CutOfRestCondition(ref A.Condition);

                        this.AttributeTestConditions.Add(A);
                        break;
                    }
                }
            }

        }

        private void CutOfRestCondition(ref string Condition)
        {
            for (int a = 0; a < Condition.Length; a++)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> aKV in BoolOperators)
                {
                    if (Condition.Substring(a).StartsWith(aKV.Key))
                    {
                        Condition = Condition.Substring(0, Condition.IndexOf(aKV.Key));
                        //NextTestStartFound = true;
                        if (aKV.Key.StartsWith(")"))
                            Condition += ")";
                        //if (aKV.Key.EndsWith("("))
                        //    Condition = "(" + Condition;
                        break;
                    }
                }
            }
        }

    }
}
