using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sinotico
    {
        internal class LoadingInfo : Form
        {
        private static LoadingInfo _loadingInfo;     
        private static Label _lbl;
        private readonly System.Windows.Forms.Timer _tmDots = new System.Windows.Forms.Timer();
        private static ProgressBar _loadingProgressBar = new ProgressBar();

        public static string InfoText { get; set; }
        private static int ProgressMax => 0;
            //public int ProgressValue { get; set; }

        private static Form _formBlock = new Form();

        private LoadingInfo()
            {
            FormBorderStyle = FormBorderStyle.FixedDialog;
            BackColor = Color.FromArgb(225, 225, 225);    
            ControlBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(400, 100);
            CheckForIllegalCrossThreadCalls = false;
            ShowInTaskbar = false;
            TopMost = true;
            }

            public sealed override Color BackColor
            {
                get { return base.BackColor; }
                set { base.BackColor = value; }
            }

//        private static void ShowBlockSiteForm()
//            {
//            _formBlock.ShowDialog();
//            }

            public static void ShowLoading()
            {
            if (MainWnd.IsAuto) return;

            if (_loadingInfo != null)
                return;

            var threadb = new Thread(delegate ()
                {
                    _formBlock = new Form
                        {
                        FormBorderStyle = FormBorderStyle.None,
                        BackColor = Color.White,
                        WindowState = FormWindowState.Maximized,
                        ShowIcon=false,
                        ShowInTaskbar=false,
                        Opacity = 0.1
                        };
                    Application.Run(_formBlock);
                    });
            threadb.SetApartmentState(ApartmentState.STA);
            threadb.Start();

            var thread = new Thread(delegate ()
                {
                    _loadingInfo = new LoadingInfo();
                    Application.Run(_loadingInfo);
                    });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            }

        public static void CloseLoading()
            {
            if (MainWnd.IsAuto) return;

            if (_loadingInfo == null) return;

            CloseBlock();
            CloseForm();
            }

        private static void CloseForm()
            {
            _loadingInfo.Close();
            _loadingInfo = null;
            }
        private static void CloseBlock()
            {
            _formBlock.Close();
            _formBlock = null;
            }

        //Delegate for cross thread call to close
//        private delegate void OpenDelegate();
//        private delegate void CloseDelegate();
//        private delegate void CloseBlockDelegate();
//
//        private void InitializeComponent()
//            { 
//            SuspendLayout();
//            // 
//            // LoadingInfo
//            // 
//            ClientSize = new System.Drawing.Size(148, 0);
//            Name = "LoadingInfo";
//            ResumeLayout(false);
//
//            }

        private readonly System.Windows.Forms.Timer _tm = new System.Windows.Forms.Timer();

        protected override void OnLoad(EventArgs e)
            {
            //_externalFlag = false;
            if (_loadingInfo == null) return;

            _lbl = new Label
                {
                Font = new Font("Microsoft Sans Serif", 8, FontStyle.Regular),
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(225, 225, 225),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleLeft,
                Width = 300,
                Height = 50
                };

            Controls.Add(_lbl);
            _lbl.Dock = DockStyle.Top;
            
            _loadingProgressBar = new ProgressBar();
            _loadingProgressBar.Location = new Point(10, 60);
            _loadingProgressBar.Width = Width - 30;
            _loadingProgressBar.Maximum = ProgressMax;
            _loadingProgressBar.Minimum = 0;
            _loadingProgressBar.Value = 0;
            _loadingProgressBar.Style = ProgressBarStyle.Marquee;
            _loadingProgressBar.MarqueeAnimationSpeed = 30;

            Controls.Add(_loadingProgressBar);
            _loadingProgressBar.BringToFront();

            Task.Factory.StartNew(() => { UpdateWaiting(); });

            base.OnLoad(e);
            }

        private void UpdateWaiting()
            {
            if (_loadingInfo == null) return;

            if (InvokeRequired)
                {
                BeginInvoke(new Action(() =>
                {
                    UpdateProgress();
                    UpdateText(InfoText);
                }));
                }
            else
                {
                UpdateProgress();
                UpdateText(InfoText);
                }
            }

        public static void UpdateProgress()
            {
            _loadingProgressBar.Maximum = 211;

            if (_loadingProgressBar.InvokeRequired)
                {
                _loadingProgressBar.BeginInvoke((MethodInvoker)delegate { _loadingProgressBar.Value = _loadingProgressBar.Value + 1; });
                } 
            else
                {
                _loadingProgressBar.Value = _loadingProgressBar.Value + 1;
                }

            _loadingProgressBar.Refresh();
            _loadingProgressBar.Update();
            }

        public static void ResetProgress()
            {
            if (_loadingInfo == null) return;

            if (_loadingProgressBar.InvokeRequired)
                {
                _loadingProgressBar.BeginInvoke((MethodInvoker)delegate { _loadingProgressBar.Value = 0; });
                }
            else
                {
                _loadingProgressBar.Value = 0;
                }

            _loadingProgressBar.Refresh();
            _loadingProgressBar.Update();
            }

        public static void UpdateText(string txt)
            {
            if (_loadingInfo == null) return;
            _lbl.Text = new string(' ', 5) + txt;
            _lbl.Refresh();
            }

        protected override void OnFormClosing(FormClosingEventArgs e)
            {
            _tm.Dispose();
            _tmDots.Dispose();

            base.OnFormClosing(e);
            }

        protected override void OnPaint(PaintEventArgs e)
            {
            e.Graphics.DrawString("ONLYOU", new Font("Tahoma", 8, FontStyle.Bold), Brushes.Orange, 2, 2);
            base.OnPaint(e);
            }
        }
    }
