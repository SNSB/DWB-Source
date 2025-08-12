namespace DiversityCollection.UserControls
{
    partial class UserControlDatawithholding
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

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.listBoxBlocked = new System.Windows.Forms.ListBox();
            this.labelBlocked = new System.Windows.Forms.Label();
            this.labelToPublish = new System.Windows.Forms.Label();
            this.buttonSetToPublish = new System.Windows.Forms.Button();
            this.textBoxBlockedReason = new System.Windows.Forms.TextBox();
            this.buttonSetBlocked = new System.Windows.Forms.Button();
            this.listBoxToPublish = new System.Windows.Forms.ListBox();
            this.labelNewWitholdingReason = new System.Windows.Forms.Label();
            this.buttonSetToPublishSingle = new System.Windows.Forms.Button();
            this.buttonSetBlockedSingle = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 4;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 96F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.listBoxBlocked, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.labelBlocked, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.labelToPublish, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonSetToPublish, 3, 2);
            this.tableLayoutPanel.Controls.Add(this.textBoxBlockedReason, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.buttonSetBlocked, 2, 2);
            this.tableLayoutPanel.Controls.Add(this.listBoxToPublish, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.labelNewWitholdingReason, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.buttonSetToPublishSingle, 3, 3);
            this.tableLayoutPanel.Controls.Add(this.buttonSetBlockedSingle, 2, 3);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 5;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(715, 431);
            this.tableLayoutPanel.TabIndex = 1;
            // 
            // listBoxBlocked
            // 
            this.listBoxBlocked.BackColor = System.Drawing.Color.Pink;
            this.tableLayoutPanel.SetColumnSpan(this.listBoxBlocked, 4);
            this.listBoxBlocked.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxBlocked.FormattingEnabled = true;
            this.listBoxBlocked.IntegralHeight = false;
            this.listBoxBlocked.Location = new System.Drawing.Point(3, 253);
            this.listBoxBlocked.Name = "listBoxBlocked";
            this.listBoxBlocked.Size = new System.Drawing.Size(709, 175);
            this.listBoxBlocked.TabIndex = 0;
            // 
            // labelBlocked
            // 
            this.labelBlocked.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelBlocked, 2);
            this.labelBlocked.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelBlocked.Location = new System.Drawing.Point(3, 226);
            this.labelBlocked.Name = "labelBlocked";
            this.labelBlocked.Size = new System.Drawing.Size(160, 24);
            this.labelBlocked.TabIndex = 1;
            this.labelBlocked.Text = "Current withholding reasons:";
            this.labelBlocked.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelToPublish
            // 
            this.labelToPublish.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelToPublish, 4);
            this.labelToPublish.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelToPublish.Location = new System.Drawing.Point(3, 3);
            this.labelToPublish.Margin = new System.Windows.Forms.Padding(3);
            this.labelToPublish.Name = "labelToPublish";
            this.labelToPublish.Size = new System.Drawing.Size(709, 13);
            this.labelToPublish.TabIndex = 2;
            this.labelToPublish.Text = "label1";
            // 
            // buttonSetToPublish
            // 
            this.buttonSetToPublish.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSetToPublish.ForeColor = System.Drawing.Color.Green;
            this.buttonSetToPublish.Image = global::DiversityCollection.Resource.ArrowsUp;
            this.buttonSetToPublish.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSetToPublish.Location = new System.Drawing.Point(443, 201);
            this.buttonSetToPublish.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.buttonSetToPublish.Name = "buttonSetToPublish";
            this.buttonSetToPublish.Size = new System.Drawing.Size(269, 24);
            this.buttonSetToPublish.TabIndex = 3;
            this.buttonSetToPublish.Text = "Remove withholding reasons from all";
            this.buttonSetToPublish.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSetToPublish.UseVisualStyleBackColor = true;
            this.buttonSetToPublish.Click += new System.EventHandler(this.buttonSetToPublish_Click);
            // 
            // textBoxBlockedReason
            // 
            this.textBoxBlockedReason.BackColor = System.Drawing.Color.Pink;
            this.textBoxBlockedReason.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxBlockedReason.Location = new System.Drawing.Point(70, 203);
            this.textBoxBlockedReason.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.textBoxBlockedReason.Name = "textBoxBlockedReason";
            this.textBoxBlockedReason.Size = new System.Drawing.Size(96, 20);
            this.textBoxBlockedReason.TabIndex = 4;
            // 
            // buttonSetBlocked
            // 
            this.buttonSetBlocked.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSetBlocked.ForeColor = System.Drawing.Color.DarkRed;
            this.buttonSetBlocked.Image = global::DiversityCollection.Resource.Stop3;
            this.buttonSetBlocked.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSetBlocked.Location = new System.Drawing.Point(166, 201);
            this.buttonSetBlocked.Margin = new System.Windows.Forms.Padding(0, 1, 3, 1);
            this.buttonSetBlocked.Name = "buttonSetBlocked";
            this.buttonSetBlocked.Size = new System.Drawing.Size(271, 24);
            this.buttonSetBlocked.TabIndex = 5;
            this.buttonSetBlocked.Text = "Add withholding reason to all";
            this.buttonSetBlocked.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSetBlocked.UseVisualStyleBackColor = true;
            this.buttonSetBlocked.Click += new System.EventHandler(this.buttonSetBlocked_Click);
            // 
            // listBoxToPublish
            // 
            this.listBoxToPublish.BackColor = System.Drawing.Color.LightGreen;
            this.tableLayoutPanel.SetColumnSpan(this.listBoxToPublish, 4);
            this.listBoxToPublish.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxToPublish.FormattingEnabled = true;
            this.listBoxToPublish.IntegralHeight = false;
            this.listBoxToPublish.Location = new System.Drawing.Point(3, 22);
            this.listBoxToPublish.Name = "listBoxToPublish";
            this.listBoxToPublish.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxToPublish.Size = new System.Drawing.Size(709, 175);
            this.listBoxToPublish.TabIndex = 6;
            // 
            // labelNewWitholdingReason
            // 
            this.labelNewWitholdingReason.AutoSize = true;
            this.labelNewWitholdingReason.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNewWitholdingReason.ForeColor = System.Drawing.Color.DarkRed;
            this.labelNewWitholdingReason.Location = new System.Drawing.Point(3, 200);
            this.labelNewWitholdingReason.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelNewWitholdingReason.Name = "labelNewWitholdingReason";
            this.labelNewWitholdingReason.Size = new System.Drawing.Size(67, 26);
            this.labelNewWitholdingReason.TabIndex = 7;
            this.labelNewWitholdingReason.Text = "New reason:";
            this.labelNewWitholdingReason.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonSetToPublishSingle
            // 
            this.buttonSetToPublishSingle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSetToPublishSingle.ForeColor = System.Drawing.Color.Green;
            this.buttonSetToPublishSingle.Image = global::DiversityCollection.Resource.ArrowUpNarrow;
            this.buttonSetToPublishSingle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSetToPublishSingle.Location = new System.Drawing.Point(470, 226);
            this.buttonSetToPublishSingle.Margin = new System.Windows.Forms.Padding(30, 0, 30, 0);
            this.buttonSetToPublishSingle.Name = "buttonSetToPublishSingle";
            this.buttonSetToPublishSingle.Size = new System.Drawing.Size(215, 24);
            this.buttonSetToPublishSingle.TabIndex = 8;
            this.buttonSetToPublishSingle.Text = "... only from selected";
            this.buttonSetToPublishSingle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSetToPublishSingle.UseVisualStyleBackColor = true;
            this.buttonSetToPublishSingle.Click += new System.EventHandler(this.buttonSetToPublishSingle_Click);
            // 
            // buttonSetBlockedSingle
            // 
            this.buttonSetBlockedSingle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSetBlockedSingle.ForeColor = System.Drawing.Color.DarkRed;
            this.buttonSetBlockedSingle.Image = global::DiversityCollection.Resource.Stop3;
            this.buttonSetBlockedSingle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSetBlockedSingle.Location = new System.Drawing.Point(196, 226);
            this.buttonSetBlockedSingle.Margin = new System.Windows.Forms.Padding(30, 0, 30, 0);
            this.buttonSetBlockedSingle.Name = "buttonSetBlockedSingle";
            this.buttonSetBlockedSingle.Size = new System.Drawing.Size(214, 24);
            this.buttonSetBlockedSingle.TabIndex = 9;
            this.buttonSetBlockedSingle.Text = "... only to selected";
            this.buttonSetBlockedSingle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSetBlockedSingle.UseVisualStyleBackColor = true;
            this.buttonSetBlockedSingle.Click += new System.EventHandler(this.buttonSetBlockedSingle_Click);
            // 
            // UserControlDatawithholding
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlDatawithholding";
            this.Size = new System.Drawing.Size(715, 431);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.ListBox listBoxBlocked;
        private System.Windows.Forms.Label labelBlocked;
        private System.Windows.Forms.Label labelToPublish;
        private System.Windows.Forms.Button buttonSetToPublish;
        private System.Windows.Forms.TextBox textBoxBlockedReason;
        private System.Windows.Forms.Button buttonSetBlocked;
        private System.Windows.Forms.ListBox listBoxToPublish;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelNewWitholdingReason;
        private System.Windows.Forms.Button buttonSetToPublishSingle;
        private System.Windows.Forms.Button buttonSetBlockedSingle;
    }
}
