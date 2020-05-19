using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sinotico
{
    public partial class FrmLayout : Form
    {
        private List<Label> _list_of_machines = new List<Label>();
        private List<Label> _list_of_lines = new List<Label>();
        private List<Label> _list_of_blocks = new List<Label>();
        private List<CurrentInfo> _list_of_currents = new List<CurrentInfo>();
        private Dictionary<string, bool> _machine_alarms = new Dictionary<string, bool>();
        private List<Cleaners> _list_of_cleaners = new List<Cleaners>();
        private Dictionary<int, int> _cleaners_per_machine = new Dictionary<int, int>();
        public static Dictionary<Label, Color> _total_eff = new Dictionary<Label, Color>();
        public static Dictionary<Label, Color> _currentColors = new Dictionary<Label, Color>();
        private List<CleanedMachines> _cleaned_machines = new List<CleanedMachines>();
        private DataTable _tbl_machines = new DataTable();
        private string _cleanersType;
        //private List<string> _listOfExcluedMachines = new List<string>();
        public static Dictionary<string, string> _fileNamesDict = new Dictionary<string, string>();
        private List<ScartiRamendi> _list_of_scarti_rammendi = new List<ScartiRamendi>();

        private System.Threading.Timer _timerLastUpdate = null;

        public static string Mode { get; set; }

        const int MAX_LINE = 14;

        private bool _is_excl_mode1;
        private void Set_is_excl_mode(bool value) => _is_excl_mode1 = value;
        private bool Get_is_excl_mode()
        {
            return _is_excl_mode1;
        }

        private readonly Font _fontBig = new Font("Tahoma", 9, FontStyle.Bold);
        private readonly Font _fontSmall = new Font("Microsoft Sans Serif", 7, FontStyle.Bold);
        private readonly Font _fontBigSq = new Font("Microsoft Sans Serif", 15, FontStyle.Bold);
        private readonly Font _fontSmallSq = new Font("Microsoft Sans Serif", 12, FontStyle.Bold);
        
        public FrmLayout()
        {
            InitializeComponent();
        }

        public FrmLayout(string mode)
        {
            InitializeComponent();
            Mode = mode;
        }

        public FrmLayout(string mode, string cleanersType)
        {
            InitializeComponent();
            Mode = mode;
            _cleanersType = cleanersType;
        }

        public static System.Threading.Timer _tmDelay;
        private void StartDelayedLoading()
        {
            if (_tmDelay != null) _tmDelay.Dispose();

            //if (_sqlHasError) return;

            TimerCallback tcb = new TimerCallback(GetDelayedData);
            AutoResetEvent are = new AutoResetEvent(false);
            Application.DoEvents();

            _tmDelay = new System.Threading.Timer(tcb, are, 3000, 0);
        }

        private void GetDelayedData(object data)
        {
                GetData();
        }

        private void CreateObjectGroups()
        {
            _list_of_machines = new List<Label>();
            _list_of_lines = new List<Label>();
            _list_of_blocks = new List<Label>();

            foreach (Panel groupBox in new Panel[] { grp1, grp2, grp3 })
            {
                if (!groupBox.Visible) continue;

                foreach (var machine in (from lbl in groupBox.Controls.OfType<Label>()
                                         where lbl.Name.Substring(0, 1) == "P"
                                         select lbl).ToList())
                {
                    machine.Font = new Font("Tahoma", 10);
                    machine.TextAlign = ContentAlignment.MiddleRight;

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

        private Pen pen = new Pen(Brushes.Gainsboro, 0);
        private Brush _brush_active = Brushes.LightGray;
        private void Labels_Paint(object sender, PaintEventArgs eventArgs)
        {
            var lbl = (Label)sender;
            var border_pen = new Pen(_brush_active, 2);
            var targetCode = lbl.Name.Substring(0, 1);
            SolidBrush strBrush = new SolidBrush(Color.FromArgb(100, 100, 100));

            if (targetCode == "P")
            {
                var pen = new Pen(new SolidBrush(lbl.BackColor), 0);
                using (GraphicsPath path = MainWnd._geometry.RoundedRectanglePath(new Rectangle(-1, -1, lbl.Width + 1, lbl.Height + 1), 4))
                {
                    SmoothingMode old = eventArgs.Graphics.SmoothingMode;
                    eventArgs.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    eventArgs.Graphics.DrawPath(pen, path);

                    lbl.Region = new Region(path);
                    eventArgs.Graphics.SmoothingMode = old;
                }

                var macNumber = lbl.Name.Substring(1, lbl.Name.Length - 1);

                var strToDraw = macNumber.PadLeft(3, '0');

                if (Mode == "eff")
                {
                    foreach (var item in _list_of_currents)
                    {
                        var machineNumber = item.MachineNumber;
                        var finest = item.Gaudge;

                        if (macNumber != machineNumber) continue;

                        eventArgs.Graphics.DrawString(machineNumber.PadLeft(3, '0'), new Font("Tahoma", 6, FontStyle.Regular), strBrush, 1, 2);
                        eventArgs.Graphics.DrawString(finest, new Font("Tahoma", 6, FontStyle.Regular), strBrush, 1, 10);

                        if (item.Alarm)
                        {
                            eventArgs.Graphics.DrawImage(Sinotico.Properties.Resources.dorbell_icons8,
                                                         new Rectangle(lbl.Width / 3 - 2, lbl.Height / 2 - 3, 10, 10));
                        }
                    }
                }
                else
                {
                    foreach (var item in _list_of_currents)
                    {
                        var machineNumber = item.MachineNumber;
                        var finest = item.Gaudge;

                        if (macNumber != machineNumber) continue;

                        eventArgs.Graphics.DrawString(machineNumber.PadLeft(3, '0'), new Font("Tahoma", 6, FontStyle.Regular), strBrush, 1, 2);
                        eventArgs.Graphics.DrawString(finest, new Font("Tahoma", 6, FontStyle.Regular), strBrush, 1, 10);

                        if (item.Cleaned == "3_months_not_cleaned" && _cleanersType != "cquality" &&
                            Mode != "rammendi" && Mode != "scarti")
                        {
                            eventArgs.Graphics.DrawImage(Sinotico.Properties.Resources.dorbell_icons8,
                                                     new Rectangle(lbl.Width / 3 - 2, lbl.Height / 2 - 3, 10, 10));
                        }
                    }
                }
            }
            else if (targetCode == "L")
            {
                using (GraphicsPath path = MainWnd._geometry.RoundedRectanglePath(new Rectangle(0, 0, lbl.Width, lbl.Height), 3))
                {
                    SmoothingMode old = eventArgs.Graphics.SmoothingMode;
                    eventArgs.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    eventArgs.Graphics.DrawPath(pen, path);

                    lbl.Region = new Region(path);
                    eventArgs.Graphics.SmoothingMode = old;
                }
                eventArgs.Graphics.DrawString(lbl.Name, new Font("Tahoma", 10, FontStyle.Regular), strBrush, 2, 1);
            }
            else if (targetCode == "S")
            {
                var pen = new Pen(new SolidBrush(lbl.BackColor), 6);
                using (GraphicsPath path = MainWnd._geometry.RoundedRectanglePath(new Rectangle(0, 0, lbl.Width, lbl.Height), 6))
                {
                    SmoothingMode old = eventArgs.Graphics.SmoothingMode;
                    eventArgs.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    eventArgs.Graphics.DrawPath(pen, path);

                    lbl.Region = new Region(path);
                    eventArgs.Graphics.SmoothingMode = old;
                }
            }
        }

        private void GetTotals()
        {
            var machineRange = new[] { 1, 14 };     //starts from line 1
            var count = 1;
            var curLineNumber = 1;

            if (Mode == "eff" || Mode == "cleaner" || Mode == "temperature" ||
                Mode == "rammendi" && !MainWnd._teli_mode || Mode == "scarti" && !MainWnd._teli_mode)
            {
                _total_eff = new Dictionary<Label, Color>();
                _total_eff.Clear();
            }

            for (var m = 1; m <= 210; m++)
            {
                count++;

                if (count < MAX_LINE) continue;

                foreach (var line in _list_of_lines.Where(p => p.Name == "L" + curLineNumber.ToString()))
                {
                    line.Text = GetMachinesDataInRange(_list_of_machines, machineRange[0], machineRange[1], 0, false).Trim('=');
                    //false => is line

                    if (Mode == "eff")
                    {
                        _total_eff.Add(line, GetEfficiencyColor(line.Text.Trim('%')));
                        line.Font = _fontBig;
                    }
                    else if (Mode == "tempStd") line.Font = _fontSmall;
                    else if (Mode == "cleaner")
                    {
                        _total_eff.Add(line, Color.FromArgb(54, 215, 86));
                        line.Font = _fontBig;
                    }
                    else if (Mode == "scarti" && !MainWnd._teli_mode || Mode == "rammendi" && !MainWnd._teli_mode)
                    {
                        _total_eff.Add(line, GetSMEfficiencyColor(line.Text.Trim('%')));
                        line.Font = _fontBig;
                    }
                    else
                        line.Font = _fontBig;
                }

                count = 1;
                machineRange = new int[] { machineRange[1] + 1, machineRange[1] + MAX_LINE };

                curLineNumber++;
            }

            S1.Text = GetMachinesDataInRange(_list_of_machines, 1, 70, 1, true);
            S2.Text = GetMachinesDataInRange(
                _list_of_machines, 71, 140, 1, true);
            S3.Text = GetMachinesDataInRange(
                _list_of_machines, 141, 210, 1, true);
            
            switch (Mode)
            {
                case "eff":
                    _total_eff.Add(S1,
                        GetEfficiencyColor(S1.Text.Trim('=', '%'),
                            Color.FromArgb(235, 235, 235)));
                    _total_eff.Add(S2,
                        GetEfficiencyColor(S2.Text.Trim('=', '%'),
                            Color.FromArgb(235, 235, 235)));
                    _total_eff.Add(S3,
                        GetEfficiencyColor(S3.Text.Trim('=', '%'),
                            Color.FromArgb(235, 235, 235)));
                    S1.Font = _fontBigSq;
                    S2.Font = _fontBigSq;
                    S3.Font = _fontBigSq;
                    break;
                case "tempStd":
                    S1.Font = _fontSmallSq;
                    S2.Font = _fontSmallSq;
                    S3.Font = _fontSmallSq;
                    break;
                case "cleaner":
                    _total_eff.Add(S1, Color.FromArgb(54, 215, 86));
                    _total_eff.Add(S2, Color.FromArgb(54, 215, 86));
                    _total_eff.Add(S3, Color.FromArgb(54, 215, 86));                    
                    S1.Font = _fontSmallSq;
                    S2.Font = _fontSmallSq;
                    S3.Font = _fontSmallSq;                    
                    break;
                case "rammendi":
                    if (MainWnd._teli_mode) break;
                    _total_eff.Add(S1,
                        GetSMEfficiencyColor(S1.Text.Trim('=', '%')));
                    _total_eff.Add(S2,
                        GetSMEfficiencyColor(S2.Text.Trim('=', '%')));
                    _total_eff.Add(S3,
                        GetSMEfficiencyColor(S3.Text.Trim('=', '%')));
                    break;
                case "scarti":
                    if (MainWnd._teli_mode) break;
                    _total_eff.Add(S1,
                        GetSMEfficiencyColor(S1.Text.Trim('=', '%')));
                    _total_eff.Add(S2,
                        GetSMEfficiencyColor(S2.Text.Trim('=', '%')));
                    _total_eff.Add(S3,
                        GetSMEfficiencyColor(S3.Text.Trim('=', '%')));
                    break;
            }
        }

        private string GetMachinesDataInRange(List<Label> lst, int from_mac, int to_mac, int round, bool isBlock)
        {
            var tmpStr = "";
            var value = 0.0;
            var counter = 0;
            int totMachineData = 0;

            _dictOfCleanersGroup = new Dictionary<string, int>();

            foreach (var item in lst)
            {
                if (item.BackColor == Color.DimGray
                        || MainWnd.WithoutFermate && item.BackColor == Color.LightGray) continue;
                if (MainWnd._isFinSelected | MainWnd._isArticleSelected
                    && item.Text == string.Empty) continue;

                var machineRegNumb = Convert.ToInt32(item.Name.Remove(0, 1));
                if (machineRegNumb < from_mac || machineRegNumb > to_mac) continue;
                counter++;

                switch (Mode)
                {
                    case "tempStd":
                        if (!item.Text.Contains(":")) continue;
                        int.TryParse(item.Text.Split(':')[0], out int hours);
                        int.TryParse(item.Text.Split(':')[1], out int minutes);
                        tmpStr = CumulateHHmm(hours, minutes);
                        break;
                    case "cleaner":
                        if (item.Text != string.Empty)
                        {
                            int.TryParse(item.Text.Split('/')[1], out var jVal);
                            value += Convert.ToDouble(jVal);
                            var nameInit = item.Text.Split('/')[0];

                            if (!_dictOfCleanersGroup.ContainsKey(nameInit))
                                _dictOfCleanersGroup.Add(nameInit, jVal);
                            else
                                _dictOfCleanersGroup[nameInit] += jVal;
                        }
                        break;
                    default:
                        {
                            if (Mode != "velo" && Mode != "cleaner" && Mode != "temperature")
                            {
                                double.TryParse(item.Text, out var result);
                                value += result;
                            }
                            break;
                        }
                }
            }
            _hours = 0; _minutes = 0;

            switch (Mode)
            {
                case "eff":
                    if (counter != 0)
                        tmpStr = "=" + Math.Round(value / counter, round).ToString() + "%";
                    break;
                case "qty":
                    tmpStr = value.ToString();
                    break;
                case "cleaner":
                    var days = (MainWnd.Get_to_date().Subtract(MainWnd.Get_from_date())).TotalDays;

                    if (days == 0 && isBlock)
                    {
                        var sb = new StringBuilder();

                        foreach (var k in _dictOfCleanersGroup)
                        {
                            totMachineData += k.Value;
                            sb.AppendLine(k.Key.ToString() + "=" + (days + 1).ToString(CultureInfo.CurrentCulture)
                                          + "/" + k.Value.ToString(CultureInfo.CurrentCulture));
                        }

                        sb.AppendLine("Tot.=" + (days + 1).ToString(CultureInfo.CurrentCulture) + "/" + totMachineData.ToString());
                        tmpStr = sb.ToString();
                    }
                    else
                    {
                        tmpStr = (days + 1).ToString(CultureInfo.CurrentCulture) + "/" + value.ToString(CultureInfo.CurrentCulture);
                    }
                    break;
                case "rammendi":
                        {
                            var rammendi = (from x in _list_of_scarti_rammendi
                                            where x.Machine >= from_mac && x.Machine <= to_mac
                                            select x).ToList();
                            double producedQty = 0;
                            double producedRamm = 0;
                            foreach (var m in rammendi)
                            {
                                producedRamm += m.Rammendi;
                                producedQty += m.TeliBuoni;
                            }

                            if (MainWnd._teli_mode) tmpStr = "=" + producedRamm.ToString();
                            else
                            {
                                var val = (Convert.ToDouble(producedRamm) / Convert.ToDouble(producedQty)) * 100;
                                tmpStr = string.Format("={0:0.0}%", val);
                            }
                            break;
                        }
                case "scarti":
                    {
                        var scarti = (from x in _list_of_scarti_rammendi
                                      where x.Machine >= from_mac && x.Machine <= to_mac
                                      select x).ToList();
                        int producedQty = 0;
                        int producedScarti = 0;
                        foreach (var m in scarti)
                        {
                            producedScarti += m.Scarti;
                            producedQty += m.TeliBuoni;
                        }
                        if (MainWnd._teli_mode) tmpStr = "=" + producedScarti.ToString();
                        else
                        {
                            var val = (Convert.ToDouble(producedScarti) / Convert.ToDouble(producedQty)) * 100;
                            tmpStr = string.Format("={0:0.0}%", val);
                        }
                        break;
                    }
                case "temperature":
                    value = 0;
                    break;
            }
            return tmpStr.ToString(CultureInfo.CurrentCulture);
        }

        private Dictionary<string, int> _dictOfCleanersGroup = new Dictionary<string, int>();
        private static Color GetEfficiencyColor(string text)
        {
            var color = default(Color);
            double.TryParse(text, out var eff);

            if (eff == 0)
                color = Color.LightGray;
            else if
                (eff > 0 && eff <= 85.0) color = Color.FromArgb(253, 129, 127);
            else if
                (eff > 85.0 && eff <= 90.0) color = Color.FromArgb(254, 215, 1);
            else if
                (eff > 90.0) color = Color.FromArgb(54, 214, 87);
            else
                color = Color.LightGray;

            return color;
        }

        private static Color GetEfficiencyColor(string text, Color optColor)
        {
            var color = default(Color);
            double.TryParse(text, out var eff);
            if (eff == 0)
                color = optColor;
            else if
                (eff > 0 && eff <= 85.0) color = Color.FromArgb(253, 129, 127);
            else if
                (eff > 85.0 && eff <= 90.0) color = Color.FromArgb(254, 215, 1);
            else if
                (eff > 90.0) color = Color.FromArgb(54, 214, 87);
            else
                color = optColor;

            return color;
        }

        public int _hours;
        public int _minutes;

        public string CumulateHHmm(int hours, int minutes)
        {
            _hours += hours;
            _minutes += minutes;

            if (_minutes <= 59) return $"{_hours}:{_minutes}";
            _minutes = _minutes % 60;
            _hours++;

            return $"{_hours}:{_minutes}";
        }

        private void ResetGlobals()
        {
            foreach (Panel groupBox in new Panel[] { grp1, grp2, grp3 })
            {
                // Collects all labels that are represents machines

                foreach (var machine in (from lbl in groupBox.Controls.OfType<Label>()
                                         where lbl.Name.Substring(0, 1) == "L" || lbl.Name.Substring(0, 1) == "S"
                                         select lbl).ToList())
                {
                    machine.BackColor = Color.Gainsboro;
                    machine.Text = "0";
                }
            }
        }
        
        private void GetData()
        {
            if (MainWnd.Get_from_date() > DateTime.Now || MainWnd.Get_to_date() > DateTime.Now) return;

            //MainWnd.Set_from_date(new DateTime(MainWnd.dtpFrom.Value.Year, dtpFrom.Value.Month, dtpFrom.Value.Day));
            //MainWnd.Set_to_date(new DateTime(dtpTo.Value.Year, dtpTo.Value.Month, dtpTo.Value.Day));

            // Checks date selection by user
            var ts = (MainWnd.Get_to_date().Subtract(MainWnd.Get_from_date()));

            if (ts.TotalDays < 0)
            {
                MessageBox.Show("Invalid date selection.");
                return;
            }

            if (!MainWnd.Get_is_excl_mode())
            {
                MainWnd._listOfExcluedMachines.Clear();
            }

            ResetGlobals();

            if (string.IsNullOrEmpty(MainWnd.Get_file_name())) MainWnd.Set_file_name(string.Empty);

            _tbl_machines = new DataTable();

            Cursor = Cursors.WaitCursor;

            //if (Mode != "cleaner")
            //{
            //    LoadingInfo.InfoText = "Loading data...";
            //    LoadingInfo.ShowLoading();
            //}

            _list_of_cleaners = new List<Cleaners>();

            if (Mode == "cleaner")
            {
                _cleaners_per_machine.Clear();
                var _dataSet = new DataSet();

                using (var con = new SqlConnection(MainWnd.conString))
                {
                    var cmd = new SqlCommand("getcleanersession", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.Add("@fromdate", SqlDbType.DateTime).Value = MainWnd.Get_from_date();
                    cmd.Parameters.Add("@todate", SqlDbType.DateTime).Value = MainWnd.Get_to_date();
                    cmd.Parameters.Add("@shift", SqlDbType.NVarChar).Value = MainWnd.Get_shift_array().ToString();
                    cmd.Parameters.Add("@type", SqlDbType.NVarChar).Value = _cleanersType;
                    cmd.Parameters.Add("@oneDayOnly",
                        SqlDbType.Bit).Value = MainWnd.Get_from_date().Equals(MainWnd.Get_to_date()) ? true : false;

                    var da = new SqlDataAdapter(cmd);
                    var ds = new DataSet();
                    da.Fill(_dataSet);
                    da.Dispose();

                    if (_cleanersType == "cquality")
                    {
                        _cleaned_machines.Clear();
                        //_mac_tooltips.Clear();
                        foreach (DataRow row in _dataSet.Tables[3].Rows)
                        {
                            _list_of_cleaners.Add(new Cleaners(row[0].ToString(), row[1].ToString(), Convert.ToInt32(row[2])));
                        }
                        foreach (DataRow row in _dataSet.Tables[4].Rows)
                        {
                            int.TryParse(row[0].ToString(), out var idm);
                            int.TryParse(row[1].ToString(), out var num_of_cleaners);
                            _cleaners_per_machine.Add(idm, num_of_cleaners);
                        }
                    }
                    else
                    {
                        //pul. ord/front
                        foreach (DataRow row in _dataSet.Tables[0].Rows)
                        {
                            _list_of_cleaners.Add(new Cleaners(row[0].ToString(), row[1].ToString(), Convert.ToInt32(row[2])));
                        }
                        foreach (DataRow row in _dataSet.Tables[1].Rows)
                        {
                            int.TryParse(row[0].ToString(), out var idm);
                            int.TryParse(row[1].ToString(), out var num_of_cleaners);
                            _cleaners_per_machine.Add(idm, num_of_cleaners);
                        }

                        _cleaned_machines.Clear();
                        if (_dataSet.Tables[2].Rows.Count >= 1)
                            foreach (DataRow row in _dataSet.Tables[2].Rows)
                            {
                                int.TryParse(row[0].ToString(), out var idm);
                                int.TryParse(row[1].ToString(), out var hrs);
                                DateTime.TryParse(row[2].ToString(), out var date);

                                var cmac = (from m in _cleaned_machines
                                            where m.MachineNumber == idm
                                            select m).SingleOrDefault();
                                if (cmac == null)
                                    _cleaned_machines.Add(new CleanedMachines(idm, hrs, date));
                                else
                                    cmac.CleanedHours = hrs;
                            }

                        foreach (var mac in _list_of_currents)
                        {
                            var tstMac = (from m in _cleaned_machines
                                          where m.MachineNumber == int.Parse(mac.MachineNumber)
                                          select m).SingleOrDefault();

                            if (tstMac == null)
                            {
                                mac.Cleaned = "3_months_not_cleaned";
                                mac.ClndHours = "Unknown";
                            }
                            else
                            {
                                if (!MainWnd.Get_to_date().Equals(MainWnd.Get_from_date()))
                                {
                                    if (tstMac.CleanedHours >= 2160)
                                    {
                                        mac.Cleaned = "3_months_not_cleaned";
                                        mac.ClndHours = tstMac.CleanedHours.ToString();
                                        mac.EvntDate = tstMac.EventDate;
                                    }
                                    else
                                    {
                                        mac.Cleaned = "less_than_3_months";
                                        mac.ClndHours = "";
                                        mac.EvntDate = tstMac.EventDate;
                                    }
                                }
                                else
                                {
                                    mac.Cleaned = "less_than_3_months";
                                    mac.ClndHours = "";
                                    mac.EvntDate = tstMac.EventDate;
                                }
                            }
                        }

                        //_mac_tooltips.Clear();
                        foreach (var lbl in _list_of_machines)
                        {
                            var macNumber = lbl.Name.Substring(1, lbl.Name.Length - 1);

                            foreach (var item in _list_of_currents)
                            {
                                var machineNumber = item.MachineNumber;

                                if (macNumber != machineNumber) continue;

                                if (Mode != "eff")
                                {
                                    if (item.Cleaned == "3_months_not_cleaned")
                                    {
                                        ToolTip info = new ToolTip();

                                        string date = item.EvntDate == DateTime.MinValue ? "Unknown" : item.EvntDate.ToShortDateString();

                                        info.SetToolTip(lbl, "Hours: " + item.ClndHours + "\nDate: " + date);
                                        info.AutoPopDelay = 30000;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                _cleaned_machines.Clear();
                if (Mode == "rammendi" || Mode == "scarti")
                {
                    var dataTbl = new DataTable();
                    using (var con = new SqlConnection(MainWnd.conString))
                    {
                        var cmd = new SqlCommand("getscartiandramendi", con)
                        {
                            CommandType = CommandType.StoredProcedure
                        };

                        cmd.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = MainWnd.Get_from_date();
                        cmd.Parameters.Add("@toDate", SqlDbType.DateTime).Value = MainWnd.Get_to_date();
                        cmd.Parameters.Add("@shift", SqlDbType.NVarChar).Value = MainWnd.Get_shift_array().ToString();

                        _list_of_scarti_rammendi = new List<ScartiRamendi>();

                        con.Open();
                        var dr = cmd.ExecuteReader();
                        dataTbl.Load(dr);
                        con.Close();
                        dr.Close();
                    }
                    var qtyTable = new DataTable();
                    using (var con = new SqlConnection(MainWnd.conString))
                    {
                        var cmd = new SqlCommand("getmachineqty", con)
                        {
                            CommandType = CommandType.StoredProcedure
                        };

                        cmd.Parameters.Add("@finesse", SqlDbType.VarChar).Value = MainWnd.Get_fin_array().ToString();
                        cmd.Parameters.Add("@from_date", SqlDbType.DateTime).Value = MainWnd.Get_from_date();
                        cmd.Parameters.Add("@to_date", SqlDbType.DateTime).Value = MainWnd.Get_to_date();
                        cmd.Parameters.Add("@shift", SqlDbType.VarChar).Value = MainWnd.Get_shift_array().ToString();

                        if (string.IsNullOrEmpty(MainWnd.Get_file_name())) MainWnd.Set_file_name(string.Empty);
                        cmd.Parameters.Add("@file_name", SqlDbType.VarChar).Value = MainWnd.Get_file_name();

                        con.Open();

                        var dr = cmd.ExecuteReader();

                        qtyTable.Load(dr);

                        con.Close();
                        dr.Close();
                    }
                    if (dataTbl.Rows.Count >= 1)
                    {
                        var shift = dataTbl.Rows[0][0].ToString();
                        var operator_code = dataTbl.Rows[0][1].ToString();
                        int.TryParse(dataTbl.Rows[0][2].ToString(), out var machine);
                        var order = dataTbl.Rows[0][3].ToString();
                        var article = dataTbl.Rows[0][4].ToString();
                        int.TryParse(dataTbl.Rows[0][5].ToString(), out var tmpTeli);
                        int.TryParse(dataTbl.Rows[0][6].ToString(), out var tmpScarti);
                        int.TryParse(dataTbl.Rows[0][7].ToString(), out var tmpRammendi);
                        var sumTeli = 0;
                        var sumScarti = 0;
                        var sumRammendi = 0;
                        foreach (DataRow row in dataTbl.Rows)
                        {
                            var sh = row[0].ToString();
                            var operCode = row[1].ToString();
                            int.TryParse(row[2].ToString(), out var mac);
                            var ord = row[3].ToString();
                            var art = row[4].ToString();
                            int.TryParse(row[5].ToString(), out var teli);
                            int.TryParse(row[6].ToString(), out var scarti);
                            int.TryParse(row[7].ToString(), out var ramm);
                            if (machine != mac)
                            {
                                sumTeli += tmpTeli;
                                sumScarti += tmpScarti;
                                sumRammendi += tmpRammendi;
                                _list_of_scarti_rammendi.Add(new ScartiRamendi(machine, sumTeli, sumScarti, sumRammendi));
                                sumTeli = 0;
                                sumScarti = 0;
                                sumRammendi = 0;
                                shift = sh;
                                operator_code = operCode;
                                order = ord;
                                article = art;
                            }
                            if (order != ord || operator_code != operCode || shift != sh || article != art)
                            {
                                sumTeli += tmpTeli;
                                sumScarti += tmpScarti;
                                sumRammendi += tmpRammendi;
                            }
                            machine = mac;
                            shift = sh;
                            operator_code = operCode;
                            order = ord;
                            article = art;
                            tmpTeli = teli;
                            tmpRammendi = ramm;
                            tmpScarti = scarti;
                        }
                        sumTeli += tmpTeli;
                        sumScarti += tmpScarti;
                        sumRammendi += tmpRammendi;
                        _list_of_scarti_rammendi.Add(new ScartiRamendi(machine, sumTeli, sumScarti, sumRammendi));
                        foreach (DataRow row in qtyTable.Rows)
                        {
                            int.TryParse(row[0].ToString(), out var machineNr);
                            int.TryParse(row[1].ToString(), out var machineTeli);
                            var query = (from mac in _list_of_scarti_rammendi
                                         where mac.Machine == machineNr
                                         select mac).SingleOrDefault();
                            if (query != null)
                            {
                                query.TeliBuoni = machineTeli;
                            }
                        }
                    }
                }
                else
                {
                    using (var con = new SqlConnection(MainWnd.conString))
                    {
                        var cmd = new SqlCommand("get_data", con)
                        {
                            CommandType = CommandType.StoredProcedure
                        };

                        cmd.Parameters.Add("@finesse", SqlDbType.VarChar).Value = MainWnd.Get_fin_array().ToString();
                        cmd.Parameters.Add("@from_date", SqlDbType.DateTime).Value = MainWnd.Get_from_date();
                        cmd.Parameters.Add("@to_date", SqlDbType.DateTime).Value = MainWnd.Get_to_date();
                        cmd.Parameters.Add("@shift", SqlDbType.VarChar).Value = MainWnd.Get_shift_array().ToString();

                        if (string.IsNullOrEmpty(MainWnd.Get_file_name())) MainWnd.Set_file_name(string.Empty);
                        cmd.Parameters.Add("@file_name", SqlDbType.VarChar).Value = MainWnd.Get_file_name();

                        con.Open();

                        var dr = cmd.ExecuteReader();

                        _tbl_machines.Load(dr);

                        con.Close();
                        dr.Close();
                    }
                    GetMachineAlarms();
                    if (_machine_alarms.Count >= 1)
                        foreach (var mac in _list_of_currents)
                        {
                            if (_machine_alarms.ContainsKey(mac.MachineNumber)) mac.Alarm = _machine_alarms[mac.MachineNumber];
                            else mac.Alarm = false;
                        }
                }
            }
            //LoadingInfo.CloseLoading();

            _currentColors.Clear();
            foreach (var machine in _list_of_machines)
            {
                machine.BackColor = Color.LightGray;
                machine.Text = default(string);

                if (Mode != "cleaner")
                {
                    if (Mode == "rammendi")
                    {
                        int.TryParse(machine.Name.Remove(0, 1), out var idm);
                        var query = (from sm in _list_of_scarti_rammendi
                                     where sm.Machine == idm
                                     select sm).SingleOrDefault();
                        machine.Font = _fontBig;
                        if (query == null)
                        {
                            machine.Text = "";
                            continue;
                        }
                        if (MainWnd._teli_mode)
                        {
                            machine.Text = query.Rammendi.ToString();
                            if (query.TeliBuoni != 0)
                                machine.BackColor = GetSMEfficiencyColor(Math.Round((Convert.ToDecimal(query.Rammendi) /
                                                                         Convert.ToDecimal(query.TeliBuoni)) * 100, 1)
                                                                         .ToString());
                            else machine.BackColor = GetSMEfficiencyColor("100.0");
                        }
                        else
                        {
                            if (query.TeliBuoni != 0)
                            {
                                machine.Text = Math.Round((Convert.ToDecimal(query.Rammendi) / Convert.ToDecimal(query.TeliBuoni)) * 100, 1)
                                               .ToString() == "0" ? "0.0" :
                                               Math.Round((Convert.ToDecimal(query.Rammendi) / Convert.ToDecimal(query.TeliBuoni)) * 100, 1)
                                               .ToString();
                                machine.BackColor = GetSMEfficiencyColor(machine.Text);
                            }
                            else
                            {
                                machine.Text = "100.0";
                                machine.BackColor = GetSMEfficiencyColor(machine.Text);
                            }
                        }
                    }
                    else if (Mode == "scarti")
                    {
                        int.TryParse(machine.Name.Remove(0, 1), out var idm);
                        var query = (from sm in _list_of_scarti_rammendi
                                     where sm.Machine == idm
                                     select sm).SingleOrDefault();
                        machine.Font = _fontBig;
                        if (query == null)
                        {
                            machine.Text = "";
                            continue;
                        }
                        if (MainWnd._teli_mode)
                        {
                            machine.Text = query.Scarti.ToString();
                            if (query.TeliBuoni != 0)
                                machine.BackColor = GetSMEfficiencyColor(Math.Round((Convert.ToDecimal(query.Scarti) /
                                                                         Convert.ToDecimal(query.TeliBuoni)) * 100, 1)
                                                                         .ToString());
                            else machine.BackColor = GetSMEfficiencyColor("100.0");
                        }
                        else
                        {
                            if (query.TeliBuoni != 0)
                            {
                                machine.Text = Math.Round((Convert.ToDecimal(query.Scarti) / Convert.ToDecimal(query.TeliBuoni)) * 100, 1)
                                               .ToString() == "0" ? "0.0" :
                                               Math.Round((Convert.ToDecimal(query.Scarti) / Convert.ToDecimal(query.TeliBuoni)) * 100, 1)
                                               .ToString();
                                machine.BackColor = GetSMEfficiencyColor(machine.Text);
                            }
                            else
                            {
                                machine.Text = "100.0";
                                machine.BackColor = GetSMEfficiencyColor(machine.Text);
                            }
                        }
                    }
                    else
                    {                        
                        foreach (DataRow row in _tbl_machines.Rows)
                        {
                            if (machine.Name.Remove(0, 1) != row[0].ToString()) continue;

                            var ferme = TimeSpan.FromMinutes(Convert.ToDouble(row[4]));
                            var tempStd = TimeSpan.FromMinutes(Convert.ToDouble(row[3]));

                            var strFerme = ferme.ToString(@"h\:mm");
                            var strTempoStandard = tempStd.ToString(@"h\:mm");
                            var temp = string.Empty;

                            switch (Mode)
                            {
                                case "eff":
                                    var eff = string.Concat(Math.Round(Convert.ToDouble(row[1]), 0).ToString(), "  ");
                                    machine.Text = eff;
                                    machine.Font = _fontBig;
                                    break;
                                case "tempStd":
                                    machine.Text = strTempoStandard;
                                    machine.Font = _fontSmall;
                                    break;
                                case "ferme":
                                    machine.Text = strFerme;
                                    machine.Font = _fontBig;
                                    break;
                                case "qty":
                                    machine.Text = row[2].ToString() + "  ";
                                    machine.Font = _fontBig;
                                    break;
                            }

                            var excl_mac =
                                    MainWnd._listOfExcluedMachines
                                    .SingleOrDefault(m => m == machine.Name.Remove(0, 1));

                            machine.BackColor =
                                !string.IsNullOrEmpty(excl_mac)
                                ? machine.BackColor = Color.DimGray :
                                machine.BackColor = GetEfficiencyColor(row[1].ToString());

                            if (!_currentColors.ContainsKey(machine))
                                _currentColors.Add(machine, machine.BackColor);
                        }
                    }
                }
                else
                {
                    var days = (MainWnd.Get_to_date().Subtract(MainWnd.Get_from_date())).TotalDays;
                    if (days == 0)
                    {
                        foreach (var item in _list_of_cleaners)
                        {
                            var macId = item.Machine;
                            var initial = item.NameInitials;
                            var prod = item.Prodgen;

                            if (machine.Name.Remove(0, 1) != macId) continue;

                            machine.Text = initial + "/" + prod.ToString();
                            machine.Font = _fontBig;
                            machine.BackColor = Color.FromArgb(54, 180, 86);
                        }
                    }
                    else
                    {
                        foreach (var item in _list_of_cleaners)
                        {
                            var macId = item.Machine;
                            var prod = item.Prodgen;
                            var num_of_cleaners = string.Empty;

                            if (machine.Name.Remove(0, 1) != macId) continue;

                            if (_cleaners_per_machine.ContainsKey(int.Parse(macId)))
                                num_of_cleaners = _cleaners_per_machine[int.Parse(macId)].ToString();

                            var oldProd = 0;
                            if (machine.Text != string.Empty)
                            {
                                int.TryParse(machine.Text.Split('/')[1], out oldProd);
                                prod += oldProd;
                            }

                            machine.Text = num_of_cleaners + "/" + prod.ToString();
                            machine.Font = _fontBig;
                            machine.BackColor = Color.FromArgb(54, 180, 86);
                        }
                    }
                }
            }

            GetTotals();
            if (Mode == "tempStd" || Mode == "qty")
            {
                PostMachineColors();
                GetTotals();                
            }
            else if (Mode == "scarti" || Mode == "rammendi") GetTotals();

            foreach (var kvp in _total_eff)
            {
                kvp.Key.BackColor = kvp.Value;
            }

            foreach (var mchn in _list_of_machines)
            {
                if (!string.IsNullOrEmpty(mchn.Text)) continue;
                if (Mode == "temperature" || Mode == "scarti" || Mode == "rammendi") mchn.Text = "";
                else mchn.Text = Mode != "cleaner" ? "0  " : "";
            }

            //if (_tmColorSwitch.Enabled)
            //{
            //    _tmColorSwitch.Stop();
            //    _tmColorSwitch.Enabled = false;
            //}

            //if (!Get_is_excl_mode())
            //{
            //    _tmColorSwitch.Enabled = true;
            //    _tmColorSwitch.Start();
            //}

            lblFerme1.Visible = false;
            lblFerme2.Visible = false;
            lblFerme3.Visible = false;

            //btnStatusImg.Image = Properties.Resources.checkmark_30;
            //btnStatusImg.Tag = 0; //normal

            //_lastToDate = Get_to_date();

            //lblStatus.Text = "Ready";
            //btnStatusImg.Image = Properties.Resources.checkmark_30;
            //statusBar.Refresh();
            //IsAuto = false;

            //_lastDetectedShift = ResumeShift(false);

            //_groupBorderWidth = S1.Location.X + S1.Width + 50;

            if (_tmDelay != null) _tmDelay = null;

            //Invalidate(); //here

            //lblSelectedMode.Text = "Mode: " + Mode;
            //var fullMode = string.Empty;
            //if (Mode == "cleaner" || Mode == "scarti" || Mode == "rammendi")
            //    fullMode = Mode.Substring(0, 1).ToUpper() + Mode.Remove(0, 1);
            //else if (Mode == "eff") fullMode = "Efficiency";
            //else if (Mode == "qty") fullMode = "Quantity";
            //else if (Mode == "tempStd") fullMode = "Tempo Standard";

            //lbl_current_mode.Text = Mode != "cleaner" ? "Mode: " + fullMode :
            //                        "Mode: " + fullMode + "/ " + _cleanersType.Substring(0, 1).ToUpper() + _cleanersType.Remove(0, 1);
            Cursor = Cursors.Default;

            //refresh all labels
            foreach (Panel groupBox in new Panel[] { grp1, grp2, grp3 })
            {
                foreach (var item in (from lbl in groupBox.Controls.OfType<Label>()
                                      select lbl).ToList())
                {
                    item.Invalidate();
                    item.Update();
                }
            }
        }

        private void GetMachineAlarms()
        {
            _machine_alarms.Clear();

            using (var con = new SqlConnection(MainWnd.conString))
            {
                var cmd = new SqlCommand("get_machine_alarm", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //cmd.Parameters.Add("@idm", SqlDbType.Int).Value = mac;
                cmd.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = MainWnd.Get_from_date();
                cmd.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = MainWnd.Get_to_date();
                cmd.Parameters.Add("@shift", SqlDbType.VarChar, 200).Value = MainWnd.Get_shift_array().ToString();

                con.Open();
                var dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        var machine = dr[0].ToString();
                        bool.TryParse(dr[1].ToString(), out var alm);
                        if (!_machine_alarms.ContainsKey(machine))
                            _machine_alarms.Add(machine, alm);
                    }
                }
                con.Close();
                dr.Close();
            }
        }

        private void FrmLayout_Load(object sender, EventArgs e)
        {
            _firstEnter = true;

            //frm.Size = new Size(parent.Width, parent.Height);
            var w = Screen.GetWorkingArea(this).Width;
            var h = Screen.GetWorkingArea(this).Height;
            //frm.Size = new Size(parent.Width, parent.Height);
            float wi = w / 1920f;
            float hi = h / 1080f;

            var f = new SizeF(wi, hi);
            this.Scale(f);
            
            _list_of_currents = new List<CurrentInfo>();

            var cmd = new SqlCommand("get_current_info", MainWnd._sql_con)
            {
                CommandType = CommandType.StoredProcedure
            };

            MainWnd._sql_con.Open();

            var dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    var fileName = dr[2].ToString();

                    int.TryParse(dr[0].ToString(), out var idm);
                    _list_of_currents.Add(new CurrentInfo(dr[0].ToString(),
                        dr[1].ToString(), fileName, dr[3].ToString(), false));

                    if (!_fileNamesDict.ContainsKey(fileName))
                        _fileNamesDict.Add(fileName, dr[2].ToString());
                }
            }

            MainWnd._sql_con.Close();
            dr.Close();

            _lastFileText = "x";
            
            CreateObjectGroups();
            //else if(Mode == "scarti")
            GetData();
            
            TimerCallback tcb = UpdateInfo;
            var are = new AutoResetEvent(true);

            _timerLastUpdate = new System.Threading.Timer(tcb, are, 3000, 60000);   //1min check
        }
        private Color GetSMEfficiencyColor(string input)
        {
            double upperBound, lowerBound;
            if (Mode == "scarti")
            {
                upperBound = 2.0;
                lowerBound = 1.5;
            }
            else
            {
                upperBound = 3.5;
                lowerBound = 2.5;
            }

            var color = default(Color);
            double.TryParse(input, out var eff);
            if (eff > upperBound)
                color = Color.FromArgb(253, 129, 127);
            else if (eff >= lowerBound && eff <= upperBound)
                color = Color.FromArgb(254, 215, 1);
            else if (eff < lowerBound)
                color = Color.FromArgb(54, 214, 87);
            else color = Color.LightGray;
            return color;
        }
        private Color GetSMEfficiencyColor(string input, Color optColor)
        {
            double upperBound, lowerBound;
            if (Mode == "scarti")
            {
                upperBound = 2.0;
                lowerBound = 1.5;
            }
            else
            {
                upperBound = 3.5;
                lowerBound = 2.5;
            }

            var color = default(Color);
            double.TryParse(input, out var eff);
            if (eff == 0.0)
                color = optColor;
            else if (eff > upperBound)
                color = Color.FromArgb(253, 129, 127);
            else if (eff >= lowerBound && eff <= upperBound)
                color = Color.FromArgb(254, 215, 1);
            else if (eff < lowerBound)
                color = Color.FromArgb(54, 214, 87);
            else color = Color.LightGray;
            return color;
        }
        private string GetFileLastWrite()
        {
            var strDtm = string.Empty;
            var dtm = DateTime.MinValue;

            using (var con = new SqlConnection(MainWnd.conString))
            {
                var cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "getlastcsvfilename";
                cmd.Connection = con;

                con.Open();
                var dr = cmd.ExecuteReader();
                if (dr.HasRows)
                    while (dr.Read())
                    {
                        dtm = Convert.ToDateTime(dr[0]);
                    }
                con.Close();
                dr.Close();
                cmd = null;
            }

            strDtm = dtm.ToString("dd.MM.yyyy") + " " + dtm.ToString("HH:mm:ss");

            Thread.Sleep(3000);

            return "ult. agg: " + strDtm;
        }

        private bool _firstEnter;
        private string _lastFileText;
        private void UpdateInfo(object lastwrite)
        {
            if (_lastFileText != GetFileLastWrite())
            {
                var lastUpdate = GetFileLastWrite();
                _lastFileText = lastUpdate;
                if (!_firstEnter)
                {
                    StartDelayedLoading();
                }
            }
            _firstEnter = false;
        }

        private void PostMachineColors()
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
                    l.BackColor = k.Value;
                }
            }

            foreach (var b in _list_of_blocks)
            {
                foreach (KeyValuePair<Label, Color> k in MainWnd._total_eff)
                {
                    var name = k.Key.Name;
                    if (b.Name != name) continue;

                    b.BackColor = k.Value;
                }
            }
        }
    }
}
    
