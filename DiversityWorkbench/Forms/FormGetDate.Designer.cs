namespace DiversityWorkbench.Forms
{
    partial class FormGetDate
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
            this.dateTimePickerDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerTime = new System.Windows.Forms.DateTimePicker();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.buttonShowTime = new System.Windows.Forms.Button();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.dateTimePickerTimeEnd = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerDateEnd = new System.Windows.Forms.DateTimePicker();
            this.labelStart = new System.Windows.Forms.Label();
            this.labelEnd = new System.Windows.Forms.Label();
            this.labelHeader = new System.Windows.Forms.Label();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // dateTimePickerDate
            // 
            this.dateTimePickerDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePickerDate.Location = new System.Drawing.Point(35, 20);
            this.dateTimePickerDate.Margin = new System.Windows.Forms.Padding(0);
            this.dateTimePickerDate.Name = "dateTimePickerDate";
            this.dateTimePickerDate.Size = new System.Drawing.Size(171, 20);
            this.dateTimePickerDate.TabIndex = 1;
            // 
            // dateTimePickerTime
            // 
            this.dateTimePickerTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePickerTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePickerTime.Location = new System.Drawing.Point(206, 20);
            this.dateTimePickerTime.Margin = new System.Windows.Forms.Padding(0);
            this.dateTimePickerTime.Name = "dateTimePickerTime";
            this.dateTimePickerTime.Size = new System.Drawing.Size(81, 20);
            this.dateTimePickerTime.TabIndex = 2;
            // 
            // buttonShowTime
            // 
            this.buttonShowTime.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonShowTime.FlatAppearance.BorderSize = 0;
            this.buttonShowTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonShowTime.Image = global::DiversityWorkbench.Properties.Resources.Time;
            this.buttonShowTime.Location = new System.Drawing.Point(267, 0);
            this.buttonShowTime.Margin = new System.Windows.Forms.Padding(0);
            this.buttonShowTime.Name = "buttonShowTime";
            this.helpProvider.SetShowHelp(this.buttonShowTime, true);
            this.buttonShowTime.Size = new System.Drawing.Size(20, 20);
            this.buttonShowTime.TabIndex = 8;
            this.buttonShowTime.UseVisualStyleBackColor = true;
            this.buttonShowTime.Click += new System.EventHandler(this.buttonShowTime_Click);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.dateTimePickerTimeEnd, 2, 2);
            this.tableLayoutPanel.Controls.Add(this.dateTimePickerDateEnd, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.dateTimePickerDate, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.dateTimePickerTime, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.labelStart, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.labelEnd, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonShowTime, 2, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(287, 41);
            this.tableLayoutPanel.TabIndex = 3;
            // 
            // dateTimePickerTimeEnd
            // 
            this.dateTimePickerTimeEnd.Dock = System.Windows.Forms.DockStyle.Top;
            this.dateTimePickerTimeEnd.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePickerTimeEnd.Location = new System.Drawing.Point(206, 40);
            this.dateTimePickerTimeEnd.Margin = new System.Windows.Forms.Padding(0);
            this.dateTimePickerTimeEnd.Name = "dateTimePickerTimeEnd";
            this.dateTimePickerTimeEnd.Size = new System.Drawing.Size(81, 20);
            this.dateTimePickerTimeEnd.TabIndex = 4;
            this.dateTimePickerTimeEnd.Visible = false;
            // 
            // dateTimePickerDateEnd
            // 
            this.dateTimePickerDateEnd.Dock = System.Windows.Forms.DockStyle.Top;
            this.dateTimePickerDateEnd.Location = new System.Drawing.Point(35, 40);
            this.dateTimePickerDateEnd.Margin = new System.Windows.Forms.Padding(0);
            this.dateTimePickerDateEnd.Name = "dateTimePickerDateEnd";
            this.dateTimePickerDateEnd.Size = new System.Drawing.Size(171, 20);
            this.dateTimePickerDateEnd.TabIndex = 3;
            this.dateTimePickerDateEnd.Visible = false;
            // 
            // labelStart
            // 
            this.labelStart.AutoSize = true;
            this.labelStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelStart.Location = new System.Drawing.Point(3, 20);
            this.labelStart.Name = "labelStart";
            this.labelStart.Size = new System.Drawing.Size(29, 20);
            this.labelStart.TabIndex = 5;
            this.labelStart.Text = "Start";
            this.labelStart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelStart.Visible = false;
            // 
            // labelEnd
            // 
            this.labelEnd.AutoSize = true;
            this.labelEnd.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelEnd.Location = new System.Drawing.Point(3, 43);
            this.labelEnd.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.labelEnd.Name = "labelEnd";
            this.labelEnd.Size = new System.Drawing.Size(29, 1);
            this.labelEnd.TabIndex = 6;
            this.labelEnd.Text = "End";
            this.labelEnd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelEnd.Visible = false;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelHeader, 2);
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(3, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(200, 20);
            this.labelHeader.TabIndex = 7;
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 41);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(287, 27);
            this.userControlDialogPanel.TabIndex = 0;
            // 
            // FormGetDate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(287, 68);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.userControlDialogPanel);
            this.Name = "FormGetDate";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set date";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.DateTimePicker dateTimePickerDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerTime;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.DateTimePicker dateTimePickerTimeEnd;
        private System.Windows.Forms.DateTimePicker dateTimePickerDateEnd;
        private System.Windows.Forms.Label labelStart;
        private System.Windows.Forms.Label labelEnd;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Button buttonShowTime;
    }
}