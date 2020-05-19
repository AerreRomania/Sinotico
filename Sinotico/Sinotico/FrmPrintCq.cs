using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sinotico
{
    public partial class FrmPrintCq : Form
    {
        public FrmPrintCq()
        {
            InitializeComponent();
        }        
        private DataTable _dtJob = new DataTable();
        private List<Image> _imgList = new List<Image>();
        private Font _bFont = new Font("Arial", 12, FontStyle.Bold);
        private Font _rFont = new Font("Arial", 9, FontStyle.Regular);
        public int IdJob { get; set; }
        public string Operator { get; set; }
        public FrmPrintCq(int idJob, string operat)
        {
            InitializeComponent();
            IdJob = idJob;
            Operator = operat;
        }
        private void FrmPrintCq_Load(object sender, EventArgs e)
        {
            _dictJob = new Dictionary<string, string>();
            _dtJob = new DataTable();

            var qJob = "select * from cquality where id='" + IdJob.ToString() + "'";
            var qImg = "select img from imgresource where idjob='" + IdJob.ToString() + "'";
            _imgList = new List<Image>();
            using (var c = new System.Data.SqlClient.SqlConnection(MainWnd.conString))
            {
                c.Open();
                var cmd = new System.Data.SqlClient.SqlCommand(qJob, c);
                var dr = cmd.ExecuteReader();
                _dtJob.Load(dr);
                dr.Close();
                cmd = new System.Data.SqlClient.SqlCommand(qImg, c);
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                    while (dr.Read())
                    {
                        byte[] myByte = (byte[])dr[0];
                        using (MemoryStream ms = new MemoryStream(myByte))
                        {
                            Image img = Image.FromStream(ms);
                            _imgList.Add(img);
                        }
                    }
                dr.Close();
                c.Close();
                cmd = null;
            }
            pbPrint.MouseEnter += pbPrint_MouseEnter;
            pbPrint.MouseLeave += pbPrint_MouseLeave;
            pbPrint.Click += pbPrint_Click;
            pnlPreview.Paint += pnlPreview_Paint;
        }
        private void pbPrint_Click(object sender, EventArgs e)
        {
            try
            {
                var pd = new PrintDocument();
                var pDiag = new PrintDialog();
                pDiag.Document = pd;
                if (pDiag.ShowDialog() == DialogResult.OK)
                {
                    pd.PrintPage += new PrintPageEventHandler(Pd_PrintPage);
                    pd.Print();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + '\n' + ex.StackTrace);
            }
        }
        private Dictionary<string, string> _dictJob = new Dictionary<string, string>();
        //private int currentImg = 0;
        //private float lineHeight = 50.0f;
        //private int pg = 1;
        //private int x = 40;
        private void Pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Default;                                   
            float pageHeight = e.PageSettings.PrintableArea.Height;
            //for (var i = currentImg; i < _imgList.Count; i++)
            //{
            //    if(pg == 1 && lineHeight == 50.0f)
            //    {
            //        e.Graphics.DrawString("ONLYOU - Sinottico CQ job report\nPrint date: " +
            //    DateTime.Now.ToString("dd/MM/yyyy") + "\n\nOperator: " + Operator, _rFont, Brushes.Black, 30, 30);

            //        var brs = Brushes.Black;
            //        lineHeight = 50.0F;
            //        var sb = new StringBuilder();
            //        foreach (DataColumn c in _dtJob.Columns)
            //        {
            //            var name = c.ColumnName;
            //            if (!_dictJob.ContainsKey(name))
            //                _dictJob.Add(name, _dtJob.Rows[0][name].ToString());
            //        }
            //        foreach (KeyValuePair<string, string> job in _dictJob)
            //        {
            //            if (job.Key == "operator_id") continue;
            //            var bStr = job.Key.ToString() + ": ";
            //            var rest = job.Value.ToString();
            //            var w = e.Graphics.MeasureString(bStr + ": ", _bFont).Width;
            //            e.Graphics.DrawString(bStr.ToUpperInvariant(), _bFont, brs, 40, 50 + lineHeight);
            //            e.Graphics.DrawString(rest, _rFont, brs, 300, 50 + lineHeight + 3);
            //            e.Graphics.DrawLine(Pens.Black, new PointF(40, 50.0F + lineHeight), new PointF(e.MarginBounds.Right - 40, 50 + lineHeight));
            //            lineHeight += e.Graphics.MeasureString(bStr, _bFont).Height;
            //        }
            //        lineHeight += 100;
            //    }
            //    if (x >= 600)
            //    {
            //        x = 40;
            //        lineHeight += 310;
            //    }
            //    if (lineHeight + 310 >= pageHeight)
            //    {
            //        e.HasMorePages = true;
            //        currentImg = i;
            //        lineHeight = 50.0f;
            //        pg++;
            //        return;
            //    }
            //    else e.HasMorePages = false;
            //        e.Graphics.DrawImage(_imgList[i], x, lineHeight, 300, 300);
            //    x += 370;
            //}
            e.Graphics.DrawString("ONLYOU - Sinottico CQ job report\nPrint date: " +
                DateTime.Now.ToString("dd/MM/yyyy") + "\n\nOperator: " + Operator, _rFont, Brushes.Black, 30, 30);
            var brs = Brushes.Black;
            var y = 50.0F;
            var sb = new StringBuilder();
            foreach (DataColumn c in _dtJob.Columns)
            {
                var name = c.ColumnName;
                if (!_dictJob.ContainsKey(name))
                    _dictJob.Add(name, _dtJob.Rows[0][name].ToString());
            }
            foreach (KeyValuePair<string, string> job in _dictJob)
            {
                if (job.Key == "operator_id") continue;
                var bStr = job.Key.ToString() + ": ";
                var rest = job.Value.ToString();
                var w = e.Graphics.MeasureString(bStr + ": ", _bFont).Width;
                e.Graphics.DrawString(bStr.ToUpperInvariant(), _bFont, brs, 40, 50 + y);
                e.Graphics.DrawString(rest, _rFont, brs, 200, 50 + y + 3);
                e.Graphics.DrawLine(Pens.Black, new PointF(40, 50.0F + y), new PointF(e.MarginBounds.Right - 40, 50 + y));
                y += e.Graphics.MeasureString(bStr, _bFont).Height;
            }
            var x = 20;
            y += 100;
            foreach (var img in _imgList)
            {
                e.Graphics.DrawImage(img, x, y, 120, 120);
                x += 130;
                if (x >= 200)
                {
                    x = 20;
                    y += 130;
                }
            }
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        }
        private void pbPrint_MouseEnter(object sender, EventArgs e)
        {
            ControlPaint.DrawBorder(pbPrint.CreateGraphics(), pbPrint.ClientRectangle, Color.LightSteelBlue, ButtonBorderStyle.Solid);
        }
        private void pbPrint_MouseLeave(object sender, EventArgs e)
        {
            pbPrint.Invalidate();
        }
        private void pnlPreview_Paint(object sender, PaintEventArgs e)
        {
            var pnl = sender as Panel;
            e.Graphics.TranslateTransform(pnl.AutoScrollPosition.X, pnl.AutoScrollPosition.Y);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Default;
            e.Graphics.DrawString("ONLYOU - Sinottico CQ job report\nPrint date: " +
                DateTime.Now.ToString("dd/MM/yyyy") + "\n\nOperator: " + Operator, _rFont, Brushes.Black, 30, 30);
            var brs = Brushes.Black;
            var y = 50.0F;
            var sb = new StringBuilder();
            foreach (DataColumn c in _dtJob.Columns)
            {
                var name = c.ColumnName;
                if (!_dictJob.ContainsKey(name))
                    _dictJob.Add(name, _dtJob.Rows[0][name].ToString());
            }
            foreach (KeyValuePair<string, string> job in _dictJob)
            {
                if (job.Key == "operator_id") continue;
                var bStr = job.Key.ToString() + ": ";
                var rest = job.Value.ToString();
                var w = e.Graphics.MeasureString(bStr + ": ", _bFont).Width;
                e.Graphics.DrawString(bStr.ToUpperInvariant(), _bFont, brs, 40, 50 + y);
                e.Graphics.DrawString(rest, _rFont, brs, 200, 50 + y + 3);
                e.Graphics.DrawLine(Pens.Black, new PointF(40, 50.0F + y), new PointF(pnl.Right - 40, 50 + y));
                y += e.Graphics.MeasureString(bStr, _bFont).Height;
            }
            var x = 20;
            y += 100;
            foreach (var img in _imgList)
            {                
                e.Graphics.DrawImage(img, x, y, 120, 120);
                x += 130;
                if (x >= 200)
                {
                    x = 20;
                    y += 130;
                }
            }            
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        }
    }
}
