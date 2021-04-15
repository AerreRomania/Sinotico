using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Sinotico
{
    public partial class MainWnd : Form
    {
        //public static string conString = "Data Source=DESKTOP-Q24FF5A;Initial Catalog=Sinotico;Integrated Security=SSPI;"; //Nikola
          public static string conString = "Data Source = 192.168.96.17; Initial Catalog = Sinotico; user=sa;password=onlyouolimpias;"; //Server
        //public static string conString = "Data Source = 192.168.1.10; Initial Catalog = Sinotico;Integrated Security=SSPI;"; //Server sergiu
        //public static string conString = "Data Source=KNSQL2014;Initial Catalog=Sinotico;Integrated Security=SSPI"; //Unused
        public static string conStringOY = "Data Source = 192.168.96.37; Initial Catalog = ONLYOU; user=sa;password=olimpiasknitting;";
        public static SqlConnection _sql_con = new SqlConnection(conString);
       

        /*
         * Objectivity
         */

        public static DateTimePicker _dateToPicker;
        public static DateTimePicker _dateFromPicker;

        public static CheckBox _sqOne;
        public static CheckBox _sqTwo;
        public static CheckBox _sqThree;

        private bool _mouseDown;
        private Point _lastLocation;

        private List<Squadra> _squadra_list = new List<Squadra>();
        private List<Label> _list_of_machines = new List<Label>();
        private List<Label> _list_of_lines = new List<Label>();
        private List<Label> _list_of_blocks = new List<Label>();
        private List<Button> _list_of_buttons = new List<Button>();
        private List<CurrentInfo> _list_of_currents = new List<CurrentInfo>();
        public static List<string> _listOfExcluedMachines = new List<string>();
        private List<MachinesProperties> _list_of_stopped_machines = new List<MachinesProperties>();
        public static List<string> ListOfSelectedShifts = new List<string>();
        private List<string> _listOfSelectedFinesses = new List<string>();
        private List<Cleaners> _list_of_cleaners = new List<Cleaners>();
        private List<CleanedMachines> _cleaned_machines = new List<CleanedMachines>();
        private List<Label> _mac_tooltips = new List<Label>();
        private int[,] _timeValues = new int[3, 4];
        List<Label> _totLabels = new List<Label>();
        private List<ScartiRamendi> _list_of_scarti_rammendi = new List<ScartiRamendi>();
        Dictionary<int, int> _teli = new Dictionary<int, int>();
        private List<Temperature> _hum_temp = new List<Temperature>();
        private List<Lines> _holidays = new List<Lines>();
        private List<Line> _articles_in_line = new List<Line>();

        //start: lists used only for GUI part
        private List<Button> _lstOfModes = new List<Button>();
        private List<Button> _listOfJobs = new List<Button>();
        //end: lists used only for GUI part

        private Dictionary<int, int> _cleaners_per_machine = new Dictionary<int, int>();
        private Dictionary<Button, Color> _mode_filters = new Dictionary<Button, Color>();
        public static Dictionary<Label, Color> _total_eff = new Dictionary<Label, Color>();
        public static Dictionary<string, string> _fileNamesDict = new Dictionary<string, string>();
        public static Dictionary<Label, Color> _currentColors = new Dictionary<Label, Color>();
        private Dictionary<Label, Color> _art_changes_colors = new Dictionary<Label, Color>();

        private ToolTip _tooltip_object = new ToolTip();
        private ToolTip _tooltip_controls = new ToolTip();
        public static readonly Geometry _geometry = new Geometry();
        private System.Timers.Timer _tmColorSwitch = new System.Timers.Timer();
        private System.Threading.Timer _timerLastUpdate = null;
        private static AutoResetEvent _load_Event = new AutoResetEvent(true);
        private System.Windows.Forms.Timer _tmExitFullScreen;

        /*
         *  Runetime properties
         */

        #region MouseOver

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);

        private enum MouseOver
        {
            Machine,
            Block,
            Total,
            Empty
        }

        //private MouseOver _mouseOver = new MouseOver();

        private static Label _mouseOverMachine1;
        private static Label _mouseOverMachine { get => _mouseOverMachine1; set => _mouseOverMachine1 = value; }

        private static string _mouseOverMachineNumber1;
        public static string _mouseOverMachineNumber
        {
            get => _mouseOverMachineNumber1;
            private set => _mouseOverMachineNumber1 = value;
        }

        #endregion

        public static bool Get_is_excl_mode()
        {
            return _is_excl_mode1;
        }

        private void Set_is_excl_mode(bool value) => _is_excl_mode1 = value;

        public static string _file_name1;

        public static string Get_file_name()
        {
            return _file_name1;
        }

        public static void Set_file_name(string value)
        {
            _file_name1 = value;
        }

        public static string Mode { get; set; }

        #region FilterArrays

        /* 1Hng
         ** SHIFTS
         */

        private static System.Text.StringBuilder _shift_array1;

        public static System.Text.StringBuilder Get_shift_array()
        {
            return _shift_array1;
        }

        public static void Set_shift_array(System.Text.StringBuilder value) => _shift_array1 = value;

        /* 2Hng
         ** FINESSES
         */

        private static System.Text.StringBuilder _fin_array1;

        public static System.Text.StringBuilder Get_fin_array()
        {
            return _fin_array1;
        }

        private static void Set_fin_array(System.Text.StringBuilder value) => _fin_array1 = value;

        #endregion

        private static DateTime _from_date1;

        public static DateTime Get_from_date()
        {
            return _from_date1;
        }

        public static void Set_from_date(DateTime value)
        {
            _from_date1 = value;
        }

        private static DateTime _to_date1;

        public static DateTime Get_to_date()
        {
            return _to_date1;
        }

        public static void Set_to_date(DateTime value)
        {
            _to_date1 = value;
        }

        private static string _tableSource;
        public static void SetTableSource(string value)
        {
            _tableSource = value;
           
        }
        public static string GetTableSource()
        {
            return _tableSource;
        }
        //private static string lastReceivedFile;

        /*
         * RUNTIME
         */
        private Panel _subMdiContainer;
        private MyMenuStrip _my_menu_strip = new MyMenuStrip(); //custom pop-up menu container 
        private MyMenuItem _my_menu_item;
        private List<string> _my_menu_items = new List<string>();
        string[] _items_collection = null;
        private Button _btnExitFullScreen;
        private Panel _pnForm = new Panel();
        private Label _lblBack = new Label();
        private Label _lblPdf = new Label();
        private Label _lblExcel = new Label();
        public static bool _isReportMode = false;
        private bool _sqlHasError = false;
        private bool _firstEnter;
        public static bool IsAuto { get; set; }
        private int _stopSq1, _stopSq2, _stopSq3;
        public static DateTime _lastToDate;
        private string _lastFileText, _lastDetectedShift;
        private int _groupBorderWidth;
        private string _cleanersType = string.Empty;
        /*
         * CONSTANTS
         */
        const int MAX_LINE = 14;
        const int WM_PARENTNOTIFY = 0x210;
        const int WM_LBUTTONDOWN = 0x201;
        const int MAX_WIDTH = 1920;
        const int MAX_HEIGHT = 1080;
        /*
         * OTHERS
         */
        private readonly Font _fontBig = new Font("Tahoma", 9, FontStyle.Bold);
        private readonly Font _fontSmall = new Font("Microsoft Sans Serif", 7, FontStyle.Bold);
        private readonly Font _fontBigSq = new Font("Microsoft Sans Serif", 15, FontStyle.Bold);
        private readonly Font _fontSmallSq = new Font("Microsoft Sans Serif", 12, FontStyle.Bold);
        private readonly Font _fontGraphicsDpi = new Font("Microsoft Sans Serif", 7, FontStyle.Bold);

        private int monitorWidth;
        private Brush _brush_active;
        private Thread _trdLoadData;

        private int GetMonitorWidth()
        {
            return monitorWidth;
        }

        private void SetMonitorWidth(int value)
        {
            monitorWidth = value;
        }

        private int monitorHeight;

        private int GetMonitorHeight()
        {
            return monitorHeight;
        }

        private void SetMonitorHeight(int value)
        {
            monitorHeight = value;
        }

        public MainWnd()
        {
            InitializeComponent();
        }

        #region UpdateMachines

        private void CallingFunction()
        {
            var methodInvoker = new MethodInvoker(CallProcedures);
            Invoke(methodInvoker);
        }

        private void GetData()
        {
            _trdLoadData = new Thread(new ThreadStart(CallingFunction))
            {
                Priority = ThreadPriority.Normal
            };

            _trdLoadData.TrySetApartmentState(ApartmentState.STA);

            if (_trdLoadData.ThreadState != ThreadState.Running)
                _trdLoadData.Start();

            grp1.Invalidate();
            grp2.Invalidate();
            grp3.Invalidate();
        }

        private Thread _trdLoadCharts;

        private void CallCharts()
        {
            var methodInvoker = new MethodInvoker(LoadCharts);
            Invoke(methodInvoker);

        }
        private void GetCharts()
        {
            _trdLoadCharts = new Thread(new ThreadStart(CallCharts))
            {
                //Name = "Updating layout data",
                Priority = ThreadPriority.Normal
            };

            //_trdLoadCharts.TrySetApartmentState(ApartmentState.STA);

            if (_trdLoadCharts.ThreadState != ThreadState.Running)
                _trdLoadCharts.Start();
        }

        private void bgvData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //LoadingInfo.CloseLoading();
        }

        private string GetFileLastWrite()
        {
            var strDtm = string.Empty;
            var dtm = DateTime.MinValue;

            using (var con = new SqlConnection(conString))
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

        private void UpdateInfo(object lastwrite)
        {
            if (_lastFileText != GetFileLastWrite())
            {
                pbLastUpdate.Enabled = true;
                pbLastUpdate.Visible = true;
                pbLastUpdate.Refresh();
                lblLastFile.Location = new Point(lblLastFile.Location.X + 20,
                    lblLastFile.Location.Y);

                var lastUpdate = GetFileLastWrite();
                lblLastFile.Text = lastUpdate;
                lblLastFile.Refresh();

                pbLastUpdate.Enabled = false;
                pbLastUpdate.Visible = false;
                lblLastFile.Location = new Point(lblLastFile.Location.X - 20,
                    lblLastFile.Location.Y);

                _lastFileText = lastUpdate;
                lblLastFile.ForeColor = Color.Blue;
                lblLastFile.Enabled = true;

                // Performs auto-update

                if (!_firstEnter
                    && !_isReportMode
                    && !_sqlHasError)
                {
                    IsAuto = true;

                    // Gets currrent shift if is new
                    var tryNewShift = ResumeShift(false);

                    if (_lastDetectedShift != tryNewShift)
                    {
                        ResumeShift(true);
                    }

                    StartDelayedLoading();

                    lblLastUpdateStatus.Text = "Last update: "
                        + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                }
            }

            if (_firstEnter)
            {
                lblLastFile.Enabled = false;
            }

            _firstEnter = false;
        }

        private void lblLastFile_MouseDown(object sender, MouseEventArgs e)
        {
            lblLastFile.ForeColor = Color.Red;
        }

        private void lblLastFile_MouseUp(object sender, MouseEventArgs e)
        {
            lblLastFile.ForeColor = Color.Blue;
        }

        #endregion

        private void CreateObjectGroups()
        {
            _list_of_machines = new List<Label>();
            _list_of_lines = new List<Label>();
            _list_of_blocks = new List<Label>();

            foreach (Panel groupBox in new Panel[] { grp1, grp2, grp3 })  //Goes through all sub-groups
            {
                // Collects all machines to their list

                //var grpH = 0.0;
                //grpH = groupBox.Height * resDivH;
                //groupBox.Height = Convert.ToInt32(grpH);

                if (!groupBox.Visible) continue;

                foreach (var machine in (from lbl in groupBox.Controls.OfType<Label>() //groupBox.Controls.OfType<Label>()
                                         where lbl.Name.Substring(0, 1) == "P"
                                         select lbl).ToList())
                {
                    //Set machine listeners

                    machine.MouseEnter -= Machine_MouseEnter;
                    machine.Click -= Machine_Click;
                    machine.DoubleClick -= Machine_DoubleClick;
                    machine.MouseEnter += Machine_MouseEnter;
                    machine.Click += Machine_Click;
                    machine.DoubleClick += Machine_DoubleClick;

                    machine.Font = new Font("Tahoma", 10);
                    machine.TextAlign = ContentAlignment.MiddleRight;

                    _list_of_machines.Add(machine);
                }

                // Collects all of the lines in their list    

                foreach (var line in (from lbl in groupBox.Controls.OfType<Label>()
                                      where lbl.Name.Substring(0, 1) == "L"
                                      select lbl).ToList())
                {
                    _list_of_lines.Add(line);
                }

                // Collects all blocks in their list

                foreach (var block in (from lbl in groupBox.Controls.OfType<Label>()
                                       where lbl.Name.Substring(0, 1) == "S"
                                       select lbl).ToList())
                {
                    _list_of_blocks.Add(block);
                }
                foreach (var chart in (from chr in groupBox.Controls.OfType<System.Windows.Forms.DataVisualization.Charting.Chart>()
                                       select chr).ToList())
                {
                    chart.Series.Clear();


                    //var series = new[] { "knitt", "comb", "manual", "yarn", "needle", "shock", "roller", "other" };

                    //foreach (var serie in series)
                    //{
                    chart.Series.Add("timeSerie");
                    //chart.Series.Add("ts");
                    //}

                    chart.Series[0].IsVisibleInLegend = false;
                    //chart.Legends[0].Position.Auto = false;
                    //chart.Legends[0].Position.X -= 5;
                    chart.Legends[0].BackColor = Color.FromArgb(230, 230, 230);
                    chart.Titles[0].Visible = false;
                    //if (Mode == "eff" || Mode == "scarti" || Mode == "rammendi")
                    //{
                    //chart.ChartAreas[0].InnerPlotPosition.Width = 70;
                    //chart.ChartAreas[0].InnerPlotPosition.Width = 70;
                    //chart.ChartAreas[0].InnerPlotPosition.X = 0;
                    //}
                    //chart.ChartAreas[0].InnerPlotPosition.Y = chart.ChartAreas[0].InnerPlotPosition.Y - 10;
                    chart.ChartAreas[0].BackColor = Color.FromArgb(230, 230, 230);
                    chart.ChartAreas[0].BorderColor = Color.FromArgb(248, 248, 248);
                    chart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(235, 235, 235);
                    chart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(235, 235, 235);
                    chart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gainsboro;
                    chart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gainsboro;
                    chart.ChartAreas[0].AxisX.MajorGrid.IntervalOffset = 20;
                    chart.ChartAreas[0].AxisX.LineColor = Color.FromArgb(235, 235, 235);
                    chart.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Gray;
                    chart.ChartAreas[0].AxisY.LineColor = Color.Silver;
                    chart.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Gray;
                    //chart.Width = 400;
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            _operatorMode = true;

            _listOfJobs = new List<Button>();
            _listOfJobs.Add(button6);
            _listOfJobs.Add(button5);
            _listOfJobs.Add(button4);
            _listOfJobs.Add(button1);
            _listOfJobs.Add(button2);
            _listOfJobs.Add(button9);
            _listOfJobs.Add(button7);
            _listOfJobs.Add(button8);
            _lstOfModes = new List<Button>();
            _lstOfModes.Add(btnSpeedMode);
            _lstOfModes.Add(btnQtyMode);
            _lstOfModes.Add(button11);
            _lstOfModes.Add(btnTimeMode);
            _lstOfModes.Add(btnEffMode);

            _dateToPicker = dtpTo;
            _dateFromPicker = dtpFrom;
            SetTableSource(Properties.Settings.Default.TableSource);
            _sqOne = cb_SQ_1;
            _sqTwo = cb_SQ_2;
            _sqThree = cb_SQ_3;

            this.BackColor = Color.FromArgb(248, 248, 248);
            // Gets the screen resolution
            SetMonitorWidth(Screen.GetWorkingArea(this).Width);
            SetMonitorHeight(Screen.GetWorkingArea(this).Height);
            // Gets the width and height ratio by default resolution
            float widthRatio = GetMonitorWidth() / 1920f;
            float heightRatio = GetMonitorHeight() / 1080f;

            // Sets the scale changing factor
            SizeF factor
                = new SizeF(widthRatio, heightRatio);
            Scale(factor);

            MaximizedBounds = Screen.PrimaryScreen.WorkingArea;
            WindowState = FormWindowState.Maximized;

            try
            {
                //var artFilePath = "\\KNSQL2014\\Utility\\Nicu\\ArtFileChange.exe";
                var fileNameChangeIco = Icon.ExtractAssociatedIcon(AppDomain.CurrentDomain.BaseDirectory + @"\" + "ArtFileChange.exe");
                pbOpenExtensionRenamer.Image = fileNameChangeIco.ToBitmap();
                pbOpenExtensionRenamer.Click += delegate
                    {
                        System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + @"\" + "ArtFileChange.exe");
                    };
            }
            catch (UnauthorizedAccessException aex)
            {
                pbOpenExtensionRenamer.Image = null;
                MessageBox.Show(aex.Message, aex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                pbOpenExtensionRenamer.Image = null;
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Enabled = false;
            Cursor = Cursors.WaitCursor;
            this.DoubleBufferedForm(true);
            _firstEnter = true;
            //this.mouseMessageFilter = new MouseMoveMessageFilter();
            //this.mouseMessageFilter.TargetForm = this;
            //Application.AddMessageFilter(this.mouseMessageFilter);
            CheckForIllegalCrossThreadCalls = false;

            _tooltip_controls = new ToolTip
            {
                AutoPopDelay = 5000,
                InitialDelay = 200,
                StripAmpersands = true,
                UseAnimation = false
            };

            _tooltip_controls.SetToolTip(cbNight, "Shift 1 - 23:00 - 07:00");
            _tooltip_controls.SetToolTip(cbMorning, "Shift 2 - 07:00 - 15:00");
            _tooltip_controls.SetToolTip(cbAfternoon, "Shift 3 - 15:00 - 23:00");
            _tooltip_controls.SetToolTip(btnFilterby, "Filtering states");
            _tooltip_controls.SetToolTip(btnPrint, "Export layout to PDF (:A3)");
            _tooltip_controls.SetToolTip(btnReport, "Reports");
            _tooltip_controls.SetToolTip(btnCalendar, "Web sensors");
            _tooltip_controls.SetToolTip(lblTotDep, "Department efficiency");
            _tooltip_controls.SetToolTip(btnWebApp, "Split screen on minimum one and maximum four window states");
            _tooltip_controls.SetToolTip(S1, "Block 1");
            _tooltip_controls.SetToolTip(S2, "Block 2");
            _tooltip_controls.SetToolTip(S3, "Block 3");
            _tooltip_controls.SetToolTip(lblLastFile, "Download the latest data by clicking on the link");
            _tooltip_controls.SetToolTip(btn_temp_t, "Temperature state");
            _tooltip_controls.SetToolTip(btn_temp_u, "Humidity state");
            _tooltip_controls.SetToolTip(btnResetState, "Load current date data");
            _tooltip_controls.SetToolTip(button10, "Print current opened layout or report");
            _tooltip_controls.SetToolTip(btnBarChart, "Load charts per squadra for current state");
            _tooltip_controls.SetToolTip(cb_SQ_1, "Hide block 1");
            _tooltip_controls.SetToolTip(cb_SQ_2, "Hide block 2");
            _tooltip_controls.SetToolTip(cb_SQ_3, "Hide block 3");
            _tooltip_controls.SetToolTip(cbGreen, "Show machines in green efficiency zone");
            _tooltip_controls.SetToolTip(cbYellow, "Show machines in yellow efficiency zone");
            _tooltip_controls.SetToolTip(cbRed, "Show machines in red efficiency zone");
            _tooltip_controls.SetToolTip(cbMostra, "Show machines in each efficiency zone");
            _tooltip_controls.SetToolTip(cbPercent, "Show percentages total state");
            _tooltip_controls.SetToolTip(cbTeli, "Show quantity values in scarti or rammendi state");
            _tooltip_controls.SetToolTip(lbl_datamissing, "No missing data");

            //Set optional date points

            Set_from_date(DateTime.Now);
            Set_to_date(DateTime.Now);

            lblFerme1.Visible = false;
            lblFerme2.Visible = false;
            lblFerme3.Visible = false;

            cboArt.DropDownWidth = 100;
            cboArt.DropDownStyle = ComboBoxStyle.DropDownList;
            cboArt.DropDownHeight = 200;
            // Gets machines community for each block

            Set_shift_array(new System.Text.StringBuilder());
            Set_fin_array(new System.Text.StringBuilder());

            // Set default values

            Mode = "eff";

            Set_is_excl_mode(false);

            _fileNamesDict.Add("<All>", "");

            Get_fin_array().Append(",3,7,14,");

            _listOfSelectedFinesses.AddRange(new[] { "cbFin3", "cbFin7", "cbFin14" });

            /*
             ** Marking the status after activation
             */

            try
            {
                LoadingInfo.InfoText = "Trying to connect to the network...\n      http://192.168.96.17 onlyouolimpias";
                LoadingInfo.ShowLoading();

                _brush_active = Brushes.LightGray;

                _sql_con.Open();

                Graphics g = CreateGraphics();

                foreach (Panel groupBox in new Panel[] { grp1, grp2, grp3 })
                {
                    // Set border to the objects
                    // if application succsessfully has been connected to sql server

                    foreach (var item in (from lbl in groupBox.Controls.OfType<Label>()
                                          select lbl).ToList())
                    {
                        item.Paint += Labels_Paint;
                    }
                }

                lblTotDep.Paint += lblTotDep_Paint;

                _sql_con.Close();
                _sqlHasError = false;
                _list_of_currents = new List<CurrentInfo>();

                var cmd = new SqlCommand("get_current_info", _sql_con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _sql_con.Open();

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

                cboArt.DataSource = new BindingSource(_fileNamesDict, dataMember: null);
                cboArt.DisplayMember = "Key";
                cboArt.ValueMember = "Value";
                cboArt.SelectedIndex = 0;

                _sql_con.Close();
                dr.Close();

                // Add secondary handler for side buttons
                // that will represents backcolor of current state

                _list_of_buttons = (from btn in pnFilterby.Controls.OfType<Button>()
                                    select btn).ToList();

                foreach (var button in _list_of_buttons)
                {
                    button.Click += SideButtonParser_ClickForBackColor;

                    if (button.BackColor == Color.Gray)
                    {
                        if (!_mode_filters.ContainsKey(button)) _mode_filters.Add(button, button.BackColor);
                    }
                }

                ResumeShift(true);

                //Tests to see if new file was extracted to database

                _lastFileText = "x";

                LoadingInfo.UpdateText("Initializing reports...");

                CreateReportPanel();
                CreateExitFullScreenButton();

                if (_btnExitFullScreen != null)
                {
                    _btnExitFullScreen.Location = new Point(Width / 2 - 25, -60);
                }

                LoadingInfo.UpdateText("Creating object groups...");

                CreateObjectGroups();

                GetData();

                _timerLastUpdate?.Dispose();

                TimerCallback tcb = UpdateInfo;
                var are = new AutoResetEvent(true);

                _timerLastUpdate = new System.Threading.Timer(tcb, are, 3000, 60000);   //1min check
            }
            catch (SqlException sqlEx)
            {
                LoadingInfo.CloseLoading();

                MessageBox.Show("Server connection error!" + '\n' + '\n' + sqlEx.Message);

                _brush_active = Brushes.Coral;

                foreach (Panel groupBox in new Panel[] { grp1, grp2, grp3 })
                {
                    // Set border to the objects
                    // if application is succsessfully connected to the server

                    foreach (var item in (from lbl in groupBox.Controls.OfType<Label>()
                                          select lbl).ToList())
                    {
                        item.Paint += Labels_Paint;
                        item.Text = "<no funct>";
                        item.Font = _fontSmall;
                    }
                }

                lblTotDep.Paint += lblTotDep_Paint;

                pnFilterby.Enabled = false;
                btnReport.Enabled = false;

                _sqlHasError = true;

                btnStatusImg.Image = Properties.Resources.error_30;
                btnStatusImg.Tag = -1; //err
                lblStatus.Text = "Server connection failed";
            }

            this.Enabled = true;
            Cursor = Cursors.Default;
            _squadra_group.Add(grp1);
            _squadra_group.Add(grp2);
            _squadra_group.Add(grp3);

            cbMostra.Checked = true;
            this.BackColor = Color.FromArgb(230, 230, 230);

            foreach (var grp in new Panel[] { grp1, grp2, grp3 })
            {
                var tempLst = (from lbl in grp.Controls.OfType<Label>()
                               where lbl.Name.Substring(0, 1) == "_"
                               select lbl).ToList();
                foreach (var element in tempLst)
                {
                    _totLabels.Add(element);
                }
            }

            if(Properties.Settings.Default.checkUpdates)
                CheckForUpdates(null);
            
            base.OnLoad(e);
        }

        private void CheckForMissingData()
        {

        }
        private void btnResetState_Click(object sender, EventArgs e)
        {
            if (_sqlHasError || _isReportMode) return;

            dtpFrom.Value = DateTime.Now;
            dtpTo.Value = DateTime.Now;

           ResumeShift(true);
           // CallProcedures();
            StartDelayedLoading();
        }

        #region Shift

        private string ResumeShift(bool perform)
        {
            var tmpShift = "";

            if (perform)
            {
                Set_shift_array(new StringBuilder(""));
                ListOfSelectedShifts.Clear();
                cbNight.Checked = false;
                cbMorning.Checked = false;
                cbAfternoon.Checked = false;
            }

            // Recognize current time of day to compare with shifts time interval
            var curTime = DateTime.Now.TimeOfDay;

            var nightShiftStart = new TimeSpan(23, 0, 0);
            var nightShiftEnd = new TimeSpan(7, 0, 0);
            var morningShiftStart = new TimeSpan(7, 0, 0);
            var morningShiftEnd = new TimeSpan(15, 0, 0);
            var afternShiftStart = new TimeSpan(15, 0, 0);
            var afternShiftEnd = new TimeSpan(23, 0, 0);

            // Recognize shifts using current times

            if (nightShiftStart > nightShiftEnd && curTime > nightShiftStart || curTime < nightShiftEnd)
            {
                tmpShift = "cbNight";
                if (perform)
                {
                    cbNight.CheckState = CheckState.Checked;
                    Get_shift_array().Append(",Night,");
                    ListOfSelectedShifts.Add("cbNight");
                }
            }
            else if (curTime > morningShiftStart && curTime < morningShiftEnd)
            {
                tmpShift = "cbMorning";
                if (perform)
                {
                    cbMorning.CheckState = CheckState.Checked;
                    Get_shift_array().Append(",Morning,");
                    ListOfSelectedShifts.Add("cbMorning");
                }
            }
            else if (curTime > afternShiftStart && curTime < afternShiftEnd)
            {
                tmpShift = "cbAfternoon";
                if (perform)
                {
                    cbAfternoon.CheckState = CheckState.Checked;
                    Get_shift_array().Append(",Afternoon,");
                    ListOfSelectedShifts.Add("cbAfternoon");
                }
            }

            return tmpShift;
        }

        //private CheckBox _cbShifts;
        //private void Shifts_Click(object sender, EventArgs e)
        //{
        //    _cbShifts = (CheckBox)sender;

        //    if (_sqlHasError || _isReportMode) return;

        //    Set_file_name(string.Empty);

        //    if (_cbShifts.Checked)
        //    {
        //        // Add checked shift 
        //        if (!ListOfSelectedShifts.Contains(_cbShifts.Name))
        //            ListOfSelectedShifts.Add(_cbShifts.Name);

        //        _cbShifts.Image = Properties.Resources.new_mach_gif;
        //    }
        //    else
        //    {
        //        // Remove unchecked shift
        //        if (ListOfSelectedShifts.Contains(_cbShifts.Name))
        //            ListOfSelectedShifts.Remove(_cbShifts.Name);

        //        _cbShifts.Image = Properties.Resources.new_mach_png;
        //    }

        //    // Build string that will contains array of shifts listed above

        //    Get_shift_array().Clear();
        //    Get_shift_array().Append(",");

        //    foreach (var item in ListOfSelectedShifts)
        //    {
        //        Get_shift_array().Append(item.Remove(0, 2) + ",");   //remove "cb" prefix
        //    }

        //    StartDelayedLoading();
        //}

        #endregion Shift

        private void CallProcedures()
        {
            SetupEfficiencyBounds();
            if (Get_from_date() > DateTime.Now || Get_to_date() > DateTime.Now) return;

            Set_from_date(new DateTime(dtpFrom.Value.Year, dtpFrom.Value.Month, dtpFrom.Value.Day));
            Set_to_date(new DateTime(dtpTo.Value.Year, dtpTo.Value.Month, dtpTo.Value.Day));

            var ts = (Get_to_date().Subtract(Get_from_date()));

            if (ts.TotalDays < 0)
            {
                MessageBox.Show("Invalid date selection.");
                return;
            }

            if (!Get_is_excl_mode())
            {
                _listOfExcluedMachines.Clear();
            }

            ResetGlobals();

            if (string.IsNullOrEmpty(Get_file_name())) Set_file_name(string.Empty);

            _tbl_machines = new DataTable();
            var _article_changes = new DataSet();
            string fullMode = string.Empty;
            Cursor = Cursors.WaitCursor;

            if (Mode != "cleaner" && Mode != "temperature")
            {
                LoadingInfo.InfoText = "Loading data...";
                LoadingInfo.ShowLoading();
            }
            if (Mode != "temperature")
            {
                foreach (var line in _list_of_lines)
                    line.ForeColor = Color.FromArgb(100, 100, 100);
                foreach (var block in _list_of_blocks)
                    block.ForeColor = Color.FromArgb(100, 100, 100);
            }

            _list_of_cleaners = new List<Cleaners>();
            string tempFormat = String.Format("{0}{1}", "F", 1);

            if (Mode == "cleaner")
            {
                _cleaners_per_machine.Clear();
                var _dataSet = new DataSet();

                using (var con = new SqlConnection(conString))
                {
                    var cmd = new SqlCommand("getcleanersession", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.Add("@fromdate", SqlDbType.DateTime).Value = Get_from_date();
                    cmd.Parameters.Add("@todate", SqlDbType.DateTime).Value = Get_to_date();
                    cmd.Parameters.Add("@shift", SqlDbType.NVarChar).Value = Get_shift_array().ToString();
                    cmd.Parameters.Add("@type", SqlDbType.NVarChar).Value = _cleanersType;
                    cmd.Parameters.Add("@oneDayOnly",
                        SqlDbType.Bit).Value = Get_from_date().Equals(Get_to_date()) ? true : false;

                    var da = new SqlDataAdapter(cmd);
                    var ds = new DataSet();
                    da.Fill(_dataSet);
                    da.Dispose();

                    if (_cleanersType == "cquality")
                    {
                        fullMode = "Control Quality";
                        _cleaned_machines.Clear();
                        _mac_tooltips.Clear();
                        _machine_alarms.Clear();
                        foreach (var mac in _list_of_currents)
                        {
                            mac.Alarm = false;
                        }
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
                        GetMachineAlarms();
                        if (_machine_alarms.Count >= 1)
                            foreach (var mac in _list_of_currents)
                            {
                                if (_machine_alarms.ContainsKey(mac.MachineNumber)) mac.Alarm = _machine_alarms[mac.MachineNumber];
                                //else mac.Alarm = false;
                            }
                    }
                    else
                    {
                        fullMode = _cleanersType;
                        _machine_alarms.Clear();
                        foreach (var mac in _list_of_currents)
                        {
                            mac.Alarm = false;
                        }
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
                                if (!Get_to_date().Equals(Get_from_date()))
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

                        _mac_tooltips.Clear();
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
                _machine_alarms.Clear();
                if (Mode == "rammendi" || Mode == "scarti")
                {
                    fullMode = Mode.Substring(0, 1).ToUpper() + Mode.Remove(0, 1);
                    var dataTbl = new DataTable();
                    using (var con = new SqlConnection(conString))
                    {
                        var cmd = new SqlCommand("getscartiandramendi", con)
                        {
                            CommandType = CommandType.StoredProcedure
                        };

                        cmd.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = Get_from_date();
                        cmd.Parameters.Add("@toDate", SqlDbType.DateTime).Value = Get_to_date();
                        cmd.Parameters.Add("@shift", SqlDbType.NVarChar).Value = Get_shift_array().ToString();

                        _list_of_scarti_rammendi = new List<ScartiRamendi>();

                        con.Open();
                        var dr = cmd.ExecuteReader();
                        dataTbl.Load(dr);
                        con.Close();
                        dr.Close();
                    }
                    var qtyTable = new DataTable();
                    using (var con = new SqlConnection(conString))
                    {
                        var cmd = new SqlCommand("get_data", con)
                        {
                            CommandType = CommandType.StoredProcedure
                        };

                        cmd.Parameters.Add("@finesse", SqlDbType.VarChar).Value = Get_fin_array().ToString();
                        cmd.Parameters.Add("@from_date", SqlDbType.DateTime).Value = Get_from_date();
                        cmd.Parameters.Add("@to_date", SqlDbType.DateTime).Value = Get_to_date();
                        cmd.Parameters.Add("@shift", SqlDbType.VarChar).Value = Get_shift_array().ToString();

                        if (string.IsNullOrEmpty(Get_file_name())) Set_file_name(string.Empty);
                        cmd.Parameters.Add("@file_name", SqlDbType.VarChar).Value = Get_file_name();

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
                        //int.TryParse(dataTbl.Rows[0][5].ToString(), out var tmpTeli);
                        int.TryParse(dataTbl.Rows[0][5].ToString(), out var tmpScarti);
                        int.TryParse(dataTbl.Rows[0][6].ToString(), out var tmpRammendi);
                        DateTime.TryParse(dataTbl.Rows[0][7].ToString(), out var startDate);
                        //var sumTeli = 0;
                        var sumScarti = 0;
                        var sumRammendi = 0;
                        foreach (DataRow row in dataTbl.Rows)
                        {
                            var sh = row[0].ToString();
                            var operCode = row[1].ToString();
                            int.TryParse(row[2].ToString(), out var mac);
                            var ord = row[3].ToString();
                            var art = row[4].ToString();
                            //int.TryParse(row[5].ToString(), out var teli);
                            int.TryParse(row[5].ToString(), out var scarti);
                            int.TryParse(row[6].ToString(), out var ramm);
                            DateTime.TryParse(row[7].ToString(), out var date);
                            if (machine != mac)
                            {
                                //sumTeli += tmpTeli;
                                sumScarti += tmpScarti;
                                sumRammendi += tmpRammendi;
                                _list_of_scarti_rammendi.Add(new ScartiRamendi(machine, 0, sumScarti, sumRammendi));
                                //sumTeli = 0;
                                sumScarti = 0;
                                sumRammendi = 0;
                                shift = sh;
                                operator_code = operCode;
                                order = ord;
                                article = art;
                                startDate = date;
                            }
                            if (order != ord || operator_code != operCode || shift != sh || article != art || startDate != date)
                            {
                                //sumTeli += tmpTeli;
                                sumScarti += tmpScarti;
                                sumRammendi += tmpRammendi;
                            }
                            machine = mac;
                            shift = sh;
                            operator_code = operCode;
                            order = ord;
                            article = art;
                            //tmpTeli = teli;
                            tmpRammendi = ramm;
                            tmpScarti = scarti;
                            startDate = date;
                        }
                        //sumTeli += tmpTeli;
                        sumScarti += tmpScarti;
                        sumRammendi += tmpRammendi;
                        _list_of_scarti_rammendi.Add(new ScartiRamendi(machine, 0, sumScarti, sumRammendi));

                        foreach (DataRow row in qtyTable.Rows)
                        {
                            int.TryParse(row[0].ToString(), out var machineNr);
                            int.TryParse(row[2].ToString(), out var machineTeli);
                            var query = (from mac in _list_of_scarti_rammendi
                                         where mac.Machine == machineNr
                                         select mac).SingleOrDefault();
                            if (query != null)
                                query.TeliBuoni = machineTeli;
                        }
                    }
                }
                else if (Mode == "articlechanges")
                {
                    fullMode = "Article Changes";
                    _article_changes = new DataSet();
                    _articles_in_line = new List<Line>();
                    _squadra_list = new List<Squadra>();
                    using (var con = new SqlConnection(conString))
                    {
                        var cmd = new SqlCommand("getartchangespermachine", con)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        cmd.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = Get_from_date();
                        cmd.Parameters.Add("@toDate", SqlDbType.DateTime).Value = Get_to_date();
                        cmd.Parameters.Add("@shift", SqlDbType.NVarChar).Value = Get_shift_array().ToString();
                        con.Open();
                        var da = new SqlDataAdapter(cmd);
                        da.Fill(_article_changes);
                        da.Dispose();
                        con.Close();
                    }
                    var fromMachine = 1;
                    var toMachine = 14;
                    foreach (var line in new string[] { "Line 1", "Line 2", "Line 3", "Line 4", "Line 5", "Line 6", "Line 7",
                                                       "Line 8", "Line 9", "Line 10", "Line 11", "Line 12", "Line 13",
                                                       "Line 14", "Line 15" })
                    {
                        var newLine = new Line()
                        {
                            LineName = line,
                            FromMachine = fromMachine,
                            ToMachine = toMachine
                        };
                        fromMachine += 14;
                        toMachine += 14;
                        _articles_in_line.Add(newLine);
                    }
                    var sqFromMachine = 1;
                    var sqToMachine = 70;
                    foreach (var squadra in new string[] { "Squadra 1", "Squadra 2", "Squadra 3" })
                    {
                        var newSquadra = new Squadra()
                        {
                            SquadraName = squadra,
                            FromMachine = sqFromMachine,
                            ToMachine = sqToMachine
                        };
                        _squadra_list.Add(newSquadra);
                        sqFromMachine += 70;
                        sqToMachine += 70;
                    }
                    foreach (DataRow row in _article_changes.Tables[1].Rows)
                    {
                        int.TryParse(row[0].ToString(), out var mac);
                        var line = (from l in _articles_in_line
                                    where l.FromMachine <= mac && l.ToMachine >= mac
                                    select l).SingleOrDefault();
                        line.InsertArticle(row[1].ToString());
                    }
                    foreach (DataRow row in _article_changes.Tables[2].Rows)
                    {
                        var article = row[0].ToString();
                        var squadra = row[1].ToString();
                        var sq = (from s in _squadra_list
                                  where s.SquadraName == squadra
                                  select s).SingleOrDefault();
                        sq.InsertArticle(article);
                    }
                }
                else if (Mode == "PCPPST")
                {
                    fullMode = "Stop Time";
                    _tbl_machines = new DataTable();
                    using (var con = new SqlConnection(conString))
                    {
                        var cmd = new SqlCommand("getdatainholdpermachine", con)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        cmd.Parameters.Add("@from_date", SqlDbType.DateTime).Value = Get_from_date();
                        cmd.Parameters.Add("@to_date", SqlDbType.DateTime).Value = Get_to_date();
                        cmd.Parameters.Add("@shift", SqlDbType.NVarChar).Value = Get_shift_array().ToString();
                        con.Open();
                        var dr = cmd.ExecuteReader();
                        _tbl_machines.Load(dr);
                        dr.Dispose();
                        con.Close();
                    }
                }
                else if (Mode == "temperature")
                {
                    if (_tempMode == "temp") fullMode = "Temperature";
                    else if (_tempMode == "hum") fullMode = "Humidity";
                    _hum_temp = new List<Temperature>();
                    using (var con = new SqlConnection(conString))
                    {
                        var cmd = new SqlCommand("get_hum_temp", con)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        cmd.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = Get_from_date();
                        cmd.Parameters.Add("@toDate", SqlDbType.DateTime).Value = Get_to_date();
                        cmd.Parameters.Add("@shift", SqlDbType.NVarChar).Value = Get_shift_array().ToString();
                        con.Open();
                        var dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                string lineNum = dr[0].ToString().Split(' ')[1];
                                var cStr = string.Concat("L", lineNum);
                                double.TryParse(dr[1].ToString(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var tem);
                                double.TryParse(dr[2].ToString(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var hum);
                                _hum_temp.Add(new Temperature(cStr, tem, hum));
                            }
                        }
                        con.Close();
                        dr.Close();
                    }
                }
                else
                {
                    if (Mode == "eff")
                    {
                        List<Label> _operatorsFields = new List<Label>();
                        foreach (Label lab in new Label[] { Operator1, Operator2, Operator3, Operator4, Operator5, Operator6,
                                Operator7, Operator8, Operator9, Operator10, Operator11, Operator12, Operator13, Operator14, Operator15})
                        {
                            _operatorsFields.Add(lab);
                        }

                        if (_operatorMode)
                        {
                            using (var con = new SqlConnection(conString))
                            {
                                var cmd = new SqlCommand("get_online_operators", con)
                                {
                                    CommandType = CommandType.StoredProcedure
                                };

                                cmd.Parameters.Add("@eventDateTime", SqlDbType.DateTime).Value = Get_to_date();
                                var shift = Get_shift_array().ToString().Split(',')[1];
                                cmd.Parameters.Add("@shift", SqlDbType.NVarChar).Value = shift;
                                con.Open();
                                var dr = cmd.ExecuteReader();
                                
                                if(dr.HasRows)
                                    while(dr.Read())
                                    {
                                        var operatorField = (from l in _operatorsFields
                                                             where l.Name.Remove(0, 8) == dr[1].ToString().Remove(0, 5)
                                                             select l).SingleOrDefault();
                                        operatorField.Text = dr[0].ToString();
                                    }
                                con.Close();
                                dr.Close();
                            }
                        }
                        else
                        {
                            foreach (var label in _operatorsFields)
                                label.Text = string.Empty;
                        }
                        _art_changes_colors.Clear();
                        fullMode = "Efficiency";
                    }
                    else if (Mode == "qty")
                        fullMode = "Quantity";
                    else if (Mode == "tempStd")
                        fullMode = "Tempo Standard";
                    using (var con = new SqlConnection(conString))
                    {
                        var cmd = new SqlCommand("get_data", con)
                        {
                            CommandType = CommandType.StoredProcedure
                        };

                        cmd.Parameters.Add("@finesse", SqlDbType.VarChar).Value = Get_fin_array().ToString();
                        cmd.Parameters.Add("@from_date", SqlDbType.DateTime).Value = Get_from_date();
                        cmd.Parameters.Add("@to_date", SqlDbType.DateTime).Value = Get_to_date();
                        cmd.Parameters.Add("@shift", SqlDbType.VarChar).Value = Get_shift_array().ToString();

                        if (string.IsNullOrEmpty(Get_file_name())) Set_file_name(string.Empty);
                        cmd.Parameters.Add("@file_name", SqlDbType.VarChar).Value = Get_file_name();

                        con.Open();

                        var dr = cmd.ExecuteReader();
                        if(!dr.HasRows) 
                        {
                           
                            lbl_datamissing.Visible = true;
                            lbl_datamissing.Text = "Missing data from SPR3!";
                            _tooltip_controls.SetToolTip(lbl_datamissing, "Data is missing for shift: "+Get_shift_array().ToString().Replace(",","")+". \n Data is from last good shift.");
                           
                            _tbl_machines.Load(dr);

                            con.Close();
                            dr.Close();
                        }
                        else
                        {
                            lbl_datamissing.Visible = false;
                            _tbl_machines.Load(dr);
                            con.Close();
                            dr.Close();
                        }
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

            _currentColors.Clear();
            foreach (var machine in _list_of_machines)
            {
                machine.BackColor = Color.LightGray;
                machine.Text = default(string);

                if (Mode != "cleaner")
                {
                    if (Mode == "temperature")
                    {
                        foreach (var mac in _list_of_machines)
                        {
                            mac.BackColor = Color.FromArgb(211, 211, 211);
                            mac.Text = string.Empty;
                        }
                        foreach (var line in _list_of_lines)
                        {
                            line.ForeColor = Color.White;
                            line.BackColor = _tempColor;
                            var q = (from t in _hum_temp
                                     where t.Line == line.Name
                                     select t).SingleOrDefault();
                            if (q != null)
                            {
                                if (_tempMode == "temp")
                                    line.Text = q.Temp.ToString(tempFormat, CultureInfo.InvariantCulture);
                                else line.Text = q.Hum.ToString(tempFormat, CultureInfo.InvariantCulture);
                            }
                            else
                                line.Text = string.Empty;
                        }

                        foreach (var block in _list_of_blocks)
                        {
                            block.BackColor = _tempColor;
                            block.ForeColor = Color.White;
                            block.Text = string.Empty;
                        }
                    }
                    else if (Mode == "articlechanges")
                    {
                        foreach (DataRow row in _article_changes.Tables[0].Rows)
                        {
                            if (machine.Name.Remove(0, 1) != row[0].ToString()) continue;
                            int.TryParse(row[1].ToString(), out var articles);
                            machine.Text = (articles - 1).ToString();
                        }
                    }
                    else if (Mode == "PCPPST")
                    {
                        foreach (DataRow row in _tbl_machines.Rows)
                        {
                            if (machine.Name.Remove(0, 1) != row[0].ToString()) continue;
                            int.TryParse(row[1].ToString(), out var seconds);
                            machine.Text = ConvertSecondsToHHmm(seconds);
                            machine.Font = _fontSmall;
                        }
                    }
                    else if (Mode == "rammendi")
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
                        if (_teli_mode)
                        {
                            machine.Text = query.Rammendi.ToString();
                            if (query.TeliBuoni != 0)
                                machine.BackColor = GetSMEfficiencyColor(Math.Round((Convert.ToDecimal(query.Rammendi) /
                                                                         Convert.ToDecimal(query.TeliBuoni)) * 100, 1)
                                                                         .ToString());
                            else
                            {
                                if (query.Rammendi != 0)
                                    machine.BackColor = GetSMEfficiencyColor("100.0");
                                else
                                    machine.BackColor = Color.LightGray;
                            }
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
                                if (query.Rammendi != 0)
                                {
                                    machine.Text = "100.0";
                                    machine.BackColor = GetSMEfficiencyColor(machine.Text);
                                }
                                else
                                {
                                    machine.Text = "";
                                    machine.BackColor = Color.LightGray;
                                }
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
                        if (_teli_mode)
                        {
                            machine.Text = query.Scarti.ToString();
                            if (query.TeliBuoni != 0)
                                machine.BackColor = GetSMEfficiencyColor(Math.Round((Convert.ToDecimal(query.Scarti) /
                                                                         Convert.ToDecimal(query.TeliBuoni)) * 100, 1)
                                                                         .ToString());
                            else
                            {
                                if (query.Scarti != 0)
                                    machine.BackColor = GetSMEfficiencyColor("100.0");
                                else
                                    machine.BackColor = Color.LightGray;
                            }
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
                                if (query.Scarti != 0)
                                {
                                    machine.Text = "100.0";
                                    machine.BackColor = GetSMEfficiencyColor(machine.Text);
                                }
                                else
                                {
                                    machine.Text = "";
                                    machine.BackColor = Color.LightGray;
                                }
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
                                    _listOfExcluedMachines
                                    .SingleOrDefault(m => m == machine.Name.Remove(0, 1));

                            machine.BackColor =
                                !string.IsNullOrEmpty(excl_mac)
                                ? machine.BackColor = Color.DimGray :
                                machine.BackColor = GetEfficiencyColor(row[1].ToString());

                            if (!_currentColors.ContainsKey(machine))
                                _currentColors.Add(machine, machine.BackColor);

                            if (!_art_changes_colors.ContainsKey(machine))
                                _art_changes_colors.Add(machine, machine.BackColor);
                        }
                    }
                }
                else
                {
                    var days = (Get_to_date().Subtract(Get_from_date())).TotalDays;
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

            ////hum/temp totals
            if (Mode == "temperature")
            {
                double squadraOne = 0.0;
                double squadraTwo = 0.0;
                double squadraThree = 0.0;
                int sq1 = 0;
                int sq2 = 0;
                int sq3 = 0;
                foreach (var line in _list_of_lines)
                {
                    double.TryParse(line.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var value);
                    var strLine = line.Name.Remove(0, 1);
                    int.TryParse(strLine, out var lineNr);
                    if (lineNr >= 1 && lineNr <= 5)
                    {
                        squadraOne += value;
                        if (!string.IsNullOrEmpty(line.Text)) sq1++;
                    }
                    else if (lineNr > 5 && lineNr <= 10)
                    {
                        squadraTwo += value;
                        if (!string.IsNullOrEmpty(line.Text)) sq2++;
                    }
                    else if (lineNr > 10 && lineNr <= 15)
                    {
                        squadraThree += value;
                        if (!string.IsNullOrEmpty(line.Text)) sq3++;
                    }
                }
                S1.Text = (squadraOne / sq1).ToString(tempFormat, CultureInfo.InvariantCulture);
                S2.Text = (squadraTwo / sq2).ToString(tempFormat, CultureInfo.InvariantCulture);
                S3.Text = (squadraThree / sq3).ToString(tempFormat, CultureInfo.InvariantCulture);
            }

            if (_tmColorSwitch.Enabled)
            {
                _tmColorSwitch.Stop();
                _tmColorSwitch.Enabled = false;
            }

            if (!Get_is_excl_mode())
            {
                _tmColorSwitch.Enabled = true;
                _tmColorSwitch.Start();
            }

            lblFerme1.Visible = false;
            lblFerme2.Visible = false;
            lblFerme3.Visible = false;

            btnStatusImg.Image = Properties.Resources.checkmark_30;
            btnStatusImg.Tag = 0; //normal

            _lastToDate = Get_to_date();

            lblStatus.Text = "Ready";
            btnStatusImg.Image = Properties.Resources.checkmark_30;
            statusBar.Refresh();
            IsAuto = false;

            _lastDetectedShift = ResumeShift(false);

            _groupBorderWidth = S1.Location.X + S1.Width + 50;

            if (_tmDelay != null) _tmDelay = null;

            lbl_current_mode.Text = "Mod. " + fullMode;
            Cursor = Cursors.Default;

            foreach (Panel groupBox in new Panel[] { grp1, grp2, grp3 })
            {
                foreach (var item in (from lbl in groupBox.Controls.OfType<Label>()
                                      select lbl).ToList())
                {
                    item.Invalidate();
                    item.Update();
                }
            }

            if (Mode == "articlechanges" || Mode == "PCPPST"
                || Mode == "qty" || Mode == "tempStd")
            {
                foreach (var kvp in _art_changes_colors)
                    kvp.Key.BackColor = kvp.Value;
            }
            else PostMachineColors();
            LoadingInfo.CloseLoading();
        }
        
        private void GetTotals()
        {            
            var machineRange = new[] { 1, 14 };     //starts from line 1
            var count = 1;
            var curLineNumber = 1;

            if (Mode == "eff" || Mode == "cleaner" || Mode == "temperature" ||
                Mode == "rammendi" && !_teli_mode || Mode == "scarti" && !_teli_mode || Mode == "PCPPST")
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
                    if(Mode != "temperature")
                        line.Text = GetMachinesDataInRange(_list_of_machines, machineRange[0], machineRange[1], 1, false).Trim('=');
                    //false => is line
                    if (Mode == "eff")
                    {
                        _total_eff.Add(line, GetEfficiencyColor(line.Text.Trim('%')));
                        _art_changes_colors.Add(line, GetEfficiencyColor(line.Text.Trim('%')));
                        line.Font = _fontBig;
                    }
                    else if (Mode == "tempStd") line.Font = _fontSmall;
                    else if (Mode == "cleaner")
                    {
                        _total_eff.Add(line, Color.FromArgb(54, 215, 86));
                        line.Font = _fontBig;
                    }
                    else if (Mode == "scarti" && !_teli_mode || Mode == "rammendi" && !_teli_mode)
                    {
                        _total_eff.Add(line, GetSMEfficiencyColor(line.Text.Trim('%')));
                        line.Font = _fontBig;
                    }
                    else line.Font = _fontBig;
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
            lblTotDep.Text = "Total:" + GetMachinesDataInRange(
                _list_of_machines, 1, 210, 1, false).Trim('=');

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
                    string _totDepEff = lblTotDep.Text.Trim('%');
                    _totDepEff = _totDepEff.Remove(0, 6);
                    _total_eff.Add(lblTotDep,
                        GetEfficiencyColor(_totDepEff,
                            Color.LightGray));
                    _art_changes_colors.Add(S1,
                        GetEfficiencyColor(S1.Text.Trim('=', '%'),
                            Color.FromArgb(235, 235, 235)));
                    _art_changes_colors.Add(S2,
                        GetEfficiencyColor(S2.Text.Trim('=', '%'),
                            Color.FromArgb(235, 235, 235)));
                    _art_changes_colors.Add(S3,
                        GetEfficiencyColor(S3.Text.Trim('=', '%'),
                            Color.FromArgb(235, 235, 235)));
                    _art_changes_colors.Add(lblTotDep,
                        GetEfficiencyColor(_totDepEff,
                            Color.LightGray));
                    S1.Font = _fontBigSq;
                    S2.Font = _fontBigSq;
                    S3.Font = _fontBigSq;
                    lblTotDep.Font = _fontBigSq;
                    break;
                case "tempStd":
                    S1.Font = _fontSmallSq;
                    S2.Font = _fontSmallSq;
                    S3.Font = _fontSmallSq;
                    lblTotDep.Font = _fontSmallSq;
                    break;
                case "cleaner":
                    _total_eff.Add(S1, Color.FromArgb(54, 215, 86));
                    _total_eff.Add(S2, Color.FromArgb(54, 215, 86));
                    _total_eff.Add(S3, Color.FromArgb(54, 215, 86));
                    _total_eff.Add(lblTotDep, Color.FromArgb(54, 215, 86));
                    S1.Font = _fontSmallSq;
                    S2.Font = _fontSmallSq;
                    S3.Font = _fontSmallSq;
                    lblTotDep.Font = _fontSmallSq;
                    break;
                case "rammendi":
                    if (_teli_mode) break;
                    _total_eff.Add(S1,
                        GetSMEfficiencyColor(S1.Text.Trim('=', '%')));
                    _total_eff.Add(S2,
                        GetSMEfficiencyColor(S2.Text.Trim('=', '%')));
                    _total_eff.Add(S3,
                        GetSMEfficiencyColor(S3.Text.Trim('=', '%')));
                    string _totDepEfficiency = lblTotDep.Text.Trim('%');
                    _totDepEff = _totDepEfficiency.Remove(0, 6);
                    _total_eff.Add(lblTotDep,
                        GetSMEfficiencyColor(_totDepEff));
                    break;
                case "scarti":
                    if (_teli_mode) break;
                    _total_eff.Add(S1,
                        GetSMEfficiencyColor(S1.Text.Trim('=', '%')));
                    _total_eff.Add(S2,
                        GetSMEfficiencyColor(S2.Text.Trim('=', '%')));
                    _total_eff.Add(S3,
                        GetSMEfficiencyColor(S3.Text.Trim('=', '%')));
                    string _totalDepEfficiency = lblTotDep.Text.Trim('%');
                    _totDepEff = _totalDepEfficiency.Remove(0, 6);
                    _total_eff.Add(lblTotDep,
                        GetSMEfficiencyColor(_totDepEff));
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
                    || WithoutFermate && item.BackColor == Color.LightGray) continue;
                if (_isFinSelected | _isArticleSelected
                    && item.Text == string.Empty) continue;
                var machineRegNumb = Convert.ToInt32(item.Name.Remove(0, 1));
                if (machineRegNumb < from_mac || machineRegNumb > to_mac) continue;
                    counter++;

                switch (Mode)
                {
                    case "PCPPST":
                        {
                            if (!item.Text.Contains(":")) continue;
                            int.TryParse(item.Text.Split(':')[0], out int h);
                            int.TryParse(item.Text.Split(':')[1], out int m);
                            tmpStr = CumulateHHmm(h, m);
                            break;
                        }
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
                            else
                                value = 0;
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
                    var days = (Get_to_date().Subtract(Get_from_date())).TotalDays;
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
                        foreach(var m in rammendi)
                        {
                            producedRamm += m.Rammendi;
                            producedQty += m.TeliBuoni;
                        }

                        if (_teli_mode) tmpStr = "=" + producedRamm.ToString();
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
                        if (_teli_mode) tmpStr = "=" + producedScarti.ToString();
                        else
                        {
                            var val = (Convert.ToDouble(producedScarti) / Convert.ToDouble(producedQty)) * 100;                            
                            tmpStr = string.Format("={0:0.0}%", val);
                        }
                        break;
                    }
                case "articlechanges":
                    {
                        tmpStr = value.ToString();
                        //if (isBlock)
                        //{
                        //    var block = (from b in _squadra_list
                        //                 where b.FromMachine <= from_mac && b.ToMachine >= to_mac
                        //                 select b).SingleOrDefault();
                        //    tmpStr = "=" + block._articles_in_squadra.Count.ToString();
                        //}
                        //else if (from_mac == 1 && to_mac == 210)
                        //{
                        //    var result = 0;
                        //    foreach(var sq in _squadra_list)
                        //    {
                        //        result += sq._articles_in_squadra.Count;
                        //    }
                        //    tmpStr = "=" + result.ToString();
                        //}
                        //else
                        //{
                        //    var lines = (from l in _articles_in_line
                        //                 where l.FromMachine >= from_mac && l.ToMachine <= to_mac
                        //                 select l).ToList();
                        //    var result = 0;
                        //    foreach (var l in lines)
                        //    {
                        //        result += l.NumberOfArticles;
                        //    }
                        //    tmpStr = "=" + result.ToString();
                        //}
                        break;
                    }
            }
            return tmpStr.ToString(CultureInfo.CurrentCulture);
        }
        private void SetupEfficiencyBounds()
        {
            FrmHolidays.dc = new DataContext(conString);
            var bounds = (from rec in DatabaseTableClasses.Tables.TblEfficiencyBounds
                          select rec).ToList();
            foreach(var item in bounds)
            {
                if(item.Type == "scarti")
                {
                    double.TryParse(item.Red, out var lwr);
                    double.TryParse(item.Green, out var upr);
                    _scartiLowerBound = lwr;
                    _scartiUpperBound = upr;
                }
                else if(item.Type == "rammendi")
                {
                    double.TryParse(item.Red, out var lwr);
                    double.TryParse(item.Green, out var upr);
                    _rammendiLowerBound = lwr;
                    _rammendiUpperBound = upr;
                }
                else if(item.Type == "eff")
                {
                    double.TryParse(item.Red, out var lwr);
                    double.TryParse(item.Green, out var upr);
                    _effLowerBound = lwr;
                    _effUpperBound = upr;
                }
            }
        }
        private double _scartiUpperBound = 1;
        private double _scartiLowerBound = 1;
        private double _rammendiUpperBound = 1;
        private double _rammendiLowerBound = 1;
        private double _effUpperBound;
        private double _effLowerBound;
        private Color GetSMEfficiencyColor(string input)
        {
            double upperBound, lowerBound;
            if (Mode == "scarti")
            {
                upperBound = _scartiUpperBound;
                lowerBound = _scartiLowerBound;
            }
            else
            {
                upperBound = _rammendiUpperBound;
                lowerBound = _rammendiLowerBound;
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
                upperBound = _scartiUpperBound;
                lowerBound = _scartiLowerBound;
            }
            else
            {
                upperBound = _rammendiUpperBound;
                lowerBound = _rammendiLowerBound;
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
        private Dictionary<string, int> _dictOfCleanersGroup = new Dictionary<string, int>();
        private Color GetEfficiencyColor(string text)
        {
            var color = default(Color);
            double.TryParse(text, out var eff);
            if (eff == 0)
                color = Color.LightGray;
            else if
                (eff > 0 && eff <= _effLowerBound) color = Color.FromArgb(253, 129, 127);
            else if
                (eff > _effLowerBound && eff <= _effUpperBound) color = Color.FromArgb(254, 215, 1);
            else if
                (eff > _effUpperBound) color = Color.FromArgb(54, 214, 87);
            else
                color = Color.LightGray;
            return color;
        }

        private Color GetEfficiencyColor(string text, Color optColor)
        {
            var color = default(Color);
            double.TryParse(text, out var eff);
            if (eff == 0)
                color = optColor;
            else if
                (eff > 0 && eff <= _effLowerBound) color = Color.FromArgb(253, 129, 127);
            else if
                (eff > _effLowerBound && eff <= _effUpperBound) color = Color.FromArgb(254, 215, 1);
            else if
                (eff > _effUpperBound) color = Color.FromArgb(54, 214, 87);
            else
                color = optColor;

            return color;
        }

        private void SwitchColor(object info, System.Timers.ElapsedEventArgs e)
        {
            foreach (var machine in _list_of_machines)
                foreach (var item in _list_of_currents)
                {
                    if (item.MachineNumber != machine.Name.Remove(0, 1)) continue;
                    if (machine.BackColor != Color.CadetBlue || !((machine.Text == "0  ") | (machine.Text == "0:00")))
                    {
                        if (machine.BackColor == Color.LightGray && (machine.Text == "0  ") | (machine.Text == "0:00"))
                            machine.BackColor = Color.CadetBlue;
                    }
                    else
                    {
                        machine.BackColor = Color.LightGray;
                    }
                }
        }

        private string ConvertSecondsToHHmm(int seconds)
        {
            int hours = seconds / 3600;
            int mins = (seconds % 3600) / 60;
            return string.Format(@"{0:D2}:{1:D2}", hours, mins);
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

        private DataTable _tbl_machines = new DataTable();
        private DataTable _tbl_global = new DataTable();

        #region LayoutTriggering
        private void Machine_MouseEnter(object sender, EventArgs eventArgs)
        {
            _mouseOverMachine = (Label)sender;

            _mouseOverMachine.Refresh();
            _mouseOverMachineNumber = _mouseOverMachine.Name.Remove(0, 1);

            if (Get_is_excl_mode()) return;
        }

        private void ShowChartsPopUp()
        {
            if (!_sqlHasError)
            {
                var charts = new RepChartsDynamic();
                ShowReport(charts);
                //frmRepChartsDynamicActive = charts;
            }
        }

        private void ShowCleanerPopUp()
        {
            if (!_sqlHasError)
            {
                var repCleaners = new RepMachineCleaners();
                repCleaners.SetPuliziaType(_cleanersType);
                ShowReport(repCleaners);
            }
        }
        private void Machine_DoubleClick(object sender, EventArgs e)
        {
            var mac = (Label)sender;
            var idm = int.Parse(mac.Name.Remove(0, 1));

            if (Mode == "scarti" || Mode == "rammendi")
            {
                var frm = new RepScartiRammendi(idm, Mode);
                ShowReport(frm);
                return;
            }
            else if (_cleanersType == "cquality")
            {
                var frm = new RepControlQuality(idm);
                ShowReport(frm);
                return;
            }
            else if (Mode != "cleaner")
            {
                _isSquadra = false;
                ShowChartsPopUp();
            }
            else
            {
                _isSquadra = false;
                ShowCleanerPopUp();
            }
        }

        public static bool _isSquadra = false;
        public static string _selSquadra;
        private void Machine_Click(object sender, EventArgs eventArgs)
        {
            //if (!Get_is_excl_mode())
            //    {
            //        if (Mode != "cleaner")
            //        {
            //            _isSquadra = false;
            //            ShowChartsPopUp();
            //        }
            //        else
            //        {
            //            _isSquadra = false;
            //            ShowCleanerPopUp();
            //        }
            //    }
                if(Get_is_excl_mode())
                {
                    if (_mouseOverMachine.BackColor == Color.DimGray)
                    {
                        foreach (var item in _list_of_machines)
                            if (item.Name == "P" + _mouseOverMachine.Name.Remove(0, 1))
                            {
                                item.BackColor = GetEfficiencyColor(item.Text);
                                _listOfExcluedMachines.Remove(_mouseOverMachine.Name.Remove(0, 1));
                            }
                    }
                    else
                    {
                        _mouseOverMachine.BackColor = Color.DimGray;
                        _listOfExcluedMachines.Add(_mouseOverMachine.Name.Remove(0, 1));
                    }

                    StartDelayedLoading();
                }
        }

        private void S1_Click(object sender, EventArgs e)
        {
            if (_sqlHasError) return;
            
            _isSquadra = true;
            _selSquadra = "S1";
            this.cb_SQ_1.Checked = true;
            ShowChartsPopUp();
        }

        private void S2_Click(object sender, EventArgs e)
        {
            if (_sqlHasError) return;
            LoadingInfo.InfoText = "Loading charts for Squadra 2...";
            LoadingInfo.ShowLoading();
            _isSquadra = true;
            _selSquadra = "S2";
            this.cb_SQ_2.Checked = true;
            ShowChartsPopUp();
        }

        private void S3_Click(object sender, EventArgs e)
        {
            if (_sqlHasError) return;
            LoadingInfo.InfoText = "Loading charts for Squadra 3...";
            LoadingInfo.ShowLoading();
            _isSquadra = true;
            _selSquadra = "S3";
            this.cb_SQ_3.Checked = true;
            ShowChartsPopUp();
        }

        #endregion

        #region SideBarCommands

        private void button10_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            foreach(var b in _lstOfModes)
            {
                if (btn.Name == b.Name) b.Image = Properties.Resources.modes1_green;
                else b.Image = Properties.Resources.modes1_silver;
            }
            Mode = "eff";
            _operatorMode = true;
            SetDefaultFilters(false);

            if (cboArt.SelectedIndex > 0)
            {
                cboArt.SelectedIndex = 0;
                _isArticleSelected = false;
                Set_file_name(string.Empty);
            }
            GetData();
        }

        private void btnNTeli_Click(object sender, EventArgs e)
        {
            ResetOperatorsFields();
            var btn = sender as Button;
            foreach (var b in _lstOfModes)
            {
                if (btn.Name == b.Name) b.Image = Properties.Resources.modes1_green;
                else b.Image = Properties.Resources.modes1_silver;
            }
            Mode = "qty";
            SetDefaultFilters(false);
            GetData();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ResetOperatorsFields();
            var btn = sender as Button;
            foreach (var b in _lstOfModes)
            {
                if (btn.Name == b.Name) b.Image = Properties.Resources.modes1_green;
                else b.Image = Properties.Resources.modes1_silver;
            }
            Mode = "tempStd";
            SetDefaultFilters(false);
            GetData();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (btnFerme.BackColor == Color.WhiteSmoke)
            {
                SetDefaultFilters(true);
                return;
            }
            FocusOtherFilters(btnFerme);
            var fin = Get_fin_array().ToString();
            if (fin != ",3,7,14,") return;

            _list_of_stopped_machines = new List<MachinesProperties>();

            foreach (var stopMac in _list_of_machines)
                if (stopMac.Text == "0  " || stopMac.Text == "0:00")
                    _list_of_stopped_machines.Add(new MachinesProperties(stopMac.Name.Remove(0, 1), stopMac.Text));

            _stopSq1 = 0;
            _stopSq2 = 0;
            _stopSq3 = 0;

            lblFerme1.Visible = true;
            lblFerme2.Visible = true;
            lblFerme3.Visible = true;

            ResetGlobals();

            foreach (var item in _list_of_machines)
            {
                item.BackColor = Color.Gainsboro; item.Text = default(string);

                foreach (var prop in _list_of_stopped_machines)
                {
                    if (item.Name.Remove(0, 1) != prop.Machine) continue;

                    item.Text = prop.Eff;
                    item.BackColor = GetEfficiencyColor(item.Text, Color.FromArgb(255, 111, 99));

                    var iMac = Convert.ToInt32(item.Name.Remove(0, 1));

                    if (iMac >= 1 && iMac <= 70) _stopSq1++;
                    if (iMac >= 71 && iMac <= 140) _stopSq2++;
                    if (iMac >= 141 && iMac <= 210) _stopSq3++;
                }
            }

            var tot = _stopSq1 + _stopSq2 + _stopSq3;

            lblFerme1.Text = "Ferme: " + _stopSq1.ToString();
            lblFerme2.Text = "Ferme: " + _stopSq2.ToString();
            lblFerme3.Text = "Ferme: " + _stopSq3.ToString();
            lblTotDep.Text = tot.ToString();

            _tmColorSwitch?.Dispose();

            _tmColorSwitch = new System.Timers.Timer(2000);
            _tmColorSwitch.Elapsed += SwitchColor;
            _tmColorSwitch.Enabled = true;
        }

        private void btnVelo_Click(object sender, EventArgs e)
        {
            ResetOperatorsFields();
            var btn = sender as Button;
            foreach (var b in _lstOfModes)
            {
                if (btn.Name == b.Name) b.Image = Properties.Resources.modes1_green;
                else b.Image = Properties.Resources.modes1_silver;
            }
            foreach (var machine in _list_of_machines)
            {
                foreach (var item in _list_of_currents.Where(m => m.MachineNumber == machine.Name.Remove(0, 1)))
                {
                    machine.Text = item.Speed;
                    machine.Font = _fontBig;
                }
            }
        }

        private void btnArticles_Click(object sender, EventArgs e)
        {
            //if (btnArtView.BackColor == Color.SeaGreen)
            //{
            //    FocusOtherFilters();
            //    btnEffMode.PerformClick();
            //    return;
            //}

            btnEffMode.BackColor = Color.Gray;
            btnTimeMode.BackColor = Color.Gray;
            btnSpeedMode.BackColor = Color.Gray;
            btnQtyMode.BackColor = Color.Gray;
            FocusOtherFilters(btnArtView);

            foreach (var machine in _list_of_machines)
            {
                //if (machine.BackColor == Color.Gainsboro) continue;

                foreach (var item in _list_of_currents.Where(m => m.MachineNumber == machine.Name.Remove(0, 1)))
                {
                    machine.Text = item.FileName;
                    machine.Font = _fontSmall;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //btnEffMode.BackColor = Color.Gray;
            //btnTimeMode.BackColor = Color.Gray;
            //btnSpeedMode.BackColor = Color.Gray;
            //btnQtyMode.BackColor = Color.Gray;

            if (btnEsclMode.BackColor == Color.FromArgb(80, 80, 80))
            {
                if (Get_is_excl_mode())
                {
                    if (_tmDelay != null) _tmDelay.Dispose();
                    SetDefaultFilters(true);
                }
                else
                {
                    Set_is_excl_mode(true);
                    FocusOtherFilters(btnEsclMode);
                }

                btnEsclMode.BackColor = Color.WhiteSmoke;
            }
            else if (btnEsclMode.BackColor == Color.WhiteSmoke)
            {
                _listOfExcluedMachines.Clear();

                foreach (var mac in _list_of_machines)
                {
                    if (mac.Text == "0  " || mac.Text == "0:00")
                    {
                        mac.BackColor = Color.DimGray;

                        _listOfExcluedMachines.Add(mac.Name.Remove(0, 1));
                    }
                }
                btnEsclMode.BackColor = Color.DimGray;

                StartDelayedLoading();
            }
            else if (btnEsclMode.BackColor == Color.DimGray)
            {
                SetDefaultFilters(true);
                return;
            }
        }

        public static bool WithoutFermate { get; set; }
        private void btnWithoutFerm_Click(object sender, EventArgs e)
        {
            WithoutFermate = !WithoutFermate;
            btnWithoutFerm.BackColor = Color.WhiteSmoke;
            if (!WithoutFermate)
            {
                btnWithoutFerm.BackColor = Color.FromArgb(80, 80, 80);
                btnEffMode.PerformClick();
                return;
            }
            GetData();
        }

        private Button _sideButtonParser;
        private void SideButtonParser_ClickForBackColor(object sender, EventArgs eventArgs)
        {
            _sideButtonParser = (Button)sender;
            foreach (var button in _list_of_buttons)
            {
                if (_sideButtonParser.BackColor != Color.Gray) continue;
                foreach (KeyValuePair<Button, Color> kvp in _mode_filters)
                {
                    if (button == kvp.Key) button.BackColor = kvp.Value;
                }
            }
            //colorize only united buttons
            if (_sideButtonParser.BackColor != SystemColors.Control)
                _sideButtonParser.BackColor = Color.FromArgb(80, 80, 80);
        }
        #endregion

        public static System.Threading.Timer _tmDelay;
        private void GetDelayedData(object data)
        {
            if (_sqlHasError || _isReportMode) return;

            try
            {
                GetData();
                //var dayRange = Get_to_date().Subtract(Get_from_date()).Days;

                //if (dayRange == 0 && !bgvCharts.IsBusy)
                //    {
                //    bgvCharts.RunWorkerAsync();
                //    }
            }
            catch
            {
                //ingored
            }
            finally
            {
                btnStopReloading.Visible = false;
                lblLastFile.Enabled = false;
                lblLastFile.ForeColor = Color.Gray;
            }
        }

        private void StartDelayedLoading()
        {
            if (_tmDelay != null) _tmDelay.Dispose();

            if (_sqlHasError || _isReportMode) return;

            btnStopReloading.Visible = true;
            lblStatus.Text = "Pending...";
            btnStatusImg.Image = Properties.Resources.load_animation;
            statusBar.Refresh();

            TimerCallback tcb = new TimerCallback(GetDelayedData);
            AutoResetEvent are = new AutoResetEvent(false);
            Application.DoEvents();

            _tmDelay = new System.Threading.Timer(tcb, are, 3000, 0);
        }

        private void btnStopReloading_ButtonClick(object sender, EventArgs e)
        {
            if (_tmDelay != null) _tmDelay.Dispose();

            lblStatus.Text = "Ready";
            btnStatusImg.Image = Properties.Resources.checkmark_30;
            btnStopReloading.Visible = false;
        }

        public static bool _isFinSelected = false;
        private void Fin_Click(object sender, EventArgs e)
        {
            var cbFin = (CheckBox)sender;

            Set_fin_array(new System.Text.StringBuilder());

            Get_fin_array().Append(",");

            if (cbFin.Checked)
            {
                cbFin.Image = Properties.Resources.output_onlinepngtools;
                _listOfSelectedFinesses.Add(cbFin.Name);
            }
            else
            {                
                cbFin.Image = Properties.Resources.filter_silver;
                _listOfSelectedFinesses.Remove(cbFin.Name);
            }

            foreach (var fin in _listOfSelectedFinesses)
            {
                Get_fin_array().Append(fin.Remove(0, 5) + ",");
            }

            if (_listOfSelectedFinesses.Count == 3)
            {
                _isFinSelected = false;
            }
            else
            {
                _isFinSelected = true;
            }

            StartDelayedLoading();
        }

        private void FocusOtherFilters()
        {
            btnArtView.BackColor = Color.FromArgb(80, 80, 80);
            btnFerme.BackColor = Color.FromArgb(80, 80, 80);
            btnEsclMode.BackColor = Color.FromArgb(80, 80, 80);
        }

        private void FocusOtherFilters(Button button)
        {
            btnArtView.BackColor = Color.FromArgb(80, 80, 80);
            btnFerme.BackColor = Color.FromArgb(80, 80, 80);
            btnEsclMode.BackColor = Color.FromArgb(80, 80, 80);

            button.BackColor = Color.WhiteSmoke;
        }

        private void SetDefaultFilters(bool perform)
        {
            FocusOtherFilters();
            Set_is_excl_mode(false);
            if (perform)
            {
                btnEffMode.PerformClick();
            }
        }

        private bool IsFullScreen { get; set; }

        private void btnFullScreen_Click(object sender, EventArgs e)
        {
            IsFullScreen = true;

            SuspendLayout();

            pnMasthead.Visible = false;

            FormBorderStyle = FormBorderStyle.None;

            WindowState = FormWindowState.Minimized;
            WindowState = FormWindowState.Maximized;

            ResumeLayout(true);

            _btnExitFullScreen.Location = new Point(Width / 2 - 25, -60);
            _btnExitFullScreen.Visible = true;

            if (_isFilterOpen)
            {
                pnFilterby.Width = 0;
            }

            lblSelectedArticle.Text = "ONLYOU - Sinotico";
        }

        private void CreateExitFullScreenButton()
        {
            // Creates exit full screen button

            _tmExitFullScreen = new System.Windows.Forms.Timer();
            _tmExitFullScreen.Tick += timer1_Tick;
            _tmExitFullScreen.Interval = 100;
            _tmExitFullScreen.Enabled = true;

            _btnExitFullScreen = new Button
            {
                BackColor = Color.White,
                Size = new Size(50, 50),
                Location = new Point(Width / 2 - 25, -60),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft Sans Serif", 15, FontStyle.Regular),
                ImageAlign = ContentAlignment.MiddleCenter,
                Image = Properties.Resources.exit_x_50_silver
            };

            _btnExitFullScreen.FlatAppearance.BorderSize = 0;

            Controls.Add(_btnExitFullScreen);

            _btnExitFullScreen.BringToFront();

            _btnExitFullScreen.Paint += (s, p) =>
            {
                //rounded control path

                var btn = (Button)s;
                var pen = new Pen(Brushes.DimGray, 10);

                using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(0, 0, btn.Width, btn.Height), 25))
                {
                    //p.Graphics.FillRectangle(Brushes.DimGray, 0, 0, btn.Width, btn.Height);
                    p.Graphics.DrawPath(pen, path);
                    btn.Region = new Region(path);
                    p.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    p.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                }
            };

            _btnExitFullScreen.Click += delegate
                {
                    SuspendLayout();

                    _btnExitFullScreen.Visible = false;

                    pnMasthead.Visible = true;

                    if (_lblBack != null) _lblBack.Text = "";

                    if (_isFilterOpen && !_isReportMode)
                    {
                        pnFilterby.Width = 130;
                    }

                    ResumeLayout(true);

                    FormBorderStyle = FormBorderStyle.Sizable;
                    IsFullScreen = false;

                    lblSelectedArticle.Text = "";
                    // Gets the screen resolution 
                };
        }

        protected override void OnResize(EventArgs e)
        {
            if (_btnExitFullScreen != null)
                _btnExitFullScreen.Location = new Point(Width - _btnExitFullScreen.Width - 50, 0);

            _my_menu_strip.Location = new Point(Width - _my_menu_strip.Width - 70, 70);

            if (_btnExitFullScreen != null)
            {
                _btnExitFullScreen.Location = new Point(Width / 2 - 25, -60);
            }

            base.OnResize(e);
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

            lblTotDep.BackColor = Color.Gainsboro;
            lblTotDep.Text = "0";

            if (cboArt.SelectedIndex > 0)
            {
                lblSelectedArticle.Text = "Article: " + cboArt.Text;
            }
            else
            {
                lblSelectedArticle.Text = "";
            }
        }

        #region MyMenuStrip

        private void btnReport_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            _my_menu_strip = new MyMenuStrip();
            _my_menu_items.Clear();
            _items_collection = new string[] { "Andamento dell'OEE rettilinee", "Produzione capi efficienza", "Produzione giornaliera",
                                               "Controllo Temperatura e Umidita", "Tempi per CQ operatori", "Tempi per Pulizie operatori",
                                               "Tempi per CQ", "Pulizie fronture", "Pulizie ordinarie", "Numero cambio articolo",
                                                 "Causali fermate", //"Causali fermate charts","Capi prodotti appaiati",
                                               "Efficienza machine in funzione e non", "Efficienza per macchina/turno", "Efficienza per macchina"};
            _my_menu_items.AddRange(_items_collection);

            Controls.Add(_my_menu_strip);

            _my_menu_strip.Location = new Point(Width - _my_menu_strip.Width - 170, 5); //70 for Y  -100
            _my_menu_strip.BringToFront();

            foreach (var item in _my_menu_items)
            {
                _my_menu_item = new MyMenuItem(item);
                _my_menu_strip.Controls.Add(_my_menu_item);
                ResetExcelButtonEvent();

                if (item == "Efficienza per macchina")
                {
                    _my_menu_item.Click += delegate
                        {
                            var frm = new RepMachines();
                            ShowReport(frm);

                            _lblExcel.Click += delegate { frm.ExportToExcel(); };
                        };
                }
                else if (item == "Efficienza per macchina/turno")
                {
                    _my_menu_item.Click += delegate
                        {
                            var frm = new RepEffMachines();
                            ShowReport(frm);

                            _lblExcel.Click += delegate { frm.ExportToExcel(); };
                        };
                }
                else if (item == "Efficienza machine in funzione e non")
                {
                    _my_menu_item.Click += delegate
                        {
                            var frm = new RepEffTurno();
                            ShowReport(frm);

                            _lblExcel.Click += delegate { frm.ExportToExcel(); };
                        };
                }
                else if (item == "Causali fermate")
                {
                    _my_menu_item.Click += delegate
                        {
                            var frm = new RepStopMachines();
                            ShowReport(frm);

                            _lblExcel.Click += delegate { frm.ExportToExcel(); };
                        };
                }
                else if (item == "Causali fermate charts")
                {
                    _my_menu_item.Click += delegate
                        {
                            var frm = new RepCharts();
                            ShowReport(frm);
                        };
                }
                else if (item == "Capi prodotti appaiati")
                {
                    _my_menu_item.Click += delegate
                        {
                            var frm = new RepArticles();
                            ShowReport(frm);
                            _lblExcel.Click += delegate { frm.ExportToExcel(); };
                        };
                }
                else if (item == "Numero cambio articolo")
                {
                    _my_menu_item.Click += delegate
                        {
                            var frm = new RepMonthChanges();
                            ShowReport(frm);
                            _lblExcel.Click += delegate { frm.ExportToExcel(); };
                        };
                }
                else if (item == "Pulizie fronture")
                {
                    _my_menu_item.Click += delegate
                    {
                        var frm = new RepCleaners();
                        frm.SetReportType("Pulizia Fronture");
                        ShowReport(frm);
                        _lblExcel.Click += delegate { frm.ExportToExcel(); };
                    };
                }
                else if (item == "Pulizie ordinarie")
                {
                    _my_menu_item.Click += delegate
                        {
                            var frm = new RepCleaners();
                            frm.SetReportType("Pulizia Ordinaria");
                            ShowReport(frm);
                            _lblExcel.Click += delegate { frm.ExportToExcel(); };
                        };
                }

                else if (item == "Tempi per Pulizie operatori")
                {
                    _my_menu_item.Click += delegate
                        {
                            var frm = new RepCleanersJob("cleaner", "Pulizia Operatori");
                            ShowReport(frm);
                            _lblExcel.Click += delegate { frm.ExportToExcel(); };
                        };
                }
                else if (item == "Tempi per CQ operatori")
                {
                    _my_menu_item.Click += delegate
                    {
                        var frm = new RepCleanersJob("cquality", "Quality Control");
                        ShowReport(frm);
                        _lblExcel.Click += delegate { frm.ExportToExcel(); };
                    };
                }
                else if (item == "Tempi per CQ")
                {
                    _my_menu_item.Click += delegate
                    {
                        var frm = new RepCleaners();
                        frm.SetReportType("cquality");
                        ShowReport(frm);
                        _lblExcel.Click += delegate { frm.ExportToExcel(); };
                    };
                }
                else if (item == "Controllo Temperatura e Umidita")
                {
                    _my_menu_item.Click += delegate
                    {
                        var frm = new RepTemperature();
                        //frm.SetReportType("cquality");
                        ShowReport(frm);
                        //_lblExcel.Click += delegate { frm.ExportToExcel(); };
                    };
                }
                else if (item == "Produzione giornaliera")
                {
                    _my_menu_item.Click += delegate
                    {
                        var frm = new RepGiornaliera();                        
                        ShowReport(frm);
                        //_lblExcel.Click += delegate { frm.ExportToExcel(); };
                    };
                }                
                else if (item == "Produzione capi efficienza")
                {
                    _my_menu_item.Click += delegate
                    {
                        var frm = new RepProduzioneCapiEfficienza();
                        ShowReport(frm);
                        //_lblExcel.Click += delegate { frm.ExportToExcel(); };
                    };
                }
                else if (item == "Andamento dell'OEE rettilinee")
                {
                    _my_menu_item.Click += delegate
                    {
                        var frm = new RepOEETEEP();
                        ShowReport(frm);
                        //_lblExcel.Click += delegate { frm.ExportToExcel(); };
                    };
                }
            }

        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_LBUTTONDOWN || (m.Msg == WM_PARENTNOTIFY && (int)m.WParam == WM_LBUTTONDOWN))
            {
                //Close menu strip control when it's out of focus.

                if (!_my_menu_strip.ClientRectangle.Contains(
                                _my_menu_strip.PointToClient(Cursor.Position)))

                    Controls.Remove(_my_menu_strip);
            }

            base.WndProc(ref m);
        }

        private void CreateReportPanel()
        {
            _subMdiContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.WhiteSmoke
            };

            Controls.Add(_subMdiContainer);
            _subMdiContainer.BringToFront();

            masthead.SendToBack();

            var pnReportNavi = new Panel
            {
                BackColor = Color.WhiteSmoke,
                Height = 40,
                Dock = DockStyle.Top,
                BorderStyle = BorderStyle.None
            };

            pnReportNavi.Paint += (s, e) =>
            {
                var linearGradientBrush = new LinearGradientBrush(pnReportNavi.ClientRectangle, Color.White, Color.White, 90);
                var cblend = new ColorBlend()
                {
                    Colors = new[] { Color.White, Color.WhiteSmoke, Color.Gainsboro, Color.Silver },
                    Positions = new[] { 0f, 0.5f, 0.85f, 1f }
                };
                linearGradientBrush.InterpolationColors = cblend;
                e.Graphics.FillRectangle(linearGradientBrush, pnReportNavi.ClientRectangle);
            };

            var lblRepTit = new Label
            {
                Text = "Reports",
                Font = new Font("Tahoma", 8, FontStyle.Bold),
                ForeColor = Color.DimGray,
                Dock = DockStyle.Left,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Width = 300,
                BackColor = Color.Transparent
            };

            _lblBack = new Label
            {
                AutoSize = false,
                Width = 55,
                Dock = DockStyle.Right,
                Image = Properties.Resources.back_icon_32,
                ImageAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            //_lblPdf = new Label
            //    {
            //    AutoSize = false,
            //    Width = 55,
            //    BorderStyle = BorderStyle.None,
            //    Dock = DockStyle.Right,
            //    Image = Properties.Resources.export_pdf_icon_32,
            //    ImageAlign = ContentAlignment.MiddleCenter,
            //    BackColor = Color.Transparent
            //    };

            _lblExcel = new Label
            {
                AutoSize = false,
                Width = 55,
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Right,
                Image = Properties.Resources.export_excel_icon_32,
                ImageAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            _lblBack.MouseEnter += delegate
                {
                    _lblBack.Image = Properties.Resources.back_icon_orange;
                };
            _lblBack.MouseLeave += delegate
                {
                    _lblBack.Image = Properties.Resources.back_icon_32;
                };

            _lblPdf.MouseEnter += delegate
                {
                    _lblPdf.Image = Properties.Resources.icons_pdf_filled_32;
                };
            _lblPdf.MouseLeave += delegate
                {
                    _lblPdf.Image = Properties.Resources.export_pdf_icon_32;
                };

            _lblExcel.MouseEnter += delegate
                {
                    _lblExcel.Image = Properties.Resources.icons_excel_filled_32;
                };
            _lblExcel.MouseLeave += delegate
                {
                    _lblExcel.Image = Properties.Resources.export_excel_icon_32;
                };

            _subMdiContainer.Controls.Add(pnReportNavi);

            pnReportNavi.Controls.Add(lblRepTit);
            //pnReportNavi.Controls.Add(_lblPdf);
            pnReportNavi.Controls.Add(_lblExcel);
            pnReportNavi.Controls.Add(_lblBack);

            _pnForm = new Panel
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                BackColor = default(Color)
            };

            _pnForm.DoubleBufferedPanel(true);

            _subMdiContainer.Resize += delegate
                {
                    _pnForm.Dock = DockStyle.Fill;
                    _pnForm.Invalidate();
                };

            _subMdiContainer.Controls.Add(_pnForm);
            _pnForm.BringToFront();

            _subMdiContainer.Visible = false;

            _lblBack.Click += (s, e) =>
            {
                _isReportMode = false;

                //for (int i = _pnForm.Controls.Count - 1; i >= 0; i--)
                //    {
                //    if ((_pnForm.Controls[i] is Form form))
                //        form.WindowState = FormWindowState.Minimized;
                //    }

                _subMdiContainer.Visible = false;

                if (_isFilterOpen)
                {
                    pnFilterby.Width = 130;
                }

                pbBackToReport.Visible = true;
            };
        }

        private void ShowReport(Form frm)
        {
            if (_sqlHasError) return;
            if (Get_from_date() == null || Get_to_date() == null || Get_shift_array() == null)
            {
                Controls.Remove(_my_menu_strip);
                MessageBox.Show("No data.");
                return;
            }
            if (_tmDelay != null) _tmDelay.Dispose();
            lblStatus.Text = "Ready";
            btnStatusImg.Image = Properties.Resources.checkmark_30;
            btnStopReloading.Visible = false;
            //_pnForm.SuspendLayout();
            Controls.Remove(_my_menu_strip);

            for (int i = _pnForm.Controls.Count - 1; i >= 0; i--)
            {
                if ((_pnForm.Controls[i] is Form form))
                    form.WindowState = FormWindowState.Minimized;
            }

            frm.TopLevel = false;
            frm.AutoScroll = true;
            //frm.FormBorderStyle = FormBorderStyle.Sizable;

            _pnForm.Controls.Add(frm);
            frm.WindowState = FormWindowState.Maximized;
            frm.Show();
            frm.BringToFront();
            frm.Refresh();

            _subMdiContainer.BringToFront();
            _subMdiContainer.Visible = true;
            _isReportMode = true;

            //_pnForm.Invalidate();
            //_pnForm.ResumeLayout(true);

            pnFilterby.Width = 0;
        }

        private void pbBackToReport_Click(object sender, EventArgs e)
        {
            if (_sqlHasError) return;
            if (_isReportMode) return;

            _subMdiContainer.BringToFront();
            _subMdiContainer.Visible = true;
            _isReportMode = true;

            pnFilterby.Width = 0;
        }

        private void ResetExcelButtonEvent()
        {
            var f1 = typeof(Control).GetField("EventClick", BindingFlags.Static | BindingFlags.NonPublic);

            var obj = f1.GetValue(_lblExcel);

            var pi = _lblExcel.GetType().GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance);

            var list = (EventHandlerList)pi.GetValue(_lblExcel, null);
            list.RemoveHandler(obj, list[obj]);
        }

        #endregion

        public static bool _isArticleSelected = false;
        private void cboArt_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cboArt.SelectedIndex <= 0)
            {
                _isArticleSelected = false;
                Set_file_name(cboArt.SelectedValue.ToString());
                GetData();
                return;
            }
            Set_file_name(cboArt.SelectedValue.ToString());
            _isArticleSelected = true;
            // avoid error on rapid scrolling by user
            GetData();
        }

        List<Label> _fields = new List<Label>();
        private void ChangeFieldsPosition(bool state)
        {
            if (state)
            {
                grp1.Left -= 20;
                grp2.Left -= 20;
                grp3.Left -= 20;
            }
            else
            {
                grp1.Left += 20;
                grp2.Left += 20;
                grp3.Left += 20;
            }
        }

        private bool _isFilterOpen;
        private void btnFilterby_Click(object sender, EventArgs e)
        {
            if (_isReportMode) return;

            pnFilterby.BackColor = Color.FromArgb(80, 80, 80);
            foreach (Control c in pnFilterby.Controls)
                c.ForeColor = Color.White;

            if (pnFilterby.Width > 0)
            {
                foreach (var lbl in _totLabels)
                    lbl.Visible = true;

                pnFilterby.Width = 0;
                _isFilterOpen = false;
                _squadra_1.Text = "SQUADRA 1:";
                _squadra_2.Text = "SQUADRA 2:";
                _squadra_3.Text = "SQUADRA 3:";
                _squadra_pnl.Width = 91;
            }
            else
            {
                foreach (var lbl in _totLabels)
                    lbl.Visible = false;
                pnFilterby.Width = 130;
                _isFilterOpen = true;
                _squadra_1.Text = "SQ 1:";
                _squadra_2.Text = "SQ 2:";
                _squadra_3.Text = "SQ 3:";
                _squadra_pnl.Width = 30;
            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            Set_from_date(new DateTime(dtpFrom.Value.Year, dtpFrom.Value.Month, dtpFrom.Value.Day));
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            Set_to_date(new DateTime(dtpTo.Value.Year, dtpTo.Value.Month, dtpTo.Value.Day));
        }

        private void dtpTo_CloseUp(object sender, EventArgs e)
        {
            var fD = Get_from_date();
            var tD = Get_to_date();

            var newTd = dtpTo.Value;

            if (tD < fD) return; // || tD == _lastToDate) return;
            if (fD > DateTime.Now || tD > DateTime.Now) return;
            // use cheat to obtain all shifts

            if (cbNight.Checked == false) cbNight.Checked = true;
            if (cbMorning.Checked == false) cbMorning.Checked = true;
            if (cbAfternoon.Checked == false) cbAfternoon.Checked = true;
            //_pb_night.Image = Properties.Resources.mac_50x31_gif;
            //_pb_morning.Image = Properties.Resources.mac_50x31_gif;
            //_pb_afternoon.Image = Properties.Resources.mac_50x31_gif;

            ListOfSelectedShifts.Clear();
            ListOfSelectedShifts.AddRange(new[] { "cbNight", "cbMorning", "cbAfternoon" });

            Set_shift_array(new System.Text.StringBuilder(",Night,Morning,Afternoon,"));

            StartDelayedLoading();
        }

        private System.Threading.Timer _tmBackButton;
        private int _blinkStop = 0;
        private void MainWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isReportMode)
            {
                if (_tmBackButton != null) _tmBackButton.Dispose();

                TimerCallback tcb = new TimerCallback(BlinkBackButton);
                AutoResetEvent are = new AutoResetEvent(true);
                Application.DoEvents();
                _tmBackButton = new System.Threading.Timer(tcb, are, 100, 100);

                e.Cancel = true;
            }
        }
        private void MainWnd_FromClosed(object sender, FormClosedEventArgs e)
        {
            if (_tmColorSwitch != null || _tmColorSwitch.Enabled)
            {
                _tmColorSwitch.Dispose();
            }

            GC.Collect();
        }

        int _img = 0;

        public static bool _is_excl_mode1;

        private void button13_Click(object sender, EventArgs e)
        {
            if (_isReportMode) return;

            if (grp1.Visible)
            {
                grp1.Visible = false;
                //btnSq1.Text = "_";
            }
            else
            {
                grp1.Visible = true;
                //btnSq1.Text = "";
            }

            CreateObjectGroups();

            StartDelayedLoading();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (_isReportMode) return;

            if (grp2.Visible)
            {
                grp2.Visible = false;
                //btnSq2.Text = "_";
            }
            else
            {
                grp2.Visible = true;
                //btnSq2.Text = "";
            }

            CreateObjectGroups();

            StartDelayedLoading();
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            if (_isReportMode) return;

            if (grp3.Visible)
            {
                grp3.Visible = false;
                //btnSq3.Text = "_";
            }
            else
            {
                grp3.Visible = true;
                //btnSq3.Text = "";
            }

            CreateObjectGroups();

            StartDelayedLoading();
        }

        private void btnStatusImg_ButtonClick(object sender, EventArgs e)
        { 
            var status = Convert.ToInt32(btnStatusImg.Tag);

            if (status == -1)
            {
                MessageBox.Show("No server connection.", "Sinotico", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                var statusMsg = new System.Text.StringBuilder();

                statusMsg.AppendLine("Active status info");
                statusMsg.AppendLine("");
                statusMsg.AppendLine("Date interval");
                statusMsg.AppendLine("________________________");
                statusMsg.AppendLine("From: " + Get_from_date().ToLongDateString());
                statusMsg.AppendLine("To: " + Get_to_date().ToLongDateString());
                statusMsg.AppendLine("");
                statusMsg.AppendLine("Shift(s) in manual order");
                statusMsg.AppendLine("________________________");

                var x = 0;
                foreach (var s in ListOfSelectedShifts)
                {
                    x++;
                    statusMsg.AppendLine(x.ToString() + ". " + s.Remove(0, 2));
                }

                statusMsg.AppendLine("");

                MessageBox.Show(statusMsg.ToString(),
                    "Sinotico status",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void button10_Click_2(object sender, EventArgs e)
        {
            var frmSplitConfig = new SplitConfiguration();
            frmSplitConfig.Show();
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            pnContainer.SendToBack();
            var oldColor = grp1.BackColor;
            grp1.BackColor = Color.White;
            grp2.BackColor = Color.White;
            grp3.BackColor = Color.White;
            pnContainer.BackColor = Color.White;
            pnContainer.Refresh();
            int width = pnContainer.Bounds.Width;
            int height = pnContainer.Bounds.Height;
            
            //842 pixels x 1191 pixels - 3508 pixels x 4961
            Bitmap bmp = new Bitmap(1050, height + 400);
            pnContainer.DrawToBitmap(bmp, new Rectangle(new Point(10, 30), bmp.Size));
            iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A3.Rotate());
            var sfd = new SaveFileDialog();
            sfd.Filter = "PDF(*.pdf)|*.pdf";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                iTextSharp.text.pdf.PdfWriter.GetInstance(doc, new FileStream(sfd.FileName, FileMode.Create));
                doc.Open();
                iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(bmp, System.Drawing.Imaging.ImageFormat.Png);
                var sb = new StringBuilder();
                foreach (var s in ListOfSelectedShifts)
                {
                    sb.Append(s.Remove(0, 2) + " - ");
                }
                //Add purpose title
                string fromDate = "From date: " + Get_from_date().ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                string toDate = "To date: " + Get_to_date().ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                iTextSharp.text.Phrase title = new iTextSharp.text.Phrase();
                title = new iTextSharp.text.Phrase(new iTextSharp.text.Chunk("SINOTICO - Layout preview " + "(" + sb.ToString() + ")  " + fromDate + " - " + toDate));
                doc.Add(title);
                doc.Add(pdfImage);
                doc.Close();
            }

            pnContainer.BringToFront();
            grp1.BackColor = oldColor;
            grp2.BackColor = oldColor;
            grp3.BackColor = oldColor;
            pnContainer.BackColor = oldColor;
            pnContainer.Refresh();
        }

        private void BlinkBackButton(object info)
        {
            if (_blinkStop >= 15)
            {
                _tmBackButton.Dispose();
                _blinkStop = 0;
                _img = 0;
            }
            switch (_img)
            {
                case 0:
                    _lblBack.Image = Properties.Resources.back_icon_32;
                    _blinkStop++;
                    _img = 1;
                    break;
                case 1:
                    _lblBack.Image = Properties.Resources.back_icon_orange;
                    _blinkStop++;
                    _img = 0;
                    break;
            }
        }

        private void LoadCharts()
        {
            if (_isReportMode || _sqlHasError) return;
            
            Cursor = Cursors.WaitCursor;

            var glb = new Globals();
            var dtS1 = new DataTable();
            var dtS2 = new DataTable();
            var dtS3 = new DataTable();

            var machines = new System.Text.StringBuilder();
            var cmd = new SqlCommand();
            SqlDataReader dr;

            using (var con = new SqlConnection(conString))
            {
                switch (Mode)
                {
                    case "temperature":
                        {
                            LoadingInfo.InfoText = "Loading charts...";
                            LoadingInfo.ShowLoading();
                            con.Open();
                            cmd = new SqlCommand("get_hum_temp_in_stop_plus", con)
                            {
                                CommandType = CommandType.StoredProcedure
                            };
                            cmd.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = Get_from_date();
                            cmd.Parameters.Add("@toDate", SqlDbType.DateTime).Value = Get_to_date();
                            cmd.Parameters.Add("@shift", SqlDbType.VarChar).Value = Get_shift_array().ToString();
                            dr = cmd.ExecuteReader();
                            dtS1.Load(dr);
                            dr.Close();
                            cmd = null;
                            con.Close();

                            switch (_tempMode)
                            {
                                case "temp":
                                    {
                                        chartSq1.PaletteCustomColors = new Color[] { Color.FromArgb(255, 127, 39) };
                                        chartSq1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
                                        //chart 1
                                        chartSq1.Series["timeSerie"].Points.Clear();                                        
                                        if (chartSq1.Series.IndexOf("ts") == -1 && chartSq2.Series.IndexOf("ts") == -1 &&
                                            chartSq3.Series.IndexOf("ts") == -1)
                                        {
                                            chartSq1.Series.Add("ts");
                                            chartSq2.Series.Add("ts");
                                            chartSq3.Series.Add("ts");
                                        }
                                        chartSq1.Series["ts"].Points.Clear();
                                        chartSq1.Series[0].IsVisibleInLegend = false;
                                        chartSq1.Series[1].IsVisibleInLegend = false;
                                        var position = 0;
                                        foreach (DataRow row in dtS1.Rows)
                                        {                                            
                                            if (row[0].ToString() == "Squadra 1")
                                            {
                                                int.TryParse(row[1].ToString(), out var hour);
                                                double.TryParse(row[2].ToString(), out var temp);
                                                chartSq1.Series["timeSerie"].Points.AddXY(position, temp);
                                                chartSq1.Series["timeSerie"].Points[position].AxisLabel = hour.ToString();
                                                chartSq1.Series["ts"].Points.AddXY(position, temp + 10);
                                                position++;
                                            }
                                        }
                                        chartSq1.Series["timeSerie"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                                        chartSq1.Series["timeSerie"]["PixelPointWidth"] = "10";
                                        chartSq1.ChartAreas[0].AxisY.Maximum = 40;
                                        chartSq1.Series["ts"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                        //chart 2
                                        chartSq2.PaletteCustomColors = new Color[] { Color.FromArgb(255, 127, 39) };
                                        chartSq2.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
                                        chartSq2.Series["timeSerie"].Points.Clear();
                                        chartSq2.Series["ts"].Points.Clear();
                                        chartSq2.Series[0].IsVisibleInLegend = false;
                                        chartSq2.Series[1].IsVisibleInLegend = false;
                                        position = 0;
                                        foreach (DataRow row in dtS1.Rows)
                                        {                                            
                                            if (row[0].ToString() == "Squadra 2")
                                            {
                                                int.TryParse(row[1].ToString(), out var hour);
                                                double.TryParse(row[2].ToString(), out var temp);
                                                chartSq2.Series["timeSerie"].Points.AddXY(position, temp);
                                                chartSq2.Series["timeSerie"].Points[position].AxisLabel = hour.ToString();
                                                chartSq2.Series["ts"].Points.AddXY(position, temp + 10);
                                                position++;
                                            }
                                        }
                                        chartSq2.Series["timeSerie"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                                        chartSq2.Series["timeSerie"]["PixelPointWidth"] = "10";
                                        chartSq2.ChartAreas[0].AxisY.Maximum = 40;
                                        chartSq2.Series["ts"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                        //chart 3
                                        chartSq3.PaletteCustomColors = new Color[] { Color.FromArgb(255, 127, 39) };
                                        chartSq3.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
                                        chartSq3.Series["timeSerie"].Points.Clear();
                                        chartSq3.Series["ts"].Points.Clear();
                                        chartSq3.Series[0].IsVisibleInLegend = false;
                                        chartSq3.Series[1].IsVisibleInLegend = false;
                                        position = 0;
                                        foreach (DataRow row in dtS1.Rows)
                                        {                                            
                                            if (row[0].ToString() == "Squadra 3")
                                            {
                                                int.TryParse(row[1].ToString(), out var hour);
                                                double.TryParse(row[2].ToString(), out var temp);
                                                chartSq3.Series["timeSerie"].Points.AddXY(position, temp);
                                                chartSq3.Series["timeSerie"].Points[position].AxisLabel = hour.ToString();
                                                chartSq3.Series["ts"].Points.AddXY(position, temp + 10);
                                                position++;
                                            }
                                        }
                                        chartSq3.Series["timeSerie"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                                        chartSq3.Series["timeSerie"]["PixelPointWidth"] = "10";
                                        chartSq3.ChartAreas[0].AxisY.Maximum = 40;
                                        chartSq3.Series["ts"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                        break;
                                    }
                                case "hum":
                                    {
                                        chartSq1.PaletteCustomColors = new Color[] { Color.FromArgb(0, 125, 179) };
                                        chartSq1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;                                        
                                        chartSq1.Series["timeSerie"].Points.Clear();
                                        if (chartSq1.Series.IndexOf("ts") == -1 && chartSq2.Series.IndexOf("ts") == -1 &&
                                            chartSq3.Series.IndexOf("ts") == -1)
                                        {
                                            chartSq1.Series.Add("ts");
                                            chartSq2.Series.Add("ts");
                                            chartSq3.Series.Add("ts");
                                        }
                                        chartSq1.Series["ts"].Points.Clear();
                                        chartSq1.Series[0].IsVisibleInLegend = false;
                                        chartSq1.Series[1].IsVisibleInLegend = false;
                                        var position = 0;
                                        foreach (DataRow row in dtS1.Rows)
                                        {
                                            if (row[0].ToString() == "Squadra 1")
                                            {
                                                int.TryParse(row[1].ToString(), out var hour);
                                                double.TryParse(row[3].ToString(), out var hum);
                                                chartSq1.Series["timeSerie"].Points.AddXY(position, hum);
                                                chartSq1.Series["timeSerie"].Points[position].AxisLabel = hour.ToString();
                                                chartSq1.Series["ts"].Points.AddXY(position, hum + 10);
                                                position++;
                                            }
                                        }
                                        chartSq1.Series["timeSerie"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                                        chartSq1.Series["timeSerie"]["PixelPointWidth"] = "10";
                                        chartSq1.ChartAreas[0].AxisY.Maximum = 50;
                                        chartSq1.Series["ts"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                        chartSq2.PaletteCustomColors = new Color[] { Color.FromArgb(0, 125, 179) };
                                        chartSq2.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
                                        chartSq2.Series["timeSerie"].Points.Clear();
                                        chartSq2.Series["ts"].Points.Clear();
                                        chartSq2.Series[0].IsVisibleInLegend = false;
                                        chartSq2.Series[1].IsVisibleInLegend = false;
                                        position = 0;
                                        foreach (DataRow row in dtS1.Rows)
                                        {
                                            if (row[0].ToString() == "Squadra 2")
                                            {
                                                int.TryParse(row[1].ToString(), out var hour);
                                                double.TryParse(row[3].ToString(), out var hum);
                                                chartSq2.Series["timeSerie"].Points.AddXY(position, hum);
                                                chartSq2.Series["timeSerie"].Points[position].AxisLabel = hour.ToString();
                                                chartSq2.Series["ts"].Points.AddXY(position, hum + 10);
                                                position++;
                                            }
                                        }
                                        chartSq2.Series["timeSerie"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                                        chartSq2.Series["timeSerie"]["PixelPointWidth"] = "10";
                                        chartSq2.ChartAreas[0].AxisY.Maximum = 50;
                                        chartSq2.Series["ts"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                        chartSq3.PaletteCustomColors = new Color[] { Color.FromArgb(0, 125, 179) };
                                        chartSq3.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
                                        chartSq3.Series["timeSerie"].Points.Clear();
                                        chartSq3.Series["ts"].Points.Clear();
                                        chartSq3.Series[0].IsVisibleInLegend = false;
                                        chartSq3.Series[1].IsVisibleInLegend = false;
                                        position = 0;
                                        foreach (DataRow row in dtS1.Rows)
                                        {
                                            if (row[0].ToString() == "Squadra 3")
                                            {
                                                int.TryParse(row[1].ToString(), out var hour);
                                                double.TryParse(row[3].ToString(), out var hum);
                                                chartSq3.Series["timeSerie"].Points.AddXY(position, hum);
                                                chartSq3.Series["timeSerie"].Points[position].AxisLabel = hour.ToString();
                                                chartSq3.Series["ts"].Points.AddXY(position, hum + 10);
                                                position++;
                                            }
                                        }
                                        chartSq3.Series["timeSerie"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                                        chartSq3.Series["timeSerie"]["PixelPointWidth"] = "10";
                                        chartSq3.ChartAreas[0].AxisY.Maximum = 50;
                                        chartSq3.Series["ts"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                        break;
                                    }
                            }
                            break;
                        }
                    case "eff":
                        {
                            LoadingInfo.InfoText = "Loading charts...";
                            LoadingInfo.ShowLoading();

                            con.Open();

                            machines.Append(",");
                            for (var i = 1; i <= 70; i++)
                            {
                                machines.Append(i.ToString() + ",");
                            }

                            cmd = new SqlCommand("get_data_in_stop_plus", con);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@from_date", SqlDbType.DateTime).Value = Get_from_date();
                            cmd.Parameters.Add("@to_date", SqlDbType.DateTime).Value = Get_to_date();
                            cmd.Parameters.Add("@shift", SqlDbType.VarChar).Value = Get_shift_array().ToString();
                            cmd.Parameters.Add("@machine", SqlDbType.VarChar).Value = machines.ToString();
                            //cmd.Parameters.Add("@machine_range", SqlDbType.Int).Value = 70;

                            dr = cmd.ExecuteReader();
                            dtS1.Load(dr);
                            dr.Close();
                            cmd = null;

                            chartsPopulated = true;
                            //clear val matrix
                            for (var i = 0; i < 3; i++)
                                for (var j = 0; j < 4; j++)
                                {
                                    _timeValues[i, j] = 0;
                                }
                            if (chartSq1.Series.IndexOf("ts") != -1 && chartSq2.Series.IndexOf("ts") != -1 &&
                                chartSq3.Series.IndexOf("ts") != -1)
                            {
                                chartSq1.Series.RemoveAt(1);
                                chartSq2.Series.RemoveAt(1);
                                chartSq3.Series.RemoveAt(1);
                            }
                            chartSq1.PaletteCustomColors = new Color[] { Color.FromArgb(0, 191, 254), Color.FromArgb(255, 2, 0), Color.FromArgb(131, 0, 125),
                                                                         Color.FromArgb(254, 215, 1)};
                            chartSq1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
                            chartSq1.Series["timeSerie"].Points.Clear();
                            chartSq1.Series[0].IsVisibleInLegend = true;
                            
                            int _knit = 0, _manual = 0, _yarn = 0, _generics = 0;
                            foreach (DataRow row in dtS1.Rows)
                            {
                                int knit = Convert.ToInt32(row[1]) > 0 ? Convert.ToInt32(row[1]) : knit = 0;
                                int comb = Convert.ToInt32(row[2]) > 0 ? Convert.ToInt32(row[2]) : comb = 0;
                                int manual = Convert.ToInt32(row[3]) > 0 ? Convert.ToInt32(row[3]) : manual = 0;
                                int yarn = Convert.ToInt32(row[4]) > 0 ? Convert.ToInt32(row[4]) : yarn = 0;
                                int needle = Convert.ToInt32(row[5]) > 0 ? Convert.ToInt32(row[5]) : needle = 0;
                                int shock = Convert.ToInt32(row[6]) > 0 ? Convert.ToInt32(row[6]) : shock = 0;
                                int roller = Convert.ToInt32(row[7]) > 0 ? Convert.ToInt32(row[7]) : roller = 0;
                                int other = Convert.ToInt32(row[8]) > 0 ? Convert.ToInt32(row[8]) : other = 0;

                                _knit += knit;
                                _manual += manual;
                                _yarn += yarn;
                                _generics = comb + needle + shock + roller + other;
                            }

                            _timeValues[0, 0] = _knit;
                            _timeValues[0, 1] = _manual;
                            _timeValues[0, 2] = _yarn;
                            _timeValues[0, 3] = _generics;

                            chartSq1.Series["timeSerie"].Points.AddXY(string.Empty, _knit);
                            chartSq1.Series["timeSerie"].Points.AddXY(string.Empty, _manual);
                            chartSq1.Series["timeSerie"].Points.AddXY(string.Empty, _yarn);
                            chartSq1.Series["timeSerie"].Points.AddXY(string.Empty, _generics);

                            chartSq1.Series["timeSerie"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;

                            chartSq1.Series["timeSerie"].Points[0].LegendText = "Knitt      "/* + _knit.ToString()*/;
                            chartSq1.Series["timeSerie"].Points[1].LegendText = "Manual     "/* + _manual.ToString()*/;
                            chartSq1.Series["timeSerie"].Points[2].LegendText = "Yarn       "/* + _yarn.ToString()*/;
                            chartSq1.Series["timeSerie"].Points[3].LegendText = "Generic    "/* + _generics.ToString()*/;
                            chartSq1.Titles["titleSquadra1"].Visible = true;

                            //block 2

                            machines = new System.Text.StringBuilder();
                            machines.Append(",");
                            for (var i = 71; i <= 140; i++)
                            {
                                machines.Append(i.ToString() + ",");
                            }

                            cmd = new SqlCommand("get_data_in_stop_plus", con);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@from_date", SqlDbType.DateTime).Value = Get_from_date();
                            cmd.Parameters.Add("@to_date", SqlDbType.DateTime).Value = Get_to_date();
                            cmd.Parameters.Add("@shift", SqlDbType.VarChar).Value = Get_shift_array().ToString();
                            cmd.Parameters.Add("@machine", SqlDbType.VarChar).Value = machines.ToString();

                            dr = cmd.ExecuteReader();
                            dtS2.Load(dr);
                            dr.Close();
                            cmd = null;
                            chartSq2.PaletteCustomColors = new Color[] { Color.FromArgb(0, 191, 254), Color.FromArgb(255, 2, 0), Color.FromArgb(131, 0, 125),
                                                                         Color.FromArgb(254, 215, 1)};
                            chartSq2.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
                            chartSq2.Series["timeSerie"].Points.Clear();
                            chartSq2.Series[0].IsVisibleInLegend = true;

                            _knit = 0;
                            _manual = 0;
                            _yarn = 0;
                            _generics = 0;
                            foreach (DataRow row in dtS2.Rows)
                            {
                                int knit = Convert.ToInt32(row[1]) > 0 ? Convert.ToInt32(row[1]) : knit = 0;
                                int comb = Convert.ToInt32(row[2]) > 0 ? Convert.ToInt32(row[2]) : comb = 0;
                                int manual = Convert.ToInt32(row[3]) > 0 ? Convert.ToInt32(row[3]) : manual = 0;
                                int yarn = Convert.ToInt32(row[4]) > 0 ? Convert.ToInt32(row[4]) : yarn = 0;
                                int needle = Convert.ToInt32(row[5]) > 0 ? Convert.ToInt32(row[5]) : needle = 0;
                                int shock = Convert.ToInt32(row[6]) > 0 ? Convert.ToInt32(row[6]) : shock = 0;
                                int roller = Convert.ToInt32(row[7]) > 0 ? Convert.ToInt32(row[7]) : roller = 0;
                                int other = Convert.ToInt32(row[8]) > 0 ? Convert.ToInt32(row[8]) : other = 0;

                                _knit += knit;
                                _manual += manual;
                                _yarn += yarn;
                                _generics = comb + needle + shock + roller + other;
                            }

                            _timeValues[1, 0] = _knit;
                            _timeValues[1, 1] = _manual;
                            _timeValues[1, 2] = _yarn;
                            _timeValues[1, 3] = _generics;

                            chartSq2.Series["timeSerie"].Points.AddXY(string.Empty, _knit);
                            chartSq2.Series["timeSerie"].Points.AddXY(string.Empty, _manual);
                            chartSq2.Series["timeSerie"].Points.AddXY(string.Empty, _yarn);
                            chartSq2.Series["timeSerie"].Points.AddXY(string.Empty, _generics);

                            chartSq2.Series["timeSerie"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
                            chartSq2.Series["timeSerie"].Points[0].LegendText = "Knitt      "/* + _knit.ToString()*/;
                            chartSq2.Series["timeSerie"].Points[1].LegendText = "Manual     "/* + _manual.ToString()*/;
                            chartSq2.Series["timeSerie"].Points[2].LegendText = "Yarn       "/* + _yarn.ToString()*/;
                            chartSq2.Series["timeSerie"].Points[3].LegendText = "Generic    "/* + _generics.ToString()*/;
                            chartSq2.Titles["titleSquadra2"].Visible = true;

                            //block 3

                            machines = new System.Text.StringBuilder();
                            machines.Append(",");
                            for (var i = 141; i <= 210; i++)
                            {
                                machines.Append(i.ToString() + ",");
                            }

                            cmd = new SqlCommand("get_data_in_stop_plus", con);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@from_date", SqlDbType.DateTime).Value = Get_from_date();
                            cmd.Parameters.Add("@to_date", SqlDbType.DateTime).Value = Get_to_date();
                            cmd.Parameters.Add("@shift", SqlDbType.VarChar).Value = Get_shift_array().ToString();
                            cmd.Parameters.Add("@machine", SqlDbType.VarChar).Value = machines.ToString();

                            dr = cmd.ExecuteReader();
                            dtS3.Load(dr);
                            dr.Close();
                            cmd = null;
                            chartSq3.PaletteCustomColors = new Color[] { Color.FromArgb(0, 191, 254), Color.FromArgb(255, 2, 0), Color.FromArgb(131, 0, 125),
                                                                         Color.FromArgb(254, 215, 1)};
                            chartSq3.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
                            chartSq3.Series["timeSerie"].Points.Clear();
                            chartSq3.Series[0].IsVisibleInLegend = true;

                            _knit = 0;
                            _manual = 0;
                            _yarn = 0;
                            _generics = 0;
                            foreach (DataRow row in dtS3.Rows)
                            {
                                int knit = Convert.ToInt32(row[1]) > 0 ? Convert.ToInt32(row[1]) : knit = 0;
                                int comb = Convert.ToInt32(row[2]) > 0 ? Convert.ToInt32(row[2]) : comb = 0;
                                int manual = Convert.ToInt32(row[3]) > 0 ? Convert.ToInt32(row[3]) : manual = 0;
                                int yarn = Convert.ToInt32(row[4]) > 0 ? Convert.ToInt32(row[4]) : yarn = 0;
                                int needle = Convert.ToInt32(row[5]) > 0 ? Convert.ToInt32(row[5]) : needle = 0;
                                int shock = Convert.ToInt32(row[6]) > 0 ? Convert.ToInt32(row[6]) : shock = 0;
                                int roller = Convert.ToInt32(row[7]) > 0 ? Convert.ToInt32(row[7]) : roller = 0;
                                int other = Convert.ToInt32(row[8]) > 0 ? Convert.ToInt32(row[8]) : other = 0;

                                _knit += knit;
                                _manual += manual;
                                _yarn += yarn;
                                _generics = comb + needle + shock + roller + other;
                            }

                            _timeValues[2, 0] = _knit;
                            _timeValues[2, 1] = _manual;
                            _timeValues[2, 2] = _yarn;
                            _timeValues[2, 3] = _generics;

                            chartSq3.Series["timeSerie"].Points.AddXY(string.Empty, _knit);
                            chartSq3.Series["timeSerie"].Points.AddXY(string.Empty, _manual);
                            chartSq3.Series["timeSerie"].Points.AddXY(string.Empty, _yarn);
                            chartSq3.Series["timeSerie"].Points.AddXY(string.Empty, _generics);

                            chartSq3.Series["timeSerie"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
                            chartSq3.Series["timeSerie"].Points[0].LegendText = "Knitt      "/* + _knit.ToString()*/;
                            chartSq3.Series["timeSerie"].Points[1].LegendText = "Manual     "/* + _manual.ToString()*/;
                            chartSq3.Series["timeSerie"].Points[2].LegendText = "Yarn       "/* + _yarn.ToString()*/;
                            chartSq3.Series["timeSerie"].Points[3].LegendText = "Generic    "/* + _generics.ToString()*/;

                            chartSq3.Titles["titleSquadra3"].Visible = true;
                            con.Close();
                            break;
                        }
                    case "scarti":
                    case "rammendi":
                        {
                            LoadingInfo.InfoText = "Loading charts...";
                            LoadingInfo.ShowLoading();

                            con.Open();

                            machines.Append(",");
                            for (var i = 1; i <= 70; i++)
                            {
                                machines.Append(i.ToString() + ",");
                            }

                            cmd = new SqlCommand("getscartirammendidatainstop", con);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@from_date", SqlDbType.DateTime).Value = Get_from_date();
                            cmd.Parameters.Add("@to_date", SqlDbType.DateTime).Value = Get_to_date();
                            cmd.Parameters.Add("@shift", SqlDbType.VarChar).Value = Get_shift_array().ToString();
                            cmd.Parameters.Add("@machine", SqlDbType.VarChar).Value = machines.ToString();

                            dr = cmd.ExecuteReader();
                            dtS1.Load(dr);
                            dr.Close();
                            cmd = null;

                            chartsPopulated = true;
                            //clear val matrix
                            for (var i = 0; i < 3; i++)
                                for (var j = 0; j < 4; j++)
                                {
                                    _timeValues[i, j] = 0;
                                }
                            if (chartSq1.Series.IndexOf("ts") != -1 && chartSq2.Series.IndexOf("ts") != -1 &&
                                chartSq3.Series.IndexOf("ts") != -1)
                            {
                                chartSq1.Series.RemoveAt(1);
                                chartSq2.Series.RemoveAt(1);
                                chartSq3.Series.RemoveAt(1);
                            }
                            chartSq1.PaletteCustomColors = new Color[] { Color.FromArgb(0, 191, 254), Color.FromArgb(255, 2, 0), Color.FromArgb(131, 0, 125),
                                                                         Color.FromArgb(254, 215, 1)};
                            chartSq1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
                            chartSq1.Series["timeSerie"].Points.Clear();
                            chartSq1.Series[0].IsVisibleInLegend = true;

                            int _teli = 0, _scarti = 0, _rammendi = 0;
                            foreach (DataRow row in dtS1.Rows)
                            {
                                int teli = Convert.ToInt32(row[1]) > 0 ? Convert.ToInt32(row[1]) : teli = 0;
                                int scarti = Convert.ToInt32(row[2]) > 0 ? Convert.ToInt32(row[2]) : scarti = 0;
                                int rammendi = Convert.ToInt32(row[3]) > 0 ? Convert.ToInt32(row[3]) : rammendi = 0;

                                _teli += teli;
                                _scarti += scarti;
                                _rammendi += rammendi;
                            }

                            _timeValues[0, 0] = _teli;
                            _timeValues[0, 1] = _scarti;
                            _timeValues[0, 2] = _rammendi;
                            //_timeValues[0, 3] = _generics;

                            chartSq1.Series["timeSerie"].Points.AddXY(string.Empty, _teli);
                            chartSq1.Series["timeSerie"].Points.AddXY(string.Empty, _scarti);
                            chartSq1.Series["timeSerie"].Points.AddXY(string.Empty, _rammendi);

                            chartSq1.Series["timeSerie"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;

                            chartSq1.Series["timeSerie"].Points[0].LegendText = "TeliBuoni      ";
                            chartSq1.Series["timeSerie"].Points[1].LegendText = "Scarti         ";
                            chartSq1.Series["timeSerie"].Points[2].LegendText = "Rammendi       ";
                            chartSq1.Titles["titleSquadra1"].Visible = true;

                            //block 2

                            machines = new System.Text.StringBuilder();
                            machines.Append(",");
                            for (var i = 71; i <= 140; i++)
                            {
                                machines.Append(i.ToString() + ",");
                            }

                            cmd = new SqlCommand("getscartirammendidatainstop", con);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@from_date", SqlDbType.DateTime).Value = Get_from_date();
                            cmd.Parameters.Add("@to_date", SqlDbType.DateTime).Value = Get_to_date();
                            cmd.Parameters.Add("@shift", SqlDbType.VarChar).Value = Get_shift_array().ToString();
                            cmd.Parameters.Add("@machine", SqlDbType.VarChar).Value = machines.ToString();

                            dr = cmd.ExecuteReader();
                            dtS2.Load(dr);
                            dr.Close();
                            cmd = null;
                            chartSq2.PaletteCustomColors = new Color[] { Color.FromArgb(0, 191, 254), Color.FromArgb(255, 2, 0), Color.FromArgb(131, 0, 125),
                                                                         Color.FromArgb(254, 215, 1)};
                            chartSq2.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
                            chartSq2.Series["timeSerie"].Points.Clear();
                            chartSq2.Series[0].IsVisibleInLegend = true;

                            _teli = 0;
                            _scarti = 0;
                            _rammendi = 0;
                            foreach (DataRow row in dtS2.Rows)
                            {
                                int teli = Convert.ToInt32(row[1]) > 0 ? Convert.ToInt32(row[1]) : teli = 0;
                                int scarti = Convert.ToInt32(row[2]) > 0 ? Convert.ToInt32(row[2]) : scarti = 0;
                                int rammendi = Convert.ToInt32(row[3]) > 0 ? Convert.ToInt32(row[3]) : rammendi = 0;

                                _teli += teli;
                                _scarti += scarti;
                                _rammendi += rammendi;
                            }

                            _timeValues[1, 0] = _teli;
                            _timeValues[1, 1] = _scarti;
                            _timeValues[1, 2] = _rammendi;
                            //_timeValues[1, 3] = _generics;

                            chartSq2.Series["timeSerie"].Points.AddXY(string.Empty, _teli);
                            chartSq2.Series["timeSerie"].Points.AddXY(string.Empty, _scarti);
                            chartSq2.Series["timeSerie"].Points.AddXY(string.Empty, _rammendi);

                            chartSq2.Series["timeSerie"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
                            chartSq2.Series["timeSerie"].Points[0].LegendText = "TeliBuoni      ";
                            chartSq2.Series["timeSerie"].Points[1].LegendText = "Scarti         ";
                            chartSq2.Series["timeSerie"].Points[2].LegendText = "Rammendi       ";
                            chartSq2.Titles["titleSquadra2"].Visible = true;

                            //block 3

                            machines = new System.Text.StringBuilder();
                            machines.Append(",");
                            for (var i = 141; i <= 210; i++)
                            {
                                machines.Append(i.ToString() + ",");
                            }

                            cmd = new SqlCommand("getscartirammendidatainstop", con);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@from_date", SqlDbType.DateTime).Value = Get_from_date();
                            cmd.Parameters.Add("@to_date", SqlDbType.DateTime).Value = Get_to_date();
                            cmd.Parameters.Add("@shift", SqlDbType.VarChar).Value = Get_shift_array().ToString();
                            cmd.Parameters.Add("@machine", SqlDbType.VarChar).Value = machines.ToString();

                            dr = cmd.ExecuteReader();
                            dtS3.Load(dr);
                            dr.Close();
                            cmd = null;
                            chartSq3.PaletteCustomColors = new Color[] { Color.FromArgb(0, 191, 254), Color.FromArgb(255, 2, 0), Color.FromArgb(131, 0, 125),
                                                                         Color.FromArgb(254, 215, 1)};
                            chartSq3.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
                            chartSq3.Series["timeSerie"].Points.Clear();
                            chartSq3.Series[0].IsVisibleInLegend = true;

                            _teli = 0;
                            _scarti = 0;
                            _rammendi = 0;
                            foreach (DataRow row in dtS3.Rows)
                            {
                                int teli = Convert.ToInt32(row[1]) > 0 ? Convert.ToInt32(row[1]) : teli = 0;
                                int scarti = Convert.ToInt32(row[2]) > 0 ? Convert.ToInt32(row[2]) : scarti = 0;
                                int rammendi = Convert.ToInt32(row[3]) > 0 ? Convert.ToInt32(row[3]) : rammendi = 0;

                                _teli += teli;
                                _scarti += scarti;
                                _rammendi += rammendi;
                            }

                            _timeValues[2, 0] = _teli;
                            _timeValues[2, 1] = _scarti;
                            _timeValues[2, 2] = _rammendi;
                            //_timeValues[2, 3] = _generics;

                            chartSq3.Series["timeSerie"].Points.AddXY(string.Empty, _teli);
                            chartSq3.Series["timeSerie"].Points.AddXY(string.Empty, _scarti);
                            chartSq3.Series["timeSerie"].Points.AddXY(string.Empty, _rammendi);

                            chartSq3.Series["timeSerie"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
                            chartSq3.Series["timeSerie"].Points[0].LegendText = "TeliBuoni      ";
                            chartSq3.Series["timeSerie"].Points[1].LegendText = "Scarti         ";
                            chartSq3.Series["timeSerie"].Points[2].LegendText = "Rammendi       ";

                            chartSq3.Titles["titleSquadra3"].Visible = true;
                            con.Close();
                            break;
                        }
                }
                LoadingInfo.CloseLoading();
                Cursor = Cursors.Default;
            }
        }

        private void btnBarChart_Click(object sender, EventArgs e)
        {
            if (_tmDelay != null) return;

            if (_isReportMode) return;

            var dayRange = Get_to_date().Subtract(Get_from_date()).Days;

            if (dayRange == 0 && Mode == "eff" || Mode == "scarti" || Mode == "rammendi"
                || Mode == "temperature")
            {
                GetCharts();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!IsFullScreen) return;

            Point pt = new Point();
            GetCursorPos(ref pt);

            if (_btnExitFullScreen.Location.Y > 0)
            {
                if (pt.Y > _btnExitFullScreen.Location.Y + _btnExitFullScreen.Height + 50)
                {
                    //hide button
                    for (var l = _btnExitFullScreen.Location.Y; l >= -60; l -= 20)
                    {
                        _btnExitFullScreen.Location = new Point((Width / 2) - (_btnExitFullScreen.Width / 2), l);
                        Thread.Sleep(10);
                        _btnExitFullScreen.Refresh();
                    }
                }

            }
            else
            {
                if (pt.Y <= 0)
                {
                    _btnExitFullScreen.BringToFront();
                    if (Controls.Contains(_btnExitFullScreen))
                    {
                        //show button
                        for (var l = -60; l <= 80; l += 20)
                        {
                            _btnExitFullScreen.Location = new Point((Width / 2) - (_btnExitFullScreen.Width / 2), l);
                            Thread.Sleep(10);
                            _btnExitFullScreen.Refresh();
                        }
                    }
                }
            }
        }

        #region Borders

        private Pen pen = new Pen(Brushes.Gainsboro, 0);

        private void Labels_Paint(object sender, PaintEventArgs eventArgs)
        {
            var lbl = (Label)sender;
            var border_pen = new Pen(_brush_active, 2);
            var targetCode = lbl.Name.Substring(0, 1);
            SolidBrush strBrush = new SolidBrush(Color.FromArgb(100, 100, 100));

            if (targetCode == "P")
            {
                var pen = new Pen(new SolidBrush(lbl.BackColor), 0);
                //eventArgs.Graphics.DrawRectangle(border_pen, new Rectangle(-1,-1, lbl.Width, lbl.Height));
                using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(-1, -1, lbl.Width + 1, lbl.Height + 1), 4))
                {
                    SmoothingMode old = eventArgs.Graphics.SmoothingMode;
                    eventArgs.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    //e.Graphics.FillPath(Brushes.Black, path);
                    eventArgs.Graphics.DrawPath(pen, path);

                    lbl.Region = new Region(path);
                    eventArgs.Graphics.SmoothingMode = old;
                }

                //eventArgs.Graphics.DrawRectangle(border_pen, new Rectangle(-1, -1, lbl.Width, lbl.Height));
                //eventArgs.Graphics.FillRectangle(new SolidBrush(lbl.BackColor), 2, 2, 15, lbl.Height - 8);

                var macNumber = lbl.Name.Substring(1, lbl.Name.Length - 1);

                var strToDraw = macNumber.PadLeft(3, '0');

                if (Mode == "eff")
                {
                    foreach (var item in _list_of_currents)
                    {
                        var machineNumber = item.MachineNumber;
                        var finest = item.Gaudge;

                        if (macNumber != machineNumber) continue;

                        // Draws machine numbers and finesses

                        eventArgs.Graphics.DrawString(machineNumber.PadLeft(3, '0'), new Font("Tahoma", 6, FontStyle.Regular), strBrush, 1, 2);
                        eventArgs.Graphics.DrawString(finest, new Font("Tahoma", 6, FontStyle.Regular), strBrush, 1, 10);

                        if (item.Alarm)
                        {
                            eventArgs.Graphics.DrawImage(Sinotico.Properties.Resources.dorbell_icons8,
                                                         new Rectangle(lbl.Width / 3 - 2, lbl.Height / 2 - 3, 10, 10));
                        }
                    }
                }
                else if (Mode != "temperature")
                {
                    foreach (var item in _list_of_currents)
                    {
                        var machineNumber = item.MachineNumber;
                        var finest = item.Gaudge;

                        if (macNumber != machineNumber) continue;

                        // Draws machine numbers and finesses

                        eventArgs.Graphics.DrawString(machineNumber.PadLeft(3, '0'), new Font("Tahoma", 6, FontStyle.Regular), strBrush, 1, 2);
                        eventArgs.Graphics.DrawString(finest, new Font("Tahoma", 6, FontStyle.Regular), strBrush, 1, 10);

                        if ((item.Alarm && Mode == "eff") ||
                            (item.Cleaned == "3_months_not_cleaned" && Mode == "cleaner" && _cleanersType != "cquality") ||
                            (item.Alarm && Mode == "cleaner" && _cleanersType == "cquality"))                        
                            eventArgs.Graphics.DrawImage(Sinotico.Properties.Resources.dorbell_icons8,
                                                     new Rectangle(lbl.Width / 3 - 7, lbl.Height / 2 - 3, 10, 10));
                    }
                }
            }
            else if (targetCode == "L")
            {
                using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(0, 0, lbl.Width, lbl.Height), 3))
                {
                    SmoothingMode old = eventArgs.Graphics.SmoothingMode;
                    eventArgs.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    //e.Graphics.FillPath(Brushes.Black, path);
                    eventArgs.Graphics.DrawPath(pen, path);

                    lbl.Region = new Region(path);
                    eventArgs.Graphics.SmoothingMode = old;
                }
                //eventArgs.Graphics.DrawRectangle(border_pen, new Rectangle(-1, -1, lbl.Width, lbl.Height));
                //Draws line number (Ln)
                if (Mode != "temperature")
                    eventArgs.Graphics.DrawString(lbl.Name, new Font("Tahoma", 10, FontStyle.Regular), strBrush, 2, 1);
                else
                    eventArgs.Graphics.DrawString(lbl.Name, new Font("Tahoma", 10, FontStyle.Regular), new SolidBrush(Color.White), 2, 1);
            }
            else if (targetCode == "S")
            {
                var pen = new Pen(new SolidBrush(lbl.BackColor), 6);
                //eventArgs.Graphics.DrawRectangle(border_pen, new Rectangle(-1,-1, lbl.Width, lbl.Height));
                using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(0, 0, lbl.Width, lbl.Height), 6))
                {
                    SmoothingMode old = eventArgs.Graphics.SmoothingMode;
                    eventArgs.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    //e.Graphics.FillPath(Brushes.Black, path);
                    eventArgs.Graphics.DrawPath(pen, path);

                    lbl.Region = new Region(path);
                    eventArgs.Graphics.SmoothingMode = old;
                }
            }
        }

        private void lblTotDep_Paint(object sender, PaintEventArgs e)
        {
            var lbl = (Label)sender;
            var machinePen = new Pen(_brush_active, 0); //frame
            var pen = new Pen(Brushes.Gainsboro, 1);

            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(0, 0, lbl.Width, lbl.Height), 4))
            {
                SmoothingMode old = e.Graphics.SmoothingMode;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                //e.Graphics.FillPath(Brushes.Black, path);
                e.Graphics.DrawPath(pen, path);

                lbl.Region = new Region(path);
                e.Graphics.SmoothingMode = old;
            }
        }

        private void pnDateTime_Paint(object sender, PaintEventArgs e)
        {
            //var pn = (Panel)sender;
            //e.Graphics.DrawRectangle(pen, 1, 1, pn.Width - 3, pn.Height - 3);
        }

        //private void panel2_Paint(object sender, PaintEventArgs e)
        //{
        //    //var pn = (Panel)sender;
        //    //e.Graphics.DrawRectangle(pen, 1, 1, pn.Width - 3, pn.Height - 3);
        //}

        private void button2_Click_1(object sender, EventArgs e)
        {
            ResetOperatorsFields();
            var btn = sender as Button;
            foreach (var b in _listOfJobs)
            {
                if (btn.Name == b.Name) b.Image = Properties.Resources.modes1_green;
                else b.Image = Properties.Resources.modes1_silver;
            }
            Mode = "cleaner";
            _cleanersType = "Pulizia Fronture";
            SetDefaultFilters(false);

            //cboArt.SelectedIndex = 0;
            _isArticleSelected = false;
            Set_file_name(string.Empty);

            GetData();
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            ResetOperatorsFields();
            var btn = sender as Button;
            foreach (var b in _listOfJobs)
            {
                if (btn.Name == b.Name) b.Image = Properties.Resources.modes1_green;
                else b.Image = Properties.Resources.modes1_silver;
            }
            Mode = "cleaner";
            _cleanersType = "Pulizia Ordinaria";

            SetDefaultFilters(false);

            //cboArt.SelectedIndex = 0;

            _isArticleSelected = false;
            Set_file_name(string.Empty);

            GetData();
        }

        private bool chartsPopulated = false;
        private void chartSq1_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.DrawRectangle(pen, 1, 1, chartSq1.Width, chartSq1.Height - 3);
            var g = (System.Windows.Forms.DataVisualization.Charting.Chart)sender;
            var h = g.Size.Height;
            var w = g.Size.Width;
            var p = new Pen(Brushes.Silver, 1);
            e.Graphics.DrawLine(p, 0, 0, 0, h);
            e.Graphics.DrawLine(p, 5, 0, w, 0);
            e.Graphics.DrawLine(p, 5, 0, 5, h - 1);

            if (chartsPopulated && Mode != "temperature" && Mode != "scarti" && Mode != "rammendi")
            {
                int y = 29;
                double total = 0;
                for (var i = 0; i < 4; i++)
                {
                    var val = Convert.ToDouble(_timeValues[0, i]);
                    total += val;
                    var formatedTs = TimeSpan.FromMinutes(val);
                    string timeFormat = string.Format("{0}:{1}",
                                                  (int)formatedTs.TotalHours < 10 ?
                                                  "0" + ((int)formatedTs.TotalHours).ToString() :
                                                  ((int)formatedTs.TotalHours).ToString(),
                                                  (int)formatedTs.Minutes < 10 ?
                                                  "0" + ((int)formatedTs.Minutes).ToString() :
                                                  ((int)formatedTs.Minutes).ToString());
                    e.Graphics.DrawString(timeFormat,
                                          new Font("Tahoma", 8, FontStyle.Regular),
                                          new SolidBrush(Color.FromArgb(109, 109, 109)),
                                          new PointF(g.Width - 32, y));
                    y += 15;
                }
                var totalTs = TimeSpan.FromMinutes(total);
                string totalTsFormat = string.Format("{0}:{1}",
                                                  (int)totalTs.TotalHours < 10 ?
                                                  "0" + ((int)totalTs.TotalHours).ToString() :
                                                  ((int)totalTs.TotalHours).ToString(),
                                                  (int)totalTs.Minutes < 10 ?
                                                  "0" + ((int)totalTs.Minutes).ToString() :
                                                  ((int)totalTs.Minutes).ToString());
                e.Graphics.DrawString("Total        " + totalTsFormat,
                                      new Font("Tahoma", 8, FontStyle.Regular),
                                      new SolidBrush(Color.FromArgb(109, 109, 109)),
                                      new PointF(g.Width - 79, y));
            }
            else if (chartsPopulated && Mode == "scarti" || Mode == "rammendi")
            {
                int y = 29;
                //double total = 0;
                for (var i = 0; i < 3; i++)
                {
                    e.Graphics.DrawString(_timeValues[0, i].ToString(),
                                          new Font("Tahoma", 8, FontStyle.Regular),
                                          new SolidBrush(Color.FromArgb(109, 109, 109)),
                                          new PointF(g.Width - 32, y));
                    y += 15;
                }
            }
            p.Dispose();
        }

        private void chartSq2_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.DrawRectangle(pen, 1, 1, chartSq2.Width - 3, chartSq2.Height - 3);
            var g = (System.Windows.Forms.DataVisualization.Charting.Chart)sender;
            var h = g.Size.Height;
            var w = g.Size.Width;
            var p = new Pen(Brushes.Silver, 1);
            e.Graphics.DrawLine(p, 0, 0, 0, h);
            e.Graphics.DrawRectangle(p, 5, 0, w, h - 1);

            if (chartsPopulated && Mode != "temperature" && Mode != "scarti" && Mode != "rammendi")
            {
                int y = 29;
                double total = 0;
                for (var i = 0; i < 4; i++)
                {
                    var val = Convert.ToDouble(_timeValues[1, i]);
                    total += val;
                    var formatedTs = TimeSpan.FromMinutes(val);
                    string timeFormat = string.Format("{0}:{1}",
                                                  (int)formatedTs.TotalHours < 10 ?
                                                  "0" + ((int)formatedTs.TotalHours).ToString() :
                                                  ((int)formatedTs.TotalHours).ToString(),
                                                  (int)formatedTs.Minutes < 10 ?
                                                  "0" + ((int)formatedTs.Minutes).ToString() :
                                                  ((int)formatedTs.Minutes).ToString());
                    e.Graphics.DrawString(timeFormat,
                                          new Font("Tahoma", 8, FontStyle.Regular),
                                          new SolidBrush(Color.FromArgb(109, 109, 109)),
                                          new PointF(g.Width - 32, y));
                    y += 15;
                }
                var totalTs = TimeSpan.FromMinutes(total);
                string totalTsFormat = string.Format("{0}:{1}",
                                                  (int)totalTs.TotalHours < 10 ?
                                                  "0" + ((int)totalTs.TotalHours).ToString() :
                                                  ((int)totalTs.TotalHours).ToString(),
                                                  (int)totalTs.Minutes < 10 ?
                                                  "0" + ((int)totalTs.Minutes).ToString() :
                                                  ((int)totalTs.Minutes).ToString());
                e.Graphics.DrawString("Total       " + totalTsFormat,
                                      new Font("Tahoma", 8, FontStyle.Regular),
                                      new SolidBrush(Color.FromArgb(109, 109, 109)),
                                      new PointF(g.Width - 77, y));
            }
            else if (chartsPopulated && Mode == "scarti" || Mode == "rammendi")
            {
                int y = 29;
                //double total = 0;
                for (var i = 0; i < 3; i++)
                {
                    e.Graphics.DrawString(_timeValues[1, i].ToString(),
                                          new Font("Tahoma", 8, FontStyle.Regular),
                                          new SolidBrush(Color.FromArgb(109, 109, 109)),
                                          new PointF(g.Width - 32, y));
                    y += 15;
                }
            }

            p.Dispose();
        }

        private void grp1_Enter(object sender, EventArgs e)
        {

        }

        private int _numOfHiddenLabels = 0;
        private List<Panel> _squadra_group = new List<Panel>();
        private void SetGroupsVisibility()
        {
            _squadra_1.Visible = true;
            _squadra_2.Visible = true;
            _squadra_3.Visible = true;

            if (_numOfHiddenLabels <= -1 || _numOfHiddenLabels > 3) return;

            switch (_numOfHiddenLabels)
            {
                case 1:
                    {
                        _line1 = Color.Silver;
                        _line2 = Color.Silver;

                        _squadra_3.Visible = false;
                        var visibleGroups = (from grps in _squadra_group
                                             where grps.Visible == true
                                             orderby grps.Name
                                             select grps).ToArray();
                        if (pnFilterby.Width > 0)
                        {
                            _squadra_1.Text = "SQ " + visibleGroups[0].Name.Remove(0, 3) + ":";
                            _squadra_2.Text = "SQ " + visibleGroups[1].Name.Remove(0, 3) + ":";
                        }
                        else
                        {
                            _squadra_1.Text = "SQUADRA " + visibleGroups[0].Name.Remove(0, 3) + ":";
                            _squadra_2.Text = "SQUADRA " + visibleGroups[1].Name.Remove(0, 3) + ":";
                        }
                        break;
                    }
                case 2:
                    {
                        _line1 = Color.Silver;
                        _line2 = _squadra_pnl.BackColor;

                        _squadra_2.Visible = false;
                        _squadra_3.Visible = false;
                        var visibleGroup = (from grp in _squadra_group
                                            where grp.Visible == true
                                            select grp).SingleOrDefault();
                        var squadra_index = visibleGroup.Name.Remove(0, 3);

                        if (pnFilterby.Width > 0)
                            _squadra_1.Text = "SQ " + squadra_index + ":";
                        else
                            _squadra_1.Text = "SQUADRA " + squadra_index + ":";

                        break;
                    }
                case 3:
                    {
                        _line1 = _squadra_pnl.BackColor;
                        _line2 = _squadra_pnl.BackColor;

                        _squadra_1.Visible = false;
                        _squadra_2.Visible = false;
                        _squadra_3.Visible = false;
                        break;
                    }
                case 0:
                    {
                        _line1 = Color.Silver;
                        _line2 = Color.Silver;

                        if (pnFilterby.Width > 0)
                        {
                            _squadra_1.Text = "SQ 1:";
                            _squadra_2.Text = "SQ 2:";
                            _squadra_3.Text = "SQ 3:";
                        }
                        else
                        {
                            _squadra_1.Text = "SQUADRA 1:";
                            _squadra_2.Text = "SQUADRA 2:";
                            _squadra_3.Text = "SQUADRA 3:";
                        }
                        break;
                    }
            }
        }

        #region test

        private void cb_SQ_1_CheckedChanged(object sender, EventArgs e)
        {
            //if (_isReportMode) return;
            if (RepChartsDynamic._RepChartsDynamic && _isReportMode)
            {
                ShowChartsPopUp();
                if (grp1.Visible)
                {
                    _numOfHiddenLabels++;
                    grp1.Visible = false;
                    //btnSq1.Text = "_";
                }
                else
                {
                    _numOfHiddenLabels--;
                    grp1.Visible = true;
                    //btnSq1.Text = "";
                }

                SetGroupsVisibility();
                CreateObjectGroups();
                StartDelayedLoading();
            }
            else
            {
                if (grp1.Visible)
                {
                    _numOfHiddenLabels++;
                    grp1.Visible = false;
                    //btnSq1.Text = "_";
                }
                else
                {
                    _numOfHiddenLabels--;
                    grp1.Visible = true;
                    //btnSq1.Text = "";
                }

                SetGroupsVisibility();
                CreateObjectGroups();
                StartDelayedLoading();
            }
        }

        private void cb_SQ_2_CheckedChanged(object sender, EventArgs e)
        {
            //if (_isReportMode) return;

            if (RepChartsDynamic._RepChartsDynamic && _isReportMode)
            {
                ShowChartsPopUp();
                if (grp2.Visible)
                {
                    _numOfHiddenLabels++;
                    grp2.Visible = false;
                    //btnSq2.Text = "_";
                }
                else
                {
                    _numOfHiddenLabels--;
                    grp2.Visible = true;
                    //btnSq2.Text = "";
                }

                SetGroupsVisibility();
                CreateObjectGroups();
                StartDelayedLoading();
            }
            else
            {
                if (grp2.Visible)
                {
                    _numOfHiddenLabels++;
                    grp2.Visible = false;
                    //btnSq2.Text = "_";
                }
                else
                {
                    _numOfHiddenLabels--;
                    grp2.Visible = true;
                    //btnSq2.Text = "";
                }

                SetGroupsVisibility();
                CreateObjectGroups();
                StartDelayedLoading();
            }
        }

        private void cb_SQ_3_CheckedChanged(object sender, EventArgs e)
        {
            //if (_isReportMode) return;
            if (RepChartsDynamic._RepChartsDynamic && _isReportMode)
            {
                ShowChartsPopUp();
                if (grp3.Visible)
                {
                    _numOfHiddenLabels++;
                    grp3.Visible = false;
                    //btnSq3.Text = "_";
                }
                else
                {
                    _numOfHiddenLabels--;
                    grp3.Visible = true;
                    //btnSq3.Text = "";
                }

                SetGroupsVisibility();
                CreateObjectGroups();
                StartDelayedLoading();
            }
            else
            {
                if (grp3.Visible)
                {
                    _numOfHiddenLabels++;
                    grp3.Visible = false;
                    //btnSq3.Text = "_";
                }
                else
                {
                    _numOfHiddenLabels--;
                    grp3.Visible = true;
                    //btnSq3.Text = "";
                }

                SetGroupsVisibility();
                CreateObjectGroups();
                StartDelayedLoading();
            }
        }

        #endregion test

        private void cb_SQ_1_Paint(object sender, PaintEventArgs e)
        {
            var _check_box = (CheckBox)sender;
            var _cb_pen = new Pen(Brushes.Silver, 3);
            var rect = _check_box.ClientRectangle;
            if (_check_box.Checked)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(125, 141, 161)), rect);
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                using (Font wing = new Font("Wingdings", 14f, FontStyle.Regular))
                    e.Graphics.DrawString("ü", wing, new SolidBrush(_check_box.BackColor), new Rectangle(rect.X - 3, rect.Y - 4, 16, 16));
            }
            else
                e.Graphics.FillRectangle(new SolidBrush(_check_box.BackColor), rect);
            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(-1, -1, _check_box.Width, _check_box.Height), 4))
            {
                SmoothingMode old = e.Graphics.SmoothingMode;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                //e.Graphics.FillPath(Brushes.Black, path);
                e.Graphics.DrawPath(_cb_pen, path);

                _check_box.Region = new Region(path);
                e.Graphics.SmoothingMode = old;
            }
        }

        private void cb_SQ_2_Paint(object sender, PaintEventArgs e)
        {
            var _check_box = (CheckBox)sender;
            var _cb_pen = new Pen(Brushes.Silver, 3);
            var rect = _check_box.ClientRectangle;
            if (_check_box.Checked)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(125, 141, 161)), rect);
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                using (Font wing = new Font("Wingdings", 14f, FontStyle.Regular))
                    e.Graphics.DrawString("ü", wing, new SolidBrush(_check_box.BackColor), new Rectangle(rect.X - 3, rect.Y - 4, 16, 16));
            }
            else
                e.Graphics.FillRectangle(new SolidBrush(_check_box.BackColor), rect);
            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(-1, -1, _check_box.Width, _check_box.Height), 4))
            {
                SmoothingMode old = e.Graphics.SmoothingMode;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                //e.Graphics.FillPath(Brushes.Black, path);
                e.Graphics.DrawPath(_cb_pen, path);

                _check_box.Region = new Region(path);
                e.Graphics.SmoothingMode = old;
            }
        }

        private void cb_SQ_3_Paint(object sender, PaintEventArgs e)
        {
            var _check_box = (CheckBox)sender;
            var _cb_pen = new Pen(Brushes.Silver, 3);
            var rect = _check_box.ClientRectangle;
            if (_check_box.Checked)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(125, 141, 161)), rect);
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                using (Font wing = new Font("Wingdings", 14f, FontStyle.Regular))
                    e.Graphics.DrawString("ü", wing, new SolidBrush(_check_box.BackColor), new Rectangle(rect.X - 3, rect.Y - 4, 16, 16));
            }
            else
                e.Graphics.FillRectangle(new SolidBrush(_check_box.BackColor), rect);
            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(-1, -1, _check_box.Width, _check_box.Height), 4))
            {
                SmoothingMode old = e.Graphics.SmoothingMode;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                //e.Graphics.FillPath(Brushes.Black, path);
                e.Graphics.DrawPath(_cb_pen, path);

                _check_box.Region = new Region(path);
                e.Graphics.SmoothingMode = old;
            }
        }

        private void grp2_Paint_1(object sender, PaintEventArgs e)
        {
            var pn = (Panel)sender;
            var x = e.ClipRectangle.X;
            var y = e.ClipRectangle.Y;
            var w = e.ClipRectangle.Right;
            var h = e.ClipRectangle.Bottom;
            e.Graphics.DrawLine(new Pen(Brushes.Silver, 2), x, y + h, w, y + h);
            e.Graphics.DrawLine(new Pen(Brushes.Silver, 2), new Point(0, 0), new Point(pnlTitleBar.Width, 0));
            if (Mode == "temperature")
            {
                var brush = new SolidBrush(_tempColor);
                var pen = new Pen(brush, 2);

                var linLst = (from l in pn.Controls.OfType<Label>()
                              where l.Name.Substring(0, 1) == "L"
                              select l).ToList();
                foreach (var line in linLst)
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawLine(pen,
                                   (line.Location.X + line.Width / 2), line.Location.Y - pn.Height / 3 - 23,
                                    line.Location.X + line.Width / 2, line.Location.Y);
                    Point circlePoint = new Point(line.Location.X + line.Width / 2 - 3, line.Location.Y - pn.Height / 3 - 27);
                    e.Graphics.FillEllipse(brush,
                                          new Rectangle(circlePoint, new Size(6, 6)));
                    e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                }
                pen.Dispose();
            }
        }

        private void grp1_Paint_1(object sender, PaintEventArgs e)
        {
            var _pnl = (Panel)sender;
            e.Graphics.DrawLine(new Pen(Brushes.Silver, 2),
                                0, _pnl.Location.Y + _pnl.Height,
                                _pnl.Location.X + _pnl.Width, _pnl.Location.Y + _pnl.Height);
            e.Graphics.DrawLine(new Pen(Brushes.Silver, 2),
                                0, 0,
                                _pnl.Location.X + _pnl.Width, 0);
            if (Mode == "temperature")
            {
                var brush = new SolidBrush(_tempColor);
                var pen = new Pen(brush, 2);

                var linLst = (from l in _pnl.Controls.OfType<Label>()
                              where l.Name.Substring(0, 1) == "L"
                              select l).ToList();
                foreach (var line in linLst)
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawLine(pen,
                                   (line.Location.X + line.Width / 2), line.Location.Y - _pnl.Height / 3 - 23,
                                    line.Location.X + line.Width / 2, line.Location.Y);
                    Point circlePoint = new Point(line.Location.X + line.Width / 2 - 3, line.Location.Y - _pnl.Height / 3 - 27);
                    e.Graphics.FillEllipse(brush,
                                          new Rectangle(circlePoint, new Size(6, 6)));
                    e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                }
                pen.Dispose();
            }
        }

        private void grp3_Paint_1(object sender, PaintEventArgs e)
        {
            var _pnl = (Panel)sender;
            var x = e.ClipRectangle.X;
            var y = e.ClipRectangle.Y;
            var w = e.ClipRectangle.Right;
            var h = e.ClipRectangle.Bottom;
            e.Graphics.DrawLine(new Pen(Brushes.Silver, 1), x, y + h - 1, w, y + h - 1);

            if (Mode == "temperature")
            {
                var brush = new SolidBrush(_tempColor);
                var pen = new Pen(brush, 2);

                var linLst = (from l in _pnl.Controls.OfType<Label>()
                              where l.Name.Substring(0, 1) == "L"
                              select l).ToList();
                foreach (var line in linLst)
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawLine(pen,
                                   (line.Location.X + line.Width / 2), line.Location.Y - _pnl.Height / 3 - 23,
                                    line.Location.X + line.Width / 2, line.Location.Y);
                    Point circlePoint = new Point(line.Location.X + line.Width / 2 - 3, line.Location.Y - _pnl.Height / 3 - 27);
                    e.Graphics.FillEllipse(brush,
                                          new Rectangle(circlePoint, new Size(6, 6)));
                    e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                }
                pen.Dispose();
            }
        }

        private void pnMasthead_Paint(object sender, PaintEventArgs e)
        {
            var pnl = sender as Panel;
            e.Graphics.DrawLine(new Pen(new SolidBrush(Color.Silver), 2), new Point(0, pnl.Height),
                                new Point(pnl.Width, pnl.Height));
            e.Graphics.DrawLine(new Pen(Color.Silver, 1),
                                new Point(cbNight.Location.X - 20, 10),
                                new Point(cbNight.Location.X - 20, pnMasthead.Height - 10));
            e.Graphics.DrawLine(new Pen(Color.FromArgb(214, 214, 214), 1),
                                new Point(cb_SQ_1.Location.X - 25, pnMasthead.Location.Y + 10),
                                new Point(cb_SQ_1.Location.X - 25, pnMasthead.Location.Y + pnMasthead.Height));
            e.Graphics.DrawLine(new Pen(new SolidBrush(Color.Silver), 1), new Point(panel1.Location.X + panel1.Width + 10, 10),
                                        new Point(panel1.Location.X + panel1.Width + 10, pnl.Height - 10));
        }

        private Color _line1 = Color.Silver;
        private Color _line2 = Color.Silver;
        private void _squadra_pnl_Paint(object sender, PaintEventArgs e)
        {
            var _squadra_Brush = new SolidBrush(_line1);

            var pnl = (Panel)sender;

            //e.Graphics.DrawLine(new Pen(Color.Silver, 1), new Point(0, grp1.Location.X + grp1.Height), new Point(pnl.Width, grp1.Location.X + grp1.Height));
            e.Graphics.DrawLine(new Pen(Color.Silver, 1),
                                new Point(0, 0),
                                new Point(grp1.Width, 0));
            e.Graphics.DrawLine(new Pen(Color.Silver, 1),
                                new Point(0, grp1.Height),
                                new Point(pnl.Width, grp1.Height));
            _squadra_Brush = new SolidBrush(_line2);
            e.Graphics.DrawLine(new Pen(Color.Silver, 1),
                                new Point(0, grp1.Height * 2 - 1),
                                new Point(pnl.Width, grp1.Height * 2 - 1));
            e.Graphics.DrawLine(new Pen(Color.Silver, 1),
                                new Point(0, grp3.Height),
                                new Point(pnl.Width, grp3.Height));
        }

        //private void SetShiftImage(CheckBox cb, bool isChecked)
        //{
        //    string _shift = cb.Name.Remove(0, 2);

        //    if (!isChecked)
        //    {
        //        switch (_shift)
        //        {
        //            case "Night":
        //                {
        //                    _pb_night.Image = Properties.Resources.mac_50x32_png;
        //                    break;
        //                }
        //            case "Morning":
        //                {
        //                    _pb_morning.Image = Properties.Resources.mac_50x32_png;
        //                    break;
        //                }
        //            case "Afternoon":
        //                {
        //                    _pb_afternoon.Image = Properties.Resources.mac_50x32_png;
        //                    break;
        //                }
        //        }
        //    }
        //    else
        //    {
        //        switch (_shift)
        //        {
        //            case "Night":
        //                {
        //                    _pb_night.Image = Properties.Resources.mac_50x31_gif;
        //                    break;
        //                }
        //            case "Morning":
        //                {
        //                    _pb_morning.Image = Properties.Resources.mac_50x31_gif;
        //                    break;
        //                }
        //            case "Afternoon":
        //                {
        //                    _pb_afternoon.Image = Properties.Resources.mac_50x31_gif;
        //                    break;
        //                }
        //        }
        //    }
        //}
        private CheckBox _cbShifts;
        private void Shifts_Click(object sender, EventArgs e)
        {
            _cbShifts = (CheckBox)sender;

            if (_sqlHasError || _isReportMode) return;

            Set_file_name(string.Empty);

            if (_cbShifts.Checked)
            {
                // Add checked shift 
                if (!ListOfSelectedShifts.Contains(_cbShifts.Name))
                    ListOfSelectedShifts.Add(_cbShifts.Name);

                //SetShiftImage(_cbShifts, _cbShifts.Checked);
                //_cbShifts.Image = Properties.Resources.mac_50x31_gif;
            }
            else
            {
                // Remove unchecked shift
                if (ListOfSelectedShifts.Contains(_cbShifts.Name))
                    ListOfSelectedShifts.Remove(_cbShifts.Name);

                //SetShiftImage(_cbShifts, _cbShifts.Checked);
                //_cbShifts.Image = Properties.Resources.new_mach_png;
            }

            // Build string that will contains array of shifts listed above

            Get_shift_array().Clear();
            Get_shift_array().Append(",");

            foreach (var item in ListOfSelectedShifts)
            {
                Get_shift_array().Append(item.Remove(0, 2) + ",");   //remove "cb" prefix
            }

            StartDelayedLoading();
        }

        private void cbNight_Paint(object sender, PaintEventArgs e)
        {
            var _check_box = (CheckBox)sender;
            var _cb_pen = new Pen(Brushes.Silver, 3);
            var rect = _check_box.ClientRectangle;
            if (_check_box.Checked)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(125, 141, 161)), rect);
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                using (Font wing = new Font("Wingdings", 14f, FontStyle.Regular))
                    e.Graphics.DrawString("ü", wing, new SolidBrush(_check_box.BackColor), new Rectangle(rect.X - 3, rect.Y - 4, 16, 16));
            }
            else
                e.Graphics.FillRectangle(new SolidBrush(_check_box.BackColor), rect);
            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(-1, -1, _check_box.Width, _check_box.Height), 4))
            {
                SmoothingMode old = e.Graphics.SmoothingMode;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(_cb_pen, path);

                _check_box.Region = new Region(path);
                e.Graphics.SmoothingMode = old;
            }
        }

        private void cbMorning_Paint(object sender, PaintEventArgs e)
        {
            var _check_box = (CheckBox)sender;
            var _cb_pen = new Pen(Brushes.Silver, 3);
            var rect = _check_box.ClientRectangle;
            if (_check_box.Checked)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(125, 141, 161)), rect);
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                using (Font wing = new Font("Wingdings", 14f, FontStyle.Regular))
                    e.Graphics.DrawString("ü", wing, new SolidBrush(_check_box.BackColor), new Rectangle(rect.X - 3, rect.Y - 4, 16, 16));
            }
            else
                e.Graphics.FillRectangle(new SolidBrush(_check_box.BackColor), rect);
            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(-1, -1, _check_box.Width, _check_box.Height), 4))
            {
                SmoothingMode old = e.Graphics.SmoothingMode;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(_cb_pen, path);

                _check_box.Region = new Region(path);
                e.Graphics.SmoothingMode = old;
            }
        }

        private void cbAfternoon_Paint(object sender, PaintEventArgs e)
        {
            var _check_box = (CheckBox)sender;
            var _cb_pen = new Pen(Brushes.Silver, 3);
            var rect = _check_box.ClientRectangle;
            if (_check_box.Checked)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(125, 141, 161)), rect);
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                using (Font wing = new Font("Wingdings", 14f, FontStyle.Regular))
                    e.Graphics.DrawString("ü", wing, new SolidBrush(_check_box.BackColor), new Rectangle(rect.X - 3, rect.Y - 4, 16, 16));
            }
            else
                e.Graphics.FillRectangle(new SolidBrush(_check_box.BackColor), rect);
            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(-1, -1, _check_box.Width, _check_box.Height), 4))
            {
                SmoothingMode old = e.Graphics.SmoothingMode;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(_cb_pen, path);

                _check_box.Region = new Region(path);
                e.Graphics.SmoothingMode = old;
            }
        }

        private void checkBox1_Paint(object sender, PaintEventArgs e)
        {
            var _check_box = (CheckBox)sender;
            var _cb_pen = new Pen(Brushes.Silver, 3);
            var rect = _check_box.ClientRectangle;
            if (_check_box.Checked)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(125, 141, 161)), rect);
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                using (Font wing = new Font("Wingdings", 14f, FontStyle.Regular))
                    e.Graphics.DrawString("ü", wing, new SolidBrush(_check_box.BackColor), new Rectangle(rect.X - 3, rect.Y - 4, 16, 16));
            }
            else
                e.Graphics.FillRectangle(new SolidBrush(_check_box.BackColor), rect);
            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(-1, -1, _check_box.Width, _check_box.Height), 4))
            {
                SmoothingMode old = e.Graphics.SmoothingMode;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(_cb_pen, path);

                _check_box.Region = new Region(path);
                e.Graphics.SmoothingMode = old;
            }
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            StartDelayedLoading();
        }

        private void cbFin3_Paint(object sender, PaintEventArgs e)
        {
            //var _btn = (CheckBox)sender;
            //SolidBrush _sb = new SolidBrush(Color.FromArgb(109, 109, 109));
            //var _btn_pen = new Pen(_sb, 3);

            //using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(-1, -1, _btn.Width, _btn.Height), 4))
            //{
            //    SmoothingMode old = e.Graphics.SmoothingMode;
            //    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            //    e.Graphics.DrawPath(_btn_pen, path);

            //    _btn.Region = new Region(path);
            //    e.Graphics.SmoothingMode = old;
            //}
        }

        private void btnEffMode_Paint(object sender, PaintEventArgs e)
        {
            //var _btn = (Button)sender;
            //SolidBrush _sb = new SolidBrush(Color.FromArgb(109, 109, 109));
            //var _btn_pen = new Pen(_sb, 3);

            //using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(-1, -1, _btn.Width, _btn.Height), 4))
            //{
            //    SmoothingMode old = e.Graphics.SmoothingMode;
            //    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            //    e.Graphics.DrawPath(_btn_pen, path);

            //    _btn.Region = new Region(path);
            //    e.Graphics.SmoothingMode = old;
            //}
        }


        private List<Color> _targetedColors = new List<Color>();
        public void CollectMachineColors(CheckBox rb)
        {
            Color targetedColor = default(Color);

            if (rb.Name == "cbGreen") targetedColor = Color.FromArgb(54, 214, 87);
            else if (rb.Name == "cbYellow") targetedColor = Color.FromArgb(254, 215, 1);
            else targetedColor = Color.FromArgb(253, 129, 127);
            
            foreach (var m in _list_of_machines)
            {
                foreach (KeyValuePair<Label, Color> k in _currentColors)
                {
                    var name = k.Key.Name;
                    if (m.Name != name) continue;

                    if(_targetedColors.Exists( c => c == k.Value))
                    {
                        m.BackColor = k.Value;
                        m.ForeColor = Color.FromArgb(100, 100, 100);
                    }
                    else
                    {
                        m.BackColor = Color.FromArgb(211, 211, 211);
                        m.ForeColor = Color.FromArgb(211, 211, 211);
                    }
                }
            }

            foreach (var grp in new Panel[] { grp1, grp2, grp3 })
            {
                var y = 0;
                var g = 0;
                var r = 0;
                var s = 0;

                var totLabList = (from t in grp.Controls.OfType<Label>()
                                  where t.Name.Substring(0, 1) == "_"
                                  select t).ToList();

                foreach (var machine in (from m in grp.Controls.OfType<Label>()
                                         where m.Name.Substring(0, 1) == "P"
                                         select m).ToList())
                {
                    if (machine.BackColor == Color.FromArgb(54, 214, 87)) g++;
                    else if (machine.BackColor == Color.FromArgb(254, 215, 1)) y++;
                    else if (machine.BackColor == Color.FromArgb(253, 129, 127)) r++;
                    else s++;
                }

                if (percentMode)
                {
                    var green = Math.Ceiling(((Convert.ToDouble(g) / 70) * 100));
                    var yellow = Math.Ceiling(((Convert.ToDouble(y) / 70) * 100));
                    var red = Math.Ceiling(((Convert.ToDouble(r) / 70) * 100));
                    var silver = Math.Ceiling(((Convert.ToDouble(s) / 70) * 100));

                    foreach (var lbl in totLabList)
                    {
                        if (lbl.Name.StartsWith("_totGreen")) lbl.Text = green.ToString() + "%";
                        else if (lbl.Name.StartsWith("_totYellow")) lbl.Text = yellow.ToString() + "%";
                        else if (lbl.Name.StartsWith("_totRed")) lbl.Text = red.ToString() + "%";
                        else lbl.Text = silver.ToString() + "%";
                    }
                }
                else
                {
                    foreach (var lbl in totLabList)
                    {
                        if (lbl.Name.StartsWith("_totGreen")) lbl.Text = g.ToString();
                        else if (lbl.Name.StartsWith("_totYellow")) lbl.Text = y.ToString();
                        else if (lbl.Name.StartsWith("_totRed")) lbl.Text = r.ToString();
                        else lbl.Text = s.ToString();
                    }
                }
            }


        }

        public void PostMachineColors()
        {
            foreach (var m in _list_of_machines)
            {
                foreach (KeyValuePair<Label, Color> k in _currentColors)
                {
                    var name = k.Key.Name;
                    if (m.Name != name) continue;
                    m.BackColor = k.Value;
                    m.ForeColor = Color.FromArgb(100, 100, 100);
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

            foreach (var grp in new Panel[] { grp1, grp2, grp3 })
            {
                var y = 0;
                var g = 0;
                var r = 0;
                var s = 0;

                var totLabList = (from t in grp.Controls.OfType<Label>()
                                  where t.Name.Substring(0, 1) == "_"
                                  select t).ToList();

                foreach (var machine in (from m in grp.Controls.OfType<Label>()
                                         where m.Name.Substring(0, 1) == "P"
                                         select m).ToList())
                {
                    if (machine.BackColor == Color.FromArgb(54, 214, 87)) g++;
                    else if (machine.BackColor == Color.FromArgb(254, 215, 1)) y++;
                    else if (machine.BackColor == Color.FromArgb(253, 129, 127)) r++;
                    else s++;
                }

                if (percentMode)
                {
                    var green = Math.Ceiling(((Convert.ToDouble(g) / 70) * 100));
                    var yellow = Math.Ceiling(((Convert.ToDouble(y) / 70) * 100));
                    var red = Math.Ceiling(((Convert.ToDouble(r) / 70) * 100));
                    var silver = Math.Ceiling(((Convert.ToDouble(s) / 70) * 100));

                    foreach (var lbl in totLabList)
                    {
                        if (lbl.Name.StartsWith("_totGreen")) lbl.Text = green.ToString() + "%";
                        else if (lbl.Name.StartsWith("_totYellow")) lbl.Text = yellow.ToString() + "%";
                        else if (lbl.Name.StartsWith("_totRed")) lbl.Text = red.ToString() + "%";
                        else lbl.Text = silver.ToString() + "%";
                    }
                }
                else
                {
                    foreach (var lbl in totLabList)
                    {
                        if (lbl.Name.StartsWith("_totGreen")) lbl.Text = g.ToString();
                        else if (lbl.Name.StartsWith("_totYellow")) lbl.Text = y.ToString();
                        else if (lbl.Name.StartsWith("_totRed")) lbl.Text = r.ToString();
                        else lbl.Text = s.ToString();
                    }
                }
            }
        }

        //private void rbGreen_CheckedChanged(object sender, EventArgs e)
        //{
        //    RadioButton rb = (RadioButton)sender;
        //    CollectMachineColors(rb);
        //}

        //private void rbYellow_CheckedChanged(object sender, EventArgs e)
        //{
        //    RadioButton rb = (RadioButton)sender;
        //    CollectMachineColors(rb);
        //}

        //private void rbRed_CheckedChanged(object sender, EventArgs e)
        //{
        //    RadioButton rb = (RadioButton)sender;
        //    CollectMachineColors(rb);
        //}

        //private void rbMostra_CheckedChanged(object sender, EventArgs e)
        //{
        //    PostMachineColors();
        //}

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

            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(230, 230, 230)), rect);
            e.Graphics.DrawEllipse(p, new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 4, rect.Height - 4));

            if (rb.Checked)
            {
                e.Graphics.DrawEllipse(new Pen(b, 1), new Rectangle(rect.Width / 2 - 4, rect.Height / 2 - 4, 6, 6));
                e.Graphics.FillEllipse(b, new Rectangle(rect.Width / 2 - 4, rect.Height / 2 - 4, 6, 6));
            }
            else e.Graphics.FillEllipse(new SolidBrush(rb.BackColor), new Rectangle(rect.Width / 2 - 4, rect.Height / 2 - 4, 6, 6));

            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
        }

        private void TotalLabels_Paint(object sender, PaintEventArgs e)
        {
            var lbl = (Label)sender;
            if (Mode == "temperature") lbl.Visible = false;
            else lbl.Visible = true;
            var rect = lbl.ClientRectangle;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var targetedColor = default(Color);

            if (lbl.Name.StartsWith("_totGreen_")) targetedColor = Color.FromArgb(54, 214, 87);
            else if (lbl.Name.StartsWith("_totYellow_")) targetedColor = Color.FromArgb(254, 215, 1);
            else if (lbl.Name.StartsWith("_totRed_")) targetedColor = Color.FromArgb(253, 129, 127);
            else if (lbl.Name.StartsWith("_totGray_")) targetedColor = Color.LightGray;

            var br = new SolidBrush(targetedColor);

            e.Graphics.DrawEllipse(new Pen(br, 2), new Rectangle(rect.X + 2, rect.Y + 2, rect.Width - 4, rect.Height - 4));

            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
        }

        private bool percentMode = false;
        private void cbPercent_CheckedChanged(object sender, EventArgs e)
        {
            if (cbPercent.Checked) percentMode = true;
            else percentMode = false;

            if (cbMostra.Checked) PostMachineColors();
            else
            {
                var checkedRb = cbGreen.Checked ? cbGreen : cbYellow.Checked ? cbYellow : cbRed;
                CollectMachineColors(checkedRb);
            }
        }

        private void cbPercent_Paint(object sender, PaintEventArgs e)
        {
            var _check_box = (CheckBox)sender;
            var _cb_pen = new Pen(Brushes.Silver, 3);
            var rect = _check_box.ClientRectangle;
            if (_check_box.Checked)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(125, 141, 161)), rect);
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                using (Font wing = new Font("Wingdings", 14f, FontStyle.Regular))
                    e.Graphics.DrawString("ü", wing, new SolidBrush(_check_box.BackColor), new Rectangle(rect.X - 3, rect.Y - 4, 16, 16));
            }
            else
                e.Graphics.FillRectangle(new SolidBrush(_check_box.BackColor), rect);
            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(-1, -1, _check_box.Width, _check_box.Height), 4))
            {
                SmoothingMode old = e.Graphics.SmoothingMode;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(_cb_pen, path);

                _check_box.Region = new Region(path);
                e.Graphics.SmoothingMode = old;
            }
        }

        private Color _tempColor;
        private string _tempMode = string.Empty;
        private void btn_temp_t_Click(object sender, EventArgs e)
        {
            if (_isReportMode) return;
            Mode = "temperature";
            _tempMode = "temp";
            _tempColor = Color.FromArgb(255, 127, 39);
            //GetCharts();
            GetData();
        }

        private void btn_temp_u_Click(object sender, EventArgs e)
        {
            if (_isReportMode) return;
            Mode = "temperature";
            _tempMode = "hum";
            _tempColor = Color.FromArgb(0, 125, 179);
            //GetCharts();
            GetData();
        }

        private void ResetOperatorsFields()
        {
            _operatorMode = false;
            foreach(Label lab in new Label[] { Operator1, Operator2, Operator3, Operator4, Operator5, Operator6, Operator7,
            Operator8, Operator9, Operator10, Operator11, Operator12, Operator13, Operator14, Operator15})
            {
                lab.Text = string.Empty;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ResetOperatorsFields();
            var btn = sender as Button;
            foreach (var b in _listOfJobs)
            {
                if (btn.Name == b.Name) b.Image = Properties.Resources.modes1_green;
                else b.Image = Properties.Resources.modes1_silver;
            }
            Mode = "cleaner";
            _cleanersType = "cquality";
            SetDefaultFilters(false);
            GetData();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ResetOperatorsFields();
            var btn = sender as Button;
            foreach (var b in _listOfJobs)
            {
                if (btn.Name == b.Name) b.Image = Properties.Resources.modes1_green;
                else b.Image = Properties.Resources.modes1_silver;
            }
            Mode = "rammendi";
            SetDefaultFilters(false);
            GetData();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            ResetOperatorsFields();
            var btn = sender as Button;
            foreach (var b in _listOfJobs)
            {
                if (btn.Name == b.Name) b.Image = Properties.Resources.modes1_green;
                else b.Image = Properties.Resources.modes1_silver;
            }
            Mode = "scarti";
            SetDefaultFilters(false);
            GetData();
        }

        public static bool _teli_mode = false;
        private void cbTeli_CheckedChanged(object sender, EventArgs e)
        {
            var cb = (CheckBox)sender;
            if (Mode != "scarti" && Mode != "rammendi") return;
            if (cb.Checked) _teli_mode = true;
            else _teli_mode = false;
            GetData();
        }

        private void cbTeli_Paint(object sender, PaintEventArgs e)
        {
            var _check_box = (CheckBox)sender;
            var _cb_pen = new Pen(Brushes.Silver, 3);
            var rect = _check_box.ClientRectangle;           
            if (_check_box.Checked)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(125, 141, 161)), rect);
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                using (Font wing = new Font("Wingdings", 14f, FontStyle.Regular))
                    e.Graphics.DrawString("ü", wing, new SolidBrush(_check_box.BackColor), new Rectangle(rect.X - 3, rect.Y - 4, 16, 16));                
            }
            else
                e.Graphics.FillRectangle(new SolidBrush(_check_box.BackColor), rect);
            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(-1, -1, _check_box.Width, _check_box.Height), 4))
            {
                SmoothingMode old = e.Graphics.SmoothingMode;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(_cb_pen, path);

                _check_box.Region = new Region(path);
                e.Graphics.SmoothingMode = old;
            }
        }

        private void checkBox2_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void CheckboxStyle_Paint(object sender, PaintEventArgs e)
        {
            CheckBox rb = sender as CheckBox;
            var rect = rb.ClientRectangle;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var targetedColor = default(Color);
            if (rb.Name == "cbGreen") targetedColor = Color.FromArgb(54, 214, 87);
            else if (rb.Name == "cbYellow") targetedColor = Color.FromArgb(254, 215, 1);
            else if (rb.Name == "cbRed") targetedColor = Color.FromArgb(253, 129, 127);
            else targetedColor = Color.LightGray;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.FillRectangle(new SolidBrush(rb.BackColor), new RectangleF(rect.X - 1, rect.Y, rect.Width, rect.Height));
            e.Graphics.DrawEllipse(new Pen(new SolidBrush(targetedColor), 2), new RectangleF(rect.X + 1, rect.Y + 1, rect.Width - 4, rect.Height - 4));
            if (rb.Checked)
                e.Graphics.FillEllipse(new SolidBrush(targetedColor), new RectangleF(rect.Width / 2 - 5, rect.Height / 2 - 5, 8, 8));
            else
                e.Graphics.FillEllipse(new SolidBrush(rb.BackColor), new RectangleF(rect.Width / 2 - 5, rect.Height / 2 - 5, 8, 8));
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
        }

        private void CheckboxColor_CheckedChanged(object sender, EventArgs e)
        {
            cbMostra.Checked = false;
            var cb = sender as CheckBox;
            var targetedColor = default(Color);
            if (cb.Name == "cbGreen") targetedColor = Color.FromArgb(54, 214, 87);
            else if (cb.Name == "cbYellow") targetedColor = Color.FromArgb(254, 215, 1);
            else if (cb.Name == "cbRed") targetedColor = Color.FromArgb(253, 129, 127);
            if (cb.Checked)
                _targetedColors.Add(targetedColor);
            else
                _targetedColors.Remove(targetedColor);
            CollectMachineColors(cb);
        }

        private void cbMostra_CheckedChanged(object sender, EventArgs e)
        {
            var cb = sender as CheckBox;
            if(cb.Checked)
            {
                _targetedColors.Clear();
                PostMachineColors();                
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            foreach (var b in _lstOfModes)
            {
                if (btn.Name == b.Name) b.Image = Properties.Resources.modes1_green;
                else b.Image = Properties.Resources.modes1_silver;
            }
            _operatorMode = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResetOperatorsFields();
            var btn = sender as Button;
            foreach (var b in _listOfJobs)
            {
                if (btn.Name == b.Name) b.Image = Properties.Resources.modes1_green;
                else b.Image = Properties.Resources.modes1_silver;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ResetOperatorsFields();
            var btn = sender as Button;
            foreach (var b in _listOfJobs)
            {
                if (btn.Name == b.Name) b.Image = Properties.Resources.modes1_green;
                else b.Image = Properties.Resources.modes1_silver;
            }
            Mode = "articlechanges";
            SetDefaultFilters(false);
            GetData();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ResetOperatorsFields();
            var btn = sender as Button;
            foreach (var b in _listOfJobs)
            {
                if (btn.Name == b.Name) b.Image = Properties.Resources.modes1_green;
                else b.Image = Properties.Resources.modes1_silver;
            }
            Mode = "PCPPST";
            SetDefaultFilters(false);
            GetData();
        }

        private void pnFilterby_Paint(object sender, PaintEventArgs e)
        {
            var pnl = sender as Panel;
            e.Graphics.DrawLine(new Pen(Color.White, 1), new Point(0, button6.Location.Y - 7),
                                new Point(pnl.Width, button6.Location.Y - 7));
            e.Graphics.DrawLine(new Pen(Color.White, 1), new Point(0, cbFin14.Location.Y + cbFin14.Height + 3),
                                new Point(pnl.Width, cbFin14.Location.Y + cbFin14.Height + 3));
            e.Graphics.DrawLine(new Pen(Color.White, 1), new Point(0, btnSpeedMode.Location.Y + btnSpeedMode.Height + 3),
                                new Point(pnl.Width, btnSpeedMode.Location.Y + btnSpeedMode.Height + 3));
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to close application?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes) Application.Exit();
            else return;
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void pnlTitleBar_DoubleClick(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                WindowState = FormWindowState.Normal;
                this.Size = new Size(1000, 600);
            }
        }

        private void pnlTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseDown = true;
            _lastLocation = e.Location;
        }

        private void pnlTitleBar_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseDown = false;
        }

        private void pnlTitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseDown)
            {
                Location = new Point(
                    (Location.X - _lastLocation.X) + e.X, (Location.Y - _lastLocation.Y) + e.Y);
                Update();
            }
        }

        private void pnlTitleBar_SizeChanged(object sender, EventArgs e)
        {
            var w = pnlTitleBar.Width;
            var wl = lblTitle.Width;

            lblTitle.Location = new Point(w / 2 - wl / 2, lblTitle.Location.Y);
        }

        //private void pnLayoutCommander_Paint(object sender, PaintEventArgs e)
        //{

        //}

        //private void pnLayoutCommander_Paint_1(object sender, PaintEventArgs e)
        //{

        //}

        private void chartSq3_Paint(object sender, PaintEventArgs e)
        {
            var g = (System.Windows.Forms.DataVisualization.Charting.Chart)sender;
            var h = g.Size.Height;
            var w = g.Size.Width;
            var p = new Pen(Brushes.Silver, 1);
            e.Graphics.DrawLine(p, 0, 0, 0, h);
            e.Graphics.DrawLine(p, 5, 0, 5, h - 1);
            e.Graphics.DrawLine(p, 5, h - 1, w, h - 1);

            if (chartsPopulated && Mode != "temperature" && Mode != "scarti" && Mode != "rammendi")
            {
                int y = 29;
                double total = 0;
                for (var i = 0; i < 4; i++)
                {
                    var val = Convert.ToDouble(_timeValues[2, i]);
                    total += val;
                    var formatedTs = TimeSpan.FromMinutes(val);
                    string timeFormat = string.Format("{0}:{1}",
                                                  (int)formatedTs.TotalHours < 10 ?
                                                  "0" + ((int)formatedTs.TotalHours).ToString() :
                                                  ((int)formatedTs.TotalHours).ToString(),
                                                  (int)formatedTs.Minutes < 10 ?
                                                  "0" + ((int)formatedTs.Minutes).ToString() :
                                                  ((int)formatedTs.Minutes).ToString());
                    e.Graphics.DrawString(timeFormat,
                                          new Font("Tahoma", 8, FontStyle.Regular),
                                          new SolidBrush(Color.FromArgb(109, 109, 109)),
                                          new PointF(g.Width - 32, y));
                    y += 15;
                }
                var totalTs = TimeSpan.FromMinutes(total);
                string totalTsFormat = string.Format("{0}:{1}",
                                                  (int)totalTs.TotalHours < 10 ?
                                                  "0" + ((int)totalTs.TotalHours).ToString() :
                                                  ((int)totalTs.TotalHours).ToString(),
                                                  (int)totalTs.Minutes < 10 ?
                                                  "0" + ((int)totalTs.Minutes).ToString() :
                                                  ((int)totalTs.Minutes).ToString());
                e.Graphics.DrawString("Total       " + totalTsFormat,
                                      new Font("Tahoma", 8, FontStyle.Regular),
                                      new SolidBrush(Color.FromArgb(109, 109, 109)),
                                      new PointF(g.Width - 77, y));
            }
            else if (chartsPopulated && Mode == "scarti" || Mode == "rammendi")
            {
                int y = 29;
                //double total = 0;
                for (var i = 0; i < 3; i++)
                {
                    e.Graphics.DrawString(_timeValues[2, i].ToString(),
                                          new Font("Tahoma", 8, FontStyle.Regular),
                                          new SolidBrush(Color.FromArgb(109, 109, 109)),
                                          new PointF(g.Width - 32, y));
                    y += 15;
                }
            }

            p.Dispose();
        }

        #endregion Borders

        #region CursorMessageing

        //private MouseMoveMessageFilter mouseMessageFilter;
        //protected override void OnClosed(EventArgs e)
        //    {
        //    base.OnClosed(e);

        //    Application.RemoveMessageFilter(this.mouseMessageFilter);
        //    }

        //class MouseMoveMessageFilter : IMessageFilter
        //    {
        //    public Form TargetForm { get; set; }
        //    public static bool CursorHidden { get; set; }

        //    public bool PreFilterMessage(ref Message m)
        //        {
        //        int numMsg = m.Msg;

        //        if (numMsg == 0x0200 /*WM_MOUSEMOVE*/)
        //            {
        //            TargetForm.Text = "Moving";   
        //            }
        //        else
        //            {
        //            TargetForm.Text = "Static";
        //            }

        //        return true;
        //        }
        //    }

        #endregion MouseMoveOveralForm

        private void btnCalendar_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            try
            {
                // true is the default, but it is important not to set it to false
                proc.StartInfo.UseShellExecute = true;
                proc.StartInfo.FileName = "http://192.168.96.17:3000";
                proc.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void pnLayoutCommander_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button10_Click_3(object sender, EventArgs e)
        {
            
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            var frmHolidays = new FrmSettings();
            frmHolidays.ShowDialog();
            frmHolidays.Dispose();
        }

        private Dictionary<string, bool> _machine_alarms = new Dictionary<string, bool>();
        private void GetMachineAlarms()
        {
            _machine_alarms.Clear();

            if (_cleanersType == "cquality" && Mode == "cleaner")
            {
                using (var con = new SqlConnection(conString))
                {
                    var cmd = new SqlCommand("get_machine_cq_alarm", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    //cmd.Parameters.Add("@idm", SqlDbType.Int).Value = mac;
                    cmd.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = Get_from_date();
                    cmd.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = Get_to_date();
                    cmd.Parameters.Add("@shift", SqlDbType.VarChar, 200).Value = Get_shift_array().ToString();

                    con.Open();
                    var dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            var machine = dr[0].ToString();
                            var note = dr[1].ToString();
                            bool alm = false;
                            if (string.IsNullOrEmpty(note)) alm = false;
                            else alm = true;
                            if (!_machine_alarms.ContainsKey(machine))
                                _machine_alarms.Add(machine, alm);
                        }
                    }
                    con.Close();
                    dr.Close();
                }
            }
            else
            {
                using (var con = new SqlConnection(conString))
                {
                    var cmd = new SqlCommand("get_machine_alarm", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    //cmd.Parameters.Add("@idm", SqlDbType.Int).Value = mac;
                    cmd.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = Get_from_date();
                    cmd.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = Get_to_date();
                    cmd.Parameters.Add("@shift", SqlDbType.VarChar, 200).Value = Get_shift_array().ToString();

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
        }

        public static int ReturnAssemblyNumber(string assemblyText)
        {
            var sb = new System.Text.StringBuilder();
            foreach (char c in assemblyText)
            {
                if (char.IsDigit(c))
                {
                    sb.Append(c);
                }
            }
            int.TryParse(sb.ToString(), out var assembluNumber);
            return assembluNumber;
        }

        private bool _hasNewUpdate = false;
        private System.Threading.Timer _tmCheck = null;

        private bool _operatorMode = false;
        private void button3_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            foreach (var b in _lstOfModes)
            {
                if (btn.Name == b.Name) b.Image = Properties.Resources.modes1_green;
                else b.Image = Properties.Resources.modes1_silver;
            }
            Mode = "eff";            
            SetDefaultFilters(false);
            _operatorMode = !_operatorMode;

            if (cboArt.SelectedIndex > 0)
            {
                cboArt.SelectedIndex = 0;
                _isArticleSelected = false;
                Set_file_name(string.Empty);
            }
            GetData();
        }

        private void CheckForUpdates(object info)
        {
            try
            {
                var pathMain = AppDomain.CurrentDomain.BaseDirectory;
                
                var strAssembOld = "";
                foreach (var file in Directory.GetFiles(pathMain))
                {
                    if (Path.GetExtension(file) != ".exe") continue;
                    strAssembOld = Assembly.LoadFile(file).GetName().Version.ToString();
                }

                var strAssembNew = "";
                foreach (var file in Directory.GetFiles(Properties.Settings.Default.downloadSource))
                {
                    if (Path.GetExtension(file) != ".exe") continue;
                    strAssembNew = Assembly.LoadFile(file).GetName().Version.ToString();
                }

                var newVr = ReturnAssemblyNumber(strAssembNew);
                var oldVr = ReturnAssemblyNumber(strAssembOld);

                if (newVr <= oldVr)
                {
                    _hasNewUpdate = false;
                    return;
                }

                _hasNewUpdate = true;

                var diag = MessageBox.Show("New version Sinotico " + strAssembNew + " is available. Do you want to update Sinotico " + strAssembOld + "?",
                    Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (diag == DialogResult.Yes)
                {
                    if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Backup"))
                    {
                        //create 'Backup folder' before update
                        Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Backup");
                    }
                    else
                    {
                        //if exist clear backup folder before update
                        foreach (var f in Directory.GetFiles(pathMain + "\\Backup"))
                        {
                            File.Delete(f);
                        }
                    }

                    var sourceFileName = AppDomain.CurrentDomain.BaseDirectory + "\\Sinotico.exe";
                    var sourceFileConfing = AppDomain.CurrentDomain.BaseDirectory + "\\Sinotico.exe.config";
                    var destinationDirApp = AppDomain.CurrentDomain.BaseDirectory + "\\Backup";

                    //perform moving current version to backup directory
                    File.Move(sourceFileName, destinationDirApp + "\\exebck");
                    File.Move(sourceFileConfing, destinationDirApp + "\\configbck");

                    var copySourceDir = Properties.Settings.Default.downloadSource;
                    // copy new version from server to local user directory
                    File.Copy(copySourceDir + "\\Sinotico.exe", AppDomain.CurrentDomain.BaseDirectory + "\\Sinotico.exe");
                    File.Copy(copySourceDir + "\\Sinotico.exe.config", AppDomain.CurrentDomain.BaseDirectory + "\\Sinotico.exe.config");
                    
                    var restart = MessageBox.Show("Your software is up to date. Do you want to restart application to continue on Sinotico " + strAssembNew + "?",
                        Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (restart == DialogResult.Yes)
                    {
                        Application.Restart();
                    }
                }
            }
            catch
            {
                //MessageBox.Show("Invalid path or network connection.", Application.ProductName + " Settings",
                //MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        private void CheckSilentForUpdates()
        {
            if (_tmCheck != null) _tmCheck.Dispose();

            TimerCallback tcb = new TimerCallback(CheckForUpdates);
            AutoResetEvent are = new AutoResetEvent(false);
            //Application.DoEvents();

            _tmCheck = new System.Threading.Timer(tcb, are, 120000, 1000);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style &= ~0xC00000; // Not allow WS_CAPTION
                return cp;
            }
        }
    }

    /// <summary>
    /// Class that deliver machines' properties from current resource.
    /// </summary>
    public class CurrentInfo
    {
        #region Constructors

        public CurrentInfo() { }

        public CurrentInfo(string machine, string gaudge, string fileName, string speed, bool alarm)
        {
            MachineNumber = machine;
            Gaudge = gaudge;
            FileName = fileName;
            Speed = speed;
            Alarm = alarm;
        }

        #endregion

        #region Properties

        public string MachineNumber { get; set; }

        public string Gaudge { get; set; }

        public string FileName { get; set; }

        public string Speed { get; set; }

        public bool Alarm { get; set; }

        public string Cleaned { get; set; }

        public string ClndHours { get; set; }

        public DateTime EvntDate { get; set; }


        #endregion
    }
    /// <summary>
    /// Class that contains ready-made outputs, 
    /// related to the real and finished production parameters.
    /// </summary>
    public class Production
    {
        #region Constructors

        public Production() { }

        public Production(int eff, int qty, float timestd, float stop, float speed)
        {
            Eff = eff;
            Qty = qty;
            TimeStd = timestd;
            MacStop = stop;
            TimeSpeed = speed;
        }
        #endregion

        #region Properties

        public int Eff { get; set; }

        public int Qty { get; set; }

        public float TimeStd { get; set; }

        public float MacStop { get; set; }

        public float TimeSpeed { get; set; }

        #endregion
    }
    /// <summary>
    /// Class that defines line as a structure.
    /// </summary>
    public class Line
    {
        public string LineName { get; set; }
        public List<string> _articles = new List<string>();
        public int NumberOfArticles { get; set; }
        public int FromMachine { get; set; }
        public int ToMachine { get; set; }

        public Line()
        {            
            _articles = new List<string>();
            NumberOfArticles = 0;
        }
        public Line(string newArticle)
        {            
            _articles = new List<string>();
            _articles.Add(newArticle);
            NumberOfArticles += 1;
        }
        public void InsertArticle(string newArticle)
        {
            _articles.Add(newArticle);
            NumberOfArticles += 1;
        }
        /// <summary>
        /// Gets or sets line name by detected machine number.
        /// </summary>
        /// <param name="machineNumber"></param>
        /// <returns></returns>
        public string GetLineNumber(int machineNumber)
        {
            int number = 1;     //set 1 as start point
            var count = 0;

            for (int x = 1; x <= machineNumber; x++)
            {
                count++;

                if (count == 15)
                {
                    count = 1;
                    number++;
                }
            }

            return "LINE " + number.ToString();
        }
        public void SetLineParameters(int machine)
        {            
            var lowerBound = 1;
            var upperBound = 14;
            for (var line = 1; line <= 15; line++)
            {
                if(machine >= lowerBound && machine <= upperBound)
                {
                    FromMachine = lowerBound;
                    ToMachine = upperBound;
                    LineName = "Line " + line.ToString();
                    break;
                }
                else
                {
                    lowerBound += 14;
                    upperBound += 14;
                }
            }
        }
    }
    public class MachinesProperties
    {
        public MachinesProperties() { }
        public MachinesProperties(string mac, string eff)
        {
            Machine = mac;
            Eff = eff;
        }
        public string Machine { get; set; }
        public string Eff { get; set; }
    }
    public class Squadra
    {
        public string SquadraName { get; set; }
        public int FromMachine { get; set; }
        public int ToMachine { get; set; }
        public List<string> _articles_in_squadra = new List<string>();

        public Squadra() { }
        public Squadra(string name)
        {
            SquadraName = name;
            _articles_in_squadra = new List<string>();
        }
        public void InsertArticle(string newArticle)
        {
            _articles_in_squadra.Add(newArticle);
        }
    }
    public class Lines
    {
        public List<DateTime> _holidays = new List<DateTime>();
        public Lines() { }
        public Lines(string lineName)
        {
            LineName = lineName;
            _holidays = new List<DateTime>();
        }
        public Lines(DateTime day, string lineName)
        {
            LineName = lineName;
            _holidays = new List<DateTime>();
            _holidays.Add(day);
        }
        public string LineName { get; set; }
    }
    public class CleanedMachines
    {
        public int MachineNumber { get; set; }
        public int CleanedHours { get; set; }
        public DateTime EventDate { get; set; }
        public CleanedMachines() { }
        public CleanedMachines(int macNum, int clndHrs, DateTime evDate)
        {
            MachineNumber = macNum;
            CleanedHours = clndHrs;
            EventDate = evDate;
        }
    }
    public class Cleaners
    {
        public Cleaners() { }
        public string Machine { get; set; }
        public string NameInitials { get; set; }
        public int Prodgen { get; set; }
        public Cleaners(string machine, string nameinit, int prodgen)
        {
            Machine = machine;
            NameInitials = nameinit;
            Prodgen = prodgen;
        }
    }
    public class ScartiRamendi
    {
        public ScartiRamendi() { }
        public ScartiRamendi(int mac,int teli_buoni, int scarti, int rammendi)
        {
            Machine = mac;
            TeliBuoni = teli_buoni;
            Scarti = scarti;
            Rammendi = rammendi;
        }
        public int Machine { get; set; }
        public int TeliBuoni { get; set; }
        public int Scarti { get; set; }
        public int Rammendi { get; set; }
    }
    public class Temperature
    {
        public Temperature() { }
        public Temperature(string line, double temp, double hum)
        {
            Line = line;
            Temp = temp;
            Hum = hum;
        }
        public string Line { get; set; }
        public double Temp { get; set; }
        public double Hum { get; set; }
    }
    public class Operator
    {
        public Operator() { }
        public Operator(string fullName, string line)
        {
            FullName = FullName;
            Line = line;
        }
        public string FullName { get; set; }
        public string Line { get; set; }
    }
}