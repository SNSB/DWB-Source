namespace DiversityCollection.Tasks
{
    partial class FormWebview
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
            this.webView2 = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.linkLabel = new System.Windows.Forms.LinkLabel();
            this.linkLabelCC = new System.Windows.Forms.LinkLabel();
            this.buttonPrevious = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.linkLabelCopyright = new System.Windows.Forms.LinkLabel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.labelTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.webView2)).BeginInit();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // webView2
            // 
            this.tableLayoutPanel.SetColumnSpan(this.webView2, 3);
            this.webView2.CreationProperties = null;
            this.webView2.DefaultBackgroundColor = System.Drawing.SystemColors.Window;
            this.webView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webView2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.webView2.Location = new System.Drawing.Point(18, 13);
            this.webView2.Margin = new System.Windows.Forms.Padding(0);
            this.webView2.Name = "webView2";
            this.webView2.Size = new System.Drawing.Size(764, 424);
            this.webView2.TabIndex = 0;
            this.webView2.ZoomFactor = 1D;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 5;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.linkLabel, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.webView2, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.linkLabelCC, 2, 2);
            this.tableLayoutPanel.Controls.Add(this.buttonPrevious, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonNext, 4, 1);
            this.tableLayoutPanel.Controls.Add(this.linkLabelCopyright, 3, 2);
            this.tableLayoutPanel.Controls.Add(this.labelTitle, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(800, 450);
            this.tableLayoutPanel.TabIndex = 1;
            // 
            // linkLabel
            // 
            this.linkLabel.AutoSize = true;
            this.linkLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linkLabel.Location = new System.Drawing.Point(21, 437);
            this.linkLabel.Name = "linkLabel";
            this.linkLabel.Size = new System.Drawing.Size(366, 13);
            this.linkLabel.TabIndex = 5;
            this.linkLabel.TabStop = true;
            this.linkLabel.Text = "Uri";
            // 
            // linkLabelCC
            // 
            this.linkLabelCC.AutoSize = true;
            this.linkLabelCC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linkLabelCC.Location = new System.Drawing.Point(396, 437);
            this.linkLabelCC.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.linkLabelCC.Name = "linkLabelCC";
            this.linkLabelCC.Size = new System.Drawing.Size(14, 13);
            this.linkLabelCC.TabIndex = 1;
            this.linkLabelCC.TabStop = true;
            this.linkLabelCC.Text = "©";
            // 
            // buttonPrevious
            // 
            this.buttonPrevious.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPrevious.FlatAppearance.BorderSize = 0;
            this.buttonPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPrevious.Image = global::DiversityCollection.Resource.ArrowPrevious1;
            this.buttonPrevious.Location = new System.Drawing.Point(0, 13);
            this.buttonPrevious.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPrevious.Name = "buttonPrevious";
            this.tableLayoutPanel.SetRowSpan(this.buttonPrevious, 2);
            this.buttonPrevious.Size = new System.Drawing.Size(18, 437);
            this.buttonPrevious.TabIndex = 2;
            this.buttonPrevious.UseVisualStyleBackColor = true;
            this.buttonPrevious.Visible = false;
            this.buttonPrevious.Click += new System.EventHandler(this.buttonPrevious_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonNext.FlatAppearance.BorderSize = 0;
            this.buttonNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNext.Image = global::DiversityCollection.Resource.ArrowNext1;
            this.buttonNext.Location = new System.Drawing.Point(782, 13);
            this.buttonNext.Margin = new System.Windows.Forms.Padding(0);
            this.buttonNext.Name = "buttonNext";
            this.tableLayoutPanel.SetRowSpan(this.buttonNext, 2);
            this.buttonNext.Size = new System.Drawing.Size(18, 437);
            this.buttonNext.TabIndex = 3;
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Visible = false;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // linkLabelCopyright
            // 
            this.linkLabelCopyright.AutoSize = true;
            this.linkLabelCopyright.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linkLabelCopyright.Location = new System.Drawing.Point(410, 437);
            this.linkLabelCopyright.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.linkLabelCopyright.Name = "linkLabelCopyright";
            this.linkLabelCopyright.Size = new System.Drawing.Size(369, 13);
            this.linkLabelCopyright.TabIndex = 4;
            this.linkLabelCopyright.TabStop = true;
            this.linkLabelCopyright.Text = "Copyright";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelTitle, 5);
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitle.Location = new System.Drawing.Point(3, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(794, 13);
            this.labelTitle.TabIndex = 6;
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelTitle.Visible = false;
            // 
            // FormWebview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "FormWebview";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormWebview";
            ((System.ComponentModel.ISupportInitialize)(this.webView2)).EndInit();
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 webView2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.LinkLabel linkLabelCC;
        private System.Windows.Forms.Button buttonPrevious;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.LinkLabel linkLabelCopyright;
        private System.Windows.Forms.LinkLabel linkLabel;
        private System.Windows.Forms.Label labelTitle;
    }
}