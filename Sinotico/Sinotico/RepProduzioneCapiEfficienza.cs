using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sinotico
{
    public partial class RepProduzioneCapiEfficienza : Form
    {
        private List<CapiComponent> _components = new List<CapiComponent>();
        private Font _smallMSSRegular = new Font("Microsoft Sans Serif", 8, FontStyle.Regular);
        private Font _smallMSSBold = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);

        public RepProduzioneCapiEfficienza()
        {
            InitializeComponent();
            btnGo.Click += btnGo_Click;
            CustomizeDataGridView(dgvReport);
        }       

        protected override void OnLoad(EventArgs e)
        {
            FilloutFilters();
            cboMonth.SelectedIndex = DateTime.Now.Month - 1;
            cboYear.SelectedIndex = 0;
            ProcessData();
            base.OnLoad(e);
        }
        private int GetYear()
        {
            int.TryParse(cboYear.Text, out var year);
            return year;
        }
        private int GetMonth()
        {
            var month = cboMonth.SelectedIndex + 1;
            return month;
        }
        private DateTime Get_Date_From()
        {
            return new DateTime(GetYear(), GetMonth(), 1);
        }
        private DateTime Get_Date_To()
        {
            var days = DateTime.DaysInMonth(GetYear(), GetMonth());
            return new DateTime(GetYear(), GetMonth(), days);
        }
        private void CustomizeDataGridView(DataGridView dgv)
        {
            dgvReport.DoubleBufferedDataGridView(true);
            dgv.DataBindingComplete += delegate
            {
                dgv.ReadOnly = true;
                dgv.AllowUserToAddRows = false;
                dgv.AllowUserToDeleteRows = false;
                dgv.AllowUserToOrderColumns = false;
                dgv.AllowUserToResizeColumns = false;
                dgv.AllowUserToResizeRows = false;
                dgv.RowHeadersVisible = false;
                dgv.EnableHeadersVisualStyles = false;
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    col.HeaderCell.Style.BackColor = Color.FromArgb(192, 192, 192);
                    col.HeaderCell.Style.ForeColor = Color.Black;
                    col.HeaderCell.Style.Font = _smallMSSBold;
                }
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    row.Height = 19;
                    if (string.IsNullOrEmpty(row.Cells[0].Value.ToString()) && string.IsNullOrEmpty(row.Cells[1].Value.ToString()) &&
                            string.IsNullOrEmpty(row.Cells[3].Value.ToString()))
                    {
                        for (var i = 0; i < dgv.Columns.Count; i++)
                        {
                            row.Cells[i].Style.BackColor = Color.White;
                            row.Cells[i].Style.ForeColor = Color.White;
                            row.Cells[i].Style.SelectionBackColor = Color.White;
                            row.Cells[i].Style.SelectionForeColor = Color.White;
                        }
                    }
                    else if (string.IsNullOrEmpty(row.Cells[0].Value.ToString()) && string.IsNullOrEmpty(row.Cells[1].Value.ToString()))
                    {
                        for (var i = 0; i < dgv.Columns.Count; i++)
                        {
                            row.Cells[i].Style.Font = _smallMSSBold;
                            if (i == 0 || i == 1)
                            {
                                row.Cells[i].Style.BackColor = Color.White;
                                row.Cells[i].Style.ForeColor = Color.White;
                                row.Cells[i].Style.SelectionBackColor = Color.White;
                                row.Cells[i].Style.SelectionForeColor = Color.White;
                            }
                            else if(i == 2)
                            {
                                row.Cells[i].Style.BackColor = Color.FromArgb(192, 192, 192);
                                row.Cells[i].Style.ForeColor = Color.Black;
                                
                            }
                            else if(i == dgv.Columns.Count - 1 || i == dgv.Columns.Count - 2)
                            {
                                row.Cells[i].Style.ForeColor = Color.Red;
                                row.Cells[i].Style.BackColor = Color.FromArgb(252, 213, 180);
                            }
                            else
                            {
                                row.Cells[i].Style.BackColor = Color.FromArgb(252, 213, 180);
                                row.Cells[i].Style.ForeColor = Color.Black;
                            }
                        }
                    }
                    else if(row.Cells[0].Value.ToString() == "Media")
                    {
                        for (var i = 0; i < dgv.Columns.Count; i++)
                        {
                            row.Cells[i].Style.Font = _smallMSSBold;
                            if (i == 0 || i == 1)
                            {
                                row.Cells[i].Style.BackColor = Color.FromArgb(243, 243, 243);
                                row.Cells[i].Style.ForeColor = Color.Black;
                            }
                            else if (i == 2)
                            {
                                row.Cells[i].Style.BackColor = Color.FromArgb(192, 192, 192);
                                row.Cells[i].Style.ForeColor = Color.Black;

                            }
                            else if (i == dgv.Columns.Count - 1 || i == dgv.Columns.Count - 2)
                            {
                                row.Cells[i].Style.ForeColor = Color.Red;
                                row.Cells[i].Style.BackColor = Color.FromArgb(252, 213, 180);
                            }
                            else
                            {
                                row.Cells[i].Style.BackColor = Color.FromArgb(252, 213, 180);
                                row.Cells[i].Style.ForeColor = Color.Black;
                            }
                        }
                    }
                    else
                    {
                        for (var i = 0; i < dgv.Columns.Count; i++)
                        {
                            if (i == dgv.Columns.Count - 1 || i == dgv.Columns.Count - 2)
                            {
                                row.Cells[i].Style.BackColor = Color.FromArgb(192, 192, 192);
                                row.Cells[i].Style.ForeColor = Color.Black;
                                row.Cells[i].Style.Font = _smallMSSBold;
                            }
                            else
                            {
                                row.Cells[i].Style.BackColor = Color.FromArgb(243, 243, 243);
                                row.Cells[i].Style.ForeColor = Color.Black;
                                row.Cells[i].Style.Font = _smallMSSRegular;
                            }
                        }
                    }
                }
            };
        }
        private void FilloutFilters()
        {
            cboMonth.Items.Clear();
            foreach (var month in new string[] { "January", "February", "March", "April", "May", "June",
                                                 "July", "August", "September", "October", "November", "December"})
                cboMonth.Items.Add(month);
            cboYear.Items.Clear();
            cboYear.Items.Add(DateTime.Now.AddYears(0).Year);
            cboYear.Items.Add(DateTime.Now.AddYears(-1).Year);
            cboYear.Items.Add(DateTime.Now.AddYears(-2).Year);
        }
        private void LoadProcedure(DataSet ds)
        {
            var cmd = new SqlCommand()
            {
                CommandText = "get_capi_report_data",
                Connection = MainWnd._sql_con,
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 99999999
            };
            cmd.Parameters.Add("@from_date", SqlDbType.DateTime).Value = Get_Date_From();
            cmd.Parameters.Add("@to_date", SqlDbType.DateTime).Value = Get_Date_To();
            cmd.Parameters.Add("@table", SqlDbType.VarChar).Value = MainWnd.GetTableSource();
            MainWnd._sql_con.Open();
            var da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            MainWnd._sql_con.Close();
            da.Dispose();
            cmd = null;
        }
        private void ProcessData()
        {
            LoadingInfo.InfoText = "Loading report...";
            LoadingInfo.ShowLoading();
            
            var dsDays = new DataSet();
            LoadProcedure(dsDays);
            _components = new List<CapiComponent>();

            //inserting date, eff and qty in list
            foreach(DataRow row in dsDays.Tables[0].Rows)
            {
                DateTime.TryParse(row[0].ToString(), out var date);
                var eff = row[1].ToString() + "%";
                int.TryParse(row[2].ToString(), out var qty);
                int.TryParse(row[3].ToString(), out var actives);
                _components.Add(new CapiComponent(date, eff, qty, actives));
            }

            //calculating scarti/rammendi per days
            DateTime.TryParse(dsDays.Tables[1].Rows[0][0].ToString(), out var startDate);
            var startShift = dsDays.Tables[1].Rows[0][1].ToString();
            int.TryParse(dsDays.Tables[1].Rows[0][2].ToString(), out var startOperatorCode);
            int.TryParse(dsDays.Tables[1].Rows[0][3].ToString(), out var startMachine);
            var startOrder = dsDays.Tables[1].Rows[0][4].ToString();
            var startArticle = dsDays.Tables[1].Rows[0][5].ToString();
            int.TryParse(dsDays.Tables[1].Rows[0][6].ToString(), out var startTrash);
            int.TryParse(dsDays.Tables[1].Rows[0][7].ToString(), out var startRepair);
            var sumTrash = 0;
            var sumRepair = 0;
            foreach (DataRow row in dsDays.Tables[1].Rows)
            {
                DateTime.TryParse(row[0].ToString(), out var date);
                var shift = row[1].ToString();
                int.TryParse(row[2].ToString(), out var operatorCode);
                int.TryParse(row[3].ToString(), out var machine);
                var order = row[4].ToString();
                var article = row[5].ToString();
                int.TryParse(row[6].ToString(), out var trash);
                int.TryParse(row[7].ToString(), out var repair);
                if(date != startDate)
                {
                    sumTrash += startTrash;
                    sumRepair += startRepair;
                    var component = (from c in _components
                                     where c.CurrentDay == startDate
                                     select c).SingleOrDefault();
                    if (component != null)
                    {
                        double.TryParse(sumTrash.ToString(), out var t);
                        double.TryParse(sumRepair.ToString(), out var r);
                        component.PercTrash = Math.Round((t / component.Pieces) * 100, 1).ToString() + "%";
                        component.PercRepair = Math.Round((r / component.Pieces) * 100, 1).ToString() + "%";
                        double.TryParse(component.Efficiency.Split('%')[0], out var eff);
                        component.EfficiencyNetta = (eff - Math.Round((t / component.Pieces) * 100, 1)).ToString() + "%";
                    }
                    sumTrash = 0;
                    sumRepair = 0;                    
                }
                if(machine != startMachine || shift != startShift || operatorCode != startOperatorCode
                   || order != startOrder || article != startArticle)
                {
                    sumTrash += startTrash;
                    sumRepair += startRepair;
                }
                startDate = date;
                startShift = shift;
                startOperatorCode = operatorCode;
                startMachine = machine;
                startOrder = order;
                startArticle = article;
                startTrash = trash;
                startRepair = repair;
            }

            sumTrash += startTrash;
            sumRepair += startRepair;
            var lstComponent = (from lc in _components
                                where lc.CurrentDay == startDate
                                select lc).SingleOrDefault();
            double.TryParse(sumTrash.ToString(), out var lt);
            double.TryParse(sumRepair.ToString(), out var lr);
            lstComponent.PercTrash = Math.Round((lt / lstComponent.Pieces) * 100, 1).ToString() + "%";
            lstComponent.PercRepair = Math.Round((lr / lstComponent.Pieces) * 100, 1).ToString() + "%";
            double.TryParse(lstComponent.Efficiency.Split('%')[0], out var lstEff);
            lstComponent.EfficiencyNetta = (lstEff - Math.Round((lt / lstComponent.Pieces) * 100, 1)).ToString() + "%";

            var tableView = new DataTable();
            CreateTableView(tableView);

            var counter = 0;
            double mediaActiveMachines = 0.0;
            double mediaRepair = 0.0;
            double mediaTrash = 0.0;
            double mediaEfficiency = 0.0;
            double mediaEfficiencyNetta = 0.0;
            var insertMedia = false;
            var rowPos = 0;
            foreach (DataRow row in tableView.Rows)
            {
                rowPos++;
                int.TryParse(row[1].ToString(), out var day);
                var component = (from c in _components
                                 where c.CurrentDay.Day == day
                                 select c).SingleOrDefault();
                if (insertMedia)
                {
                    if (counter > 0)
                    {
                        row[3] = Math.Round(mediaActiveMachines / counter, 0);
                        row[5] = Math.Round(mediaRepair / counter, 1).ToString() + "%";
                        row[6] = Math.Round(mediaTrash / counter, 1).ToString() + "%";
                        row[7] = Math.Round(mediaEfficiency / counter, 1).ToString() + "%";
                        row[8] = Math.Round(mediaEfficiencyNetta / counter, 1).ToString() + "%";
                    }
                    counter = 0;
                    mediaActiveMachines = 0.0;
                    mediaRepair = 0.0;
                    mediaTrash = 0.0;
                    mediaEfficiency = 0.0;
                    mediaEfficiencyNetta = 0.0;
                    insertMedia = false;
                }
                else
                {
                    if (row[0].ToString() == "Sunday")
                        insertMedia = true;
                    if (component != null)
                    {
                        counter++;
                        double.TryParse(component.PercRepair.Split('%')[0], out var r);
                        double.TryParse(component.PercTrash.Split('%')[0], out var t);
                        double.TryParse(component.Efficiency.Split('%')[0], out var e);
                        double en = 0.0;
                        if(!string.IsNullOrEmpty(component.EfficiencyNetta) && component.EfficiencyNetta.Contains('%'))
                            double.TryParse(component.EfficiencyNetta.Split('%')[0], out en);

                        mediaActiveMachines += component.ActiveMachines;
                        mediaRepair += r;
                        mediaTrash += t;
                        mediaEfficiency += e;
                        mediaEfficiencyNetta += en;

                        row[2] = string.Empty; // totale capi
                        row[3] = component.ActiveMachines;
                        row[4] = string.Empty; //capi per mac
                        row[5] = component.PercRepair;
                        row[6] = component.PercTrash;
                        row[7] = component.Efficiency; //eff
                        row[8] = component.EfficiencyNetta;
                    }
                }
                if(tableView.Rows.Count - 1 == rowPos)
                {
                    if (counter > 0)
                    {
                        row[3] = Math.Round(mediaActiveMachines / counter, 0);
                        row[5] = Math.Round(mediaRepair / counter, 1).ToString() + "%";
                        row[6] = Math.Round(mediaTrash / counter, 1).ToString() + "%";
                        row[7] = Math.Round(mediaEfficiency / counter, 1).ToString() + "%";
                        row[8] = Math.Round(mediaEfficiencyNetta / counter, 1).ToString() + "%";
                    }
                }
            }
    
            if (dgvReport.DataSource != null)
                dgvReport.DataSource = null;

            dgvReport.DataSource = tableView;
            //calculate values for media row --position 0
            for (var c = 1; c < dgvReport.Columns.Count; c++)
            {
                var count = 0;
                int intVal = 0;
                double doubleVal = 0.0;
                for (var r = 2; r < dgvReport.Rows.Count; r++)
                {
                    if (string.IsNullOrEmpty(dgvReport.Rows[r].Cells[c].Value.ToString())) continue;
                    count++;

                    if(c == 3)
                    {
                        int.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString(), out var val);
                        intVal += val;
                    }
                    else if(c >= 5 && c <= 8)
                    {
                        double.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString().Split('%')[0], out var val);
                        doubleVal += val;
                    } 
                }
                if(c == 1)
                {
                    if (count == 0)
                    {
                        dgvReport.Rows[0].Cells[c].Value = "0";
                    }
                    else
                    {
                        var daysInMonth = DateTime.DaysInMonth(GetYear(), GetMonth());
                        double.TryParse(count.ToString(), out var workedDays);
                        dgvReport.Rows[0].Cells[c].Value = Math.Round((workedDays / daysInMonth) * 100, 1).ToString() + "%";
                    }
                }
                else if(c == 3)
                {
                    if (count == 0)
                    {
                        dgvReport.Rows[0].Cells[c].Value = "0";
                    }
                    else
                    {
                        dgvReport.Rows[0].Cells[c].Value = intVal / count;
                    }
                }
                else if(c >= 5 && c <= 8)
                {
                    if (count == 0)
                    {
                        dgvReport.Rows[0].Cells[c].Value = "0";
                    }
                    else
                    {
                        dgvReport.Rows[0].Cells[c].Value = Math.Round(doubleVal / count, 1).ToString() + "%";
                    }
                }
            }
            LoadingInfo.CloseLoading();
        }        
        private void CreateTableView(DataTable dt)
        {
            dt.Columns.Add("WeekDays");
            dt.Columns.Add("Giorno");
            dt.Columns.Add("Totale capi");
            dt.Columns.Add("N. macchine in fuzione");
            dt.Columns.Add("N. capi macchina");
            dt.Columns.Add("% Ramm");
            dt.Columns.Add("% Scarti");
            dt.Columns.Add("% Effic. Netta");
            dt.Columns.Add("% Effic. Lorda");

            var monthMediaRow = dt.NewRow();
            monthMediaRow[0] = "Media";
            dt.Rows.Add(monthMediaRow);
            var sepRow = dt.NewRow();
            dt.Rows.Add(sepRow);
            for (var day = 1; day <= DateTime.DaysInMonth(GetYear(), GetMonth()); day++)
            {
                var firstDate = new DateTime(GetYear(), GetMonth(), day);
                var rowToInsert = dt.NewRow();
                rowToInsert[0] = firstDate.DayOfWeek;
                rowToInsert[1] = day;
                dt.Rows.Add(rowToInsert);
                if (firstDate.DayOfWeek == DayOfWeek.Sunday || day == DateTime.DaysInMonth(GetYear(), GetMonth()))
                {
                    var mediaPerWeek = dt.NewRow();
                    dt.Rows.Add(mediaPerWeek);
                    var separator = dt.NewRow();
                    dt.Rows.Add(separator);
                }
            }
        }
        private void btnGo_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if(string.IsNullOrEmpty(cboMonth.Text) || string.IsNullOrEmpty(cboYear.Text))
            {
                MessageBox.Show("Invalid month or year value!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ProcessData();
        }
    }

    public class CapiComponent
    {
        public CapiComponent()
        {
        }

        public CapiComponent(DateTime currentDay, string efficiency, int pieces, int activeMachines)
        {
            CurrentDay = currentDay;
            Efficiency = efficiency;
            Pieces = pieces;
            ActiveMachines = activeMachines;
            PercTrash = string.Empty;
            PercRepair = string.Empty;
        }

        public DateTime CurrentDay { get; set; }
        public string Efficiency { get; set; }
        public int Pieces { get; set; }
        public int ActiveMachines { get; set; }
        public string PercTrash { get; set; }
        public string PercRepair { get; set; }
        public string EfficiencyNetta { get; set; }
    }
    public class Week
    {
        public Week() { }
        public Week(int f, int l, int wn)
        {
            FirstDay = f;
            LastDay = l;
            WeekNumber = wn;
            WeekDays = new List<int>();
            for (var i = f; i <= l; i++)
                WeekDays.Add(i);
        }
        public int FirstDay { get; set; }
        public int LastDay { get; set; }
        public int WeekNumber { get; set; }
        public List<int> WeekDays { get; set; }
    }
}
