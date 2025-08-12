namespace DiversityWorkbench.Forms
{
    partial class FormFeedbackSorting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFeedbackSorting));
            this.label = new System.Windows.Forms.Label();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonAscending = new System.Windows.Forms.RadioButton();
            this.radioButtonUnsorted = new System.Windows.Forms.RadioButton();
            this.radioButtonDescending = new System.Windows.Forms.RadioButton();
            this.pictureBoxAscending = new System.Windows.Forms.PictureBox();
            this.pictureBoxDescending = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAscending)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDescending)).BeginInit();
            this.SuspendLayout();
            // 
            // label
            // 
            this.label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label.Location = new System.Drawing.Point(3, 0);
            this.label.Name = "label";
            this.tableLayoutPanel.SetRowSpan(this.label, 3);
            this.label.Size = new System.Drawing.Size(117, 75);
            this.label.TabIndex = 0;
            this.label.Text = "label1";
            this.label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.label, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.radioButtonAscending, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.radioButtonUnsorted, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.radioButtonDescending, 2, 2);
            this.tableLayoutPanel.Controls.Add(this.pictureBoxAscending, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.pictureBoxDescending, 1, 2);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(263, 75);
            this.tableLayoutPanel.TabIndex = 1;
            // 
            // radioButtonAscending
            // 
            this.radioButtonAscending.AutoSize = true;
            this.radioButtonAscending.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonAscending.Location = new System.Drawing.Point(142, 3);
            this.radioButtonAscending.Name = "radioButtonAscending";
            this.radioButtonAscending.Size = new System.Drawing.Size(75, 18);
            this.radioButtonAscending.TabIndex = 1;
            this.radioButtonAscending.TabStop = true;
            this.radioButtonAscending.Text = "Ascending";
            this.radioButtonAscending.UseVisualStyleBackColor = true;
            this.radioButtonAscending.Click += new System.EventHandler(this.radioButtonAscending_Click);
            // 
            // radioButtonUnsorted
            // 
            this.radioButtonUnsorted.AutoSize = true;
            this.radioButtonUnsorted.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonUnsorted.Location = new System.Drawing.Point(142, 27);
            this.radioButtonUnsorted.Name = "radioButtonUnsorted";
            this.radioButtonUnsorted.Size = new System.Drawing.Size(74, 18);
            this.radioButtonUnsorted.TabIndex = 2;
            this.radioButtonUnsorted.TabStop = true;
            this.radioButtonUnsorted.Text = "Not sorted";
            this.radioButtonUnsorted.UseVisualStyleBackColor = true;
            this.radioButtonUnsorted.Click += new System.EventHandler(this.radioButtonUnsorted_Click);
            // 
            // radioButtonDescending
            // 
            this.radioButtonDescending.AutoSize = true;
            this.radioButtonDescending.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonDescending.Location = new System.Drawing.Point(142, 51);
            this.radioButtonDescending.Name = "radioButtonDescending";
            this.radioButtonDescending.Size = new System.Drawing.Size(82, 21);
            this.radioButtonDescending.TabIndex = 3;
            this.radioButtonDescending.TabStop = true;
            this.radioButtonDescending.Text = "Descending";
            this.radioButtonDescending.UseVisualStyleBackColor = true;
            this.radioButtonDescending.Click += new System.EventHandler(this.radioButtonDescending_Click);
            // 
            // pictureBoxAscending
            // 
            this.pictureBoxAscending.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxAscending.Image = global::DiversityWorkbench.Properties.Resources.ArrowUp;
            this.pictureBoxAscending.Location = new System.Drawing.Point(123, 3);
            this.pictureBoxAscending.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.pictureBoxAscending.Name = "pictureBoxAscending";
            this.pictureBoxAscending.Size = new System.Drawing.Size(16, 21);
            this.pictureBoxAscending.TabIndex = 4;
            this.pictureBoxAscending.TabStop = false;
            // 
            // pictureBoxDescending
            // 
            this.pictureBoxDescending.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxDescending.Image = global::DiversityWorkbench.Properties.Resources.ArrowDown;
            this.pictureBoxDescending.Location = new System.Drawing.Point(123, 51);
            this.pictureBoxDescending.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.pictureBoxDescending.Name = "pictureBoxDescending";
            this.pictureBoxDescending.Size = new System.Drawing.Size(16, 21);
            this.pictureBoxDescending.TabIndex = 5;
            this.pictureBoxDescending.TabStop = false;
            // 
            // FormFeedbackSorting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(263, 75);
            this.Controls.Add(this.tableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormFeedbackSorting";
            this.Text = "Sorting of the feedback";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAscending)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDescending)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.RadioButton radioButtonAscending;
        private System.Windows.Forms.RadioButton radioButtonUnsorted;
        private System.Windows.Forms.RadioButton radioButtonDescending;
        private System.Windows.Forms.PictureBox pictureBoxAscending;
        private System.Windows.Forms.PictureBox pictureBoxDescending;
    }
}