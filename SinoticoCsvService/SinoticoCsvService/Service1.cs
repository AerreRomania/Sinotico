using System.ServiceProcess;
using System.Timers;
using CsvLibExpress;

namespace SinoticoCsvService
    {
    public partial class CsvImporter : ServiceBase
        {
        /* CsvLibExpress*/
        private readonly CsvLib Lib = new CsvLib();
        private readonly Log Log = new Log();
        private readonly CsvRammendi Ram = new CsvRammendi();

        private Timer _timer = new Timer();

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvImporter"/> class.
        /// </summary>
        public CsvImporter()
            {
            InitializeComponent();
            }
        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control Manager (SCM) 
        /// or when the operating system starts (for a service that starts automatically). 
        /// Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
            {
            System.Threading.Thread trd = 
                new System.Threading.Thread(new System.Threading.ThreadStart(InitTimer));

            trd.Start();
            }
        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is sent to the service by the Service Control Manager (SCM). 
        /// Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop()
            {
            _timer.Enabled = false;
            Log.WriteLog(message: "Service has been stopped");
            }
        private void InitTimer()
            {
            Log.WriteLog(message: "Service has been started");

            _timer = new System.Timers.Timer();
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(_timer_Tick);
            _timer.Interval = 100000;   //1,8min check
            _timer.Enabled = true;
            _timer.Start();
            }
        private void _timer_Tick(object sender, ElapsedEventArgs elapsed)
            {
            Lib.ExtractMyFile();
            Ram.SendFileToDb();
            }
        }
    }
