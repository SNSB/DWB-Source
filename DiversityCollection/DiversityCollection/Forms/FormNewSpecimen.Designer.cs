namespace DiversityCollection.Forms
{
    partial class FormNewSpecimen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNewSpecimen));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.buttonSearchForNewAccessionNumber = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonCreateNewSpecimen = new System.Windows.Forms.Button();
            this.labelHeader = new System.Windows.Forms.Label();
            this.labelAccessionNumber = new System.Windows.Forms.Label();
            this.textBoxAccessionNumber = new System.Windows.Forms.TextBox();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 2;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.Controls.Add(this.buttonSearchForNewAccessionNumber, 1, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonCancel, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.buttonCreateNewSpecimen, 1, 3);
            this.tableLayoutPanelMain.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.labelAccessionNumber, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxAccessionNumber, 0, 2);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 4;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(483, 315);
            this.tableLayoutPanelMain.TabIndex = 1;
            // 
            // buttonSearchForNewAccessionNumber
            // 
            this.buttonSearchForNewAccessionNumber.Location = new System.Drawing.Point(244, 23);
            this.buttonSearchForNewAccessionNumber.Name = "buttonSearchForNewAccessionNumber";
            this.buttonSearchForNewAccessionNumber.Size = new System.Drawing.Size(173, 46);
            this.buttonSearchForNewAccessionNumber.TabIndex = 0;
            this.buttonSearchForNewAccessionNumber.Text = "Search for a new accession number";
            this.buttonSearchForNewAccessionNumber.UseVisualStyleBackColor = true;
            this.buttonSearchForNewAccessionNumber.Click += new System.EventHandler(this.buttonSearchForNewAccessionNumber_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(3, 180);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonCreateNewSpecimen
            // 
            this.buttonCreateNewSpecimen.Location = new System.Drawing.Point(244, 180);
            this.buttonCreateNewSpecimen.Name = "buttonCreateNewSpecimen";
            this.buttonCreateNewSpecimen.Size = new System.Drawing.Size(186, 55);
            this.buttonCreateNewSpecimen.TabIndex = 2;
            this.buttonCreateNewSpecimen.Text = "Create new specimen";
            this.buttonCreateNewSpecimen.UseVisualStyleBackColor = true;
            this.buttonCreateNewSpecimen.Click += new System.EventHandler(this.buttonCreateNewSpecimen_Click);
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.labelHeader.Location = new System.Drawing.Point(3, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(196, 13);
            this.labelHeader.TabIndex = 3;
            this.labelHeader.Text = "Please enter the new accession number";
            // 
            // labelAccessionNumber
            // 
            this.labelAccessionNumber.AutoSize = true;
            this.labelAccessionNumber.Location = new System.Drawing.Point(3, 20);
            this.labelAccessionNumber.Name = "labelAccessionNumber";
            this.labelAccessionNumber.Size = new System.Drawing.Size(121, 13);
            this.labelAccessionNumber.TabIndex = 4;
            this.labelAccessionNumber.Text = "New accession number:";
            // 
            // textBoxAccessionNumber
            // 
            this.textBoxAccessionNumber.Location = new System.Drawing.Point(3, 160);
            this.textBoxAccessionNumber.Name = "textBoxAccessionNumber";
            this.textBoxAccessionNumber.Size = new System.Drawing.Size(100, 20);
            this.textBoxAccessionNumber.TabIndex = 5;
            this.textBoxAccessionNumber.TextChanged += new System.EventHandler(this.textBoxAccessionNumber_TextChanged);
            // 
            // FormNewSpecimen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 315);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormNewSpecimen";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create a new specimen";
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Button buttonSearchForNewAccessionNumber;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonCreateNewSpecimen;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Label labelAccessionNumber;
        private System.Windows.Forms.TextBox textBoxAccessionNumber;
    }
}