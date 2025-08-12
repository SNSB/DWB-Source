namespace DiversityWorkbench.Forms
{
    partial class FormGetSelectionFromCheckedList
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGetSelectionFromCheckedList));
            this.labelHeader = new System.Windows.Forms.Label();
            this.checkedListBox = new System.Windows.Forms.CheckedListBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonCheckAll = new System.Windows.Forms.Button();
            this.buttonCheckNone = new System.Windows.Forms.Button();
            this.labelExclude = new System.Windows.Forms.Label();
            this.textBoxExclude = new System.Windows.Forms.TextBox();
            this.buttonRequery = new System.Windows.Forms.Button();
            this.labelCounter = new System.Windows.Forms.Label();
            this.labelRestrict = new System.Windows.Forms.Label();
            this.textBoxRestrict = new System.Windows.Forms.TextBox();
            this.buttonInclude = new System.Windows.Forms.Button();
            this.labelSelect = new System.Windows.Forms.Label();
            this.textBoxSelect = new System.Windows.Forms.TextBox();
            this.buttonSelect = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.labelRemove = new System.Windows.Forms.Label();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.textBoxRemove = new System.Windows.Forms.TextBox();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.labelHeader, 2);
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(3, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(220, 24);
            this.labelHeader.TabIndex = 1;
            this.labelHeader.Text = "label1";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkedListBox
            // 
            this.checkedListBox.CheckOnClick = true;
            this.tableLayoutPanel1.SetColumnSpan(this.checkedListBox, 3);
            this.checkedListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox.FormattingEnabled = true;
            this.checkedListBox.IntegralHeight = false;
            this.checkedListBox.Location = new System.Drawing.Point(3, 56);
            this.checkedListBox.Name = "checkedListBox";
            this.checkedListBox.Size = new System.Drawing.Size(301, 275);
            this.checkedListBox.TabIndex = 2;
            this.checkedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox_ItemCheck);
            this.checkedListBox.Click += new System.EventHandler(this.checkedListBox_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.checkedListBox, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonCheckAll, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonCheckNone, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelExclude, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.textBoxExclude, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.buttonRequery, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelCounter, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelRestrict, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.textBoxRestrict, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.buttonInclude, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.labelSelect, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.textBoxSelect, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.buttonSelect, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.buttonReset, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelRemove, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.buttonRemove, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.textBoxRemove, 1, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(307, 438);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // buttonCheckAll
            // 
            this.buttonCheckAll.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonCheckAll.Location = new System.Drawing.Point(3, 27);
            this.buttonCheckAll.Name = "buttonCheckAll";
            this.buttonCheckAll.Size = new System.Drawing.Size(75, 23);
            this.buttonCheckAll.TabIndex = 3;
            this.buttonCheckAll.Text = "Check all";
            this.toolTip.SetToolTip(this.buttonCheckAll, "Select all entries");
            this.buttonCheckAll.UseVisualStyleBackColor = true;
            this.buttonCheckAll.Click += new System.EventHandler(this.buttonCheckAll_Click);
            // 
            // buttonCheckNone
            // 
            this.buttonCheckNone.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonCheckNone.Location = new System.Drawing.Point(229, 27);
            this.buttonCheckNone.Name = "buttonCheckNone";
            this.buttonCheckNone.Size = new System.Drawing.Size(75, 23);
            this.buttonCheckNone.TabIndex = 4;
            this.buttonCheckNone.Text = "Check none";
            this.buttonCheckNone.UseVisualStyleBackColor = true;
            this.buttonCheckNone.Click += new System.EventHandler(this.buttonCheckNone_Click);
            // 
            // labelExclude
            // 
            this.labelExclude.AutoSize = true;
            this.labelExclude.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelExclude.Location = new System.Drawing.Point(47, 334);
            this.labelExclude.Name = "labelExclude";
            this.labelExclude.Size = new System.Drawing.Size(48, 26);
            this.labelExclude.TabIndex = 5;
            this.labelExclude.Text = "Exclude:";
            this.labelExclude.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxExclude
            // 
            this.textBoxExclude.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxExclude.Location = new System.Drawing.Point(101, 337);
            this.textBoxExclude.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.textBoxExclude.Name = "textBoxExclude";
            this.textBoxExclude.Size = new System.Drawing.Size(122, 20);
            this.textBoxExclude.TabIndex = 6;
            this.textBoxExclude.Text = "*_log";
            this.toolTip.SetToolTip(this.textBoxExclude, "Exclude all entries like the entered text (use * as wildcard)");
            // 
            // buttonRequery
            // 
            this.buttonRequery.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonRequery.Location = new System.Drawing.Point(229, 337);
            this.buttonRequery.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.buttonRequery.Name = "buttonRequery";
            this.buttonRequery.Size = new System.Drawing.Size(75, 23);
            this.buttonRequery.TabIndex = 7;
            this.buttonRequery.Text = "Requery";
            this.buttonRequery.UseVisualStyleBackColor = true;
            this.buttonRequery.Click += new System.EventHandler(this.buttonRequery_Click);
            // 
            // labelCounter
            // 
            this.labelCounter.AutoSize = true;
            this.labelCounter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCounter.Location = new System.Drawing.Point(101, 24);
            this.labelCounter.Name = "labelCounter";
            this.labelCounter.Size = new System.Drawing.Size(122, 29);
            this.labelCounter.TabIndex = 8;
            this.labelCounter.Text = "0";
            this.labelCounter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelRestrict
            // 
            this.labelRestrict.AutoSize = true;
            this.labelRestrict.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRestrict.Location = new System.Drawing.Point(3, 363);
            this.labelRestrict.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.labelRestrict.Name = "labelRestrict";
            this.labelRestrict.Size = new System.Drawing.Size(92, 23);
            this.labelRestrict.TabIndex = 9;
            this.labelRestrict.Text = "Restrict to:";
            this.labelRestrict.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxRestrict
            // 
            this.textBoxRestrict.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRestrict.Location = new System.Drawing.Point(101, 360);
            this.textBoxRestrict.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.textBoxRestrict.Name = "textBoxRestrict";
            this.textBoxRestrict.Size = new System.Drawing.Size(122, 20);
            this.textBoxRestrict.TabIndex = 10;
            this.toolTip.SetToolTip(this.textBoxRestrict, "Restrict to entries like the entered text (use * as wildcard)");
            // 
            // buttonInclude
            // 
            this.buttonInclude.Location = new System.Drawing.Point(229, 360);
            this.buttonInclude.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.buttonInclude.Name = "buttonInclude";
            this.buttonInclude.Size = new System.Drawing.Size(75, 23);
            this.buttonInclude.TabIndex = 11;
            this.buttonInclude.Text = "Requery";
            this.buttonInclude.UseVisualStyleBackColor = true;
            this.buttonInclude.Click += new System.EventHandler(this.buttonInclude_Click);
            // 
            // labelSelect
            // 
            this.labelSelect.AutoSize = true;
            this.labelSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSelect.Location = new System.Drawing.Point(3, 386);
            this.labelSelect.Name = "labelSelect";
            this.labelSelect.Size = new System.Drawing.Size(92, 26);
            this.labelSelect.TabIndex = 12;
            this.labelSelect.Text = "Add to selection:";
            this.labelSelect.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.labelSelect, "Select all entries matching the search string (use * as wildcard)");
            // 
            // textBoxSelect
            // 
            this.textBoxSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSelect.Location = new System.Drawing.Point(101, 389);
            this.textBoxSelect.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.textBoxSelect.Name = "textBoxSelect";
            this.textBoxSelect.Size = new System.Drawing.Size(122, 20);
            this.textBoxSelect.TabIndex = 13;
            // 
            // buttonSelect
            // 
            this.buttonSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSelect.Image = global::DiversityWorkbench.Properties.Resources.Add;
            this.buttonSelect.Location = new System.Drawing.Point(229, 389);
            this.buttonSelect.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(75, 23);
            this.buttonSelect.TabIndex = 14;
            this.toolTip.SetToolTip(this.buttonSelect, "Select all entries matching the search string (use * as wildcard) ");
            this.buttonSelect.UseVisualStyleBackColor = true;
            this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonReset.Image = global::DiversityWorkbench.Properties.Resources.Transfrom;
            this.buttonReset.Location = new System.Drawing.Point(283, 0);
            this.buttonReset.Margin = new System.Windows.Forms.Padding(0);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(24, 24);
            this.buttonReset.TabIndex = 15;
            this.toolTip.SetToolTip(this.buttonReset, "Reset to original list");
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // labelRemove
            // 
            this.labelRemove.AutoSize = true;
            this.labelRemove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRemove.Location = new System.Drawing.Point(3, 415);
            this.labelRemove.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.labelRemove.Name = "labelRemove";
            this.labelRemove.Size = new System.Drawing.Size(92, 23);
            this.labelRemove.TabIndex = 16;
            this.labelRemove.Text = "Remove from sel.:";
            this.labelRemove.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.toolTip.SetToolTip(this.labelRemove, "Remove items matching the search string from the selection (use * as wildcard)");
            // 
            // buttonRemove
            // 
            this.buttonRemove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRemove.Image = global::DiversityWorkbench.Properties.Resources.Minus;
            this.buttonRemove.Location = new System.Drawing.Point(229, 412);
            this.buttonRemove.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(75, 23);
            this.buttonRemove.TabIndex = 17;
            this.toolTip.SetToolTip(this.buttonRemove, "Remove items matching the search string from the selection (use * as wildcard)");
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // textBoxRemove
            // 
            this.textBoxRemove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRemove.Location = new System.Drawing.Point(101, 412);
            this.textBoxRemove.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.textBoxRemove.Name = "textBoxRemove";
            this.textBoxRemove.Size = new System.Drawing.Size(122, 20);
            this.textBoxRemove.TabIndex = 18;
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(4, 442);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(307, 27);
            this.userControlDialogPanel.TabIndex = 0;
            // 
            // FormGetSelectionFromCheckedList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 473);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.userControlDialogPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormGetSelectionFromCheckedList";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormGetSelectionFromCheckedList";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.CheckedListBox checkedListBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonCheckAll;
        private System.Windows.Forms.Button buttonCheckNone;
        private System.Windows.Forms.Label labelExclude;
        private System.Windows.Forms.TextBox textBoxExclude;
        private System.Windows.Forms.Button buttonRequery;
        private System.Windows.Forms.Label labelCounter;
        private System.Windows.Forms.Label labelRestrict;
        private System.Windows.Forms.TextBox textBoxRestrict;
        private System.Windows.Forms.Button buttonInclude;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelSelect;
        private System.Windows.Forms.TextBox textBoxSelect;
        private System.Windows.Forms.Button buttonSelect;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Label labelRemove;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.TextBox textBoxRemove;
    }
}