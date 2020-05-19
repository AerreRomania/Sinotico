using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Data.SqlClient;
using System.Data.Linq;
using Sinotico.DatabaseTableClasses;

namespace Sinotico
{
    public partial class FrmHolidays : Form
    {
        public static DataContext dc = new DataContext(MainWnd.conString);

        private List<Lines> _definedHolidays = new List<Lines>();       
        private List<DateTime> _dbHolidays = new List<DateTime>();
        private List<DateTime> _toInsert = new List<DateTime>();        
        private List<DateTime> _toDelete = new List<DateTime>();

        public static Font _consolas = new Font("Consolas", 8, FontStyle.Regular);

        public DateTime StartDate
        {
            get
            {
                return new DateTime(dtpStartDate.Value.Year, dtpStartDate.Value.Month, dtpStartDate.Value.Day);
            }
            set
            {
                StartDate = new DateTime(dtpStartDate.Value.Year, dtpStartDate.Value.Month, dtpStartDate.Value.Day);
            }
        }
        public DateTime EndDate
        {
            get
            {
                return new DateTime(dtpEndDate.Value.Year, dtpEndDate.Value.Month, dtpEndDate.Value.Day);
            }
            set
            {
                EndDate = new DateTime(dtpEndDate.Value.Year, dtpEndDate.Value.Month, dtpEndDate.Value.Day);
            }
        }

