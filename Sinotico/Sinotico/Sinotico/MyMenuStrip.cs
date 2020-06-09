using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sinotico
    {
    class MyMenuStrip : Panel
        {
        public MyMenuStrip()
            {
            BackColor = Color.WhiteSmoke;
            BorderStyle = BorderStyle.None;
            Cursor = Cursors.Hand;
            SetStyle(
               ControlStyles.UserPaint |
               ControlStyles.AllPaintingInWmPaint |
               ControlStyles.OptimizedDoubleBuffer,
               true);
            }

        protected override void OnControlAdded(ControlEventArgs e)
            {
            var height = 0;

            for (var i = 0; i <= Controls.Count - 1; i++)
                {
                //take added control heights
                var ctrl = this.Controls[i];
                //increment height by cumulative value of controls' height
                height += ctrl.Height;
                }

            Size = new Size(250, height);

            base.OnControlAdded(e);
            }

        /// <summary>
        /// Correct container position with direct casting of caller control.
        /// Applied changes works only to the owner-parental form.
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="owner"></param>
        public void InitializeContainer(Control ctrl, Form owner)
            {
            if (Controls.Count != 0)
                {
                //remove all added controls from this first
                Controls.Clear();

                foreach (Control c in Controls.OfType<Button>())
                    {
                    Controls.Remove(c);
                    }
                }

            //set menu modified location under the casted button          
            Location = new Point(ctrl.Location.X +
                (Width / 2 - ctrl.Width / 2) - 50, ctrl.Location.Y + ctrl.Height + 1);

            owner.Controls.Add(this);

            BringToFront();
            }
        }
    internal class MyMenuItem : Button
        {
        public MyMenuItem(string text)
            {
            Text = text;
            ForeColor = Color.DimGray;
            BackColor = Color.WhiteSmoke;
            Font = new Font("Segoe UI", 9, FontStyle.Bold);
            Dock = DockStyle.Top;
            Height = 45;
            AutoSize = false;
            TextAlign = ContentAlignment.MiddleLeft;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 1;
            FlatAppearance.BorderColor = Color.DarkGray;
            ImageAlign = ContentAlignment.MiddleLeft;
            }
        }
    }
