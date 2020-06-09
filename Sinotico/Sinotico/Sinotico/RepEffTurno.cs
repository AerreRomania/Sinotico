using Sinotico.DatabaseTableClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace Sinotico
    {
    public partial class RepEffTurno : Form
        {
        private DataTable _dt1 = new DataTable();
        private DataTable _dt2 = new DataTable();
        private DataTable _dt3 = new DataTable();

        private DataTable _table_report = new DataTable();

        private List<DateTime> _list_of_dates = new List<DateTime>();

        private Font _up_title_font = new Font("Tahoma", 12, FontStyle.Regular);
        private Font _down_title_font = new Font("Tahoma", 7, FontStyle.Regular);

        private StringBuilder _machines_array = new StringBuilder();
        private int _from_machine;
        private int _to_machine;

        public RepEffTurno()
            {
            InitializeComponent();

            dgvReport.AllowUserToAddRows = false;
            dgvReport.AllowUserToDeleteRows = false;
            dgvReport.AllowUserToResizeColumns = false;
            dgvReport.AllowUserToResizeRows = false;
            dgvReport.AllowUserToOrderColumns = false;
            dgvReport.ReadOnly = true;
            }

        private DateTime GetDateFrom
        {
            get { return new DateTime(dtpFrom.Value.Year, dtpFrom.Value.Month, dtpFrom.Value.Day); }
            set { dtpFrom.Value = value; }
        }
        private DateTime GetDateTo
        {
            get { return new DateTime(dtpTo.Value.Year, dtpTo.Value.Month, dtpTo.Value.Day); }
            set { dtpTo.Value = value; }
        }

        private void RepEffTurno_Load(object sender, EventArgs e)
            {
            dgvReport.DoubleBufferedDataGridView(true);

            dgvReport.DataBindingComplete += delegate
                {
                    dgvReport.Focus();
                    foreach (DataGridViewColumn col in dgvReport.Columns)
                        col.SortMode = DataGridViewColumnSortMode.NotSortable;
                    // total row
                    if (dgvReport.Rows.Count >= 1)
                        {
                        dgvReport.Rows[0].ReadOnly = true;
                        dgvReport.Rows[0].Frozen = true;

                        dgvReport.Rows[0].DefaultCellStyle.Font = new Font("Tahoma", 8, FontStyle.Bold);
                        dgvReport.Rows[0].DefaultCellStyle.ForeColor = Color.Red;
                        dgvReport.Rows[0].DefaultCellStyle.BackColor = Color.White;
                        dgvReport.Rows[0].DefaultCellStyle.SelectionBackColor = Color.White;
                        dgvReport.Rows[0].DefaultCellStyle.SelectionForeColor = dgvReport.Rows[0].DefaultCellStyle.ForeColor;
                        }

                    // specialize filter column
                    dgvReport.Columns[0].Width = 200;
                    dgvReport.Columns[0].HeaderCell.Style.Font = new Font("Tahoma", 10, FontStyle.Regular);
                    dgvReport.Columns[0].HeaderCell.Style.ForeColor = Color.MidnightBlue;
                    dgvReport.Columns[0].HeaderCell.Style.BackColor = Color.White;
                    dgvReport.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvReport.Columns[0].HeaderCell.Style.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;

                    for (var c = 1; c <= dgvReport.Columns.Count - 1; c++)
                        {
                        dgvReport.Columns[c].Width = 90;
                        dgvReport.Columns[c].HeaderCell.Style.Font = new Font("Tahoma", 7);
                        dgvReport.Columns[c].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgvReport.Columns[c].HeaderCell.Style.Alignment =
                        DataGridViewContentAlignment.BottomLeft;
                        }

                    dgvReport.Columns[0].Frozen = true;

                    LoadingInfo.CloseLoading();
                    };

            _from_machine = 1;
            _to_machine = 210;
            GetMachinesArray();

            LoadReport();
            
            cboBlocks.Items.Add("<All>");
            cboLines.Items.Add("<All>");
            for (var i = 1; i <= 15; i++)
                {
                if (i < 4)
                    {
                    cboBlocks.Items.Add("Squadra " + i.ToString());
                    }

                cboLines.Items.Add("LINEA " + i.ToString());
                }

            LoadingInfo.CloseLoading();
            }

        private void CreateDataTable()
            {
            _table_report = new DataTable();

            var columns = new string[]
               {
                    "DATA",
                    "macch. che lav.",
                    "Comp. Macch. Ferme",
                    "nr macch.ferme",
                    "macch. che lav.1",
                    "Comp. Macch. Ferme1",
                    "nr macch.ferme1",
                    "macch. che lav2.",
                    "Comp. Macch. Ferme2",
                    "nr macch.ferme2",
                    "macch. che lav.3",
                    "Comp. Macch. Ferme3",
                    "nr macch.ferme3",
                   };

            for (var c = 0; c < columns.Length; c++)
                {
                _table_report.Columns.AddRange(new DataColumn[] { new DataColumn(columns[c]) });
                }
            }

        private double _daysRange = 0.0;
        private int machinesNr = 210;
        private void LoadProcedure(string shift, DataTable dt)
        {
            SqlDataReader dr;
            
            if (string.IsNullOrEmpty(cboLines.Text) && string.IsNullOrEmpty(cboLines.Text))
                machinesNr = 210;
            if (!string.IsNullOrEmpty(cboLines.Text))
                machinesNr = 14;
            if (!string.IsNullOrEmpty(cboBlocks.Text))
                machinesNr = 70;
               
            var cmd = new SqlCommand("getmergedproductioneffects", MainWnd._sql_con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add("@from_date", SqlDbType.DateTime).Value = GetDateFrom;
            cmd.Parameters.Add("@to_date", SqlDbType.DateTime).Value = GetDateTo;
            cmd.Parameters.Add("@shift", SqlDbType.VarChar).Value = shift;
            cmd.Parameters.Add("@machine", SqlDbType.VarChar).Value = _machines_array.ToString();
            cmd.Parameters.Add("@machinesNr", SqlDbType.Int).Value = machinesNr;

            MainWnd._sql_con.Open();
            dr = cmd.ExecuteReader();
            dt.Load(dr);
            MainWnd._sql_con.Close();
            dr.Close();
            cmd = null;

            _daysRange = GetDateTo.Subtract(GetDateFrom).TotalDays;
            if (_daysRange == 0)
                _daysRange = 1;
            else
                _daysRange += 1.0;
        }

        private void LoadReport()
            {
            //try
                //{
                LoadingInfo.InfoText = "Loading report...";
                LoadingInfo.ShowLoading();
                
                CreateDataTable();

                dgvReport.DataSource = null;

                _dt1 = new DataTable();
                LoadProcedure("NIGHT", _dt1);
                _dt2 = new DataTable();
                LoadProcedure("MORNING", _dt2);
                _dt3 = new DataTable();
                LoadProcedure("AFTERNOON", _dt3);

                _list_of_dates = new List<DateTime>();
             
                for (var dt = GetDateFrom; dt <= GetDateTo; dt = dt.AddDays(+1))
                    {
                    _list_of_dates.Add(dt);
                    }

                _table_report.Rows.Add("TOTALI PERIODO");

                foreach (var date in _list_of_dates)
                    {
                    var newRow = _table_report.NewRow();
                    newRow[0] = date.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

                    foreach (DataRow row in _dt1.Rows)
                        {
                        var rowDate = Convert.ToDateTime(row[0]);

                        if (rowDate.ToString("yyyy-MM-dd") == date.ToString("yyyy-MM-dd"))
                            {
                            newRow[1] = row[2].ToString();
                            newRow[2] = row[3].ToString();
                            newRow[3] = row[4].ToString();
                            }
                        }

                    foreach (DataRow row in _dt2.Rows)
                        {
                        var rowDate = Convert.ToDateTime(row[0]);

                        if (rowDate.ToString("yyyy-MM-dd") == date.ToString("yyyy-MM-dd"))
                            {
                            newRow[4] = row[2].ToString();
                            newRow[5] = row[3].ToString();
                            newRow[6] = row[4].ToString();
                            }
                        }

                    foreach (DataRow row in _dt3.Rows)
                        {
                        var rowDate = Convert.ToDateTime(row[0]);

                        if (rowDate.ToString("yyyy-MM-dd") == date.ToString("yyyy-MM-dd"))
                            {
                            newRow[7] = row[2].ToString();
                            newRow[8] = row[3].ToString();
                            newRow[9] = row[4].ToString();
                            }
                        }

                    _table_report.Rows.Add(newRow);
                    }

                dgvReport.DataSource = _table_report;
                
                foreach (DataGridViewColumn column in dgvReport.Columns)
                {
                    if (column.Index == 2 || column.Index == 5 || column.Index == 8 || column.Index == 11) // 1, 4, 7, 10
                        column.HeaderText = "macch. che lav.";
                    else if (column.Index == 1 || column.Index == 4 || column.Index == 7 || column.Index == 10) //2, 5, 8, 11
                        column.HeaderText = "Comp. Macch. Ferme";
                    else if (column.Index == 3 || column.Index == 6 || column.Index == 9 || column.Index == 12)
                        column.HeaderText = "nr macch.ferme";
                }
                        
                CalculateTotalsVertical();
                CalculateTotalsHorizontal();

                if (cboLines.Text != string.Empty)
                    {
                    lblSelFilter.Text = cboLines.Text;
                    }
                else if (cboBlocks.Text != string.Empty)
                    {
                    lblSelFilter.Text = cboBlocks.Text;
                    }
                else
                    {
                    lblSelFilter.Text = "All";
                    }
            //    }
            //catch
            //    {
            //    LoadingInfo.CloseLoading();
            //    MessageBox.Show("Server error or timeout expiration has occured.");
            //    }
            //finally
            //    {
            //    MainWnd._sql_con.Close();
            //    } 
            }
        
        private void dgvReport_Paint(object sender, PaintEventArgs e)
            {
            var t = 0;
            var specBrush = default(Brush);
            var defW = 90;
            var txt = "";

            for (int j = 1; j < dgvReport.ColumnCount - 1;)
                {
                t++;

                Rectangle r1 = dgvReport.GetCellDisplayRectangle(j, -1, true);
                int w2 = dgvReport.GetCellDisplayRectangle(j, -1, true).Width;

                if (t <= 3)
                    {
                    specBrush = Brushes.PapayaWhip;
                    txt = "Turno " + t;
                    }
                else if (t == 4)
                    {
                    specBrush = Brushes.PaleGreen;
                    txt = "MEDIA TURNO TOTALE";
                    }

                r1.X = r1.X - 1;
                r1.Y += 1;

                r1.Width = (r1.Width + w2 + defW);
                r1.Height = r1.Height / 2;

                e.Graphics.FillRectangle(specBrush, r1);
                e.Graphics.DrawRectangle(Pens.White, r1);

                StringFormat format = new StringFormat
                    {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                    };

                e.Graphics.DrawString(txt,
                    _up_title_font,
                    new SolidBrush(dgvReport.ColumnHeadersDefaultCellStyle.ForeColor),
                    r1,
                    format);

                j += 3;
                }
            }

        private void dgvReport_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex > 0 && e.ColumnIndex < dgvReport.ColumnCount - 1)
            {
                Rectangle r2 = e.CellBounds;
                r2.Y += e.CellBounds.Height / 2;
                r2.Height = e.CellBounds.Height / 2;
                e.PaintBackground(r2, true);
                e.PaintContent(r2);
                e.Handled = true;
            }
        }

        private void dgvReport_Scroll(object sender, ScrollEventArgs e)
            {
            dgvReport.Invalidate();
            }

        private void cboLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboBlocks.Text = string.Empty;
            var idx = cboLines.SelectedIndex + 1;

            if (cboLines.SelectedIndex == 0)
            {
                _from_machine = 1;
                _to_machine = 210;
                cboLines.Text = string.Empty;
            }
            else
            {
                _to_machine = idx * 14;
                _from_machine = _to_machine - 13;
            }
            GetMachinesArray();
            LoadReport();
        }

        private void cboBlocks_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboLines.Text = string.Empty;
            var i = cboBlocks.SelectedIndex;

            if (i == 0)
            {
                _from_machine = 1;
                _to_machine = 210;
                cboBlocks.Text = string.Empty;
            }
            else if (i == 1)
            {
                _from_machine = 1;
                _to_machine = 70;
            }
            else if (i == 2)
            {
                _from_machine = 71;
                _to_machine = 140;
            }
            else if (i == 3)
            {
                _from_machine = 141;
                _to_machine = 210;
            }
            GetMachinesArray();
            LoadReport();
        }

        private void GetMachinesArray()
            {
            _machines_array = new StringBuilder();
            _machines_array.Append(",");

            for (var m = _from_machine; m <= _to_machine; m++)
                {
                _machines_array.Append(m.ToString() + ",");
                }
            }

        private void CalculateTotalsVertical()
            {
            //horizontal and vertical calculator
            var eff1 = 0.0;
            var eff2 = 0.0;
            var eff3 = 0.0;
            var eff4 = 0.0;
            var eff5 = 0.0;
            var eff6 = 0.0;
            var qty1 = 0;
            var qty2 = 0;
            var qty3 = 0;

            var c1 = 0;
            var c2 = 0;
            var c3 = 0;
            var c4 = 0;
            var c5 = 0;
            var c6 = 0;

            var main = new MainWnd();
            for (var r = 1; r <= dgvReport.Rows.Count - 1; r++)
                {

                double.TryParse(dgvReport.Rows[r].Cells[1].Value.ToString(), out double e1);
                if (!string.IsNullOrEmpty(dgvReport.Rows[r].Cells[1].Value.ToString())) c1++;
                eff1 += e1;
                double.TryParse(dgvReport.Rows[r].Cells[2].Value.ToString(), out double e2);
                if (!string.IsNullOrEmpty(dgvReport.Rows[r].Cells[2].Value.ToString())) c2++;
                eff2 += e2;

                int.TryParse(dgvReport.Rows[r].Cells[3].Value.ToString(), out int q1);
                qty1 += q1;

                double.TryParse(dgvReport.Rows[r].Cells[4].Value.ToString(), out double e3);
                if (!string.IsNullOrEmpty(dgvReport.Rows[r].Cells[4].Value.ToString())) c3++;
                eff3 += e3;
                double.TryParse(dgvReport.Rows[r].Cells[5].Value.ToString(), out double e4);
                if (!string.IsNullOrEmpty(dgvReport.Rows[r].Cells[5].Value.ToString())) c4++;
                eff4 += e4;

                int.TryParse(dgvReport.Rows[r].Cells[6].Value.ToString(), out int q2);
                qty2 += q2;

                double.TryParse(dgvReport.Rows[r].Cells[7].Value.ToString(), out double e5);
                if (!string.IsNullOrEmpty(dgvReport.Rows[r].Cells[7].Value.ToString())) c5++;
                eff5 += e5;
                double.TryParse(dgvReport.Rows[r].Cells[8].Value.ToString(), out double e6);
                if (!string.IsNullOrEmpty(dgvReport.Rows[r].Cells[8].Value.ToString())) c6++;
                eff6 += e6;

                int.TryParse(dgvReport.Rows[r].Cells[9].Value.ToString(), out int q3);
                qty3 += q3;
                }

            eff1 = Math.Round(eff1 / (c1 == 0 ? 1 : c1), 1);
            eff2 = Math.Round(eff2 / (c2 == 0 ? 1 : c2), 1);
            eff3 = Math.Round(eff3 / (c3 == 0 ? 1 : c3), 1);
            eff4 = Math.Round(eff4 / (c4 == 0 ? 1 : c4), 1);
            eff5 = Math.Round(eff5 / (c5 == 0 ? 1 : c5), 1);
            eff6 = Math.Round(eff6 / (c6 == 0 ? 1 : c6), 1);

            dgvReport.Rows[0].Cells[1].Value = eff1 == 0.0 ? string.Empty : eff1.ToString() + "%";
            dgvReport.Rows[0].Cells[2].Value = eff2 == 0.0 ? string.Empty : eff2.ToString() + "%";
            dgvReport.Rows[0].Cells[4].Value = eff3 == 0.0 ? string.Empty : eff3.ToString() + "%";
            dgvReport.Rows[0].Cells[5].Value = eff4 == 0.0 ? string.Empty : eff4.ToString() + "%";
            dgvReport.Rows[0].Cells[7].Value = eff5 == 0.0 ? string.Empty : eff5.ToString() + "%";
            dgvReport.Rows[0].Cells[8].Value = eff6 == 0.0 ? string.Empty : eff6.ToString() + "%";

            dgvReport.Rows[0].Cells[3].Value = Math.Round(Convert.ToDouble(qty1 / _daysRange), 0).ToString();
            dgvReport.Rows[0].Cells[6].Value = Math.Round(Convert.ToDouble(qty2 / _daysRange), 0).ToString();
            dgvReport.Rows[0].Cells[9].Value = Math.Round(Convert.ToDouble(qty3 / _daysRange), 0).ToString();
            }

        private void CalculateTotalsHorizontal()
            {
            var main = new MainWnd();

            for (var r = 0; r <= dgvReport.Rows.Count - 1; r++)
                {
                main._hours = 0;
                main._minutes = 0;
                var eff = 0.0;
                var eff2 = 0.0;
                var qty = 0;
                var c1 = 0;
                var c2 = 0;
                var c3 = 0;

                for (var c = 0; c <= dgvReport.Columns.Count - 1; c++)
                    {
                    if (c == 1 || c == 4 || c == 7)
                        {
                        if (r > 0)
                            {
                            double.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString(), out double e);
                            if (!string.IsNullOrEmpty(dgvReport.Rows[r].Cells[c].Value.ToString())) c1++;
                            eff += e;
                            }
                        else
                            {
                            double.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString().Split('%')[0], out double e);
                            if (!string.IsNullOrEmpty(dgvReport.Rows[r].Cells[c].Value.ToString())) c1++;
                            eff += e;
                            }
                        }
                    if (c == 2 || c == 5 || c == 8)
                        {
                        if (r > 0)
                            {
                            double.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString(), out double e);
                            if (!string.IsNullOrEmpty(dgvReport.Rows[r].Cells[c].Value.ToString())) c2++;
                            eff2 += e;
                            }
                        else
                            {
                            double.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString().Split('%')[0], out double e);
                            if (!string.IsNullOrEmpty(dgvReport.Rows[r].Cells[c].Value.ToString())) c2++;
                            eff2 += e;
                            }
                        }
                    if (c == 3 || c == 6 || c == 9)
                        {
                        int.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString(), out int q);
                        if (!string.IsNullOrEmpty(dgvReport.Rows[r].Cells[c].Value.ToString())) c3++;
                        qty += q;
                        }
                    }

                var effic = Math.Round(eff / (c1 == 0 ? 1 : c1), 1);
                var effic2 = Math.Round(eff2 / (c2 == 0 ? 1 : c2), 1);
                dgvReport.Rows[r].Cells[10].Value = effic == 0.0 ? string.Empty : effic.ToString() + "%";
                dgvReport.Rows[r].Cells[11].Value = effic2 == 0.0 ? string.Empty : effic2.ToString() + "%";
                double.TryParse(qty.ToString(), out var qt);
                if (c3 == 0) c3 = 1;
                var mediaQty = Math.Round(qt / c3, 1);
                dgvReport.Rows[r].Cells[12].Value = mediaQty.ToString();
                }
            }

        private void button2_Click(object sender, EventArgs e)
            {
            cboBlocks.Text = string.Empty;
            cboLines.Text = string.Empty;

            _from_machine = 1;
            _to_machine = 210;
            GetMachinesArray();

            LoadReport();         
            }

        private void btnMachine_Click(object sender, EventArgs e)
            {

            }

        public void ExportToExcel()
            {
            var export = new ExcelExport();
            export.ExportToExcel(dgvReport, Text);
            }
        }
    }
