using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace CsvLibExpress
{
   public class CsvRammendi
    {

        private string Path = @"\\192.168.96.7\c$\Report SHIMA\ExportScartiRammendi"; //Knitting
        //private string Path= @"\\192.168.114.6\d$\Report\ExportScartiRammendi"; //Serbia
        Log log = new Log();
        private string GetShift(string shift)
        {
            var str = string.Empty;
            switch (shift)
            {
                case "1":
                    str = "NIGHT";
                    break;
                case "2":
                    str = "MORNING";
                    break;
                case "3":
                    str = "AFTERNOON";
                    break;
            }
            return str;
        }
        public void SendFileToDb()
        {
            var str = new string[] {"id", "data", "turno", "operator_code", "macchina" ,"commessa",
                                    "articolo" ,"teli_buoni" ,"scarti" ,"rammendi" ,"data_scarti","data_rammendi", "filename"};
            var dataTable = new DataTable();
            for (var i = 0; i <= str.Length - 1; i++)
            {
                dataTable.Columns.Add(str[i]);
                if (i == 1 || i == 10 || i == 11) dataTable.Columns[i].DataType = typeof(DateTime);
            }
            var listOfFiles = new List<string>();
            //var con = new SqlConnection("Data Source = 192.168.96.17; Initial Catalog = Sinotico_Serbia; user=sa;password=onlyouolimpias;");
            var con = new SqlConnection("Data Source = 192.168.96.17; Initial Catalog = Sinotico; user=sa;password=onlyouolimpias;");
            var cmd = new SqlCommand("select distinct filename from scarti_rammendi", con);
            con.Open();
            var dr = cmd.ExecuteReader();
            if (dr.HasRows) while (dr.Read()) listOfFiles.Add(dr[0].ToString());
            con.Close();
            cmd = null;

            try
            {
                foreach (var file in Directory.GetFiles(Path))
                {
                    var startTime = DateTime.Now;
                    var getFileName = System.IO.Path.GetFileName(file);
                    if (listOfFiles.Contains(getFileName)) continue;
                    using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        sr.ReadLine();
                        while (!sr.EndOfStream)
                        {
                            string[] rows = sr.ReadLine().Split(',');
                            var newRow = dataTable.NewRow();
                            for (int i = 1; i < str.Length - 1; i++)
                            {
                                var date = new DateTime();
                                if (i == 1 || i == 10 || i == 11)
                                {
                                    DateTime.TryParse(rows[i].ToString(), out date);
                                    if (date == DateTime.MinValue) newRow[i] = DBNull.Value; else newRow[i] = date;
                                }
                                else
                                {
                                    if (i == 2)
                                        newRow[i] = GetShift(rows[i]);
                                    else
                                        newRow[i] = rows[i];
                                }
                            }
                            newRow[12] = getFileName;
                            dataTable.Rows.Add(newRow);
                        }
                    }
                    using (var sbc = new SqlBulkCopy(con))
                    {
                        foreach (DataColumn col in dataTable.Columns)
                        {
                            sbc.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                        }
                        sbc.DestinationTableName = "scarti_rammendi";
                        con.Open();
                        sbc.WriteToServer(dataTable, DataRowState.Added);
                        con.Close();
                        sbc.Close();
                    }
                    var endTime = DateTime.Now;
                    var ms = endTime.Subtract(startTime).TotalMilliseconds;
                    log.WriteLog(message:
                        getFileName
                        + " Status: Accepted; Estimated time: "
                        + ms.ToString() + "ms");
                }

            }
            catch (UnauthorizedAccessException unex)
            {
                log.WriteLog(message: "! Deny " + unex.Message);
            }
            catch (Exception ex)
            {
                log.WriteLog(message: "! Deny " + ex.Message);
            }
        }
    }
}
