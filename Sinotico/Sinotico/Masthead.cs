using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Sinotico
    {
    public partial class Masthead : UserControl
        {
        public Masthead()
            { 
            InitializeComponent();

            //setup
            Dock = DockStyle.Top;
            SendToBack();
            Height = 70;

            SetStyle(
                System.Windows.Forms.ControlStyles.UserPaint |
                System.Windows.Forms.ControlStyles.AllPaintingInWmPaint |
                System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer,
                true);
            }

        /// <summary>
        /// Property that can be found in 'Property grid'
        /// </summary>
        [Description("Text displayed as a title.")]
        [Category("Data")]
        public string TitleText
            {
            get => lbl_Title.Text;
            set => lbl_Title.Text = value;
            }

        private void ScanproTitleBar_Load(object sender, EventArgs e)
            {
            //title appereance

            lbl_Title.BackColor = Color.Black;
            
            lbl_Title.ForeColor = Color.White;
            lbl_Title.Text = TitleText;
            lbl_Title.TextAlign = ContentAlignment.MiddleCenter; //software default
            }
        }
    }