using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Sinotico.DatabaseTableClasses;
using System.Drawing.Drawing2D;

namespace Sinotico
{
    public partial class RepMachines : Form
    {
        MainWnd _main_wnd = new MainWnd();
        private int _yearFrom, _monthFrom, _yearTo, _monthTo;
        private DataTable _table_report = new DataTable();
        private DataTable _table_data = new DataTable();
        private delegate void TableDelegate();
        private Line _line = new Line();
        private System.Text.StringBuilder _fin_array = new System.Text.StringBuilder();
        private DataRow totalRow;
        private bool isOperatorMode;
        private List<LineOperator> _currentOperators = new List<LineOperator>();

        public RepMachines()
        {
            InitializeComponent();
            dgvReport.DoubleBufferedDataGridView(true);
            cbGreen.CheckedChanged += cbColorFilter_CheckChanged;
            cbMostra.CheckedChanged += cbColorFilter_CheckChanged;
            cbYellow.CheckedChanged += cbColorFilter_CheckChanged;
            cbRed.CheckedChanged += cbColorFilter_CheckChanged;
            cbRed.Paint += CheckboxStyle_Paint;
            cbYellow.Paint += CheckboxStyle_Paint;
            cbGreen.Paint += CheckboxStyle_Paint;
            cbMostra.Paint += CheckboxStyle_Paint;
        }
        public DateTime GetDateFrom
        {
            get => new DateTime(dtpFrom.Value.Year
                                ,dtpFrom.Value.Month
                                ,dtpFrom.Value.Day);
            set
            {
                dtpFrom.Value = value;
            }
        }
        public DateTime GetDateTo
        {
            get => new DateTime(dtpTo.Value.Year
                                ,dtpTo.Value.Month
                                ,dtpTo.Value.Day); 
            set
            {
                dtpTo.Value = value;
            }
        }
        
        protected override void OnLoad(EventArgs e)
        {
            PopulateOperatorsCbo();
            isOperatorMode = false;
            cboMedia.Items.Clear();
            cboMedia.Items.Add("<All>");
            cboMedia.Items.Add("Green range");
            cboMedia.Items.Add("Yellow range");
            cboMedia.Items.Add("Red range");
            dgvReport.AllowUserToAddRows = false;
            dgvReport.AllowUserToDeleteRows = false;
            dgvReport.AllowUserToResizeColumns = false;
            dgvReport.AllowUserToResizeRows = false;
            dgvReport.AllowUserToOrderColumns = false;
            dgvReport.ReadOnly = true;
            dgvReport.AllowUserToOrderColumns = false;
            _from_machine = 1;
            _to_machine = 210;
            LoadReport();
            for (var i = 1; i <= 15; i++)
            {
                if (i < 4)
                {
                    cboBlocks.Items.Add("Squadra " + i.ToString());
                }
                cboLines.Items.Add("LINEA " + i.ToString());
            }
            cbMostra.Checked = true;
            base.OnLoad(e);
        }

        private void RepMachines_Load(object sender, EventArgs e)
        {

        }

        private void CreateTableView(DataTable table)
        {
            table.Columns.Add("linea", typeof(int));
            table.Columns.Add("operator");
            table.Columns.Add("sep1");
            table.Columns.Add("giorni");
            table.Columns.Add("sep2");
            table.Columns.Add(GetDateFrom.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture));
            table.Columns.Add("sep3");
            table.Columns.Add("Media", typeof(double));
            table.Columns.Add("Squadra", typeof(string));
        }

