using System;
using System.Windows.Forms;
public class WebServiceProgressDialog : Form
{
    private Label lblMessage;
    public WebServiceProgressDialog(string message)
    {
        InitializeComponent(message);
    }
    private void InitializeComponent(string message)
    {
        this.lblMessage = new Label();
        this.SuspendLayout();
        // Label
        this.lblMessage.AutoSize = true;
        this.lblMessage.Location = new System.Drawing.Point(12, 20);
        this.lblMessage.Name = "lblMessage";
        this.lblMessage.Size = new System.Drawing.Size(500, 20);
        this.lblMessage.Text = message;
        // ProgressDialog
        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 80);
        this.Controls.Add(this.lblMessage);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "Please Wait";
        this.ControlBox = false; // Disable close button
        this.ResumeLayout(false);
        this.PerformLayout();
    }
    public void UpdateMessage(string message)
    {
        if (this.lblMessage.InvokeRequired)
        {
            this.lblMessage.Invoke(new Action(() => this.lblMessage.Text = message));
        }
        else
        {
            this.lblMessage.Text = message;
        }
    }
}

