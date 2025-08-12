namespace DiversityCollection.Forms
{
    partial class FormCollectorIsResponsible
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCollectorIsResponsible));
            this.labelHeader = new System.Windows.Forms.Label();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.buttonFirstCollector = new System.Windows.Forms.Button();
            this.buttonAllCollectors = new System.Windows.Forms.Button();
            this.labelFirstCollector = new System.Windows.Forms.Label();
            this.labelAllCollectors = new System.Windows.Forms.Label();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelHeader, 2);
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(3, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(339, 26);
            this.labelHeader.TabIndex = 0;
            this.labelHeader.Text = "There is more than 1 collector. Please choose the option for the entry for the re" +
                "sponsible persons of the identification.";
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonFirstCollector, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonAllCollectors, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.labelFirstCollector, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.labelAllCollectors, 0, 2);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(345, 110);
            this.tableLayoutPanel.TabIndex = 1;
            // 
            // buttonFirstCollector
            // 
            this.buttonFirstCollector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonFirstCollector.Image = global::DiversityCollection.Resource.Agent;
            this.buttonFirstCollector.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonFirstCollector.Location = new System.Drawing.Point(61, 29);
            this.buttonFirstCollector.Name = "buttonFirstCollector";
            this.buttonFirstCollector.Size = new System.Drawing.Size(281, 36);
            this.buttonFirstCollector.TabIndex = 1;
            this.buttonFirstCollector.Text = "button1";
            this.buttonFirstCollector.UseMnemonic = false;
            this.buttonFirstCollector.UseVisualStyleBackColor = true;
            this.buttonFirstCollector.Click += new System.EventHandler(this.buttonFirstCollector_Click);
            // 
            // buttonAllCollectors
            // 
            this.buttonAllCollectors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAllCollectors.Image = global::DiversityCollection.Resource.Group;
            this.buttonAllCollectors.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonAllCollectors.Location = new System.Drawing.Point(61, 71);
            this.buttonAllCollectors.Name = "buttonAllCollectors";
            this.buttonAllCollectors.Size = new System.Drawing.Size(281, 36);
            this.buttonAllCollectors.TabIndex = 2;
            this.buttonAllCollectors.Text = "button1";
            this.buttonAllCollectors.UseMnemonic = false;
            this.buttonAllCollectors.UseVisualStyleBackColor = true;
            this.buttonAllCollectors.Click += new System.EventHandler(this.buttonAllCollectors_Click);
            // 
            // labelFirstCollector
            // 
            this.labelFirstCollector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFirstCollector.Location = new System.Drawing.Point(3, 26);
            this.labelFirstCollector.Name = "labelFirstCollector";
            this.labelFirstCollector.Size = new System.Drawing.Size(52, 42);
            this.labelFirstCollector.TabIndex = 3;
            this.labelFirstCollector.Text = "first collector";
            this.labelFirstCollector.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelAllCollectors
            // 
            this.labelAllCollectors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAllCollectors.Location = new System.Drawing.Point(3, 68);
            this.labelAllCollectors.Name = "labelAllCollectors";
            this.labelAllCollectors.Size = new System.Drawing.Size(52, 42);
            this.labelAllCollectors.TabIndex = 4;
            this.labelAllCollectors.Text = "all collectors";
            this.labelAllCollectors.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormCollectorIsResponsible
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(345, 110);
            this.Controls.Add(this.tableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormCollectorIsResponsible";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Collector is responsible for identification";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button buttonFirstCollector;
        private System.Windows.Forms.Button buttonAllCollectors;
        private System.Windows.Forms.Label labelFirstCollector;
        private System.Windows.Forms.Label labelAllCollectors;
    }
}