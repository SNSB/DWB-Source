using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.UserControls
{
    public partial class UserControlGeometry : UserControl
    {
        public enum DisplayStyle { Points, Line, Polygon, Area };
        private System.Collections.Generic.Dictionary<System.Data.DataTable, DisplayStyle> _TableList;
        private System.Collections.Generic.List<System.Drawing.Color> _ColorList;
        private System.Collections.Generic.List<float> _LineWidthList;
        private string _xColumn;
        private string _yColumn;
        private float _xMin;
        private float _xMax;
        private float _yMin;
        private float _yMax;

        public UserControlGeometry()
        {
            InitializeComponent();
        }

        public void initGeometry(string xColumnName, string yColumnName,
            ref System.Collections.Generic.Dictionary<System.Data.DataTable, DisplayStyle> TableList,
            System.Collections.Generic.List<System.Drawing.Color> ColorList,
            System.Collections.Generic.List<float> LineWidthList)
        {
            this._ColorList = ColorList;
            this._LineWidthList = LineWidthList;
            this._TableList = TableList;
            this._xColumn = xColumnName;
            this._yColumn = yColumnName;
            this.pictureBox.Refresh();
            int i = 0;
            foreach (System.Collections.Generic.KeyValuePair<System.Data.DataTable, DisplayStyle> KV in TableList)
            {
                this.drawTableContent(KV.Key, KV.Value, ColorList[i], LineWidthList[i]);
                i++;
            }
        }

        private void drawTableContent(System.Data.DataTable Table, DisplayStyle Style, System.Drawing.Color Color, float Width)
        {
            System.Collections.Generic.List<System.Drawing.PointF> Points = new List<PointF>();
            foreach (System.Data.DataRow R in Table.Rows)
            {
                System.Drawing.PointF P = new PointF();
                P.X = float.Parse(R[this._xColumn].ToString());
                P.Y = float.Parse(R[this._yColumn].ToString());
                Points.Add(P);
            }
            if (Points.Count > 0)
            {
                this.initDrawingRange(Points);
                this.drawObject(Style, Points, Color, Width);
            }
        }

        private void initDrawingRange(System.Collections.Generic.List<System.Drawing.PointF> Points)
        {
            foreach (System.Drawing.PointF P in Points)
            {
                if (this._xMax < P.X) this._xMax = P.X;
                if (this._xMin > P.X) this._xMin = P.X;
                if (this._yMax < P.Y) this._yMax = P.Y;
                if (this._yMin > P.Y) this._yMin = P.Y;
            }
        }

        private System.Drawing.Point PositionOfPointInControl(System.Drawing.PointF Point, System.Windows.Forms.Control Control)
        {
            System.Drawing.Point P = new Point();
            float NewX = (Point.X - this._xMin) / (this._xMax - this._xMin);
            P.X = (int)(NewX * Control.Width);
            float NewY = (Point.Y - this._xMin) / (this._yMax - this._yMin);
            P.Y = (int)(NewY * Control.Width);
            return P;
        }

        private void drawObject(
            DisplayStyle DisplayStyle,
            System.Collections.Generic.List<System.Drawing.PointF> Points,
            System.Drawing.Color Color,
            float LineWidth)
        {
            System.Drawing.PointF[] PointArray = Points.ToArray();

            this.pictureBox.Refresh();
            System.Drawing.Graphics Graphic = this.CreateGraphics();
            System.Drawing.Pen Pen = new Pen(Color, LineWidth);
            switch (DisplayStyle)
            {
                case DisplayStyle.Points:
                    foreach (System.Drawing.PointF P in Points)
                    {
                        System.Drawing.Point iP = this.PositionOfPointInControl(P, this.pictureBox);
                        Graphic.DrawEllipse(Pen, iP.X + 100, iP.Y + 100, 2, 2);
                        Graphic.DrawEllipse(Pen, (int)iP.X + 200, (int)iP.Y + 200, 2, 2);
                        Graphic.DrawEllipse(Pen, (int)iP.X - 200, (int)iP.Y - 200, 2, 2);
                        Graphic.DrawRectangle(Pen, 10, 10, 10, 10); // Test
                    }
                    break;
                case DisplayStyle.Line:
                    Graphic.DrawLines(Pen, Points.ToArray());
                    break;
                case DisplayStyle.Polygon:
                    break;
                case DisplayStyle.Area:
                    break;
            }
            Graphic.DrawLine(Pen, 10, 10, 20, 20);
            System.Drawing.Font F = new Font(FontFamily.GenericSansSerif, 20);
            float x = 150.0f;
            float y = 50.0f;
            int i = 600;
            Graphic.DrawRectangle(Pen, i, i, i, i); // Test
            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

            Graphic.DrawString("Test", F, drawBrush, x, y);
            foreach (System.Drawing.PointF P in Points)
            {
                Graphic.DrawEllipse(Pen, P.X + 100, P.Y + 100, 2, 2);
                Graphic.DrawEllipse(Pen, (int)P.X + 200, (int)P.Y + 200, 2, 2);
                Graphic.DrawEllipse(Pen, (int)P.X - 200, (int)P.Y - 200, 2, 2);
            }
            //Graphic.DrawLines(Pen, Points.ToArray());
        }
    }
}
