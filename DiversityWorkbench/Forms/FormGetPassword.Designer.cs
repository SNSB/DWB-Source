namespace DiversityWorkbench.Forms
{
    partial class FormGetPassword
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGetPassword));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.checkBoxShowSigns = new System.Windows.Forms.CheckBox();
            this.labelUser = new System.Windows.Forms.Label();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxPassword, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.checkBoxShowSigns, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.labelUser, 0, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 4;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(217, 100);
            this.tableLayoutPanel.TabIndex = 1;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(3, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(211, 27);
            this.labelHeader.TabIndex = 0;
            this.labelHeader.Text = "Please enter password for user";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPassword.Location = new System.Drawing.Point(3, 49);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(211, 20);
            this.textBoxPassword.TabIndex = 1;
            // 
            // checkBoxShowSigns
            // 
            this.checkBoxShowSigns.AutoSize = true;
            this.checkBoxShowSigns.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBoxShowSigns.Location = new System.Drawing.Point(3, 75);
            this.checkBoxShowSigns.Name = "checkBoxShowSigns";
            this.checkBoxShowSigns.Size = new System.Drawing.Size(211, 17);
            this.checkBoxShowSigns.TabIndex = 2;
            this.checkBoxShowSigns.Text = "Show signs";
            this.checkBoxShowSigns.UseVisualStyleBackColor = true;
            this.checkBoxShowSigns.Click += new System.EventHandler(this.checkBoxShowSigns_Click);
            // 
            // labelUser
            // 
            this.labelUser.AutoSize = true;
            this.labelUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelUser.Location = new System.Drawing.Point(3, 30);
            this.labelUser.Margin = new System.Windows.Forms.Padding(3);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(211, 13);
            this.labelUser.TabIndex = 3;
            this.labelUser.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 100);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(217, 27);
            this.userControlDialogPanel.TabIndex = 0;
            // 
            // FormGetPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(217, 127);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.userControlDialogPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormGetPassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Password ?";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.CheckBox checkBoxShowSigns;
        private System.Windows.Forms.Label labelUser;
    }
}