        private void LoadReport()
        {
            try
            {
                LoadingInfo.InfoText = "Loading report...";
                LoadingInfo.ShowLoading();                
                btnSort.BackColor = Color.White;
                _yearFrom = GetDateFrom.Year;
                _monthFrom = GetDateFrom.Month;
                _yearTo = GetDateTo.Year;
                _monthTo = GetDateTo.Month;
                var startDate = new DateTime(_yearFrom, _monthFrom, GetDateFrom.Day);
                var endDate = new DateTime(_yearTo, _monthTo, GetDateTo.Day);
                var dayRange = endDate.Subtract(startDate).Days;
                var media = 0.0;
                var workDays = 0;
                _table_report = new DataTable();
                if (dgvReport.DataSource != null)
                    dgvReport.DataSource = null;
                if(isOperatorMode)
                {
                    if(GetDateTo.Subtract(GetDateFrom).TotalDays.Equals(0))
                    {
                        LoadProcedure(startDate, endDate, cboMedia.Text); //populate table 
                        LoadOperators(GetDateFrom, cboOperators.Text);
                        CreateTableView(_table_report);

                        if (_currentOperators.Count <= 0)
                        {
                            LoadingInfo.CloseLoading();
                            MessageBox.Show("There is no data for selected operator!"
                                            ,"Wrong selection"
                                            ,MessageBoxButtons.OK
                                            ,MessageBoxIcon.Warning);
                            return;
                        }

                        foreach(DataRow r in _table_data.Rows)
                        {
                            var line = r[0].ToString();
                            double.TryParse(r[1].ToString(), out var efficiency);
                            var shift = r[2].ToString();

                            if (string.IsNullOrEmpty(line))
                                continue;
                            
                            var lineOperators = (from o in _currentOperators
                                                where o.WorkingLine == line && o.Shift == shift
                                                select o).ToList();
                            if (lineOperators == null || lineOperators.Count <= 0)
                                continue;
                            foreach(var lineOperator in lineOperators)
                                lineOperator.Efficiency = efficiency;
                        }

                        var totRow = _table_report.NewRow();
                        totRow["linea"] = 0;
                        totRow["operator"] = "Total";
                        totRow["giorni"] = 0;
                        _table_report.Rows.Add(totRow);
                        foreach(var line in new string[] { "LINE 1", "LINE 2", "LINE 3", "LINE 4", "LINE 5",
                                                           "LINE 6", "LINE 7", "LINE 8", "LINE 9", "LINE 10",
                                                           "LINE 11", "LINE 12", "LINE 13", "LINE 14", "LINE 15"})
                        {
                            var lineOperators = (from o in _currentOperators
                                                 where o.WorkingLine == line
                                                 select o).ToList();

                            if (lineOperators.Count <= 0 || lineOperators == null)
                                continue;

                            var totLineRow = _table_report.NewRow();
                            var counter = 0;
                            double totEff = 0.0;
                            foreach(var currentOperator in lineOperators)
                            {
                                var newRow = _table_report.NewRow();
                                int.TryParse(currentOperator.WorkingLine.Split(' ')[1], out var lineNr);
                                newRow["linea"] = lineNr;
                                newRow["operator"] = currentOperator.FullName;
                                newRow["giorni"] = 1;
                                newRow[GetDateFrom.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture)] = currentOperator.Efficiency;
                                newRow["Media"] = currentOperator.Efficiency;
                                totLineRow["operator"] = currentOperator.WorkingLine;
                                totEff += currentOperator.Efficiency;
                                counter++;
                                _table_report.Rows.Add(newRow);
                            }
                            var lineTotEff = Math.Round(totEff / counter, 1);
                            totLineRow[GetDateFrom.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture)] = lineTotEff;
                            totLineRow["Media"] = lineTotEff;
                            _table_report.Rows.Add(totLineRow);
                        }
                        dgvReport.DataSource = _table_report;
                        CustomizeDataGridView();
                        dgvReport.Refresh();
                        LoadingInfo.CloseLoading();
                        if (dgvReport.Rows.Count <= 1)
                            return;
                        CalculateTotalEff(totRow);                        
                        return;
                    }
                    else
                    {
                        LoadingInfo.CloseLoading();
                        MessageBox.Show("Select only one day!"
                                        ,"Wrong selection"
                                        ,MessageBoxButtons.OK
                                        ,MessageBoxIcon.Warning);
                        return;
                    }
                }
                _table_report.Columns.Add("linea", typeof(int));
                _table_report.Columns.Add("macchina");
                _table_report.Columns.Add("sep1");
                _table_report.Columns.Add("giorni");
                _table_report.Columns.Add("sep2");
                for (var date = new DateTime(_yearFrom, _monthFrom, GetDateFrom.Day);
                         date <= new DateTime(_yearTo, _monthTo, GetDateTo.Day);
                         date = date.AddDays(+1))
                {
                    var col = new DataColumn()
                    {
                        ColumnName = date.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture),
                    };
                    _table_report.Columns.Add(col);
                }
                _table_report.Columns.Add("sep3");
                _table_report.Columns.Add("Media", typeof(double));
                _table_report.Columns.Add("Squadra", typeof(string));
                LoadProcedure(startDate, endDate, cboMedia.Text);
                var firstRead = true;
                var lst_of_totals = new List<string>();
                var lastLine = string.Empty;
                // Add total row
                totalRow = _table_report.NewRow();
                totalRow[1] = "Total";
                _table_report.Rows.Add(totalRow);
                var row = _table_report.NewRow();
                for (int i = _from_machine; i <= _to_machine; i++)
                {
                    media = 0.0;
                    workDays = 0;
                    if (firstRead)
                    {
                        row = _table_report.NewRow();
                        row[0] = Convert.ToInt32(_line.GetLineNumber(i).Split(' ')[1]);
                        row[1] = i.ToString().PadLeft(3, '0');

                        foreach (DataRow dRow in _table_data.Rows)
                        {
                            var machine = dRow[0].ToString();
                            var eff = dRow[1].ToString();
                            var date = Convert.ToDateTime(dRow.ItemArray.GetValue(2)).ToString("yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);

                            if (machine != i.ToString())
                                continue;

                            row[date] = eff;
                            double.TryParse(eff, out var tEff);
                            media += tEff;
                            workDays++;
                        }

                        row[3] = workDays.ToString();

                        if (media == 0 || workDays == 0)
                            continue;

                        row["Media"] = Math.Round((media / workDays), 1);
                        _table_report.Rows.Add(row);

                        firstRead = false;
                        lastLine = _line.GetLineNumber(i).Split(' ')[1];
                    }
                    else if (lastLine == _line.GetLineNumber(i).Split(' ')[1])
                    {
                        row = _table_report.NewRow();
                        row[0] = Convert.ToInt32(_line.GetLineNumber(i).Split(' ')[1]);
                        row[1] = i.ToString().PadLeft(3, '0');

                        foreach (DataRow dRow in _table_data.Rows)
                        {
                            var machine = dRow[0].ToString();
                            var eff = dRow[1].ToString();
                            var date = Convert.ToDateTime(dRow.ItemArray.GetValue(2)).ToString("yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);

                            if (machine != i.ToString())
                                continue;

                            row[date] = eff;
                            double.TryParse(eff, out var tEff);
                            media += tEff;
                            workDays++;
                        }

                        row[3] = workDays.ToString();

                        if (media == 0 || workDays == 0)
                            continue;

                        row["Media"] = Math.Round((media / workDays), 1);
                        _table_report.Rows.Add(row);
                        lastLine = _line.GetLineNumber(i).Split(' ')[1];
                    }
                    else
                    {
                        //tRow.SetParentRow(row);
                        if (!lst_of_totals.Contains("LINEA " + lastLine + " TOTALE"))
                        {
                            var tRow = _table_report.NewRow();
                            tRow[1] = "LINEA " + lastLine + " TOTALE";
                            lst_of_totals.Add(tRow[1].ToString());
                            _table_report.Rows.Add(tRow);
                            CalculateTotalEff(tRow, lastLine);
                        }

                        row = _table_report.NewRow();
                        row[0] = Convert.ToInt32(_line.GetLineNumber(i).Split(' ')[1]);
                        row[1] = i.ToString().PadLeft(3, '0');

                        foreach (DataRow dRow in _table_data.Rows)
                        {
                            var machine = dRow[0].ToString();
                            var eff = dRow[1].ToString();
                            var date = Convert.ToDateTime(dRow.ItemArray.GetValue(2)).ToString("yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);

                            if (machine != i.ToString())
                                continue;

                            row[date] = eff;
                            double.TryParse(eff, out var tEff);
                            media += tEff;
                            workDays++;
                        }

                        row[3] = workDays.ToString();

                        if (media == 0 || workDays == 0)
                            continue;

                        row["Media"] = Math.Round((media / workDays), 1);

                        //separtor
                        //var sepRow = _table_report.NewRow();
                        //_table_report.Rows.Add(sepRow);
                        //separator 

                        _table_report.Rows.Add(row);
                        lastLine = _line.GetLineNumber(i).Split(' ')[1];
                    }
                }

                var lastRow = _table_report.NewRow();
                lastRow[1] = "LINEA " + lastLine + " TOTALE"; //i.ToString().PadLeft(3, '0');
                _table_report.Rows.Add(lastRow);
                CalculateTotalEff(lastRow, lastLine);

                //lastRow.SetParentRow(row);

                dgvReport.DataSource = _table_report;

                CustomizeDataGridView();
                dgvReport.Refresh();
                LoadingInfo.CloseLoading();
                if (dgvReport.Rows.Count <= 1)
                    return;
                CalculateTotalEff(totalRow);
            }
            catch (Exception ex)
            {
                //LoadingInfo.CloseLoading();
                MessageBox.Show(ex.Message);
            }
            finally
            {
                LoadingInfo.CloseLoading();
                MainWnd._sql_con.Close();
            }
        }

        private void CalculateTotalEff(DataRow row)
        {
            var total = 0.0;
            var count = 0;
            for (var c = 5; c <= _table_report.Columns.Count - 1; c++)
            {
                total = 0;
                count = 0;
                for (var r = 1; r <= _table_report.Rows.Count - 1; r++)
                {
                    if (string.IsNullOrEmpty(_table_report.Rows[r][c].ToString())) continue;

                    if (string.IsNullOrEmpty(_table_report.Rows[r][0].ToString())
                        && string.IsNullOrEmpty(_table_report.Rows[r][3].ToString()))
                    {
                        var totEff = double.TryParse(_table_report.Rows[r][c].ToString().Split('%')[0], out var e);

                        total += e;
                        count++;
                    }
                }
                row[0] = "0";
                row[3] = "0";
                if (count == 0) count = 1;
                var totalValue = Math.Round((total / count),1).ToString();
                if (totalValue == "0" ||
                    string.IsNullOrEmpty(totalValue)) totalValue = string.Empty;
                else
                    row[c] = totalValue;
            }
        }

        private void CalculateTotalEff(DataRow row, string line)
        {
            var total = 0.0;
            //var count = 0;
            for (var c = 5; c <= _table_report.Columns.Count - 1; c++)
            {
                total = 0;
                //count = 0;
                for (var r = 1; r <= _table_report.Rows.Count - 1; r++)
                {
                    if (!string.IsNullOrEmpty(_table_report.Rows[r][c].ToString()))
                        if (_table_report.Rows[r][0].ToString() == line)
                        {
                            double.TryParse(_table_report.Rows[r][c].ToString().Split('%')[0], out var tmpEff);
                            total += tmpEff;
                            //count++;
                        }
                }
                //if (count == 0) count = 1;
                var totalValue = Math.Round((total / 14),1).ToString();

                if (totalValue == "0" ||
                    string.IsNullOrEmpty(totalValue)) totalValue = string.Empty;
                else
                    row[c] = totalValue;
            }
        }

        private void LoadProcedure(DateTime start, DateTime end, string color)
        {
            _table_data = new DataTable();

            var cmd = new SqlCommand("get_data_in_month", MainWnd._sql_con)
            {
                CommandType = CommandType.StoredProcedure
            };
            if (string.IsNullOrEmpty(color) || color == "<All>")
                color = string.Empty;
            cmd.Parameters.Add("@from_date", SqlDbType.DateTime).Value = start;
            cmd.Parameters.Add("@to_date", SqlDbType.DateTime).Value = end;
            cmd.Parameters.Add("@shift", SqlDbType.VarChar).Value = MainWnd.Get_shift_array().ToString();
            cmd.Parameters.Add("@finesse", SqlDbType.VarChar).Value = MainWnd.Get_fin_array().ToString();
            cmd.Parameters.Add("@colore", SqlDbType.VarChar).Value = color;
            cmd.Parameters.Add("@operatorMode", SqlDbType.Bit).Value = isOperatorMode;

            MainWnd._sql_con.Open();
            var dr = cmd.ExecuteReader();
            _table_data.Load(dr);
            MainWnd._sql_con.Close();
            dr.Close();
            cmd = null;
        }

        private bool _sorted = false;
        private void button1_Click(object sender, EventArgs e)
        {
            if (_sorted) return;
            if (!_reset) return;
            if (dgvReport.Rows.Count <= 2 || dgvReport.Columns.Count == 0 ||
                _table_report.Rows.Count <= 2)
            {
                MessageBox.Show("No data to be sorted.", "Sinotico", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _sorted = true;
            LoadingInfo.InfoText = "Sorting data...";
            LoadingInfo.ShowLoading();
            //remove sub-totals
            foreach (DataGridViewRow row in dgvReport.Rows)
            {
                if (string.IsNullOrEmpty(row.Cells[0].Value.ToString()) &&
                    string.IsNullOrEmpty(row.Cells[2].Value.ToString()))
                {
                    dgvReport.Rows.Remove(row);
                } 
            }

            DataView view = _table_report.DefaultView;
            view.Sort = "linea ASC, Media ASC";
            if (dgvReport.DataSource != null) dgvReport.DataSource = null;
            dgvReport.DataSource = view;
            CustomizeDataGridView();
            btnSort.BackColor = Color.Yellow;
            LoadingInfo.CloseLoading();
        }

        private void dgvReport_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (_sorted && dgvReport.Rows.Count > 0 && e.RowIndex > 0 && e.RowIndex < dgvReport.Rows.Count - 1 &&
                e.ColumnIndex >= 0 && e.ColumnIndex <= dgvReport.Columns.Count - 1)
            {
                for (var i = 1; i <= 15; i++)
                {
                    // Tests to see the difference between previous and next line value

                    if (dgvReport.Rows[e.RowIndex - 1].Cells[0].Value.ToString() == i.ToString() &&
                         dgvReport.Rows[e.RowIndex].Cells[0].Value.ToString() != i.ToString())
                    {
                        // Draws line splitter between cur-line end and new line index

                        e.Graphics.FillRectangle(new SolidBrush(e.CellStyle.BackColor),
                            e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width, e.CellBounds.Height);

                        //e.Graphics.DrawLine(Pens.Red,e.CellBounds.X,e.CellBounds.Y, e.CellBounds.Width,e.CellBounds.Y);

                        // Draws values in the splitter rectangle

                        if (e.ColumnIndex == 0)
                        {
                            var str = new StringFormat();
                            str.LineAlignment = StringAlignment.Center;
                            str.Alignment = StringAlignment.Center;

                            e.Graphics.DrawString(e.Value.ToString(),
                                e.CellStyle.Font, Brushes.Black, e.CellBounds.Width / 2, e.CellBounds.Y + 15, str);
                        }
                        else if (e.ColumnIndex == 1)
                        {
                            e.Graphics.DrawString(e.Value.ToString(),
                                e.CellStyle.Font, Brushes.Black, e.CellBounds.X + 60, e.CellBounds.Y + 8, StringFormat.GenericDefault);
                        }
                        else if (e.ColumnIndex == 2)
                        {
                            e.Graphics.DrawString(e.Value.ToString(),
                                e.CellStyle.Font, Brushes.Black, e.CellBounds.X + 19, e.CellBounds.Y + 8, StringFormat.GenericDefault);
                        }
                        else if (e.ColumnIndex > 2 && e.ColumnIndex < dgvReport.ColumnCount - 1)
                        {
                            e.Graphics.DrawString(e.Value.ToString(),
                                e.CellStyle.Font, Brushes.Black, e.CellBounds.X + 10, e.CellBounds.Y + 9, StringFormat.GenericDefault);
                        }
                        else if (e.ColumnIndex == dgvReport.ColumnCount - 1)
                        {
                            e.Graphics.DrawString(e.Value.ToString(),
                               e.CellStyle.Font, Brushes.Black, e.CellBounds.X + 1, e.CellBounds.Y + 9, StringFormat.GenericDefault);

                            e.Handled = true;
                        }
                    }
                }
            }
            else if (_is_squadra_filter && dgvReport.Rows.Count > 1 && e.RowIndex > 0 && e.RowIndex < dgvReport.Rows.Count - 1 &&
                    e.ColumnIndex >= 0 && e.ColumnIndex <= dgvReport.Columns.Count - 1)
            {
                for (var i = 5; i <= 15; i += 5)
                {
                    var firstComp = Convert.ToInt32(dgvReport.Rows[e.RowIndex - 1].Cells[1].Value.ToString().Split(' ')[1]);
                    var secondComp = Convert.ToInt32(dgvReport.Rows[e.RowIndex].Cells[1].Value.ToString().Split(' ')[1]);

                    if (firstComp == i && secondComp != i)
                    {
                        e.Graphics.FillRectangle(new SolidBrush(e.CellStyle.BackColor),
                            e.CellBounds.X,
                            e.CellBounds.Y,
                            e.CellBounds.Width,
                            e.CellBounds.Height);

                        e.Graphics.DrawLine(Pens.Red,
                            e.CellBounds.X,
                            e.CellBounds.Y,
                            e.CellBounds.Width,
                            e.CellBounds.Y);

                        // Draws values in the splitter rectangle

                        if (e.ColumnIndex == 0)
                        {
                            var str = new StringFormat();
                            str.LineAlignment = StringAlignment.Center;
                            str.Alignment = StringAlignment.Center;

                            e.Graphics.DrawString(e.Value.ToString(),
                                e.CellStyle.Font,
                                Brushes.Black,
                                e.CellBounds.Width / 2,
                                e.CellBounds.Y + 15, str);
                        }
                        else if (e.ColumnIndex == 1)
                        {
                            e.Graphics.DrawString(e.Value.ToString(),
                                e.CellStyle.Font,
                                Brushes.Black,
                                e.CellBounds.X + 60,
                                e.CellBounds.Y + 8,
                                StringFormat.GenericDefault);
                        }
                        else if (e.ColumnIndex == 2)
                        {
                            e.Graphics.DrawString(e.Value.ToString(),
                                e.CellStyle.Font,
                                Brushes.Black,
                                e.CellBounds.X + 19,
                                e.CellBounds.Y + 8,
                                StringFormat.GenericDefault);
                        }
                        else if (e.ColumnIndex > 2 && e.ColumnIndex < dgvReport.ColumnCount - 1)
                        {
                            e.Graphics.DrawString(e.Value.ToString(),
                                e.CellStyle.Font,
                                Brushes.Black,
                                e.CellBounds.X + 10,
                                e.CellBounds.Y + 9,
                                StringFormat.GenericDefault);
                        }
                        else if (e.ColumnIndex == dgvReport.ColumnCount - 1)
                        {
                            e.Graphics.DrawString(e.Value.ToString(),
                               e.CellStyle.Font,
                               Brushes.Black,
                               e.CellBounds.X + 1,
                               e.CellBounds.Y + 9,
                               StringFormat.GenericDefault);

                            e.Handled = true;
                        }
                    }
                }
            }
        }

        private int _from_machine;
        private int _to_machine;
        private bool _is_squadra_filter = false;
        private void cboLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboBlocks.Text = string.Empty;
            cboMedia.Text = string.Empty;
            cboOperators.Text = string.Empty;
            _is_squadra_filter = false;
            if (cboLines.SelectedIndex == 0)
            {
                _from_machine = 1;
                _to_machine = 210;
                cboLines.Text = "";
                cboBlocks.Text = "";
                cboMedia.Text = string.Empty;
                cboOperators.Text = string.Empty;
                LoadReport();
            }
            else
            {
                var idx = cboLines.SelectedIndex;
                _to_machine = idx * 14;
                _from_machine = _to_machine - 13;
                LoadReport();
            }
        }

        private void cboBlocks_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboLines.Text = string.Empty;
            cboMedia.Text = string.Empty;
            cboOperators.Text = string.Empty;
            var i = cboBlocks.SelectedIndex;
            _is_squadra_filter = false;
            _sorted = false;
            _reset = false;
            if (i == 0)
            {
                _from_machine = 1;
                _to_machine = 210;
                cboBlocks.Text = string.Empty;
                cboLines.Text = string.Empty;
                cboMedia.Text = string.Empty;
                cboOperators.Text = string.Empty;
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
            LoadReport();
            _table_report.AcceptChanges();
            foreach (DataRow row in _table_report.Rows)
            {
                if (row[0].ToString() != string.Empty && row[2].ToString() != string.Empty &&
                    row[1].ToString() != "Total")
                {
                    row.Delete();
                }
            }
            _table_report.AcceptChanges();
            if (_is_squadra_filter)
            {
                var medEff = 0.0;
                var r = 0;
                _table_report.AcceptChanges();
                foreach (DataRow row in _table_report.Rows)
                {
                    //if (row[1].ToString() == "Total") continue;
                    r++;
                    if (row[1].ToString() != "Total")
                    {
                        double.TryParse(row[_table_report.Columns.Count - 2].ToString(), out var eff);
                        medEff += eff;
                    }
                    if (r == 6)
                    {
                        row[_table_report.Columns.Count - 1] = Math.Round((medEff / 5),1).ToString();

                        r = 1;
                        medEff = 0;
                    }
                }
                _table_report.AcceptChanges();
            }
            CustomizeDataGridView();

            //when all machines in one line are turned off we see duplicates of last active line
            //couldn't find where's problem
            // here we delete all duplicates
            var str = dgvReport.Rows[1].Cells[1].Value.ToString();
            for(var r = 2; r < dgvReport.Rows.Count; r ++)
            {
                var s = dgvReport.Rows[r].Cells[1].Value.ToString();
                if (str == s)
                    dgvReport.Rows.Remove(dgvReport.Rows[r - 1]);
                str = s;
            }
        }

        //private void dgvReport_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        //{
        //    //CustomizeDataGridView();
        //}

        private bool _reset = true;
        private void button2_Click(object sender, EventArgs e)
        {
            _is_squadra_filter = false;
            _sorted = false;
            _from_machine = 1;
            _to_machine = 210;
            LoadReport();
            cboBlocks.Text = string.Empty;
            cboLines.Text = string.Empty;
            cboMedia.Text = string.Empty;
            cboOperators.Text = string.Empty;
            _reset = true;
        }

        private void btnLineTot_Click(object sender, EventArgs e)
        {
            isOperatorMode = false;
            _is_squadra_filter = false;
            _sorted = false;
            _from_machine = 1;
            _to_machine = 210;
            LoadReport();
            cboBlocks.Text = string.Empty;
            cboLines.Text = string.Empty;
            cboMedia.Text = string.Empty;
            cboOperators.Text = string.Empty;
            _reset = true;
        }

        private void btnSquadraTot_Click(object sender, EventArgs e)
        {
            isOperatorMode = false;
            _is_squadra_filter = false;
            _sorted = false;
            _reset = false;
            _from_machine = 1;
            _to_machine = 210;
            LoadReport();
            cboBlocks.Text = string.Empty;
            cboLines.Text = string.Empty;
            cboMedia.Text = string.Empty;
            cboOperators.Text = string.Empty;
            _table_report.AcceptChanges();
            foreach (DataRow row in _table_report.Rows)
            {
                if (row[0].ToString() != string.Empty && row[2].ToString() != string.Empty &&
                    row[1].ToString() != "Total")
                {
                    row.Delete();
                }
            }
            _table_report.AcceptChanges();
            if (_is_squadra_filter)
            {
                var medEff = 0.0;
                var r = 0;
                _table_report.AcceptChanges();
                foreach (DataRow row in _table_report.Rows)
                {
                    r++;
                    if (row[1].ToString() != "Total")
                    {
                        double.TryParse(row[_table_report.Columns.Count - 2].ToString(), out var eff);
                        medEff += eff;
                    }
                    if (r == 6)
                    {
                        row[_table_report.Columns.Count - 1] = Math.Round((medEff / 5),1).ToString();

                        r = 1;
                        medEff = 0;
                    }
                }
                _table_report.AcceptChanges();
            }
            CustomizeDataGridView();
        }
        
        private void dgvReport_Scroll(object sender, ScrollEventArgs e)
        {
            dgvReport.Invalidate();
        }

        private void CustomizeDataGridView()
        {
            dgvReport.Focus();
            if (dgvReport.Rows.Count >= 1)
            {
                if (dgvReport.Rows[0].Cells[1].Value.ToString() == "Total")
                {
                    dgvReport.Rows[0].ReadOnly = true;
                    dgvReport.Rows[0].Frozen = true;
                }
            }
            // specialize first 3 columns
            dgvReport.Columns[0].HeaderCell.Style.ForeColor = Color.Black;
            dgvReport.Columns[0].HeaderCell.Style.Font = new Font("Tahoma", 9, FontStyle.Bold);
            dgvReport.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvReport.Columns[0].Width = 50;
            dgvReport.Columns[0].DataGridView.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
            dgvReport.Columns[1].HeaderCell.Style.ForeColor = Color.Black;
            dgvReport.Columns[1].HeaderCell.Style.Font = new Font("Tahoma", 9, FontStyle.Bold);
            dgvReport.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvReport.Columns[1].Width = 180;
            dgvReport.Columns[1].HeaderText = "nr macchina";
            dgvReport.Columns[1].DataGridView.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
            dgvReport.Columns[3].HeaderCell.Style.ForeColor = Color.Black;
            dgvReport.Columns[3].HeaderCell.Style.Font = new Font("Tahoma", 9, FontStyle.Bold);
            dgvReport.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvReport.Columns[3].Width = 50;
            dgvReport.Columns[3].DataGridView.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
            foreach (DataGridViewColumn column in dgvReport.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dgvReport.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 8, FontStyle.Bold);
            dgvReport.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 192, 0);
            for (var c = 5; c <= dgvReport.Columns.Count - 4; c++)
            {
                dgvReport.Columns[c].Width = 40;

                dgvReport.Columns[c].HeaderText =
                Convert.ToDateTime(dgvReport.Columns[c].Name).ToString("dd/MM", System.Globalization.CultureInfo.InvariantCulture);
                dgvReport.Columns[c].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            // media column
            dgvReport.Columns["Media"].Width = 50;
            dgvReport.Columns["Media"].DefaultCellStyle.BackColor = Color.LightGray;
            // freeze first 3 columns at the end
            for (var i = 0; i <= 4; i++)
                dgvReport.Columns[i].Frozen = true;
            dgvReport.Columns["Squadra"].Width = 50;
            var visib = false;
            if (_is_squadra_filter) visib = true;
            dgvReport.Columns["Squadra"].Visible = visib;
            // total row
            dgvReport.Rows[0].DefaultCellStyle.Font = new Font("Tahoma", 8, FontStyle.Regular);
            dgvReport.Rows[0].DefaultCellStyle.ForeColor = Color.Black;
            dgvReport.Rows[0].DefaultCellStyle.BackColor = Color.FromArgb(255, 192, 0);
            dgvReport.Rows[0].DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 192, 0);
            dgvReport.Rows[0].DefaultCellStyle.SelectionForeColor = dgvReport.Rows[0].DefaultCellStyle.ForeColor;
            for (var r = 1; r <= dgvReport.Rows.Count - 1; r++)
            {
                for (var c = 5; c <= dgvReport.Columns.Count - 4; c++)
                {
                    var eff = double.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString().Split('%')[0], out var effic);
                    if ((effic > 0.0 && effic < 85.0 || string.IsNullOrEmpty(dgvReport.Rows[r].Cells[c].Value.ToString()))
                        && !string.IsNullOrEmpty(dgvReport.Rows[r].Cells[0].Value.ToString()))
                        dgvReport.Rows[r].Cells[c].Style.BackColor = Color.FromArgb(255, 234, 232);
                    if (Convert.ToDateTime(dgvReport.Columns[c].Name).DayOfWeek == DayOfWeek.Sunday)
                        dgvReport.Rows[r].Cells[c].Style.BackColor = Color.Gainsboro;
                }
                if (string.IsNullOrEmpty(dgvReport.Rows[r].Cells[0].Value.ToString())
                && dgvReport.Rows[r].Cells[1].Value.ToString() != "Total"
                && string.IsNullOrEmpty(dgvReport.Rows[r].Cells[3].Value.ToString())
                && !string.IsNullOrEmpty(dgvReport.Rows[r].Cells[1].Value.ToString()))
                {
                    dgvReport.Rows[r].DefaultCellStyle.Font = new Font("Tahoma", 8, FontStyle.Bold);
                    //dgvReport.Rows[r].DefaultCellStyle.ForeColor = Color.Black;
                    dgvReport.Rows[r].DefaultCellStyle.BackColor = Color.FromArgb(218, 216, 218); ;
                    dgvReport.Rows[r].DefaultCellStyle.SelectionBackColor = Color.OldLace;
                    dgvReport.Rows[r].DefaultCellStyle.SelectionForeColor = Color.Black;
                }
                dgvReport.Rows[r].Cells[dgvReport.ColumnCount - 1].Style.BackColor = Color.White;
                dgvReport.Rows[r].Cells[dgvReport.ColumnCount - 1].Style.SelectionBackColor = Color.White;
                if (dgvReport.Rows[r].Cells[dgvReport.ColumnCount - 1].Value.ToString() != string.Empty)
                    dgvReport.Rows[r].Cells[dgvReport.ColumnCount - 1].Style.SelectionBackColor = Color.OldLace;
                if (string.IsNullOrEmpty(dgvReport.Rows[r].Cells[1].Value.ToString()))
                {
                    dgvReport.Rows[r].Height = 10;
                    dgvReport.Rows[r].DefaultCellStyle.BackColor = Color.White;
                }
            }
            foreach (var columnName in new string[] { "sep1", "sep2", "sep3" })
            {
                dgvReport.Columns[columnName].HeaderText = string.Empty;
                dgvReport.Columns[columnName].Width = 10;
                dgvReport.Columns[columnName].DefaultCellStyle.BackColor = dgvReport.BackgroundColor;
                dgvReport.Columns[columnName].DefaultCellStyle.SelectionBackColor = dgvReport.BackgroundColor;
                dgvReport.Columns[columnName].DefaultCellStyle.SelectionForeColor = dgvReport.BackgroundColor;
                dgvReport.Columns[columnName].HeaderCell.Style.BackColor = dgvReport.BackgroundColor;
            }
            foreach(DataGridViewRow row in dgvReport.Rows)
            {
                foreach (var columnName in new string[] { "sep1", "sep2", "sep3" })
                {
                    row.Cells[columnName].Style.BackColor = dgvReport.BackgroundColor;
                    row.Cells[columnName].Style.SelectionBackColor = dgvReport.BackgroundColor;
                    row.Cells[columnName].Style.SelectionForeColor = dgvReport.BackgroundColor;
                }
            }
        }

        public void ExportToExcel()
        {
            var export = new ExcelExport();
            export.ExportToExcel(dgvReport, Text);
        }

        private List<string> _operators = new List<string>();
        private void PopulateOperatorsCbo()
        {
            cboOperators.Items.Clear();
            cboOperators.Items.Add("<All>");
            _operators = new List<string>();
            var query = "select Angajat from Angajati where IdSector = @IdSector and Mansione = @JobType";
            using (var con = new SqlConnection(MainWnd.conStringOY))
            {
                var cmd = new SqlCommand(query, con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.Add("@IdSector", SqlDbType.Int).Value = 7;
                cmd.Parameters.Add("@JobType", SqlDbType.NVarChar).Value = "OPERATORI";
                con.Open();
                var dr = cmd.ExecuteReader();

                if (dr.HasRows)
                    while (dr.Read())
                    {
                        _operators.Add(dr[0].ToString());
                    }
                con.Close();
                dr.Close();
            }
            if (_operators.Count <= 0)
                return;
            else
            {
                foreach (var o in _operators)
                    cboOperators.Items.Add(o);
            }
        }
        private void LoadOperators(DateTime currentDate, string operatorName)
        {
            _currentOperators = new List<LineOperator>();

            var cmd = new SqlCommand("GetLineOperators", MainWnd._sql_con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add("@CurrentDate", SqlDbType.DateTime).Value = currentDate;
            cmd.Parameters.Add("@shift", SqlDbType.VarChar).Value = MainWnd.Get_shift_array().ToString();
            if (operatorName == "<All>")
                operatorName = string.Empty;
            cmd.Parameters.Add("@operatorName", SqlDbType.VarChar).Value = operatorName;
            MainWnd._sql_con.Open();
            var dr = cmd.ExecuteReader();
            if(dr.HasRows)
                while(dr.Read())
                {
                    DateTime.TryParse(dr[2].ToString(), out var startDateTime);
                    var currOperator = new LineOperator()
                    {
                        FullName = dr[0].ToString(),
                        WorkingLine = "LINE " + dr[1].ToString().Remove(0, 5),
                        Date = startDateTime,                        
                        Shift = dr[3].ToString()
                    };
                    _currentOperators.Add(currOperator);
                }
            MainWnd._sql_con.Close();
            dr.Close();
            cmd = null;
        }

        #region Checkboxes part
        private void SetRowsVisibility()
        {
            //reset everything
            for (var r = 1; r <= dgvReport.Rows.Count - 1; r++)
            {
                for (var c = 5; c <= dgvReport.Columns.Count - 4; c++)
                {
                    if (string.IsNullOrEmpty(dgvReport.Rows[r].Cells[0].Value.ToString())) continue;
                    dgvReport.Rows[r].Cells[c].Style.ForeColor = Color.Black;
                    dgvReport.Rows[r].Cells[c].Style.SelectionForeColor = Color.White;
                    dgvReport.Rows[r].Cells[c].Style.SelectionBackColor = Color.FromArgb(0, 120, 215); //blue
                }
            }

            for (var r = 1; r <= dgvReport.Rows.Count - 1; r++)
            {
                for (var c = 5; c <= dgvReport.Columns.Count - 4; c++)
                {
                    if (string.IsNullOrEmpty(dgvReport.Rows[r].Cells[0].Value.ToString())) continue;

                    if (_mostraFilter)
                    {
                        //reset everything
                        dgvReport.Rows[r].Cells[c].Style.ForeColor = Color.Black;
                        dgvReport.Rows[r].Cells[c].Style.SelectionForeColor = Color.White;
                        dgvReport.Rows[r].Cells[c].Style.SelectionBackColor = Color.FromArgb(0, 120, 215); //blue
                    }
                    else if (_greenFilter && _yellowFilter)
                    {
                        DateTime.TryParse(dgvReport.Columns[c].Name, out var currDate);
                        decimal.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString(), out var eff);
                        if (eff >= 0.0M && eff < 85.0M)
                        {
                            if (currDate.DayOfWeek == DayOfWeek.Sunday)
                                dgvReport.Rows[r].Cells[c].Style.ForeColor = Color.Gainsboro;
                            else
                                dgvReport.Rows[r].Cells[c].Style.ForeColor = Color.FromArgb(255, 234, 232); //red
                            dgvReport.Rows[r].Cells[c].Style.SelectionForeColor = Color.FromArgb(0, 120, 215);
                            dgvReport.Rows[r].Cells[c].Style.SelectionBackColor = Color.FromArgb(0, 120, 215); //blue
                        }
                    }
                    else if (_greenFilter && _redFilter)
                    {
                        DateTime.TryParse(dgvReport.Columns[c].Name, out var currDate);
                        decimal.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString(), out var eff);
                        if (eff >= 85.0M && eff < 90.0M)
                        {
                            if (currDate.DayOfWeek == DayOfWeek.Sunday)
                                dgvReport.Rows[r].Cells[c].Style.ForeColor = Color.Gainsboro;
                            else
                                dgvReport.Rows[r].Cells[c].Style.ForeColor = Color.FromArgb(230, 230, 230); //silver
                            dgvReport.Rows[r].Cells[c].Style.SelectionForeColor = Color.FromArgb(0, 120, 215);
                            dgvReport.Rows[r].Cells[c].Style.SelectionBackColor = Color.FromArgb(0, 120, 215); //blue
                        }
                    }
                    else if (_yellowFilter && _redFilter)
                    {
                        DateTime.TryParse(dgvReport.Columns[c].Name, out var currDate);
                        decimal.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString(), out var eff);
                        if (eff >= 90.0M)
                        {
                            if (currDate.DayOfWeek == DayOfWeek.Sunday)
                                dgvReport.Rows[r].Cells[c].Style.ForeColor = Color.Gainsboro;
                            else
                                dgvReport.Rows[r].Cells[c].Style.ForeColor = Color.FromArgb(230, 230, 230); //silver
                            dgvReport.Rows[r].Cells[c].Style.SelectionForeColor = Color.FromArgb(0, 120, 215);
                            dgvReport.Rows[r].Cells[c].Style.SelectionBackColor = Color.FromArgb(0, 120, 215); //blue
                        }
                    }
                    else if (_greenFilter)
                    {
                        DateTime.TryParse(dgvReport.Columns[c].Name, out var currDate);
                        decimal.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString(), out var eff);
                        //hide red cells
                        if (eff >= 0.0M && eff < 85.0M)
                        {
                            if (currDate.DayOfWeek == DayOfWeek.Sunday)
                                dgvReport.Rows[r].Cells[c].Style.ForeColor = Color.Gainsboro;
                            else
                                dgvReport.Rows[r].Cells[c].Style.ForeColor = Color.FromArgb(255, 234, 232); //red
                            dgvReport.Rows[r].Cells[c].Style.SelectionForeColor = Color.FromArgb(0, 120, 215);
                            dgvReport.Rows[r].Cells[c].Style.SelectionBackColor = Color.FromArgb(0, 120, 215); //blue
                        }
                        //hide silver cells
                        else if (eff >= 85.0M && eff < 90.0M)
                        {
                            if (currDate.DayOfWeek == DayOfWeek.Sunday)
                                dgvReport.Rows[r].Cells[c].Style.ForeColor = Color.Gainsboro;
                            else
                                dgvReport.Rows[r].Cells[c].Style.ForeColor = Color.FromArgb(230, 230, 230); //silver
                            dgvReport.Rows[r].Cells[c].Style.SelectionForeColor = Color.FromArgb(0, 120, 215);
                            dgvReport.Rows[r].Cells[c].Style.SelectionBackColor = Color.FromArgb(0, 120, 215); //blue
                        }
                    }
                    else if (_yellowFilter)
                    {
                        DateTime.TryParse(dgvReport.Columns[c].Name, out var currDate);
                        decimal.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString(), out var eff);
                        //hide red cells
                        if (eff >= 0.0M && eff < 85.0M)
                        {
                            if (currDate.DayOfWeek == DayOfWeek.Sunday)
                                dgvReport.Rows[r].Cells[c].Style.ForeColor = Color.Gainsboro;
                            else
                                dgvReport.Rows[r].Cells[c].Style.ForeColor = Color.FromArgb(255, 234, 232); //red
                            dgvReport.Rows[r].Cells[c].Style.SelectionForeColor = Color.FromArgb(0, 120, 215);
                            dgvReport.Rows[r].Cells[c].Style.SelectionBackColor = Color.FromArgb(0, 120, 215); //blue
                        }
                        //hide silver cells
                        else if (eff > 90.0M)
                        {
                            if (currDate.DayOfWeek == DayOfWeek.Sunday)
                                dgvReport.Rows[r].Cells[c].Style.ForeColor = Color.Gainsboro;
                            else
                                dgvReport.Rows[r].Cells[c].Style.ForeColor = Color.FromArgb(230, 230, 230); //silver
                            dgvReport.Rows[r].Cells[c].Style.SelectionForeColor = Color.FromArgb(0, 120, 215);
                            dgvReport.Rows[r].Cells[c].Style.SelectionBackColor = Color.FromArgb(0, 120, 215); //blue
                        }
                    }
                    else if (_redFilter)
                    {
                        DateTime.TryParse(dgvReport.Columns[c].Name, out var currDate);
                        decimal.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString(), out var eff);
                        if (eff >= 85.0M)
                        {
                            if (currDate.DayOfWeek == DayOfWeek.Sunday)
                                dgvReport.Rows[r].Cells[c].Style.ForeColor = Color.Gainsboro;
                            else
                                dgvReport.Rows[r].Cells[c].Style.ForeColor = Color.FromArgb(230, 230, 230); //silver
                            dgvReport.Rows[r].Cells[c].Style.SelectionForeColor = Color.FromArgb(0, 120, 215);
                            dgvReport.Rows[r].Cells[c].Style.SelectionBackColor = Color.FromArgb(0, 120, 215); //blue
                        }
                    }
                }
            }
        }
        private bool _greenFilter = false;
        private bool _yellowFilter = false;
        private bool _redFilter = false;

        private void btnMediaTot_Click(object sender, EventArgs e)
        {
            isOperatorMode = false;
            _is_squadra_filter = false;
            _sorted = false;
            _from_machine = 1;
            _to_machine = 210;
            cboBlocks.Text = string.Empty;
            cboLines.Text = string.Empty;
            cboMedia.Text = string.Empty;
            cboOperators.Text = string.Empty;
            LoadReport();
            _reset = true;
        }
        private void cboMedia_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cb = sender as ComboBox;
            List<DataGridViewRow> rows = new List<DataGridViewRow>();
            btnMediaTot.PerformClick();
            if (cb.SelectedIndex == 0)
            {
                return;
            }
            else if (cb.SelectedIndex == 1)
            {                
                for (var r = 1; r < dgvReport.Rows.Count; r++)
                {
                    if (string.IsNullOrEmpty(dgvReport.Rows[r].Cells[0].Value.ToString()))
                        continue;
                    double.TryParse(dgvReport.Rows[r].Cells["Media"].Value.ToString(), out var eff);
                    if (eff < 90) rows.Add(dgvReport.Rows[r]);
                }
                foreach (DataGridViewRow row in rows)
                    dgvReport.Rows.Remove(row);
                dgvReport.Refresh();
            }
            else if (cb.SelectedIndex == 2)
            {
                for (var r = 1; r < dgvReport.Rows.Count; r++)
                {
                    if (string.IsNullOrEmpty(dgvReport.Rows[r].Cells[0].Value.ToString()))
                        continue;
                    double.TryParse(dgvReport.Rows[r].Cells[dgvReport.Columns.Count - 2].Value.ToString(), out var eff);
                    if (eff >0 && eff < 85 || eff >= 90) rows.Add(dgvReport.Rows[r]);
                }
                foreach (DataGridViewRow row in rows)
                    dgvReport.Rows.Remove(row);
                dgvReport.Refresh();
            }
            else if (cb.SelectedIndex == 3)
            {
                for (var r = 1; r < dgvReport.Rows.Count; r++)
                {
                    if (string.IsNullOrEmpty(dgvReport.Rows[r].Cells[0].Value.ToString()))
                        continue;
                    double.TryParse(dgvReport.Rows[r].Cells[dgvReport.Columns.Count - 2].Value.ToString(), out var eff);
                    if (eff >= 85) rows.Add(dgvReport.Rows[r]);
                }
                foreach (DataGridViewRow row in rows)
                    dgvReport.Rows.Remove(row);
                dgvReport.Refresh();
            }
        }

        private void btnOperatorsTot_Click(object sender, EventArgs e)
        {
            isOperatorMode = true;
            _is_squadra_filter = false;
            _sorted = false;
            _from_machine = 1;
            _to_machine = 210;
            cboBlocks.Text = string.Empty;
            cboLines.Text = string.Empty;
            cboMedia.Text = string.Empty;
            cboOperators.Text = string.Empty;
            LoadReport();
            _reset = true;
        }

        private void cboOperators_SelectedIndexChanged(object sender, EventArgs e)
        {
            isOperatorMode = true;
            _is_squadra_filter = false;
            _sorted = false;
            _from_machine = 1;
            _to_machine = 210;
            cboBlocks.Text = string.Empty;
            cboLines.Text = string.Empty;
            cboMedia.Text = string.Empty;
            LoadReport();
            _reset = true;
        }

        private bool _mostraFilter = false;
        private void cbColorFilter_CheckChanged(object sender, EventArgs e)
        {
            var cb = sender as CheckBox;

            if (cb.Checked)
            {
                if (cb.Name == "cbMostra")
                {
                    _mostraFilter = true;
                    _greenFilter = false;
                    _yellowFilter = false;
                    _redFilter = false;
                    cbGreen.Checked = false;
                    cbYellow.Checked = false;
                    cbRed.Checked = false;
                }
                else
                {
                    _mostraFilter = false;
                    cbMostra.Checked = false;
                    if (cb.Name == "cbGreen")
                    {
                        _greenFilter = true;                        
                    }
                    else if (cb.Name == "cbYellow")
                    {
                        _yellowFilter = true;
                    }
                    else if (cb.Name == "cbRed")
                    {
                        _redFilter = true;
                    }
                }
            }
            else
            {
                if (cb.Name == "cbGreen")
                {
                    _greenFilter = false;
                }
                else if (cb.Name == "cbYellow")
                {
                    _yellowFilter = false;
                }
                else if (cb.Name == "cbRed")
                {
                    _redFilter = false;
                }
                else if (cb.Name == "cbMostra")
                {
                    _mostraFilter = false;
                }
            }

            if(_greenFilter && _yellowFilter && _redFilter)
            {
                cbMostra.Checked = true;
                cbGreen.Checked = false;
                cbYellow.Checked = false;
                cbRed.Checked = false;
                _greenFilter = false;
                _yellowFilter = false;
                _redFilter = false;
                _mostraFilter = true;
            }
            SetRowsVisibility();
        }
        private void CheckboxStyle_Paint(object sender, PaintEventArgs e)
        {
            CheckBox rb = sender as CheckBox;
            var rect = rb.ClientRectangle;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var targetedColor = default(Color);

            if (rb.Name == "cbGreen")
                targetedColor = Color.FromArgb(54, 214, 87);
            else if (rb.Name == "cbYellow")
                targetedColor = Color.FromArgb(254, 215, 1);
            else if (rb.Name == "cbRed")
                targetedColor = Color.FromArgb(253, 129, 127);
            else
                targetedColor = Color.LightGray;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.FillRectangle(new SolidBrush(rb.BackColor)
                                     ,new RectangleF(rect.X - 1, rect.Y, rect.Width, rect.Height));
            e.Graphics.DrawEllipse(new Pen(new SolidBrush(targetedColor), 2)
                                           ,new RectangleF(rect.X + 1, rect.Y + 1, rect.Width - 4, rect.Height - 4));
            if (rb.Checked)
                e.Graphics.FillEllipse(new SolidBrush(targetedColor)
                                       ,new RectangleF(rect.Width / 2 - 5, rect.Height / 2 - 5, 8, 8));
            else
                e.Graphics.FillEllipse(new SolidBrush(rb.BackColor)
                                       ,new RectangleF(rect.Width / 2 - 5, rect.Height / 2 - 5, 8, 8));
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
        }
        #endregion Checkboxes part
    }
    public class LineOperator
    {
        public string FullName { get; set; }

        public string WorkingLine { get; set; }

        public double Efficiency { get; set; }

        public DateTime Date { get; set; }

        public string Shift { get; set; }
    }
}