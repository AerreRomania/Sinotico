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
using Sinotico.DatabaseTableClasses;

namespace Sinotico
{
    public partial class RepGiornaliera : Form
    {
        private List<Component> _components = new List<Component>();
        public RepGiornaliera()
        {
            InitializeComponent();
            btnGo.Click += btnGo_Click;
            dgvReport.DataBindingComplete += dgvReport_BindingComplete;
            dgvReport.CellPainting += dgvReport_CellPainting;
            dgvReport.Paint += dgvReport_Paint;
            dgvReport.Scroll += dgvReport_Scroll;
            CustomizeDataGridView(dgvReport);
        }        
        protected override void OnLoad(EventArgs e)
        {
            ProcessData();
            base.OnLoad(e);
        }
        private void CreateTableView(DataTable dt)
        {
            dt.Columns.Add("days");
            dt.Columns.Add("fin3");
            dt.Columns.Add("fin5");
            dt.Columns.Add("fin7");
            dt.Columns.Add("fin14");
            dt.Columns.Add("active_machines");
            dt.Columns.Add("euro");
            dt.Columns.Add("euro_total");
            dt.Columns.Add("eff_sq_1");
            dt.Columns.Add("eff_sq_2");
            dt.Columns.Add("eff_sq_3");
            dt.Columns.Add("eff_total");
            dt.Columns.Add("eff_total_netta");
            dt.Columns.Add("teli_rammendo_sq_1");
            dt.Columns.Add("teli_rammendo_sq_2");
            dt.Columns.Add("teli_rammendo_sq_3");
            dt.Columns.Add("teli_rammendo_sq_total");
            dt.Columns.Add("teli_rammendo_mid");
            dt.Columns.Add("eff_rammendo_sq_1");
            dt.Columns.Add("eff_rammendo_sq_2");
            dt.Columns.Add("eff_rammendo_sq_3");
            dt.Columns.Add("eff_rammendo_sq_total");
            dt.Columns.Add("teli_scarti_sq_1");
            dt.Columns.Add("teli_scarti_sq_2");
            dt.Columns.Add("teli_scarti_sq_3");
            dt.Columns.Add("teli_scarti_sq_total");
            dt.Columns.Add("eff_scarti_sq_1");
            dt.Columns.Add("eff_scarti_sq_2");
            dt.Columns.Add("eff_scarti_sq_3");
            dt.Columns.Add("eff_scarti_teli_total");
            dt.Columns.Add("eff_scarti_tempo_total");
        } 
        private void CustomizeDataGridView(DataGridView dgv)
        {
            dgvReport.DoubleBufferedDataGridView(true);
            dgv.DataBindingComplete += delegate
            {
                 dgv.ReadOnly = true;
                 dgv.AllowUserToAddRows = false;
                 dgv.AllowUserToDeleteRows = false;
                 dgv.AllowUserToOrderColumns = false;
                 dgv.AllowUserToResizeColumns = false;
                 dgv.AllowUserToResizeRows = false;
                 dgv.RowHeadersVisible = false;
                 
                 for(var r = 0; r < 2; r++)
                     for(var c = 1; c < dgv.Columns.Count; c++)
                     {
                         if(c == 8 || c == 13 || c == 18 || c == 22)
                         {
                             dgv.Rows[r].Cells[c].Style.BackColor = Color.FromArgb(0, 255, 0);
                         }
                         else if(c == 9 || c == 14 || c == 19 || c == 23)
                         {
                             dgv.Rows[r].Cells[c].Style.BackColor = Color.FromArgb(255, 204, 0);
                         }
                         else if(c == 10 || c == 15 || c == 20 || c == 24)
                         {
                             dgv.Rows[r].Cells[c].Style.BackColor = Color.FromArgb(153, 204, 255);
                         }
                         else
                         {
                             dgv.Rows[r].Cells[c].Style.BackColor = Color.FromArgb(255, 255, 153);
                         }
                     }
                 
                 var str = new string[] { "Squadra(all)", "03", "05", "07", "14", "nr.", "Euro", string.Empty, "Squadra 1", "Squadra 2",
                                          "Squadra 3", "TOTALE", "TOTALE", "Squadra 1", "Squadra 2", "Squadra 3", "TOTALE", string.Empty,
                                          "Squadra 1", "Squadra 2", "Squadra 3", "TOTALE", "Squadra 1", "Squadra 2", "Squadra 3", "TOTALE",
                                          "Squadra 1", "Squadra 2", "Squadra 3", "% Totale Teli", "% Totale Tempo"};
                 var strPosition = 0;
                 foreach (DataGridViewColumn col in dgv.Columns)
                 {
                     col.SortMode = DataGridViewColumnSortMode.NotSortable;
                     col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                     if(col.Index == 1 || col.Index == 2 || col.Index == 3 || col.Index == 4)
                     {
                         col.HeaderCell.Style.Font = new Font("Tahoma", 8, FontStyle.Bold);
                     }
                     else
                     {
                         col.HeaderCell.Style.Font = new Font("Tahoma", 8, FontStyle.Regular);
                     }
                     
                     col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                     col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;
                     col.HeaderText = "\n\n\n\n\n\n" + str[strPosition];
                     strPosition++;

                    if (col.Index == 0)
                    {
                        col.Width = 100;
                    }
                    else if (col.Index >= 8 && col.Index <= 10 ||
                             col.Index >= 13 && col.Index <= 15 ||
                             col.Index >= 18 && col.Index <= 20 ||
                             col.Index >= 22 && col.Index <= 24 ||
                             col.Index >= 26 && col.Index <= 28)
                    {
                        col.Width = 70;
                    }
                     else
                    {
                        col.Width = 50;
                    }
                }
                
                 for (var r = 0; r < dgv.Rows.Count; r++)
                     for (var c = 0; c < dgv.Columns.Count; c++)
                     {
                         if (r == 0 && c == 0 || r == 1 && c == 0)
                         {
                             dgv.Rows[r].Cells[c].Style.ForeColor = Color.FromArgb(51, 51, 153);
                             if(r == 0)
                                dgv.Rows[r].Cells[c].Style.Font = new Font("Tahoma", 8, FontStyle.Regular);
                             else
                                 dgv.Rows[r].Cells[c].Style.Font = new Font("Tahoma", 8, FontStyle.Bold);
                         }
                         else if ((c == 11 || c == 12 || c == 16 || c == 17 || c == 21 ||
                                   c == 25 || c == 29 || c == 30) && r > 1)
                         {
                             dgv.Rows[r].Cells[c].Style.ForeColor = Color.Red;
                             dgv.Rows[r].Cells[c].Style.Font = new Font("Tahoma", 8, FontStyle.Regular);
                         }
                         else if(r == 0 || r == 1)
                        {
                            dgvReport.Rows[r].Height = 25;
                            dgvReport.Rows[r].Cells[c].Style.ForeColor = Color.Black;
                            dgvReport.Rows[r].Cells[c].Style.Font = new Font("Tahoma", 9, FontStyle.Bold);
                        }
                         else
                         {
                             dgv.Rows[r].Cells[c].Style.ForeColor = Color.Black;
                             dgv.Rows[r].Cells[c].Style.Font = new Font("Tahoma", 8, FontStyle.Regular);
                         }
                         
                     }
                dgv.Columns[2].Visible = false;
                dgv.Columns[6].Visible = false;
                dgv.Columns[7].Visible = false;
            };
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
            int w = 0;
            float x = 0.0F;
            float y = 0.0F;
            StringFormat format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
            }; 
            //main titles
            for (var i = 1; i < dgvReport.Columns.Count; i ++)
            {
                Rectangle rect = dgvReport.GetCellDisplayRectangle(i, -1, true);
                w += rect.Width;
                if(i == 1 || i == 13 || i == 22)
                {
                    x = rect.X;
                    y = rect.Y;
                }
                if (i == 12)
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;                    
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(183, 222, 232)),
                                             new RectangleF(x, y, w, rect.Height / 3));

