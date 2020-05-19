using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

namespace ArtFileChange
    {
    public partial class Operators : Form
        {
        public Operators()
            {
            InitializeComponent();
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

        private void Operators_Load(object sender, EventArgs e)
            {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToResizeColumns = false;

            LoadJobTypes();
            LoadOperators();

            txtName.TextSize = 50;
            txtCode.TextSize = 4;
            }

        private void LoadOperators()
            {
            var dt = new System.Data.DataTable();

            if (dataGridView1.DataSource != null) dataGridView1.DataSource = null;

            var q = "select * from operators";
            
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

            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Width = 60;
            dataGridView1.Columns[2].Width = 150;
            dataGridView1.Columns[3].Width = 150;
            }

        private void LoadJobTypes()
            {
            comboBox1.Items.Clear();
            _jobTypes.Clear();

            var q = "select * from jobtypes";
            using (var con = new SqlConnection(Config.ConString))
                {
                var cmd = new SqlCommand(q, con);

                con.Open();
                var dr = cmd.ExecuteReader();
                if (dr.HasRows)
                    {
                    while (dr.Read())
                        {
                        _jobTypes.Add(Convert.ToInt32(dr[0]), dr[1].ToString());
                    }
                    }
                con.Close();
                dr.Close();
                cmd = null;

                comboBox1.DataSource = new BindingSource(_jobTypes, null);
                comboBox1.ValueMember = "key";
                comboBox1.DisplayMember = "value";
            }
            }

        private Dictionary<int, string> _jobTypes = new Dictionary<int, string>();
        private void InsertNewOperator()
            {
            var newOper = txtName.UserText;
            var newCode = txtCode.UserText;
            var newJobType = comboBox1.Text;
            var newJobId = (int)comboBox1.SelectedValue;
            var newEmail = txtEmail.Text;

            var i = 0;
            if (dataGridView1.Rows.Count > 0)
                {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                    var code = row.Cells[1].Value.ToString();

                    if (newCode == code) i++;
                    }
                }

            if (i > 0)
                {
                MessageBox.Show("Operator already exist.");
                return;
                }

            var q = "insert into operators (code,fullname,operatorinfo,jobtype_id,email) values (@p1, @p2, @p3,@p4,@p5)";

            using (var con = new SqlConnection(Config.ConString))
                {
                var cmd = new SqlCommand(q, con);
                cmd.Parameters.Add("@p1", SqlDbType.NVarChar).Value = newCode;
                cmd.Parameters.Add("@p2", SqlDbType.NVarChar).Value = newOper;
                cmd.Parameters.Add("@p3", SqlDbType.NVarChar).Value = newJobType;
                cmd.Parameters.Add("@p4", SqlDbType.Int).Value = newJobId;
                cmd.Parameters.Add("@p5", SqlDbType.NVarChar).Value = newEmail;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                }
            }

        private void btnGenSave_Click(object sender, EventArgs e)
            {
            var clr = txtName.BorderColor;

            if (txtCode.UserText == string.Empty 
                || txtName.UserText == string.Empty
                || comboBox1.Text == string.Empty )
                {
                MessageBox.Show("Must edit all fields.");
                return;
                }

            if (txtCode.UserText.Length < 4)
                {
                txtCode.BorderColor = Color.Crimson;
                txtCode.Refresh();
                MessageBox.Show("Code must have 4 digits.");
                return;
                }

            InsertNewOperator();
            LoadOperators();
            txtCode.UserText = "";
            txtName.UserText = "";
            comboBox1.Text = "";

            txtCode.BorderColor = clr;
            txtCode.Refresh();
            }
        }
    }
