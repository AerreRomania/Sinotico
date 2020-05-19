using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using static System.Windows.Forms.ComboBox;

namespace ArtFileChange
    {
    public partial class MyTextBox : UserControl
        {
        public MyTextBox()
            {
            InitializeComponent();
            }

        [Description("Group border color.")]
        [Category("Data")]
        public Color BorderColor { get; set; }

        [Description("Allows user only to select from the drop-down list")]
        [Category("Behavior")]
        public bool AllowTyping { get; set; }

        [Description("Allows user only to select witch type of control will use")]
        [Category("Behavior")]
        public bool UseTextualOnly { get; set; }

        [Description("Allows user to type in upper case mode")]
        [Category("Behavior")]
        public bool UseUpperCase { get; set; }

        [Description("Insert items to combobox")]
        [Category("Data")]
        public ObjectCollection Collection { get; set; }

        public ComboBox comboBoxControl { get { return comboBox; } set { comboBox = value; } }

        private Geometry _geometry = new Geometry();

        protected override void OnPaint(PaintEventArgs e)
            {
            var pen = new Pen(new SolidBrush(BorderColor), 4);

            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(-1, -1, Width, Height), 7))
                {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawLine(new Pen(new SolidBrush(BorderColor), 4), 0, Height, Width, Height);

                Region = new Region(path);
                e.Graphics.SmoothingMode = e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                }

            base.OnPaint(e);
            }
        
        private void MyTextBox_Load(object sender, EventArgs f)
            {
            comboBox.Width = Width - 20;
            textBox1.Width = Width - 20;
            CheckForIllegalCrossThreadCalls = false;
            if (UseTextualOnly)
                {
                comboBox.Visible = false;
                textBox1.Visible = true;

                if (UseUpperCase)
                    {
                    textBox1.CharacterCasing = CharacterCasing.Upper;
                    }
                }
            else
                {
                comboBox.Visible = true;
                textBox1.Visible = false;                
                }

            if (!AllowTyping)
                {
                if (UseTextualOnly)
                    {
                    textBox1.ReadOnly = true;
                    textBox1.BackColor = Color.White;
                    return;
                    }

                //comboBox.AllowDrop = true;
                //comboBox.DrawMode = DrawMode.OwnerDrawFixed;
                comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                //comboBox.FormattingEnabled = true;

                if (Collection != null)
                    {
                    foreach (var item in Collection)
                        {
                        comboBox.Items.Add(item);
                        }

                    comboBox.DropDownHeight = 200;
                    comboBox.DropDownWidth = 200;
                    }
                }

            textBox1.GotFocus += delegate 
            {
                var trd = new System.Threading.Thread(new System.Threading.ThreadStart(DecrementFont));
                if (trd.ThreadState != System.Threading.ThreadState.Running)
                {
                    trd.TrySetApartmentState(System.Threading.ApartmentState.STA);
                    trd.Start();
                }
            };
            textBox1.LostFocus += delegate
            {
                var trd = new System.Threading.Thread(new System.Threading.ThreadStart(IncrementFont));
                if (trd.ThreadState != System.Threading.ThreadState.Running)
                {
                    trd.TrySetApartmentState(System.Threading.ApartmentState.STA);
                    trd.Start();
                }
            };
        }

        private void DecrementFont()
        {
            for (var i = 14; i >= 8; i--)
            {
                label1.Font = new Font("Segoe UI", i, FontStyle.Regular);
                System.Threading.Thread.Sleep(10);
                label1.Refresh();
            }
            label1.ForeColor = Color.SteelBlue;
        }
        private void IncrementFont()
        {
            for (var i = 8; i <= 14; i++)
            {
                label1.Font = new Font("Segoe UI", i, FontStyle.Regular);
                System.Threading.Thread.Sleep(10);
                label1.Refresh();
            }
            label1.ForeColor = Color.Black;
        }

        public void ClearCollection()
            {
            if (Collection != null) Collection.Clear();
            //comboBox.Items.Clear();
            }
        
        [Description("Text displayed as a title.")]
        [Category("Data")]
        public string TitleText {
            get => label1.Text;
            set => label1.Text = value;
            }

        [Description("Text displayed as a title.")]
        [Category("Data")]
        public int TextSize {
            get {
                if (UseTextualOnly)
                    {
                    return textBox1.MaxLength;
                    }
                else
                    {
                    return comboBox.MaxLength;
                    }
                }
            set {
                if (UseTextualOnly)
                    {
                    textBox1.MaxLength = value;
                    }
                else
                    {
                    comboBox.MaxLength = value;
                    }
                }
            }

        [Description("Text displayed as an input.")]
        [Category("Data")]
        public string UserText {   
            get {
                if (UseTextualOnly)
                    {
                    return textBox1.Text;
                    }
                else
                    {
                    return comboBox.Text;
                    }
                }
            set {
                if (UseTextualOnly)
                    {
                    textBox1.Text = value;
                    }
                else
                    {
                    comboBox.Text = value;
                    }
                }
            }
        }
    }
