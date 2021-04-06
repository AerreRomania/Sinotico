using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Sinotico
    {
    public partial class RepMonthChanges : Form
        {
        public RepMonthChanges()
            {
            InitializeComponent();
            }

        private string _year;
        private DataTable _dataTable;
        private DataTable _tblChanges;
        private DataSet _dataSet;
        private int _mode = 0;

        private void RepMonthChanges_Load(object sender, EventArgs e)
            {
            dgvReport.DoubleBufferedDataGridView(true);
            AddYearCombo();
            cboYear.SelectedIndexChanged += cboYear_ChangeIndex;

            dgvReport.DataBindingComplete += dgvReport_DataBindComplete;

            _mode = 1;

            CreateTableStructure();
            }

        private void dgvReport_DataBindComplete(object sender, DataGridViewBindingCompleteEventArgs e)
            {
            }

        private void AddYearCombo()
            {
            var curYear = DateTime.Now.Year;
            var curYearBefore = curYear - 3;

            for (var i = curYearBefore; i <= curYear; i++)
                {
                cboYear.Items.Add(i);
                }

            var tmpStr = curYear.ToString();
            cboYear.SelectedIndex = cboYear.FindString(tmpStr);

            _year = tmpStr;
            lblFrom.Text = _year;
            lblTo.Text = "31.12" + "." + _year;

            //CreateTableStructure();

            dgvReport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvReport.ColumnHeadersHeight = dgvReport.ColumnHeadersHeight * 2;
            dgvReport.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
            dgvReport.CellPainting += new DataGridViewCellPaintingEventHandler(dataGridView1_CellPainting);
            dgvReport.Paint += new PaintEventHandler(dataGridView1_Paint);
            dgvReport.Scroll += new ScrollEventHandler(dataGridView1_Scroll);
            dgvReport.ColumnWidthChanged += new DataGridViewColumnEventHandler(dataGridView1_ColumnWidthChanged);
            }

        private void cboYear_ChangeIndex(object sender, EventArgs e)
            {
            _year = cboYear.Text;

            lblFrom.Text = _year;
            lblTo.Text = "31.12" + "." + _year;
            }

        private void LoadChanges(string txt)
            {
            _tblChanges = new DataTable();

            var cmd = new SqlCommand(txt, MainWnd._sql_con)
                {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout=1000
                };

            cmd.Parameters.Add("@year", SqlDbType.Int).Value = Convert.ToInt32(_year);
            cmd.Parameters.Add("@table", SqlDbType.VarChar).Value = MainWnd.GetTableSource();
            
            MainWnd._sql_con.Open();
            var dr = cmd.ExecuteReader();
            _tblChanges.Load(dr);
            MainWnd._sql_con.Close();
            dr.Close();
            cmd = null;
            }

        private void CreateTableStructure()
            {
            try
                {
                LoadingInfo.ShowLoading();
                LoadingInfo.InfoText = "Loading data for " + _year + ".\n     Please wait...";

                _dataSet = new DataSet();
                _dataSet.Tables.Clear();
                _dataTable = new DataTable();

                if (dgvReport.DataSource != null) dgvReport.DataSource = null;

                _dataTable.Columns.Add("Somma");
                _dataTable.Columns.Add("splitter_0");

                for (var i = 1; i <= 12; i++)
                    {
                    _dataTable.Columns.Add("uno_" + i.ToString());
                    _dataTable.Columns.Add("due_" + i.ToString());
                    _dataTable.Columns.Add("tre_" + i.ToString());
                    _dataTable.Columns.Add("tot_" + i.ToString());
                    _dataTable.Columns.Add("splitter_" + i.ToString());
                }

                for (var i = 1; i <= 31; i++)
                    {
                    var newRow = _dataTable.NewRow();
                    newRow[0] = i.ToString();

                    _dataTable.Rows.Add(newRow);
                    }

                _dataTable.Columns.Add("uno_t");
                _dataTable.Columns.Add("due_t");
                _dataTable.Columns.Add("tre_t");
                _dataTable.Columns.Add("tot_t");

                var cmdTxt = "";

                if (_mode == 1)
                    {
                    cmdTxt = "getarticleschanges";
                    }
                else if (_mode == 3)
                    {
                    cmdTxt = "getsizechanges";
                    }
                else if (_mode == 2)
                    {
                    cmdTxt = "getcolorchanges";
                    }

                LoadChanges(cmdTxt);

                LoadingInfo.UpdateText("Almost done...");
                
                for (var r = 0; r <= _tblChanges.Rows.Count - 1; r++)
                    {
                    var row = _tblChanges.Rows[r];
                   
                    DateTime.TryParse(row[1].ToString(), out var rDate);
                    var day = rDate.Day.ToString();
                    var month = rDate.Month.ToString();
                    var changes = row[2].ToString();

                    int.TryParse(row[0].ToString(), out var sqrTag);    //squadra number
                    var squadra = GetSquadra(sqrTag);
                    var findField = squadra + "_" + month.ToString();

                    for (var j = 0; j<= _dataTable.Rows.Count- 1; j++)
                        {
                        var jRow = _dataTable.Rows[j];
                        if (jRow[0].ToString() == day)
                            {
                            int.TryParse(jRow[findField].ToString(), out var jChange);

                            jRow[findField] = jChange + Convert.ToInt32(changes);
                            }
                        }
                    }

                dgvReport.DataSource = _dataTable;

                dgvReport.Columns[0].Width = 45;
                dgvReport.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvReport.Columns[0].HeaderCell.Style.BackColor = Color.FromArgb(242, 242, 242);
                dgvReport.Columns[0].HeaderCell.Style.ForeColor = Color.Black;
                dgvReport.Columns[0].DefaultCellStyle.BackColor = Color.FromArgb(242, 242, 242);
                dgvReport.Columns[0].HeaderText = "Somma";
                dgvReport.Columns[0].Frozen = true;
                dgvReport.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgvReport.Columns[1].Width = 5;
                dgvReport.Columns[1].HeaderText = string.Empty;

                for (var i = 2; i <= dgvReport.Columns.Count - 1; i++)
                    {
                    if(i % 5 == 1)
                    {
                        dgvReport.Columns[i].Width = 5;
                        dgvReport.Columns[i].HeaderText = string.Empty;
                        continue;
                    }
                    dgvReport.Columns[i].HeaderCell.Style.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Regular);
                    dgvReport.Columns[i].Width = 35;

                    dgvReport.Columns[i].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 7, FontStyle.Regular);
                    dgvReport.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                for (var i = 2; i <= 63; i += 5)
                    {
                    dgvReport.Columns[i - 1].DefaultCellStyle.BackColor = Color.White;
                    dgvReport.Columns[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 231);
                    dgvReport.Columns[i + 1].DefaultCellStyle.BackColor = Color.FromArgb(236, 245, 231);
                    dgvReport.Columns[i + 2].DefaultCellStyle.BackColor = Color.FromArgb(252, 233, 220);
                    dgvReport.Columns[i + 3].DefaultCellStyle.BackColor = Color.FromArgb(242,242,242);

                    dgvReport.Columns[i].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 231);
                    dgvReport.Columns[i + 1].HeaderCell.Style.BackColor = Color.FromArgb(236, 245, 231);
                    dgvReport.Columns[i + 2].HeaderCell.Style.BackColor = Color.FromArgb(252, 233, 220);
                    dgvReport.Columns[i + 3].HeaderCell.Style.BackColor = Color.FromArgb(242, 242, 242);
                    }

                for (var i = 2; i <= dgvReport.Columns.Count - 1; i++)
                    {
                    if (i % 5 == 1) continue;
                    dgvReport.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;
                    dgvReport.Columns[i].HeaderText = "\n\n" + dgvReport.Columns[i].HeaderText.Split('_')[0] + "\n";
                    }

                CalculateTotalHorizontal();
                CalculateTotalVertical();

                foreach (DataGridViewColumn c in dgvReport.Columns)
                    {
                    c.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                
                if (_mode == 1)
                {
                    lbl_articolo_total.Text = "Total: " + _yearTotal;
                    lbl_colore_total.Text = string.Empty;
                    lbl_taglia_total.Text = string.Empty;
                }
                else if (_mode == 2)
                {
                    lbl_articolo_total.Text = string.Empty;
                    lbl_colore_total.Text = "Total: " + _yearTotal;
                    lbl_taglia_total.Text = string.Empty;
                }
                else if (_mode == 3)
                {
                    lbl_articolo_total.Text = string.Empty;
                    lbl_colore_total.Text = string.Empty;
                    lbl_taglia_total.Text = "Total: " + _yearTotal;
                }

                LoadingInfo.CloseLoading();
                }
            catch (Exception ex)
                {
                LoadingInfo.CloseLoading();
                MessageBox.Show(ex.Message);
                }
            }
        
        private string GetSquadra(int sq)
            {
            var squadra = "";
            
            switch (sq)
                {
                case 1: squadra = "uno";
                break;
                case 2: squadra = "due";
                break;
                case 3: squadra = "tre";
                break;
                }

            return squadra;
            }

        private void btnReload_Click(object sender, EventArgs e)
            {
            CreateTableStructure();
            }

        private void CalculateTotalHorizontal()
            {
            for (var r = 0; r<= dgvReport.Rows.Count - 1; r++)
                {
                var u = 0;
                var d = 0;
                var t = 0;
                var tot = 0;
                var tripCount = 0;
                var lastTot = 0;

                for (var c = 2; c <= dgvReport.Columns.Count - 1; c++)
                    {
                    if (c % 5 == 1) continue;

                    if (tripCount < 3)
                        {
                        int.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString(), out var totVal);
                        tot += totVal;
                        }
                    
                    if (dgvReport.Columns[c].HeaderText == "\n\n"+"uno"+"\n")
                        {
                        int.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString(), out var cUno);
                        u += cUno;
                        }
                    else if (dgvReport.Columns[c].HeaderText == "\n\n"+"due"+"\n")
                        {
                        int.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString(), out var cDuo);
                        d += cDuo;
                        }
                    else if (dgvReport.Columns[c].HeaderText == "\n\n"+"tre"+"\n")
                        {
                        int.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString(), out var cTre);
                        t += cTre;
                        }
                    else if (dgvReport.Columns[c].HeaderText == "\n\n" +"tot"+"\n")
                        {
                        dgvReport.Rows[r].Cells[c].Value = tot.ToString();
                        lastTot += tot;
                        tot = 0;
                        tripCount = 0;
                        }
                    }
                
                dgvReport.Rows[r].Cells["uno_t"].Value = u.ToString();
                dgvReport.Rows[r].Cells["due_t"].Value = d.ToString();
                dgvReport.Rows[r].Cells["tre_t"].Value = t.ToString();
                dgvReport.Rows[r].Cells[dgvReport.Columns.Count - 1].Value = lastTot.ToString();
                }                 
            }

        private string _yearTotal = string.Empty;
        private void CalculateTotalVertical()
        {
            var hdTxt = "";
            for (var c = 1; c <= dgvReport.Columns.Count - 1; c++)
            {
                if (c % 5 == 1) continue;

                hdTxt = dgvReport.Columns[c].HeaderText;
                var sum = 0;

                for (var r = 0; r <= dgvReport.Rows.Count - 1; r++)
                {
                    int.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString(), out var changes);
                    sum += changes;
                }

                dgvReport.Columns[c].HeaderText = sum.ToString() + hdTxt;
                _yearTotal = sum.ToString();
            }
        }

        #region DataGridViewMergedHeaders

        private Rectangle _mergRect = new Rectangle();

        void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
            {
            dgvReport.Invalidate(_mergRect);
            dgvReport.Invalidate();
            }

        void dataGridView1_Scroll(object sender, ScrollEventArgs e)
            {
            if (e.NewValue > e.OldValue)
                {
                dgvReport.Invalidate(_mergRect);
                }
            else
                {
                dgvReport.Invalidate();
                dgvReport.Invalidate(_mergRect);
                }
            }

        void dataGridView1_Paint(object sender, PaintEventArgs e)
        {
            string[] monthes = { "Gennaio", "Febbraio", "Marzo", "Aprile", "Maggio", "Giugno",
                "Luglio", "Agosto", "Settembre", "Ottobre", "Novembre", "Dicembre", "Totale"};

            var i = 0;
            for (int j = 2; j < 63;)
            {
                _mergRect = this.dgvReport.GetCellDisplayRectangle(j, -1, true);
                int w2 = this.dgvReport.GetCellDisplayRectangle(j, -1, true).Width;

                _mergRect.X += -1;
                _mergRect.Y += 0;

                _mergRect.Width = _mergRect.Width + 105;
                _mergRect.Height = _mergRect.Height / 2 - 10;

                e.Graphics.FillRectangle(new SolidBrush(Color.Gainsboro), _mergRect);
                e.Graphics.DrawRectangle(Pens.Black, _mergRect);

                StringFormat format = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                e.Graphics.DrawString(monthes[i],
                    new Font("Microsoft Sans Serif", 8, FontStyle.Regular),
                    new SolidBrush(Color.Black),
                    _mergRect,
                    format);
                i++;
                j += 5;
            }
        }

        void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                Rectangle r2 = e.CellBounds;
                r2.Y += e.CellBounds.Height / 2 - 5;
                r2.Height = e.CellBounds.Height / 2 - 5;

                e.PaintBackground(r2, true);
                e.PaintContent(r2);
                e.Handled = true;
            }
            if (e.RowIndex == -1 && e.ColumnIndex > 1 && e.ColumnIndex <= dgvReport.Columns.Count - 1 && e.ColumnIndex % 5 != 1)
            {
                if (dgvReport.Columns[e.ColumnIndex].HeaderText.Contains("tot"))
                {
                    var totRect = new Rectangle(e.CellBounds.X - 1, e.CellBounds.Height - 30,
                   e.CellBounds.Width, e.CellBounds.Height - 10);

                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(242, 242, 242)), totRect);
                    e.Graphics.DrawRectangle(Pens.Black, totRect);
                    e.Graphics.DrawLine(Pens.Black, e.CellBounds.X, e.CellBounds.Height - 1,
                        e.CellBounds.Width, e.CellBounds.Height - 1);

                    StringFormat format = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };

                    var msStr = e.Graphics.MeasureString("tot", new Font("Microsoft Sans Serif", 8, FontStyle.Regular));
                    var msW = msStr.Width;

                    e.Graphics.DrawString("tot",
                        new Font("Microsoft Sans Serif", 8, FontStyle.Regular),
                        new SolidBrush(Color.Black), (e.CellBounds.X) + e.CellBounds.Width / 2 - msW / 2, e.CellBounds.Height - 28);
                }
                e.Handled = true;
            }
            if (e.RowIndex == -1 && e.ColumnIndex == 0)
            {
                Rectangle r1 = e.CellBounds;
                r1.Height = e.CellBounds.Height / 2 - 8;
                r1.Width = e.CellBounds.Width - 3;
                r1.X = e.CellBounds.X + 1;
                r1.Y = e.CellBounds.Y + 1;

                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(242, 242, 242)), r1);
                var lstRct = new Rectangle(e.CellBounds.X + 1, r1.Y + r1.Height + 44, e.CellBounds.Width + 10, 12);
                e.Graphics.FillRectangle(Brushes.White, lstRct);
                var rctUnderTot = new Rectangle(e.CellBounds.X + 1, e.CellBounds.Height - 40,
                 e.CellBounds.Width, 10);
                e.Graphics.FillRectangle(Brushes.White, rctUnderTot);
                e.Handled = true;
            }
            if (e.ColumnIndex % 5 == 1)
            {
                var rect = e.CellBounds;
                rect.Width = rect.Width - 1;
                rect.Y = rect.Y - 1;
                rect.Height = rect.Height + 1;
                e.Graphics.FillRectangle(Brushes.White, rect);
                e.Handled = true;
            }
            if (e.RowIndex == -1 && e.ColumnIndex > 0)
            {
                var rect = new Rectangle(e.CellBounds.X, e.CellBounds.Height / 2 - 10, e.CellBounds.Width, 5);
                e.Graphics.FillRectangle(Brushes.White, rect);
                var rctUnderTot = new Rectangle(e.CellBounds.X, e.CellBounds.Height - 40,
                 e.CellBounds.Width, 10);
                e.Graphics.FillRectangle(Brushes.White, rctUnderTot);
                if (e.ColumnIndex > 1)
                {
                    var lstRct = new Rectangle(e.CellBounds.X - 1, rect.Y + rect.Height + 42, e.CellBounds.Width + 10, 12);
                    e.Graphics.FillRectangle(Brushes.White, lstRct);
                }
                if (e.ColumnIndex % 5 != 1)
                {
                    e.Graphics.DrawLine(Pens.Black, new Point(e.CellBounds.X, rect.Y + rect.Height),
                                        new Point(e.CellBounds.X + e.CellBounds.Width, rect.Y + rect.Height));
                    e.Graphics.DrawLine(Pens.Black, new Point(e.CellBounds.X, rect.Y + rect.Height + 14),
                                        new Point(e.CellBounds.X + e.CellBounds.Width, rect.Y + rect.Height + 14));
                    e.Graphics.DrawLine(Pens.Black, new Point(e.CellBounds.X, rect.Y + rect.Height + 25),
                                        new Point(e.CellBounds.X + e.CellBounds.Width, rect.Y + rect.Height + 25));
                    e.Graphics.DrawLine(Pens.Black, new Point(e.CellBounds.X, rect.Y + rect.Height + 41),
                                        new Point(e.CellBounds.X + e.CellBounds.Width, rect.Y + rect.Height + 41));
                }
                e.Handled = true;
            }
        }
        #endregion

        public void ExportToExcel()
            {
            var export = new ExcelExport();
            export.ExportToExcel(dgvReport, Text);
            }

        private void btnLineTot_Click(object sender, EventArgs e)
            {
            _mode = 1;
            CreateTableStructure();
            }

        private void btnSquadraTot_Click(object sender, EventArgs e)
            {
            _mode = 2;
            CreateTableStructure();
            }

        private void button1_Click(object sender, EventArgs e)
            {
            _mode = 3;
            CreateTableStructure();
            }
        }
    }
