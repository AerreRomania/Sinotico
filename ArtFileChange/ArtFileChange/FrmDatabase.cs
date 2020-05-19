using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArtFileChange
    {
    public partial class FrmDatabase : Form
        {
        public FrmDatabase()
            {
            InitializeComponent();
            
            myTextBox1.UseUpperCase = true;
            myTextBox2.UseUpperCase = true;
            myTextBox3.UseUpperCase = true;
            }

        protected override void OnLoad(EventArgs e)
            {
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.BackgroundColor = Color.WhiteSmoke;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;

            myTextBox1.TextSize = 2;

            LoadData();

            base.OnLoad(e);
            }

        private Geometry _geometry = new Geometry();
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

        private void LoadData()
            {
            var dt = new DataTable();
            var q = "select * from combarchive";
            using (var con = new SqlConnection(Config.ConString))
                {
                var cmd = new SqlCommand(q, con);
                con.Open();
                var dr = cmd.ExecuteReader();
                dt.Load(dr);
                con.Close();
                dr.Close();
                }

            dataGridView1.DataSource = dt;
            
            foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                row.Height = 50;
                row.DefaultCellStyle.Font = new Font("Segoe UI", 9);
                row.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                col.Width = 300;
                col.HeaderCell.Style.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "SCELT";
            dataGridView1.Columns[2].HeaderText = "DESCRIPTION";
            dataGridView1.Columns[3].HeaderText = "DESCRIPTION EXTENDED";
            }

        private void btnGenSave_Click(object sender, EventArgs e)
            {
            if (myTextBox1.UserText == "" || myTextBox2.UserText == ""
                || myTextBox3.UserText == "")
                {
                MessageBox.Show("Input isn't valid.");
                return;
                }

            var q = "insert into combarchive (scelt,description,descest) values (@param1,@param2,@param3)";

            using (var con = new SqlConnection(Config.ConString))
                {
                var cmd = new SqlCommand(q, con);
                cmd.Parameters.Add("@param1", SqlDbType.NVarChar).Value = myTextBox1.UserText;
                cmd.Parameters.Add("@param2", SqlDbType.NVarChar).Value = myTextBox2.UserText;
                cmd.Parameters.Add("@param3", SqlDbType.NVarChar).Value = myTextBox3.UserText;

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                }

            LoadData();

            myTextBox1.UserText = "";
            myTextBox2.UserText = "";
            myTextBox3.UserText = "";

            myTextBox1.Focus();
            }
        }
    }
