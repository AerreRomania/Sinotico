using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

namespace wcfcleaners
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public DataTable GetAlmTable()
        {
            var dt = new DataTable();
            //dt.Columns.Add("Flag", typeof(string));
            dt.Columns.Add("Id", typeof(string));   //0
            dt.Columns.Add("Machine", typeof(string));
            dt.Columns.Add("Shift", typeof(string)); // 2
            dt.Columns.Add("Operator", typeof(string));
            dt.Columns.Add("Start", typeof(string));    //4
            dt.Columns.Add("End", typeof(string));
            dt.Columns.Add("Note", typeof(string)); //6
            dt.Columns.Add("Type", typeof(string));
            dt.Columns.Add("ProgNr", typeof(string)); // 8
            const string q = "select id,machine,shift,operator,startclean,endclean,note,type,prognum from cleaning where alm='true'";
            var tmpdt = new DataTable();
            using (var con = new SqlConnection(Config.ConnString))
            {
                var cmd = new SqlCommand(q, con);
                con.Open();
                var dr = cmd.ExecuteReader();
                tmpdt.Load(dr);
                con.Close();
                dr.Close();
            }
            foreach (DataRow row in tmpdt.Rows)
            {
                //#91c6ff
                var nRow = dt.NewRow();
                //nRow[0] = "Remove";
                nRow[0] = row.ItemArray.GetValue(0).ToString(); //id
                nRow[1] = row.ItemArray.GetValue(1).ToString(); //machine
                nRow[2] = row.ItemArray.GetValue(2).ToString(); //shift
                nRow[3] = row.ItemArray.GetValue(3).ToString(); //operater
                nRow[4] = Convert.ToDateTime(row.ItemArray.GetValue(4)).ToString("dd/MM/yyyy HH:mm");   //startclean
                nRow[5] = Convert.ToDateTime(row.ItemArray.GetValue(5)).ToString("dd/MM/yyyy HH:mm");   //endclean
                nRow[6] = row.ItemArray.GetValue(6).ToString(); //note
                nRow[7] = row.ItemArray.GetValue(7).ToString(); //type
                nRow[8] = row.ItemArray.GetValue(8).ToString(); //prognum
                dt.Rows.Add(nRow);
            }
            return dt;
        }
        public DataTable GetGeneralTable()
        {
            //return string.Format("You entered: {0}", value);
            var dt = new DataTable();
            var todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            const string q = "select * from cleaning where convert(date,eventdate,101)=@param1";
            using (var con = new SqlConnection(Config.ConnString))
            {
                var cmd = new SqlCommand(q, con);
                cmd.Parameters.Add("@param1", SqlDbType.DateTime).Value = todayDate;
                con.Open();
                var dr = cmd.ExecuteReader();
                dt.Load(dr);
                con.Close();
                dr.Close();
            }
            return dt;
        }
        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
        public string GetOperatorName(string code)
        {
            var str = "";
            var q = "select fullname from operators where code='" + code + "'";
            using (var con = new SqlConnection(Config.ConnString))
            {
                var cmd = new SqlCommand(q, con);
                con.Open();
                var dr = cmd.ExecuteReader();
                if (dr.HasRows)
                    while (dr.Read())
                        str = dr[0].ToString();
                con.Close();
                dr.Close();
            }
            return str;
        }
        public int GetProgressiveNumber(DateTime evDate, string cShift, int mac, string operatorx, string type)
        {
            var i = 0;
            var newEventDate = new DateTime(evDate.Year, evDate.Month, evDate.Day);
            var q = "select prognum from cleaning where convert(date,eventdate,101)= '" + newEventDate.Date.ToString("yyyy-MM-dd") + "' " +
                "and shift='" + cShift + "' and machine='" + mac + "' and operator='" + operatorx
                    + "' and type='" + type + "'";
            using (var con = new SqlConnection(Config.ConnString))
            {
                var cmd = new SqlCommand(q, con);
                con.Open();
                var dr = cmd.ExecuteReader();

                if (!dr.HasRows)
                    i = 1;
                else
                    while (dr.Read())
                    {
                        int.TryParse(dr[0].ToString(), out var j);
                        i = j + 1;
                    }
            }
            return i;
        }
        public void InsertNewOperation(DateTime evDate, string cShift, string operatorName, DateTime cStart, DateTime cEnd,
            int machine, string reason, string cType, string note, DateTime dateLoad, int progNum, int tempoMin, string ptPrec)
        {
            var q = "insert into cleaning (eventdate,shift,operator,startclean,endclean,machine,reason,type," +
                             "note,dateload,prognum,tempomin,ptprec)" +
                             " values (@param1,@param2,@param3,@param4,@param5,@param6,@param7," +
                             "@param8,@param9,@param10,@param11,@param12,@param13)";
            using (var con = new SqlConnection(Config.ConnString))
            {
                var cmd = new SqlCommand(q, con);
                cmd.Parameters.Add("@param1", SqlDbType.DateTime).Value = evDate;
                cmd.Parameters.Add("@param2", SqlDbType.NVarChar).Value = cShift;
                cmd.Parameters.Add("@param3", SqlDbType.NVarChar).Value = operatorName;
                cmd.Parameters.Add("@param4", SqlDbType.DateTime).Value = cStart;
                cmd.Parameters.Add("@param5", SqlDbType.DateTime).Value = cEnd;
                cmd.Parameters.Add("@param6", SqlDbType.Int).Value = machine;
                cmd.Parameters.Add("@param7", SqlDbType.NVarChar).Value = reason;
                cmd.Parameters.Add("@param8", SqlDbType.NVarChar).Value = cType;
                cmd.Parameters.Add("@param9", SqlDbType.NVarChar).Value = note;
                cmd.Parameters.Add("@param10", SqlDbType.DateTime).Value = dateLoad;
                cmd.Parameters.Add("@param11", SqlDbType.Int).Value = progNum;
                cmd.Parameters.Add("@param12", SqlDbType.Int).Value = tempoMin;
                cmd.Parameters.Add("@param13", SqlDbType.NVarChar).Value = ptPrec;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public long GetJobId()
        {
            var q = "select max(id) from cleaning";
            long id = 0;
            using (var c = new SqlConnection(Config.ConnString))
            {
                var cmd = new SqlCommand(q, c);
                c.Open();
                var dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        id = Convert.ToInt64(dr[0].ToString());
                    }
                }
                c.Close();
            }
            return id;
        }
        public void UpdateOperation(DateTime end, long id, string note)
        {
            var q = "update cleaning set endclean=@param1,note=@param2 where " +
                    "id=@param3";
            using (var con = new SqlConnection(Config.ConnString))
            {
                var cmd = new SqlCommand(q, con);
                cmd.Parameters.Add("@param1", SqlDbType.DateTime).Value = end;
                cmd.Parameters.Add("@param2", SqlDbType.NVarChar).Value = note;
                cmd.Parameters.Add("@param3", SqlDbType.BigInt).Value = id;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public void UpdateOperationManually(int id, string note)
        {
            const string q = "update cleaning set note=@param1 where id=@param2";
            using (var con = new SqlConnection(Config.ConnString))
            {
                var cmd = new SqlCommand(q, con);

                //end clean
                cmd.Parameters.Add("@param1", SqlDbType.NVarChar).Value = note;
                //note
                cmd.Parameters.Add("@param2", SqlDbType.Int).Value = id;
            }
        }
        public void ActivateAlarm(long id)
        {
            const string q = "update cleaning set alm='true' where id=@param1";

            using (var con = new SqlConnection(Config.ConnString))
            {
                var cmd = new SqlCommand(q, con);
                cmd.Parameters.Add("@param1", SqlDbType.BigInt).Value = id;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public void ActivateAlarm(long id, string note)
        {
            const string q = "update cleaning set alm='true',note=@param1 where id=@param2";

            using (var con = new SqlConnection(Config.ConnString))
            {
                var cmd = new SqlCommand(q, con);
                cmd.Parameters.Add("@param1", SqlDbType.NVarChar).Value = note;
                cmd.Parameters.Add("@param2", SqlDbType.BigInt).Value = id;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public void DeactivateAlarm(long id)
        {
            const string q = "update cleaning set alm='false' where id=@param1";

            using (var con = new SqlConnection(Config.ConnString))
            {
                var cmd = new SqlCommand(q, con);
                cmd.Parameters.Add("@param1", SqlDbType.BigInt).Value = id;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public string GetShift()
        {
            var tmpShift = "";
            // Use to compare with specific time-spans
            var curTime = DateTime.Now.TimeOfDay;
            var nightShiftStart = new TimeSpan(23, 0, 0);
            var nightShiftEnd = new TimeSpan(7, 0, 0);
            var morningShiftStart = new TimeSpan(7, 0, 0);
            var morningShiftEnd = new TimeSpan(15, 0, 0);
            var afternShiftStart = new TimeSpan(15, 0, 0);
            var afternShiftEnd = new TimeSpan(23, 0, 0);
            // Recognize shifts using current times
            if (nightShiftStart > nightShiftEnd && curTime > nightShiftStart || curTime < nightShiftEnd)
                tmpShift = "NIGHT";
            else if (curTime > morningShiftStart && curTime < morningShiftEnd)
                tmpShift = "MORNING";
            else if (curTime > afternShiftStart && curTime < afternShiftEnd)
                tmpShift = "AFTERNOON";
            return tmpShift;
        }
        public bool HasError()
        {
            return false;
        }
    }
}
