using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Data;
using Newtonsoft.Json;
using System.Data.Linq;

namespace CsvLibExpress
{

    public class Rootobject
    {
        public bool success { get; set; }
        public int count { get; set; }
        public Sensor[] sensors { get; set; }
    }

    public class Sensor
    {
        public string mac { get; set; }
        public long? updatedAt { get; set; }
        public string type { get; set; }
        public float? temp { get; set; }
        public int? hum { get; set; }
        public int? lux { get; set; }
        public int? rssi { get; set; }
        public string linea { get; set; }
        public string zona { get; set; }
    }
    
    //public class TemperatureExtractor
    //{
    //    private Rootobject _root;
    //    public static DataContext dc;
    //    public static string Context = "Data Source=KNSQL2014;Initial Catalog=Sinotico;Integrated Security=SSPI";

    //    public TemperatureExtractor()
    //    {
    //        _root = ExtractJson();
    //        dc = new DataContext(Context);
    //    }

    //    public Rootobject GetRoot()
    //    {
    //        return _root;
    //    }

    //    public Rootobject ExtractJson()
    //    {
    //        var json = new WebClient().DownloadString("http://127.0.0.1:5000/sensors");
    //        var jsonList = JsonConvert.DeserializeObject<Rootobject>(json);
    //        return jsonList;
    //    }
    //    public void InsertNewRecords()
    //    {
    //        var sensorList = (from s in GetRoot().sensors
    //                          where s.zona == "Tessitura"
    //                          select s).ToList();
    //        foreach (var item in sensorList)
    //        {
    //            hum_temp newRecord = new hum_temp();
    //            newRecord.Mac = item.mac;
    //            newRecord.UpdatedAt = item.updatedAt.ToString();
    //            newRecord.Type = item.type;
    //            newRecord.Temp = item.temp.ToString();
    //            newRecord.Hum = item.hum.ToString();
    //            newRecord.Lux = item.lux.ToString();
    //            newRecord.Rssi = item.rssi.ToString();
    //            newRecord.Linea = item.linea;
    //            newRecord.Zona = item.zona;
    //            newRecord.EventDate = DateTime.Now;
    //            newRecord.Turno = GetShift();
    //            Tables.TblSensors.InsertOnSubmit(newRecord);
    //        }
    //        dc.SubmitChanges();
    //    }
    //    string GetShift()
    //    {
    //        var tmpShift = "";
    //        // Use to compare with specific time-spans
    //        var curTime = DateTime.Now.TimeOfDay;
    //        var nightShiftStart = new TimeSpan(23, 0, 0);
    //        var nightShiftEnd = new TimeSpan(7, 0, 0);
    //        var morningShiftStart = new TimeSpan(7, 0, 0);
    //        var morningShiftEnd = new TimeSpan(15, 0, 0);
    //        var afternShiftStart = new TimeSpan(15, 0, 0);
    //        var afternShiftEnd = new TimeSpan(23, 0, 0);
    //        // Recognize shifts using current times
    //        if (nightShiftStart > nightShiftEnd && curTime > nightShiftStart || curTime < nightShiftEnd)
    //            tmpShift = "NIGHT";
    //        else if (curTime > morningShiftStart && curTime < morningShiftEnd)
    //            tmpShift = "MORNING";
    //        else if (curTime > afternShiftStart && curTime < afternShiftEnd)
    //            tmpShift = "AFTERNOON";
    //        return tmpShift;
    //    }
    //}
}
