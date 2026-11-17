using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversitySamplingPlots
{
    public partial class FormGraphicTest : Form
    {
        public FormGraphicTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.pictureBox1.Refresh();
            float LineWidth = 1;
            System.Drawing.Color Color = System.Drawing.Color.Red;
            //System.Drawing.Graphics Graphic = this.CreateGraphics();
            System.Drawing.Pen Pen = new Pen(Color, LineWidth);
            System.Drawing.Rectangle R = new Rectangle(100, 100, 10, 10);
            System.Drawing.Graphics rectArea = this.pictureBox1.CreateGraphics();
            System.Drawing.Pen SquarePen = new System.Drawing.Pen(System.Drawing.Color.Blue, 1);
            rectArea.DrawRectangle(SquarePen, R);
            //rectArea.DrawEllipse(Pen, R);
            this.pictureBox1.Show();
            System.Drawing.Graphics rectAreaPanel = this.panel1.CreateGraphics();
            System.Drawing.Rectangle R2 = new Rectangle(200, 200, 20, 20);

            rectAreaPanel.DrawEllipse(Pen, R2);
            /*
            this.pictureBox1.Refresh();
            this.Refresh();
            System.Drawing.Graphics graphics = this.CreateGraphics();
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(
                100, 100, 200, 200);
            graphics.DrawEllipse(System.Drawing.Pens.Black, rectangle);
            graphics.DrawRectangle(System.Drawing.Pens.Red, rectangle);
            Graphic.DrawRectangle(Pen, 10, 10, 10, 10); // Test
            System.Drawing.SolidBrush brush1 = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
            System.Drawing.Graphics formGraphics = this.CreateGraphics();
            formGraphics.FillRectangle(brush1, new System.Drawing.Rectangle(0, 0, 200, 300));
            brush1.Dispose();
            formGraphics.Dispose();
            this.Refresh();
             */
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox2.Refresh();
            float LineWidth = 1;
            System.Drawing.Color Color = System.Drawing.Color.Red;
            //System.Drawing.Graphics Graphic = this.CreateGraphics();
            System.Drawing.Pen Pen = new Pen(Color, LineWidth);
            System.Drawing.Rectangle R = new Rectangle(100, 100, 10, 10);
            System.Drawing.Graphics rectArea = this.pictureBox2.CreateGraphics();
            System.Drawing.Pen SquarePen = new System.Drawing.Pen(System.Drawing.Color.Red, 1);
            rectArea.DrawRectangle(SquarePen, R);
            rectArea.DrawEllipse(Pen, R);
            this.pictureBox2.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.ImagePath = this.textBox1.Text;
        }

        public string ImagePath
        {
            get
            {
                return this.textBox1.Text;
            }
            set
            {
                bool OK = true;
                this.SuspendLayout();
                string Path = value;
                this.textBox1.Text = value;
                if (Path == "")
                {
                    this.pictureBox2.Image = null;
                }
                else
                {
                    try
                    {
                        Path = Path.Replace("/", "\\");
                        if (Path.IndexOf("http:") == 0)
                        {
                            try
                            {
                                System.Net.WebRequest webrq = System.Net.WebRequest.Create(value);
                                System.Drawing.Bitmap bmp = (Bitmap)Bitmap.FromStream(webrq.GetResponse().GetResponseStream());
                                this.pictureBox2.Image = bmp;
                            }
                            catch (System.Exception x)
                            {
                            }
                        }
                        else
                        {
                            System.IO.FileInfo File = new System.IO.FileInfo(Path);
                            if (File.Exists)
                            {
                                try
                                {
                                    System.Drawing.Bitmap bmp = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(value);
                                    this.pictureBox2.Image = bmp;
                                }
                                catch (System.Exception ex)
                                {
                                    OK = false;
                                    //System.Windows.Forms.MessageBox.Show(x.Message);
                                }
                            }
                            else
                                OK = false;
                            //MessageBox.Show("The file " + Path + " could not be found");
                        }
                    }
                    catch (System.ArgumentException ex)
                    {
                        try
                        {
                            System.Net.WebRequest webrq = System.Net.WebRequest.Create(value);
                            System.Drawing.Bitmap bmp = (Bitmap)Bitmap.FromStream(webrq.GetResponse().GetResponseStream());
                            this.pictureBox2.Image = bmp;
                        }
                        catch (System.Exception x)
                        {
                        }
                    }
                    finally
                    {
                    }
                }
                this.ResumeLayout();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // setting the webbrowser
            char HochKomma = '"';
            System.Uri URI = new Uri("http://maps.google.com/maps");
            this.webBrowser1.Url = URI;
            System.Windows.Forms.HtmlDocument Html = this.webBrowser1.Document;
            System.Windows.Forms.HtmlElement Header = Html.GetElementsByTagName("head")[0];
            //System.Windows.Forms.HtmlElement Title = Header.GetElementsByTagName("title")[0];
            //Title. = "Test";
            //string HeaderInnerHtml = @"<meta http-equiv=" + HochKomma + "content-type" + HochKomma + " content=" + HochKomma + "text/html; charset=utf-8" + HochKomma + "/> " +
            //"<title>Google Maps JavaScript API Example: Simple Polyline</title> " +
            //"<script src=" + HochKomma + "http://maps.google.com/maps?file=api&amp;v=2&amp;key=ABQIAAAAzr2EBOXUKnm_jVnk0OJI7xSosDVG8KKPE1-m51RBrvYughuyMxQ-i1QfUnH94QxWIa6N4U6MouMmBA" + HochKomma + 
            //"type=" + HochKomma + "text/javascript" + HochKomma + "></script> " +
            //"<script type=" + HochKomma + "text/javascript" + HochKomma + "> " +
            //"function initialize() { " +
            //"if (GBrowserIsCompatible()) {" +
            //"var map = new GMap2(document.getElementById(" + HochKomma + "map_canvas" + HochKomma + "));" +
            //"map.setCenter(new GLatLng(37.4419, -122.1419), 13);" +
            //"var polyline = new GPolyline([" +
            //"new GLatLng(37.4419, -122.1419)," +
            //"new GLatLng(37.4519, -122.1519)" +
            //"], " + HochKomma + "#ff0000" + HochKomma + ", 10);" +
            //"map.addOverlay(polyline);" +
            //"}</script>";
            //HeaderInnerHtml = HeaderInnerHtml.Replace(@"\", "");
            //Header.InnerHtml = HeaderInnerHtml;

            System.Windows.Forms.HtmlElement Body = Html.Body;
            Body.InnerHtml = "<div id=" + HochKomma + "map_canvas" + HochKomma + " style=" + HochKomma + "width: 500px; height: 300px" + HochKomma + "></div>" +
            "<div id=" + HochKomma + "message" + HochKomma + "></div>";
            Body.SetAttribute("onload", "initialize()");
            Body.SetAttribute("onunload", "GUnload()");
        }

    }
}
