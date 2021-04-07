using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;

namespace CsvLibExpress
{
    public class CsvLib
    {
        public static readonly int DefaultMonths = 3;
        public static readonly string DefaultTable = "csv_extend";
        /// <summary>
        /// The original directory of SPR3
        /// </summary>
        private string _originalStorePath = "\\\\192.168.96.7\\c$\\Report SHIMA"; //olimpias knitting
        //private string _originalStorePath = "\\\\192.168.114.6\\d$\\Report"; // olimpias serbia
        //\\192.168.96.7\c$\Report SHIMA 
        //\\192.168.96.7\\c$\\SPR3-PLM\\SPR3_REPORT
        /// <summary>
        /// Startup directory of the service store
        /// </summary>
        private string _myStorePath = AppDomain.CurrentDomain.BaseDirectory + "csv_store"; //= "D:\\Sinotico\\extractor\\csv_store";
        /// <summary>
        /// The target prefix of the target file
        /// </summary>
        private readonly string _targetPrefix = "PR";  //old uspx: PW       
        /// <summary>
        /// File that will be extracted.
        /// </summary>
        private string MyFile;
        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>
        /// Connection string.
        /// </value>
        //public string Context { get; set; } = "Data Source=KNSQL2014;Initial Catalog=Sinotico_Serbia;Integrated Security=SSPI";
        public string Context { get; set; } = "Data Source=KNSQL2014;Initial Catalog=Sinotico;Integrated Security=SSPI";
        private readonly Log log = new Log();
        /// <summary>
        /// Populates the data table using CSV format.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="dataTable">The datatable.</param>
        private void PopulateTableUsingCsv(string path, DataTable dataTable)
        {
            // clear datatable
            if (dataTable.Rows.Count > 0) dataTable.Rows.Clear();
            if (dataTable.Columns.Count > 0) dataTable.Columns.Clear();
            dataTable.Clear();

            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            //here only read
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    // Skip first 3 unnecessary lines
                    sr.ReadLine();
                    sr.ReadLine();
                    sr.ReadLine();
                    string[] headers = sr.ReadLine().Split(',');
                    // Creates datatable columns from CSV file
                    foreach (string header in headers)
                        dataTable.Columns.Add(header);
                    //extra columns upgraded to original
                    dataTable.Columns.Add("part");
                    dataTable.Columns.Add("size");
                    dataTable.Columns.Add("color");
                    dataTable.Columns.Add("filenamefull");
                    while (!sr.EndOfStream)
                    // Add rows to datatable from CSV file
                    {
                        string[] rows = sr.ReadLine().Split(',');
                        var newRow = dataTable.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            if (i == 17)
                                newRow[17] = GetShiftName(rows[17].ToString());
                            else if (i == 24)
                                newRow[24] = GetFilename(rows[24].ToString(), 3, false);
                            else
                                //insert original values in string
                                newRow[i] = rows[i];
                        }
                        //add parts of the 'Filename' column by pattern
                        var fn = rows[24].ToString();
                        newRow["part"] = GetFilename(fn, 0, false);
                        newRow["size"] = GetFilename(fn, 1, false);
                        newRow["color"] = GetFilename(fn, 2, false);
                        newRow["filenamefull"] = GetFilename(fn, -1, true);
                        dataTable.Rows.Add(newRow);
                    }
                }
            }
        }
        private void TryToCreateClonedStore()
        {
            // Creates directory for CSV files
            if (!Directory.Exists(_myStorePath))
                Directory.CreateDirectory(_myStorePath);
        }
        public void ExtractMyFile()
        {
            TryToCreateClonedStore();   //TODO -> Check memory and time usage to perform this method every time
            try
            {
                foreach (var file in Directory.GetFiles(_originalStorePath))
                // Loop trough original store to find file with target prefix
                {
                    var csvDataTable = new DataTable(); //upgrading data
                    var csvTmpTable = new DataTable();  //temporary data
                    //file.Split('\\').Last();
                    var originalFileName = Path.GetFileName(file); //Gets the only file name and extension
                    if (originalFileName.Substring(0, 2) != _targetPrefix) continue;
                    if (string.IsNullOrEmpty(originalFileName) || originalFileName == string.Empty) continue;
                    MyFile = Path.Combine(_myStorePath, originalFileName);
                    // Gets the file name
                    var myFileName = Path.GetFileName(MyFile); //MyFile.Split('\\').Last();
                    if (File.Exists(MyFile)) continue;
                    var sourceFileName = Path.Combine(_originalStorePath, originalFileName);
                    File.Copy(sourceFileName, MyFile, true);
                    var startTime = DateTime.Now;
                    var con = new SqlConnection(Context);
                    var cmd = new SqlCommand();
                    // Gets the file creation date
                    var myFileLastWrite = File.GetLastWriteTime(MyFile);
                    if (con.State == ConnectionState.Open) con.Close();
                    var query = "insert into csv_files " +
                        "([file_id],[file],[file_name],[file_import_date_time],[file_last_write_date_time]) " +
                        "values (@param1, @param2, @param3, @param4, @param5)";
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@param1",
                        SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();  //id
                    cmd.Parameters.AddWithValue("@param2",
                        SqlDbType.VarBinary).Value = File.ReadAllBytes(MyFile);  //file
                    cmd.Parameters.AddWithValue("@param3",
                        SqlDbType.VarChar).Value = myFileName;   //file_name
                    cmd.Parameters.AddWithValue("@param4",
                        SqlDbType.DateTime).Value = DateTime.Now;    //post datetime
                    cmd.Parameters.AddWithValue("@param5",
                        SqlDbType.DateTime).Value = myFileLastWrite;   //file creation datetime
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    cmd = null;
                    log.WriteLog(message: myFileName + " Status: Sent over filestream");
                    //
                    // Copy datatable content to database table
                    //
                    csvDataTable = new DataTable();
                    PopulateTableUsingCsv(MyFile, csvDataTable);
                    using (var sbc = new SqlBulkCopy(con))
                    {
                        sbc.DestinationTableName = "csv_history";
                        con.Open();
                        sbc.WriteToServer(csvDataTable, DataRowState.Added);
                        con.Close();
                        sbc.Close();
                    }
                    using (var sbc = new SqlBulkCopy(con))
                    {
                        sbc.DestinationTableName = "csv_extend";
                        con.Open();
                        sbc.WriteToServer(csvDataTable, DataRowState.Added);
                        con.Close();
                        sbc.Close();
                    }
                    using (var sbc=new SqlBulkCopy(con))
                    {
                        sbc.DestinationTableName = "csv_currentyear";
                        con.Open();
                        sbc.WriteToServer(csvDataTable, DataRowState.Added);
                        con.Close();
                        sbc.Close();
                    }
                    //
                    // Copies last readed file into db temp_table
                    //
                    PopulateTableUsingCsv(MyFile, csvTmpTable);
                    cmd = new SqlCommand("delete from csv_temporary", con);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    cmd = null;
                    using (var sbc = new SqlBulkCopy(con))
                    {
                        sbc.DestinationTableName = "csv_temporary";
                        con.Open();
                        sbc.WriteToServer(csvTmpTable, DataRowState.Added);
                        con.Close();
                        sbc.Close();
                    }
                    // Writes last extracted file status
                    var endTime = DateTime.Now;
                    var ms = endTime.Subtract(startTime).TotalMilliseconds;
                    log.WriteLog(message:
                        myFileName
                        + " Status: Accepted; Estimated time: "
                        + ms.ToString() + "ms");

                    /*Now extract scarti & rammendi csv file*/

                    //var endTimeRamm = DateTime.Now;
                    //ms = endTimeRamm.Subtract(endTime).TotalMilliseconds;
                    //var csvRamm = new CsvRammendi();
                    //csvRamm.SendFileToDb();
                    //log.WriteLog(message: "Status: Scarti-rammendi accepted; Estimated time: " + 
                    //    ms.ToString() + "ms");

                    //var _HumidityTemperature = new TemperatureExtractor();
                    //_HumidityTemperature.InsertNewRecords();
                }

                EraseDataFromDatabase(DefaultTable, DefaultMonths, Context);
                EraseLastYear("csv_currentyear", Context);
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

        /* Erasing data from given table name in past older then months parameter */
        private void EraseDataFromDatabase(string table, int months, string context)
        {
            string query = $"DELETE FROM {table} WHERE CAST([Data Output Time] AS DATETIME) < DATEADD(MM,-{months},GETDATE())";

            using (var con = new SqlConnection(context))
            {
                var cmd = new SqlCommand(query, con)
                {
                    CommandType = CommandType.Text,
                    CommandTimeout= 300
                };
                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                log.WriteLog(message: $"Rows deleted from {table}: {rowsAffected}");
                con.Close();
            }
        }
        private void EraseLastYear(string table, string context)
        {
            string query = $"DELETE FROM {table} where DATEPART(YEAR,CAST([Data Output Time] as date)) != DATEPART(year, getdate())";

            using (var con = new SqlConnection(context))
            {
                var cmd = new SqlCommand(query, con)
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 300
                };
                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                log.WriteLog(message: $"Rows deleted from {table}: {rowsAffected}");
                con.Close();
            }
        }

        #region WorkWithFileName
        private bool CompareFileNameFormat(string txt)
        {
            var reg = "(.*?)(-)(.*?)(-)(.*?)(-)(.*?)(\\.)(.)(.)(.)";
            var r = new Regex(reg,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var m = r.Match(txt);
            if (m.Success)
                return true;
            else
                return false;
        }
        private string GetFilename(string text, int seq, bool showError)
        {
            var formatCorrect = CompareFileNameFormat(text);

            if (!formatCorrect)
            {
                if (showError)
                    return text;
                else
                    return "errform";
            }
            else
            {
                if (seq == -1)
                    return text;
                else if (seq == 3)
                    return text.Split('-')[seq].Split('.')[0];
                else
                    return text.Split('-')[seq];
            }
        }
        public string GetShiftName(string shift)
        {
            var str = "";
            switch (shift)
            {
                case "TURNO1":
                    str = "NIGHT";
                    break;
                case "TURNO2":
                    str = "MORNING";
                    break;
                case "TURNO3":
                    str = "AFTERNOON";
                    break;
            }
            return str;
        }
    }

    #endregion WorkWithFileName
}