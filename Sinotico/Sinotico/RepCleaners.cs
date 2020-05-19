using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Sinotico
{
    public partial class RepCleaners : Form
    {
        public static string _reportType = string.Empty;
        public void SetReportType(string type)
        {
            string s = type == "cquality" ? "TEMPI PER CQ" : type;
            _reportType = type;
            label1.Text = s.ToUpper();           
            this.Text = s;
        }
        
        public RepCleaners()
        {
            InitializeComponent();
        }

        private bool _yearMode = false;
        private string _year;
        private DataSet _dataSet = new DataSet();
        private SqlDataAdapter _da = new SqlDataAdapter();
        private int _yearFrom, _monthFrom, _yearTo, _monthTo;
        private DataTable _tableReport = new DataTable();

        public void ExportToExcel()
        {
            var export = new ExcelExport();
            export.ExportToExcel(dgvReport, Text);
        }

        private void AddYearCombo()
        {
            var curYear = DateTime.Now.Year;
            var curYearBefore = curYear - 3;

            for (var i = curYearBefore; i <= curYear; i++)
            {
                cmbYear.Items.Add(i);
            }

            var tmpStr = curYear.ToString();
            cmbYear.SelectedIndex = cmbYear.FindString(tmpStr);

            _year = tmpStr;
            lblFrom.Text = _year;
            lblTo.Text = "31.12" + "." + _year;
        }
        
        private int _filter;

        private void btnMachine_Click(object sender, EventArgs e)
        {
            button1.BackColor = Color.White;
            btnSquadra.BackColor = Color.White;
            btnMachine.BackColor = Color.LightGray;           
            
            _filter = 0;
            GetData();
        }

        private void btnSquadra_Click(object sender, EventArgs e)
        {
            btnMachine.BackColor = Color.White;
            button1.BackColor = Color.White;
            btnSquadra.BackColor = Color.LightGray;           

            _filter = 2;
            GetData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            btnMachine.BackColor = Color.White;
            btnSquadra.BackColor = Color.White;
            button1.BackColor = Color.LightGray;           

            _filter = 1;
            GetData();
        }

        protected override void OnLoad(EventArgs e)
        {
            dgvReport.AllowUserToAddRows = false;
            dgvReport.AllowUserToDeleteRows = false;
            dgvReport.AllowUserToResizeColumns = false;
            dgvReport.AllowUserToResizeRows = false;
            dgvReport.AllowUserToOrderColumns = false;
            dgvReport.ReadOnly = true;
            //dgvReport.DefaultCellStyle.BackColor = Color.FromArgb(232, 232, 232);
            dgvReport.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvReport.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvReport.ColumnHeadersHeight = 70;
            
            _mode = 1;
            _filter = 0; //as default
            dgvReport.DoubleBufferedDataGridView(true);

            GetData();

            dgvReport.DataBindingComplete += delegate
            {
                dgvReport.Rows[1].Height = 7;
                if(!_yearMode)
                if (_tempo_clicked)
                    for(var _row = 1; _row <= dgvReport.Rows.Count - 1; _row ++)
                        for(var _column = 3; _column <= dgvReport.Columns.Count - 1; _column ++)
                        {                         
                            TimeSpan.TryParse(dgvReport.Rows[_row].Cells[_column].Value.ToString(), out var timeS);

                            if (timeS == new TimeSpan(0, 0, 0, 0) &&
                                string.IsNullOrEmpty(dgvReport.Rows[_row].Cells[_column].Value.ToString()))
                                continue;

                            TimeSpan _actTs = new TimeSpan(timeS.Days, timeS.Hours, timeS.Minutes, timeS.Seconds);
                            TimeSpan _btmBorder = new TimeSpan(0, 20, 0, 0);
                            TimeSpan _topBorder = new TimeSpan(0, 40, 0, 0);

                            if (TimeSpan.Compare(_actTs, _btmBorder) == -1 || TimeSpan.Compare(_actTs, _topBorder) == 1)
                                dgvReport.Rows[_row].Cells[_column].Style.BackColor = Color.FromArgb(255, 155, 155);
                        }

                for (var _cell = 0; _cell < dgvReport.Columns.Count; _cell++)
                {
                    if (_cell == 2) continue;
                    dgvReport.Rows[0].Cells[_cell].Style.BackColor = Color.FromArgb(255, 223, 45);
                }
            };

            AddYearCombo();
            
            base.OnLoad(e);
        }

        private ComboBox filter = new ComboBox();
        private bool _filterCreated = false;
        private void CreateHeaderFilter(DataGridView dgv, int columnIndex)
        {
            if (_filterCreated) return;
            filter = new ComboBox();
            filter.Name = dgv.Columns[columnIndex].Name;

            string[] data = new string[] { "Squadra 1", "Squadra 2", "Squadra 3" };

            filter.Items.Add(string.Empty);
            foreach(var item in data)
            filter.Items.Add(item);

            Rectangle rect = dgv.GetCellDisplayRectangle(columnIndex, -1, true);
            filter.Size = new Size(rect.Width - 1, rect.Height / 2 - 1);
            filter.Location = new Point(rect.X + (rect.Width - filter.Width) - 1,
                                         rect.Y + (rect.Height - filter.Height) - 1);

            dgv.Controls.Add(filter);
            _filterCreated = true;

            filter.SelectedIndexChanged += delegate
            {
                GetData();                
            };
            }

        private List<Color> _date_back_colors = new List<Color>();
        private void CreateTableView()
        {
            if (dgvReport.DataSource != null) dgvReport.DataSource = null;

            if (!_yearMode)
            {
                _date_back_colors = new List<Color>();

                _yearFrom = dtpStartDate.Value.Year;
                _monthFrom = dtpStartDate.Value.Month;
                _yearTo = dtpEndDate.Value.Year;
                _monthTo = dtpEndDate.Value.Month;
                var startDate = new DateTime(_yearFrom, _monthFrom, dtpStartDate.Value.Day);
                var endDate = new DateTime(_yearTo, _monthTo, dtpEndDate.Value.Day);

                lblFrom.Text = dtpStartDate.Value.ToString("dd-MMM-yy", System.Globalization.CultureInfo.InvariantCulture);
                lblTo.Text = dtpEndDate.Value.ToString("dd-MMM-yy", System.Globalization.CultureInfo.InvariantCulture);

                var dayRange = endDate.Subtract(startDate).Days;

                _tableReport = new DataTable();

                if (dgvReport.DataSource != null) dgvReport.DataSource = null;  //clear dgv

                _tableReport.Columns.Add("macchina");
                _tableReport.Columns.Add("TOTAL");
                _tableReport.Columns.Add("sep1");

                for (var date = new DateTime(_yearFrom, _monthFrom, dtpStartDate.Value.Day); date <= new DateTime(_yearTo, _monthTo, dtpEndDate.Value.Day); date = date.AddDays(+1))
                {
                    if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                        _date_back_colors.Add(Color.FromArgb(214, 214, 214));
                    else _date_back_colors.Add(Color.FromArgb(232, 232, 232));

                    var col = new DataColumn()
                    {
                        ColumnName = date.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture),
                    };

                    _tableReport.Columns.Add(col);
                }

                var totRow = _tableReport.NewRow();
                foreach (DataColumn column in _tableReport.Columns)
                {
                    if (column.ColumnName == "macchina")
                        totRow[column] = "TOTAL";
                    else
                        totRow[column] = "";
                }

                _tableReport.Rows.Add(totRow);

                //sep row
                totRow = _tableReport.NewRow();
                foreach (DataColumn column in _tableReport.Columns)
                {
                    totRow[column] = string.Empty;
                }
                _tableReport.Rows.Add(totRow);

                if (_filter == 0)
                {

                    for (var i = 1; i <= 210; i++)
                    {
                        var newRow = _tableReport.NewRow();
                        newRow[0] = i.ToString();
                        _tableReport.Rows.Add(newRow);
                    }
                }
                else if (_filter == 1)
                {
                    var lst = ActiveOperators();

                    foreach (var str in lst)
                    {
                        var newRow = _tableReport.NewRow();

                        newRow[0] = str;

                        _tableReport.Rows.Add(newRow);
                    }
                }
                else if (_filter == 2)
                {
                    for (var i = 1; i <= 3; i++)
                    {
                        var newr = _tableReport.NewRow();

                        newr[0] = "Squadra " + i.ToString();

                        _tableReport.Rows.Add(newr);
                    }
                }
            }
            else
            {
                _tableReport = new DataTable();

                if (dgvReport.DataSource != null) dgvReport.DataSource = null;

                _tableReport.Columns.Add("macchina");
                _tableReport.Columns.Add("TOTAL");
                _tableReport.Columns.Add("sep1");

                for (var month = 1; month <= 12; month++)
                {
                    var col = new DataColumn()
                    {
                        ColumnName = month.ToString(),
                    };

                    _tableReport.Columns.Add(col);
                }

                var totRow = _tableReport.NewRow();
                foreach (DataColumn column in _tableReport.Columns)
                {
                    if (column.ColumnName == "macchina")
                        totRow[column] = "TOTAL";
                    else
                        totRow[column] = "";
                }

                _tableReport.Rows.Add(totRow);

                //sep row
                totRow = _tableReport.NewRow();
                foreach (DataColumn column in _tableReport.Columns)
                {
                    totRow[column] = string.Empty;
                }
                _tableReport.Rows.Add(totRow);

                if (_filter == 0)
                {
                    var start = 1;
                    var end = 210;
                    if (filter != null)
                    {
                        if (filter.SelectedIndex == 1)
                        {
                            start = 1;
                            end = 70;
                        }
                        else if (filter.SelectedIndex == 2)
                        {
                            start = 71;
                            end = 140;
                        }
                        else if (filter.SelectedIndex == 3)
                        {
                            start = 141;
                            end = 210;
                        }
                    }
                  
                    for (var i = start; i <= end; i++)
                    {
                        var newRow = _tableReport.NewRow();
                        newRow[0] = i.ToString();
                        _tableReport.Rows.Add(newRow);
                    }
                }
                else if (_filter == 1)
                {
                    var lst = ActiveOperators();

                    foreach (var str in lst)
                    {
                        var newRow = _tableReport.NewRow();

                        newRow[0] = str;

                        _tableReport.Rows.Add(newRow);
                    }
                }
                else if (_filter == 2)
                {
                    for (var i = 1; i <= 3; i++)
                    {
                        var newr = _tableReport.NewRow();

                        newr[0] = "Squadra " + i.ToString();

                        _tableReport.Rows.Add(newr);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var s = dtpStartDate.Value;
            var en = dtpEndDate.Value;

            var ts = en.Subtract(s).Days;

            if (ts <= 0)
            {
                MessageBox.Show("Empty time interval", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _yearMode = false;

            GetData();
        }

        private int _mode = 0;
        private bool _tempo_clicked = false;
        private void button4_Click(object sender, EventArgs e)
        {
            _tempo_clicked = true;
            button3.BackColor = Color.White;
            button3.ForeColor = Color.DarkRed;
            button4.BackColor = Color.IndianRed;
            button4.ForeColor = Color.White;

            _mode = 1;
            // _filter = 0;
            GetData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _tempo_clicked = false;
            button4.BackColor = Color.White;
            button4.ForeColor = Color.DarkRed;
            button3.BackColor = Color.IndianRed;
            button3.ForeColor = Color.White;

            _mode = 2;
            // _filter = 0;
            GetData();
        }

        private void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            _yearMode = true;
            _year = cmbYear.Text;

            //lblFrom.Text = _year;
            lblTo.Text = "31.12" + "." + _year;
            GetData();
        }

        private void dgvReport_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= -1)
            {
                if (e.ColumnIndex == 2)
                {
                    var rect = new System.Drawing.Rectangle(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width, e.CellBounds.Height);
                    e.Graphics.FillRectangle(new SolidBrush(dgvReport.BackgroundColor), rect);
                    e.Handled = true;
                }
            }
            if(e.RowIndex == 1)
            {
                    var rect = new System.Drawing.Rectangle(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width, e.CellBounds.Height);
                    e.Graphics.FillRectangle(new SolidBrush(dgvReport.BackgroundColor), rect);
                    e.Handled = true;
            }
        }

        private void GetData()
        {
            var s = dtpStartDate.Value;
            var en = dtpEndDate.Value;

            var ts = en.Subtract(s).Days;

            if (ts <= 0 && !_yearMode) return;

            _dataSet = new DataSet();

            if (dgvReport.DataSource != null) dgvReport.DataSource = null;

            if (!_yearMode)
            {
                if (_reportType == "cquality")
                {
                    using (var con = new SqlConnection(MainWnd.conString))
                    {
                        var cmd = new SqlCommand();
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        var cmdTxt = "";
                        if (_mode == 1) cmdTxt = "getcqualitypermonth";
                        else if (_mode == 2) cmdTxt = "getcqualitypermonthticks";
                        cmd.CommandText = cmdTxt;
                        cmd.Parameters.Add("@start", SqlDbType.DateTime).Value = dtpStartDate.Value;
                        cmd.Parameters.Add("@end", SqlDbType.DateTime).Value = dtpEndDate.Value;
                        cmd.Parameters.Add("@squadra", SqlDbType.NVarChar).Value = filter.Text;
                        _da = new SqlDataAdapter(cmd);
                        _da.Fill(_dataSet);
                        _da.Dispose();
                    }
                }
                else
                {
                    using (var con = new SqlConnection(MainWnd.conString))
                    {
                        var cmd = new SqlCommand();
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        var cmdTxt = "";
                        if (_mode == 1) cmdTxt = "getcleanerspermonth";
                        else if (_mode == 2) cmdTxt = "getcleanerspermonthticks";
                        cmd.CommandText = cmdTxt;
                        cmd.Parameters.Add("@start", SqlDbType.DateTime).Value = dtpStartDate.Value;
                        cmd.Parameters.Add("@end", SqlDbType.DateTime).Value = dtpEndDate.Value;
                        cmd.Parameters.Add("@puliziaType", SqlDbType.NVarChar).Value = _reportType;
                        cmd.Parameters.Add("@squadra", SqlDbType.NVarChar).Value = filter.Text;
                        _da = new SqlDataAdapter(cmd);
                        _da.Fill(_dataSet);
                        _da.Dispose();
                    }
                }
            }
            else
            {
                if (_reportType == "cquality")
                {
                    using (var con = new SqlConnection(MainWnd.conString))
                    {
                        var cmd = new SqlCommand();
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        var cmdTxt = "";
                        if (_mode == 1) cmdTxt = "getcqualityperyear";
                        else if (_mode == 2) cmdTxt = "getcqualityperyearticks";
                        cmd.CommandText = cmdTxt;
                        cmd.Parameters.Add("@year", SqlDbType.Int).Value = int.Parse(_year);
                        cmd.Parameters.Add("@squadra", SqlDbType.NVarChar).Value = filter.Text;
                        _da = new SqlDataAdapter(cmd);
                        _da.Fill(_dataSet);
                        _da.Dispose();
                    }
                }
                else
                {
                    using (var con = new SqlConnection(MainWnd.conString))
                    {
                        var cmd = new SqlCommand();
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        var cmdTxt = "";
                        if (_mode == 1) cmdTxt = "getcleanersperyear";
                        else if (_mode == 2) cmdTxt = "getcleanersperyearticks";
                        cmd.CommandText = cmdTxt;
                        cmd.Parameters.Add("@year", SqlDbType.Int).Value = int.Parse(_year);
                        cmd.Parameters.Add("@puliziaType", SqlDbType.NVarChar).Value = _reportType;
                        cmd.Parameters.Add("@squadra", SqlDbType.NVarChar).Value = filter.Text;
                        _da = new SqlDataAdapter(cmd);
                        _da.Fill(_dataSet);
                        _da.Dispose();
                    }
                }

            }

            CreateTableView();

            if (!_yearMode)
            {
                foreach (DataRow dRow in _dataSet.Tables[_filter].Rows)
                {
                    var date = Convert.ToDateTime(dRow[0]).ToString("yyyy-MM-dd");
                    var dinamo = dRow[1].ToString();
                    var minutes = dRow[2].ToString();

                    int.TryParse(minutes, out var mins);

                    foreach (DataRow row in _tableReport.Rows)
                    {
                        if (dinamo != row[0].ToString()) continue;

                        if (_mode == 1)
                        {
                            row[date] = ConvertSecondsToHHmm(mins * 60).ToString();
                        }
                        else
                        {
                            row[date] = mins.ToString();
                        }
                    }

                }
            }
            else
            {
                foreach (DataRow dRow in _dataSet.Tables[_filter].Rows)
                {
                    int.TryParse(dRow[0].ToString(), out var month);
                    var dinamo = dRow[1].ToString();
                    var minutes = dRow[2].ToString();

                    int.TryParse(minutes, out var mins);

                    foreach (DataRow row in _tableReport.Rows)
                    {
                        if (dinamo != row[0].ToString()) continue;

                        if (_mode == 1)
                        {
                            //here
                            row[month + 2] = ConvertSecondsToHHmm(mins * 60).ToString();
                        }
                        else
                        {
                            //here
                            row[month + 2] = mins.ToString();
                        }
                    }

                }
            }
            _tableReport.Columns.Add("sq");
            dgvReport.DataSource = _tableReport;
            dgvReport.Rows[0].Frozen = true;
            dgvReport.Columns["sq"].Visible = false;
            dgvReport.Columns["sep1"].Width = 7;
            dgvReport.Columns["sep1"].HeaderText = string.Empty;
            foreach (DataGridViewRow row in dgvReport.Rows)
            {
                int.TryParse(row.Cells[0].Value.ToString(), out var mac);

                if (mac >= 1 && mac <= 70) row.Cells[dgvReport.Columns.Count - 1].Value = "Squadra1";
                else if (mac >= 71 && mac <= 140) row.Cells[dgvReport.Columns.Count - 1].Value = "Squadra2";
                else if (mac >= 141 && mac <= 210) row.Cells[dgvReport.Columns.Count - 1].Value = "Squadra3";
            }
            if (!_yearMode)
            {
                dgvReport.Columns[0].Width = 180;
                dgvReport.Columns["TOTAL"].Width = 50;
                for (var i = 3; i <= dgvReport.Columns.Count - 2; i++)
                {
                    dgvReport.Columns[i].Width = 50;
                    dgvReport.Columns[i].HeaderText =
                        Convert.ToDateTime(dgvReport.Columns[i].HeaderText).ToString("dd.MM");
                }
                dgvReport.Columns[0].DefaultCellStyle.BackColor = Color.FromArgb(214, 214, 214);
                dgvReport.Columns[1].DefaultCellStyle.BackColor = Color.FromArgb(214, 214, 214);
                var position = 0;
                for (var _column = 3; _column <= dgvReport.Columns.Count - 2; _column++)
                {
                    dgvReport.Columns[_column].DefaultCellStyle.BackColor = _date_back_colors[position];
                    position++;
                }
            }
            else
            {
                dgvReport.Columns[0].DefaultCellStyle.BackColor = Color.FromArgb(214, 214, 214);
                dgvReport.Columns[1].DefaultCellStyle.BackColor = Color.FromArgb(214, 214, 214);

                for (var _column = 3; _column <= dgvReport.Columns.Count - 1; _column++)
                    dgvReport.Columns[_column].DefaultCellStyle.BackColor = Color.FromArgb(232, 232, 232);

                dgvReport.Columns[0].Width = 180;
                dgvReport.Columns["TOTAL"].Width = 90;
                var position = 2;
                for (var i = 3; i <= dgvReport.Columns.Count - 1; i++)
                {
                    dgvReport.Columns[i].Width = 90;
                    string month = position - 1 <= 9 ? "0" + (position - 1).ToString() : (position - 1).ToString();
                    dgvReport.Columns[i].HeaderText = month + "." + _year;
                    position++;
                }
            }
            if (_mode == 1)
            {
                //time
                //vertical totals
                var totalOftotals = 0.0;
                foreach (DataGridViewRow row in dgvReport.Rows)
                {
                    if (row.Index == 0 || row.Index == 1) continue;

                    var minutes = 0.0;
                    foreach (DataGridViewColumn column in dgvReport.Columns)
                    {
                        if (column.Index == 0 || column.Index == 1 || column.Index == 2 ||
                           column.Index == dgvReport.Columns.Count - 1) continue;

                        TimeSpan.TryParse(dgvReport.Rows[row.Index].Cells[column.Index].Value.ToString(), out var tsTime);
                        minutes += tsTime.TotalMinutes;
                    }
                    totalOftotals += minutes;
                    TimeSpan formatedTs = TimeSpan.FromMinutes(minutes);
                    string tt = string.Format("{0}:{1}",
                                              (int)formatedTs.TotalHours < 10 ?
                                              "0" + ((int)formatedTs.TotalHours).ToString() :
                                              ((int)formatedTs.TotalHours).ToString(),
                                              (int)formatedTs.Minutes < 10 ?
                                              "0" + ((int)formatedTs.Minutes).ToString() :
                                              ((int)formatedTs.Minutes).ToString());
                    dgvReport.Rows[row.Index].Cells[1].Value = tt;
                }
                //horizontal totals
                foreach (DataGridViewColumn column in dgvReport.Columns)
                {
                    if (column.Index == 0 || column.Index == 1 || column.Index == 2 ||
                           column.Index == dgvReport.Columns.Count - 1) continue;

                    var minutes = 0.0;
                    foreach (DataGridViewRow row in dgvReport.Rows)
                    {
                        if (row.Index == 0 || row.Index == 1) continue;

                        TimeSpan.TryParse(dgvReport.Rows[row.Index].Cells[column.Index].Value.ToString(), out var tsTime);
                        minutes += tsTime.TotalMinutes;
                    }
                    totalOftotals += minutes;
                    TimeSpan formatedTs = TimeSpan.FromMinutes(minutes);
                    string tt = string.Format("{0}:{1}",
                                              (int)formatedTs.TotalHours < 10 ?
                                              "0" + ((int)formatedTs.TotalHours).ToString() :
                                              ((int)formatedTs.TotalHours).ToString(),
                                              (int)formatedTs.Minutes < 10 ?
                                              "0" + ((int)formatedTs.Minutes).ToString() :
                                              ((int)formatedTs.Minutes).ToString());
                    dgvReport.Rows[0].Cells[column.Index].Value = tt;
                }
                TimeSpan formTotTs = TimeSpan.FromMinutes(totalOftotals);
                string totTs = string.Format("{0}:{1}",
                                                  (int)formTotTs.TotalHours < 10 ?
                                                  "0" + ((int)formTotTs.TotalHours).ToString() :
                                                  ((int)formTotTs.TotalHours).ToString(),
                                                  (int)formTotTs.Minutes < 10 ?
                                                  "0" + ((int)formTotTs.Minutes).ToString() :
                                                  ((int)formTotTs.Minutes).ToString());
                dgvReport.Rows[0].Cells[1].Value = totTs;
            }
            else
            {
                var totalOftotals = 0;
                //intervents
                //vertical totals
                foreach (DataGridViewRow row in dgvReport.Rows)
                {
                    if (row.Index == 0 || row.Index == 1) continue;

                    var totalIntervents = 0;
                    foreach (DataGridViewColumn column in dgvReport.Columns)
                    {
                        if (column.Index == 0 || column.Index == 1 || column.Index == 2 ||
                           column.Index == dgvReport.Columns.Count - 1) continue;

                        int.TryParse(dgvReport.Rows[row.Index].Cells[column.Index].Value.ToString(), out var intervents);
                        totalIntervents += intervents;
                    }
                    totalOftotals += totalIntervents;
                    dgvReport.Rows[row.Index].Cells[1].Value = totalIntervents;
                }
                //horizontal totals
                foreach (DataGridViewColumn column in dgvReport.Columns)
                {
                    if (column.Index == 0 || column.Index == 1 || column.Index == 2 ||
                           column.Index == dgvReport.Columns.Count - 1) continue;

                    var totalIntervents = 0;
                    foreach (DataGridViewRow row in dgvReport.Rows)
                    {
                        if (row.Index == 0 || row.Index == 1) continue;

                        int.TryParse(dgvReport.Rows[row.Index].Cells[column.Index].Value.ToString(), out var intervents);
                        totalIntervents += intervents;
                    }
                    totalOftotals += totalIntervents;
                    dgvReport.Rows[0].Cells[column.Index].Value = totalIntervents;
                }
                dgvReport.Rows[0].Cells[1].Value = totalOftotals;
            }
            CreateHeaderFilter(dgvReport, 0);
        }

        private string ConvertSecondsToHHmm(int seconds)
        {
            int hours = seconds / 3600;
            int mins = (seconds % 3600) / 60;
            return string.Format(@"{0:D2}:{1:D2}", hours, mins);
        }

        private List<String> ActiveOperators()
        {
            var lst = new List<string>();

            string q = string.Empty;
            if(_reportType == "cquality")
            {
                q = "select fullname from operators where cast(code as int) > 0 and operatorinfo = 'CQ'";               
            }
            else
            {
                q = "select fullname from operators where cast(code as int) > 0 and operatorinfo = 'PULIZIA ORDINARIA'";
            }

            var cmd = new SqlCommand();
            using (var c = new SqlConnection(MainWnd.conString))
            {
                cmd = new SqlCommand(q, c);
                c.Open();
                var dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lst.Add(dr[0].ToString());
                    }
                }
                c.Close();
                dr.Close();
            }

            return lst;
        }
    }
}
