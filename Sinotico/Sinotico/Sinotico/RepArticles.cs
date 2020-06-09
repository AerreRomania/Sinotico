using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Sinotico
    {
    public partial class RepArticles : Form
        {
        private DataSet _dataSet = new DataSet();
        private DataTable _dataTable = new DataTable();
        private string _fileName;

        public RepArticles()
            {
            InitializeComponent();
            //dgvReport.DataBindingComplete += dgvReport_DataBinding;
        }

        private DateTime Get_Date_From()
        {
            return new DateTime(dtpFrom.Value.Year, dtpFrom.Value.Month, dtpFrom.Value.Day);
        }
        private DateTime Get_Date_To()
        {
            return new DateTime(dtpTo.Value.Year, dtpTo.Value.Month, dtpTo.Value.Day);
        }

        protected override void OnLoad(EventArgs e)
            {
            dgvReport.DoubleBufferedDataGridView(true);            

            dgvReport.ShowCellToolTips = true;

            _fileName = string.Empty;

            try
                {
                LoadingInfo.InfoText = "Loading report...";
                LoadingInfo.ShowLoading();

                LoadData();
               
                LoadingInfo.CloseLoading();
                }
            catch
                {
                LoadingInfo.CloseLoading();
                }

            base.OnLoad(e);
            }

        private void LoadProcedure()
        {
            var cmd = new SqlCommand("[getarticlepartitionedqty]", MainWnd._sql_con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = Get_Date_From();
            cmd.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = Get_Date_To();
            cmd.Parameters.Add("@file", SqlDbType.VarChar).Value = _fileName;

            var da = new SqlDataAdapter(cmd);
            var ds = new DataSet();
            da.Fill(_dataSet);
            da.Dispose();
        }
        private void CreateTableView()
        {
            _dataTable.Columns.Add("N°", typeof(string));
            _dataTable.Columns.Add("Articolo");
            _dataTable.Columns.Add("Tot.");
            _dataTable.Columns.Add("Data In.");
            _dataTable.Columns.Add("Data Fi.");
            _dataTable.Columns.Add("Colore");
            _dataTable.Columns.Add("Taglia");

            foreach (DataRow row in _dataSet.Tables[0].Rows)
            {
                _dataTable.Columns.Add(row[0].ToString(), typeof(int));
            }

            _dataTable.Columns.Add("Capi App.");
            _dataTable.Columns.Add("Diff.");

            foreach (DataRow row in _dataSet.Tables[0].Rows)
            {
                _dataTable.Columns.Add(row[0].ToString() + "_n", typeof(int));
            }

            foreach (DataRow row in _dataSet.Tables[0].Rows)
            {
                _dataTable.Columns.Add(row[0].ToString() + "_%", typeof(int));
            }

            _dataTable.Columns.Add("sep1");
            _dataTable.Columns.Add("sep2");
            _dataTable.Columns.Add("sep3");
            _dataTable.Columns.Add("sep4");
        }

        private void LoadData()
        {
            _dataSet = new DataSet();
            _dataTable = new DataTable();
            //_fileName = string.Empty;
           
            LoadProcedure();
            CreateTableView();            

            var nr = 0;

            var htbl = new System.Collections.Hashtable();
            var idx = 0;

            var totRow = _dataTable.NewRow();
            totRow[0] = "TOTAL";
            _dataTable.Rows.Add(totRow);
            idx++;

            var lastArt = string.Empty;
            var lstOfMins = new List<int>();

            var tableMinimums = _dataSet.Tables[2];
            
            foreach (DataRow row in _dataSet.Tables[1].Rows)
            {
                var hKey = string.Empty;

                var fileName = row[0].ToString();
                var color = row[1].ToString();
                var size = row[2].ToString();
                var part = row[3].ToString();
                var dtStart = Convert.ToDateTime(row[5]).ToString("dd/MM/yyyy");
                var dtEnd = Convert.ToDateTime(row[6]).ToString("dd/MM/yyyy");
                int.TryParse(row["qtyx"].ToString(), out var qty);
                if (string.IsNullOrEmpty(qty.ToString())) qty = 0;
                var lstTag = GetFileTags(fileName);

                var min = 0;

                foreach (DataRow mRow in tableMinimums.Rows)
                {
                    var filex = mRow[0].ToString();
                    var colox = mRow[1].ToString();
                    var sizex = mRow[2].ToString();

                    var qtyx = Convert.ToInt32(mRow[3]);

                    if (filex == fileName && colox == color && sizex == size)
                    {
                        min = qtyx;
                    }
                }

                hKey = fileName + color + size;

                if (htbl.ContainsKey(hKey))
                {
                    var j = Convert.ToInt32(htbl[hKey]);
                    var total = 0;

                    total = Convert.ToInt32(_dataTable.Rows[j].ItemArray.GetValue(2)); //get last total

                    _dataTable.Rows[j][2] = total + qty; //update total
                    _dataTable.Rows[j][part] = qty;    //update part-cell

                    _dataTable.Rows[j]["Diff."] = (total + qty) - min;

                    _dataTable.Rows[j][part + "_n"] = qty - min;    //update part-cell
                }
                else
                {
                    var newRow = _dataTable.NewRow();

                    if (lastArt != fileName)
                    {
                        lstOfMins.Clear();
                        newRow[0] = string.Empty;
                        newRow[0] = fileName;
                        _dataTable.Rows.Add(newRow);
                        idx++;
                        nr = 0;

                        newRow = _dataTable.NewRow();

                        nr++;
                        newRow[0] = "";
                        //newRow[1] = fileName;
                        newRow[2] = qty; //tot
                        newRow[3] = dtStart;
                        newRow[4] = dtEnd;
                        newRow[5] = color;
                        newRow[6] = size;
                        newRow[part] = qty;
                        newRow[part + "_n"] = qty - min;
                        newRow["Capi App."] = min;

                        _dataTable.Rows.Add(newRow);
                        lstOfMins.Add(qty);
                        htbl.Add(hKey, idx);
                        idx++;
                    }
                    else
                    {
                        nr++;
                        newRow[0] = "";
                        //newRow[1] = fileName;
                        newRow[2] = qty; //tot
                        newRow[3] = dtStart;
                        newRow[4] = dtEnd;
                        newRow[5] = color;
                        newRow[6] = size;
                        newRow[part] = qty;
                        newRow[part + "_n"] = qty - min;
                        newRow["Capi App."] = min;

                        _dataTable.Rows.Add(newRow);
                        lstOfMins.Add(qty);
                        htbl.Add(hKey, idx);
                        idx++;
                    }

                    lastArt = fileName;
                }
            }
            if (dgvReport.DataSource != null)
                dgvReport.DataSource = null;

            dgvReport.DataSource = _dataTable;

            exclRows = new List<int>();
            exclRows.Add(0);
            foreach (DataGridViewRow row in dgvReport.Rows)
            {
                if (string.IsNullOrEmpty(row.Cells[0].Value.ToString()))
                    exclRows.Add(row.Index);
            }

            var fName = string.Empty;
            foreach (DataGridViewRow row in dgvReport.Rows)
            {
                if (row.Cells[0].Value.ToString() == "TOTAL") continue;
                if (!string.IsNullOrEmpty(row.Cells[0].Value.ToString()))
                {
                    fName = row.Cells[0].Value.ToString();
                }
                else
                {
                    row.Cells[0].Value = fName;
                }
            }

            var autoGenColumns = _dataSet.Tables[0].Rows.Count - 1;

            dgvReport.Columns[0].Width = 100;

            for (var i = 7; i <= 7 + autoGenColumns; i++)
            {
                dgvReport.Columns[i].Width = 40;
            }

            for (var i = 10 + autoGenColumns; i <= dgvReport.Columns.Count - 1; i++)
            {
                dgvReport.Columns[i].Width = 40;
            }

            dgvReport.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopLeft;
            dgvReport.Columns[1].Visible = false;

            dgvReport.Columns["sep1"].DisplayIndex = 3;
            dgvReport.Columns["sep2"].DisplayIndex = 9 + autoGenColumns;
            dgvReport.Columns["sep3"].DisplayIndex = 12 + autoGenColumns;
            dgvReport.Columns["sep4"].DisplayIndex = 14 + autoGenColumns * 2;

            for (var s = 1; s <= 4; s++)
            {
                dgvReport.Columns["sep" + s.ToString()].HeaderCell.Style.BackColor = Color.White;
                dgvReport.Columns["sep" + s.ToString()].HeaderText = "";
                dgvReport.Columns["sep" + s.ToString()].DefaultCellStyle.BackColor = Color.White;
                dgvReport.Columns["sep" + s.ToString()].DefaultCellStyle.SelectionBackColor = Color.White;
                dgvReport.Columns["sep" + s.ToString()].Width = 10;
            }

            dgvReport.Columns["Capi App."].DefaultCellStyle.BackColor = Color.Gainsboro;
            dgvReport.Columns["Diff."].DefaultCellStyle.BackColor = Color.Gainsboro;

            for (var i = 10 + autoGenColumns; i <= 10 + autoGenColumns * 2; i++)
            {
                dgvReport.Columns[i].HeaderText = dgvReport.Columns[i].HeaderText.Split('_')[0];
            }

            for (var i = 0; i <= 6; i++)
            {
                dgvReport.Columns[i].Frozen = true;
                dgvReport.Columns[i].DefaultCellStyle.BackColor = Color.FromArgb(225, 225, 225);
            }

            for (var i = 0; i <= dgvReport.Columns.Count - 1; i++)
            {
                dgvReport.Columns[i].DefaultCellStyle.Font =
                    new Font("Microsoft Sans Serif", 8);
            }

            for (var c = 7; c <= 9 + autoGenColumns; c++)
            {
                var tot = 0;

                foreach (DataGridViewRow r in dgvReport.Rows)
                {
                    int.TryParse(r.Cells[c].Value.ToString(), out var tVal);

                    tot += tVal;
                }

                dgvReport.Rows[0].Cells[c].Value = tot.ToString();
            }

            for (var c = 20; c < 31; c++)
            {
                var tot = 0;
                foreach (DataGridViewRow r in dgvReport.Rows)
                {
                    int.TryParse(r.Cells[c].Value.ToString(), out var tVal);
                    tot += tVal;
                }
                dgvReport.Rows[0].Cells[c].Value = tot.ToString();
            }
            var totAll = 0;
            var artTot = 0;
            var firstAppear = 0;
            foreach (DataGridViewRow roww in dgvReport.Rows)
            {
                if (string.IsNullOrEmpty(roww.Cells[2].Value.ToString()) && roww.Index > 0)
                {
                    dgvReport.Rows[firstAppear].Cells[2].Value = artTot;
                    firstAppear = roww.Index;
                    artTot = 0;
                }
                int.TryParse(roww.Cells[2].Value.ToString(), out var tVal);
                artTot += tVal;
                if (!string.IsNullOrEmpty(roww.Cells[3].Value.ToString()))
                    totAll += tVal;
            }
            dgvReport.Rows[firstAppear].Cells[2].Value = artTot;

            dgvReport.Rows[0].Cells[2].Value = totAll.ToString();

            var bound = dgvReport.Columns["Diff."].Index;
            foreach (DataGridViewColumn c in dgvReport.Columns)
            {
                c.SortMode = DataGridViewColumnSortMode.NotSortable;
                if (c.Index > bound && c.Name != "sep1" && c.Name != "sep2" && c.Name != "sep3") c.Visible = false;
            }

            CreateFilter();
            GetArticleCodification();

            dgvReport.DataBindingComplete += delegate
             {
                 dgvReport.Rows[0].Frozen = true;
                 dgvReport.Rows[0].DefaultCellStyle.BackColor = Color.White;
                 dgvReport.Rows[0].DefaultCellStyle.SelectionBackColor = Color.White;
                 dgvReport.Rows[0].DefaultCellStyle.ForeColor = Color.Crimson;
                 dgvReport.Rows[0].DefaultCellStyle.SelectionForeColor = Color.Crimson;
                 dgvReport.Rows[0].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);

                 foreach (DataGridViewRow row in dgvReport.Rows)
                 {
                     if (exclRows.Exists(x => x == row.Index)) continue;
                     else
                     {
                         row.DefaultCellStyle.BackColor = Color.White;
                         row.DefaultCellStyle.SelectionBackColor = Color.White;
                         row.DefaultCellStyle.ForeColor = Color.Black;
                         row.DefaultCellStyle.SelectionForeColor = Color.Black;
                         row.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);
                         row.Height = row.Height + 10;
                         row.Cells[2].Style.ForeColor = Color.Red;
                     }
                 }
             };
        }

        private List<int> exclRows = new List<int>();
        private void dgvReport_DataBinding(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvReport.Rows[0].Frozen = true;
            dgvReport.Rows[0].DefaultCellStyle.BackColor = Color.White;
            dgvReport.Rows[0].DefaultCellStyle.SelectionBackColor = Color.White;
            dgvReport.Rows[0].DefaultCellStyle.ForeColor = Color.Crimson;
            dgvReport.Rows[0].DefaultCellStyle.SelectionForeColor = Color.Crimson;
            dgvReport.Rows[0].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);
            
            foreach (DataGridViewRow row in dgvReport.Rows)
            {
                if (exclRows.Exists(x=>x == row.Index)) continue;
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                    row.DefaultCellStyle.SelectionBackColor = Color.White;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                    row.DefaultCellStyle.SelectionForeColor = Color.Black;
                    row.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);
                    row.Height = row.Height + 10;
                    row.Cells[2].Style.ForeColor = Color.Red;
                }
            }
        }

        private List<string> GetFileTags(string art)
            {
            var str = "";

            var q = "select comb from artcomb where article='" + art + "'";
            var con = new SqlConnection(MainWnd.conString);
            var cmd = new SqlCommand(q, con);
            con.Open();
            var dr = cmd.ExecuteReader();
            if (dr.HasRows)
                {
                while (dr.Read())
                    {
                    str = dr[0].ToString();
                    }
                }
            con.Close();
            dr.Close();

            var tagsList = str.Split(',').ToList();

            return tagsList;
            }

        private void button2_Click(object sender, EventArgs e)
            {
            try
                {
                LoadingInfo.InfoText = "Loading report...";
                LoadingInfo.ShowLoading();

                LoadData();

                LoadingInfo.CloseLoading();
                }
            catch
                {
                LoadingInfo.CloseLoading();
                }
            }

        public void ExportToExcel()
            {
            var export = new ExcelExport();
            export.ExportToExcel(dgvReport, Text);
            }

        private void dgvReport_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
            {
            //if (e.RowIndex > -1 && dgvReport.Rows[e.RowIndex].Cells[0].Value.ToString() == string.Empty)
            //    {
            //    _rect = new Rectangle(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width, e.CellBounds.Height);

            //    e.Graphics.FillRectangle(Brushes.WhiteSmoke, _rect);

            //    var form = new StringFormat();
            //    form.Alignment = StringAlignment.Center;
            //    form.FormatFlags = StringFormatFlags.NoClip;

            //    e.Graphics.DrawString(dgvReport.Rows[e.RowIndex].Cells[2].Value.ToString(), 
            //        new Font("Segoe UI", 11, FontStyle.Bold), 
            //        Brushes.Black,       
            //        e.ClipBounds.Width / 2 - 300, 
            //        e.CellBounds.Y + 9, form);

            //    e.Handled = true;
            //    }
            }

        private void dgvReport_Scroll(object sender, ScrollEventArgs e)
            {
            //dgvReport.Invalidate();
            //dgvReport.Invalidate(_rect);
            }

        private ComboBox _cbArt;
        private void CreateFilter()
            {
            _cbArt?.Dispose();

            _cbArt = new ComboBox
                {
                //Name = dgvReport.Columns[0].Name,
                BackColor = Color.Gold,
                Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular),
                Parent = dgvReport,
                //Width = 30,
                DropDownWidth=140,
                FlatStyle = FlatStyle.Flat,
                };

            foreach (KeyValuePair<string, string> kvp in MainWnd._fileNamesDict)
                {
                _cbArt.Items.Add(kvp.Value);
                }

            dgvReport.Controls.Add(_cbArt);

            var headerRect = dgvReport.GetColumnDisplayRectangle(0, true);
            _cbArt.Location = new Point(headerRect.Location.X, _cbArt.Height - 4);
            _cbArt.Size = new Size(headerRect.Width - 1, dgvReport.ColumnHeadersHeight);

            _cbArt.SelectedIndexChanged += (s, e) =>
            {
                if (_cbArt.SelectedIndex <= 0)
                    {
                    _fileName =string.Empty;
                    }
                else
                    {
                    _fileName = _cbArt.Text;
                    }
            };
            }

        private List<ArticlePartsCodification> _lstCodif = new List<ArticlePartsCodification>();
        private void GetArticleCodification()
            {
            _lstCodif = new List<ArticlePartsCodification>();

            var q = "select * from combarchive";
            using (var c = new SqlConnection(MainWnd.conString))
                {
                var cmd = new SqlCommand(q, c);
                c.Open();
                var dr = cmd.ExecuteReader();
                if (dr.HasRows)
                    {
                    while (dr.Read())
                        {
                        _lstCodif.Add(new ArticlePartsCodification(dr[1].ToString(), dr[2].ToString(), dr[3].ToString()));
                        }
                    }

                c.Close();
                dr.Close();
                } 
            }
        
        private void dgvReport_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
            {           
            if (e.RowIndex == -1 && e.ColumnIndex != -1)
                {
                var hText = dgvReport.Columns[e.ColumnIndex].HeaderText;

                var desc = from codif in _lstCodif
                           where codif.Part == hText.Trim()
                           select codif;

                var lstDesc = desc.ToList();
                
                if (lstDesc.Count <= 0) return;

                var text = lstDesc.First();

                dgvReport.Columns[e.ColumnIndex].ToolTipText = "Descrizione: " + 
                    text.Description + 
                  "\n" + "Descrizione estesa: " + 
                  text.ExtendedDesc;
                }
            }
        }

    public class ArticlePartsCodification
        {
        public string Part { get; set; }

        public string Description { get; set; }

        public string ExtendedDesc { get; set; }

        public ArticlePartsCodification(string part, string desc, string extendedDesc)
            {
            Part = part;
            Description = desc;
            ExtendedDesc = extendedDesc;
            }
        }
    }
