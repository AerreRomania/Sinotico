using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sinotico
{
    public partial class RepCleanersJob : Form
    {
        public string Mode { get; set; }

        public string Title { get; set; }

        public bool SingleMachineSelected { get; set; }

        public int Machine { get; set; }

        private Label[] _media_labels = new Label[2];
        private DataTable _table = new DataTable();
        int days = 0;
        int totMachines = 0;

        public RepCleanersJob()
        {
            InitializeComponent();
        }        
        public RepCleanersJob(string mode, string title)
        {
            InitializeComponent();
            Mode = mode;
            Title = title;
            string s = Mode == "cquality" ? "TEMPI PER CQ OPERATORI" : "TEMPI PER PULIZIE OPERATORI";
            this.Text = s;
        }
        public RepCleanersJob(string mode, string title, int snglMac)
        {
            InitializeComponent();
            Mode = mode;
            Title = title;
            string s = Mode == "cquality" ? "Report Quality Control" : "Report Pulizia Operatori";
            this.Text = s;
            SingleMachineSelected = true;
            Machine = snglMac;
        }
        private void RepCleanersJob_Load(object sender, EventArgs e)
        {
            dgvReport.AllowUserToAddRows = false;
            dgvReport.AllowUserToDeleteRows = false;
            dgvReport.AllowUserToResizeColumns = false;
            dgvReport.AllowUserToResizeRows = false;
            dgvReport.AllowUserToOrderColumns = false;
            dgvReport.ReadOnly = true;
            dgvReport.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvReport.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
            dgvReport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvReport.ColumnHeadersHeight = 50;
            dgvReport.DefaultCellStyle.BackColor = Color.FromArgb(232, 232, 232);
            dgvReport.DoubleBufferedDataGridView(true);            
            LoadOperators();
            lblPuliziaJob.Text = Title;
            GetData();            
            btnHelp.Click += btnHelp_Click;
            if (Mode != "cquality") panel2.Visible = false;
            else panel2.Visible = true;
        }
        public void ExportToExcel()
        {
            var export = new ExcelExport();
            export.ExportToExcel(dgvReport, Text);
        }
        private void LoadOperators()
        {
            //var q = string.Empty;
            //if (Mode != "cquality") q = "select fullname from operators";
            //else if (Mode == "cleaner") q = "select fullname from operators where operatorinfo = 'CQ' ";
            //else return;
            //using (var c = new System.Data.SqlClient.SqlConnection(MainWnd.conString))
            //{
            //    var cmd = new System.Data.SqlClient.SqlCommand(q, c);
            //    c.Open();
            //    var dr = cmd.ExecuteReader();
            //    if (dr.HasRows)
            //    {
            //        while (dr.Read())
            //        {
            //            if (!cbOperat.Items.Contains(dr[0].ToString()))
            //            {
            //                cbOperat.Items.Add(dr[0].ToString());
            //            }
            //        }
            //    }
            //    c.Close();
            //    dr.Close();
            //}
        }
        static Tuple<string, string> GetLineSquadra(int machine)
        {
            string squadra = string.Empty;
            string line = string.Empty;

            if (machine <= 70) squadra = "Squadra 1";
            else if (machine >= 71 && machine <= 140) squadra = "Squadra 2";
            else if (machine >= 141) squadra = "Squadra 3";

            int lowerBound = 0;
            int upperBound = 0;
            int firstInLine = 0;
            int lastInLine = 0;
            if (squadra == "Squadra 1")
            {
                firstInLine = 1;
                lastInLine = 14;
                lowerBound = 0;
                upperBound = 5;
            }
            else if (squadra == "Squadra 2")
            {
                firstInLine = 71;
                lastInLine = 84;
                lowerBound = 5;
                upperBound = 11;
            }
            else if (squadra == "Squadra 3")
            {
                firstInLine = 141;
                lastInLine = 154;
                lowerBound = 10;
                upperBound = 16;
            }
            for (var i = lowerBound; i < upperBound; i++)
            {
                if (machine >= firstInLine && machine <= lastInLine)
                {
                    line = "Linea " + (i + 1).ToString();
                    break;
                }
                firstInLine += 14;
                lastInLine += 14;
            }
            return Tuple.Create(line, squadra);
        }
        private void CreateColumns()
        {
            _table = new DataTable();
            if(Mode == "cquality")
            {
                _table.Columns.Add("Nr");
                _table.Columns.Add("Linea");
                _table.Columns.Add("Squadra");
                _table.Columns.Add("Mac");              
                _table.Columns.Add("Operatore");
                _table.Columns.Add("Progr");
                _table.Columns.Add("Data Inizio");
                _table.Columns.Add("Data Fine");
                _table.Columns.Add("Durata controllo");
                _table.Columns.Add("Durata pausa");
                _table.Columns.Add("Note");
                _table.Columns.Add("Commessa");
                _table.Columns.Add("Articolo");
                _table.Columns.Add("Componente");
                _table.Columns.Add("Taglia");
                _table.Columns.Add("Colore");
                _table.Columns.Add("Cotta");
                _table.Columns.Add("idjob");
            }
            else
            {
                _table.Columns.Add("Nr");
                _table.Columns.Add("Mac");
                _table.Columns.Add("Operatore");
                _table.Columns.Add("Progr");
                _table.Columns.Add("Squadra");
                _table.Columns.Add("Pulizia");
                _table.Columns.Add("Data Inizio");
                _table.Columns.Add("Data Fine");
                _table.Columns.Add("Durata controllo");
                _table.Columns.Add("Durata pausa");
                _table.Columns.Add("Alarme");
                _table.Columns.Add("Note");
            }            
        }        
        private void GetData()
        {
            CreateColumns();
            var dt = new DataTable();
            if (dgvReport.DataSource != null)
                dgvReport.DataSource = null;
            //var oper = cbOperat.SelectedIndex <= -1 ? string.Empty : cbOperat.Text;
            using (var con = new System.Data.SqlClient.SqlConnection(MainWnd.conString))
            {
                var cmd = new System.Data.SqlClient.SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                if (Mode != "cquality") cmd.CommandText = "[getcleanersticks]";
                else cmd.CommandText = "[getcqualityticks]";
                DateTime startDate = new DateTime();
                DateTime endDate = new DateTime();
                if (SingleMachineSelected)
                {
                    dtpEndDate.Visible = false;
                    dtpStartDate.Visible = false;
                    cmd.Parameters.Add("@idm", SqlDbType.NVarChar).Value = Machine;
                    startDate = MainWnd.Get_from_date();
                    endDate = MainWnd.Get_to_date();
                }
                else
                {
                    startDate = new DateTime(dtpStartDate.Value.Year, dtpStartDate.Value.Month, dtpStartDate.Value.Day);
                    endDate = new DateTime(dtpEndDate.Value.Year, dtpEndDate.Value.Month, dtpEndDate.Value.Day);
                }
                if ((tbColore.Text != string.Empty || tbCommessa.Text != string.Empty ||
                    tbCotta.Text != string.Empty || tbComponente.Text != string.Empty) && Mode == "cquality")
                {
                    cmd.Parameters.Add("@startdate", SqlDbType.DateTime).Value = new DateTime(2019, 1, 1);
                    cmd.Parameters.Add("@enddate", SqlDbType.DateTime).Value = DateTime.Now;
                }
                else
                {
                    cmd.Parameters.Add("@startdate", SqlDbType.DateTime).Value = startDate;
                    cmd.Parameters.Add("@enddate", SqlDbType.DateTime).Value = endDate;
                }

                if(tbColore.Text != string.Empty && Mode == "cquality")                
                    cmd.Parameters.Add("@colorValue", SqlDbType.NVarChar).Value = tbColore.Text;
                if (tbCommessa.Text != string.Empty && Mode == "cquality")
                    cmd.Parameters.Add("@commessaValue", SqlDbType.NVarChar).Value = tbCommessa.Text;
                if (tbCotta.Text != string.Empty && Mode == "cquality")
                    cmd.Parameters.Add("@cottaValue", SqlDbType.NVarChar).Value = tbCotta.Text;
                if (tbComponente.Text != string.Empty && Mode == "cquality")
                    cmd.Parameters.Add("@componenteValue", SqlDbType.NVarChar).Value = tbComponente.Text;

                con.Open();
                var dr = cmd.ExecuteReader();
                dt.Load(dr);
                con.Close();
                dr.Close();
            }
            var nr = 0;
            var lastOperator = string.Empty;
            var lastEndTime = DateTime.MinValue;
            var mediaWork = 0.0;
            var mediaPause = 0.0;
            if (dt.Rows.Count <= 0)
            {
                try
                {
                    if (_filtersList.Count > 0)
                    {
                        foreach (ComboBox c in _filtersList)
                        {
                            c.Hide();
                        }
                    }
                    if (_media_labels.Length > 0)
                    {
                        foreach (Label l in _media_labels)
                        {
                            l.Hide();
                        }
                    }
                }
                catch
                {
                    //dgvReport.Controls.Clear();
                    MessageBox.Show("No data.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                    return;
            }
            var puliziaMachines = new List<int>();
            foreach (DataRow row in dt.Rows)
            {
                //2019-02-20 17:29:36.707	10	test user	1	Pulizia Fronture	2019-02-20 17:29:36.723	2019-02-20 17:29:57.190
                nr++;
                int.TryParse(row[1].ToString(), out var machine);
                var operat = row[2].ToString();
                var prog = row[3].ToString();
                var note = string.Empty;
                var commessa = string.Empty;
                var articolo = string.Empty;
                var componente = string.Empty;
                var taglia = string.Empty;
                var colore = string.Empty;
                var cotta = string.Empty;
                var type = string.Empty;
                int cqId = 0;
                DateTime dtstart = new DateTime();
                DateTime dtend = new DateTime();
                bool alarm = false;
                if (Mode == "cquality")
                {
                    dtstart = Convert.ToDateTime(row[4]);
                    dtend = Convert.ToDateTime(row[5]);
                    note = row[6].ToString();
                    commessa = row[7].ToString();
                    articolo = row[8].ToString();
                    componente = row[9].ToString();
                    taglia = row[10].ToString();
                    colore = row[11].ToString();
                    cotta = row[12].ToString();
                    int.TryParse(row[13].ToString(), out cqId);
                }
                else
                {
                    type = row[4].ToString();
                    bool.TryParse(row[8].ToString(), out alarm);
                    dtstart = Convert.ToDateTime(row[5]);
                    dtend = Convert.ToDateTime(row[6]);
                    note = row[7].ToString();
                }
                var newRow = _table.NewRow();
                if (!puliziaMachines.Contains(machine))
                    puliziaMachines.Add(machine);
                if (operat != lastOperator) lastEndTime = DateTime.MinValue;
                newRow[0] = nr.ToString();                
                if(Mode == "cquality")
                {
                    Tuple<string, string> info = GetLineSquadra(machine);
                    newRow[1] = info.Item1;
                    newRow[2] = info.Item2;
                    newRow[3] = machine;
                    newRow[4] = operat.ToString();
                    newRow[5] = prog.ToString();
                    newRow[6] = dtstart.ToString("dd/MM/yyyy HH:mm:ss");
                    newRow[7] = dtend.ToString("dd/MM/yyyy HH:mm:ss");
                }
                else
                {
                    newRow[1] = machine;
                    newRow[2] = operat.ToString();
                    newRow[3] = prog.ToString();
                    newRow[4] = "";
                    newRow[5] = type.ToString();
                    newRow[6] = dtstart.ToString("dd/MM/yyyy HH:mm:ss");
                    newRow[7] = dtend.ToString("dd/MM/yyyy HH:mm:ss");
                }                
                var currDate = new DateTime(dtstart.Year, dtstart.Month, dtstart.Day);
                var tmpControl = dtend.Subtract(dtstart).TotalSeconds;
                var controlSec = TimeSpan.FromSeconds(tmpControl);
                mediaWork += tmpControl;
                var tmpPause = 0.0;
                var controlPause = new TimeSpan(0, 0, 0);
                if (lastEndTime == DateTime.MinValue)
                {
                    controlPause = new TimeSpan(0, 0, 0);
                }
                else
                {
                    tmpPause = dtstart.Subtract(lastEndTime).TotalSeconds;
                    controlPause = TimeSpan.FromSeconds(tmpPause);
                    
                    mediaPause += tmpPause;
                }                
                if(Mode == "cquality")
                {
                    newRow[8] = controlSec.ToString(@"hh\:mm\:ss");
                    newRow[9] = controlPause.ToString(@"hh\:mm\:ss");
                    newRow[10] = note;
                    newRow[11] = commessa;
                    newRow[12] = articolo;
                    newRow[13] = componente;
                    newRow[14] = taglia;
                    newRow[15] = colore;
                    newRow[16] = cotta;
                    newRow[17] = cqId;
                }
                else
                {
                    newRow[8] = controlSec.ToString(@"hh\:mm\:ss");
                    newRow[9] = controlPause.ToString(@"hh\:mm\:ss");
                    newRow[10] = alarm ? "1" : "0";
                    newRow[11] = note;
                }
                _table.Rows.Add(newRow);
                lastEndTime = dtend;
                lastOperator = operat;
            } 
            mediaWork = mediaWork / nr;
            mediaPause = mediaPause / nr;
            var mediaWorkTime = TimeSpan.FromSeconds(mediaWork);
            var mediaPauseTime = TimeSpan.FromSeconds(mediaPause);
            days = Convert.ToInt32(dtpEndDate.Value.Subtract(dtpStartDate.Value).TotalDays) + 1;
            dgvReport.DataSource = _table;
            long _tot_pause = 0;
            long _tot_controllo = 0;
            foreach (DataGridViewRow row in dgvReport.Rows)
            {
                TimeSpan.TryParse(row.Cells["Durata controllo"].Value.ToString(), out var durata_controllo);
                TimeSpan.TryParse(row.Cells["Durata pausa"].Value.ToString(), out var durata_pausa);
                _tot_pause += durata_pausa.Ticks;
                _tot_controllo += durata_controllo.Ticks;
            }
            TimeSpan totTsTime = TimeSpan.FromTicks(_tot_controllo + _tot_pause);
            string totalTime = string.Format("{0}:{1}:{2}",
                                             (int)totTsTime.TotalHours,
                                             totTsTime.Minutes,
                                             totTsTime.Seconds);
            TimeSpan controlloTsTime = TimeSpan.FromTicks(_tot_controllo);
            string controlloTime = string.Format("{0}:{1}:{2}",
                                                 (int)controlloTsTime.TotalHours,
                                                 controlloTsTime.Minutes,
                                                 controlloTsTime.Seconds);
            TimeSpan pausaTsTime = TimeSpan.FromTicks(_tot_pause);
            string pausaTime = string.Format("{0}:{1}:{2}",
                                                 (int)pausaTsTime.TotalHours,
                                                 pausaTsTime.Minutes,
                                                 pausaTsTime.Seconds);
            lblMedia.Text = "Giorni " + days + "     "//Media " + ""
                            //+ mediaWorkTime.ToString(@"hh\:mm\:ss") + "   |   "
                            //+ mediaPauseTime.ToString(@"hh\:mm\:ss")
                            + "     Tot. durata controllo:  " + controlloTime
                            + "  |  " + "Tot. durata pausa:  " + pausaTime
                            + " | Tot. time: " + totalTime;
            totMachines = puliziaMachines.Count;
            string txt = Mode == "cquality" ? "Tot. CQ: " : "Tot. pulizie:  ";
            lblPuliziaJob.Text = txt + nr +
                                 "  Tot. machines:  " + totMachines.ToString();
            if (Mode == "cquality")
            {
                dgvReport.Columns["Linea"].Width = 65;
                dgvReport.Columns["Squadra"].Width = 65;
                dgvReport.Columns["Note"].Width = 180;
                dgvReport.Columns["Commessa"].Width = 70;
                dgvReport.Columns["Articolo"].Width = 70;
                dgvReport.Columns["Taglia"].Width = 50;
                dgvReport.Columns["Componente"].Width = 70;
                dgvReport.Columns["Colore"].Width = 50;
                dgvReport.Columns["Cotta"].Width = 50;
                dgvReport.Columns["idjob"].Visible = false;
                dgvReport.Columns["Operatore"].Width = 130;
            }
            else
            {
                dgvReport.Columns["Note"].Width = 360;
                dgvReport.Columns["Operatore"].Width = 130;
                dgvReport.Columns["Squadra"].Width = 50;
                dgvReport.Columns["Alarme"].Width = 60;
            }
            dgvReport.Columns["Nr"].Width = 50;
            dgvReport.Columns["Mac"].Width = 50;
            dgvReport.Columns["Progr"].Width = 40;
            dgvReport.Columns["Data Inizio"].Width = 130;
            dgvReport.Columns["Data Fine"].Width = 130;
            dgvReport.Columns["Durata controllo"].Width = 70;
            dgvReport.Columns["Durata pausa"].Width = 70;
            dgvReport.Sort(dgvReport.Columns["Operatore"], ListSortDirection.Ascending);
            int _current_row = 1;
            foreach(DataGridViewRow row in dgvReport.Rows)
            {
                row.Cells["Nr"].Value = _current_row;
                _current_row++;
            }
            if(Mode == "cquality")
            {
                var operators = 1;
                var operatorsName = dgvReport.Rows[0].Cells[4].Value.ToString();
                foreach(DataGridViewRow row in dgvReport.Rows)
                {
                    var currentOperator = row.Cells[4].Value.ToString();
                    if(currentOperator != operatorsName)
                    {
                        operators++;
                        operatorsName = currentOperator;
                    }
                }
                double.TryParse(operators.ToString(), out var numOfOperators);
                TimeSpan tmpOtherTime = TimeSpan.FromHours(numOfOperators * 7.5);
                var otherTimeTicks = tmpOtherTime.Ticks;
                TimeSpan otherTime = TimeSpan.FromTicks(otherTimeTicks - (_tot_controllo + _tot_pause));
                string frmtOtherTime = string.Format("{0}:{1}:{2}",
                                             (int)otherTime.TotalHours,
                                             otherTime.Minutes,
                                             otherTime.Seconds);
                lblMedia.Text += "    Tot. ore disponsibili " + frmtOtherTime; 
            }
            if (dgvReport.Rows.Count >= 1)
            {
                if (dgvReport.Controls.OfType<ComboBox>().Count() <= 0)
                {
                    CreateFilters();
                    _media_labels[0].Text = mediaWorkTime.ToString(@"hh\:mm\:ss");
                    _media_labels[1].Text = mediaPauseTime.ToString(@"hh\:mm\:ss");
                    PopulateFiltersData();
                }
                else
                {
                    PopulateFiltersData();
                    foreach (ComboBox c in _filtersList)
                    {
                        c.Show();
                    }
                    foreach (Label l in _media_labels)
                    {
                        l.Show();
                    }
                }
            }
        }
        #region Filtering
        private List<ComboBox> _filtersList = new List<ComboBox>();
        private static string _strFilter;
        private static void CollectFiltersQuery(DataGridView dgv)
        {
            var i = 0;
            _strFilter = string.Empty;
            foreach (var c in dgv.Controls)
            {
                if (c is ComboBox _combo_box)
                {
                    if (_combo_box.Text != string.Empty)
                    {
                        i++;

                        int.TryParse(_combo_box.Tag.ToString(), out var _column_index);

                        if (i == 1)
                        {                            
                            _strFilter = string.Format("CONVERT(" + dgv.Columns[_column_index].DataPropertyName +
                    ", System.String) like '%" + _combo_box.Text.Replace("'", "''") + "%'");
                        }
                        else
                        {
                            var str = " and CONVERT(" + dgv.Columns[_column_index].DataPropertyName +
                    ", System.String) like '%" + _combo_box.Text.Replace("'", "''") + "%'";

                            _strFilter += str;
                        }
                    }
                }
            }
        }
        private void PopulateFiltersData()
        {
            foreach(Control control in dgvReport.Controls)
            {
                if(control is ComboBox _filter)
                {
                    int.TryParse(_filter.Tag.ToString(), out var _index);
                    //populate filter data
                    _filter.Items.Clear();
                    int i = 0;
                    foreach (DataGridViewRow row in dgvReport.Rows)
                    {
                        DataGridViewCell cell = row.Cells[_index];
                        if (_filter.Items.Contains(cell.Value.ToString()) || cell.Value.ToString() == string.Empty)
                            continue;
                        _filter.Items.Insert(i, cell.Value.ToString());
                    }
                    _filter.Items.Insert(i, string.Empty);
                    if (_filter.Items.Count != 0)
                        _filter.SelectedIndex = 0;
                    _filter.SelectedValueChanged += delegate
                    {
                        CollectFiltersQuery(dgvReport);

                        BindingSource _bin_source = new BindingSource()
                        {
                            DataSource = dgvReport.DataSource,
                            Filter = _strFilter
                        };
                        dgvReport.DataSource = _bin_source;
                        var _pulizia_machines = new List<int>();
                        int _row_nr = 1;
                        long _tot_pause = 0;
                        long _tot_controllo = 0;
                        bool _no_records = false;
                        //update media and pulizia labels depends on selection
                        foreach (DataGridViewRow row in dgvReport.Rows)
                        {
                            if (row.Visible)
                            {
                                _no_records = true;
                                row.Cells["Nr"].Value = _row_nr.ToString();
                                _row_nr++;

                                int.TryParse(row.Cells["Mac"].Value.ToString(), out var machine);
                                if (!_pulizia_machines.Contains(machine)) _pulizia_machines.Add(machine);

                                TimeSpan.TryParse(row.Cells["Durata controllo"].Value.ToString(), out var durata_controllo);
                                TimeSpan.TryParse(row.Cells["Durata pausa"].Value.ToString(), out var durata_pausa);
                                _tot_pause += durata_pausa.Ticks;
                                _tot_controllo += durata_controllo.Ticks;
                            }
                            else continue;
                        }
                        if (!_no_records)
                        {
                            lblPuliziaJob.Text = "Tot. pulizie:   0" +
                                                 "  Tot. machines:   0";
                            lblMedia.Text = "Giorni " + days + "     "//Media " + ""
                                //+ "00:00:00" + "   |   "
                                //+ "00:00:00"
                                + "     Tot. durata controllo:   00:00:00"
                                + "  |  " + "Tot. durata pausa:   00:00:00"
                                + " | Tot. time:  00:00:00";
                            return;
                        }
                        totMachines = _pulizia_machines.Count;
                        string txt = Mode == "cquality" ? "Tot. CQ: " : "Tot. pulizie:  ";
                        lblPuliziaJob.Text = txt + (_row_nr - 1).ToString() +
                                     "  Tot. machines:  " + totMachines.ToString();
                        var oper = (from o in dgvReport.Controls.OfType<ComboBox>()
                                    where o.Tag.ToString() == dgvReport.Columns["Operatore"].DisplayIndex.ToString()
                                    select o).SingleOrDefault();
                        var idm = (from m in dgvReport.Controls.OfType<ComboBox>()
                                   where m.Tag.ToString() == dgvReport.Columns["Mac"].DisplayIndex.ToString()
                                   select m).SingleOrDefault();
                        ComboBox type = new ComboBox();
                        if(Mode != "cquality")
                        type = (from t in dgvReport.Controls.OfType<ComboBox>()
                                    where t.Tag.ToString() == dgvReport.Columns["Pulizia"].DisplayIndex.ToString()
                                    select t).SingleOrDefault();
                        int.TryParse(idm.Text, out var mach);
                        string media = GetMediaWork(oper.Text, mach, type.Text, _row_nr - 1);
                        TimeSpan totTsTime = TimeSpan.FromTicks(_tot_controllo + _tot_pause);
                        string totalTime = string.Format("{0}:{1}:{2}",
                                                         (int)totTsTime.TotalHours,
                                                         totTsTime.Minutes,
                                                         totTsTime.Seconds);
                        TimeSpan controlloTsTime = TimeSpan.FromTicks(_tot_controllo);
                        string controlloTime = string.Format("{0}:{1}:{2}",
                                                             (int)controlloTsTime.TotalHours,
                                                             controlloTsTime.Minutes,
                                                             controlloTsTime.Seconds);
                        TimeSpan pausaTsTime = TimeSpan.FromTicks(_tot_pause);
                        string pausaTime = string.Format("{0}:{1}:{2}",
                                                             (int)pausaTsTime.TotalHours,
                                                             pausaTsTime.Minutes,
                                                             pausaTsTime.Seconds);
                        lblMedia.Text = "Giorni " + days + "     "//Media " + ""
                                //+ media.Split('|')[0] + "   |   "
                                //+ media.Split('|')[1]
                                + "     Tot. durata controllo:  " + controlloTime
                                + "  |  " + "Tot. durata pausa:  " + pausaTime
                                + " | Tot. time: " + totalTime;
                        _media_labels[0].Text = media.Split('|')[0];
                        _media_labels[1].Text = media.Split('|')[1];

                        if (Mode == "cquality")
                        {
                            var operators = 1;
                            var operatorsName = dgvReport.Rows[0].Cells[4].Value.ToString();
                            foreach (DataGridViewRow row in dgvReport.Rows)
                            {
                                var currentOperator = row.Cells[4].Value.ToString();
                                if (currentOperator != operatorsName)
                                {
                                    operators++;
                                    operatorsName = currentOperator;
                                }
                            }
                            double.TryParse(operators.ToString(), out var numOfOperators);
                            TimeSpan tmpOtherTime = TimeSpan.FromHours(numOfOperators * 7.5);
                            var otherTimeTicks = tmpOtherTime.Ticks;
                            TimeSpan otherTime = TimeSpan.FromTicks(otherTimeTicks - (_tot_controllo + _tot_pause));
                            string frmtOtherTime = string.Format("{0}:{1}:{2}",
                                                         (int)otherTime.TotalHours,
                                                         otherTime.Minutes,
                                                         otherTime.Seconds);
                            lblMedia.Text += "    Tot. ore disponsibili " + frmtOtherTime;
                        }
                    };
                }
            }
        }
        private void CreateFilters()
        {
            var durControlIndex = 8;
            var durPausaIndex = 9;
            var end = 0;
            int[] _columns_indexes = new int[] { };
            if (Mode == "cquality")
            {
                _columns_indexes = new int[] { 1, 2, 3, 4, 5, 11, 12, 13, 14, 15, 16 };
                end = 9;
                durControlIndex = 8;
                durPausaIndex = 9; 
            }
            else
            {
                _columns_indexes = new int[] { 1, 2, 3, 4, 5, 10 };
                end = 4;
                durControlIndex = 8;
                durPausaIndex = 9;
            }
            Rectangle rect = dgvReport.GetCellDisplayRectangle(durControlIndex, -1, true);
            var lblControl = new Label()
            {
                Name = "lbl_control",
                AutoSize = false,
                BackColor = Color.Gold,
                ForeColor = Color.Black,
                Text = string.Empty,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Times New Roman", 8, FontStyle.Regular),
                Size = new Size(rect.Width - 2, rect.Height / 3),
                Location = new Point(rect.X + 1, rect.Y + rect.Height - rect.Height / 3 - 1),
            };
            rect = dgvReport.GetCellDisplayRectangle(durPausaIndex, -1, true);
            var lblPausa = new Label()
            {
                Name = "lbl_pausa",
                AutoSize = false,
                BackColor = Color.Gold,
                ForeColor = Color.Black,
                Text = string.Empty,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Times New Roman", 8, FontStyle.Regular),
                Size = new Size(rect.Width - 2, rect.Height / 3),
                Location = new Point(rect.X + 1, rect.Y + rect.Height - rect.Height / 3 - 1)
            };
            _media_labels[0] = lblControl;
            _media_labels[1] = lblPausa;
            dgvReport.Controls.Add(lblControl);
            dgvReport.Controls.Add(lblPausa);
            for (var _filter_num = 0; _filter_num <= end; _filter_num++)
            {
                ComboBox _filter = new ComboBox();
                _filter.Tag = _columns_indexes[_filter_num];
                int.TryParse(_filter.Tag.ToString(), out var _index);
                rect = dgvReport.GetCellDisplayRectangle(_index, -1, true);
                _filter.Size = new Size(rect.Width - 1, rect.Height / 2 - 1);
                _filter.Location = new Point(rect.X + (rect.Width - _filter.Width) - 1,
                                             rect.Y + (rect.Height - _filter.Height) - 1);
                dgvReport.Controls.Add(_filter);
                _filtersList.Add(_filter);
            }
        }
        private string GetMediaWork(string oper, int idm, string type, int machines)
        {
            var dt = new DataTable();
            using (var con = new System.Data.SqlClient.SqlConnection(MainWnd.conString))
            {
                var cmd = new System.Data.SqlClient.SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;

                if (Mode != "cquality") cmd.CommandText = "[getcleanersticks]";
                else cmd.CommandText = "[getcqualityticks]";
                DateTime startDate = new DateTime(dtpStartDate.Value.Year, dtpStartDate.Value.Month, dtpStartDate.Value.Day);
                DateTime endDate = new DateTime(dtpEndDate.Value.Year, dtpEndDate.Value.Month, dtpEndDate.Value.Day);
                cmd.Parameters.Add("@startdate", SqlDbType.DateTime).Value = startDate;
                cmd.Parameters.Add("@enddate", SqlDbType.DateTime).Value = endDate;
                cmd.Parameters.Add("@operator", SqlDbType.NVarChar, 50).Value = oper;
                cmd.Parameters.Add("@idm", SqlDbType.Int).Value = idm;
                cmd.Parameters.Add("@cleaningType", SqlDbType.NVarChar, 50).Value = type;
                con.Open();
                var dr = cmd.ExecuteReader();
                dt.Load(dr);
                con.Close();
                dr.Close();
            }
            var mediaWork = 0.0;
            var mediaPause = 0.0;
            var lastEndTime = DateTime.MinValue;
            foreach (DataRow row in dt.Rows)
            {
                DateTime.TryParse(row[4].ToString(), out var dtstart);
                DateTime.TryParse(row[5].ToString(), out var dtend);
                var tmpControl = dtend.Subtract(dtstart).TotalSeconds;
                mediaWork += tmpControl;
                var tmpPause = 0.0;
                var controlPause = new TimeSpan(0, 0, 0);
                if (lastEndTime == DateTime.MinValue)
                {
                    controlPause = new TimeSpan(0, 0, 0);
                }
                else
                {
                    tmpPause = dtstart.Subtract(lastEndTime).TotalSeconds;
                    mediaPause += tmpPause;
                }

                lastEndTime = dtend;
            }
            mediaWork = mediaWork / machines;
            mediaPause = mediaPause / machines;
            var mediaWorkTime = TimeSpan.FromSeconds(mediaWork);
            var mediaPauseTime = TimeSpan.FromSeconds(mediaPause);
            return mediaWorkTime.ToString(@"hh\:mm\:ss") + "|" + mediaPauseTime.ToString(@"hh\:mm\:ss");
        }
        #endregion Filtering
        private void button2_Click(object sender, EventArgs e)
        { 
            GetData();
        }
        private void dgvReport_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var cell = dgvReport.CurrentCell.RowIndex;
            int.TryParse(dgvReport.Rows[cell].Cells["idjob"].Value.ToString(), out var id);
            var operat = dgvReport.Rows[cell].Cells["Operatore"].Value.ToString();
            var f = new FrmPrintCq(id, operat);
            f.ShowDialog();
            f.Dispose();
        }
        private void btnHelp_Click(object sender, EventArgs e)
        {
            if (pnlHelp.Visible) pnlHelp.Visible = false;
            else
            {
                pnlHelp.Dock = DockStyle.Fill;
                pnlHelp.Visible = true;
            }
        }
    }
}
