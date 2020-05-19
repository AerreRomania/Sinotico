using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sinotico
    {
    public partial class RepStopMachines : Form
        {
        private DataTable _table_report = new DataTable();
        
        private Color TotalColor { get => Color.FromArgb(167, 200, 125); } 

        public RepStopMachines()
            {
            InitializeComponent();

            dgvReport.AllowUserToAddRows = false;
            dgvReport.AllowUserToDeleteRows = false;
            dgvReport.AllowUserToResizeColumns = false;
            dgvReport.AllowUserToResizeRows = false;
            dgvReport.AllowUserToOrderColumns = false;
            dgvReport.ReadOnly = true;

            dgvReport.DoubleBufferedDataGridView(true);
            }

        protected override void OnLoad(EventArgs e)
            {
            
            lblFrom.Text = MainWnd.Get_from_date().ToString("dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            lblTo.Text = MainWnd.Get_to_date().ToString("dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

            CreateReport();

            LoadingInfo.CloseLoading();
            base.OnLoad(e);
            }

        private void CreateTableColumns()
            {
            _table_report = new DataTable();
            _table_report.Columns.Clear();

            var cols = new string[] { "Macchina",
                "tempo1", "%1",
                "tempo2", "%2",
                "tempo3", "%3",
                "tempo4", "%4",
                "tempo5", "%5",
                "tempo6", "%6",
                "tempo7", "%7",
                "tempo8", "%8",
                "tempo9", "%9",
                "tempo10", "%10",
                "tempo11", "%11",
                "tempo12", "%12",
                "tempo13", "%13",
                "tempo14", "%14",
                "tempo15", "%15",
                "tempo16", "%16",
                "tempo17", "%17",
                "tempo18", "%18",
                "tempo19", "%19",
                "tempo20", "%20",
                "tempo21", "%21",
                "tempo22", "%22",
                "tempo23", "%23",
                "tempo24", "%24",
                "tempo25", "%25",
                "totale tempo", "% tempo fermata", "differenza"};
            
            foreach (var c in cols)
                {
                _table_report.Columns.Add(new DataColumn(c));
                }
            }

        private double _shiftMinutes = 0;
        private void CreateReport()
        {
            LoadingInfo.InfoText = "Loading report...";
            LoadingInfo.ShowLoading();

            CreateTableColumns();

            var table_data = new DataTable();

            var cmd = new SqlCommand("get_data_in_hold", MainWnd._sql_con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add("@from_date", SqlDbType.DateTime).Value = MainWnd.Get_from_date();
            cmd.Parameters.Add("@to_date", SqlDbType.DateTime).Value = MainWnd.Get_to_date();
            cmd.Parameters.Add("@shift", SqlDbType.VarChar).Value = MainWnd.Get_shift_array().ToString();

            MainWnd._sql_con.Open();
            var dr = cmd.ExecuteReader();
            table_data.Load(dr);
            MainWnd._sql_con.Close();
            dr.Close();

            var timeTot = 0;

            var separators = (from c in MainWnd.Get_shift_array().ToString().ToCharArray()
                                   where c == ','
                                   select c).ToArray().Length;
            //2 commas -> 1 shift selected
            //3 commas -> 2 shifts selected
            //4 commas -> 3 shifts selected
            //nr of shifts * 8hrs
            var totAvailableTime = (MainWnd.Get_from_date().Subtract(MainWnd.Get_to_date()).TotalDays + 1) * (separators - 1) * 8 * 3600; //in seconds

            _shiftMinutes = (480 * MainWnd.ListOfSelectedShifts.Count);

            var newRow = _table_report.NewRow();
            newRow[0] = "TOTAL";
            _table_report.Rows.Add(newRow);
            //sep row
            newRow = _table_report.NewRow();
            _table_report.Rows.Add(newRow);
            foreach (DataRow row in table_data.Rows)
            {
                newRow = _table_report.NewRow();
                timeTot = 0;

                newRow[0] = row[0].ToString();
                newRow[1] = ConvertSecondsToHHmm(Convert.ToInt32(row[1]));
                newRow[2] = Percentage(row[1]);
                newRow[3] = ConvertSecondsToHHmm(Convert.ToInt32(row[2]));
                newRow[4] = Percentage(row[2]);
                newRow[5] = ConvertSecondsToHHmm(Convert.ToInt32(row[3]));
                newRow[6] = Percentage(row[3]);
                newRow[7] = ConvertSecondsToHHmm(Convert.ToInt32(row[4]));
                newRow[8] = Percentage(row[4]);
                newRow[9] = ConvertSecondsToHHmm(Convert.ToInt32(row[5]));
                newRow[10] = Percentage(row[5]);
                newRow[11] = ConvertSecondsToHHmm(Convert.ToInt32(row[6]));
                newRow[12] = Percentage(row[6]);
                newRow[13] = ConvertSecondsToHHmm(Convert.ToInt32(row[7]));
                newRow[14] = Percentage(row[7]);

                newRow[15] = ConvertSecondsToHHmm(Convert.ToInt32(row[8]));
                newRow[16] = Percentage(row[8]);
                newRow[17] = ConvertSecondsToHHmm(Convert.ToInt32(row[9]));
                newRow[18] = Percentage(row[9]);
                newRow[19] = ConvertSecondsToHHmm(Convert.ToInt32(row[10]));
                newRow[20] = Percentage(row[10]);
                newRow[21] = ConvertSecondsToHHmm(Convert.ToInt32(row[11]));
                newRow[22] = Percentage(row[11]);
                newRow[23] = ConvertSecondsToHHmm(Convert.ToInt32(row[12]));
                newRow[24] = Percentage(row[12]);
                newRow[25] = ConvertSecondsToHHmm(Convert.ToInt32(row[13]));
                newRow[26] = Percentage(row[13]);
                newRow[27] = ConvertSecondsToHHmm(Convert.ToInt32(row[14]));
                newRow[28] = Percentage(row[14]);
                newRow[29] = ConvertSecondsToHHmm(Convert.ToInt32(row[15]));
                newRow[30] = Percentage(row[15]);
                newRow[31] = ConvertSecondsToHHmm(Convert.ToInt32(row[16]));
                newRow[32] = Percentage(row[16]);
                newRow[33] = ConvertSecondsToHHmm(Convert.ToInt32(row[17]));
                newRow[34] = Percentage(row[17]);
                newRow[35] = ConvertSecondsToHHmm(Convert.ToInt32(row[18]));
                newRow[36] = Percentage(row[18]);
                newRow[37] = ConvertSecondsToHHmm(Convert.ToInt32(row[19]));
                newRow[38] = Percentage(row[19]);
                newRow[39] = ConvertSecondsToHHmm(Convert.ToInt32(row[20]));
                newRow[40] = Percentage(row[20]);
                newRow[41] = ConvertSecondsToHHmm(Convert.ToInt32(row[21]));
                newRow[42] = Percentage(row[21]);
                newRow[43] = ConvertSecondsToHHmm(Convert.ToInt32(row[22]));
                newRow[44] = Percentage(row[22]);
                newRow[45] = ConvertSecondsToHHmm(Convert.ToInt32(row[23]));
                newRow[46] = Percentage(row[23]);
                newRow[47] = ConvertSecondsToHHmm(Convert.ToInt32(row[24]));
                newRow[48] = Percentage(row[24]);
                newRow[49] = ConvertSecondsToHHmm(Convert.ToInt32(row[25]));
                newRow[50] = Percentage(row[25]);

                for (var i = 1; i <= 25; i++)
                {
                    int.TryParse(row[i].ToString(), out int obj);
                    timeTot += obj;
                }

                newRow[51] = ConvertSecondsToHHmm(timeTot);
                newRow[52] = Percentage(timeTot);
                double.TryParse(row[26].ToString(), out var tessituraTime);
                var diff = totAvailableTime - tessituraTime;
                int.TryParse(diff.ToString(), out var differenza);
                newRow[53] = ConvertSecondsToHHmm(differenza);

                _table_report.Rows.Add(newRow);
            }

            dgvReport.DataSource = _table_report;
            dgvReport.Columns["differenza"].ToolTipText = "Nr of selected days * 24 - total tempo tessitura";

            CalculateTotals();

            // specialize filter column
            dgvReport.Columns[0].Width = 200;
            dgvReport.Columns[0].HeaderCell.Style.Font = new Font("Tahoma", 10, FontStyle.Regular);
            dgvReport.Columns[0].HeaderCell.Style.ForeColor = Color.MidnightBlue;
            dgvReport.Columns[0].HeaderCell.Style.BackColor = Color.White;
            dgvReport.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvReport.Columns[0].HeaderCell.Style.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
            string[] _columnHeaders = new string[] { "PETTINE", "MANUALE", "FILATO", "AGHI", "URTO", "PRINCIPALE", "ALTRO", "CHANGE STYLE",
                                                  "CHANGE COLOR", "MC BREAKDOWN",  "YARN DELAY", "YARN QUALITY", "TECHNIQUE", "MAINTENANCE",
                                                  "ORDER SHORT", "CAMBIO TAGLIA", "PREPRODUZIONE", "SVILUPOTAGLIE", "PROTOTIPO", "CAMPIONARIO",
                                                  "RIPARAZIONI", "CAMBIO ART.", "PULIZIA ORDINE", "PULIZIA FRONTURE", "TOTALI"};


            var position = 0;
            foreach (var header in _columnHeaders)
            {
                _columnHeaders[position] = header;
                position++;
            }
            position = 0;
            for (var h = 1; h <= dgvReport.ColumnCount - 4; h += 2)
            {
                dgvReport.Columns[h].HeaderText = _columnHeaders[position] + Environment.NewLine + Environment.NewLine + Environment.NewLine + "tempo";
                position++;
            }
            position = 0;
            for (var h = 2; h <= dgvReport.Columns.Count - 3; h += 2)
            {
                dgvReport.Columns[h].HeaderText = _columnHeaders[position] + Environment.NewLine + Environment.NewLine + Environment.NewLine + "%";
                position++;
                dgvReport.Columns[h].DefaultCellStyle.BackColor = Color.Silver;
            }
            dgvReport.Columns[53].HeaderText = Environment.NewLine + Environment.NewLine + Environment.NewLine + "   differenza";

            dgvReport.Columns[51].HeaderText = _columnHeaders[_columnHeaders.Length - 1] + Environment.NewLine + Environment.NewLine + Environment.NewLine + "totale tempo";
            dgvReport.Columns[52].HeaderText = _columnHeaders[_columnHeaders.Length - 1] + Environment.NewLine + Environment.NewLine + Environment.NewLine + "% tempo fermata";

            for (var c = 1; c <= dgvReport.Columns.Count - 1; c++)
            {
                dgvReport.Columns[c].Width = 80;
                dgvReport.Columns[c].HeaderCell.Style.Font = new Font("Tahoma", 8);
                dgvReport.Columns[c].HeaderCell.Style.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
                dgvReport.Columns[c].DefaultCellStyle.Alignment =
               DataGridViewContentAlignment.MiddleCenter;
            }

            dgvReport.Columns[51].DefaultCellStyle.BackColor = TotalColor;
            dgvReport.Columns[51].HeaderCell.Style.BackColor = TotalColor;
            dgvReport.Columns[52].DefaultCellStyle.BackColor = TotalColor;
            dgvReport.Columns[52].HeaderCell.Style.BackColor = TotalColor;
            dgvReport.Columns[53].DefaultCellStyle.BackColor = TotalColor;
            dgvReport.Columns[53].HeaderCell.Style.BackColor = TotalColor;
            dgvReport.Columns[0].Frozen = true;

            SetColumnVisibilityByTotalValue();

            dgvReport.DataBindingComplete += delegate
             {
                 dgvReport.Rows[1].DefaultCellStyle.SelectionBackColor = Color.White;
                 dgvReport.Rows[1].DefaultCellStyle.SelectionForeColor = Color.White;
                 dgvReport.Rows[1].Height = 10;
                 dgvReport.Rows[1].DefaultCellStyle.BackColor = Color.White;
                 for (var c = 0; c < dgvReport.Columns.Count; c++)
                 {
                     dgvReport.Rows[1].Cells[c].Style.BackColor = Color.White;
                     dgvReport.Rows[0].Cells[c].Style.BackColor = Color.FromArgb(180, 180, 180);
                 }
             };

            LoadingInfo.CloseLoading();
        }

        private void CalculateTotals()
        {
            var activeMachines = dgvReport.Rows.Count - 2;
            for(var c = 1; c < dgvReport.Columns.Count; c++)
            {
                var totalEff = 0.0;
                long totalPercEff = 0;
                for(var r = 2; r < dgvReport.Rows.Count; r++)
                {
                    if(c % 2 == 0)
                    {
                        double.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString(), out var eff);
                        totalEff += eff;
                    }
                    else
                    {
                        int.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString().Split(':')[0], out var hrs);
                        int.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString().Split(':')[1], out var mins);
                        TimeSpan ts = new TimeSpan(hrs, mins, 0);
                        totalPercEff += ts.Ticks;
                    }
                }
                if(c % 2 == 0 && c < dgvReport.Columns.Count - 1)
                {
                    dgvReport.Rows[0].Cells[c].Value = Math.Round(totalEff / activeMachines, 1).ToString();
                }
                else if(c % 2 == 1 && c < dgvReport.Columns.Count - 1)
                {
                    var t = TimeSpan.FromTicks(totalPercEff);
                    int.TryParse(t.TotalSeconds.ToString(), out var secs);
                    dgvReport.Rows[0].Cells[c].Value = ConvertSecondsToHHmm(secs);
                }
                else
                {
                    var t = TimeSpan.FromTicks(totalPercEff);
                    int.TryParse(t.TotalSeconds.ToString(), out var secs);
                    dgvReport.Rows[0].Cells[c].Value = ConvertSecondsToHHmm(secs / activeMachines);
                }
            }
        }

        private double Percentage(object n)
            {
            if (n == null || _shiftMinutes == 0) return 0;
                    
            return Math.Round(Convert.ToDouble(((Convert.ToDouble(n) / 60) * 100) / _shiftMinutes), 1);
            }

        private void SetColumnVisibilityByTotalValue()
            {
            //sets all columns to be visible
            for (var i = 0; i<= dgvReport.ColumnCount-1;i++)
                {
                dgvReport.Columns[i].Visible = true;
                }

            var sum = 0.0;

            for (var i = 2; i <= dgvReport.Columns.Count - 3; i += 2)
            //searching through eff columns as double type
                {
                sum = 0;
                for (var r = 2; r <= dgvReport.Rows.Count - 1; r++)
                    {
                    double.TryParse(dgvReport.Rows[r].Cells[i].Value.ToString(), out var eff);

                    sum += eff;
                    }

                if (sum <= 0)
                    {
                    dgvReport.Columns[i - 1].Visible = false;
                    dgvReport.Columns[i].Visible = false;
                    }
                }

            dgvReport.Refresh();
            }

        private void dgvReport_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
            {
            if (e.RowIndex == -1 && e.ColumnIndex > 0 && e.ColumnIndex < dgvReport.ColumnCount - 1)
                {
                Rectangle r2 = e.CellBounds;
                r2.Y += e.CellBounds.Height / 2;
                r2.Height = e.CellBounds.Height / 2;
                e.PaintBackground(r2, true);
                e.PaintContent(r2);
                e.Handled = true;
                }
            }

        private void dgvReport_Paint(object sender, PaintEventArgs e)
        {
            var c = 0;
            var headtxt = "";
            var headclr = DefaultBackColor;

            for (int j = 1; j < dgvReport.ColumnCount - 1;)
            {
                c++;

                Rectangle r1 = dgvReport.GetCellDisplayRectangle(j, -1, true);
                int w2 = dgvReport.GetCellDisplayRectangle(j, -1, true).Width;

                if (c == 1)
                {
                    headtxt = "PETTINE";
                }
                else if (c == 2)
                {
                    headtxt = "MANUALE";
                }
                else if (c == 3)
                {
                    headtxt = "FILATO";
                }
                else if (c == 4)
                {
                    headtxt = "AGHI";
                }
                else if (c == 5)
                {
                    headtxt = "URTO";
                }
                else if (c == 6)
                {
                    headtxt = "PRINCIPALE";
                }
                else if (c == 7)
                {
                    headtxt = "ALTRO";
                }
                else if (c == 8)
                {
                    headtxt = "CHANGE STYLE";
                }
                else if (c == 9)
                {
                    headtxt = "CHANGE COLOR";
                }
                else if (c == 10)
                {
                    headtxt = "MC BREAKDOWN";
                }
                else if (c == 11)
                {
                    headtxt = "YARN DELAY";
                }
                else if (c == 12)
                {
                    headtxt = "YARN QUALITY";
                }
                else if (c == 13)
                {
                    headtxt = "TECHNIQUE";
                }
                else if (c == 14)
                {
                    headtxt = "MAINTENANCE";
                }
                else if (c == 15)
                {
                    headtxt = "ORDER SHORT";
                }
                else if (c == 16)
                {
                    headtxt = "CAMBIO TAGLIA";
                }
                else if (c == 17)
                {
                    headtxt = "PREPRODUZIONE";
                }
                else if (c == 18)
                {
                    headtxt = "SVILUPOTAGLIE";
                }
                else if (c == 19)
                {
                    headtxt = "PROTOTIPO";
                }
                else if (c == 20)
                {
                    headtxt = "CAMPIONARIO";
                }
                else if (c == 21)
                {
                    headtxt = "RIPARAZIONI";
                }
                else if (c == 22)
                {
                    headtxt = "CAMBIO ART.";
                }
                else if (c == 23)
                {
                    headtxt = "PULIZIA ORDINE";
                }
                else if (c == 24)
                {
                    headtxt = "PULIZIA FRONTURE";
                }
                else if (c == 25)
                {
                    headtxt = "CAMBIO AGHI";
                }

                if (c <= 25)
                {
                    headclr = Color.OldLace;
                }
                else
                {
                    headtxt = "TOTALI";
                    headclr = TotalColor;
                }

                r1.X = r1.X - 1;

                if (c == 26)
                    r1.Width = (r1.Width + 2 * w2);
                else
                    r1.Width = (r1.Width + w2);

                r1.Height = r1.Height / 2;

                e.Graphics.FillRectangle(new SolidBrush(headclr), r1);
                e.Graphics.DrawRectangle(Pens.White, r1);

                StringFormat format = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                e.Graphics.DrawString(headtxt,
                    dgvReport.ColumnHeadersDefaultCellStyle.Font,
                    Brushes.Black,
                    r1,
                    format);

                    j += 2;
            }
        }

        private void dgvReport_Scroll(object sender, ScrollEventArgs e)
            {
            dgvReport.Invalidate();
            }

        private string ConvertSecondsToHHmm(int seconds)
            {
            int hours = seconds / 3600;
            int mins = (seconds % 3600) / 60;
            return string.Format(@"{0:D2}:{1:D2}", hours, mins);
            }

        private void button2_Click(object sender, EventArgs e)
            {         
            CreateReport();
            lblFrom.Text = MainWnd.Get_from_date().ToString("dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            lblTo.Text = MainWnd.Get_to_date().ToString("dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
        }

        private int _hideCol = -1;
        private void btnHide_Click(object sender, EventArgs e)
            {
            switch (_hideCol)
                {
                case -1:

                foreach (DataGridViewColumn c in dgvReport.Columns)
                    {
                    c.Visible = true;
                    }
                btnHide.Image = Properties.Resources.hide;
                _hideCol = 0;
                break;

                case 0:
                SetColumnVisibilityByTotalValue();
                btnHide.Image = Properties.Resources.unhide_30;
                _hideCol = -1;
                break;
                }
            }
        public void ExportToExcel()
            {
            var export = new ExcelExport();
            export.ExportToExcel(dgvReport, Text);
            }
        }
    }