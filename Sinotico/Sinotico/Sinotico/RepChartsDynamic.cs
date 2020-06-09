using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sinotico
{
    public partial class RepChartsDynamic : Form
    {
        public static bool _RepChartsDynamic = false;
        Globals global = new Globals();
        private List<Label> _machines = new List<Label>();
        private Label _lbl_shfit;
        private System.Windows.Forms.DataVisualization.Charting.Chart _chart;
        private DataGridView _dgv;
        private StringBuilder _machines_array;
        private Panel _panel_shift;
        private List<string> _lines_array = new List<string>();
        private DataGridView[] _dgvTables = new DataGridView[3];

        public RepChartsDynamic()
        {
            InitializeComponent(); 
        }        

        private void RepChartsDynamic_Load(object sender, EventArgs e)
        {
            _dgvTables[0] = dgv_sq_1;
            _dgvTables[1] = dgv_sq_2;
            _dgvTables[2] = dgv_sq_3;

            _RepChartsDynamic = true;

            chart1.Visible = false;
            chart2.Visible = false;
            chart3.Visible = false;

            //LoadingInfo.InfoText = "Loading charts for Squadra 1...";
            //LoadingInfo.ShowLoading();

            _machines_array = new StringBuilder();
            _machines_array.Append(',');

            _lines_array = new List<string>();

            if (!MainWnd._isSquadra)
            {
                _machines_array.Append(MainWnd._mouseOverMachineNumber + ",");

                var line = new Line();

                //lblSelTitle.Text = "Macchina: " + MainWnd._mouseOverMachineNumber + " (" + line.GetLineNumber(Convert.ToInt32(MainWnd._mouseOverMachineNumber)) + ")";

                Text = "Macc: " + MainWnd._mouseOverMachineNumber;

                CreateMachinesReport();
            }
            else
            {
                if (MainWnd._sqOne.Checked ||
                    MainWnd._sqTwo.Checked ||
                    MainWnd._sqThree.Checked)
                {
                    PopulateChartData();
                    //CreateTableView();
                }

                CreateObjectGroups();
                PostMachineColors();
                rbMostraTutto.Checked = true;
            }

            try
            {
                LoadingInfo.CloseLoading();
            }
            catch
            {

            }
        }
        
        public void CollectMachineColors(RadioButton rb)
        {
            Color targetedColor = default(Color);

            if (rb.Name == "rbGreen") targetedColor = Color.FromArgb(54, 214, 87);
            else if (rb.Name == "rbYellow") targetedColor = Color.FromArgb(254, 215, 1);
            else targetedColor = Color.FromArgb(253, 129, 127);

            foreach (var m in _list_of_machines)
            {
                foreach (KeyValuePair<Label, Color> k in MainWnd._currentColors)
                {
                    var name = k.Key.Name;
                    if (m.Name != name) continue;

                    if (k.Value == targetedColor)
                        m.BackColor = k.Value;
                    else m.BackColor = Color.Gainsboro;
                }
            }
        }

        public void PostMachineColors()
        {
            foreach (var m in _list_of_machines)
            {
                foreach (KeyValuePair<Label, Color> k in MainWnd._currentColors)
                {
                    var name = k.Key.Name;
                    if (m.Name != name) continue;
                    m.BackColor = k.Value;
                }
            }

            foreach (var l in _list_of_lines)
            {
                foreach (KeyValuePair<Label, Color> k in MainWnd._total_eff)
                {
                    var name = k.Key.Name;
                    if (l.Name != name) continue;
                    l.ForeColor = k.Value;
                }
            }

            foreach (var b in _list_of_blocks)
            {
                foreach (KeyValuePair<Label, Color> k in MainWnd._total_eff)
                {
                    var name = k.Key.Name;
                    if (b.Name != name) continue;

                    if (name.Substring(1, name.Length - 1) == "1") txtLabel1.ForeColor = k.Value;
                    else if (name.Substring(1, name.Length - 1) == "2") txtLabel2.ForeColor = k.Value;
                    else txtLabel3.ForeColor = k.Value;
                }
            }
        }

        private List<Label> _list_of_machines = new List<Label>();
        private List<Label> _list_of_lines = new List<Label>();
        private List<Label> _list_of_blocks = new List<Label>();
        public void CreateObjectGroups()
        {
            _list_of_machines = new List<Label>();
            _list_of_lines = new List<Label>();
            _list_of_blocks = new List<Label>();

            foreach (Panel groupBox in new Panel[] { panel2, panel3, panel4 })
            {
                if (!groupBox.Visible) continue;

                foreach (var machine in (from lbl in groupBox.Controls.OfType<Label>()
                                         where lbl.Name.Substring(0, 1) == "P"
                                         select lbl).ToList())
                {
                    _list_of_machines.Add(machine);
                }

                foreach (var line in (from lbl in groupBox.Controls.OfType<Label>()
                                      where lbl.Name.Substring(0, 1) == "L"
                                      select lbl).ToList())
                {
                    _list_of_lines.Add(line);
                }

                foreach (var block in (from lbl in groupBox.Controls.OfType<Label>()
                                       where lbl.Name.Substring(0, 1) == "S"
                                       select lbl).ToList())
                {
                    _list_of_blocks.Add(block);
                }
            }
        }

        private void StyleGridView(DataGridView dgv)
        {
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
            dgv.AllowUserToOrderColumns = false;
            dgv.ReadOnly = true;
            dgv.MultiSelect = false;
            dgv.EnableHeadersVisualStyles = false;

            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.BackgroundColor = Color.FromArgb(240, 240, 240);
            dgv.BorderStyle = BorderStyle.None;
            dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgv.Font = new Font("Microsoft Sans Serif", 8);
            dgv.ForeColor = Color.Black;
            dgv.ColumnHeadersHeight = 30;
            dgv.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgv.AdvancedCellBorderStyle.All = DataGridViewAdvancedCellBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.GridColor = Color.White;
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
            dgv.DoubleBufferedDataGridView(true);

            dgv.DataBindingComplete += delegate
             {
                 dgv.RowHeadersVisible = false;

                 for (var i = 0; i <= 4; i++)
                     dgv.Columns[i].HeaderCell.Style.BackColor = Color.FromArgb(181, 181, 181);

                 for (var c = 5; c <= dgv.Columns.Count - 3; c++)
                 {
                     dgv.Columns[c].HeaderCell.Style.BackColor = global.GetStopColors()[c - 5];

                     dgv.Columns[c].HeaderCell.Style.ForeColor = Color.White;
                     dgv.Columns[c].HeaderCell.Style.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);

                     dgv.Columns[c].HeaderText = global.StopReasonsIt[c - 5];
                 }
                 dgv.Columns["Generic Stops"].HeaderCell.Style.ForeColor = Color.White;

                 dgv.Columns["Turni"].Visible = false;
                 dgv.Columns["Periodo"].Visible = false;
                 dgv.Columns["Fin"].Visible = false;
                 dgv.Columns["Articol"].Visible = false;
                 dgv.Columns["Teli"].Visible = false;


                 dgv.Columns["Generic Stops"].HeaderCell.Style.BackColor = Color.Gold;
                 dgv.Columns["TOTAL"].HeaderCell.Style.BackColor = Color.FromArgb(181, 181, 181);

                 var dgvSize = 0;
                 foreach (DataGridViewColumn column in dgv.Columns)
                 {
                     if (column.Index == 0 || column.Index == 6 || column.Index == 12)
                     {
                         column.Width = 60;
                         dgvSize += 60;
                         continue;
                     }
                     column.Width = 50;
                     dgvSize += 50;
                 }
                 dgv.Size = new Size(dgvSize + 50, 80);
             };

        }

        private void CreateMachinesReport()
        {
            LoadingInfo.InfoText = "Loading chart...";
            LoadingInfo.ShowLoading();
            /*
             **Create controls based on shifts selection
             */

            var global = new Globals();

            var chart_data = new DataTable();
            var table_data = new DataTable();

            // Loads charts data

            using (var con = new SqlConnection(MainWnd.conString))
            {
                var cmd = new SqlCommand("get_data_in_hold_per_hour", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@from_date", SqlDbType.DateTime).Value = MainWnd.Get_to_date();
                cmd.Parameters.Add("@to_date", SqlDbType.DateTime).Value = MainWnd.Get_to_date();
                cmd.Parameters.Add("@machine", SqlDbType.VarChar).Value = _machines_array.ToString();

                con.Open();
                var dr = cmd.ExecuteReader();
                chart_data.Load(dr);
                con.Close();
                dr.Close();
                cmd = null;

                // Loads table data

                cmd = new SqlCommand("get_data_in_hold_per_shift", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@from_date", SqlDbType.DateTime).Value = MainWnd.Get_from_date();
                cmd.Parameters.Add("@to_date", SqlDbType.DateTime).Value = MainWnd.Get_to_date();
                cmd.Parameters.Add("@shift", SqlDbType.VarChar).Value = MainWnd.Get_shift_array().ToString();
                cmd.Parameters.Add("@machine", SqlDbType.VarChar).Value = _machines_array.ToString();

                con.Open();
                dr = cmd.ExecuteReader();
                table_data.Load(dr);
                con.Close();
                dr.Close();
                cmd = null;
            }

            // Creates controls with data structure

            var topObjLocation = 20;
            var leftObjLocation = 17;
            var objW = Width - 50;

            _panel_shift = new Panel();
            _panel_shift.Dock = DockStyle.Fill;
            _panel_shift.AutoScroll = true;
            Controls.Add(_panel_shift);
            _panel_shift.BringToFront();

            foreach (var shift in MainWnd.ListOfSelectedShifts)
            {
                // Add shift title

                _lbl_shfit = new Label();
                _lbl_shfit.AutoSize = true;
                _lbl_shfit.Font = new Font("Microsoft Sans Serif", 15);
                _lbl_shfit.ForeColor = Color.Black;

                var shf = "";
                var upShift = string.Empty;

                if (shift.Remove(0, 2) == "Night")
                {
                    shf = "Turno 1";
                    upShift = "NIGHT";
                }
                else if (shift.Remove(0, 2) == "Morning")
                {
                    shf = "Turno 2";
                    upShift = "MORNING";
                }
                else if (shift.Remove(0, 2) == "Afternoon")
                {
                    shf = "Turno 3";
                    upShift = "AFTERNOON";
                }

                _lbl_shfit.Text = shf;

                var lblW = _lbl_shfit.Width;
                var locX = (Width / 2 - lblW / 2);

                _lbl_shfit.Location = new Point(locX - 20, topObjLocation + 10);

                _panel_shift.Controls.Add(_lbl_shfit);

                topObjLocation += _lbl_shfit.Height;

                // Convert table data
                var conv_table_data = new DataTable();

                foreach (DataColumn dc in table_data.Columns)
                {
                    if (dc.ColumnName == "stop_time") continue;

                    conv_table_data.Columns.Add(dc.ColumnName); //copies datacolumns           
                }

                conv_table_data.Columns.Add("Generic Stops");
                conv_table_data.Columns.Add("TOTAL");

                foreach (DataRow row in table_data.Rows)
                {
                    var newRow = conv_table_data.NewRow();

                    if (row[1].ToString() != upShift) continue;

                    var knitTime = Convert.ToInt32(row[8]);
                    var stopTime = Convert.ToInt32(row[16]);

                    var total = 0;

                    for (var i = 0; i <= 7; i++)
                    {
                        if (i == 0)
                        {
                            newRow[i] = Convert.ToDateTime(row[i]).ToShortDateString();
                        }
                        else
                        {
                            newRow[i] = row[i].ToString();
                        }
                    }

                    for (var i = 8; i <= 15; i++)
                    {
                        newRow[i] = ConvertSecondsToHHmm(Convert.ToInt32(row[i]));

                        total += Convert.ToInt32(row[i]);
                    }

                    var genericStop = 0;
                    var allStops = 0;

                    for (var i = 9; i <= 15; i++)
                    {
                        allStops += Convert.ToInt32(row[i]);
                    }

                    genericStop = stopTime - allStops;

                    if (genericStop < 0) genericStop = 0;
                    total += genericStop;

                    newRow[16] = ConvertSecondsToHHmm(genericStop);
                    newRow[17] = ConvertSecondsToHHmm(total);
                    conv_table_data.Rows.Add(newRow);
                }

                //Add chart

                _chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
                _chart.Size = new Size(objW, 400);
                _chart.Location = new Point(leftObjLocation, topObjLocation + 20);
                _chart.BackColor = Color.WhiteSmoke;
                topObjLocation += _chart.Height;

                _chart.ChartAreas.Clear();
                _chart.Series.Clear();

                _chart.ChartAreas.Add("hold");

                _chart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gainsboro;
                _chart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gainsboro;
                _chart.ChartAreas[0].AxisX.MajorGrid.IntervalOffset = 20;
                _chart.ChartAreas[0].BackColor = Color.WhiteSmoke;
                _chart.ChartAreas[0].AxisX.LineColor = Color.WhiteSmoke;
                _chart.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Gray;
                _chart.ChartAreas[0].AxisY.LineColor = Color.Silver;
                _chart.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Gray;
                _chart.ChartAreas[0].AxisY.Maximum = 60;
                //_chart.ChartAreas[0].AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Hours;
                //_chart.ChartAreas[0].AxisX.LabelAutoFitMinFontSize = 15;

                _chart.Series.Add("knitt");
                _chart.Series.Add("comb");
                _chart.Series.Add("manual");
                _chart.Series.Add("yarn");
                _chart.Series.Add("needle");
                _chart.Series.Add("shock");
                _chart.Series.Add("roller");
                _chart.Series.Add("other");
                _chart.Series.Add("generic");

                _chart.Legends.Clear();

                _chart.Series["knitt"].Color = global.GetStopColors()[0];
                _chart.Series["comb"].Color = global.GetStopColors()[1];
                _chart.Series["manual"].Color = global.GetStopColors()[2];
                _chart.Series["yarn"].Color = global.GetStopColors()[3];
                _chart.Series["needle"].Color = global.GetStopColors()[4];
                _chart.Series["shock"].Color = global.GetStopColors()[5];
                _chart.Series["roller"].Color = global.GetStopColors()[6];
                _chart.Series["other"].Color = global.GetStopColors()[7];

                _chart.Series["generic"].Color = Color.Gold;

                foreach (DataRow row in chart_data.Rows)
                {
                    var hour = Convert.ToInt32(row[0]);

                    int knit = Convert.ToInt32(row[1]) > 0 ? Convert.ToInt32(row[1]) : knit = 0;
                    int comb = Convert.ToInt32(row[3]) > 0 ? Convert.ToInt32(row[3]) : comb = 0;
                    int manual = Convert.ToInt32(row[4]) > 0 ? Convert.ToInt32(row[4]) : manual = 0;
                    int yarn = Convert.ToInt32(row[5]) > 0 ? Convert.ToInt32(row[5]) : yarn = 0;
                    int needle = Convert.ToInt32(row[6]) > 0 ? Convert.ToInt32(row[6]) : needle = 0;
                    int shock = Convert.ToInt32(row[7]) > 0 ? Convert.ToInt32(row[7]) : shock = 0;
                    int roller = Convert.ToInt32(row[8]) > 0 ? Convert.ToInt32(row[8]) : roller = 0;
                    int other = Convert.ToInt32(row[9]) > 0 ? Convert.ToInt32(row[9]) : other = 0;

                    if (knit > 60) knit = 60;

                    if (shift == "cbNight" && hour == 23)
                    {
                        _chart.Series["knitt"].Points.AddXY(hour.ToString(), knit);
                        _chart.Series["comb"].Points.AddXY(hour.ToString(), comb);
                        _chart.Series["manual"].Points.AddXY(hour.ToString(), manual);
                        _chart.Series["yarn"].Points.AddXY(hour.ToString(), yarn);
                        _chart.Series["needle"].Points.AddXY(hour.ToString(), needle);
                        _chart.Series["shock"].Points.AddXY(hour.ToString(), shock);
                        _chart.Series["roller"].Points.AddXY(hour.ToString(), roller);
                        _chart.Series["other"].Points.AddXY(hour.ToString(), other);

                        var stopTime = Convert.ToInt32(row[2]);
                        var otherTime = 0;

                        for (var i = 3; i <= chart_data.Columns.Count - 9; i++)
                        {
                            otherTime += Convert.ToInt32(row[i]);
                        }

                        var unknow = stopTime - otherTime;

                        if (unknow > 60) unknow = 60;
                        else if (unknow < 0) unknow = 0;

                        if ((knit + unknow) > 60) unknow = 60 - knit;

                        _chart.Series["generic"].Points.AddXY(hour.ToString(), unknow);
                    }

                    if (shift == "cbNight" && hour >= 0 && hour <= 6 || shift == "cbMorning" && hour >= 7 && hour <= 14 ||
                        shift == "cbAfternoon" && hour >= 15 && hour <= 22)
                    {
                        _chart.Series["knitt"].Points.AddXY(hour.ToString(), knit);
                        _chart.Series["comb"].Points.AddXY(hour.ToString(), comb);
                        _chart.Series["manual"].Points.AddXY(hour.ToString(), manual);
                        _chart.Series["yarn"].Points.AddXY(hour.ToString(), yarn);
                        _chart.Series["needle"].Points.AddXY(hour.ToString(), needle);
                        _chart.Series["shock"].Points.AddXY(hour.ToString(), shock);
                        _chart.Series["roller"].Points.AddXY(hour.ToString(), roller);
                        _chart.Series["other"].Points.AddXY(hour.ToString(), other);

                        var stopTime = Convert.ToInt32(row[2]);
                        var otherTime = 0;

                        for (var i = 3; i <= 9; i++)
                        {
                            otherTime += Convert.ToInt32(row[i]);
                        }

                        var unknow = stopTime - otherTime;

                        if (unknow > 60) unknow = 60;
                        else if (unknow < 0) unknow = 0;

                        if ((knit + unknow) > 60) unknow = 60 - knit;

                        _chart.Series["generic"].Points.AddXY(hour.ToString(), unknow);
                    }
                }

                foreach (var s in _chart.Series)
                {
                    s.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;
                    s.Font = new Font("Tahoma", 8, FontStyle.Bold);
                    s.LabelForeColor = Color.Black;
                    s.IsValueShownAsLabel = true;

                    foreach (var d in s.Points)
                    {
                        if (d.YValues[0] < 1)
                        {
                            d.IsValueShownAsLabel = false;
                        }
                    }
                }

                _panel_shift.Controls.Add(_chart);

                _dgv = new DataGridView();
                _dgv.Size = new Size(objW, 150);
                _dgv.Location = new Point(leftObjLocation, topObjLocation + 20);

                topObjLocation += _dgv.Height + 20;

                _dgv.EnableHeadersVisualStyles = false;
                _dgv.DoubleBufferedDataGridView(true);

                _dgv.AllowUserToAddRows = false;
                _dgv.AllowUserToDeleteRows = false;
                _dgv.AllowUserToResizeColumns = false;
                _dgv.AllowUserToResizeRows = false;
                _dgv.AllowUserToOrderColumns = false;
                _dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                _dgv.ReadOnly = true;
                _dgv.BackgroundColor = Color.WhiteSmoke;
                _dgv.BorderStyle = BorderStyle.None;
                _dgv.MultiSelect = false;
                _dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;
                _dgv.Font = new Font("Microsoft Sans Serif", 9);
                _dgv.ForeColor = Color.FromArgb(60, 60, 60);
                _dgv.CellBorderStyle = DataGridViewCellBorderStyle.None;

                _dgv.DataSource = conv_table_data;

                _dgv.DataBindingComplete += delegate
                {
                    _dgv.Columns[1].Visible = false;

                    for (var c = 8; c <= _dgv.Columns.Count - 3; c++)
                    {
                        _dgv.Columns[c].HeaderCell.Style.BackColor = global.GetStopColors()[c - 8];

                        _dgv.Columns[c].HeaderCell.Style.ForeColor = Color.White;
                        _dgv.Columns[c].HeaderCell.Style.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);

                        _dgv.Columns[c].HeaderText = global.StopReasonsIt[c - 8];
                    }

                    _dgv.Columns["Generic Stops"].HeaderCell.Style.BackColor = Color.Gold;
                    _dgv.Columns["Generic Stops"].HeaderCell.Style.ForeColor = Color.White;
                    _dgv.Columns["Generic Stops"].HeaderCell.Style.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);

                    foreach (DataGridViewColumn c in _dgv.Columns)
                    {
                        _dgv.AutoResizeColumn(c.DisplayIndex);
                    }
                };

                _panel_shift.Controls.Add(_dgv);
            }

            LoadingInfo.CloseLoading();
        }
        
        public void PopulateChartData()
        {
            var global = new Globals();
            var chart_data = new DataTable();
            var table_data = new DataTable();
            var cbArray = new CheckBox[] { MainWnd._sqOne, MainWnd._sqTwo, MainWnd._sqThree};
            var currentCb = 0;
            var processedCb = 0;
            var checks = 0;
            var checksDone = 0;
            var gridIndex = 0;

            foreach(var cb in cbArray)
            {
                if (cb.Checked) checks++;
            }
            
            foreach (var chart in new System.Windows.Forms.DataVisualization.Charting.Chart[] { chart1, chart2, chart3})
            {
                table_data = new DataTable();

                var machinesArray = string.Empty;

                if (processedCb >= 3 || checks <= checksDone) return;

                while (processedCb < 3)
                {
                    if(cbArray[processedCb].Checked)
                    {
                        currentCb = processedCb;
                        processedCb++;
                        break;
                    }
                    processedCb++;
                }

                if (currentCb == 0)
                    {
                    machinesArray = string.Empty;
                        machinesArray = ",1,2,3,4,5,6,7,8,9,10,11,12,13,14" +
                        ",15,16,17,18,19,20,21,22,23,24,25,26,27,28" +
                        ",29,30,31,32,33,34,35,36,37,38,39,40,41,42" +
                        ",43,44,45,46,47,48,49,50,51,52,53,54,55,56" +
                        ",57,58,59,60,61,62,63,64,65,66,67,68,69,70,";
                    checksDone++;
                    }
                    else if (currentCb == 1)
                    {
                    machinesArray = string.Empty;
                    machinesArray = ",71,72,73,74,75,76,77,78,79,80,81,82,83,84" +
                        ",85,86,87,88,89,90,91,92,93,94,95,96,97,98" +
                        ",99,100,101,102,103,104,105,106,107,108,109,110,111,112" +
                        ",113,114,115,116,117,118,119,120,121,122,123,124,125,126" +
                        ",127,128,129,130,131,132,133,134,135,136,137,138,139,140,";
                    checksDone++;
                }
                    else if (currentCb == 2)
                    {
                    machinesArray = string.Empty;
                    machinesArray = ",141,142,143,144,145,146,147,148,149,150,151,152,153,154" +
                        ",155,156,157,158,159,160,161,162,163,164,165,166,167,168" +
                        ",169,170,171,172,173,174,175,176,177,178,179,180,181,182" +
                        ",183,184,185,186,187,188,189,190,191,192,193,194,195,196" +
                        ",197,198,199,200,201,202,203,204,205,206,207,208,209,210,";
                    checksDone++;
                }
                    else continue;

                chart.Visible = true;
                _dgvTables[gridIndex].Visible = true;

                using (var con = new SqlConnection(MainWnd.conString))
                {
                    var cmd = new SqlCommand("get_data_in_hold_per_hour", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@from_date", SqlDbType.DateTime).Value = MainWnd.Get_to_date();
                    cmd.Parameters.Add("@to_date", SqlDbType.DateTime).Value = MainWnd.Get_to_date();
                    cmd.Parameters.Add("@machine", SqlDbType.VarChar).Value = machinesArray;

                    chart_data = new DataTable();

                    con.Open();
                    var dr = cmd.ExecuteReader();
                    chart_data.Load(dr);
                    con.Close();
                    dr.Close();
                    cmd = null;

                    cmd = new SqlCommand("get_data_in_hold_per_shift", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@from_date", SqlDbType.DateTime).Value = MainWnd.Get_from_date();
                    cmd.Parameters.Add("@to_date", SqlDbType.DateTime).Value = MainWnd.Get_to_date();
                    cmd.Parameters.Add("@shift", SqlDbType.VarChar).Value = MainWnd.Get_shift_array().ToString();
                    cmd.Parameters.Add("@machine", SqlDbType.VarChar).Value = machinesArray;

                    con.Open();
                    dr = cmd.ExecuteReader();
                    table_data.Load(dr);
                    con.Close();
                    dr.Close();
                    cmd = null;
                }

                chart.ChartAreas.Clear();
                chart.Series.Clear();

                chart.ChartAreas.Add("hold");
                chart.ChartAreas[0].AxisX.MajorGrid.LineColor = SystemColors.Control;
                chart.ChartAreas[0].AxisY.MajorGrid.LineColor = SystemColors.Control;
                chart.ChartAreas[0].AxisX.MajorGrid.IntervalOffset = 20;
                chart.ChartAreas[0].BackColor = SystemColors.Control;
                chart.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Gray;
                chart.ChartAreas[0].AxisY.LineColor = Color.FromArgb(158, 158, 158);
                chart.ChartAreas[0].AxisX.LineColor = Color.FromArgb(158, 158, 158);
                chart.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Gray;
                chart.ChartAreas[0].AxisY.Maximum = 30000;

                chart.Series.Add("knitt");
                chart.Series.Add("manual");
                chart.Series.Add("yarn");
                chart.Series.Add("generic");
                
                chart.Series["knitt"].Color = Color.FromArgb(0, 191, 254);
                chart.Series["manual"].Color = Color.FromArgb(255, 2, 0);
                chart.Series["yarn"].Color = Color.FromArgb(131, 0, 125);
                chart.Series["generic"].Color = Color.Gold;

                for (var i = 0; i <= chart.Series.Count - 1; i++)
                {
                    chart.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;
                }

                List<int> shiftHours = new List<int>();
                int[] legendValues = new int[4];
                int _knitt = 0, _manual = 0, _yarn = 0, _generics = 0;
                foreach (var shift in MainWnd.ListOfSelectedShifts)
                {
                    shiftHours.Clear();

                    if (shift == "cbMorning")
                    {
                        for (int i = 7; i <= 14; i++) shiftHours.Add(i);
                    }
                    else if (shift == "cbAfternoon")
                    {
                        for (int i = 15; i <= 22; i++) shiftHours.Add(i);
                    }
                    else if (shift == "cbNight")
                    {
                        for (int i = 0; i <= 6; i++)
                            shiftHours.Add(i);
                        shiftHours.Add(23);
                    }

                    legendValues[3] += _generics;
                    legendValues[2] += _yarn;
                    legendValues[1] += _manual;
                    legendValues[0] += _knitt;

                    _knitt = 0;
                    _manual = 0;
                    _yarn = 0;
                    _generics = 0;

                    foreach (DataRow row in chart_data.Rows)
                    {
                        var hour = Convert.ToInt32(row[0]);

                        int knit, comb, manual, yarn, needle, shock, roller, other;

                        if (shiftHours.Contains(hour))
                        {
                            knit = Convert.ToInt32(row[1]) > 0 ? Convert.ToInt32(row[1]) : knit = 0;
                            comb = Convert.ToInt32(row[3]) > 0 ? Convert.ToInt32(row[3]) : comb = 0;
                            manual = Convert.ToInt32(row[4]) > 0 ? Convert.ToInt32(row[4]) : manual = 0;
                            yarn = Convert.ToInt32(row[5]) > 0 ? Convert.ToInt32(row[5]) : yarn = 0;
                            needle = Convert.ToInt32(row[6]) > 0 ? Convert.ToInt32(row[6]) : needle = 0;
                            shock = Convert.ToInt32(row[7]) > 0 ? Convert.ToInt32(row[7]) : shock = 0;
                            roller = Convert.ToInt32(row[8]) > 0 ? Convert.ToInt32(row[8]) : roller = 0;
                            other = Convert.ToInt32(row[9]) > 0 ? Convert.ToInt32(row[9]) : other = 0;

                            _knitt += knit;
                            _manual += manual;
                            _yarn += yarn;
                            _generics += comb + needle + shock + roller + other;
                        }
                    }
                    chart.Series["knitt"].Points.AddXY(shift.Remove(0, 2), _knitt.ToString());
                    chart.Series["manual"].Points.AddXY(shift.Remove(0, 2), _manual.ToString());
                    chart.Series["yarn"].Points.AddXY(shift.Remove(0, 2), _yarn.ToString());
                    chart.Series["generic"].Points.AddXY(shift.Remove(0, 2), _generics.ToString());                    

                    chart.Titles[chart.Name + "Title"].Visible = true;
                    chart.Titles[chart.Name + "Title"].Text = "SQUADRA " + (currentCb + 1).ToString() + ":";

                    int position = 0;
                    foreach (var serie in chart.Series)
                    {
                        serie.IsVisibleInLegend = true;
                        serie.LegendText = " = " + legendValues[position];
                        position++;
                    }
                    position = 0;  
                }

                var conv_table_data = new DataTable();
    
                double stopTime = 0;
                double total = 0;
                double genericStop = 0;
                double allStops = 0;

                foreach (DataColumn dc in table_data.Columns)
                {
                    if (dc.ColumnName == "stop_time" || dc.ColumnName == "Color" || dc.ColumnName == "Size" ||
                        dc.ColumnName == "Component") continue;

                    conv_table_data.Columns.Add(dc.ColumnName); //copies datacolumns           
                }

                conv_table_data.Columns.Add("Generic Stops");
                conv_table_data.Columns.Add("TOTAL");

                var newRow = conv_table_data.NewRow();
                var grid_col_idx = 5;

                TimeSpan formatedTs = new TimeSpan();

                for (var col_idx = 8; col_idx < 16; col_idx++)
                {
                    double current_cell_value = 0;
                    for (var row_idx = 0; row_idx < table_data.Rows.Count; row_idx++)
                    {
                        current_cell_value += Convert.ToDouble(table_data.Rows[row_idx][col_idx]);
                    }

                    formatedTs = TimeSpan.FromSeconds(current_cell_value);
                    string time = string.Format("{0}:{1}",
                                              (int)formatedTs.TotalHours < 10 ?
                                              "0" + ((int)formatedTs.TotalHours).ToString() :
                                              ((int)formatedTs.TotalHours).ToString(),
                                              (int)formatedTs.Minutes < 10 ?
                                              "0" + ((int)formatedTs.Minutes).ToString() :
                                              ((int)formatedTs.Minutes).ToString());
                    newRow[grid_col_idx] = time;
                    grid_col_idx++;
                    total += current_cell_value;

                    if (col_idx == 8) continue;
                    else allStops += current_cell_value;
                }
                
                for (var row_idx = 0; row_idx < table_data.Rows.Count; row_idx++)
                    stopTime += Convert.ToInt32(table_data.Rows[row_idx][16]);
                
                genericStop = stopTime - allStops;

                if (genericStop < 0) genericStop = 0;
                total += genericStop;

                formatedTs = TimeSpan.FromSeconds(genericStop);
                string generic_time = string.Format("{0}:{1}",
                                          (int)formatedTs.TotalHours < 10 ?
                                          "0" + ((int)formatedTs.TotalHours).ToString() :
                                          ((int)formatedTs.TotalHours).ToString(),
                                          (int)formatedTs.Minutes < 10 ?
                                          "0" + ((int)formatedTs.Minutes).ToString() :
                                          ((int)formatedTs.Minutes).ToString());
                newRow[13] = generic_time;
                formatedTs = TimeSpan.FromSeconds(total);
                string total_time = string.Format("{0}:{1}",
                                          (int)formatedTs.TotalHours < 10 ?
                                          "0" + ((int)formatedTs.TotalHours).ToString() :
                                          ((int)formatedTs.TotalHours).ToString(),
                                          (int)formatedTs.Minutes < 10 ?
                                          "0" + ((int)formatedTs.Minutes).ToString() :
                                          ((int)formatedTs.Minutes).ToString());
                newRow[14] = total_time;
                conv_table_data.Rows.Add(newRow);

                StyleGridView(_dgvTables[gridIndex]);
                _dgvTables[gridIndex].DataSource = conv_table_data;

                gridIndex++;
            }
        }

        private string ConvertSecondsToHHmm(int value)
        {
            TimeSpan time = TimeSpan.FromSeconds(value);
            return time.ToString(@"hh\:mm");
        }

        private void LabelsPaint(object sender, PaintEventArgs e)
        {
            Label lbl = (Label)sender;
            var init = lbl.Name.Substring(0, 1);

            if (init == "L")
            {
                var col = lbl.ForeColor;
                var rect = lbl.ClientRectangle;

                ControlPaint.DrawBorder(e.Graphics, rect,
                                        col, 3, ButtonBorderStyle.Solid,
                                        col, 3, ButtonBorderStyle.Solid,
                                        col, 3, ButtonBorderStyle.Solid,
                                        col, 3, ButtonBorderStyle.Solid);

                using (GraphicsPath path = MainWnd._geometry.RoundedRectanglePath(new Rectangle(-1, -1, lbl.Width + 1, lbl.Height + 1), 4))
                {
                    var pen = new Pen(new SolidBrush(lbl.BackColor), 0);
                    SmoothingMode old = e.Graphics.SmoothingMode;
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawPath(pen, path);

                    lbl.Region = new Region(path);
                    e.Graphics.SmoothingMode = old;
                }
            }
            else if (init == "P")
            {
                using (GraphicsPath path = MainWnd._geometry.RoundedRectanglePath(new Rectangle(-1, -1, lbl.Width + 1, lbl.Height + 1), 4))
                {
                    var pen = new Pen(new SolidBrush(lbl.BackColor), 0);
                    SmoothingMode old = e.Graphics.SmoothingMode;
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawPath(pen, path);

                    lbl.Region = new Region(path);
                    e.Graphics.SmoothingMode = old;
                }
            }
            else if (init == "S")
            {
                var stringCapacity = lbl.Name.Length - 1;
                var num = lbl.Name.Substring(1, stringCapacity);

                var col = lbl.ForeColor;

                if (num == "1") col = txtLabel1.ForeColor;
                else if (num == "2") col = txtLabel2.ForeColor;
                else col = txtLabel3.ForeColor;

                var rect = lbl.ClientRectangle;

                ControlPaint.DrawBorder(e.Graphics, rect,
                                        col, 3, ButtonBorderStyle.Solid,
                                        col, 3, ButtonBorderStyle.Solid,
                                        col, 3, ButtonBorderStyle.Solid,
                                        col, 3, ButtonBorderStyle.Solid);

                int x = rect.Width / 2 - 2;
                int y = rect.Height / 2 - 2;

                e.Graphics.DrawEllipse(new Pen(new SolidBrush(col), 1), x, y, 4, 4);
                e.Graphics.FillEllipse(new SolidBrush(col), x, y, 4, 4);

                using (GraphicsPath path = MainWnd._geometry.RoundedRectanglePath(new Rectangle(-1, -1, lbl.Width + 1, lbl.Height + 1), 4))
                {
                    var pen = new Pen(new SolidBrush(lbl.BackColor), 0);
                    SmoothingMode old = e.Graphics.SmoothingMode;
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawPath(pen, path);

                    lbl.Region = new Region(path);
                    e.Graphics.SmoothingMode = old;
                }

            }
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {
            SolidBrush brush = new SolidBrush(Color.FromArgb(96, 96, 96));
            e.Graphics.DrawLine((new Pen(brush, 2)),
                                new Point(panel2.Location.X - 3, 10),
                                new Point(panel2.Location.X - 3, panel4.Location.Y + panel4.Height));
        }

        private void RadioButtons_Paint(object sender, PaintEventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            var rect = rb.ClientRectangle;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var targetedColor = default(Color);

            if (rb.Name == "rbGreen") targetedColor = Color.FromArgb(54, 214, 87);
            else if (rb.Name == "rbYellow") targetedColor = Color.FromArgb(254, 215, 1);
            else if (rb.Name == "rbRed") targetedColor = Color.FromArgb(253, 129, 127);
            else targetedColor = Color.LightGray;

            SolidBrush b = new SolidBrush(targetedColor);
            Pen p = new Pen(b, 3);

            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(109, 109, 109)), rect);

            e.Graphics.DrawEllipse(p, new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 4, rect.Height - 4));

            if (rb.Checked)
            {
                e.Graphics.DrawEllipse(new Pen(b, 2), new Rectangle(rect.Width / 2 - 4, rect.Height / 2 - 4, 6, 6));
                e.Graphics.FillEllipse(b, new Rectangle(rect.Width / 2 - 4, rect.Height / 2 - 4, 6, 6));
            }
            else e.Graphics.FillEllipse(new SolidBrush(rb.BackColor), new Rectangle(rect.Width / 2 - 4, rect.Height / 2 - 4, 6, 6));

            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
        }

        private void RadioButtons_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;

            if (rb.Name == "rbGreen" || rb.Name == "rbRed" ||
               rb.Name == "rbYellow") CollectMachineColors(rb);
            else PostMachineColors();
        }
    }
}


