namespace DiversityWorkbench.Forms
{
    partial class FormPassword
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPassword));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelHaeder = new System.Windows.Forms.Label();
            this.labelOldPW = new System.Windows.Forms.Label();
            this.labelNewPW1 = new System.Windows.Forms.Label();
            this.labelNewPW2 = new System.Windows.Forms.Label();
            this.textBoxOldPW = new System.Windows.Forms.TextBox();
            this.textBoxNewPW1 = new System.Windows.Forms.TextBox();
            this.textBoxNewPW2 = new System.Windows.Forms.TextBox();
            this.labelLoginInfo = new System.Windows.Forms.Label();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.labelHaeder, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelOldPW, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.labelNewPW1, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.labelNewPW2, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.textBoxOldPW, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.textBoxNewPW1, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.textBoxNewPW2, 1, 4);
            this.tableLayoutPanel.Controls.Add(this.labelLoginInfo, 0, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 5;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(417, 117);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelHaeder
            // 
            this.labelHaeder.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelHaeder, 2);
            this.labelHaeder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHaeder.Location = new System.Drawing.Point(3, 0);
            this.labelHaeder.Name = "labelHaeder";
            this.labelHaeder.Size = new System.Drawing.Size(411, 19);
            this.labelHaeder.TabIndex = 0;
            this.labelHaeder.Text = "Please enter your new password";
            this.labelHaeder.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelOldPW
            // 
            this.labelOldPW.AutoSize = true;
            this.labelOldPW.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOldPW.Location = new System.Drawing.Point(3, 39);
            this.labelOldPW.Name = "labelOldPW";
            this.labelOldPW.Size = new System.Drawing.Size(93, 26);
            this.labelOldPW.TabIndex = 1;
            this.labelOldPW.Text = "Old password:";
            this.labelOldPW.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelNewPW1
            // 
            this.labelNewPW1.AutoSize = true;
            this.labelNewPW1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNewPW1.Location = new System.Drawing.Point(3, 65);
            this.labelNewPW1.Name = "labelNewPW1";
            this.labelNewPW1.Size = new System.Drawing.Size(93, 26);
            this.labelNewPW1.TabIndex = 2;
            this.labelNewPW1.Text = "New password:";
            this.labelNewPW1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelNewPW2
            // 
            this.labelNewPW2.AutoSize = true;
            this.labelNewPW2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNewPW2.Location = new System.Drawing.Point(3, 91);
            this.labelNewPW2.Name = "labelNewPW2";
            this.labelNewPW2.Size = new System.Drawing.Size(93, 26);
            this.labelNewPW2.TabIndex = 3;
            this.labelNewPW2.Text = "Repeat password:";
            this.labelNewPW2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxOldPW
            // 
            this.textBoxOldPW.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxOldPW.Location = new System.Drawing.Point(102, 42);
            this.textBoxOldPW.Name = "textBoxOldPW";
            this.textBoxOldPW.PasswordChar = '*';
            this.textBoxOldPW.Size = new System.Drawing.Size(312, 20);
            this.textBoxOldPW.TabIndex = 4;
            this.textBoxOldPW.TextChanged += new System.EventHandler(this.textBoxOldPW_TextChanged);
            // 
            // textBoxNewPW1
            // 
            this.textBoxNewPW1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxNewPW1.Location = new System.Drawing.Point(102, 68);
            this.textBoxNewPW1.Name = "textBoxNewPW1";
            this.textBoxNewPW1.PasswordChar = '*';
            this.textBoxNewPW1.Size = new System.Drawing.Size(312, 20);
            this.textBoxNewPW1.TabIndex = 5;
            this.textBoxNewPW1.TextChanged += new System.EventHandler(this.textBoxNewPW1_TextChanged);
            // 
            // textBoxNewPW2
            // 
            this.textBoxNewPW2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxNewPW2.Location = new System.Drawing.Point(102, 94);
            this.textBoxNewPW2.Name = "textBoxNewPW2";
            this.textBoxNewPW2.PasswordChar = '*';
            this.textBoxNewPW2.Size = new System.Drawing.Size(312, 20);
            this.textBoxNewPW2.TabIndex = 6;
            this.textBoxNewPW2.TextChanged += new System.EventHandler(this.textBoxNewPW2_TextChanged);
            // 
            // labelLoginInfo
            // 
            this.labelLoginInfo.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelLoginInfo, 2);
            this.labelLoginInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLoginInfo.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.labelLoginInfo.Location = new System.Drawing.Point(3, 19);
            this.labelLoginInfo.Name = "labelLoginInfo";
            this.labelLoginInfo.Size = new System.Drawing.Size(411, 20);
            this.labelLoginInfo.TabIndex = 7;
            this.labelLoginInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 117);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(417, 27);
            this.userControlDialogPanel.TabIndex = 1;
            // 
            // FormPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 144);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.userControlDialogPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormPassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change password";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelHaeder;
        private System.Windows.Forms.Label labelOldPW;
        private System.Windows.Forms.Label labelNewPW1;
        private System.Windows.Forms.Label labelNewPW2;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.TextBox textBoxOldPW;
        private System.Windows.Forms.TextBox textBoxNewPW1;
        private System.Windows.Forms.TextBox textBoxNewPW2;
        private System.Windows.Forms.Label labelLoginInfo;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}