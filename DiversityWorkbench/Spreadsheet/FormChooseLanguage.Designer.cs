namespace DiversityWorkbench.Spreadsheet
{
    partial class FormChooseLanguage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormChooseLanguage));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.radioButtonEN = new System.Windows.Forms.RadioButton();
            this.pictureBoxEN = new System.Windows.Forms.PictureBox();
            this.radioButtonDE = new System.Windows.Forms.RadioButton();
            this.pictureBoxDE = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDE)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 4;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.radioButtonEN, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.pictureBoxDE, 3, 1);
            this.tableLayoutPanel.Controls.Add(this.radioButtonDE, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.pictureBoxEN, 1, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(183, 43);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelHeader, 4);
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(3, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(177, 21);
            this.labelHeader.TabIndex = 0;
            this.labelHeader.Text = "Please select the language";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radioButtonEN
            // 
            this.radioButtonEN.AutoSize = true;
            this.radioButtonEN.Checked = true;
            this.radioButtonEN.Location = new System.Drawing.Point(3, 24);
            this.radioButtonEN.Name = "radioButtonEN";
            this.radioButtonEN.Size = new System.Drawing.Size(14, 13);
            this.radioButtonEN.TabIndex = 1;
            this.radioButtonEN.TabStop = true;
            this.radioButtonEN.UseVisualStyleBackColor = true;
            this.radioButtonEN.CheckedChanged += new System.EventHandler(this.radioButtonEN_CheckedChanged);
            // 
            // pictureBoxEN
            // 
            this.pictureBoxEN.Image = global::DiversityWorkbench.Properties.Resources.English;
            this.pictureBoxEN.Location = new System.Drawing.Point(23, 24);
            this.pictureBoxEN.Name = "pictureBoxEN";
            this.pictureBoxEN.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxEN.TabIndex = 2;
            this.pictureBoxEN.TabStop = false;
            // 
            // radioButtonDE
            // 
            this.radioButtonDE.AutoSize = true;
            this.radioButtonDE.Location = new System.Drawing.Point(94, 24);
            this.radioButtonDE.Name = "radioButtonDE";
            this.radioButtonDE.Size = new System.Drawing.Size(14, 13);
            this.radioButtonDE.TabIndex = 3;
            this.radioButtonDE.UseVisualStyleBackColor = true;
            this.radioButtonDE.CheckedChanged += new System.EventHandler(this.radioButtonDE_CheckedChanged);
            // 
            // pictureBoxDE
            // 
            this.pictureBoxDE.Image = global::DiversityWorkbench.Properties.Resources.Deutsch;
            this.pictureBoxDE.Location = new System.Drawing.Point(114, 24);
            this.pictureBoxDE.Name = "pictureBoxDE";
            this.pictureBoxDE.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxDE.TabIndex = 4;
            this.pictureBoxDE.TabStop = false;
            // 
            // FormChooseLanguage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(183, 43);
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormChooseLanguage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Language";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDE)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.RadioButton radioButtonEN;
        private System.Windows.Forms.PictureBox pictureBoxDE;
        private System.Windows.Forms.RadioButton radioButtonDE;
        private System.Windows.Forms.PictureBox pictureBoxEN;
    }
}