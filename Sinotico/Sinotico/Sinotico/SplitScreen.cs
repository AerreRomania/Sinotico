using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sinotico
{
    public partial class SplitScreen : Form
    {
        private string[] formModes = new string[] { "eff", "qty", "cleaner", "tempStd", "rammendi", "scarti" };
        public static string[] selectedModes = new string[] { };
        public static string[] _titles = new string[] { };
        private Timer _refreshWatch = null;

        public SplitScreen()
        {
            InitializeComponent();
        }

        private void SplitScreen_Load(object sender, EventArgs e)
        {
            var w = Screen.GetWorkingArea(this).Width;
            var h = Screen.GetWorkingArea(this).Height;

            float wi = w / 1920f;
            float hi = h / 1080f;

            var f = new SizeF(wi, hi);
            this.Scale(f);

            LoadLayouts();            

            _refreshWatch = new Timer();
            _refreshWatch.Interval = 300_000; //5mins
            _refreshWatch.Tick += Timer_Tick;
            _refreshWatch.Start();
        }

        private void LoadLayouts()
        {
            for (var i = 0; i <= 3; i++)
            {
                var type = "";
                var idx = 0;
                if (selectedModes[i] == string.Empty) continue;
                else if (selectedModes[i] == "Pulizia Fronture" || selectedModes[i] == "Pulizia Ordinaria" || selectedModes[i] == "cquality")
                {
                    type = selectedModes[i];
                    idx = 2;
                }
                else
                {
                    for (var j = 0; j < formModes.Length; j++)
                    {
                        if (formModes[j] == selectedModes[i])
                        {
                            idx = j;
                            break;
                        }
                    }
                }
                var frm = new FrmLayout(formModes[idx], type);
                var parent = tblPanel.Controls.Find("pnl" + (i + 1).ToString(), true).FirstOrDefault() as Panel;
                frm.TopLevel = false;
                frm.FormBorderStyle = FormBorderStyle.None;
                frm.WindowState = FormWindowState.Maximized;
                parent.Controls.Add(frm);
                frm.Show();
            }
            var counter = 0;
            foreach (var title in new Label[] { lbl_title_one, lbl_title_two, lbl_title_three, lbl_title_four })
            {
                title.Text = _titles[counter];
                counter++;
            }
        }
        private void Timer_Tick(object sender, EventArgs args)
        {
            LoadLayouts();
        }

        private void LabelsTitle_Paint(object sender, PaintEventArgs e)
        {
            var rect = e.ClipRectangle;

            e.Graphics.DrawRectangle(Pens.Gray, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
        }

        private void tblPanel_Paint(object sender, PaintEventArgs e)
        {
            foreach (var pnl in new Panel[] { pnl1, pnl2, pnl3, pnl4 })
            {
                e.Graphics.DrawRectangle(new Pen(Brushes.Gray, 1), new Rectangle(pnl.Location.X - 1, pnl.Location.Y - 1, pnl.Width + 1, pnl.Height + 1));
            }
        }
    }
}
