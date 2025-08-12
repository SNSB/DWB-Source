namespace DiversityCollection.Forms
{
    partial class FormConvertImages
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

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxImageTarget = new System.Windows.Forms.GroupBox();
            this.buttonConvertImages = new System.Windows.Forms.Button();
            this.labelTarget = new System.Windows.Forms.Label();
            this.checkBoxPreviewImages = new System.Windows.Forms.CheckBox();
            this.checkBoxWebImages = new System.Windows.Forms.CheckBox();
            this.buttonFolderPreview = new System.Windows.Forms.Button();
            this.buttonFolderWeb = new System.Windows.Forms.Button();
            this.labelFormat = new System.Windows.Forms.Label();
            this.labelTargetFolders = new System.Windows.Forms.Label();
            this.textBoxFolderWeb = new System.Windows.Forms.TextBox();
            this.comboBoxImageFormatWeb = new System.Windows.Forms.ComboBox();
            this.comboBoxImageFormatPreview = new System.Windows.Forms.ComboBox();
            this.textBoxFolderPreview = new System.Windows.Forms.TextBox();
            this.groupBoxImageScaling = new System.Windows.Forms.GroupBox();
            this.textBoxScalePreview = new System.Windows.Forms.TextBox();
            this.textBoxScaleWeb = new System.Windows.Forms.TextBox();
            this.radioButtonWidth = new System.Windows.Forms.RadioButton();
            this.comboBoxImageWidthWeb = new System.Windows.Forms.ComboBox();
            this.comboBoxImageWidthPreview = new System.Windows.Forms.ComboBox();
            this.radioButtonScale = new System.Windows.Forms.RadioButton();
            this.groupBoxImageTarget.SuspendLayout();
            this.groupBoxImageScaling.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxImageTarget
            // 
            this.groupBoxImageTarget.Controls.Add(this.buttonConvertImages);
            this.groupBoxImageTarget.Controls.Add(this.labelTarget);
            this.groupBoxImageTarget.Controls.Add(this.checkBoxPreviewImages);
            this.groupBoxImageTarget.Controls.Add(this.checkBoxWebImages);
            this.groupBoxImageTarget.Controls.Add(this.buttonFolderPreview);
            this.groupBoxImageTarget.Controls.Add(this.buttonFolderWeb);
            this.groupBoxImageTarget.Controls.Add(this.labelFormat);
            this.groupBoxImageTarget.Controls.Add(this.labelTargetFolders);
            this.groupBoxImageTarget.Controls.Add(this.textBoxFolderWeb);
            this.groupBoxImageTarget.Controls.Add(this.comboBoxImageFormatWeb);
            this.groupBoxImageTarget.Controls.Add(this.comboBoxImageFormatPreview);
            this.groupBoxImageTarget.Controls.Add(this.textBoxFolderPreview);
            this.groupBoxImageTarget.Controls.Add(this.groupBoxImageScaling);
            this.groupBoxImageTarget.Location = new System.Drawing.Point(12, 12);
            this.groupBoxImageTarget.Name = "groupBoxImageTarget";
            this.groupBoxImageTarget.Size = new System.Drawing.Size(903, 168);
            this.groupBoxImageTarget.TabIndex = 73;
            this.groupBoxImageTarget.TabStop = false;
            this.groupBoxImageTarget.Text = "Image target";
            // 
            // buttonConvertImages
            // 
            this.buttonConvertImages.Location = new System.Drawing.Point(401, 123);
            this.buttonConvertImages.Name = "buttonConvertImages";
            this.buttonConvertImages.Size = new System.Drawing.Size(94, 23);
            this.buttonConvertImages.TabIndex = 69;
            this.buttonConvertImages.Text = "Convert images";
            this.buttonConvertImages.UseVisualStyleBackColor = true;
            // 
            // labelTarget
            // 
            this.labelTarget.Location = new System.Drawing.Point(8, 16);
            this.labelTarget.Name = "labelTarget";
            this.labelTarget.Size = new System.Drawing.Size(552, 16);
            this.labelTarget.TabIndex = 68;
            this.labelTarget.Text = "Images can be converted to differing image formats and a differing size. You need" +
    " local access to the folders";
            // 
            // checkBoxPreviewImages
            // 
            this.checkBoxPreviewImages.Location = new System.Drawing.Point(8, 80);
            this.checkBoxPreviewImages.Name = "checkBoxPreviewImages";
            this.checkBoxPreviewImages.Size = new System.Drawing.Size(104, 24);
            this.checkBoxPreviewImages.TabIndex = 67;
            this.checkBoxPreviewImages.Text = "Preview images";
            // 
            // checkBoxWebImages
            // 
            this.checkBoxWebImages.Location = new System.Drawing.Point(8, 56);
            this.checkBoxWebImages.Name = "checkBoxWebImages";
            this.checkBoxWebImages.Size = new System.Drawing.Size(88, 24);
            this.checkBoxWebImages.TabIndex = 66;
            this.checkBoxWebImages.Text = "Web images";
            // 
            // buttonFolderPreview
            // 
            this.buttonFolderPreview.Image = global::DiversityCollection.Resource.Folder;
            this.buttonFolderPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonFolderPreview.Location = new System.Drawing.Point(687, 80);
            this.buttonFolderPreview.Name = "buttonFolderPreview";
            this.buttonFolderPreview.Size = new System.Drawing.Size(24, 20);
            this.buttonFolderPreview.TabIndex = 65;
            // 
            // buttonFolderWeb
            // 
            this.buttonFolderWeb.Image = global::DiversityCollection.Resource.Folder;
            this.buttonFolderWeb.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonFolderWeb.Location = new System.Drawing.Point(687, 56);
            this.buttonFolderWeb.Name = "buttonFolderWeb";
            this.buttonFolderWeb.Size = new System.Drawing.Size(24, 20);
            this.buttonFolderWeb.TabIndex = 64;
            // 
            // labelFormat
            // 
            this.labelFormat.Location = new System.Drawing.Point(717, 40);
            this.labelFormat.Name = "labelFormat";
            this.labelFormat.Size = new System.Drawing.Size(48, 16);
            this.labelFormat.TabIndex = 12;
            this.labelFormat.Text = "Format";
            // 
            // labelTargetFolders
            // 
            this.labelTargetFolders.Location = new System.Drawing.Point(112, 40);
            this.labelTargetFolders.Name = "labelTargetFolders";
            this.labelTargetFolders.Size = new System.Drawing.Size(120, 16);
            this.labelTargetFolders.TabIndex = 11;
            this.labelTargetFolders.Text = "Target directories";
            // 
            // textBoxFolderWeb
            // 
            this.textBoxFolderWeb.BackColor = System.Drawing.Color.Pink;
            this.textBoxFolderWeb.Location = new System.Drawing.Point(112, 56);
            this.textBoxFolderWeb.Name = "textBoxFolderWeb";
            this.textBoxFolderWeb.Size = new System.Drawing.Size(569, 20);
            this.textBoxFolderWeb.TabIndex = 3;
            // 
            // comboBoxImageFormatWeb
            // 
            this.comboBoxImageFormatWeb.BackColor = System.Drawing.Color.Pink;
            this.comboBoxImageFormatWeb.Items.AddRange(new object[] {
            ".gif",
            ".ipg",
            ".png",
            ".tif"});
            this.comboBoxImageFormatWeb.Location = new System.Drawing.Point(717, 56);
            this.comboBoxImageFormatWeb.Name = "comboBoxImageFormatWeb";
            this.comboBoxImageFormatWeb.Size = new System.Drawing.Size(48, 21);
            this.comboBoxImageFormatWeb.TabIndex = 9;
            this.comboBoxImageFormatWeb.Text = ".jpg";
            // 
            // comboBoxImageFormatPreview
            // 
            this.comboBoxImageFormatPreview.BackColor = System.Drawing.Color.Pink;
            this.comboBoxImageFormatPreview.Items.AddRange(new object[] {
            ".gif",
            ".ipg",
            ".png",
            ".tif"});
            this.comboBoxImageFormatPreview.Location = new System.Drawing.Point(717, 80);
            this.comboBoxImageFormatPreview.Name = "comboBoxImageFormatPreview";
            this.comboBoxImageFormatPreview.Size = new System.Drawing.Size(48, 21);
            this.comboBoxImageFormatPreview.TabIndex = 10;
            this.comboBoxImageFormatPreview.Text = ".jpg";
            // 
            // textBoxFolderPreview
            // 
            this.textBoxFolderPreview.BackColor = System.Drawing.Color.Pink;
            this.textBoxFolderPreview.Location = new System.Drawing.Point(112, 80);
            this.textBoxFolderPreview.Name = "textBoxFolderPreview";
            this.textBoxFolderPreview.Size = new System.Drawing.Size(569, 20);
            this.textBoxFolderPreview.TabIndex = 5;
            // 
            // groupBoxImageScaling
            // 
            this.groupBoxImageScaling.Controls.Add(this.textBoxScalePreview);
            this.groupBoxImageScaling.Controls.Add(this.textBoxScaleWeb);
            this.groupBoxImageScaling.Controls.Add(this.radioButtonWidth);
            this.groupBoxImageScaling.Controls.Add(this.comboBoxImageWidthWeb);
            this.groupBoxImageScaling.Controls.Add(this.comboBoxImageWidthPreview);
            this.groupBoxImageScaling.Controls.Add(this.radioButtonScale);
            this.groupBoxImageScaling.Location = new System.Drawing.Point(777, 16);
            this.groupBoxImageScaling.Name = "groupBoxImageScaling";
            this.groupBoxImageScaling.Size = new System.Drawing.Size(120, 92);
            this.groupBoxImageScaling.TabIndex = 63;
            this.groupBoxImageScaling.TabStop = false;
            this.groupBoxImageScaling.Text = "Scaling";
            // 
            // textBoxScalePreview
            // 
            this.textBoxScalePreview.Location = new System.Drawing.Point(64, 64);
            this.textBoxScalePreview.Name = "textBoxScalePreview";
            this.textBoxScalePreview.Size = new System.Drawing.Size(48, 20);
            this.textBoxScalePreview.TabIndex = 59;
            this.textBoxScalePreview.Text = "2,4";
            this.textBoxScalePreview.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxScaleWeb
            // 
            this.textBoxScaleWeb.Location = new System.Drawing.Point(64, 40);
            this.textBoxScaleWeb.Name = "textBoxScaleWeb";
            this.textBoxScaleWeb.Size = new System.Drawing.Size(48, 20);
            this.textBoxScaleWeb.TabIndex = 58;
            this.textBoxScaleWeb.Text = "12";
            this.textBoxScaleWeb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // radioButtonWidth
            // 
            this.radioButtonWidth.Checked = true;
            this.radioButtonWidth.Location = new System.Drawing.Point(8, 16);
            this.radioButtonWidth.Name = "radioButtonWidth";
            this.radioButtonWidth.Size = new System.Drawing.Size(56, 24);
            this.radioButtonWidth.TabIndex = 0;
            this.radioButtonWidth.TabStop = true;
            this.radioButtonWidth.Text = "Width";
            // 
            // comboBoxImageWidthWeb
            // 
            this.comboBoxImageWidthWeb.BackColor = System.Drawing.Color.Pink;
            this.comboBoxImageWidthWeb.Items.AddRange(new object[] {
            "100",
            "200",
            "400",
            "800"});
            this.comboBoxImageWidthWeb.Location = new System.Drawing.Point(8, 40);
            this.comboBoxImageWidthWeb.Name = "comboBoxImageWidthWeb";
            this.comboBoxImageWidthWeb.Size = new System.Drawing.Size(48, 21);
            this.comboBoxImageWidthWeb.TabIndex = 56;
            this.comboBoxImageWidthWeb.Text = "800";
            // 
            // comboBoxImageWidthPreview
            // 
            this.comboBoxImageWidthPreview.BackColor = System.Drawing.Color.Pink;
            this.comboBoxImageWidthPreview.Items.AddRange(new object[] {
            "100",
            "200",
            "400",
            "800"});
            this.comboBoxImageWidthPreview.Location = new System.Drawing.Point(8, 64);
            this.comboBoxImageWidthPreview.Name = "comboBoxImageWidthPreview";
            this.comboBoxImageWidthPreview.Size = new System.Drawing.Size(48, 21);
            this.comboBoxImageWidthPreview.TabIndex = 57;
            this.comboBoxImageWidthPreview.Text = "200";
            // 
            // radioButtonScale
            // 
            this.radioButtonScale.Location = new System.Drawing.Point(64, 16);
            this.radioButtonScale.Name = "radioButtonScale";
            this.radioButtonScale.Size = new System.Drawing.Size(52, 24);
            this.radioButtonScale.TabIndex = 1;
            this.radioButtonScale.Text = "Scale";
            // 
            // FormConvertImages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(925, 191);
            this.Controls.Add(this.groupBoxImageTarget);
            this.Name = "FormConvertImages";
            this.Text = "FormConvertImages";
            this.groupBoxImageTarget.ResumeLayout(false);
            this.groupBoxImageTarget.PerformLayout();
            this.groupBoxImageScaling.ResumeLayout(false);
            this.groupBoxImageScaling.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxImageTarget;
        private System.Windows.Forms.Button buttonConvertImages;
        private System.Windows.Forms.Label labelTarget;
        private System.Windows.Forms.CheckBox checkBoxPreviewImages;
        private System.Windows.Forms.CheckBox checkBoxWebImages;
        private System.Windows.Forms.Button buttonFolderPreview;
        private System.Windows.Forms.Button buttonFolderWeb;
        private System.Windows.Forms.Label labelFormat;
        private System.Windows.Forms.Label labelTargetFolders;
        private System.Windows.Forms.TextBox textBoxFolderWeb;
        private System.Windows.Forms.ComboBox comboBoxImageFormatWeb;
        private System.Windows.Forms.ComboBox comboBoxImageFormatPreview;
        private System.Windows.Forms.TextBox textBoxFolderPreview;
        private System.Windows.Forms.GroupBox groupBoxImageScaling;
        private System.Windows.Forms.TextBox textBoxScalePreview;
        private System.Windows.Forms.TextBox textBoxScaleWeb;
        private System.Windows.Forms.RadioButton radioButtonWidth;
        private System.Windows.Forms.ComboBox comboBoxImageWidthWeb;
        private System.Windows.Forms.ComboBox comboBoxImageWidthPreview;
        private System.Windows.Forms.RadioButton radioButtonScale;
    }
}