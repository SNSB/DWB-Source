namespace DiversityWorkbench.XsltEditor
{
    partial class FormEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEditor));
            this.toolStripEditor = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSetRegistryKey = new System.Windows.Forms.ToolStripButton();
            this.panelEditor = new System.Windows.Forms.Panel();
            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.toolStripTemplate = new System.Windows.Forms.ToolStrip();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.labelContent = new System.Windows.Forms.Label();
            this.labelTemplate = new System.Windows.Forms.Label();
            this.labelSchema = new System.Windows.Forms.Label();
            this.textBoxContent = new System.Windows.Forms.TextBox();
            this.textBoxTemplate = new System.Windows.Forms.TextBox();
            this.textBoxSchema = new System.Windows.Forms.TextBox();
            this.toolStripEditor.SuspendLayout();
            this.panelEditor.SuspendLayout();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripEditor
            // 
            this.toolStripEditor.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripEditor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSetRegistryKey});
            this.toolStripEditor.Location = new System.Drawing.Point(0, 0);
            this.toolStripEditor.Name = "toolStripEditor";
            this.toolStripEditor.Size = new System.Drawing.Size(835, 25);
            this.toolStripEditor.TabIndex = 0;
            this.toolStripEditor.Text = "toolStrip1";
            // 
            // toolStripButtonSetRegistryKey
            // 
            this.toolStripButtonSetRegistryKey.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSetRegistryKey.Image = global::DiversityWorkbench.Properties.Resources.Registry;
            this.toolStripButtonSetRegistryKey.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSetRegistryKey.Name = "toolStripButtonSetRegistryKey";
            this.toolStripButtonSetRegistryKey.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSetRegistryKey.Text = "Setting the registry key for the usage of IE 11 functions";
            this.toolStripButtonSetRegistryKey.Click += new System.EventHandler(this.toolStripButtonSetRegistryKey_Click);
            // 
            // panelEditor
            // 
            this.panelEditor.BackColor = System.Drawing.SystemColors.Window;
            this.panelEditor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanelMain.SetColumnSpan(this.panelEditor, 2);
            this.panelEditor.Controls.Add(this.richTextBox3);
            this.panelEditor.Controls.Add(this.richTextBox2);
            this.panelEditor.Controls.Add(this.richTextBox1);
            this.panelEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEditor.Location = new System.Drawing.Point(30, 82);
            this.panelEditor.Margin = new System.Windows.Forms.Padding(30);
            this.panelEditor.Name = "panelEditor";
            this.panelEditor.Size = new System.Drawing.Size(775, 439);
            this.panelEditor.TabIndex = 1;
            // 
            // richTextBox3
            // 
            this.richTextBox3.Location = new System.Drawing.Point(419, 66);
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.Size = new System.Drawing.Size(118, 93);
            this.richTextBox3.TabIndex = 2;
            this.richTextBox3.Text = "";
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(474, 227);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(100, 96);
            this.richTextBox2.TabIndex = 1;
            this.richTextBox2.Text = "";
            this.richTextBox2.Click += new System.EventHandler(this.richTextBox2_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(161, 129);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(60, 53);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            this.richTextBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.richTextBox1_MouseClick);
            this.richTextBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.richTextBox1_MouseDown);
            this.richTextBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.richTextBox1_MouseMove);
            this.richTextBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.richTextBox1_MouseUp);
            // 
            // toolStripTemplate
            // 
            this.toolStripTemplate.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripTemplate.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripTemplate.Location = new System.Drawing.Point(0, 602);
            this.toolStripTemplate.Name = "toolStripTemplate";
            this.toolStripTemplate.Size = new System.Drawing.Size(835, 25);
            this.toolStripTemplate.TabIndex = 2;
            this.toolStripTemplate.Text = "toolStrip1";
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 2;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.panelEditor, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.labelContent, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.labelTemplate, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.labelSchema, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxContent, 1, 0);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxTemplate, 1, 1);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxSchema, 1, 3);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 4;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(835, 577);
            this.tableLayoutPanelMain.TabIndex = 3;
            // 
            // labelContent
            // 
            this.labelContent.AutoSize = true;
            this.labelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelContent.Location = new System.Drawing.Point(3, 0);
            this.labelContent.Name = "labelContent";
            this.labelContent.Size = new System.Drawing.Size(54, 26);
            this.labelContent.TabIndex = 2;
            this.labelContent.Text = "Content:";
            this.labelContent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelTemplate
            // 
            this.labelTemplate.AutoSize = true;
            this.labelTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTemplate.Location = new System.Drawing.Point(3, 26);
            this.labelTemplate.Name = "labelTemplate";
            this.labelTemplate.Size = new System.Drawing.Size(54, 26);
            this.labelTemplate.TabIndex = 3;
            this.labelTemplate.Text = "Template:";
            this.labelTemplate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelSchema
            // 
            this.labelSchema.AutoSize = true;
            this.labelSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSchema.Location = new System.Drawing.Point(3, 551);
            this.labelSchema.Name = "labelSchema";
            this.labelSchema.Size = new System.Drawing.Size(54, 26);
            this.labelSchema.TabIndex = 4;
            this.labelSchema.Text = "Schema:";
            this.labelSchema.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxContent
            // 
            this.textBoxContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxContent.Location = new System.Drawing.Point(63, 3);
            this.textBoxContent.Name = "textBoxContent";
            this.textBoxContent.ReadOnly = true;
            this.textBoxContent.Size = new System.Drawing.Size(769, 20);
            this.textBoxContent.TabIndex = 5;
            // 
            // textBoxTemplate
            // 
            this.textBoxTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxTemplate.Location = new System.Drawing.Point(63, 29);
            this.textBoxTemplate.Name = "textBoxTemplate";
            this.textBoxTemplate.ReadOnly = true;
            this.textBoxTemplate.Size = new System.Drawing.Size(769, 20);
            this.textBoxTemplate.TabIndex = 6;
            // 
            // textBoxSchema
            // 
            this.textBoxSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSchema.Location = new System.Drawing.Point(63, 554);
            this.textBoxSchema.Name = "textBoxSchema";
            this.textBoxSchema.ReadOnly = true;
            this.textBoxSchema.Size = new System.Drawing.Size(769, 20);
            this.textBoxSchema.TabIndex = 7;
            // 
            // FormEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(835, 627);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Controls.Add(this.toolStripTemplate);
            this.Controls.Add(this.toolStripEditor);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormEditor";
            this.Text = "Editor for XSLT";
            this.toolStripEditor.ResumeLayout(false);
            this.toolStripEditor.PerformLayout();
            this.panelEditor.ResumeLayout(false);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripEditor;
        private System.Windows.Forms.Panel panelEditor;
        private System.Windows.Forms.ToolStrip toolStripTemplate;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Label labelContent;
        private System.Windows.Forms.Label labelTemplate;
        private System.Windows.Forms.Label labelSchema;
        private System.Windows.Forms.TextBox textBoxContent;
        private System.Windows.Forms.TextBox textBoxTemplate;
        private System.Windows.Forms.TextBox textBoxSchema;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.RichTextBox richTextBox3;
        private System.Windows.Forms.ToolStripButton toolStripButtonSetRegistryKey;
    }
}