        public FrmHolidays()
        {
            InitializeComponent();
            btnGo.Click += btnGo_Click;
            btnSave.Click += btnSave_Click;
            dgvHolidays.CellClick += DataGridViewCell_Click;
            dc = new DataContext(MainWnd.conString);
        }
        protected override void OnLoad(EventArgs e)
        {
            dc = new DataContext(MainWnd.conString);
            StyleDataGridView(dgvHolidays);
            dtpStartDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            int days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            dtpEndDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, days);
            BuildGridStructure();
            DoubleBuffered = true;
            dgvHolidays.DoubleBufferedDataGridView(true);
            base.OnLoad(e);
        }
        private void StyleDataGridView(DataGridView dgv)
        {
            dgv.DataBindingComplete += delegate
            {
                dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;
                dgv.ReadOnly = true;
                dgv.AllowUserToResizeRows = false;
                dgv.AllowUserToDeleteRows = false;
                dgv.AllowUserToAddRows = false;
                dgv.AllowUserToResizeColumns = false;
                dgv.AllowUserToOrderColumns = false;
                dgv.AllowDrop = false;
                dgv.RowHeadersVisible = false;
                dgv.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(132, 189, 165);
                dgv.RowsDefaultCellStyle.ForeColor = Color.FromArgb(60, 60, 60);
                dgv.BorderStyle = BorderStyle.None;
                dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(132, 189, 165);
                var year = DateTime.Now;
                foreach (DataGridViewColumn c in dgv.Columns)
                {
                    if (c.Index == 0) c.Width = 70;
                    else c.Width = 35;
                    c.SortMode = DataGridViewColumnSortMode.NotSortable;
                    c.DefaultCellStyle.Font = _consolas;
                    c.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    c.DefaultCellStyle.BackColor = Color.FromArgb(190, 190, 190);
                    if (c.Index >= 1)
                    {
                        //if (dgv.Columns[c.Index - 1].Name == "31/12") year = year.AddYears(+1);
                        var d = c.Name.Split('/')[0];
                        var m = c.Name.Split('/')[1];
                        int.TryParse(d, out var day);
                        int.TryParse(m, out var month);
                        var currentDate = new DateTime(year.Year, month, day);
                        if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                            c.DefaultCellStyle.BackColor = Color.FromArgb(215, 215, 215);
                    }
                }
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgv.ColumnHeadersDefaultCellStyle.Font = _consolas;
                dgv.EnableHeadersVisualStyles = false;
                dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.SeaGreen;
            };
        }
        private void DataGridViewCell_Click(object sender, DataGridViewCellEventArgs e)
        {
            var cell = dgvHolidays.SelectedCells[0];
            var value = string.Empty;
            if (cell.ColumnIndex == 0) return;
            if (string.IsNullOrEmpty(cell.Value.ToString())) value = "X";
            else value = string.Empty;
            foreach(DataGridViewRow row in dgvHolidays.Rows)
            {
                row.Cells[cell.ColumnIndex].Value = value;
            }
            var year = DateTime.Now;
            int y = year.Year;
            var d = dgvHolidays.Columns[cell.ColumnIndex].Name.Split('/')[0];
            var m = dgvHolidays.Columns[cell.ColumnIndex].Name.Split('/')[1];
            if (dgvHolidays.Columns[cell.ColumnIndex - 1].Name == "31/12") year = year.AddYears(1);
            int.TryParse(d, out var day);
            int.TryParse(m, out var month);

            var newHoliday = new DateTime(y, month, day);
            if (value == "X")
            {
                _toDelete.Remove(newHoliday);
                if (!_toInsert.Contains(newHoliday.Date)) _toInsert.Add(newHoliday);
            }
            else
            {
                _toDelete.Add(newHoliday);
                if (_toInsert.Contains(newHoliday.Date)) _toInsert.Remove(newHoliday);
            }
        }
        private void BuildGridStructure()
        {
            dc = new DataContext(MainWnd.conString);
            _toDelete = new List<DateTime>();
            _toInsert = new List<DateTime>();
            _definedHolidays = new List<Lines>();
            var _structure = new DataTable();

            if (!(dgvHolidays.DataSource is null)) dgvHolidays.DataSource = null;

            var holidays = (from h in Tables.TblHolidays
                            where h.Holiday >= StartDate.Date &&
                            h.Holiday.Date <= EndDate.Date
                            select h).ToList();
            foreach (var item in holidays)
            {
                if (!_dbHolidays.Contains(item.Holiday.Date)) _dbHolidays.Add(item.Holiday);
            }            
            if (holidays.Count >= 1)
            {
                foreach (var h in holidays)
                {
                    var queryLine = (from l in _definedHolidays
                                     where l.LineName == h.LineName.ToString()
                                     select l).SingleOrDefault();
                    if (queryLine == null)
                    {
                        _definedHolidays.Add(new Lines(h.Holiday, h.LineName));
                    }
                    else
                    {
                        queryLine._holidays.Add(h.Holiday);
                    }
                }
            }
            _structure.Columns.Add("Lines");
            var weekendColumn = 1;
            for(var date = StartDate; date <= EndDate; date = date.AddDays(+1))
            { 
                _structure.Columns.Add(date.ToString("dd/MM", CultureInfo.InvariantCulture));
                weekendColumn++;
            }
            foreach(var line in new string[] { "Line 1", "Line 2", "Line 3", "Line 4", "Line 5", "Line 6", "Line 7",
                                               "Line 8", "Line 9", "Line 10", "Line 11", "Line 12", "Line 13", "Line 14", "Line 15"})
            {
                var newRow = _structure.NewRow();
                newRow[0] = line;
                _structure.Rows.Add(newRow);
            }
            dgvHolidays.DataSource = _structure;
            foreach (DataGridViewRow row in dgvHolidays.Rows)
            {
                var line = row.Cells[0].Value.ToString();
                var lineQuery = (from l in _definedHolidays
                                 where l.LineName == line
                                 select l).SingleOrDefault();
                if (lineQuery != null && lineQuery._holidays.Count >= 1)
                {
                    foreach (var day in lineQuery._holidays)
                    {
                        row.Cells[day.ToString("dd/MM", CultureInfo.InvariantCulture)].Value = "X";
                    }
                }
                var currentYear = StartDate;
                for(var i = 1; i<dgvHolidays.Columns.Count; i++)
                {
                    if (dgvHolidays.Rows[0].Cells[i].Value.ToString() == "X")
                    {
                        //if (dgvHolidays.Columns[i - 1].Name == "31/12") currentYear = currentYear.AddYears(1);
                        var year = currentYear.Year;
                        int.TryParse(dgvHolidays.Columns[i].Name.Split('/')[1], out var month);
                        int.TryParse(dgvHolidays.Columns[i].Name.Split('/')[0], out var day);
                        DateTime date = new DateTime(year, month, day);
                        _toInsert.Add(date);
                    }
                }
            }
        }
        private void UpdateHolidays()
        {
            dc = new DataContext(MainWnd.conString);
            foreach (var date in _toInsert)
            {
                if (!_dbHolidays.Contains(date.Date))
                {                    
                    foreach (var line in new string[] { "Line 1", "Line 2", "Line 3", "Line 4", "Line 5", "Line 6", "Line 7",
                                               "Line 8", "Line 9", "Line 10", "Line 11", "Line 12", "Line 13", "Line 14", "Line 15"})
                    {
                        var newHoliday = new Holidays()
                        {
                            Holiday = date,
                            LineName = line
                        };
                        Tables.TblHolidays.InsertOnSubmit(newHoliday);
                    }                    
                }
            }            
            foreach (var item in _toDelete)
            {
                var deleted = (from d in Tables.TblHolidays
                               where d.Holiday.Date == item.Date
                               select d).ToList();
                foreach(var d in deleted)
                {
                    Tables.TblHolidays.DeleteOnSubmit(d);
                }            
            }
            dc.SubmitChanges();
        }
        private void btnGo_Click(object sender, EventArgs e)
        {
            BuildGridStructure();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            UpdateHolidays();
        }
    }
}
