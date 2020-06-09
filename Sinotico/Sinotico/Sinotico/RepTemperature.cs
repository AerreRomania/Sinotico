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
using System.Globalization;
using System.Drawing.Drawing2D;

namespace Sinotico
{
    public partial class RepTemperature : Form
    {
        public RepTemperature()
        {
            InitializeComponent();
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
            dgvReport.ColumnHeadersHeight = 40;
            dgvReport.RowTemplate.Height = 30;
            dgvReport.DataBindingComplete += delegate
            {
                foreach (DataGridViewColumn c in dgvReport.Columns)
                {
                    c.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                dgvReport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;           
                dgvReport.GridColor = Color.White;                
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
            dgvReport.DoubleBufferedDataGridView(true);
            rbTemp.Checked = true;
            rbHum.Checked = false;
            btnStart.Click += btnStart_Click;
            btnStart.Paint += btnStart_Paint;
            base.OnLoad(e);
        }

        private void GetData(string mode)
        {
            if (dgvReport.DataSource != null)
                dgvReport.DataSource = null;

            var dataTbl = new DataTable();
            var dateFrom = new DateTime(dtpFrom.Value.Year, dtpFrom.Value.Month, dtpFrom.Value.Day);
            var dateTo = new DateTime(dtpTo.Value.Year, dtpTo.Value.Month, dtpTo.Value.Day);
            dataTbl.Columns.Add("Squadra");
            dataTbl.Columns.Add("Media");
            if (dateFrom.Equals(dateTo))
            {
                for(var i = 0; i<= 23; i++)                
                    dataTbl.Columns.Add(string.Concat(i.ToString(), ":00"));                
            }
            else
            {
                for (var day = dateFrom.Date; day <= dateTo.Date; day = day.AddDays(+1))                
                    dataTbl.Columns.Add(day.ToString("dd/MM", CultureInfo.InvariantCulture));                
            }
            var rowSq1 = dataTbl.NewRow();
            rowSq1[0] = "Squadra1";
            rowSq1[1] = "";
            var rowSq2 = dataTbl.NewRow();
            rowSq2[0] = "Squadra2";
            rowSq2[1] = "";
            var rowSq3 = dataTbl.NewRow();
            rowSq3[0] = "Squadra3";
            rowSq3[1] = "";
            var tmpTbl = new DataTable();
            using (SqlConnection con = new SqlConnection(MainWnd.conString))
            {
                SqlCommand cmd = new SqlCommand("get_hum_temp_reportdata", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = dateFrom;
                cmd.Parameters.Add("@toDate", SqlDbType.DateTime).Value = dateTo;
                con.Open();
                var dr = cmd.ExecuteReader();
                tmpTbl.Load(dr);
                dr.Close();
                con.Close();
            }
            if(tmpTbl.Rows.Count <= 0)
            {
                MessageBox.Show("No Data!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            double sq1 = 0;
            double sq2 = 0;
            double sq3 = 0;
            int sq1Divider = 0;
            int sq2Divider = 0;
            int sq3Divider = 0;
            //1 day only
            if (dateFrom.Equals(dateTo))
            {
                int.TryParse(tmpTbl.Rows[0][1].ToString(), out var hour);
                var column = string.Concat(hour.ToString(), ":00");
                foreach (DataRow row in tmpTbl.Rows)
                {
                    column = string.Concat(hour.ToString(), ":00");
                    int.TryParse(row[1].ToString(), out var currentHour);
                    if (hour != currentHour)
                    {
                        rowSq1[column] = Math.Round(sq1 / sq1Divider, 1);
                        rowSq2[column] = Math.Round(sq2 / sq2Divider, 1);
                        rowSq3[column] = Math.Round(sq3 / sq3Divider, 1);
                        sq1Divider = 0;
                        sq2Divider = 0;
                        sq3Divider = 0;
                        sq1 = 0.0;
                        sq2 = 0.0;
                        sq3 = 0.0;
                        hour = currentHour;
                    }
                    int.TryParse(row[0].ToString().Split(' ')[1], out var line);
                    double.TryParse(row[2].ToString(), out var temp);
                    int.TryParse(row[3].ToString(), out var hum);
                    int.TryParse(row[4].ToString(), out var recNum);
                    switch (mode)
                    {
                        case "temperature":
                            {
                                if (line >= 1 && line <= 5)
                                {
                                    sq1 += (temp / recNum);
                                    sq1Divider++;
                                }
                                else if (line > 5 && line <= 10)
                                {
                                    sq2 += (temp / recNum);
                                    sq2Divider++;
                                }
                                else if (line > 10)
                                {
                                    sq3 += (temp / recNum);
                                    sq3Divider++;
                                }
                                break;
                            }
                        case "humidity":
                            {
                                if (line >= 1 && line <= 5)
                                {
                                    sq1 += (hum / recNum);
                                    sq1Divider++;
                                }
                                else if (line > 5 && line <= 10)
                                {
                                    sq2 += (hum / recNum);
                                    sq2Divider++;
                                }
                                else if (line > 10)
                                {
                                    sq3 += (hum / recNum);
                                    sq3Divider++;
                                }
                                break;
                            }
                    }
                }
                rowSq1[column] = Math.Round(sq1 / sq1Divider, 1);
                rowSq2[column] = Math.Round(sq2 / sq2Divider, 1);
                rowSq3[column] = Math.Round(sq3 / sq3Divider, 1);
            }
            //range of days
            else
            {
                int.TryParse(tmpTbl.Rows[0][1].ToString(), out var day);
                int.TryParse(tmpTbl.Rows[0][6].ToString(), out var m);
                var column = string.Empty;
                if (day < 10) column = "0" + day.ToString() + "/";
                else column = day.ToString() + "/";
                if (m < 10) column += "0" + m.ToString();
                else column += m.ToString();
                foreach (DataRow row in tmpTbl.Rows)
                {
                    int.TryParse(row[1].ToString(), out var currentDay);
                    int.TryParse(row[6].ToString(), out var mo);

                    if (day < 10) column = "0" + day.ToString() + "/";
                    else column = day.ToString() + "/";
                    if (m < 10) column += "0" + m.ToString();
                    else column += m.ToString();
                    if (day != currentDay || mo != m)
                    {
                        rowSq1[column] = Math.Round(sq1 / sq1Divider, 1);
                        rowSq2[column] = Math.Round(sq2 / sq2Divider, 1);
                        rowSq3[column] = Math.Round(sq3 / sq3Divider, 1);
                        sq1 = 0.0;
                        sq2 = 0.0;
                        sq3 = 0.0;
                        sq1Divider = 0;
                        sq2Divider = 0;
                        sq3Divider = 0;
                        day = currentDay;
                        m = mo;
                    }
                    int.TryParse(row[0].ToString().Split(' ')[1], out var line);
                    double.TryParse(row[2].ToString(), out var temp);
                    int.TryParse(row[3].ToString(), out var hum);
                    int.TryParse(row[4].ToString(), out var recNum);
                    switch (mode)
                    {
                        case "temperature":
                            {
                                if (line >= 1 && line <= 5)
                                {
                                    sq1 += (temp / recNum);
                                    sq1Divider++;
                                }
                                else if (line > 5 && line <= 10)
                                {
                                    sq2 += (temp / recNum);
                                    sq2Divider++;
                                }
                                else if (line > 10)
                                {
                                    sq3 += (temp / recNum);
                                    sq3Divider++;
                                }                                    
                                break;
                            }
                        case "humidity":
                            {
                                if (line >= 1 && line <= 5)
                                {
                                    sq1 += (hum / recNum);
                                    sq1Divider++;
                                }
                                else if (line > 5 && line <= 10)
                                {
                                    sq2 += (hum / recNum);
                                    sq2Divider++;
                                }
                                else if (line > 10)
                                {
                                    sq3 += (hum / recNum);
                                    sq3Divider++;
                                }
                                break;
                            }
                    }
                }
                rowSq1[column] = Math.Round(sq1 / sq1Divider, 1);
                rowSq2[column] = Math.Round(sq2 / sq2Divider, 1);
                rowSq3[column] = Math.Round(sq3 / sq3Divider, 1);
            }
            dataTbl.Rows.Add(rowSq1);
            dataTbl.Rows.Add(rowSq2);
            dataTbl.Rows.Add(rowSq3);
            dgvReport.DataSource = dataTbl;
            foreach(DataGridViewRow row in dgvReport.Rows)
            {
                double total = 0.0;
                int nr = 0;
                for(var i = 2; i < dgvReport.Columns.Count; i ++)
                {
                    if(!string.IsNullOrEmpty(row.Cells[i].Value.ToString()))
                    {
                        nr++;
                        double.TryParse(row.Cells[i].Value.ToString(), out var val);
                        total += val;
                    }
                }
                row.Cells[1].Value = Math.Round(total / nr, 1);
            }
            foreach(DataGridViewColumn column in dgvReport.Columns)
            {
                if (column.Index > 1)
                    column.Width = 50;
                else column.Width = 65;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            var dateFrom = new DateTime(dtpFrom.Value.Year, dtpFrom.Value.Month, dtpFrom.Value.Day);
            var dateTo = new DateTime(dtpTo.Value.Year, dtpTo.Value.Month, dtpTo.Value.Day);
            if (dateFrom.Date > dateTo.Date) return;
            if (rbTemp.Checked)
                GetData("temperature");
            else if (rbHum.Checked)
                GetData("humidity");
        }
        private void btnStart_Paint(object sender, PaintEventArgs e)
        {
            var btn = sender as Button;
            var pen = new Pen(new SolidBrush(btn.BackColor), 0);
            using (GraphicsPath path = MainWnd._geometry.RoundedRectanglePath(new Rectangle(0, 0, btn.Width, btn.Height), 5))
            {
                SmoothingMode old = e.Graphics.SmoothingMode;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(pen, path);
                btn.Region = new Region(path);
                e.Graphics.SmoothingMode = old;
            }
            pen.Dispose();
        }
    }
}
