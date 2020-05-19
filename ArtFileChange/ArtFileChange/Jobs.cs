using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArtFileChange
    {
    public partial class Jobs : Form
        {
        public Jobs()
            {
            InitializeComponent();
            }

        private void Jobs_Load(object sender, EventArgs e)
            {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToResizeColumns = false;

            LoadJobType();
            }

        private void LoadJobType()
            {
            var dt = new DataTable();
            if (dataGridView1.DataSource != null) dataGridView1.DataSource = null;

            var q = "select * from jobtypes";

            using (var con = new System.Data.SqlClient.SqlConnection(Config.ConString))
                {
                var cmd = new System.Data.SqlClient.SqlCommand(q, con);

                con.Open();
                var dr = cmd.ExecuteReader();
                dt.Load(dr);
                con.Close();
                dr.Close();
                }

            dataGridView1.DataSource = dt;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Width = 200;
            }

        private void AddNewJobType()
            {
            var newJob = txtName.UserText;

            var i = 0;
            if (dataGridView1.Rows.Count > 0)
                {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                    var exJob = row.Cells[1].Value.ToString();

                    if (newJob == exJob)
                        {
                        i++;
                        }
                    }
                }

            if (i > 0)
                {
                MessageBox.Show("Type already exist.");
                return;
                }

            var q = "insert into jobtypes (jobname) values (@param1)";

            using (var con = new System.Data.SqlClient.SqlConnection(Config.ConString))
                {
                var cmd = new System.Data.SqlClient.SqlCommand(q, con);
                cmd.Parameters.Add("@param1", SqlDbType.NVarChar).Value = newJob;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                }

            txtName.UserText = string.Empty;
            }

        private void btnGenSave_Click(object sender, EventArgs e)
            {
            if (txtName.UserText == string.Empty) return;
            AddNewJobType();
            LoadJobType();
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
        }
    }
