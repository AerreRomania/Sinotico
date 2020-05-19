using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Linq;

namespace ArtFileChange
    {
    public partial class FrmArticleCombination : Form
        {
        private Geometry _geometry = new Geometry();

        public FrmArticleCombination()
            {
            InitializeComponent();

            LoadParts();
            }

        private void FrmArticleCombination_Load(object sender, EventArgs e)
            {
            lblTArt.Text = FrmRename.TargetArticle;
            lblPart.Text = FrmRename.TargetPart;

            _lstParts = new List<string>();
            LoadList();

            if (!_lstParts.Contains(lblPart.Text))
                {
                _lstParts.Add(lblPart.Text);
                }

            CreateTags();

            btnGenSave.Click += (s, generx) => {
                Close();
            };
            }

        private void btnGenSave_Paint(object sender, PaintEventArgs e)
            {
            var btn = (Button)sender;

            var pen = new Pen(new SolidBrush(Color.White), 4);

            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(0, 0, btn.Width, btn.Height), 7))
                {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                //e.Graphics.FillPath(Brushes.White, path);
                e.Graphics.DrawPath(pen, path);

                btn.Region = new Region(path);
                e.Graphics.SmoothingMode = e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                }
            }

        private void LoadParts()
            {
            myTextBox1.Collection = new ComboBox.ObjectCollection(myTextBox1.comboBoxControl);

            var q = "select scelt from combarchive";
            using (var con = new SqlConnection(Config.ConString))
                {
                var cmd = new SqlCommand(q, con);

                con.Open();
                var dr = cmd.ExecuteReader();
                if (dr.HasRows)
                    {
                    while (dr.Read())
                        {
                        myTextBox1.Collection.Add(dr[0].ToString());
                        }
                    }
                con.Close();
                dr.Close();
                cmd = null;
                }
            }

        private void button1_Paint(object sender, PaintEventArgs e)
            {
            var btn = (Button)sender;

            var pen = new Pen(new SolidBrush(Color.White), 4);

            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(0, 0, btn.Width, btn.Height), 7))
                {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                //e.Graphics.FillPath(Brushes.White, path);
                e.Graphics.DrawPath(pen, path);

                btn.Region = new Region(path);
                e.Graphics.SmoothingMode = e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                }
            }
       
        private List<string> _lstParts = new List<string>();
        private void button1_Click(object sender, EventArgs e)
            {
            if (myTextBox1.UserText == "") return;

            if (_lstParts.Contains(myTextBox1.UserText))
                {
                MessageBox.Show("Part already exist.");
                }
            else
                {
                _lstParts.Add(myTextBox1.UserText);
                }
            
            CreateTags();
            }

        private int topObjLocation = 5;
        private int leftObjLocation = 5;
        private int count = 0;

        private void CreateTags()
            {
            RemoveAllTags();

            topObjLocation = 8;
            leftObjLocation = 8;
            count = 0;

            foreach (var item in _lstParts)
                {
                count++;

                var pn = new Panel();
                var lbl = new Label();
                var btn = new Button();

                pn.Cursor = Cursors.Hand;
                lbl.Cursor = Cursors.Hand;
                btn.Cursor = Cursors.Hand;

                pn.Size = new Size(50, 20);
                pn.BackColor = Color.SteelBlue;
                pn.Paint += (s, e) => {

                    var p = (Panel)s;

                    var pen = new Pen(new SolidBrush(Color.Blue), 4);

                    using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(-1, -1, p.Width, p.Height), 4))
                        {
                        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        //e.Graphics.FillPath(Brushes.White, path);
                        e.Graphics.DrawPath(pen, path);

                        p.Region = new Region(path);
                        e.Graphics.SmoothingMode = e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                        }
                };

                pn.MouseEnter += (s, e) => {
                    pn.BackColor = Color.DeepSkyBlue;
                    btn.BackColor = Color.DeepSkyBlue;
                    lbl.BackColor = Color.DeepSkyBlue;
                };
                pn.MouseLeave += (s, e) => {
                    pn.BackColor = Color.SteelBlue;
                    btn.BackColor = Color.SteelBlue;
                    lbl.BackColor = Color.SteelBlue;
                };
                btn.MouseEnter += (s, e) => {
                    pn.BackColor = Color.DeepSkyBlue;
                    btn.FlatAppearance.MouseOverBackColor = Color.DeepSkyBlue;
                    lbl.BackColor = Color.DeepSkyBlue;
                };
                btn.MouseLeave += (s, e) => {
                    pn.BackColor = Color.SteelBlue;
                    btn.BackColor = Color.SteelBlue;
                    lbl.BackColor = Color.SteelBlue;
                };
                lbl.MouseEnter += (s, e) => {
                    pn.BackColor = Color.DeepSkyBlue;
                    btn.BackColor = Color.DeepSkyBlue;
                    lbl.BackColor = Color.DeepSkyBlue;
                };
                lbl.MouseLeave += (s, e) => {
                    pn.BackColor = Color.SteelBlue;
                    btn.BackColor = Color.SteelBlue;
                    lbl.BackColor = Color.SteelBlue;
                };

                lbl.Text = item;
                lbl.ForeColor = Color.White;
                lbl.BackColor = Color.SteelBlue;
                lbl.Font = new Font("Segoe Ui", 9, FontStyle.Regular);
                lbl.Dock = DockStyle.Left;
                lbl.AutoSize = false;
                lbl.Width = 30;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.BackColor = Color.SteelBlue;
                btn.ForeColor = Color.White;
                btn.Text = "X";
                btn.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);
                btn.TextAlign = ContentAlignment.MiddleCenter;
                btn.Dock = DockStyle.Right;
                btn.Width = 20;
                pn.Controls.Add(lbl);
                pn.Controls.Add(btn);

                btn.Click += (s, e) => {
                    _lstParts.Remove(lbl.Text);

                    CreateTags();
                };

                pn.Location = new Point(leftObjLocation, topObjLocation);
                leftObjLocation += 53;

                pnTags.Controls.Add(pn);

                if (count == 5)
                    {
                    leftObjLocation = 8;
                    topObjLocation += 23;
                    count = 0;
                    }
                }        
            }

        public void RemoveAllTags()
            {
            var Panels = new List<Panel>();
            var Stack = new Stack<Control>();
            Stack.Push(pnTags);
            while (Stack.Count > 0)
                {
                Control Ctrl = Stack.Pop();
                Panels.Add(Ctrl as Panel);
                foreach (Control C in Ctrl.Controls)
                    Stack.Push(C);
                }
            foreach (Panel P in Panels)
                {
                if (P != null && P.Name != "pnTags")
                    P.Dispose();
                }
            }

        private void pnTags_Paint(object sender, PaintEventArgs e)
            {
            var btn = (Panel)sender;

            var pen = new Pen(new SolidBrush(Color.LightGray), 4);

            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(0, 0, btn.Width, btn.Height), 7))
                {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                //e.Graphics.FillPath(Brushes.White, path);
                e.Graphics.DrawPath(pen, path);

                btn.Region = new Region(path);
                e.Graphics.SmoothingMode = e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                }
            }

        private void LoadList()
            {
            var q = "select * from artcomb where article='" + FrmRename.TargetArticle + "'";
            var strTags = "";

            using (var con = new SqlConnection(Config.ConString))
                {
                var cmd = new SqlCommand(q, con);
                con.Open();

                var dr = cmd.ExecuteReader();

                if (dr.HasRows)
                    {
                    while (dr.Read())
                        {
                        strTags = dr[2].ToString();
                        }
                    }

                con.Close();
                dr.Close();
                }

            if (strTags.Length == 0) return;

            var tagsList = strTags.Split(',').ToList();

            foreach (var item in tagsList)
                _lstParts.Add(item);         
            }

        private void btnGenSave_Click(object sender, EventArgs e)
            {
            if (_lstParts.Count <= 0)
                {
                MessageBox.Show("Deleting all tags to that article can cause calculation problems to the main software's reporter.");
                return;
                };

            var sb = new System.Text.StringBuilder();
            var output = "";
            foreach (var item in _lstParts)
                { 
                sb.Append(item + ",");
                }

            output = sb.ToString().Remove(sb.ToString().Length - 1, 1);
            
            var q = "delete from artcomb where article='" + FrmRename.TargetArticle + "'";
            var u = "insert into artcomb (article,comb,note) values (@param1,@param2,@param3)";

            using (var con = new SqlConnection(Config.ConString))
                {
                var cmd = new SqlCommand(q, con);
                con.Open();
                cmd.ExecuteNonQuery();
                //con.Close();
                cmd = null;

                cmd = new SqlCommand(u, con);
                cmd.Parameters.Add("@param1", SqlDbType.NVarChar).Value = FrmRename.TargetArticle;
                cmd.Parameters.Add("@param2", SqlDbType.NVarChar).Value = output;
                cmd.Parameters.Add("@param3", SqlDbType.NVarChar).Value = "combat";

                cmd.ExecuteNonQuery();
                con.Close();
                cmd = null;
                }
            }
        }
    }
