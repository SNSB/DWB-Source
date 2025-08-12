namespace DiversityWorkbench.Forms
{
    partial class FormGetString
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelHeader = new System.Windows.Forms.Label();
            this.textBoxString = new System.Windows.Forms.TextBox();
            this.userControlDialogPanel1 = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(3, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.labelHeader.Size = new System.Drawing.Size(243, 21);
            this.labelHeader.TabIndex = 1;
            this.labelHeader.Text = "please enter string";
            // 
            // textBoxString
            // 
            this.textBoxString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxString.Location = new System.Drawing.Point(3, 24);
            this.textBoxString.Margin = new System.Windows.Forms.Padding(3, 3, 3, 9);
            this.textBoxString.Multiline = true;
            this.textBoxString.Name = "textBoxString";
            this.textBoxString.Size = new System.Drawing.Size(243, 56);
            this.textBoxString.TabIndex = 2;
            // 
            // userControlDialogPanel1
            // 
            this.userControlDialogPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel1.Location = new System.Drawing.Point(4, 93);
            this.userControlDialogPanel1.Name = "userControlDialogPanel1";
            this.userControlDialogPanel1.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel1.Size = new System.Drawing.Size(249, 27);
            this.userControlDialogPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxString, 0, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(249, 89);
            this.tableLayoutPanel.TabIndex = 3;
            // 
            // FormGetString
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(257, 124);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.userControlDialogPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormGetString";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Enter string";
            this.Shown += new System.EventHandler(this.FormGetString_Shown);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel1;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.TextBox textBoxString;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}