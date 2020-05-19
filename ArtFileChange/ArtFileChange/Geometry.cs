using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ArtFileChange
    {
    class Geometry
        {

        /// <summary>
        /// Function that controls bounds of geometric figures in a plane.
        /// </summary>
        /// <param name="bounds"></param>
        /// <param name="rad"></param>
        /// <returns></returns>
        public GraphicsPath RoundedRectanglePath(Rectangle bounds, int rad)
            {
            var diameter = rad * 2;
            var size = new Size(diameter, diameter);
            var arc = new Rectangle(bounds.Location, size);
            var path = new GraphicsPath();

            if (rad == 0)
                {
                path.AddRectangle(bounds);
                return path;
                }

            path.AddArc(arc, 180, 90);
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure(); //be sure to close the figure

            return path;
            }

        public void GetBeanCurveRegion(PaintEventArgs e, Control pn)
            {
            e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.Default;

            //var pn = (Button)sender;

            var radius = 50;
            float r2 = radius / 2f;
            GraphicsPath GraphPath = new GraphicsPath();
            RectangleF Rect = new RectangleF(0, 0, pn.Width, pn.Height);
            GraphPath.AddArc(Rect.X, Rect.Y, radius, radius, 180, 90);
            GraphPath.AddLine(Rect.X + r2, Rect.Y, Rect.Width - r2, Rect.Y);
            GraphPath.AddArc(Rect.X + Rect.Width - radius, Rect.Y, radius, radius, 270, 90);
            GraphPath.AddLine(Rect.Width, Rect.Y + r2, Rect.Width, Rect.Height - r2);
            GraphPath.AddArc(Rect.X + Rect.Width - radius,
                             Rect.Y + Rect.Height - radius, radius, radius, 0, 90);
            GraphPath.AddLine(Rect.Width - r2, Rect.Height, Rect.X + r2, Rect.Height);
            GraphPath.AddArc(Rect.X, Rect.Y + Rect.Height - radius, radius, radius, 90, 90);
            GraphPath.AddLine(Rect.X, Rect.Height - r2, Rect.X, Rect.Y + r2);

            GraphPath.CloseFigure();

            pn.Region = new Region(GraphPath);

            var pen = new Pen(Color.PowderBlue, 4f);
            pen.Alignment = PenAlignment.Right;
            e.Graphics.DrawPath(pen, GraphPath);
            }

        /// <summary>
        /// Color invertor that calls hexcone model.
        /// </summary>
        /// <param name="ColourToInvert"></param>
        /// <returns></returns>
        public Color InvertedColor(Color ColourToInvert)
            {
            return Color.FromArgb((byte)~ColourToInvert.R, (byte)~ColourToInvert.G, (byte)~ColourToInvert.B);
            }
        }
    }