                    int.TryParse(x.ToString(), out var ix);
                    int.TryParse(y.ToString(), out var iy);
                    e.Graphics.DrawRectangle(Pens.Black, new Rectangle(ix, iy, w - 1, rect.Height / 3));

                    e.Graphics.DrawString("EFFICIENZE",
                                          new Font("Tahoma", 12, FontStyle.Bold),
                                          new SolidBrush(dgvReport.ColumnHeadersDefaultCellStyle.ForeColor),
                                          new RectangleF(x, y, w, rect.Height / 3),
                                          format);
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    x = 0.0F;
                    y = 0.0F;
                    w = 0;
                }
                else if(i == 21)
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(217, 217, 217)),
                                             new RectangleF(x, y, w, rect.Height / 3));

                    int.TryParse(x.ToString(), out var ix);
                    int.TryParse(y.ToString(), out var iy);
                    e.Graphics.DrawRectangle(Pens.Black, new Rectangle(ix, iy, w - 1, rect.Height / 3));

                    
                    e.Graphics.DrawString("RAMMENDO",
                                          new Font("Tahoma", 12, FontStyle.Bold),
                                          new SolidBrush(dgvReport.ColumnHeadersDefaultCellStyle.ForeColor),
                                          new RectangleF(x, y, w, rect.Height / 3),
                                          format);
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    x = 0.0F;
                    y = 0.0F;
                    w = 0;
                }
                else if (i == dgvReport.Columns.Count - 1)
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(216, 228, 188)),
                                             new RectangleF(x, y, w, rect.Height / 3));

                    int.TryParse(x.ToString(), out var ix);
                    int.TryParse(y.ToString(), out var iy);
                    e.Graphics.DrawRectangle(Pens.Black, new Rectangle(ix, iy, w - 1, rect.Height / 3));
                    
                    e.Graphics.DrawString("SCARTI",
                                          new Font("Tahoma", 12, FontStyle.Bold),
                                          new SolidBrush(dgvReport.ColumnHeadersDefaultCellStyle.ForeColor),
                                          new RectangleF(x, y, w, rect.Height / 3),
                                          format);
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    x = 0.0F;
                    y = 0.0F;
                    w = 0;
                }
            }

            //blocks
            w = 0;
            x = 0.0f;
            y = 0.0f;
            string[] str = new string[] { "Finezza", "N. macchine in lavoro", string.Empty, string.Empty, "% - Efficienze lorde",
                                          "% efficienza netta ( - ore scarti)", "Nr Teli a rammendo prodotti", string.Empty, "% - Rammendo",
                                          "Nr Teli scarti", "% - Scarti"};
            var strPosition = 0;
            Rectangle r = dgvReport.GetCellDisplayRectangle(1, -1, true);
            x = r.X;
            y = r.Y + r.Height / 3 + 3;
            for (var i = 1; i < dgvReport.Columns.Count; i ++)
            {
                r = dgvReport.GetCellDisplayRectangle(i, -1, true);
                w += r.Width;
                if(i >= 4 && i <= 7 || i == 11 || i == 12 || i == 16 || i == 17 ||
                   i == 21 || i == 25 || i == 30)
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.FillRectangle(new SolidBrush(Color.White),
                                             new RectangleF(x, y - 2, w, r.Height / 3 + 15));
                    int.TryParse(x.ToString(), out var ix);
                    int.TryParse(y.ToString(), out var iy);
                    e.Graphics.DrawRectangle(Pens.Black, new Rectangle(ix, iy, w, r.Height / 3 + 15));
                    e.Graphics.DrawString(str[strPosition],
                                          new Font("Tahoma", 8, FontStyle.Regular),
                                          new SolidBrush(dgvReport.ColumnHeadersDefaultCellStyle.ForeColor),
                                          new RectangleF(x, y, w, r.Height / 3 + 15),
                                          format);
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    w = 0;
                    r = dgvReport.GetCellDisplayRectangle(i + 1, -1, true);
                    x = r.X;
                    y = r.Y + r.Height / 3 + 3;
                    strPosition++;
                }
            }
        }
        private void dgvReport_Scroll(object sender, ScrollEventArgs e)
        {
            dgvReport.Invalidate();
        }
        private void ProcessData()
        {
            if(Get_Date_From() > Get_Date_To())
            {
                MessageBox.Show("Invalid date selection!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            LoadingInfo.InfoText = "Loading report...";
            LoadingInfo.ShowLoading();
            var dsDays = new DataSet();
            LoadProcedure(dsDays);
            _components = new List<Component>();
            if (dsDays.Tables[0].Rows.Count <= 0)
            {
                LoadingInfo.CloseLoading();
                return;
            }
            //creating list of components with days and active machines fields
            foreach (DataRow row in dsDays.Tables[0].Rows)
            {
                DateTime.TryParse(row[0].ToString(), out var currDay);
                int.TryParse(row[1].ToString(), out var actMachines);
                var newComp = new Component();
                newComp.CurrentDay = currDay;
                newComp.ActiveMachines = actMachines;
                _components.Add(newComp);
            }
            //inserting efficiency for finezzes
            foreach (DataRow row in dsDays.Tables[1].Rows)
            {
                DateTime.TryParse(row[0].ToString(), out var currDay);
                int.TryParse(row[1].ToString(), out var fin);
                var eff = row[2].ToString();
                var component = (from c in _components
                                 where c.CurrentDay == currDay
                                 select c).SingleOrDefault();
                if (fin == 7) component.FinSeven = eff + "%";
                else if (fin == 14) component.FinFourtheen = eff + "%";
                else if (fin == 3) component.FinThree = eff + "%";
            }
            //inserting efficiency per squadra'
            DateTime.TryParse(dsDays.Tables[2].Rows[0][0].ToString(), out var startDay);
            var effSq1 = 0.0;
            var effSq2 = 0.0;
            var effSq3 = 0.0;
            var qtySq1 = 0;
            var qtySq2 = 0;
            var qtySq3 = 0;
            foreach (DataRow row in dsDays.Tables[2].Rows)
            {
                DateTime.TryParse(row[0].ToString(), out var currDay);
                int.TryParse(row[1].ToString(), out var mac);
                double.TryParse(row[2].ToString(), out var eff);
                int.TryParse(row[3].ToString(), out var piecesProd);
                if(startDay != currDay)
                {
                    var component = (from c in _components
                                        where c.CurrentDay == startDay
                                        select c).SingleOrDefault();
                    if (effSq1 == 0.0)
                    {
                        component.EffOne = "0.0%";
                    }
                    else
                    {
                        component.EffOne = Math.Round(effSq1 / 70, 1).ToString() + "%";
                    }
                    if (effSq2 == 0.0)
                    {
                        component.EffTwo = "0.0%";
                    }
                    else
                    {
                        component.EffTwo = Math.Round(effSq2 / 70, 1).ToString() + "%";
                    }
                    if(effSq3 == 0.0)
                    {
                        component.EffThree = "0.0%";
                    }
                    else
                    {
                        component.EffThree = Math.Round(effSq3 / 70, 1).ToString() + "%";
                    }
                    component.PiecesProducedSqOne = qtySq1;
                    component.PiecesProducedSqTwo = qtySq2;
                    component.PiecesProducedSqThree = qtySq3;
                    effSq1 = 0.0;
                    effSq2 = 0.0;
                    effSq3 = 0.0;
                    qtySq1 = 0;
                    qtySq2 = 0;
                    qtySq3 = 0;
                }                
                if (mac >= 1 && mac <= 70)
                {
                    effSq1 += eff;
                    qtySq1 += piecesProd;
                }
                else if (mac >= 71 && mac <= 140)
                {
                    effSq2 += eff;
                    qtySq2 += piecesProd;
                }
                else if (mac >= 141 && mac <= 210)
                {
                    effSq3 += eff;
                    qtySq3 += piecesProd;
                }
                startDay = currDay;
            }
            var tmpComponent = (from c in _components
                            where c.CurrentDay == startDay
                            select c).SingleOrDefault();
            tmpComponent.EffOne = Math.Round(effSq1 / 70, 1).ToString() + "%";
            tmpComponent.EffTwo = Math.Round(effSq2 / 70, 1).ToString() + "%";
            tmpComponent.EffThree = Math.Round(effSq3 / 70, 1).ToString() + "%";
            tmpComponent.PiecesProducedSqOne = qtySq1;
            tmpComponent.PiecesProducedSqTwo = qtySq2;
            tmpComponent.PiecesProducedSqThree = qtySq3;


            //calculating scarti/rammendi pieces produced and efficiency
            //and inserting into list
            if (dsDays.Tables[3].Rows.Count <= 0)
            {
                LoadingInfo.CloseLoading();
                return;
            }
            DateTime.TryParse(dsDays.Tables[3].Rows[0][0].ToString(), out var date);
            var operator_code = dsDays.Tables[3].Rows[0][1].ToString();
            int.TryParse(dsDays.Tables[3].Rows[0][2].ToString(), out var machine);
            var order = dsDays.Tables[3].Rows[0][3].ToString();
            var article = dsDays.Tables[3].Rows[0][4].ToString();
            int.TryParse(dsDays.Tables[3].Rows[0][5].ToString(), out var tmpScarti);
            int.TryParse(dsDays.Tables[3].Rows[0][6].ToString(), out var tmpRammendi);
            var shift = dsDays.Tables[3].Rows[0][7].ToString();
            var sqOneScarti = 0;
            var sqTwoScarti = 0;
            var sqThreeScarti = 0;
            var sqOneRammendi = 0;
            var sqTwoRammendi = 0;
            var sqThreeRammendi = 0;
            var scarti = 0;
            var ramm = 0;
            foreach (DataRow row in dsDays.Tables[3].Rows)
            {
                DateTime.TryParse(row[0].ToString(), out var dt);
                var operCode = row[1].ToString();
                int.TryParse(row[2].ToString(), out var mac);
                var ord = row[3].ToString();
                var art = row[4].ToString();
                int.TryParse(row[5].ToString(), out scarti);
                int.TryParse(row[6].ToString(), out ramm);
                var sh = row[7].ToString();
                if (date != dt)
                {
                    sqThreeScarti += scarti;
                    sqThreeRammendi += ramm;
                    var component = (from c in _components
                                     where c.CurrentDay == date
                                     select c).SingleOrDefault();
                    component.ScartiPiecesOne = sqOneScarti;
                    component.ScartiPiecesTwo = sqTwoScarti;
                    component.ScartiPiecesThree = sqThreeScarti;
                    component.ScartiPiecesTotal = sqOneScarti + sqTwoScarti + sqThreeScarti;
                    double.TryParse(sqOneScarti.ToString(), out var sone);
                    double.TryParse(sqTwoScarti.ToString(), out var stwo);
                    double.TryParse(sqThreeScarti.ToString(), out var sthree);
                    double.TryParse(component.PiecesProducedSqOne.ToString(), out var qone);
                    double.TryParse(component.PiecesProducedSqTwo.ToString(), out var qtwo);
                    double.TryParse(component.PiecesProducedSqThree.ToString(), out var qthree);
                    component.ScartiEffOne = Math.Round((sone / qone) * 100, 1).ToString() + "%";
                    component.ScartiEffTwo = Math.Round((stwo / qtwo) * 100, 1).ToString() + "%";
                    component.ScartiEffThree = Math.Round((sthree / qthree) * 100, 1).ToString() + "%";

                    component.RammendiPiecesOne = sqOneRammendi;
                    component.RammendiPiecesTwo = sqTwoRammendi;
                    component.RammendiPiecesThree = sqThreeRammendi;
                    component.RammendiPiecesTotal = sqOneRammendi + sqTwoRammendi + sqThreeRammendi;
                    double.TryParse(sqOneRammendi.ToString(), out var rone);
                    double.TryParse(sqTwoRammendi.ToString(), out var rtwo);
                    double.TryParse(sqThreeRammendi.ToString(), out var rthree);
                    component.RammendiEffOne = Math.Round((rone / qone) * 100, 1).ToString() + "%";
                    component.RammendiEffTwo = Math.Round((rtwo / qtwo) * 100, 1).ToString() + "%";
                    component.RammendiEffThree = Math.Round((rthree / qthree) * 100, 1).ToString() + "%";

                    sqOneScarti = 0;
                    sqTwoScarti = 0;
                    sqThreeScarti = 0;
                    sqOneRammendi = 0;
                    sqTwoRammendi = 0;
                    sqThreeRammendi = 0;
                    //date = dt;
                    //operator_code = operCode;
                    //order = ord;
                    //article = art;
                    //shift = sh;
                }
                if (order != ord || operator_code != operCode || article != art || machine != mac || shift != sh)
                {
                    if(machine >= 1 && machine <= 70)
                    {
                        sqOneScarti += tmpScarti;
                        sqOneRammendi += tmpRammendi;
                    }
                    else if(machine >= 71 && machine <= 140)
                    {
                        sqTwoScarti += tmpScarti;
                        sqTwoRammendi += tmpRammendi;
                    }
                    else if(machine >= 141 && machine <= 210)
                    {
                        sqThreeScarti += tmpScarti;
                        sqThreeRammendi += tmpRammendi;
                    }
                }
                machine = mac;
                date = dt;
                operator_code = operCode;
                order = ord;
                article = art;
                tmpRammendi = ramm;
                tmpScarti = scarti;
                shift = sh;
            }

            //last selected day
            var comp = (from cp in _components
                        where cp.CurrentDay == date
                        select cp).SingleOrDefault();
            sqThreeScarti += tmpScarti;
            sqThreeRammendi += tmpRammendi;
            comp.ScartiPiecesOne = sqOneScarti;
            comp.ScartiPiecesTwo = sqTwoScarti;
            comp.ScartiPiecesThree = sqThreeScarti;
            comp.ScartiPiecesTotal = sqOneScarti + sqTwoScarti + sqThreeScarti;
            double.TryParse(sqOneScarti.ToString(), out var ssone);
            double.TryParse(sqTwoScarti.ToString(), out var sstwo);
            double.TryParse(sqThreeScarti.ToString(), out var ssthree);
            double.TryParse(comp.PiecesProducedSqOne.ToString(), out var qqone);
            double.TryParse(comp.PiecesProducedSqTwo.ToString(), out var qqtwo);
            double.TryParse(comp.PiecesProducedSqThree.ToString(), out var qqthree);
            comp.ScartiEffOne = Math.Round((ssone / qqone) * 100, 1).ToString() + "%";
            comp.ScartiEffTwo = Math.Round((sstwo / qqtwo) * 100, 1).ToString() + "%";
            comp.ScartiEffThree = Math.Round((ssthree / qqthree) * 100, 1).ToString() + "%";

            comp.RammendiPiecesOne = sqOneRammendi;
            comp.RammendiPiecesTwo = sqTwoRammendi;
            comp.RammendiPiecesThree = sqThreeRammendi;
            comp.RammendiPiecesTotal = sqOneRammendi + sqTwoRammendi + sqThreeRammendi;
            double.TryParse(sqOneRammendi.ToString(), out var rrone);
            double.TryParse(sqTwoRammendi.ToString(), out var rrtwo);
            double.TryParse(sqThreeRammendi.ToString(), out var rrthree);
            comp.RammendiEffOne = Math.Round((rrone / qqone) * 100, 1).ToString() + "%";
            comp.RammendiEffTwo = Math.Round((rrtwo / qqtwo) * 100, 1).ToString() + "%";
            comp.RammendiEffThree = Math.Round((rrthree / qqthree) * 100, 1).ToString() + "%";
            //last selected day

            foreach(DataRow row in dsDays.Tables[4].Rows)
            {
                DateTime.TryParse(row[0].ToString(), out var dt);
                int.TryParse(row[1].ToString(), out var fin);
                int.TryParse(row[2].ToString(), out var machines);
                var component = (from c in _components
                                 where c.CurrentDay == dt
                                 select c).SingleOrDefault();
                if(fin == 3)
                {
                    component.MacOnFin3 = machines;
                }
                else if(fin == 7)
                {
                    component.MacOnFin7 = machines;
                }
                else if(fin == 14)
                {
                    component.MacOnFin14 = machines;
                }
            }

            var finalTable = new DataTable();
            CreateTableView(finalTable);
            var totalRow = finalTable.NewRow();
            totalRow[0] = "TOTALI";
            finalTable.Rows.Add(totalRow);
            var mediaRow = finalTable.NewRow();
            mediaRow[0] = "MEDIA";
            finalTable.Rows.Add(mediaRow);
            foreach(var record in _components)
            {
                //skip holidays
                if (string.IsNullOrEmpty(record.FinFourtheen) && string.IsNullOrEmpty(record.FinSeven) &&
                   string.IsNullOrEmpty(record.FinThree))                
                    continue;
                
                var counter = 0;
                var newRow = finalTable.NewRow();
                newRow[0] = record.CurrentDay.Day;
                newRow[1] = record.FinThree;
                newRow[2] = string.Empty; //fin5
                newRow[3] = record.FinSeven;
                newRow[4] = record.FinFourtheen;
                newRow[5] = record.ActiveMachines;
                newRow[6] = string.Empty; //euro
                newRow[7] = string.Empty; //next to euro

                if (!string.IsNullOrEmpty(record.EffOne)) counter++;
                newRow[8] = record.EffOne;
                if (!string.IsNullOrEmpty(record.EffTwo)) counter++;
                newRow[9] = record.EffTwo;
                if (!string.IsNullOrEmpty(record.EffThree)) counter++;
                newRow[10] = record.EffThree;
                double eff1 = 0.0;
                double eff2 = 0.0;
                double eff3 = 0.0;
                if (record.EffOne.Contains("%"))
                    double.TryParse(record.EffOne.Split('%')[0], out eff1);                
                if (record.EffTwo.Contains("%"))
                    double.TryParse(record.EffTwo.Split('%')[0], out eff2);
                if (record.EffThree.Contains("%"))
                    double.TryParse(record.EffThree.Split('%')[0], out eff3);
                var totalEff = 0.0;
                var totalEffSquadra = eff1 + eff2 + eff3;
                if (totalEffSquadra <= 0.0 || counter <= 0)
                    totalEff = 0.0;
                else
                    totalEff = Math.Round((totalEffSquadra) / counter, 1);
                newRow[11] = /*totalEff == double.NaN ? string.Empty : */totalEff.ToString() + "%"; //total for eff

                var effLorda = totalEff;
                //newRow[12] = string.Empty; // eff - time for producing scarti

                newRow[13] = record.RammendiPiecesOne;
                newRow[14] = record.RammendiPiecesTwo;
                newRow[15] = record.RammendiPiecesThree;
                newRow[16] = record.RammendiPiecesTotal;
                newRow[17] = string.Empty; //something in middle between scarti teli and scarti % columns

                counter = 0;
                if (!string.IsNullOrEmpty(record.RammendiEffOne)) counter++;
                newRow[18] = record.RammendiEffOne;
                if (!string.IsNullOrEmpty(record.RammendiEffTwo)) counter++;
                newRow[19] = record.RammendiEffTwo;
                if (!string.IsNullOrEmpty(record.RammendiEffThree)) counter++;
                newRow[20] = record.RammendiEffThree;

                if (!string.IsNullOrEmpty(record.RammendiEffOne) && record.RammendiEffOne.Contains('%'))
                    double.TryParse(record.RammendiEffOne.Split('%')[0], out eff1);
                if (!string.IsNullOrEmpty(record.RammendiEffTwo) && record.RammendiEffTwo.Contains('%'))
                    double.TryParse(record.RammendiEffTwo.Split('%')[0], out eff2);
                if (!string.IsNullOrEmpty(record.RammendiEffThree) && record.RammendiEffThree.Contains('%'))
                    double.TryParse(record.RammendiEffThree.Split('%')[0], out eff3);

                var totalSquadraEfficiency = eff1 + eff2 + eff3;
                if (totalSquadraEfficiency <= 0.0 || counter <= 0)
                    totalEff = 0.0;
                else
                    totalEff = Math.Round((totalSquadraEfficiency) / counter, 1);

                newRow[21] = /*totalEff == double.NaN ? string.Empty : */totalEff.ToString() + "%";
                
                newRow[22] = record.ScartiPiecesOne;
                newRow[23] = record.ScartiPiecesTwo;
                newRow[24] = record.ScartiPiecesThree;
                newRow[25] = record.ScartiPiecesTotal;

                counter = 0;
                if (!string.IsNullOrEmpty(record.ScartiEffOne)) counter++;
                newRow[26] = record.ScartiEffOne;
                if (!string.IsNullOrEmpty(record.ScartiEffTwo)) counter++;
                newRow[27] = record.ScartiEffTwo;
                if (!string.IsNullOrEmpty(record.ScartiEffThree)) counter++;
                newRow[28] = record.ScartiEffThree;
                if(!string.IsNullOrEmpty(record.ScartiEffOne) && record.ScartiEffOne.Contains('%'))
                    double.TryParse(record.ScartiEffOne.Split('%')[0], out eff1);
                if (!string.IsNullOrEmpty(record.ScartiEffTwo) && record.ScartiEffTwo.Contains('%'))
                    double.TryParse(record.ScartiEffTwo.Split('%')[0], out eff2);
                if (!string.IsNullOrEmpty(record.ScartiEffThree) && record.ScartiEffThree.Contains('%'))
                    double.TryParse(record.ScartiEffThree.Split('%')[0], out eff3);
                var effSquadraTotal = eff1 + eff2 + eff3;
                if (effSquadraTotal <= 0.0 || counter <= 0)
                    totalEff = 0.0;
                else
                    totalEff = Math.Round((effSquadraTotal) / counter, 1);
                newRow[29] =/* totalEff == double.NaN ? string.Empty : */totalEff.ToString() + "%";

                newRow[12] = (effLorda - totalEff) == double.NaN ? string.Empty : (effLorda - totalEff).ToString() + "%";
                
                newRow[30] = string.Empty;
                finalTable.Rows.Add(newRow);
            }
            if (dgvReport.DataSource != null)
                dgvReport.DataSource = null;
            dgvReport.DataSource = finalTable;
            CalculateHeaders();
            LoadingInfo.CloseLoading();
        }
        private void LoadProcedure(DataSet ds)
        {
            var cmd = new SqlCommand()
            {
                CommandText = "get_giornaliera_data",
                Connection = MainWnd._sql_con,
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 99999999
            };
            cmd.Parameters.Add("@from_date", SqlDbType.DateTime).Value = Get_Date_From();
            cmd.Parameters.Add("@to_date", SqlDbType.DateTime).Value = Get_Date_To();
            MainWnd._sql_con.Open();
            var da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            MainWnd._sql_con.Close();
            da.Dispose();
            cmd = null;
        }
        private DateTime Get_Date_From()
        {
            return new DateTime(dtpFrom.Value.Year, dtpFrom.Value.Month, dtpFrom.Value.Day);
        }
        private DateTime Get_Date_To()
        {
            return new DateTime(dtpTo.Value.Year, dtpTo.Value.Month, dtpTo.Value.Day);
        }
        private void btnGo_Click(object sender, EventArgs e)
        {
            ProcessData();
        }
        private void dgvReport_BindingComplete(object sender, EventArgs e)
        {
            dgvReport.DoubleBufferedDataGridView(true);
        }
        private void CalculateHeaders()
        {
            for (var c = 1; c < dgvReport.Columns.Count; c++)
            {
                var total = 0.0;
                var counter = 0;
                //var daysOfFinezza = 0;
                for (var r = 2; r < dgvReport.Rows.Count; r++)
                {
                    if (string.IsNullOrEmpty(dgvReport.Rows[r].Cells[c].Value.ToString()))
                        continue;
                    //daysOfFinezza++;
                    if (dgvReport.Rows[r].Cells[c].Value.ToString().Contains('%'))
                    {
                        double.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString().Split('%')[0], out var eff);
                        total += eff;
                        counter++;
                    }
                    else
                    {
                        double.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString(), out var pieces);
                        total += pieces;
                        counter++;
                    }
                }
                if (c == 5 || c >= 13 && c <= 16 || c >= 22 && c <= 25)
                {
                    var r = 1;
                    if (c == 5)
                        r = 0;
                    if (counter == 0)
                        dgvReport.Rows[1].Cells[c].Value = string.Empty;
                    else
                        dgvReport.Rows[1].Cells[c].Value = Math.Round(total / counter, r).ToString();

                    if (c == 5)
                        dgvReport.Rows[0].Cells[c].Value = string.Empty;
                    else
                        dgvReport.Rows[0].Cells[c].Value = total.ToString();
                }
                else if (c >= 1 && c <= 4 || c >= 8 && c <= 12 || c >= 18 && c <= 21 ||
                        c >= 26 && c <= 30)
                {
                    if (c == 1 || c == 3 || c == 4)
                    {
                        var finMachines = 0;
                        foreach (var comp in _components)
                        {
                            if (c == 1)
                            {
                                finMachines += comp.MacOnFin3;
                            }
                            if (c == 3)
                            {
                                finMachines += comp.MacOnFin7;
                            }
                            if (c == 4)
                            {
                                finMachines += comp.MacOnFin14;
                            }
                        }
                        if (counter == 0)
                            dgvReport.Rows[0].Cells[c].Value = string.Empty;
                        else
                        {
                            double.TryParse(finMachines.ToString(), out var fMac);
                            dgvReport.Rows[0].Cells[c].Value = Math.Round(fMac / counter, 1);
                        }
                    }

                    if(counter == 0)
                        dgvReport.Rows[1].Cells[c].Value = string.Empty;
                    else
                        dgvReport.Rows[1].Cells[c].Value = Math.Round(total / counter, 1).ToString() + "%";
                }
                else
                    dgvReport.Rows[1].Cells[c].Value = string.Empty;
            }
        }
    }
    public class Component
    {
        public Component()
        {
        }

        public DateTime CurrentDay { get; set; }

        public int PiecesProducedSqOne { get; set; }
        public int PiecesProducedSqTwo { get; set; }
        public int PiecesProducedSqThree { get; set; }

        public string FinThree{ get; set; }
        public string FinSeven { get; set; }
        public string FinFourtheen { get; set; }
        public int ActiveMachines { get; set; }

        public string EffOne { get; set; }
        public string EffTwo { get; set; }
        public string EffThree { get; set; }

        public int ScartiPiecesOne { get; set; }
        public int ScartiPiecesTwo { get; set; }
        public int ScartiPiecesThree { get; set; }
        public int ScartiPiecesTotal { get; set; }
        public string ScartiEffOne { get; set; }
        public string ScartiEffTwo { get; set; }
        public string ScartiEffThree { get; set; }

        public int RammendiPiecesOne { get; set; }
        public int RammendiPiecesTwo { get; set; }
        public int RammendiPiecesThree { get; set; }
        public int RammendiPiecesTotal { get; set; }
        public string RammendiEffOne { get; set; }
        public string RammendiEffTwo { get; set; }
        public string RammendiEffThree { get; set; }

        public int MacOnFin3 { get; set; }
        public int MacOnFin7 { get; set; }
        public int MacOnFin14 { get; set; }
    }
}
