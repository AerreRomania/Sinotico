using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.Linq;
using System.Linq;

namespace Sinotico
    {
    public partial class RepEffMachines : Form
    {
        private List<string> _colors = new List<string>();
        private AutoCompleteStringCollection _acsc;
        private DataTable _table_report = new DataTable();
        private Font _up_title_font = new Font("Tahoma", 12, FontStyle.Regular);
        private Font _down_title_font = new Font("Tahoma", 7, FontStyle.Regular);
        private string _file_name;
        private string _fin;
        private string _color;
        private BindingSource _bsRep = new BindingSource();
        private StringBuilder _machines_array = new StringBuilder();
        private List<TurnoScartiRammendi> _mac_scarti_rammendi = new List<TurnoScartiRammendi>();
        private string _current_filter = string.Empty;

        public RepEffMachines()
        {
            InitializeComponent();
            dgvReport.DoubleBufferedDataGridView(true);
        }

        private DateTime Get_from_date()
        {
            return new DateTime(dtpFrom.Value.Year, dtpFrom.Value.Month, dtpFrom.Value.Day);
        }
        private DateTime Get_to_date()
        {
            return new DateTime(dtpTo.Value.Year, dtpTo.Value.Month, dtpTo.Value.Day);
        }

        private void RepEffMachines_Load(object sender, EventArgs e)
            {
            LoadingInfo.CloseLoading();

            dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            dtpTo.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            cbMedia.Items.Clear();
            cbMedia.Items.Add("<All>");
            cbMedia.Items.Add("Green range");
            cbMedia.Items.Add("Yellow range");
            cbMedia.Items.Add("Red range");

            _current_filter = "[Machine Registration No.]";

            _file_name = string.Empty;
            _fin = string.Empty;
            _color = string.Empty;

            dgvReport.AllowUserToAddRows = false;
            dgvReport.AllowUserToDeleteRows = false;
            dgvReport.AllowUserToResizeColumns = false;
            dgvReport.AllowUserToResizeRows = false;
            dgvReport.AllowUserToOrderColumns = false;
            dgvReport.ReadOnly = true;

            cboArt.DropDownHeight = 150;
            cboArt.DropDownWidth = 200;

            for (var i = 1; i <= 210; i++)
                {
                cboMac.Items.Add(i.ToString());
                }

            foreach (KeyValuePair<string, string> kvp in MainWnd._fileNamesDict)
                {
                if (kvp.Key == "<Reset>") continue;

                //TODO
                //insert splitted values (file name)
                cboArt.Items.Add(kvp.Value);    
                }

            //CollectAvailableColors();           

            _machines_array = new StringBuilder();
            _machines_array.Append(",");

            for (var i = 1; i <= 210; i++)
                {
                _machines_array.Append(i.ToString() + ",");
                }

            var startFilter = "[Machine Registration No.]";
            LoadReport(startFilter);

            dgvReport.DataBindingComplete += delegate
                {
                    if (_txtFilter == null)
                        {
                        dgvReport.Focus();
                        }

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

                    foreach (DataGridViewColumn col in dgvReport.Columns)
                        col.SortMode = DataGridViewColumnSortMode.NotSortable;

                    // specialize filter column
                    dgvReport.Columns[0].Width = 200;
                    dgvReport.Columns[0].DefaultCellStyle.BackColor = Color.FromArgb(205, 205, 205);
                    dgvReport.Columns[0].HeaderCell.Style.Font = new Font("Tahoma", 10, FontStyle.Regular);
                    dgvReport.Columns[0].HeaderCell.Style.ForeColor = Color.MidnightBlue;
                    dgvReport.Columns[0].HeaderCell.Style.BackColor = Color.White;
                    dgvReport.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvReport.Columns[0].HeaderCell.Style.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;

                    //sep column
                    dgvReport.Columns[16].Width = 10;
                    dgvReport.Columns[16].HeaderText = string.Empty;
                    dgvReport.Columns[16].DefaultCellStyle.BackColor = Color.White;
                    dgvReport.Columns[16].DefaultCellStyle.SelectionBackColor = Color.White;
                    dgvReport.Columns[16].DefaultCellStyle.SelectionForeColor = Color.White;
                    //end sep column

                    // other columns
                    for (var c = 1; c <= dgvReport.Columns.Count - 1; c++)
                        {
                        if (c == 16)
                            continue;
                        dgvReport.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 219, 94);
                        dgvReport.Columns[c].Width = 60;
                        dgvReport.Columns[c].HeaderCell.Style.Font = new Font("Tahoma", 7);
                        dgvReport.Columns[c].HeaderCell.Style.Alignment =
                        DataGridViewContentAlignment.BottomLeft;
                        dgvReport.Columns[c].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        
                        if(c >= 1 && c <= 5 || c >= 11 && c <= 15) dgvReport.Columns[c].DefaultCellStyle.BackColor = Color.FromArgb(232, 232, 232);
                        else if(c >= 6 && c <=10 || c >= 16 && c <= 20) dgvReport.Columns[c].DefaultCellStyle.BackColor = Color.FromArgb(221, 221, 221);
                    }

                    for (var h = 26; h <= dgvReport.ColumnCount - 2; h += 2)
                        {
                        dgvReport.Columns[h].HeaderText = Environment.NewLine + Environment.NewLine + Environment.NewLine + "tempo";
                        }
                    for (var h = 27; h <= dgvReport.Columns.Count - 1; h += 2)
                        {
                        dgvReport.Columns[h].HeaderText = Environment.NewLine + Environment.NewLine + Environment.NewLine + "%";
                        dgvReport.Columns[h].DefaultCellStyle.BackColor = Color.Silver;
                        }

                    dgvReport.Columns[0].Frozen = true;
                    foreach (DataGridViewRow row in dgvReport.Rows)
                    {
                        if (row.Index == 0) continue;
                        foreach (DataGridViewColumn col in dgvReport.Columns)
                        {
                            if (col.Index == 1 || col.Index == 6 || col.Index == 11)
                            {
                                decimal.TryParse(row.Cells[col.Index].Value.ToString(), out var eff);
                                var color = GetEfficiencyColor(eff);
                                row.Cells[col.Index].Style.ForeColor = color;
                                if(color == Color.Red)                               
                                    row.Cells[col.Index].Style.Font = new Font("Tahoma", 8, FontStyle.Bold);                                
                                else row.Cells[col.Index].Style.Font = new Font("Tahoma", 8, FontStyle.Regular);

                            }
                            else if(col.Index == 17)
                            {
                                decimal.TryParse(row.Cells[col.Index].Value.ToString().Trim('%'), out var eff);
                                var color = GetEfficiencyColor(eff);
                                row.Cells[col.Index].Style.ForeColor = color;
                                if (color == Color.Red)
                                    row.Cells[col.Index].Style.Font = new Font("Tahoma", 8, FontStyle.Bold);
                                else row.Cells[col.Index].Style.Font = new Font("Tahoma", 8, FontStyle.Regular);
                            }
                        }
                    } 
                };
            }
        
        private void CreateColumns()
            {
            _table_report = new DataTable();

            var columns = new string[]
                {
                    "Macchina",
                    "Efficienza",
                    "Tempo tessitura",
                    "Nr teli",
                    "% scarti",
                    "% rammendo",
                    "Efficienza1",
                    "Tempo tessitura1",
                    "Nr teli1",
                    "% scarti1",
                    "% rammendo1",
                    "Efficienza2",
                    "Tempo tessitura2",
                    "Nr teli2",
                    "% scarti2",
                    "% rammendo2",
                    "sep1",
                    "Efficienza3",
                    "Tempo tessitura3",
                    "Nr teli3",
                    "% scarti3",
                    "% rammendo3",
                    "Ore apertura",
                    "Ore Utilizzo",
                    "Ore Efficienza",
                    "Ore Qualità",      //--------24
                    "tempo1", "%1",     //-------25
                    "tempo2", "%2",
                    "tempo3", "%3",
                    "tempo4", "%4",
                    "tempo5", "%5",
                    "tempo6", "%6",
                    "tempo7", "%7",
                    "tempo8", "%8",
                    "tempo9", "%9",
                    "tempo10", "%10",
                    "tempo11", "%11",
                    "tempo12", "%12",
                    "tempo13", "%13",
                    "tempo14", "%14",
                    "tempo15", "%15",
                    "tempo16", "%16",
                    "tempo17", "%17",
                    "tempo18", "%18",
                    "tempo19", "%19",
                    "tempo20", "%20",
                    "tempo21", "%21",
                    "tempo22", "%22",
                    "tempo23", "%23",
                    "tempo24", "%24",
                    "tempo25", "%25"  //-----------74
                    };

            for (var c = 0; c < columns.Length; c++)
                {
                _table_report.Columns.AddRange(new DataColumn[] { new DataColumn(columns[c], typeof(string)) });
                }
            _table_report.Columns.Add("sorts", typeof(double));
            }

        private void LoadReport(object filter)
        {
            LoadingInfo.InfoText = "Loading report...";
            LoadingInfo.ShowLoading();

            if (_txtFilter != null) dgvReport.Controls.Remove(_txtFilter);

                if (dgvReport.DataSource != null)
                    dgvReport.DataSource = null;

                CreateColumns();
                
                DataSet ds = new DataSet();
                var cmd = new SqlCommand("get_data_in_eff", MainWnd._sql_con)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 300
                };

                cmd.Parameters.Add("@from_date", SqlDbType.DateTime).Value = Get_from_date();
                cmd.Parameters.Add("@to_date", SqlDbType.DateTime).Value = Get_to_date();
                cmd.Parameters.Add("@machine", SqlDbType.VarChar).Value = _machines_array.ToString();
                cmd.Parameters.Add("@filter", SqlDbType.VarChar).Value = filter.ToString();
                cmd.Parameters.Add("@file_name", SqlDbType.VarChar).Value = _file_name;
                cmd.Parameters.Add("@finesse", SqlDbType.VarChar).Value = _fin;
                cmd.Parameters.Add("@table", SqlDbType.VarChar).Value = MainWnd.GetTableSource();

                MainWnd._sql_con.Open();
                var da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                MainWnd._sql_con.Close();
                //dr.Close();
                cmd = null;

            if(ds.Tables[0].Rows.Count <= 0 || ds.Tables[1].Rows.Count <= 0)
            {
                LoadingInfo.CloseLoading();
                MessageBox.Show("No data!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

                var keys = new List<string>();

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (!keys.Contains(row[0].ToString()))
                        keys.Add(row[0].ToString());
                }

                DataRow[] rowNight = ds.Tables[0].Select("Shift = 'NIGHT'");
                DataRow[] rowMorning = ds.Tables[0].Select("Shift = 'MORNING'");
                DataRow[] rowAfternoon = ds.Tables[0].Select("Shift = 'AFTERNOON'");

                _acsc = new AutoCompleteStringCollection();

                _colors = new List<string>();

                _table_report.Rows.Add("TOTALI PERIODO");

                foreach (var key in keys)
                {
                    var newRow = _table_report.NewRow();
                    var tmpKey = key;

                    if (filter.ToString() == "[Machine Registration No.]")
                    {
                        tmpKey = tmpKey.PadLeft(3, '0');
                    }

                    if (filter.ToString() == "[File Name]")
                    {
                        _acsc.AddRange(new[] { key });
                    }

                    newRow[0] = tmpKey;

                    foreach (var row in rowNight)
                    {
                        if (key != row[0].ToString()) continue;
                        newRow[1] = row[2].ToString();
                        newRow[2] = ConvertTime(row[3]);
                        newRow[3] = row[4].ToString();
                        if (filter.ToString() != "Gauge" && filter.ToString() != "[File Name]")
                        {
                            int.TryParse(row[0].ToString(), out var mac);
                            if (!_mac_scarti_rammendi.Exists(m => m.Machine == mac))
                            {
                                var newItem = new TurnoScartiRammendi(mac);
                                int.TryParse(row[4].ToString(), out var qty);
                                newItem.Turno1Qty = qty;
                                _mac_scarti_rammendi.Add(newItem);
                            }
                        }
                    }
                    foreach (var r in rowMorning)
                    {
                        if (key != r[0].ToString()) continue;
                        newRow[6] = r[2].ToString();
                        newRow[7] = ConvertTime(r[3]);
                        newRow[8] = r[4].ToString();
                        if (filter.ToString() != "Gauge" && filter.ToString() != "[File Name]")
                        {
                            int.TryParse(r[0].ToString(), out var mac);
                            int.TryParse(r[4].ToString(), out var qty);
                            if (!_mac_scarti_rammendi.Exists(m => m.Machine == mac))
                            {
                                var newItem = new TurnoScartiRammendi(mac);
                                newItem.Turno2Qty = qty;
                                _mac_scarti_rammendi.Add(newItem);
                            }
                            else
                            {
                                var item = (from it in _mac_scarti_rammendi
                                            where it.Machine == mac
                                            select it).SingleOrDefault();
                                item.Turno2Qty = qty;
                            }
                        }
                    }
                    foreach (var r in rowAfternoon)
                    {
                        if (key != r[0].ToString()) continue;
                        newRow[11] = r[2].ToString();
                        newRow[12] = ConvertTime(r[3]);
                        newRow[13] = r[4].ToString();
                        if (filter.ToString() != "Gauge" && filter.ToString() != "[File Name]")
                        {
                            int.TryParse(r[0].ToString(), out var mac);
                            int.TryParse(r[4].ToString(), out var qty);
                            if (!_mac_scarti_rammendi.Exists(m => m.Machine == mac))
                            {
                                var newItem = new TurnoScartiRammendi(mac);
                                newItem.Turno3Qty = qty;
                                _mac_scarti_rammendi.Add(newItem);
                            }
                            else
                            {
                                var item = (from it in _mac_scarti_rammendi
                                            where it.Machine == mac
                                            select it).SingleOrDefault();
                                item.Turno3Qty = qty;
                            }
                        }
                    }

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (key != row[0].ToString()) continue;

                        newRow[26] = ConvertSecondsToHHmm(Convert.ToInt32(row[5]));
                        newRow[27] = GetPercentage(row[5]);
                        newRow[28] = ConvertSecondsToHHmm(Convert.ToInt32(row[6]));
                        newRow[29] = GetPercentage(row[6]);
                        newRow[30] = ConvertSecondsToHHmm(Convert.ToInt32(row[7]));
                        newRow[31] = GetPercentage(row[7]);
                        newRow[32] = ConvertSecondsToHHmm(Convert.ToInt32(row[8]));
                        newRow[33] = GetPercentage(row[8]);
                        newRow[34] = ConvertSecondsToHHmm(Convert.ToInt32(row[9]));
                        newRow[35] = GetPercentage(row[9]);
                        newRow[36] = ConvertSecondsToHHmm(Convert.ToInt32(row[10]));
                        newRow[37] = GetPercentage(row[10]);
                        newRow[38] = ConvertSecondsToHHmm(Convert.ToInt32(row[11]));
                        newRow[39] = GetPercentage(row[11]);

                        newRow[40] = ConvertSecondsToHHmm(Convert.ToInt32(row[12]));
                        newRow[41] = GetPercentage(row[12]);
                        newRow[42] = ConvertSecondsToHHmm(Convert.ToInt32(row[13]));
                        newRow[43] = GetPercentage(row[13]);
                        newRow[44] = ConvertSecondsToHHmm(Convert.ToInt32(row[14]));
                        newRow[45] = GetPercentage(row[14]);
                        newRow[46] = ConvertSecondsToHHmm(Convert.ToInt32(row[15]));
                        newRow[47] = GetPercentage(row[15]);
                        newRow[48] = ConvertSecondsToHHmm(Convert.ToInt32(row[16]));
                        newRow[49] = GetPercentage(row[16]);
                        newRow[50] = ConvertSecondsToHHmm(Convert.ToInt32(row[17]));
                        newRow[51] = GetPercentage(row[17]);
                        newRow[52] = ConvertSecondsToHHmm(Convert.ToInt32(row[18]));
                        newRow[53] = GetPercentage(row[18]);
                        newRow[54] = ConvertSecondsToHHmm(Convert.ToInt32(row[19]));
                        newRow[55] = GetPercentage(row[19]);
                        newRow[56] = ConvertSecondsToHHmm(Convert.ToInt32(row[20]));
                        newRow[57] = GetPercentage(row[20]);
                        newRow[58] = ConvertSecondsToHHmm(Convert.ToInt32(row[21]));
                        newRow[59] = GetPercentage(row[21]);
                        newRow[60] = ConvertSecondsToHHmm(Convert.ToInt32(row[22]));
                        newRow[61] = GetPercentage(row[22]);
                        newRow[62] = ConvertSecondsToHHmm(Convert.ToInt32(row[23]));
                        newRow[63] = GetPercentage(row[23]);
                        newRow[64] = ConvertSecondsToHHmm(Convert.ToInt32(row[24]));
                        newRow[65] = GetPercentage(row[24]);
                        newRow[66] = ConvertSecondsToHHmm(Convert.ToInt32(row[25]));
                        newRow[67] = GetPercentage(row[25]);
                        newRow[68] = ConvertSecondsToHHmm(Convert.ToInt32(row[26]));
                        newRow[69] = GetPercentage(row[26]);
                        newRow[70] = ConvertSecondsToHHmm(Convert.ToInt32(row[27]));
                        newRow[71] = GetPercentage(row[27]);
                        newRow[72] = ConvertSecondsToHHmm(Convert.ToInt32(row[28]));
                        newRow[73] = GetPercentage(row[28]);
                        newRow[74] = ConvertSecondsToHHmm(Convert.ToInt32(row[29]));
                        newRow[75] = GetPercentage(row[29]);
                    }
                    _table_report.Rows.Add(newRow);
                }

                //calculating scarti/rammendi
                if (filter.ToString() != "Gauge" && filter.ToString() != "[File Name]")
                {
                    var shift = ds.Tables[1].Rows[0][0].ToString();
                    var operator_code = ds.Tables[1].Rows[0][1].ToString();
                    int.TryParse(ds.Tables[1].Rows[0][2].ToString(), out var machine);
                    var order = ds.Tables[1].Rows[0][3].ToString();
                    var article = ds.Tables[1].Rows[0][4].ToString();
                    int.TryParse(ds.Tables[1].Rows[0][5].ToString(), out var tmpScarti);
                    int.TryParse(ds.Tables[1].Rows[0][6].ToString(), out var tmpRammendi);
                    var sumScarti = 0;
                    var sumRammendi = 0;
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        var sh = row[0].ToString();
                        var operCode = row[1].ToString();
                        int.TryParse(row[2].ToString(), out var mac);
                        var ord = row[3].ToString();
                        var art = row[4].ToString();
                        int.TryParse(row[5].ToString(), out var scarti);
                        int.TryParse(row[6].ToString(), out var ramm);
                        if (machine != mac || shift != sh)
                        {
                            sumScarti += tmpScarti;
                            sumRammendi += tmpRammendi;

                            var rec = (from r in _mac_scarti_rammendi
                                       where r.Machine == machine
                                       select r).SingleOrDefault();
                        if (rec == null) continue;
                            if (shift == "NIGHT")
                            {
                                rec.Turno1Scarti = sumScarti;
                                rec.Turno1Rammendi = sumRammendi;
                            }
                            else if (shift == "MORNING")
                            {
                                rec.Turno2Scarti = sumScarti;
                                rec.Turno2Rammendi = sumRammendi;
                            }
                            else if (shift == "AFTERNOON")
                            {
                                rec.Turno3Scarti = sumScarti;
                                rec.Turno3Rammendi = sumRammendi;
                            }

                            sumScarti = 0;
                            sumRammendi = 0;
                            shift = sh;
                            operator_code = operCode;
                            order = ord;
                            article = art;
                        }
                        if (order != ord || operator_code != operCode || article != art)
                        {
                            sumScarti += tmpScarti;
                            sumRammendi += tmpRammendi;
                        }
                        machine = mac;
                        shift = sh;
                        operator_code = operCode;
                        order = ord;
                        article = art;
                        tmpRammendi = ramm;
                        tmpScarti = scarti;
                    }
                    sumScarti += tmpScarti;
                    sumRammendi += tmpRammendi;

                    var lastRec = (from r in _mac_scarti_rammendi
                                   where r.Machine == machine
                                   select r).SingleOrDefault();
                if (lastRec != null)
                {
                    if (shift == "NIGHT")
                    {
                        lastRec.Turno1Scarti = sumScarti;
                        lastRec.Turno1Rammendi = sumRammendi;
                    }
                    else if (shift == "MORNING")
                    {
                        lastRec.Turno2Scarti = sumScarti;
                        lastRec.Turno2Rammendi = sumRammendi;
                    }
                    else if (shift == "AFTERNOON")
                    {
                        lastRec.Turno3Scarti = sumScarti;
                        lastRec.Turno3Rammendi = sumRammendi;
                    }
                }
                }

                _bsRep.DataSource = _table_report;
                dgvReport.DataSource = _bsRep;

                //insert scarti/rammendi into datagridview
                if (filter.ToString() != "Gauge" && filter.ToString() != "[File Name]")
                {
                    foreach (DataGridViewRow dgvRow in dgvReport.Rows)
                    {
                        if (dgvRow.Index == 0) continue;
                        int.TryParse(dgvRow.Cells[0].Value.ToString(), out var mac);
                        var rec = (from r in _mac_scarti_rammendi
                                   where r.Machine == mac
                                   select r).SingleOrDefault();

                        double.TryParse(rec.Turno1Scarti.ToString(), out var t1s);
                        double.TryParse(rec.Turno2Scarti.ToString(), out var t2s);
                        double.TryParse(rec.Turno3Scarti.ToString(), out var t3s);

                        double.TryParse(rec.Turno1Rammendi.ToString(), out var t1r);
                        double.TryParse(rec.Turno2Rammendi.ToString(), out var t2r);
                        double.TryParse(rec.Turno3Rammendi.ToString(), out var t3r);

                        double.TryParse(rec.Turno1Qty.ToString(), out var t1q);
                        double.TryParse(rec.Turno2Qty.ToString(), out var t2q);
                        double.TryParse(rec.Turno3Qty.ToString(), out var t3q);

                        if (t1q == 0)
                        {
                            dgvRow.Cells[4].Value = string.Empty;
                            dgvRow.Cells[5].Value = string.Empty;
                        }
                        else
                        {
                            dgvRow.Cells[4].Value = Math.Round(((t1s / t1q) * 100), 1).ToString();
                            dgvRow.Cells[5].Value = Math.Round(((t1r / t1q) * 100), 1).ToString();
                        }

                        if (t2q == 0)
                        {
                            dgvRow.Cells[9].Value = string.Empty;
                            dgvRow.Cells[10].Value = string.Empty;
                        }
                        else
                        {
                            dgvRow.Cells[9].Value = Math.Round(((t2s / t2q) * 100), 1).ToString();
                            dgvRow.Cells[10].Value = Math.Round(((t2r / t2q) * 100), 1).ToString();
                        }

                        if (t3q == 0)
                        {
                            dgvRow.Cells[14].Value = string.Empty;
                            dgvRow.Cells[15].Value = string.Empty;
                        }
                        else
                        {
                            dgvRow.Cells[14].Value = Math.Round(((t3s / t3q) * 100), 1).ToString();
                            dgvRow.Cells[15].Value = Math.Round(((t3r / t3q) * 100), 1).ToString();
                        }
                    }
                }
                
                dgvReport.Columns[dgvReport.Columns.Count - 1].Visible = false;
                foreach (DataGridViewColumn col in dgvReport.Columns)
                {
                    if (col.Index == 1 || col.Index == 6 || col.Index == 11 || col.Index == 17)
                        col.HeaderText = "Efficienza";
                    else if (col.Index == 2 || col.Index == 7 || col.Index == 12 || col.Index == 18)
                        col.HeaderText = "Tempo tessitura";
                    else if (col.Index == 3 || col.Index == 8 || col.Index == 13 || col.Index == 19)
                        col.HeaderText = "Nr teli";
                    else if (col.Index == 4 || col.Index == 9 || col.Index == 14 || col.Index == 20)
                        col.HeaderText = "% scarti";
                    else if (col.Index == 5 || col.Index == 10 || col.Index == 15 || col.Index == 21)
                        col.HeaderText = "% rammendo";
                }
                CalculateTotalsVertical(filter.ToString());
                CalculateTotalsHorizontal(filter.ToString());

            for (var c = 26; c <= dgvReport.Columns.Count - 2; c += 2)
            {
                for (var r = 1; r <= dgvReport.Rows.Count - 1; r++)
                {
                    if (string.IsNullOrEmpty(dgvReport.Rows[r].Cells[18].Value.ToString()) ||
                        dgvReport.Rows[r].Cells[18].Value.ToString().Contains(":")) continue;
                    double.TryParse(dgvReport.Rows[r].Cells[18].Value.ToString().Split(':')[0], out var totHours);
                    double.TryParse(dgvReport.Rows[r].Cells[18].Value.ToString().Split(':')[1], out var totMinutes);
                    var totSeconds = totMinutes * 60 + totHours * 3600;

                    double.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString().Split(':')[0], out var hours);
                    double.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString().Split(':')[1], out var minutes);
                    var seconds = minutes * 60 + hours * 3600;

                    if (totSeconds == 0)
                        dgvReport.Rows[r].Cells[c + 1].Value = "0.0";
                    else
                        dgvReport.Rows[r].Cells[c + 1].Value = Math.Round((seconds / totSeconds) * 100, 1);
                }
            }
            for (var c = 26; c <= dgvReport.Columns.Count - 2; c += 2)
            {
                double.TryParse(dgvReport.Rows[0].Cells[18].Value.ToString().Split(':')[0], out var totHours);
                double.TryParse(dgvReport.Rows[0].Cells[18].Value.ToString().Split(':')[1], out var totMinutes);
                var totSeconds = totMinutes * 60 + totHours * 3600;

                double.TryParse(dgvReport.Rows[0].Cells[c].Value.ToString().Split(':')[0], out var hours);
                double.TryParse(dgvReport.Rows[0].Cells[c].Value.ToString().Split(':')[1], out var minutes);
                var seconds = minutes * 60 + hours * 3600;

                if (totSeconds == 0)
                    dgvReport.Rows[0].Cells[c + 1].Value = "0.0";
                else
                    dgvReport.Rows[0].Cells[c + 1].Value = Math.Round((seconds / totSeconds) * 100, 1);
            }
                filter = null;

                SetColumnVisibilityByTotalValue();

                LoadingInfo.CloseLoading();

                //LoadingInfo.CloseLoading();
               // MessageBox.Show("No data!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }
        private static Color GetEfficiencyColor(decimal eff)
        {
            var color = default(Color);
            if
                (eff > 0 && eff <= 85.0M) color = Color.Red;
            else if
                (eff > 85.0M && eff <= 90.0M) color = Color.DarkOrange;
            else if
                (eff > 90.0M) color = Color.Green;
            return color;
        }
        private double GetPercentage(object n)
            {
            if (n == null) return 0;

            return Math.Round(Convert.ToDouble(((Convert.ToDouble(n) / 60) * 100) / 1440), 1);
            }

        private string ConvertSecondsToHHmm(int seconds)
            {
            int hours = seconds / 3600;
            int mins = (seconds % 3600) / 60;
            return string.Format("{0:D2}:{1:D2}", hours, mins);
            }

        private void SetColumnVisibilityByTotalValue()
            {
            //sets all columns to be visible
            for (var i = 26; i <= dgvReport.ColumnCount - 2; i++)
                {
                dgvReport.Columns[i].Visible = true;
                }

                for (var i = 27; i <= dgvReport.Columns.Count - 1; i += 2)
            
                    //searching through eff columns as double type
                {
                double sum = 0;
                for (var r = 1; r <= dgvReport.Rows.Count - 1; r++)
                    {
                    double.TryParse(dgvReport.Rows[r].Cells[i].Value.ToString(), out var eff);

                    sum += eff;
                    }

                if (sum <= 0)
                    {
                    dgvReport.Columns[i - 1].Visible = false;
                    dgvReport.Columns[i].Visible = false;
                    }
                }

            dgvReport.Refresh();
            }

        #region Totals

        private void CalculateTotalsVertical(string filter)
            {
            //horizontal and vertical calculator
            //var count = 0;
            var eff1Divider = 0;
            var eff2Divider = 0;
            var eff3Divider = 0;
            var eff1 = 0.0;
            var eff2 = 0.0;
            var eff3 = 0.0;
            var qty1 = 0;
            var qty2 = 0;
            var qty3 = 0;
            var tm1 = "";
            var tm2 = "";
            var tm3 = "";
            double scarti1 = 0.0;
            double scarti2 = 0.0;
            double scarti3 = 0.0;
            double rammendi1 = 0.0;
            double rammendi2 = 0.0;
            double rammendi3 = 0.0;

            var main = new MainWnd();
            for (var r = 1; r <= dgvReport.Rows.Count - 1; r++)
                {
                if (!string.IsNullOrEmpty(dgvReport.Rows[r].Cells[1].Value.ToString())) eff1Divider++;
                double.TryParse(dgvReport.Rows[r].Cells[1].Value.ToString(), out double e1);
                eff1 += e1;
                if (!string.IsNullOrEmpty(dgvReport.Rows[r].Cells[6].Value.ToString())) eff2Divider++;
                double.TryParse(dgvReport.Rows[r].Cells[6].Value.ToString(), out double e2);
                eff2 += e2;
                if (!string.IsNullOrEmpty(dgvReport.Rows[r].Cells[11].Value.ToString())) eff3Divider++;
                double.TryParse(dgvReport.Rows[r].Cells[11].Value.ToString(), out double e3);
                eff3 += e3;

                if (dgvReport.Rows[r].Cells[2].Value.ToString().Contains(":"))
                    {
                    int.TryParse(dgvReport.Rows[r].Cells[2].Value.ToString().Split(':')[0], out int t1);
                    int.TryParse(dgvReport.Rows[r].Cells[2].Value.ToString().Split(':')[1], out int m1);
                    tm1 = main.CumulateHHmm(t1, m1);
                    }

                int.TryParse(dgvReport.Rows[r].Cells[3].Value.ToString(), out int q1);
                qty1 += q1;
                int.TryParse(dgvReport.Rows[r].Cells[8].Value.ToString(), out int q2);
                qty2 += q2;
                int.TryParse(dgvReport.Rows[r].Cells[13].Value.ToString(), out int q3);
                qty3 += q3;

                double.TryParse(dgvReport.Rows[r].Cells[4].Value.ToString(), out var s1);
                scarti1 += s1;
                double.TryParse(dgvReport.Rows[r].Cells[9].Value.ToString(), out var s2);
                scarti2 += s2;
                double.TryParse(dgvReport.Rows[r].Cells[14].Value.ToString(), out var s3);
                scarti3 += s3;

                double.TryParse(dgvReport.Rows[r].Cells[5].Value.ToString(), out var r1);
                rammendi1 += r1;
                double.TryParse(dgvReport.Rows[r].Cells[10].Value.ToString(), out var r2);
                rammendi2 += r2;
                double.TryParse(dgvReport.Rows[r].Cells[15].Value.ToString(), out var r3);
                rammendi3 += r3;
            }

            main._hours = 0;
            main._minutes = 0;
            for (var r = 1; r <= dgvReport.Rows.Count - 1; r++)
                {
                if (dgvReport.Rows[r].Cells[7].Value.ToString().Contains(":"))
                    {
                    int.TryParse(dgvReport.Rows[r].Cells[7].Value.ToString().Split(':')[0], out int t2);
                    int.TryParse(dgvReport.Rows[r].Cells[7].Value.ToString().Split(':')[1], out int m2);
                    tm2 = main.CumulateHHmm(t2, m2);
                    }
                }

            main._hours = 0;
            main._minutes = 0;
            for (var r = 1; r <= dgvReport.Rows.Count - 1; r++)
                {
                if (dgvReport.Rows[r].Cells[12].Value.ToString().Contains(":"))
                    {
                    int.TryParse(dgvReport.Rows[r].Cells[12].Value.ToString().Split(':')[0], out int t3);
                    int.TryParse(dgvReport.Rows[r].Cells[12].Value.ToString().Split(':')[1], out int m3);
                    tm3 = main.CumulateHHmm(t3, m3);
                    }
                }
            var _finEff1Divider = 0;
            var _finEff2Divider = 0;
            var _finEff3Divider = 0;
            if(filter == "[Machine Registration No.]")
            {
                if (string.IsNullOrEmpty(cbSquadra.Text))
                {
                    _finEff1Divider = 210;
                    _finEff2Divider = 210;
                    _finEff3Divider = 210;
                }
                else
                {
                    _finEff1Divider = 70;
                    _finEff2Divider = 70;
                    _finEff3Divider = 70;
                }
            }
            else if(filter == "[File Name]")
            {
                _finEff1Divider = eff1Divider;
                _finEff2Divider = eff2Divider;
                _finEff3Divider = eff3Divider;
            }
            else if(filter == "[colorname]")
            {
                _finEff1Divider = eff1Divider;
                _finEff2Divider = eff2Divider;
                _finEff3Divider = eff3Divider;
            }
            else if(filter == "Gauge")
            {
                _finEff1Divider = eff1Divider;
                _finEff2Divider = eff2Divider;
                _finEff3Divider = eff3Divider;
            }

            eff1 = Math.Round(eff1 / (_finEff1Divider = _finEff1Divider == 0 ? 1 : _finEff1Divider), 1);
            eff2 = Math.Round(eff2 / (_finEff2Divider = _finEff2Divider == 0 ? 1 : _finEff2Divider), 1);
            eff3 = Math.Round(eff3 / (_finEff3Divider = _finEff3Divider == 0 ? 1 : _finEff3Divider), 1);

            if (filter.ToString() != "Gauge" && filter.ToString() != "[colorname]" && filter.ToString() != "[File Name]")
            {
                scarti1 = Math.Round((scarti1 / (_finEff1Divider = _finEff1Divider == 0 ? 1 : _finEff1Divider)), 1);
                scarti2 = Math.Round((scarti2 / (_finEff2Divider = _finEff2Divider == 0 ? 1 : _finEff2Divider)), 1);
                scarti3 = Math.Round((scarti3 / (_finEff3Divider = _finEff3Divider == 0 ? 1 : _finEff3Divider)), 1);

                rammendi1 = Math.Round((rammendi1 / (_finEff1Divider = _finEff1Divider == 0 ? 1 : _finEff1Divider)), 1);
                rammendi2 = Math.Round((rammendi2 / (_finEff2Divider = _finEff2Divider == 0 ? 1 : _finEff2Divider)), 1);
                rammendi3 = Math.Round((rammendi3 / (_finEff3Divider = _finEff3Divider == 0 ? 1 : _finEff3Divider)), 1);
            }

            dgvReport.Rows[0].Cells[1].Value = eff1 == 0.0 ? string.Empty : eff1.ToString() + "%";
            dgvReport.Rows[0].Cells[6].Value = eff2 == 0.0 ? string.Empty : eff2.ToString() + "%";
            dgvReport.Rows[0].Cells[11].Value = eff3 == 0.0 ? string.Empty : eff3.ToString() + "%";
            dgvReport.Rows[0].Cells[2].Value = tm1;
            dgvReport.Rows[0].Cells[7].Value = tm2;
            dgvReport.Rows[0].Cells[12].Value = tm3;
            dgvReport.Rows[0].Cells[3].Value = qty1.ToString();
            dgvReport.Rows[0].Cells[8].Value = qty2.ToString();
            dgvReport.Rows[0].Cells[13].Value = qty3.ToString();
            if (filter.ToString() != "Gauge" && filter.ToString() != "[colorname]" && filter.ToString() != "[File Name]")
            {
                dgvReport.Rows[0].Cells[4].Value = scarti1 == 0.0 ? string.Empty : scarti1.ToString() + "%";
                dgvReport.Rows[0].Cells[9].Value = scarti2 == 0.0 ? string.Empty : scarti2.ToString() + "%";
                dgvReport.Rows[0].Cells[14].Value = scarti3 == 0.0 ? string.Empty : scarti3.ToString() + "%";
                dgvReport.Rows[0].Cells[5].Value = rammendi1 == 0.0 ? string.Empty : rammendi1.ToString() + "%";
                dgvReport.Rows[0].Cells[10].Value = rammendi2 == 0.0 ? string.Empty : rammendi2.ToString() + "%";
                dgvReport.Rows[0].Cells[15].Value = rammendi3 == 0.0 ? string.Empty : rammendi3.ToString() + "%";
            }

            //calculate fermate time 
            for (var c = 26; c <= dgvReport.ColumnCount - 2; c += 2)
            {
                main._hours = 0;
                main._minutes = 0;
                var totalTime = "";
                for (var r = 1; r <= dgvReport.RowCount - 1; r++)
                {
                    int.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString().Split(':')[0], out int h);
                    int.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString().Split(':')[1], out int m);
                    totalTime = main.CumulateHHmm(h, m);
                }
                dgvReport.Rows[0].Cells[c].Value = totalTime.ToString();
            }

            var hoursRange = (Get_from_date().Subtract(Get_to_date()).TotalDays + 1) * 24;
            //calculate fermate eff
            for (var c = 27; c <= dgvReport.ColumnCount - 2; c += 2)
                {
                int.TryParse(dgvReport.Rows[0].Cells[c - 1].Value.ToString().Split(':')[0], out int h);
                int.TryParse(dgvReport.Rows[0].Cells[c - 1].Value.ToString().Split(':')[1], out int m);
                var t = h.ToString() + "." + m.ToString();
                double.TryParse(t, out var time);
                var totalEff = Math.Round((time / hoursRange) * 100,1);
                dgvReport.Rows[0].Cells[c].Value = totalEff.ToString();
                }
            }

        private void CalculateTotalsHorizontal(string filter)
        {
            var main = new MainWnd();

            for (var r = 0; r <= dgvReport.Rows.Count - 1; r++)
            {
                main._hours = 0;
                main._minutes = 0;
                var eff = 0.0;
                var time = "";
                var qty = 0;
                var turno = 3;
                double scarti = 0.0;
                double rammendi = 0.0;
                for (var c = 0; c <= dgvReport.Columns.Count - 2; c++)
                {
                    if (c == 16) continue;
                    if (c == 1 || c == 6 || c == 11)
                    {
                        if (r > 0)
                        {
                            if (string.IsNullOrEmpty(dgvReport.Rows[r].Cells[c].Value.ToString()) ||
                                dgvReport.Rows[r].Cells[c].Value.ToString() == "0" ||
                                dgvReport.Rows[r].Cells[c].Value.ToString() == "0.0")
                                turno--;
                            double.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString(), out double e);
                            eff += e;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(dgvReport.Rows[r].Cells[c].Value.ToString()) ||
                                dgvReport.Rows[r].Cells[c].Value.ToString() == "0" ||
                                dgvReport.Rows[r].Cells[c].Value.ToString() == "0.0")
                            {
                                turno--;
                                continue;
                            }
                            double.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString().Split('%')[0], out double e);
                            eff += e;
                        }
                    }
                    if (c == 2 || c == 7 || c == 12)
                    {
                        if (dgvReport.Rows[r].Cells[c].Value.ToString().Contains(":"))
                        {
                            int.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString().Split(':')[0], out int t);
                            int.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString().Split(':')[1], out int m);
                            time = main.CumulateHHmm(t, m);
                        }
                    }
                    if (c == 3 || c == 8 || c == 13)
                    {
                        int.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString(), out int q);
                        qty += q;
                    }
                    if (filter.ToString() != "Gauge" && filter.ToString() != "[colorname]" && filter.ToString() != "[File Name]")
                    {
                        if (c == 4 || c == 9 || c == 14)
                        {
                            if (r > 0)
                            {
                                double.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString(), out double s);
                                scarti += s;
                            }
                            else
                            {
                                double.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString().Split('%')[0], out double s);
                                scarti += s;
                            }
                        }
                        if (c == 5 || c == 10 || c == 15)
                        {
                            if (r > 0)
                            {
                                double.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString(), out double ramm);
                                rammendi += ramm;
                            }
                            else
                            {
                                double.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString().Split('%')[0], out double ramm);
                                rammendi += ramm;
                            }
                        }
                    }
                }

                var effic = Math.Round(eff / turno, 1);
                var totalScarti = Math.Round(scarti / turno, 1);
                var totalRammendi = Math.Round(rammendi / turno, 1);
                dgvReport.Rows[r].Cells[17].Value = effic.ToString() + "%";
                dgvReport.Rows[r].Cells[18].Value = time;
                dgvReport.Rows[r].Cells[19].Value = qty;
                if (filter.ToString() != "Gauge" && filter.ToString() != "[colorname]" && filter.ToString() != "[File Name]")
                {
                    dgvReport.Rows[r].Cells[20].Value = totalScarti.ToString() + "%";
                    dgvReport.Rows[r].Cells[21].Value = totalRammendi.ToString() + "%";
                }
                dgvReport.Rows[r].Cells[76].Value = effic;
            }
        }

        private string ConvertTime(object value)
            {
            var ts = TimeSpan.FromMinutes(Convert.ToDouble(value));
            return string.Format("{0}:{1}", (int)ts.TotalHours, ts.Minutes);
            }

        #endregion

        #region Drawings

        private void dgvReport_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
            {
            if (e.RowIndex == -1 && e.ColumnIndex > 0 && e.ColumnIndex < dgvReport.ColumnCount - 2)
                {
                Rectangle r2 = e.CellBounds;
                r2.Y += e.CellBounds.Height / 2;
                r2.Height = e.CellBounds.Height / 2;
                e.PaintBackground(r2, true);
                e.PaintContent(r2);
                e.Handled = true;
                }
            //if(e.RowIndex == - 1 && e.ColumnIndex > 0 && e.ColumnIndex <21)
            //{
            //    var rect = new Rectangle(new Point(e.CellBounds.X, e.CellBounds.Height / 2 - 10),
            //                             new Size(e.CellBounds.Width, 5));
            //    e.Graphics.FillRectangle(Brushes.White, rect);
            //    e.Handled = true;
            //}
            }

        private void dgvReport_Scroll(object sender, ScrollEventArgs e)
            {
            dgvReport.Invalidate();
            }

        private void dgvReport_Paint(object sender, PaintEventArgs e)
            {
            var t = 0;
            var specBrush = default(Brush);
            var defW = 180;
            var txt = "";

            for (int c = 1; c < dgvReport.ColumnCount - 2;)
                {
                t++;

                var r1 = dgvReport.GetCellDisplayRectangle(c, -1, true);
                var w2 = dgvReport.GetCellDisplayRectangle(c, -1, true).Width;
                if (t == 1)
                    {
                    specBrush = new SolidBrush(Color.FromArgb(255, 201, 14));
                    defW = 180;
                    txt = "TURNO " + t;
                    _up_title_font = new Font("Tahoma", 8, FontStyle.Regular);
                    }
                else if (t == 2)
                    {
                    specBrush = new SolidBrush(Color.FromArgb(255, 201, 14));
                    defW = 180;
                    txt = "TURNO " + t;
                    _up_title_font = new Font("Tahoma", 8, FontStyle.Regular);
                    }
                else if (t == 3)
                    {
                    specBrush = new SolidBrush(Color.FromArgb(255, 201, 14));
                    defW = 180;
                    txt = "TURNO " + t;
                    _up_title_font = new Font("Tahoma", 8, FontStyle.Regular);
                    }
                else if(t == 4)
                {
                    specBrush = new SolidBrush(Color.White);
                    defW = 10;
                    txt = string.Empty;
                    _up_title_font = new Font("Tahoma", 8, FontStyle.Regular);
                }
                else if (t == 5)
                    {
                    specBrush = new SolidBrush(Color.FromArgb(255, 201, 14));
                    defW = 230;
                    txt = "TURNO TOTALE";
                    _up_title_font = new Font("Tahoma", 8, FontStyle.Regular);
                    }
                else if (t == 6)
                    {
                    specBrush = Brushes.LightGray;
                    defW = 120;
                    txt = "PERFORMANCE";
                    _up_title_font = new Font("Tahoma", 8, FontStyle.Regular);
                    }
                else if (t == 7)
                    {
                    txt = "PETTINE";
                    }
                else if (t == 8)
                    {
                    txt = "MANUALE";
                    }
                else if (t == 9)
                    {
                    txt = "FILATO";
                    }
                else if (t == 10)
                    {
                    txt = "AGHI";
                    }
                else if (t == 11)
                    {
                    txt = "URTO";
                    }
                else if (t == 12)
                    {
                    txt = "PRINCIPALE";
                    }
                else if (t == 13)
                    {
                    txt = "ALTRO";
                    }
                else if (t == 14)
                    {
                    txt = "CHANGE STYLE";
                    }
                else if (t == 15)
                    {
                    txt = "CHANGE COLOR";
                    }
                else if (t == 16)
                    {
                    txt = "MC BREAKDOWN";
                    }
                else if (t == 17)
                    {
                    txt = "YARN DELAY";
                    }
                else if (t == 18)
                    {
                    txt = "YARN QUALITY";
                    }
                else if (t == 19)
                    {
                    txt = "TECHNIQUE";
                    }
                else if (t == 20)
                    {
                    txt = "MAINTENANCE";
                    }
                else if (t == 21)
                    {
                    txt = "ORDER SHORT";
                    }
                else if (t == 22)
                    {
                    txt = "CAMBIO TAGLIA";
                    }
                else if (t == 23)
                    {
                    txt = "PREPRODUZIONE";
                    }
                else if (t == 24)
                    {
                    txt = "SVILUPOTAGLIE";
                    }
                else if (t == 25)
                    {
                    txt = "PROTOTIPO";
                    }
                else if (t == 26)
                    {
                    txt = "CAMPIONARIO";
                    }
                else if (t == 27)
                    {
                    txt = "RIPARAZIONI";
                    }
                else if (t == 28)
                    {
                    txt = "CAMBIO ART.";
                    }
                else if (t == 29)
                    {
                    txt = "PULIZIA ORDINE";
                    }
                else if (t == 30)
                    {
                    txt = "PULIZIA FRONTURE";
                    }
                else if (t == 31)
                    {
                    txt = "CAMBIO AGHI";
                    }

                if (t >= 7)
                    {
                    specBrush = Brushes.LightBlue;
                    defW = 5;
                    _up_title_font = new Font("Tahoma", 8, FontStyle.Regular);
                    }

                if(t == 4)
                {
                    e.Graphics.FillRectangle(Brushes.White, new RectangleF(r1.X, r1.Y, 10, r1.Height));
                }

                r1.X = r1.X - 1;
                r1.Y += 1;

                r1.Width = (r1.Width + w2 + defW);
                r1.Height = r1.Height / 2 - 2;

                e.Graphics.FillRectangle(specBrush, r1);
                e.Graphics.DrawRectangle(Pens.White, r1);
                
                    var rect = new Rectangle(new Point(r1.X, r1.Y + r1.Height),
                                             new Size(r1.Width, 5));
                    e.Graphics.FillRectangle(Brushes.White, rect);

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

                if (t < 4)
                    c += 5;
                if (t == 4)
                    c += 1;
                if (t == 5)
                    c += 5;
                if (t == 6)
                    c += 4;
                if (t >= 7)
                    c += 2;
                }
            }


        #endregion

        #region Filters
        
        private void btnMachine_Click(object sender, EventArgs e)
            {
            ResetFilters();
            
            _machines_array.Clear();
            _machines_array.Append(",");

            for (var i = 1; i <= 210; i++)
                {
                _machines_array.Append(i.ToString() + ",");
                }
            LoadReport("[Machine Registration No.]");
            dgvReport.Columns[0].HeaderText = "Macchina";
            _current_filter = "[Machine Registration No.]";
            }

        private void btnArt_Click(object sender, EventArgs e)
            {
            ResetFilters();
            //_divider = cboArt.Items.Count;
            LoadReport("[File Name]");
            dgvReport.Columns[0].HeaderText = "Articolo";
            _current_filter = "[File Name]";

            IntegrateTableFilter();
            }

        //private void btnColor_Click(object sender, EventArgs e)
        //{
        //    ResetFilters();
        //    //_divider = cbColor.Items.Count;
        //    LoadReport("[colorname]");
        //    dgvReport.Columns[0].HeaderText = "Colore";

        //    IntegrateTableFilter();
        //}

        private void btnFin_Click(object sender, EventArgs e)
            {
            ResetFilters();
            //_divider = cboFin.Items.Count;
            LoadReport("Gauge");
            dgvReport.Columns[0].HeaderText = "Finezza";
            _current_filter = "Gauge";
            }

        private void btnSquadra_Click(object sender, EventArgs e)
            {
            _current_filter = "[Machine Registration No.]";
            //duplicates machine filter for squadra
            btnMachine.PerformClick();
            }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbSquadraSelectedIndexChanged();
        }
        private void cbSquadraSelectedIndexChanged()
        {
            if (cbSquadra.SelectedIndex == 0)
            {
                btnSquadra.PerformClick();
                return;
            }

            _file_name = string.Empty;
            _fin = string.Empty;

            cboMac.Text = "";
            cboArt.Text = "";
            cboFin.Text = "";
            cbMedia.Text = "";
            _machines_array = new StringBuilder();
            _machines_array.Append(",");

            if (cbSquadra.Text == "Squadra 1")
            {
                for (var i = 1; i <= 70; i++)                
                    _machines_array.Append(i.ToString() + ",");
            }
            else if (cbSquadra.Text == "Squadra 2")
            {
                for (var i = 71; i <= 140; i++)                
                    _machines_array.Append(i.ToString() + ",");
            }
            else if (cbSquadra.Text == "Squadra 3")
            {
                for (var i = 141; i <= 210; i++)                
                    _machines_array.Append(i.ToString() + ",");
            }
            LoadReport("[Machine Registration No.]");
        }

        //Reset filters

        private void ResetFilters()
            {
            _file_name = string.Empty;
            _fin = string.Empty;
            _color = string.Empty;
            cboMac.Text = "";
            cboArt.Text = "";
            cboFin.Text = "";
            cbSquadra.Text = "";
            cbMedia.Text = "";            
            }
        #endregion

        private void cboMac_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbMacSelectedIndexChanged();
        }
        private void cbMacSelectedIndexChanged()
        {
            if (cboMac.SelectedIndex == 0)
            {
                btnMachine.PerformClick();
                return;
            }

            _file_name = string.Empty;
            _fin = string.Empty;
            _color = string.Empty;

            cboArt.Text = "";
            cboFin.Text = "";
            cbSquadra.Text = "";
            cbMedia.Text = "";
            _machines_array = new StringBuilder();
            _machines_array.Append(",");

            _machines_array.Append(cboMac.Text + ",");

            LoadReport("[Machine Registration No.]");
            dgvReport.Columns[0].HeaderText = "Macchina";
        }

        private void cboArt_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbArtSelectedIndexChanged();
        }
        private void cbArtSelectedIndexChanged()
        {
            if (cboArt.SelectedIndex == 0)
            {
                btnArt.PerformClick();
                return;
            }
            _file_name = string.Empty;
            _fin = string.Empty;
            _color = string.Empty;

            cboMac.Text = "";
            cboFin.Text = "";
            cbSquadra.Text = "";
            cbMedia.Text = "";
            _file_name = cboArt.SelectedItem.ToString();

            LoadReport("[File Name]");
            dgvReport.Columns[0].HeaderText = "Articolo";
        }

        private void cboFin_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbFinSelectedIndexChanged();
        }
        private void cbFinSelectedIndexChanged()
        {
            if (cboFin.SelectedIndex == 0)
            {
                btnFin.PerformClick();
                return;
            }
            _file_name = string.Empty;
            _fin = string.Empty;

            cboMac.Text = "";
            cboArt.Text = "";
            cbSquadra.Text = "";
            cbMedia.Text = "";
            _color = string.Empty;

            _fin = cboFin.SelectedItem.ToString();
            LoadReport("Gauge");
            dgvReport.Columns[0].HeaderText = "Finezza";
        }

        //private void cbColor_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    cbColorSelectedIndexChanged();
        //}
        //private void cbColorSelectedIndexChanged()
        //{
        //    if (cbMedia.SelectedIndex == 0)
        //    {
        //        btnMedia.PerformClick();
        //        return;
        //    }
        //    _file_name = string.Empty;
        //    _fin = string.Empty;

        //    cboMac.Text = "";
        //    cboFin.Text = "";
        //    cbSquadra.Text = "";
        //    cboArt.Text = "";
        //    _color = cbMedia.SelectedItem.ToString();
        //    LoadReport("[colorname]");
        //    dgvReport.Columns[0].HeaderText = "Colore";
        //}

        private TextBox _txtFilter;
        private void IntegrateTableFilter()
            {
            if (dgvReport.Rows.Count <= 1) return;

            if (_txtFilter != null) dgvReport.Controls.Remove(_txtFilter);

            //create filter

            _txtFilter = new TextBox
                {
                Name = "_txtArtFilter",
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.AliceBlue,
                ForeColor = Color.Black,
                Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular),
                Parent = dgvReport
                };

            dgvReport.Controls.Add(_txtFilter);

            _txtFilter.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            _txtFilter.AutoCompleteSource = AutoCompleteSource.CustomSource;
            _txtFilter.AutoCompleteCustomSource = _acsc; //bind

            var headerRect = dgvReport.GetColumnDisplayRectangle(0, true);
            _txtFilter.Location = new Point(headerRect.Location.X + 1, 90 - _txtFilter.Height - 2);
            _txtFilter.Size = new Size(headerRect.Width - 2, dgvReport.ColumnHeadersHeight);

            _txtFilter.TextChanged += delegate
                {
                    _bsRep.Filter = string.Format("CONVERT(Macchina, System.String) like '%" + _txtFilter.Text.Replace("'", "''") + "%'");

                    dgvReport.DataSource = _bsRep;
                    dgvReport.Columns[dgvReport.Columns.Count - 1].Visible = false;
                    dgvReport.Refresh();
                    };
            }

        private int _hidden = 0;
        private void btnHide_Click(object sender, EventArgs e)
            {
            switch (_hidden)
                {
                case 0:
                for (var c = 22; c <= dgvReport.Columns.Count - 2; c++)
                    {
                    dgvReport.Columns[c].Visible = false;
                    }

                btnHide.Image = Properties.Resources.unhide_30;

                _hidden = 1;
                break;
                case 1:
                for (var c = 22; c <= dgvReport.Columns.Count - 2; c++)
                    {
                    dgvReport.Columns[c].Visible = true;
                    }

                SetColumnVisibilityByTotalValue();

                btnHide.Image = Properties.Resources.hide;

                _hidden = 0;
                break;
                }

            dgvReport.Focus();
            }

        public void ExportToExcel()
            {
            var export = new ExcelExport();
            export.ExportToExcel(dgvReport, Text);
            }

        private void btnSort_Click(object sender, EventArgs e)
        {
            dgvReport.Sort(dgvReport.Columns[76], System.ComponentModel.ListSortDirection.Descending);
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            var filter = "Macchina";
            if (dgvReport.DataSource != null)
                filter = dgvReport.Columns[0].HeaderText.ToString();

            if (filter == "Articolo")
            {
                if (string.IsNullOrEmpty(cboArt.Text))
                    LoadReport("[File Name]");
                else
                    cbArtSelectedIndexChanged();
            }
            else if (filter == "Macchina")
            {
                if (!string.IsNullOrEmpty(cbSquadra.Text))
                    cbSquadraSelectedIndexChanged();
                else if (!string.IsNullOrEmpty(cboMac.Text))
                    cbMacSelectedIndexChanged();
                else if (string.IsNullOrEmpty(cboMac.Text))
                    LoadReport("[Machine Registration No.]");
            }
            else if (filter == "Finezza")
            {
                if (string.IsNullOrEmpty(cboFin.Text))
                    LoadReport("Gauge");
                else cbFinSelectedIndexChanged();

            }
        }

        private void btnMedia_Click(object sender, EventArgs e)
        {
            ResetFilters();
            LoadReport(_current_filter);

            if(_current_filter == "[Machine Registration No.]")
                dgvReport.Columns[0].HeaderText = "Macchina";
            else if(_current_filter == "[File Name]")            
                dgvReport.Columns[0].HeaderText = "Articolo";            
            else if(_current_filter == "[Gauge]")            
                dgvReport.Columns[0].HeaderText = "Finezza";            

            IntegrateTableFilter();
        }

        private void cbMedia_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboArt.Text = string.Empty;
            cboFin.Text = string.Empty;
            cboMac.Text = string.Empty;
            cbSquadra.Text = string.Empty;

            var cb = sender as ComboBox;
            List<DataGridViewRow> rows = new List<DataGridViewRow>();
            btnMedia.PerformClick();
            if (cb.SelectedIndex == 0)
            {
                return;
            }
            else if (cb.SelectedIndex == 1)
            {
                for (var r = 1; r < dgvReport.Rows.Count; r++)
                {
                    double.TryParse(dgvReport.Rows[r].Cells[17].Value.ToString().Split('%')[0], out var eff);
                    if (eff <= 90) rows.Add(dgvReport.Rows[r]);
                }
                foreach (DataGridViewRow row in rows)
                    dgvReport.Rows.Remove(row);
                dgvReport.Refresh();
            }
            else if (cb.SelectedIndex == 2)
            {
                for (var r = 1; r < dgvReport.Rows.Count; r++)
                {
                    double.TryParse(dgvReport.Rows[r].Cells[17].Value.ToString().Split('%')[0], out var eff);
                    if (eff > 0 && eff < 85 || eff > 90) rows.Add(dgvReport.Rows[r]);
                }
                foreach (DataGridViewRow row in rows)
                    dgvReport.Rows.Remove(row);
                dgvReport.Refresh();
            }
            else if (cb.SelectedIndex == 3)
            {
                for (var r = 1; r < dgvReport.Rows.Count; r++)
                {
                    double.TryParse(dgvReport.Rows[r].Cells[17].Value.ToString().Split('%')[0], out var eff);
                    if (eff >= 85) rows.Add(dgvReport.Rows[r]);
                }
                foreach (DataGridViewRow row in rows)
                    dgvReport.Rows.Remove(row);
                dgvReport.Refresh();
            }
        }
    }
    public class TurnoScartiRammendi
    {
        public TurnoScartiRammendi() { }
        public TurnoScartiRammendi(int mac)
        {
            Machine = mac;
        }
        public int Machine { get; set; }

        public int Turno1Scarti { get; set; }
        public int Turno2Scarti { get; set; }
        public int Turno3Scarti { get; set; }

        public int Turno1Rammendi { get; set; }
        public int Turno2Rammendi { get; set; }
        public int Turno3Rammendi { get; set; }

        public int Turno1Qty { get; set; }
        public int Turno2Qty { get; set; }
        public int Turno3Qty { get; set; }
    }
}
