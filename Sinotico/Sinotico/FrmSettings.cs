using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sinotico.DatabaseTableClasses;
using System.Data.Linq;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Sinotico
{
    public partial class FrmSettings : Form
    {
        private ToolTip _toolTip = new ToolTip();
        private bool _hasNewUpdate = false;
        private System.Threading.Timer _tmCheck;
        private IntensityBounds currIntesity = new IntensityBounds();

        public FrmSettings()
        {           
            InitializeComponent();
            btnSave.Click += btnSave_Click;
            cbUpdateRuntime.CheckedChanged += cbUpdateRuntime_CheckedChanged;
            btnUpdate.Click += btnUpdate_Click;
        }

        protected override void OnLoad(EventArgs e)
        {
            FrmHolidays.dc = new DataContext(MainWnd.conString);
            StyleDataGridView(dgvSettings);
            LoadData();

            cb_interval.SelectedIndex = GetInterval(MainWnd.GetTableSource());

            if (Properties.Settings.Default.UpdateSettings)
            {
                //update user settings from previous version
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.UpdateSettings = false;
                Properties.Settings.Default.Save();
            }
            AddClearFilterButton(txtDownloadSource, "btnAddPath");

            cbUpdateRuntime.Checked = Properties.Settings.Default.checkUpdates;
            txtDownloadSource.Text = Properties.Settings.Default.downloadSource;
            Intensity_slider.Value = currIntesity.Value;
            intensity_label.Text = "Selected intesity: "+Intensity_slider.Value.ToString();

            //var pathMain = AppDomain.CurrentDomain.BaseDirectory;            
            //var strAssembOld = "";
            //foreach (var file in Directory.GetFiles(pathMain))
            //{
            //    if (Path.GetExtension(file) != ".exe") continue;
            //    strAssembOld = Assembly.LoadFile(file).GetName().Version.ToString();
            //}
            //linkLabel1.Text = "Sinotico version: " + strAssembOld;
            //linkLabel1.Refresh();

            base.OnLoad(e);
        }


        #region Update
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
                //pbSettings.Refresh();
            }
            catch
            {
                MessageBox.Show("Invalid path or network connection.", Application.ProductName + " Settings",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _hasNewUpdate = false;
                //pbSettings.Refresh();
            }
        }
        private void cbUpdateRuntime_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                _toolTip.Show("Update silent listener that will gives information about new version when is available.", (CheckBox)sender);
            }
            else
            {
                _toolTip.Hide((CheckBox)sender);
            }
        }
        private void AddClearFilterButton(TextBox txt, string name)
        {
            var _browseDownloadPath = new Button
            {
                Size = new Size(24, txt.ClientSize.Height + 2)
            };
            _browseDownloadPath.Location = new Point(txt.ClientSize.Width - _browseDownloadPath.Width, -1);
            _browseDownloadPath.BackgroundImage = Properties.Resources.icons8_folder_24;
            _browseDownloadPath.BackgroundImageLayout = ImageLayout.Zoom;
            _browseDownloadPath.Cursor = Cursors.Default;
            _browseDownloadPath.BackColor = default(Color);
            _browseDownloadPath.Name = name;
            _browseDownloadPath.Parent = txt;

            _browseDownloadPath.Click += (s, g) =>
            {
                var srcPath = new FolderBrowserDialog();
                srcPath.Description = "Search download path";
                if (srcPath.ShowDialog() == DialogResult.OK)
                {
                    txt.Text = srcPath.SelectedPath;

                    Properties.Settings.Default.downloadSource = srcPath.SelectedPath;
                    Properties.Settings.Default.Save();
                }
            };

            //SendMessage(txt.Handle, 0xd3, (IntPtr)2, (IntPtr)(_browseDownloadPath.Width << 16));
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                var pathMain = AppDomain.CurrentDomain.BaseDirectory;

                linkLabel1.Text = "Checking for updates...";
                linkLabel1.Refresh();
                SuspendLayout();
                var strAssembOld = "";
                foreach (var file in Directory.GetFiles(pathMain))
                {
                    if (Path.GetExtension(file) != ".exe") continue;
                    strAssembOld = Assembly.LoadFile(file).GetName().Version.ToString();
                }

                var strAssembNew = "";
                foreach (var file in Directory.GetFiles(txtDownloadSource.Text))
                {
                    if (Path.GetExtension(file) != ".exe") continue;
                    strAssembNew = Assembly.LoadFile(file).GetName().Version.ToString();
                }

                var newVr = ReturnAssemblyNumber(strAssembNew);
                var oldVr = ReturnAssemblyNumber(strAssembOld);

                if (newVr <= oldVr)
                {
                    linkLabel1.Text = "0 results";
                    linkLabel1.ForeColor = Color.Red;
                    linkLabel1.Refresh();

                    ResumeLayout(true);
                    MessageBox.Show("No updates available.",
                        Application.ProductName,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    linkLabel1.Text = "";
                    linkLabel1.Refresh();
                    return;
                }

                linkLabel1.Text = "1 results";
                linkLabel1.ForeColor = Color.SeaGreen;
                linkLabel1.Refresh();

                var diag = MessageBox.Show("New version Sinotico " + strAssembNew + " is available. Do you want to update Sinotico " + strAssembOld + "?",
                    Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (diag == DialogResult.Yes)
                {
                    linkLabel1.Text = "Updaing " + strAssembNew;
                    linkLabel1.ForeColor = Color.DodgerBlue;
                    linkLabel1.Refresh();

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

                    var copySourceDir = txtDownloadSource.Text;
                    // copy new version from server to local user directory
                    File.Copy(copySourceDir + "\\Sinotico.exe", AppDomain.CurrentDomain.BaseDirectory + "\\Sinotico.exe");
                    File.Copy(copySourceDir + "\\Sinotico.exe.config", AppDomain.CurrentDomain.BaseDirectory + "\\Sinotico.exe.config");

                    ResumeLayout(true);
                    linkLabel1.Text = "";
                    linkLabel1.ForeColor = Color.Black;
                    linkLabel1.Refresh();

                    var restart = MessageBox.Show("Your software is up to date. Do you want to restart application to continue on Sinotico " + strAssembNew + "?",
                        Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (restart == DialogResult.Yes)
                    {
                        Application.Restart();
                    }
                }
                else
                {
                    ResumeLayout(true);
                    linkLabel1.Text = "";
                    linkLabel1.Refresh();
                }
            }
            catch
            {
                MessageBox.Show("Invalid path or network connection.", Application.ProductName + " Settings",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ResumeLayout(true);
                linkLabel1.Text = "";
                linkLabel1.Refresh();
            }
        }
        #endregion Update

        #region Colors
        private void LoadData()
        {
            FrmHolidays.dc = new DataContext(MainWnd.conString);
            var data = (from records in Tables.TblEfficiencyBounds
                        select records).ToList();
            dgvSettings.DataSource = data;
            var intesinty = (from intesities in Tables.TblIntensity select intesities).FirstOrDefault();
            currIntesity = intesinty;

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            FrmHolidays.dc = new DataContext(MainWnd.conString);
            foreach(DataGridViewRow row in dgvSettings.Rows)
            {
                var record = (from rec in Tables.TblEfficiencyBounds
                             where rec.Type == row.Cells["Type"].Value.ToString()
                             select rec).SingleOrDefault();
                record.Green = row.Cells["Green"].Value.ToString();
                record.Yellow = row.Cells["Yellow"].Value.ToString();
                record.Red = row.Cells["Red"].Value.ToString();
            }
            FrmHolidays.dc.SubmitChanges();
            LoadData();
        }
        private void StyleDataGridView(DataGridView dgv)
        {
            dgv.DataBindingComplete += delegate
            {
                dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;
                dgv.ReadOnly = false;
                dgv.AllowUserToResizeRows = false;
                dgv.AllowUserToDeleteRows = false;
                dgv.AllowUserToAddRows = true;
                dgv.AllowUserToResizeColumns = false;
                dgv.AllowUserToOrderColumns = false;
                dgv.AutoSize = true;
                dgv.AllowDrop = false;
                dgv.RowHeadersVisible = false;
                dgv.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(132, 189, 165);
                dgv.RowsDefaultCellStyle.ForeColor = Color.FromArgb(60, 60, 60);
                dgv.BorderStyle = BorderStyle.None;
                dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(132, 189, 165);
               
                foreach (DataGridViewColumn c in dgv.Columns)
                {
                    if (c.Name == "Id") c.Visible = false;
                    c.SortMode = DataGridViewColumnSortMode.NotSortable;
                    c.DefaultCellStyle.Font = FrmHolidays._consolas;
                    c.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    c.DefaultCellStyle.BackColor = Color.FromArgb(230, 230, 230);
               
                }
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgv.ColumnHeadersDefaultCellStyle.Font = FrmHolidays._consolas;
                dgv.EnableHeadersVisualStyles = false;
                dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.Gold;
            };
        }
        #endregion Colors
        private void btnCalendar_Click(object sender, EventArgs e)
        {
            var frmHolidays = new FrmHolidays();
            frmHolidays.ShowDialog();
            frmHolidays.Dispose();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cbUpdateRuntime_CheckedChanged_1(object sender, EventArgs e)
        {
            Properties.Settings.Default.checkUpdates = ((CheckBox)sender).Checked;
            Properties.Settings.Default.Save();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            FrmHolidays.dc = new DataContext(MainWnd.conString);
           
                var record = (from intens in Tables.TblIntensity
                              where intens.Id == currIntesity.Id
                              select intens).SingleOrDefault();
            record.Type = currIntesity.Type;
            record.Value = Intensity_slider.Value;
            FrmHolidays.dc.SubmitChanges();
            
            LoadData();
        }
        private bool block = false;
        private int sliderValue = 50;
        private int step = 25;
        private void Intensity_slider_ValueChanged(object sender, EventArgs e)
        {
            if (block) return;
            sliderValue = Intensity_slider.Value;
            if (sliderValue % step != 0)
            { 
                sliderValue = (sliderValue / step) * step;
                block = true;
                Intensity_slider.Value = sliderValue;
                block = false;
            }
            intensity_label.Text ="Selected intesity: "+ Intensity_slider.Value.ToString();
        }

        private int GetInterval(string interval)
        {
            int index=-1;
           switch(interval)
            {
                case "extendview": index=0;
                    break;
                case "currentyearview": index=1;
                    break;
                case "historyview":index= 2;
                    break;
            }
            return index;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
                switch (cb_interval.SelectedIndex)
                {
                    case 0:
                        {
                            MainWnd.SetTableSource("extendview");
                            
                            Properties.Settings.Default.TableSource = MainWnd.GetTableSource();
                        Properties.Settings.Default.Save();
                        MessageBox.Show("Saved!", "Data Interval");
                    }
                        break;
                    case 1:
                        {
                            MainWnd.SetTableSource("currentyearview");
                            
                        Properties.Settings.Default.TableSource = MainWnd.GetTableSource();
                        Properties.Settings.Default.Save();
                        MessageBox.Show("Saved!", "Data Interval");
                    }
                        break;
                    case 2:
                        {
                            MainWnd.SetTableSource("historyview");
                           
                        Properties.Settings.Default.TableSource = MainWnd.GetTableSource();
                        Properties.Settings.Default.Save();
                        MessageBox.Show("Saved!", "Data Interval");
                    }
                        break;
                   
                }
            
        }
    }
}
