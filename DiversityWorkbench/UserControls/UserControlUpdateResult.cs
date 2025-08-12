using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.UserControls
{
    public enum ProcessingState
    {
        notStarted,
        running,
        failed,
        skipped,
        success
    }

    public partial class UserControlUpdateResult : UserControl
    {
        private string _ScriptTitle;
        private string _ScriptText;
        private string _OptionalText = "";
        private bool _Skip = false;
        private bool _SetOptions = true;

        public string ScriptTitle
        {
            get { return _ScriptTitle; }
            //set { _ScriptTitle = value; }
        }

        public bool Optional
        {
            get { return _OptionalText.Length > 0; }
        }

        public bool Skip
        {
            get { return _Skip; }
        }

        public UserControlUpdateResult(string updateScriptTitle, string updateScriptText)
        {
            InitializeComponent();
            _ScriptTitle = updateScriptTitle;
            _ScriptText = updateScriptText;

            // Handle optional updates
            this.buttonOptional.Tag = false;
            int startIdx = updateScriptText.IndexOf("--#OPTIONAL");
            if (startIdx > -1)
            {
                startIdx += "--#OPTIONAL".Length;
                int endIdx = updateScriptText.IndexOf("--#END", startIdx);
                if (startIdx < endIdx)
                {
                    _OptionalText = updateScriptText.Substring(startIdx, endIdx - startIdx).Replace("--", "").Replace("\r\n", " ").Trim();
                }
                else
                {
                    _OptionalText = "";
                }
            }
            this.buttonOptional.Visible = Optional;
            _Skip = Optional;
        }

        private void buttonResult_Click(object sender, EventArgs e)
        {
            if (this.textBoxResultFile.Text != string.Empty)
            {
                if (textBoxResultFile.Text.EndsWith(".xml", StringComparison.InvariantCultureIgnoreCase))
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(textBoxResultFile.Text);
                    string cvt = ScriptProcessor.ConvertResult(fi);
                    if (cvt != null)
                        SetResultFile(cvt);
                }
                DiversityWorkbench.Forms.FormWebBrowser html = new DiversityWorkbench.Forms.FormWebBrowser(this.textBoxResultFile.Text);
                html.ShowDialog();
            }
            else if (_ScriptTitle != string.Empty)
            {
                DiversityWorkbench.Forms.FormEditText et = new DiversityWorkbench.Forms.FormEditText(_ScriptTitle, _ScriptText, true);
                et.ShowDialog();
            }
        }

        public void SetResultFile(string file)
        {
            if (file != string.Empty)
            {
                textBoxResultFile.Text = file;
                textBoxResultFile.SelectionStart = this.textBoxResultFile.Text.Length;
                textBoxResultFile.SelectionLength = 1;
                toolTip.SetToolTip(textBoxResultFile, "Click on text to view update results");
            }
        }

        public void SetRunning(ProcessingState state = ProcessingState.running)
        {
            switch (state)
            {
                case ProcessingState.notStarted:
                    buttonResult.Image = null;
                    buttonResult.Text = "Not started";
                    toolTip.SetToolTip(buttonResult, "Script has not been executed yet, click on button \"Start update\"\r\nClick here to view update script");
                    break;
                case ProcessingState.running:
                    buttonResult.Image = DiversityWorkbench.Properties.Resources.wait_animation;
                    buttonResult.Text = "Running";
                    toolTip.SetToolTip(buttonResult, "Update is running");
                    break;
                case ProcessingState.failed:
                    buttonResult.Image = DiversityWorkbench.Properties.Resources.Delete;
                    buttonResult.Text = "Failed";
                    toolTip.SetToolTip(buttonResult, "Click on button to view updates results");
                    break;
                case ProcessingState.success:
                    buttonResult.Image = DiversityWorkbench.Properties.Resources.OK;
                    buttonResult.Text = "OK";
                    toolTip.SetToolTip(buttonResult, "Click on button to view update results");
                    break;
                default:
                    break;
            }
        }

        public void SetResult(bool success)
        {
            // Lock changing of options
            _SetOptions = false;

            if (Skip)
            {
                this.buttonResult.Image = DiversityWorkbench.Properties.Resources.OK;
                this.buttonResult.Text = "Skipped";
                this.toolTip.SetToolTip(buttonResult, "No update result file available");
            }
            else
            {
                if (success)
                {
                    this.buttonResult.Image = DiversityWorkbench.Properties.Resources.OK;
                    this.buttonResult.Text = "OK";
                }
                else
                {
                    this.buttonResult.Image = DiversityWorkbench.Properties.Resources.Delete;
                    this.buttonResult.Text = "Failed";
                }
                this.toolTip.SetToolTip(buttonResult, "Click on button to view update results");
            }
        }

        private void buttonOptional_Click(object sender, EventArgs e)
        {
            if (_SetOptions)
            {
                if (MessageBox.Show(_OptionalText + "\r\n\r\nExecute optional update steps?", "Optional update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _Skip = false;
                    this.buttonOptional.BackColor = Color.Yellow;
                    this.buttonOptional.FlatAppearance.BorderColor = Color.Green;
                    this.buttonOptional.Image = DiversityWorkbench.Properties.Resources.OK;
                }
                else
                {
                    _Skip = true;
                    this.buttonOptional.BackColor = SystemColors.Control;
                    this.buttonOptional.FlatAppearance.BorderColor = Color.Black;
                    this.buttonOptional.Image = DiversityWorkbench.Properties.Resources.Delete;
                }
            }
            else
                MessageBox.Show(_OptionalText, "Optional update", MessageBoxButtons.OK);
        }
    }
}
