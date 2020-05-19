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
    public partial class RepScartiRammendi : Form
    {
        public static int Machine { get; set; }
        public static string ReportMode { get; set; }

        public RepScartiRammendi()
        {
            InitializeComponent();
        }
        public RepScartiRammendi(int mac, string mode)
        {
            InitializeComponent();
            Machine = mac;
            ReportMode = mode;
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
            GetData(Machine, ReportMode);
            dgvReport.DoubleBufferedDataGridView(true);
            base.OnLoad(e);
        }
        private void CreateTableView(DataTable dt)
        {
            dt.Columns.Add("Macchina");
            dt.Columns.Add("Articolo");
            dt.Columns.Add("Commessa");
            dt.Columns.Add("Turno");
            dt.Columns.Add("TeliBuoni");
            dt.Columns.Add("Scarti");
            dt.Columns.Add("Rammendi");
            dt.Columns.Add("Data");
        }
        private void LoadProcedure(DataTable dt, int idm)
        {
            using (var con = new SqlConnection(MainWnd.conString))
            {
                var cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                cmd.CommandText = "[getscartirammendireportdata]";
                cmd.Parameters.Add("@machine", SqlDbType.Int).Value = idm;
                cmd.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = MainWnd.Get_from_date();
                cmd.Parameters.Add("@toDate", SqlDbType.DateTime).Value = MainWnd.Get_to_date();
                cmd.Parameters.Add("@shift", SqlDbType.NVarChar).Value = MainWnd.Get_shift_array().ToString();
                con.Open();
                var dr = cmd.ExecuteReader();
                dt.Load(dr);
                con.Close();
                dr.Close();
            }
        }
        private void GetData(int idm, string mode)
        {
            var tmpTable = new DataTable();
            var finTbl = new DataTable();
            CreateTableView(finTbl);
            LoadProcedure(tmpTable, idm);

            if (tmpTable.Rows.Count <= 0 || dgvReport.DataSource != null)
            {
                dgvReport.DataSource = null;
                return;
            }

            int.TryParse(tmpTable.Rows[0][0].ToString(), out var machine);
            var article = tmpTable.Rows[0][1].ToString();
            var order = tmpTable.Rows[0][2].ToString();
            var operator_code = tmpTable.Rows[0][3].ToString();
            var shift = tmpTable.Rows[0][4].ToString();       
            //int.TryParse(tmpTable.Rows[0][5].ToString(), out var tmpTeli);
            int.TryParse(tmpTable.Rows[0][5].ToString(), out var tmpScarti);
            int.TryParse(tmpTable.Rows[0][6].ToString(), out var tmpRammendi);
            DateTime.TryParse(tmpTable.Rows[0][7].ToString(), out var startDate);

            foreach (DataRow row in tmpTable.Rows)
            {
                int.TryParse(row[0].ToString(), out var mac);
                var art = row[1].ToString();
                var ord = row[2].ToString();
                var operCode = row[3].ToString();
                var sh = row[4].ToString();
                //int.TryParse(row[5].ToString(), out var teli);
                int.TryParse(row[5].ToString(), out var scarti);
                int.TryParse(row[6].ToString(), out var ramm);
                DateTime.TryParse(row[7].ToString(), out var date);

                if (startDate != date || order != ord || operator_code != operCode || shift != sh || article != art)
                {
                    if (mode == "scarti")
                    {
                        if (tmpScarti != 0)
                        {
                            var newRow = finTbl.NewRow();
                            newRow[0] = machine;
                            newRow[1] = article;
                            newRow[2] = order;
                            newRow[3] = shift;
                            newRow[4] = 0;
                            newRow[5] = tmpScarti;
                            newRow[6] = tmpRammendi;
                            newRow[7] = startDate.ToShortDateString();
                            finTbl.Rows.Add(newRow);
                        }
                    }
                    else if (mode == "rammendi")
                    {
                        if (tmpRammendi != 0)
                        {
                            var newRow = finTbl.NewRow();
                            newRow[0] = machine;
                            newRow[1] = article;
                            newRow[2] = order;
                            newRow[3] = shift;
                            newRow[4] = 0;
                            newRow[5] = tmpScarti;
                            newRow[6] = tmpRammendi;
                            newRow[7] = startDate.ToShortDateString();
                            finTbl.Rows.Add(newRow);
                        }
                    }
                }
                machine = mac;
                shift = sh;
                operator_code = operCode;
                order = ord;
                article = art;
                //tmpTeli = teli;
                tmpRammendi = ramm;
                tmpScarti = scarti;
                startDate = date;
            }
            var lastNewRow = finTbl.NewRow();
            lastNewRow[0] = machine;
            lastNewRow[1] = article;
            lastNewRow[2] = order;
            lastNewRow[3] = shift;
            lastNewRow[4] = 0;
            lastNewRow[5] = tmpScarti;
            lastNewRow[6] = tmpRammendi;
            lastNewRow[7] = startDate.ToShortDateString();
            finTbl.Rows.Add(lastNewRow);
            dgvReport.DataSource = finTbl;
            dgvReport.Columns["TeliBuoni"].Visible = false;
        }
    }
}
