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
    public partial class UserControlXslTableField : UserControl
    {
        #region Parameter

        private DiversityWorkbench.XslEditor.XslNode _XslNode;
        
        #endregion

        #region Construction
        
        public UserControlXslTableField(DiversityWorkbench.XslEditor.XslNode XslNode)
        {
            InitializeComponent();
            this._XslNode = XslNode;
            this.setControls();
        }
        
        #endregion

        #region Controls

        private void setControls()
        {
            if (this._XslNode.Content != null && this._XslNode.Content.Trim().Length > 0)
            {
                System.Windows.Forms.Label Lvalue = new Label();
                Lvalue.Text = this._XslNode.Content.Trim();
                this.Controls.Add(Lvalue);
                Lvalue.Dock = DockStyle.Top;
            }
            if (this._XslNode.XslNodes.Count > 0)
            {
                System.Windows.Forms.TreeView TV = new TreeView();
                TV.Scrollable = true;
                DiversityWorkbench.XslEditor.XslEditor.CreateNodeHierarchy(this._XslNode.XslNodes[0], TV, this._XslNode.XslEditor);
                TV.ExpandAll();
                this.Controls.Add(TV);
                TV.Dock = DockStyle.Fill;
                TV.BringToFront();
            }
            //if (this._XslNode.Value != null && this._XslNode.Value.Trim().Length > 0)
            //{
            //    System.Windows.Forms.Label Lvalue = new Label();
            //    Lvalue.Text = this._XslNode.Value.Trim();
            //    this.Controls.Add(Lvalue);
            //    Lvalue.Dock = DockStyle.Top;
            //}
            //if (this._XslNode.XslNodes.Count > 0)
            //{
            //    if (this._XslNode.XslNodes[0].XslNodeType.StartsWith("xsl:"))
            //    {
            //        System.Windows.Forms.TreeView TV = new TreeView();
            //        DiversityWorkbench.XslEditor.XslEditor.CreateNodeHierarchy(this._XslNode.XslNodes[0], TV);
            //        this.Controls.Add(TV);
            //        TV.Dock = DockStyle.Fill;
            //    }
            //}
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.XslEditor.FormXslNodeEditor F = new FormXslNodeEditor(this._XslNode);
            F.ShowDialog();
        }
        
        #endregion

        #region Properties
        
        public int SpanColumn
        {
            get
            {
                int i = 1;
                if (this._XslNode.Attributes != null && this._XslNode.Attributes.Count > 0)
                {
                    if (this._XslNode.Attributes.ContainsKey("colspan"))
                        int.TryParse(this._XslNode.Attributes["colspan"], out i);
                }
                return i;
            }
        }
        
        public int SpanRow
        {
            get
            {
                int i = 1;
                if (this._XslNode.Attributes != null && this._XslNode.Attributes.Count > 0)
                {
                    if (this._XslNode.Attributes.ContainsKey("rowspan"))
                        int.TryParse(this._XslNode.Attributes["rowspan"], out i);
                }
                return i;
            }
        }
        
        #endregion

    }
}
