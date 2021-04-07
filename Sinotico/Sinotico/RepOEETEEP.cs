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
    public partial class RepOEETEEP : Form
    {
        List<MachinesStops> _stops = new List<MachinesStops>();
        private Font _smallMSSRegular = new Font("Microsoft Sans Serif", 8, FontStyle.Regular);
        private ToolTip _info = new ToolTip();

        public RepOEETEEP()
        {
            InitializeComponent();

            btnGo.Click += btnGo_Click;
            btnInfo.Click += btnInfo_Click;
            CustomizeDataGridView(dgvReport);
            btnParameters.Click += btnParameters_Click;
            btnCharts.Click += btnCharts_Click;
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
            foreach(var h in holidays)
            {
                if (h.Holiday != startDate)
                {
                    counter++;
                }
                startDate = h.Holiday;
            }
            return counter;
        }
        private string Get_Finezza_Array()
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
            if(string.IsNullOrEmpty(cboFin.Text) || cboFin.Text == "<All>")
            {
                return new List<int>() { 3, 7, 14 };
            }
            else
            {
                int.TryParse(cboFin.Text, out var fin);
                return new List<int>() { fin };
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            FilloutFilter(cboFin);
            lblInfo.Visible = false;
            dgvReport.Visible = true;
            lblInfo.BackColor = Color.WhiteSmoke;
            lblInfo.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
            lblInfo.Text = "                                -   Legend  -\n\n" +
                           "    -   Tempo calendario(Tc) = (Data fine - Data inizio + 1) * mac. considerate - Festivita(ferie)\n" +
                           "    -   Tempo possibile(Tp) = Tempo calendario - (Mancanza lavoro ore + Fermata straordinaria ore)\n" +
                           "    -   Tempo disponibile(Td) = Tempo possibile - some tempi fermate\n" +
                           "    -   Tempo Interruzioni Operative = soma tempi fermate(7,8,9,10)\n" +
                           "    -   Tempo Effettivo(Te )= Tempo Disponibile(Td) - soma tempi fermate(7,8,9,10)\n" +
                           "    -   Tempo Produttivo(Tr) = Tempo Effettivo(Te) - Tempo  Interruzioni Operative\n" +
                           "    -   Tempo(VA) Valore Aggiunto -Tempo Produttivo(Tr) - Tempo per scarti e rammendi\n" +
                           "    -   Quality(Tva / Tr) = (Tempo Produtivo - Tempo ramendi e scarti ) / Tempo produtivo\n" +
                           "    -   OEE - Tempo a valore aggiunto = Quality(Tva / Tr) * Efficenza(Tr / Td) * Disponbilità(Td / Tp)\n" +
                           "    -   Total Effective Equipment Performance(TEEP) = OEE * Saturazione(Tp / Tc)";
            base.OnLoad(e);
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

        private void ProcessData()
        {
            if (Get_Date_From() > Get_Date_To())
            {
                MessageBox.Show("Invalid date selection!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            LoadingInfo.InfoText = "Loading report...";
            LoadingInfo.ShowLoading();
            _stops = new List<MachinesStops>();
            LoadProcedure(_stops);

            var table = new DataTable();
            CreateTableView(table);
            
            foreach (var cat in new string[] { "Tempo possibile", "Cambi [ore]", "Pulizia [ore]", "Campionario [ore]", "Cambio aghi [ore]",
                                            "Qualita filato [ore]", "Maintance [ore]", "Rottura filato [ore]", "Pulizia fronture [ore]",
                                            "Interventi meccanici [ore]", "Varie [ore]", "Tempo Produttivo", "Perdita per scarti [ore]",
                                            "Perdita per rammendi [ore]", "Tempo VA", "Tempo disponibile [ore]", "Utilazation - Saturazione %",
                                            "Availability - Disponbilita %", "Efficienza %", "Quality ore %", "Quality teli %",
                                            "", //sep row
                                            "Tempo possibile %", "Cambi %", "Pulizia %", "Campionario %", "Cambio aghi %",
                                            "Qualita filato %", "Varie %", "Rottura filato %", "Pulizia fronture %",
                                            "Interventi meccanici %", "Varie %", "Tempo Produttivo %", "Perdita per scarti %",
                                            "Perdita per rammendi %", "OEE %","TEEP %"})
            {
                var newRow = table.NewRow();
                newRow["Categories"] = cat;
                table.Rows.Add(newRow);
            }

            if (dgvReport.DataSource != null)
            {
                dgvReport.DataSource = null;
            }
            dgvReport.DataSource = table;
            
            double cambi = 0.0, pulizia = 0.0, campionario = 0.0, cambioAghi = 0.0, qualitaFilato = 0.0,
                   varieOne = 0.0, rotturaFilato = 0.0, puliziaFron = 0.0, meccanici = 0.0, varieTwo = 0.0,
                   totalTempoProdutivo = 0.0, totalTempoPossibile = 0.0, totTempoVA = 0.0, totTempoScarti = 0.0, totTempoRammendi = 0.0;

            FrmHolidays.dc = new System.Data.Linq.DataContext(MainWnd.conString);
            var finList = Get_Selected_Finezza();
            for (var c = 1; c < dgvReport.Columns.Count - 2; c++)
            {
                var month = (from s in _stops
                             where s.Date == dgvReport.Columns[c].Name
                             select s).SingleOrDefault();
                if (month == null)
                {
                    continue;
                }
                
                int.TryParse(dgvReport.Columns[c].Name.Split('.')[0], out var y);
                int.TryParse(dgvReport.Columns[c].Name.Split('.')[1], out var m);
                var currentDate = new DateTime(y, m, 1);
                var parameters = (from p in Tables.TblMonthTrash
                                  where p.Date == currentDate
                                  select p).ToList();
                if (parameters == null || parameters.Count <= 0)
                {
                    continue;
                }

                //calculating times for each month and each finezza
                double tempoCalendario = 0.0;
                double tempoPossibile = 0.0;
                double tempoDisponssibile = 0.0;
                double tempoProdutivo = 0.0;
                double tempoScarti = 0.0;
                double tempoRammendi = 0.0;
                double tempoCalendarioTeep = 0.0;
                double difTpoTpro = 0.0;
                double totFermate = 0.0;
                foreach (var uniqueParameters in parameters)
                {
                    if(!finList.Exists(f => f == uniqueParameters.Finezza))
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
                    var tempoIterruzioni = month.RotturaFilatoOre + month.PuliziaFrontureOre + month.MeccaniciOre + month.VarieTOre;
                    var tempoEffetivo = tmpTempoDisponssibile - (month.RotturaFilatoOre + month.PuliziaFrontureOre + month.MeccaniciOre + month.VarieTOre);
                    double.TryParse(uniqueParameters.CalendarioTEEP.ToString(), out var tmpTempoCalendarioTEEP);
                    tempoCalendario += tmpTempoCalendario;
                    tempoCalendarioTeep += tmpTempoCalendarioTEEP * 3600;
                    tempoPossibile += tmpTempoPossibile;
                    tempoDisponssibile += tmpTempoDisponssibile;
                    tempoProdutivo += tmpTempoProdutivo;
                    tempoScarti += tmpTempoScarti;
                    tempoRammendi += tmpTempoRammendi;
                }

                #region firstTablePart
                //caculate varie 
                difTpoTpro =tempoPossibile - tempoProdutivo;
                totFermate = month.CambiOre + month.PuliziaOre + 
                    month.CampionarioOre + month.CambioAghiOre + month.QualitaFilatoOre + 
                    month.MaintanceOre + month.RotturaFilatoOre + month.PuliziaFrontureOre + 
                    month.MeccaniciOre;
                    month.VarieTOre += (difTpoTpro - totFermate);

                dgvReport.Rows[0].Cells[c].Value = ConvertSecondsToHHmm(tempoPossibile);
                totalTempoPossibile += tempoPossibile;
                dgvReport.Rows[1].Cells[c].Value = ConvertSecondsToHHmm(month.CambiOre);
                cambi += month.CambiOre;
                dgvReport.Rows[2].Cells[c].Value = ConvertSecondsToHHmm(month.PuliziaOre);
                pulizia += month.PuliziaOre;
                dgvReport.Rows[3].Cells[c].Value = ConvertSecondsToHHmm(month.CampionarioOre);
                campionario += month.CampionarioOre;
                dgvReport.Rows[4].Cells[c].Value = ConvertSecondsToHHmm(month.CambioAghiOre);
                cambioAghi += month.CambioAghiOre;
                dgvReport.Rows[5].Cells[c].Value = ConvertSecondsToHHmm(month.QualitaFilatoOre);
                qualitaFilato += month.QualitaFilatoOre;
                dgvReport.Rows[6].Cells[c].Value = ConvertSecondsToHHmm(month.MaintanceOre);
                varieOne += month.MaintanceOre;
                dgvReport.Rows[7].Cells[c].Value = ConvertSecondsToHHmm(month.RotturaFilatoOre);
                rotturaFilato += month.RotturaFilatoOre;
                dgvReport.Rows[8].Cells[c].Value = ConvertSecondsToHHmm(month.PuliziaFrontureOre);
                puliziaFron += month.PuliziaFrontureOre;
                dgvReport.Rows[9].Cells[c].Value = ConvertSecondsToHHmm(month.MeccaniciOre);
                meccanici += month.MeccaniciOre;
                dgvReport.Rows[10].Cells[c].Value = ConvertSecondsToHHmm(month.VarieTOre);
                varieTwo += month.VarieTOre;

                dgvReport.Rows[11].Cells[c].Value = ConvertSecondsToHHmm(tempoProdutivo); 
                totalTempoProdutivo += tempoProdutivo;
                dgvReport.Rows[12].Cells[c].Value = ConvertSecondsToHHmm(tempoScarti);
                totTempoScarti += tempoScarti;
                dgvReport.Rows[13].Cells[c].Value = ConvertSecondsToHHmm(tempoRammendi);
                totTempoRammendi += tempoRammendi;
                dgvReport.Rows[14].Cells[c].Value = ConvertSecondsToHHmm(tempoProdutivo - tempoScarti - tempoRammendi);
                totTempoVA += tempoProdutivo - tempoScarti - tempoRammendi;
                var quality = Math.Round(((tempoProdutivo - tempoScarti - tempoRammendi) / tempoProdutivo) * 100, 1);
                var percQualityTeli = Math.Round(((month.TeliScarti + month.TeliRammendi) / month.Teli) * 100, 1); // instead of 1s add teliScarti + teliRammendi
                var qualityTeli = 100 - percQualityTeli;
                dgvReport.Rows[15].Cells[c].Value = ConvertSecondsToHHmm(tempoDisponssibile);
                dgvReport.Rows[16].Cells[c].Value = Math.Round((tempoPossibile / tempoCalendario) * 100, 1);
                dgvReport.Rows[17].Cells[c].Value = Math.Round((tempoDisponssibile / tempoPossibile) * 100, 1);
                dgvReport.Rows[18].Cells[c].Value = Math.Round((tempoProdutivo / tempoDisponssibile) * 100, 1);
                dgvReport.Rows[19].Cells[c].Value = quality;
                dgvReport.Rows[20].Cells[c].Value = qualityTeli;
               
                #endregion firstTablePart
                //row[21] sep row
                #region secondTablePart
                dgvReport.Rows[22].Cells[c].Value = "100";
                dgvReport.Rows[23].Cells[c].Value = Math.Round((month.CambiOre / (tempoPossibile)) * 100, 1);
                dgvReport.Rows[24].Cells[c].Value = Math.Round((month.PuliziaOre / (tempoPossibile)) * 100, 1);
                dgvReport.Rows[25].Cells[c].Value = Math.Round((month.CampionarioOre / (tempoPossibile)) * 100, 1);
                dgvReport.Rows[26].Cells[c].Value = Math.Round((month.CambioAghiOre / (tempoPossibile)) * 100, 1);
                dgvReport.Rows[27].Cells[c].Value = Math.Round((month.QualitaFilatoOre / (tempoPossibile)) * 100, 1);
                dgvReport.Rows[28].Cells[c].Value = Math.Round((month.MaintanceOre / (tempoPossibile)) * 100, 1);
                dgvReport.Rows[29].Cells[c].Value = Math.Round((month.RotturaFilatoOre / (tempoPossibile)) * 100, 1);
                dgvReport.Rows[30].Cells[c].Value = Math.Round((month.PuliziaFrontureOre / (tempoPossibile)) * 100, 1);
                dgvReport.Rows[31].Cells[c].Value = Math.Round((month.MeccaniciOre / (tempoPossibile)) * 100, 1);
                dgvReport.Rows[32].Cells[c].Value = Math.Round((month.VarieTOre / (tempoPossibile)) * 100, 1);
                dgvReport.Rows[33].Cells[c].Value = Math.Round((tempoProdutivo / (tempoPossibile)) * 100, 1);
                dgvReport.Rows[34].Cells[c].Value = Math.Round((totTempoScarti / tempoPossibile) * 100, 1);
                dgvReport.Rows[35].Cells[c].Value = Math.Round((totTempoRammendi / tempoPossibile) * 100, 1);
                var oee = Math.Round(((quality * ((tempoProdutivo / tempoDisponssibile)*100) * ((tempoDisponssibile / tempoPossibile)*100))) / 10_000, 1);
                dgvReport.Rows[36].Cells[c].Value = oee;
                var teep = Math.Round(oee * (((tempoPossibile / tempoCalendarioTeep) * 100) / 100), 1); 
                dgvReport.Rows[37].Cells[c].Value = teep;
                #endregion secondTablePart
            }
            //totals
            #region firstPartTableTotals
            dgvReport.Rows[0].Cells[dgvReport.Columns.Count - 1].Value = ConvertSecondsToHHmm(totalTempoPossibile);
            dgvReport.Rows[1].Cells[dgvReport.Columns.Count - 1].Value = ConvertSecondsToHHmm(cambi);
            dgvReport.Rows[2].Cells[dgvReport.Columns.Count - 1].Value = ConvertSecondsToHHmm(pulizia);
            dgvReport.Rows[3].Cells[dgvReport.Columns.Count - 1].Value = ConvertSecondsToHHmm(campionario);
            dgvReport.Rows[4].Cells[dgvReport.Columns.Count - 1].Value = ConvertSecondsToHHmm(cambioAghi);
            dgvReport.Rows[5].Cells[dgvReport.Columns.Count - 1].Value = ConvertSecondsToHHmm(qualitaFilato);
            dgvReport.Rows[6].Cells[dgvReport.Columns.Count - 1].Value = ConvertSecondsToHHmm(varieOne);
            dgvReport.Rows[7].Cells[dgvReport.Columns.Count - 1].Value = ConvertSecondsToHHmm(rotturaFilato);
            dgvReport.Rows[8].Cells[dgvReport.Columns.Count - 1].Value = ConvertSecondsToHHmm(puliziaFron);
            dgvReport.Rows[9].Cells[dgvReport.Columns.Count - 1].Value = ConvertSecondsToHHmm(meccanici);
            dgvReport.Rows[10].Cells[dgvReport.Columns.Count - 1].Value = ConvertSecondsToHHmm(varieTwo);
            dgvReport.Rows[11].Cells[dgvReport.Columns.Count - 1].Value = ConvertSecondsToHHmm(totalTempoProdutivo);
            dgvReport.Rows[12].Cells[dgvReport.Columns.Count - 1].Value = ConvertSecondsToHHmm(totTempoScarti);
            dgvReport.Rows[13].Cells[dgvReport.Columns.Count - 1].Value = ConvertSecondsToHHmm(totTempoRammendi);
            dgvReport.Rows[14].Cells[dgvReport.Columns.Count - 1].Value = ConvertSecondsToHHmm(totTempoVA);
            #endregion firstPartTableTotals
            #region secondPartTableTotals
            //sep row [21]
            dgvReport.Rows[22].Cells[dgvReport.Columns.Count - 1].Value = "100";
            dgvReport.Rows[23].Cells[dgvReport.Columns.Count - 1].Value = Math.Round((cambi / totalTempoPossibile) * 100, 1);
            dgvReport.Rows[24].Cells[dgvReport.Columns.Count - 1].Value = Math.Round((pulizia / totalTempoPossibile) * 100, 1);
            dgvReport.Rows[25].Cells[dgvReport.Columns.Count - 1].Value = Math.Round((campionario / totalTempoPossibile) * 100, 1);
            dgvReport.Rows[26].Cells[dgvReport.Columns.Count - 1].Value = Math.Round((cambioAghi / totalTempoPossibile) * 100, 1);
            dgvReport.Rows[27].Cells[dgvReport.Columns.Count - 1].Value = Math.Round((qualitaFilato / totalTempoPossibile) * 100, 1);
            dgvReport.Rows[28].Cells[dgvReport.Columns.Count - 1].Value = Math.Round((varieOne / totalTempoPossibile) * 100, 1);
            dgvReport.Rows[29].Cells[dgvReport.Columns.Count - 1].Value = Math.Round((rotturaFilato / totalTempoPossibile) * 100, 1);
            dgvReport.Rows[30].Cells[dgvReport.Columns.Count - 1].Value = Math.Round((puliziaFron / totalTempoPossibile) * 100, 1);
            dgvReport.Rows[31].Cells[dgvReport.Columns.Count - 1].Value = Math.Round((meccanici / totalTempoPossibile) * 100, 1);
            dgvReport.Rows[32].Cells[dgvReport.Columns.Count - 1].Value = Math.Round((varieTwo / totalTempoPossibile) * 100, 1);
            dgvReport.Rows[33].Cells[dgvReport.Columns.Count - 1].Value = Math.Round((totalTempoProdutivo / totalTempoPossibile) * 100, 1);
            dgvReport.Rows[34].Cells[dgvReport.Columns.Count - 1].Value = Math.Round((totTempoScarti / totalTempoPossibile) * 100, 1);
            dgvReport.Rows[35].Cells[dgvReport.Columns.Count - 1].Value = Math.Round((totTempoRammendi / totalTempoPossibile) * 100, 1);
            #endregion secondPartTableTotals

            LoadingInfo.CloseLoading();
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

        private string ConvertSecondsToHHmm(double seconds)
        {
            var hours = Math.Round(seconds / 3600, 2);
            return hours.ToString("N", new System.Globalization.CultureInfo("en-US"));

            //TimeSpan time = TimeSpan.FromSeconds(seconds);
            //var hours = Math.Round(time.TotalHours, 0);
            //var minutes = time.Minutes;
            //string strHours = "", strMinutes = "";
            //if (hours < 10) strHours = "0" + hours.ToString();
            //else strHours = hours.ToString();
            //if (minutes < 10) strMinutes = "0" + minutes.ToString();
            //else strMinutes = minutes.ToString();
            //return string.Format("{0}:{1}", strHours, strMinutes);
        }

        private void LoadProcedure(List<MachinesStops> lstStops)
        {
            var ds = new DataSet();
            var cmd = new System.Data.SqlClient.SqlCommand()
            {
                CommandText = "get_OEE_rep_data",
                Connection = MainWnd._sql_con,
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 99999999
            };
            cmd.Parameters.Add("@from_date", SqlDbType.DateTime).Value = Get_Date_From();
            cmd.Parameters.Add("@to_date", SqlDbType.DateTime).Value = Get_Date_To();
            cmd.Parameters.Add("@finesse", SqlDbType.VarChar).Value = Get_Finezza_Array();
            cmd.Parameters.Add("@table", SqlDbType.VarChar).Value = MainWnd.GetTableSource();
            MainWnd._sql_con.Open();
            var da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count >= 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
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
                    double.TryParse(dr[26].ToString(), out var teli);

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
                    newStop.Teli = teli;
                    lstStops.Add(newStop);
                }
            }
            //calculate scarti rammendi
            if(ds.Tables[1].Rows.Count >= 1)
            {
                DateTime.TryParse(ds.Tables[1].Rows[0][0].ToString(), out var startDate);
                var startShift = ds.Tables[1].Rows[0][1].ToString();
                int.TryParse(ds.Tables[1].Rows[0][2].ToString(), out var startOperatorCode);
                int.TryParse(ds.Tables[1].Rows[0][3].ToString(), out var startMachine);
                var startOrder = ds.Tables[1].Rows[0][4].ToString();
                var startArticle = ds.Tables[1].Rows[0][5].ToString();
                int.TryParse(ds.Tables[1].Rows[0][6].ToString(), out var startTrash);
                int.TryParse(ds.Tables[1].Rows[0][7].ToString(), out var startRepair);
                var sumTrash = 0;
                var sumRepair = 0;
                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    DateTime.TryParse(row[0].ToString(), out var date);
                    var shift = row[1].ToString();
                    int.TryParse(row[2].ToString(), out var operatorCode);
                    int.TryParse(row[3].ToString(), out var machine);
                    var order = row[4].ToString();
                    var article = row[5].ToString();
                    int.TryParse(row[6].ToString(), out var trash);
                    int.TryParse(row[7].ToString(), out var repair);
                    if (date.Month != startDate.Month)
                    {
                        sumTrash += startTrash;
                        sumRepair += startRepair;
                        var m = string.Empty;
                        var month = startDate.Month;
                        if (month < 10) m = "0" + month.ToString();
                        else m = month.ToString();
                        var dt = startDate.Year.ToString() + "." + m;

                        var component = (from c in lstStops
                                         where c.Date == dt
                                         select c).SingleOrDefault();
                        if (component != null)
                        {
                            double.TryParse(sumTrash.ToString(), out var t);
                            double.TryParse(sumRepair.ToString(), out var r);
                            component.TeliScarti = t;
                            component.TeliRammendi = r;
                        }
                        sumTrash = 0;
                        sumRepair = 0;
                        startDate = date;
                    }
                    if (machine != startMachine || shift != startShift || operatorCode != startOperatorCode
                       || order != startOrder || article != startArticle || date.Day != startDate.Day)
                    {
                        sumTrash += startTrash;
                        sumRepair += startRepair;
                    }
                    startDate = date;
                    startShift = shift;
                    startOperatorCode = operatorCode;
                    startMachine = machine;
                    startOrder = order;
                    startArticle = article;
                    startTrash = trash;
                    startRepair = repair;
                }
                //last record
                sumTrash += startTrash;
                sumRepair += startRepair;
                var mo = string.Empty;
                var tmpMonth = startDate.Month;
                if (tmpMonth < 10) mo = "0" + tmpMonth.ToString();
                else mo = tmpMonth.ToString();
                var tmpDate = startDate.Year.ToString() + "." + mo;
                var cmp = (from c in lstStops
                                 where c.Date == tmpDate
                           select c).SingleOrDefault();
                if (cmp != null)
                {
                    double.TryParse(sumTrash.ToString(), out var t);
                    double.TryParse(sumRepair.ToString(), out var r);
                    cmp.TeliScarti = t;
                    cmp.TeliRammendi = r;
                }
            }
            
            MainWnd._sql_con.Close();
            da.Dispose();
            cmd = null;
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            ProcessData();
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if(dgvReport.Visible)
            {
                btn.BackColor = Color.FromArgb(230, 230, 230);
                dgvReport.Dock = DockStyle.None;
                lblInfo.Dock = DockStyle.Fill;
                dgvReport.Visible = false;
                lblInfo.Visible = true;
            }
            else
            {
                btn.BackColor = Color.WhiteSmoke;
                lblInfo.Dock = DockStyle.None;
                dgvReport.Dock = DockStyle.Fill;                
                dgvReport.Visible = true;
                lblInfo.Visible = false;
            }
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
                dgv.EnableHeadersVisualStyles = false;
                dgv.Columns[0].HeaderText = "Sintesi OEE+";
                dgv.Columns[0].Width = 150;
                dgv.Columns[dgv.Columns.Count - 1].HeaderText = "Totale";
                dgv.Columns["Separator1"].Width = 10;
                dgv.Columns["Separator1"].HeaderText = string.Empty;
                dgv.Columns["Separator1"].DefaultCellStyle.BackColor = Color.White;
                dgv.Columns["Separator1"].DefaultCellStyle.SelectionBackColor = Color.White;
                dgv.Columns["Separator1"].DefaultCellStyle.SelectionForeColor = Color.White;
                dgv.Columns["Separator1"].HeaderCell.Style.BackColor = Color.White;
                dgv.Rows[21].Height = 10;
                dgv.Rows[21].DefaultCellStyle.BackColor = Color.White;
                dgv.Rows[21].DefaultCellStyle.SelectionBackColor = Color.White;
                dgv.Rows[21].DefaultCellStyle.SelectionForeColor = Color.White;
                dgv.Rows[dgv.Rows.Count - 1].DefaultCellStyle.BackColor = Color.FromArgb(112, 173, 71);
                dgv.Rows[dgv.Rows.Count - 2].DefaultCellStyle.BackColor = Color.FromArgb(112, 173, 71);
                dgv.Rows[dgv.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.White;
                dgv.Rows[dgv.Rows.Count - 2].DefaultCellStyle.ForeColor = Color.White;
                for (var i = 0; i < dgv.Columns.Count; i++)
                {
                    if (dgv.Columns[i].Name == "Separator1") continue;
                    dgv.Rows[0].Cells[i].Style.BackColor = Color.FromArgb(48, 84, 150);
                    dgv.Rows[14].Cells[i].Style.BackColor = Color.FromArgb(48, 84, 150);
                    dgv.Rows[22].Cells[i].Style.BackColor = Color.FromArgb(198, 89, 17);
                    dgv.Rows[21].Cells[i].Style.BackColor = Color.WhiteSmoke;
                    dgv.Rows[20].Cells[i].Style.BackColor = Color.WhiteSmoke;
                }
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                    if (col.Name == "Separator1") continue;
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    col.HeaderCell.Style.BackColor = Color.White;//Color.FromArgb(48, 84, 150);
                    col.HeaderCell.Style.ForeColor = Color.Black;
                    col.HeaderCell.Style.Font = _smallMSSRegular;
                    col.DefaultCellStyle.Font = _smallMSSRegular;
                }
                for (var r = 1; r < dgv.Rows.Count - 2; r++)
                {
                    if (r == 14 || r == 21 || r == 22) continue;
                    for (var c = 0; c < dgv.Columns.Count; c++)
                    {
                        if (c == dgv.Columns["Separator1"].Index) continue;
                        dgv.Rows[r].Cells[c].Style.BackColor = Color.White;//Color.FromArgb(255, 255, 203);
                        dgv.Rows[0].Cells[c].Style.ForeColor = Color.White;
                        dgv.Rows[14].Cells[c].Style.ForeColor = Color.White;
                        dgv.Rows[22].Cells[c].Style.ForeColor = Color.White;
                        //dgv.Rows[14].Cells[c].Style.ForeColor = Color.White;
                    }  
                    if (r >= 1 && r <= 13)
                        dgv.Rows[r].Cells[0].Style.ForeColor = Color.FromArgb(48, 84, 150);
                    if (r >= 22 && r <= 35)
                        dgv.Rows[r].Cells[0].Style.ForeColor = Color.FromArgb(198, 89, 17);
                }
            };
        }

        private void dgvReport_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            var dgv = sender as DataGridView;
            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                System.Drawing.Rectangle r2 = e.CellBounds;
                r2.Y += e.CellBounds.Height / 2;
                r2.Height = e.CellBounds.Height / 2;
                e.PaintBackground(r2, true);
                e.PaintContent(r2);
                e.Handled = true;
            }
            if (e.ColumnIndex == dgv.Columns.Count - 2)
            {
                var r = e.CellBounds;
                e.Graphics.FillRectangle(new SolidBrush(dgv.BackgroundColor), new RectangleF(r.X - 1, r.Y, r.Width + 1, r.Height));
                e.Handled = true;
            }
        }

        private void btnParameters_Click(object sender, EventArgs e)
        {
            var frmParameters = new MonthScartiRammendi();
            frmParameters.WindowState = FormWindowState.Maximized;
            frmParameters.ShowDialog();
            frmParameters.Dispose();
        }

        private void btnCharts_Click(object sender, EventArgs args)
        {
            var frmCharts = new RepCharts();
            frmCharts.ShowDialog();
            frmCharts.Dispose();
        }
    }
    public class MachinesStops
    {
        public MachinesStops() { }
        public string Date { get; set; } 
        public double CambiOre { get; set; }
        public double PuliziaOre { get; set; }
        public double CampionarioOre { get; set; }
        public double CambioAghiOre { get; set; }
        public double QualitaFilatoOre { get; set; }
        public double MaintanceOre { get; set; }
        public double RotturaFilatoOre { get; set; }
        public double PuliziaFrontureOre { get; set; }
        public double MeccaniciOre { get; set; }
        public double VarieTOre { get; set; }
        public double TempoProduttivo { get; set; }
        public double PerditaScartiOre { get; set; }
        public double PerditaRammendiOre { get; set; }
        public double TempoVA { get; set; }
        public double Teli { get; set; }
        public double TeliScarti { get; set; }
        public double TeliRammendi { get; set; }
    }
}
