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
    public partial class RepCharts : Form
    {
        private List<MachinesStops> _stops = new List<MachinesStops>();
        private List<ChartElement> _oees = new List<ChartElement>();
        private List<ChartElement> _oeeSecondary = new List<ChartElement>();

        public RepCharts()
        {
            InitializeComponent();
        }

        private DateTime Get_Date_From()
        {
            return new DateTime(dtpFrom.Value.Year, dtpFrom.Value.Month, dtpFrom.Value.Day);
        }
        private DateTime Get_Date_To()
        {
            return new DateTime(dtpTo.Value.Year, dtpTo.Value.Month, dtpTo.Value.Day);
        }

        private int Get_Holidays_In_Range(DateTime dateFrom, DateTime dateTo)
        {
            FrmHolidays.dc = new System.Data.Linq.DataContext(MainWnd.conString);
            var holidays = (from h in Tables.TblHolidays
                            where h.Holiday >= dateFrom &&
                            h.Holiday <= dateTo
                            select h).ToList();
            if (holidays.Count == 0) return 0;
            var counter = 1;
            var startDate = new DateTime();
            startDate = holidays[0].Holiday;
            foreach (var h in holidays)
            {
                if (h.Holiday != startDate)
                {
                    counter++;
                }
                startDate = h.Holiday;
            }
            return counter;
        }
        private string Get_finezza_array()
        {
            var finArray = string.Empty;
            switch (cboFin.Text)
            {
                case "":
                    finArray = ",3,7,14,";
                    break;
                case "<All>":
                    finArray = ",3,7,14,";
                    break;
                case "3":
                    finArray = ",3,";
                    break;
                case "7":
                    finArray = ",7,";
                    break;
                case "14":
                    finArray = ",14,";
                    break;
            }
            return finArray;
        }
        private List<int> Get_Selected_Finezza()
        {
            if (string.IsNullOrEmpty(cboFin.Text) || cboFin.Text == "<All>")
            {
                return new List<int>() { 3, 7, 14 };
            }
            else
            {
                int.TryParse(cboFin.Text, out var fin);
                return new List<int>() { fin };
            }
        }

        private void FilloutFilter(ComboBox c)
        {
            c.Items.Clear();
            c.Items.Add("<All>");
            switch (c.Name)
            {
                case "cboFin":
                    c.Items.Add("3");
                    c.Items.Add("7");
                    c.Items.Add("14");
                    break;
            }
        }

        private void LoadProcedure(List<MachinesStops> lstStops)
        {
            var cmd = new System.Data.SqlClient.SqlCommand()
            {
                CommandText = "get_OEE_chart_data",
                Connection = MainWnd._sql_con,
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 99999999
            };
            cmd.Parameters.Add("@from_date", SqlDbType.DateTime).Value = Get_Date_From();
            cmd.Parameters.Add("@to_date", SqlDbType.DateTime).Value = Get_Date_To();
            cmd.Parameters.Add("@finesse", SqlDbType.VarChar).Value = Get_finezza_array();
            MainWnd._sql_con.Open();
            var dr = cmd.ExecuteReader();

            if(dr.HasRows)
            {
                while(dr.Read())
                { 
                    var newStop = new MachinesStops();

                    int.TryParse(dr[1].ToString(), out var month);
                    var m = string.Empty;
                    if (month < 10) m = "0" + month.ToString();
                    else m = month.ToString();
                    newStop.Date = dr[0].ToString() + "." + m;

                    double.TryParse(dr[2].ToString(), out var pettine);
                    double.TryParse(dr[3].ToString(), out var manuale);
                    double.TryParse(dr[4].ToString(), out var filato);
                    double.TryParse(dr[5].ToString(), out var aghi);
                    double.TryParse(dr[6].ToString(), out var urto);
                    double.TryParse(dr[7].ToString(), out var principale);
                    double.TryParse(dr[8].ToString(), out var altro);
                    double.TryParse(dr[9].ToString(), out var change_style);
                    double.TryParse(dr[10].ToString(), out var change_color);
                    double.TryParse(dr[11].ToString(), out var mc_breakdown);
                    double.TryParse(dr[12].ToString(), out var yarn_delay);
                    double.TryParse(dr[13].ToString(), out var yarn_quality);
                    double.TryParse(dr[14].ToString(), out var technique);
                    double.TryParse(dr[15].ToString(), out var maintance);
                    double.TryParse(dr[16].ToString(), out var cambio_taglia);
                    double.TryParse(dr[17].ToString(), out var preproduzione);
                    double.TryParse(dr[18].ToString(), out var svilu_potaglie);
                    double.TryParse(dr[19].ToString(), out var prototipo);
                    double.TryParse(dr[20].ToString(), out var campionario);
                    double.TryParse(dr[21].ToString(), out var riparazioni);
                    double.TryParse(dr[22].ToString(), out var cambio_art);
                    double.TryParse(dr[23].ToString(), out var pulizia_ord);
                    double.TryParse(dr[24].ToString(), out var pulizia_fron);
                    double.TryParse(dr[25].ToString(), out var cambio_aghi);

                    newStop.CambiOre = change_style + cambio_art + change_color + pettine + cambio_taglia;
                    newStop.PuliziaOre = pulizia_ord;
                    newStop.CampionarioOre = svilu_potaglie + prototipo + campionario + preproduzione;
                    newStop.CambioAghiOre = cambio_aghi;
                    newStop.QualitaFilatoOre = yarn_quality;
                    newStop.MaintanceOre = maintance + yarn_delay;
                    newStop.RotturaFilatoOre = filato;
                    newStop.PuliziaFrontureOre = pulizia_fron;
                    newStop.MeccaniciOre = manuale + mc_breakdown + riparazioni + urto + principale;
                    newStop.VarieTOre = technique + altro;
                    lstStops.Add(newStop);
                }
            }
            
            MainWnd._sql_con.Close();
            dr.Close();
            cmd = null;
        }
        private void CreateTableView(DataTable table)
        {
            table.Columns.Add("Categories");
            for (var date = Get_Date_From(); date <= Get_Date_To(); date = date.AddMonths(+1))
            {
                table.Columns.Add(date.ToString("yyyy.MM", System.Globalization.CultureInfo.InvariantCulture));
            }
            table.Columns.Add("Separator1");
            table.Columns.Add("Totals");
        }

        private void ProcessData()
        {
            if (Get_Date_From() > Get_Date_To())
            {
                MessageBox.Show("Invalid date selection!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            _stops = new List<MachinesStops>();
            LoadProcedure(_stops);

            _oees = new List<ChartElement>();
            _oeeSecondary = new List<ChartElement>();
            var finList = Get_Selected_Finezza();
            foreach (var month in _stops)
            {
                FrmHolidays.dc = new System.Data.Linq.DataContext(MainWnd.conString);
                int.TryParse(month.Date.Split('.')[0], out var y);
                int.TryParse(month.Date.Split('.')[1], out var m);
                var currentDate = new DateTime(y, m, 1);
                var parameters = (from p in Tables.TblMonthTrash
                                  where p.Date == currentDate
                                  select p).ToList();
                if (parameters == null || parameters.Count <= 0)
                {
                    continue;
                }

                double tempoPossibile = 0.0;
                double tempoDisponssibile = 0.0;
                double tempoProdutivo = 0.0;
                double tempoScarti = 0.0;
                double tempoRammendi = 0.0;
                foreach (var uniqueParameters in parameters)
                {
                    if (!finList.Exists(f => f == uniqueParameters.Finezza))
                    {
                        continue;
                    }
                    var daysInMonth = DateTime.DaysInMonth(uniqueParameters.Date.Year, uniqueParameters.Date.Month);
                    double.TryParse(daysInMonth.ToString(), out var days);
                    var tmpTempoCalendario = (days * uniqueParameters.ConsiderateMac
                                            - Get_Holidays_In_Range(currentDate, new DateTime(currentDate.Year, currentDate.Month, daysInMonth))
                                            * uniqueParameters.ConsiderateMac) * 86_400;
                    var tmpTempoPossibile = tmpTempoCalendario - (uniqueParameters.FermataStraordinaria + uniqueParameters.MancanzaLavoro) * 3600;
                    var tmpTempoProdutivo = uniqueParameters.Produtivo * 3600;
                    double.TryParse((uniqueParameters.Scarti * 3600).ToString(), out var tmpTempoScarti);
                    double.TryParse((uniqueParameters.Rammendi * 3600).ToString(), out var tmpTempoRammendi);
                    var tmpTempoDisponssibile = tmpTempoPossibile - (month.CambiOre + month.PuliziaOre + month.CampionarioOre + month.CambioAghiOre +
                                                           month.QualitaFilatoOre + month.MaintanceOre);
                    tempoPossibile += tmpTempoPossibile;
                    tempoDisponssibile += tmpTempoDisponssibile;
                    tempoProdutivo += tmpTempoProdutivo;
                    tempoScarti += tmpTempoScarti;
                    tempoRammendi += tmpTempoRammendi;
                }

                var quality = Math.Round((tempoProdutivo - tempoScarti - tempoRammendi) / tempoProdutivo, 1);
                var oee = Math.Round(((quality * ((tempoProdutivo / tempoDisponssibile) * 100) * ((tempoDisponssibile / tempoPossibile) * 100))) / 100, 1);
                _oees.Add(new ChartElement(month.Date, oee));
                _oeeSecondary.Add(
                    new ChartElement(
                        month.Date,
                        oee, Math.Round((month.CambiOre / tempoPossibile) * 100,1),
                        Math.Round((month.CambioAghiOre / tempoPossibile) * 100, 1),
                        2)); //generics ?
            }
        }
        private void LoadOeeChart()
        {
            ProcessData();
            oeeChart.Series.Clear();
            oeeChart.Series.Add("oeeSer");
            oeeChart.Series["oeeSer"].IsVisibleInLegend = false;
            oeeChart.Series["oeeSer"].Points.Clear();
            oeeChart.Series["oeeSer"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            oeeChart.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
            oeeChart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(210, 210, 210);
            oeeChart.ChartAreas[0].AxisX.LineColor = Color.FromArgb(80, 80, 80);
            oeeChart.ChartAreas[0].AxisY.LineColor = Color.FromArgb(80, 80, 80);
            oeeChart.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.FromArgb(80, 80, 80);
            oeeChart.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.FromArgb(80, 80, 80);
            oeeChart.ChartAreas[0].AxisX.InterlacedColor = Color.FromArgb(80, 80, 80);
            oeeChart.ChartAreas[0].AxisY.InterlacedColor = Color.FromArgb(80, 80, 80);
            oeeChart.ChartAreas[0].AxisX.Title = "Date";
            oeeChart.ChartAreas[0].AxisX.TitleFont = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
            oeeChart.ChartAreas[0].AxisX.TitleForeColor = Color.FromArgb(60, 60, 60);
            oeeChart.ChartAreas[0].AxisY.Title = "OEE  [%]";
            oeeChart.ChartAreas[0].AxisY.TitleFont = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
            oeeChart.ChartAreas[0].AxisY.TitleForeColor = Color.FromArgb(60, 60, 60);
            oeeChart.Series["oeeSer"].Font = new Font("Microsoft Sans Serif", 6, FontStyle.Regular);
            oeeChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            oeeChart.Series["oeeSer"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            oeeChart.Series["oeeSer"].MarkerSize = 6;
            oeeChart.Series["oeeSer"].MarkerColor = Color.FromArgb(79, 129, 189);
            oeeChart.Series["oeeSer"].Color = Color.FromArgb(79, 129, 189);
            if(_oees.Count >= 1)
            {
                foreach (var oee in _oees)
                {
                    oeeChart.Series["oeeSer"].Points.AddXY(oee.Date, oee.OeeValue);
                }
            }            
        }
        private void LoadOeeSecondaryChart()
        {
            oeeSecondaryChart.Series.Clear();
            oeeSecondaryChart.Series.Add("oeeSer");
            oeeSecondaryChart.Series.Add("petineSer");
            oeeSecondaryChart.Series.Add("aghiSer");
            oeeSecondaryChart.Series.Add("genericsSer");
            oeeSecondaryChart.Series[0].Color = Color.FromArgb(0, 175, 80);
            oeeSecondaryChart.Series[1].Color = Color.FromArgb(254, 221, 168);
            oeeSecondaryChart.Series[2].Color = Color.FromArgb(160, 145, 226);
            oeeSecondaryChart.Series[3].Color = Color.FromArgb(247, 150, 71);
            foreach (var chartSer in oeeSecondaryChart.Series)
            {
                chartSer.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;
                chartSer.Font = new Font("Microsoft Sans Serif", 6, FontStyle.Regular);
                chartSer.Points.Clear();
            }
            oeeSecondaryChart.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
            oeeSecondaryChart.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            oeeSecondaryChart.ChartAreas[0].AxisX.LineColor = Color.FromArgb(80, 80, 80);
            oeeSecondaryChart.ChartAreas[0].AxisY.LineColor = Color.FromArgb(80, 80, 80);
            oeeSecondaryChart.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.FromArgb(80, 80, 80);
            oeeSecondaryChart.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.FromArgb(80, 80, 80);
            oeeSecondaryChart.ChartAreas[0].AxisX.InterlacedColor = Color.FromArgb(80, 80, 80);
            oeeSecondaryChart.ChartAreas[0].AxisY.InterlacedColor = Color.FromArgb(80, 80, 80);
            oeeSecondaryChart.ChartAreas[0].AxisX.Title = "Date";
            oeeSecondaryChart.ChartAreas[0].AxisX.TitleFont = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
            oeeSecondaryChart.ChartAreas[0].AxisX.TitleForeColor = Color.FromArgb(60, 60, 60);
            oeeSecondaryChart.ChartAreas[0].AxisY.Title = "Vabre OEE";
            oeeSecondaryChart.ChartAreas[0].AxisY.TitleFont = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
            oeeSecondaryChart.ChartAreas[0].AxisY.TitleForeColor = Color.FromArgb(60, 60, 60);
            oeeSecondaryChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            oeeSecondaryChart.Series[0].LegendText = "OEE";
            oeeSecondaryChart.Series[1].LegendText = "Petine";
            oeeSecondaryChart.Series[2].LegendText = "Aghi";
            oeeSecondaryChart.Series[3].LegendText = "Generic stops";
            oeeSecondaryChart.Legends[0].BackColor = Color.WhiteSmoke;
            if(_oeeSecondary.Count >= 1)
            {
                foreach (var oee in _oeeSecondary)
                {
                    oeeSecondaryChart.Series["oeeSer"].Points.AddXY(oee.Date, oee.OeeValue);
                    oeeSecondaryChart.Series["petineSer"].Points.AddXY(oee.Date, oee.Petine);
                    oeeSecondaryChart.Series["aghiSer"].Points.AddXY(oee.Date, oee.Aghi);
                    oeeSecondaryChart.Series["genericsSer"].Points.AddXY(oee.Date, oee.GenericStop);
                }
            }            
        }

        private void RepCharts_Load(object sender, EventArgs e)
        {
            FilloutFilter(cboFin);
            tbMachine.Text = "210";
            oeeChart.Series.Clear();
            oeeSecondaryChart.Series.Clear();
        }

        private void GetChartsReport()
        {
            //try
            //    {
            //    lblFrom.Text = MainWnd.Get_from_date().ToString("dd-MMM-yy", System.Globalization.CultureInfo.InvariantCulture);
            //    lblTo.Text = MainWnd.Get_to_date().ToString("dd-MMM-yy", System.Globalization.CultureInfo.InvariantCulture);

            //    var tmpDtPie = new DataTable();
            //    var tmpDtStacked = new DataTable();
            //    var glb = new Globals();

            //    using (var con = new SqlConnection(MainWnd.conString))
            //        {
            //        con.Open();
            //        var cmd = new SqlCommand("get_data_in_stop", con);
            //        cmd.CommandType = CommandType.StoredProcedure;

            //        cmd.Parameters.Add("@from_date", SqlDbType.DateTime).Value = MainWnd.Get_to_date();
            //        cmd.Parameters.Add("@to_date", SqlDbType.DateTime).Value = MainWnd.Get_to_date();
            //        cmd.Parameters.Add("@shift", SqlDbType.VarChar).Value = MainWnd.Get_shift_array().ToString();

            //        var dr = cmd.ExecuteReader();
            //        tmpDtPie.Load(dr);
            //        dr.Close();
            //        cmd = null;

            //        var dayRange = MainWnd.Get_to_date().Subtract(MainWnd.Get_from_date()).Days;
            //        if (dayRange == 0) dayRange = 1;

            //        if (dayRange > 1)
            //            {
            //            LoadingInfo.CloseLoading();
            //            return;
            //            }

            //        var shiftsCount = MainWnd.ListOfSelectedShifts.Count;
            //        if (shiftsCount == 0) shiftsCount = 1;

            //        var macCount = (210 * shiftsCount) * dayRange;

            //        if (tmpDtPie.Rows.Count <= 0) return;
            //        double.TryParse(tmpDtPie.Rows[0][0].ToString(), out double p1);
            //        double.TryParse(tmpDtPie.Rows[0][1].ToString(), out double p2);
            //        double.TryParse(tmpDtPie.Rows[0][2].ToString(), out double p3);
            //        double.TryParse(tmpDtPie.Rows[0][3].ToString(), out double p4);
            //        double.TryParse(tmpDtPie.Rows[0][4].ToString(), out double p5);
            //        double.TryParse(tmpDtPie.Rows[0][5].ToString(), out double p6);
            //        double.TryParse(tmpDtPie.Rows[0][6].ToString(), out double p7);
            //        double.TryParse(tmpDtPie.Rows[0][7].ToString(), out double p8);

            //        var i1 = Math.Round(p1 / macCount, 0);
            //        var i2 = Math.Round(p2 / macCount, 0);
            //        var i3 = Math.Round(p3 / macCount, 0);
            //        var i4 = Math.Round(p4 / macCount, 0);
            //        var i5 = Math.Round(p5 / macCount, 0);
            //        var i6 = Math.Round(p6 / macCount, 0);
            //        var i7 = Math.Round(p7 / macCount, 0);
            //        var i8 = Math.Round(p8 / macCount, 0);

            //        chart1.Series.Clear();
            //        chart1.Legends.Clear();

            //        //Add a new chart-series
            //        chart1.ChartAreas[0].AxisX.IsMarginVisible = false;

            //        string sp = "StopPercentage";
            //        chart1.Series.Add(sp);

            //        // set chart type to "Pie"

            //        chart1.Series[sp].Points.AddXY(i1.ToString() + "%", i1);
            //        chart1.Series[sp].Points.AddXY(i2.ToString() + "%", i2);
            //        chart1.Series[sp].Points.AddXY(i3.ToString() + "%", i3);
            //        chart1.Series[sp].Points.AddXY(i4.ToString() + "%", i4);
            //        chart1.Series[sp].Points.AddXY(i5.ToString() + "%", i5);
            //        chart1.Series[sp].Points.AddXY(i6.ToString() + "%", i6);
            //        chart1.Series[sp].Points.AddXY(i7.ToString() + "%", i7);
            //        chart1.Series[sp].Points.AddXY(i8.ToString() + "%", i8);

            //        // set points colors

            //        for (var i = 0; i <= chart1.Series[sp].Points.Count - 1; i++)
            //            {
            //            chart1.Series[sp].Points[i].Color = glb.GetStopColors()[i];
            //            }

            //        chart1.Series[sp].ChartType =
            //            System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

            //        chart1.Series[sp]["PieLabelStyle"] = "Outside";
            //        chart1.Series[sp]["3DLabelLineSize"] = "120";
            //        /*
            //         * Overlay chart optimization
            //         */

            //        var machines = new StringBuilder();
            //        machines.Append(",");
            //        for (var i = 1; i <= 210; i++)
            //            {
            //            machines.Append(i.ToString() + ",");
            //            }

            //        cmd = new SqlCommand("get_data_in_stop_plus", con);
            //        cmd.CommandType = CommandType.StoredProcedure;

            //        cmd.Parameters.Add("@from_date", SqlDbType.DateTime).Value = MainWnd.Get_from_date();
            //        cmd.Parameters.Add("@to_date", SqlDbType.DateTime).Value = MainWnd.Get_to_date();
            //        cmd.Parameters.Add("@shift", SqlDbType.VarChar).Value = MainWnd.Get_shift_array().ToString();
            //        cmd.Parameters.Add("@machine", SqlDbType.VarChar).Value = machines.ToString();
            //        cmd.Parameters.Add("@machine_range", SqlDbType.Int).Value = 210;

            //        dr = cmd.ExecuteReader();
            //        tmpDtStacked.Load(dr);
            //        dr.Close();
            //        cmd = null;

            //        foreach (System.Windows.Forms.DataVisualization.Charting.Series srs in chart2.Series)
            //            {
            //            srs.Points.Clear();
            //            srs.ToolTip = srs.Name;
            //            }

            //        foreach (DataRow row in tmpDtStacked.Rows)
            //            {
            //            chart2.Series["knitt"].Points.AddXY(row[0].ToString(), row[1].ToString());
            //            chart2.Series["comb"].Points.AddXY(row[0].ToString(), row[2].ToString());
            //            chart2.Series["manual"].Points.AddXY(row[0].ToString(), row[3].ToString());
            //            chart2.Series["yarn"].Points.AddXY(row[0].ToString(), row[4].ToString());
            //            chart2.Series["needle"].Points.AddXY(row[0].ToString(), row[5].ToString());
            //            chart2.Series["shock"].Points.AddXY(row[0].ToString(), row[6].ToString());
            //            chart2.Series["roller"].Points.AddXY(row[0].ToString(), row[7].ToString());
            //            chart2.Series["other"].Points.AddXY(row[0].ToString(), row[8].ToString());
            //            }

            //        for (var i = 0; i <= chart2.Series.Count - 1; i++)
            //            {
            //            chart2.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;
            //            }

            //        chart2.Series["knitt"].Color = glb.GetStopColors()[0];
            //        chart2.Series["comb"].Color = glb.GetStopColors()[1];
            //        chart2.Series["manual"].Color = glb.GetStopColors()[2];
            //        chart2.Series["yarn"].Color = glb.GetStopColors()[3];
            //        chart2.Series["needle"].Color = glb.GetStopColors()[4];
            //        chart2.Series["shock"].Color = glb.GetStopColors()[5];
            //        chart2.Series["roller"].Color = glb.GetStopColors()[6];
            //        chart2.Series["other"].Color = glb.GetStopColors()[7];

            //        con.Close();
            //        LoadingInfo.CloseLoading();
            //        }
            //    }
            //catch
            //    {
            //    LoadingInfo.CloseLoading();
            //    MessageBox.Show("Server error or timeout expiration has occured.");
            //    }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadingInfo.InfoText = "Loading report...";
            LoadingInfo.ShowLoading();
            LoadOeeChart();
            LoadOeeSecondaryChart();
            LoadingInfo.CloseLoading();
        }
    }
    public class ChartElement
    {
        public ChartElement() { }
        public ChartElement(string date, double oeeValue)
        {
            Date = date;
            OeeValue = oeeValue;
        }
        public ChartElement(string date, double oeeValue, double petine, double aghi, double genericStop)
        {
            Date = date;
            OeeValue = oeeValue;
            Petine = petine;
            Aghi = aghi;
            GenericStop = genericStop;
        }
        public string Date { get; set; }
        public double OeeValue { get; set; }
        public double Petine { get; set; }
        public double Aghi { get; set; }
        public double GenericStop { get; set; }
    }
}
