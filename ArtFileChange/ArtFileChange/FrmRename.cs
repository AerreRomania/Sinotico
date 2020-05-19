using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Data.SqlClient;

namespace ArtFileChange
    {
    public partial class FrmRename : Form
        {
        public FrmRename()
            {
            InitializeComponent();

            LoadParts();
            }

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        private string _renamedFileName;
        private string _selectedFileName;
        private string _fileExtent;
        private string _filePath;

        private int _wd;
        private int _hg;

        public static System.Collections.Generic.List<string> ListOfHistory { get; set; }

        private void ShowFileDialog()
            {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
                {
                _selectedFileName = ofd.FileName;
                Icon ico = Icon.ExtractAssociatedIcon(ofd.FileName);
                lblFilename.Text = System.IO.Path.GetFileName(ofd.FileName);
                lblPath.Text = ofd.FileName;
                pbFileico.Image = ico.ToBitmap();

                _fileExtent = _selectedFileName.Split('.').Last();
                _filePath = Path.GetDirectoryName(ofd.FileName);
                }

            ofd.Dispose();
            }

        private void FrmRename_Load(object sender, EventArgs e)
            {
            // Hide
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);

            _renamedFileName = string.Empty;
            _selectedFileName = string.Empty;

            ListOfHistory = new System.Collections.Generic.List<string>();

            textBox3.UserText = "000";

            _wd = Width;
            _hg = Height;

            textBox3.TextSize = 3;
            textBox2.TextSize = 3;
            textBox5.TextSize = 8;

            FormClosing += form_close;
            }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
            {
            ShowFileDialog();
            }
        
        private void button1_Click(object sender, EventArgs e)
            {
            
            }

        public static string TargetArticle;
        public static string TargetPart;

        private void FormatData()
            {
            if (textBox1.UserText == string.Empty
             || textBox2.UserText == string.Empty
             || textBox3.UserText == string.Empty
             || textBox5.UserText == string.Empty)
                {
                textBox4.UserText = "";
                MessageBox.Show("Input ins't valid.");
                return;
                }

            if (_selectedFileName == string.Empty)
                {
                MessageBox.Show("No selected file.");
                return;
                }

            _renamedFileName = textBox1.UserText + "-" + textBox2.UserText + "-" + textBox3.UserText + "-" + textBox5.UserText;

            textBox4.UserText = _renamedFileName;
            textBox4.BorderColor = Color.DodgerBlue;
            textBox4.Refresh();

            try
                {
                var oldFile = _selectedFileName;
                var newFile = _filePath + "\\" + textBox4.UserText + "." + _fileExtent;

                if (File.Exists(oldFile))
                    {
                    File.Copy(oldFile, newFile, true);
                    File.Delete(oldFile);
                    }

                ListOfHistory.Add(Path.GetFileName(oldFile) +
                    " -> " + Path.GetFileName(newFile));
                
                TargetArticle = textBox5.UserText;
                TargetPart = textBox1.UserText;

                var frmArtComb = new FrmArticleCombination();
                frmArtComb.ShowDialog();
                frmArtComb.Dispose();

                textBox4.BorderColor = Color.Crimson;

                ResetData();
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message);
                }
            }

        private void button3_Click_1(object sender, EventArgs e)
            {
            FormatData();         
            }
        
        private void button2_Click(object sender, EventArgs e)
            {
            }

        private void ResetData()
            {
            textBox1.UserText = "";
            textBox2.UserText = "";
            textBox3.UserText = "";
            textBox5.UserText = "";
            pbFileico.Image = Properties.Resources.generate_24;
            lblFilename.Text = "Selected file name";
            lblPath.Text = "Selected file full address";
            textBox4.UserText = "";

            _selectedFileName = string.Empty;
            _fileExtent = string.Empty;
            _renamedFileName = string.Empty;
            _filePath = string.Empty;
            }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
            {
            //SaveData();
            }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
            {
            ResetData();
            }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
            {
            Application.Exit();
            }

        private void form_close(object sender, FormClosingEventArgs e)
            {
            if (_selectedFileName != string.Empty)
                {
                var dr = MessageBox.Show("Are you sure you want to discard your changes?", "AFC", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                    {
                    e.Cancel = false;
                    }
                else
                    {
                    e.Cancel = true;
                    }
                }
            else
                {
                e.Cancel = false;
                }

            }

        private void button4_Click(object sender, EventArgs e)
            {
            
            }

        private void historyToolStripMenuItem_Click_1(object sender, EventArgs e)
            {
            var hist = new FrmHistory();
            hist.ShowDialog();
            hist.Dispose();
            }

        private Geometry _geometry = new Geometry();
        private void button3_Paint(object sender, PaintEventArgs e)
            {
            var btn = (Button)sender;

            var pen = new Pen(new SolidBrush(Color.White), 4);

            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(0,0, btn.Width, btn.Height), 7))
                {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                //e.Graphics.FillPath(Brushes.White, path);
                e.Graphics.DrawPath(pen, path);

                btn.Region = new Region(path);
                e.Graphics.SmoothingMode = e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                }
            }

        private void button1_Click_1(object sender, EventArgs e)
            {
           
            }

        private void btnGenSave_Paint(object sender, PaintEventArgs e)
            {
            var btn = (Button)sender;

            var pen = new Pen(new SolidBrush(Color.White), 4);

            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(0, 0, btn.Width, btn.Height), 7))
                {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                //e.Graphics.FillPath(Brushes.White, path);
                e.Graphics.DrawPath(pen, path);

                btn.Region = new Region(path);
                e.Graphics.SmoothingMode = e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                }
            }

        private void lblSearch_Click(object sender, EventArgs e)
            {
            ShowFileDialog();
            }

        private void btnGenSave_Click(object sender, EventArgs e)
            {
            //SaveData();
            }

        private void lblReset_Click(object sender, EventArgs e)
            {
            ResetData();
            }

        private void OpenDatabase()
            {
            var db = new FrmDatabase();

            Visible = false;
            db.WindowState = FormWindowState.Maximized;

            db.ShowDialog();
            db.Dispose();

            Application.Restart();
            }

        private void lblDb_Click(object sender, EventArgs e)
            {
            OpenDatabase();
            }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
            {
            OpenDatabase();
            }

        private void LoadParts()
            {
            textBox1.ClearCollection();
            textBox1.Collection = new ComboBox.ObjectCollection(textBox1.comboBoxControl);

            var q = "select scelt from combarchive";
            using (var con = new SqlConnection(Config.ConString))
                {
                var cmd = new SqlCommand(q, con);

                con.Open();
                var dr = cmd.ExecuteReader();
                if (dr.HasRows)
                    {
                    while (dr.Read())
                        {
                        textBox1.Collection.Add(dr[0].ToString());
                        }
                    }
                con.Close();
                dr.Close();
                cmd = null;
                }     
            }

        private void fromatToolStripMenuItem_Click(object sender, EventArgs e)
            {
            FormatData();
            }

        private void FrmRename_SizeChanged(object sender, EventArgs e)
            {
            textBox4.Refresh();

            if (WindowState == FormWindowState.Maximized)
                {
                lblLogo.Visible = true;
                }
            else
                {
                if (WindowState == FormWindowState.Normal)
                    {
                    if (Width > _wd + 100 && Height > _hg + 100)
                        {
                        lblLogo.Visible = true;
                        }
                    else
                        {
                        lblLogo.Visible = false;
                        }
                    }
                else
                    {
                    lblLogo.Visible = false;
                    }             
                }
            }

        private void pbFileico_DoubleClick(object sender, EventArgs e)
            {
           
            }

        private void pbFileico_Click(object sender, EventArgs e)
            {
            ShowFileDialog();
            }

        private void jobTypesToolStripMenuItem_Click(object sender, EventArgs e)
            {
            var f = new Jobs();
            f.StartPosition = FormStartPosition.CenterScreen;
            f.ShowDialog();
            f.Dispose();
            }

        private void operatorsToolStripMenuItem_Click(object sender, EventArgs e)
            {
            var f = new Operators();
            f.StartPosition = FormStartPosition.CenterScreen;
            f.ShowDialog();
            f.Dispose();
            }
        }
    }
