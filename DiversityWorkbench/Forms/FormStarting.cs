using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormStarting : Form
    {
        private System.Collections.Generic.Dictionary<string, System.DateTime> _Times;

        public enum Unit { Agents, Collection, Details, Import, Database }


        public FormStarting(string Text, System.Drawing.Image Image)
        {
            InitializeComponent();
            this.label.Text = Text;
            if (Text.EndsWith("..."))
                Text = Text.Substring(0, Text.Length - 3);
            this.Text = Text;
            this.pictureBox.Image = Image;
        }

        public FormStarting(Unit unit, string FormText, string Title, int MaxSteps, string firstStep = "", bool TopMost = true, bool ShowInTaskbar = true)
        {
            InitializeComponent();

            // window
            this.ShowInTaskbar = ShowInTaskbar;
            this.TopMost = TopMost;

            // Texts
            this.Text = FormText;
            this.label.Text = Title;

            // Images
            try
            {
                switch (unit)
                {
                    case Unit.Agents:
                        //this.Text = "Connecting to DiversityAgents";
                        this.Icon = DiversityWorkbench.Properties.Resources.DiversityAgents;
                        //System.Drawing.Bitmap bitmap = DiversityWorkbench.Properties.Resources.DiversityAgents.ToBitmap();
                        //this.pictureBox.Image = bitmap;
                        break;
                    case Unit.Database:
                        //this.Text = "Connecting ...";
                        this.Icon = DiversityWorkbench.Properties.Resources.Database128;
                        break;
                    case Unit.Details:
                        this.Icon = DiversityWorkbench.Properties.Resources.Lupe128;
                        break;
                }
                System.Drawing.Bitmap bitmap = this.Icon.ToBitmap();
                this.pictureBox.Image = bitmap;

                // Progress bar
                this.progressBar.Value = 0;
                this.progressBar.Visible = true;

                // Current step
                this.labelCurrentStep.Text = firstStep;
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void setIconFromImageList(Unit unit)
        {
        }

        public FormStarting(System.Drawing.Icon icon, string Target = "")
        {
            InitializeComponent();
            this.Icon = icon;
            this.pictureBox.Image = icon.ToBitmap();
            if (Target.Length > 0)
            {
                this.Text = Target;
                this.label.Text = "Starting " + Target;
            }
        }

        public FormStarting(System.Drawing.Bitmap bitmap, string Target = "")
        {
            InitializeComponent();
            IntPtr Hicon = bitmap.GetHicon();
            Icon icon = Icon.FromHandle(Hicon);
            this.Icon = icon;
            this.pictureBox.Image = bitmap;
            if (Target.Length > 0)
            {
                this.Text = Target;
                if (Target.Length > 0)
                {
                    if (Target.Length > 10)
                    {
                        this.label.Text = Target.Substring(0, Target.IndexOf(" ")) + "...";
                    }
                    else
                    {
                        this.label.Text = Target;
                    }
                }
                else
                    this.label.Text = "Starting";
            }
        }

        public void setMax(int Max)
        {
            this.progressBar.Maximum = Max;
        }

        public void setEnd() { this.progressBar.Value = this.progressBar.Maximum; }

        public void ShowCurrentStep(string CurrentStep)
        {
            this.labelCurrentStep.Text = CurrentStep;
            if (this.progressBar.Value < this.progressBar.Maximum)
                this.progressBar.Value++;
            Application.DoEvents();
            this.CatchTime(CurrentStep);
        }

        private void CatchTime(string CurrentStep)
        {
            if (this._Times == null)
                this._Times = new Dictionary<string, DateTime>();
            string Step = (this._Times.Count + 1).ToString() + " " + CurrentStep;
            this._Times.Add(Step, System.DateTime.Now);
        }

        private void FormStarting_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this._Times != null && this._Times.Count > 0)
            {
                string Message = "";
                DateTime? previous = null;
                string PreviousStep = null;
                foreach (System.Collections.Generic.KeyValuePair<string, DateTime> keyValue in this._Times)
                {
                    if (previous == null)
                    {
                        previous = keyValue.Value;
                        PreviousStep = keyValue.Key;
                        Message += keyValue.Value.ToShortTimeString() + "\r\n";
                    }
                    else
                    {
                        double TimeNeeded = (keyValue.Value - (DateTime)previous).TotalSeconds;
                        Message += System.Math.Round(TimeNeeded, 2).ToString() + " s:\t" + PreviousStep + "\r\n";
                        previous = keyValue.Value;
                        PreviousStep = keyValue.Key;
                    }
                }
                double LastTimeNeeded = (_Times.Last().Value - (DateTime)previous).TotalSeconds;
                Message += System.Math.Round(LastTimeNeeded, 2).ToString() + " s:\t" + _Times.Last().Key + "\r\n";
                if (Message.Length > 0)
                {
#if DEBUG
                    //System.Windows.Forms.MessageBox.Show(Message);
#endif
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("Start", "FormStarting", Message, true, true);
                }
            }
        }

    }
}
