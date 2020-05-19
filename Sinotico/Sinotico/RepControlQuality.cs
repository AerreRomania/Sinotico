using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sinotico
{
    public partial class RepControlQuality : Form
    {
        private DataTable _repData = new DataTable();
        public static int Machine { get; set; }

        public RepControlQuality()
        {
            InitializeComponent();
        }

        public RepControlQuality(int mac)
        {
            InitializeComponent();
            Machine = mac;
        }

        protected override void OnLoad(EventArgs e)
        {
            dgvReport.AllowUserToAddRows = false;
            dgvReport.AllowUserToDeleteRows = false;
            dgvReport.AllowUserToResizeColumns = false;
            dgvReport.AllowUserToResizeRows = false;
            dgvReport.AllowUserToOrderColumns = false;
            dgvReport.ReadOnly = true;
            dgvReport.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvReport.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvReport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvReport.DefaultCellStyle.BackColor = Color.FromArgb(232, 232, 232);
            dgvReport.ColumnHeadersDefaultCellStyle.BackColor = Color.Gold;            
            dgvReport.MultiSelect = false;
            dgvReport.RowsDefaultCellStyle.SelectionBackColor = Color.SteelBlue;
            dgvReport.RowsDefaultCellStyle.SelectionForeColor = Color.White;
            dgvReport.RowHeadersDefaultCellStyle.SelectionBackColor = Color.LightYellow;
            dgvReport.EnableHeadersVisualStyles = true;
            dgvReport.BorderStyle = BorderStyle.None;
            dgvReport.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            dgvReport.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvReport.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvReport.DataBindingComplete += delegate
            {
                //disallow manual sorting to follow production life-cycle
                foreach (DataGridViewColumn c in dgvReport.Columns)
                {
                    c.SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                dgvReport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                dgvReport.ColumnHeadersHeight = 40;

                dgvReport.GridColor = Color.White;
                dgvReport.RowTemplate.Height = 30;
                dgvReport.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

                dgvReport.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                dgvReport.EnableHeadersVisualStyles = false;
                dgvReport.ColumnHeadersDefaultCellStyle.BackColor = Color.Gold;

                dgvReport.RowHeadersVisible = false;
                for (var i = 0; i <= dgvReport.Columns.Count - 1; i++)
                {
                    var c = dgvReport.Columns[i];
                    c.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8);
                    c.HeaderCell.Style.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Regular);
                    c.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            };
            GetData(Machine);
            dgvReport.DoubleBufferedDataGridView(true);
            base.OnLoad(e);
        }
        private void GetData(int idm)
        {
            _repData = new DataTable();
            _repData.Columns.Add("Machine");
            _repData.Columns.Add("Articolo");
            _repData.Columns.Add("Commessa");
            _repData.Columns.Add("Componente");
            _repData.Columns.Add("Taglia");
            _repData.Columns.Add("Colore");
            _repData.Columns.Add("Cotta");
            _repData.Columns.Add("Data Inizio");
            _repData.Columns.Add("Data Fine");
            _repData.Columns.Add("Durata CQ");
            _repData.Columns.Add("Operatore");
            _repData.Columns.Add("Note");
            var tmpTable = new DataTable();
            using (var con = new SqlConnection(MainWnd.conString))
            {
                var cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                cmd.CommandText = "[getcqreportdata]";     
                cmd.Parameters.Add("@idm", SqlDbType.Int).Value = idm;
                cmd.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = MainWnd.Get_from_date();
                cmd.Parameters.Add("@toDate", SqlDbType.DateTime).Value = MainWnd.Get_to_date();
                con.Open();
                var dr = cmd.ExecuteReader();
                tmpTable.Load(dr);
                con.Close();
                dr.Close();
            }
            if (tmpTable.Rows.Count <= 0)
            {
                dgvReport.DataSource = _repData;
                return;
            }                       
            foreach (DataRow row in tmpTable.Rows)
            {
                var machine = row[0].ToString();
                var art = row[1].ToString();
                var commessa = row[2].ToString();
                var comp = row[3].ToString();
                var taglia = row[4].ToString();
                var col = row[5].ToString();
                var cotta = row[6].ToString();
                DateTime.TryParse(row[7].ToString(), out var startTime);
                DateTime.TryParse(row[8].ToString(), out var endTime);
                var tsStart = TimeSpan.FromTicks(startTime.Ticks);
                var tsEnd = TimeSpan.FromTicks(endTime.Ticks);
                var durationTime = TimeSpan.FromTicks(tsEnd.Ticks - tsStart.Ticks);
                var note = row[9].ToString();
                var op = row[10].ToString();
                var newRow = _repData.NewRow();

                newRow[0] = machine;
                newRow[1] = art;
                newRow[2] = commessa;
                newRow[3] = comp;
                newRow[4] = taglia;
                newRow[5] = col;
                newRow[6] = cotta;
                newRow[7] = startTime;
                newRow[8] = endTime;
                newRow[9] = durationTime;
                newRow[10] = op;
                newRow[11] = note;
                _repData.Rows.Add(newRow);
            }
            dgvReport.DataSource = _repData;
            dgvReport.Columns["Note"].Width = 330;
            dgvReport.Columns["Data Inizio"].Width = 130;
            dgvReport.Columns["Data Fine"].Width = 130;
            dgvReport.Columns["Taglia"].Width = 50;
            dgvReport.Columns["Machine"].Width = 60;
            dgvReport.Columns["Commessa"].Width = 70;
            dgvReport.Columns["Taglia"].Width = 60;
            dgvReport.Columns["Cotta"].Width = 50;
            dgvReport.Columns["Operatore"].Width = 170;
            dgvReport.Columns["Colore"].Width = 60;
        }
    }
}
