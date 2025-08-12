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
    public partial class FormMessageDialog : Form
    {
        private FormMessageDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Show message dialog panel with two buttons (Yes and No). The button texts are set by function parameters and
        /// </summary>
        /// <param name="message">Message text</param>
        /// <param name="caption">Window title</param>
        /// <param name="yes">Text for 'Yes' button</param>
        /// <param name="no">Text for 'No' button</param>
        /// <param name="def">Default button (accepted wit 'Enter') button1 = Yes, button2 = No</param>
        /// <param name="icon">Message box icon</param>
        /// <returns>Yes or No</returns>
        public static DialogResult Show(string message, string caption, string yes, string no, MessageBoxDefaultButton def, MessageBoxIcon icon)
        {
            FormMessageDialog MD = new FormMessageDialog();

            MD.Text = caption;
            MD.textBoxMessage.Text = message;
            MD.textBoxMessage.Select(0, 0);
            MD.buttonNo.Text = no;
            MD.buttonYes.Text = yes;
            if (def == MessageBoxDefaultButton.Button1)
                MD.buttonYes.Select();
            else
                MD.buttonNo.Select();

            switch (icon)
            {
                case MessageBoxIcon.Asterisk:
                    MD.pictureBoxIcon.Image = SystemIcons.Asterisk.ToBitmap();
                    break;
                case MessageBoxIcon.Error:
                    MD.pictureBoxIcon.Image = SystemIcons.Error.ToBitmap();
                    break;
                case MessageBoxIcon.Exclamation:
                    MD.pictureBoxIcon.Image = SystemIcons.Exclamation.ToBitmap();
                    break;
                case MessageBoxIcon.Question:
                    MD.pictureBoxIcon.Image = SystemIcons.Question.ToBitmap();
                    break;
                default:
                    MD.splitContainerMain.Panel1Collapsed = true;
                    break;
            }

            if (MD.ShowDialog() == DialogResult.Yes)
                return DialogResult.Yes;
            else
                return DialogResult.No;
        }

        /// <summary>
        /// Simple dialog to enable extraction of message text. Only one botton with OK displayed
        /// </summary>
        /// <param name="message">Message that may be copied</param>
        /// <param name="caption">Window title</param>
        /// <param name="icon">Message box icon</param>
        /// <returns>Yes</returns>
        public static DialogResult Show(string message, string caption, MessageBoxIcon icon)
        {
            FormMessageDialog MD = new FormMessageDialog();

            MD.Text = caption;
            MD.textBoxMessage.Text = message;
            MD.textBoxMessage.Select(0, 0);
            MD.buttonNo.Text = "OK";
            MD.buttonYes.Visible = false;
            MD.buttonNo.Select();
            MD.Height = MD.MinimumSize.Height;

            switch (icon)
            {
                case MessageBoxIcon.Asterisk:
                    MD.pictureBoxIcon.Image = SystemIcons.Asterisk.ToBitmap();
                    break;
                case MessageBoxIcon.Error:
                    MD.pictureBoxIcon.Image = SystemIcons.Error.ToBitmap();
                    break;
                case MessageBoxIcon.Exclamation:
                    MD.pictureBoxIcon.Image = SystemIcons.Exclamation.ToBitmap();
                    break;
                case MessageBoxIcon.Question:
                    MD.pictureBoxIcon.Image = SystemIcons.Question.ToBitmap();
                    break;
                default:
                    MD.splitContainerMain.Panel1Collapsed = true;
                    break;
            }
            MD.ShowDialog();
            return DialogResult.Yes;
        }

        private void buttonNo_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonYes_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
