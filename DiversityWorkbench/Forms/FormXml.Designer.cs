namespace DiversityWorkbench.Forms
{
    partial class FormXml
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.userControlXMLTree = new DiversityWorkbench.UserControls.UserControlXMLTree();
            this.SuspendLayout();
            // 
            // userControlXMLTree
            // 
            this.userControlXMLTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlXMLTree.Location = new System.Drawing.Point(0, 0);
            this.userControlXMLTree.Name = "userControlXMLTree";
            this.userControlXMLTree.Size = new System.Drawing.Size(284, 262);
            this.userControlXMLTree.TabIndex = 0;
            this.userControlXMLTree.XML = "";
            // 
            // FormXml
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.userControlXMLTree);
            this.Name = "FormXml";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "XML";
            this.ResumeLayout(false);

        }

        #endregion

        private UserControls.UserControlXMLTree userControlXMLTree;
    }
}