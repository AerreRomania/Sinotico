using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;

namespace CsvLibExpress
{
    [Table(Name = "hum_temp")]
    public class hum_temp
    {
        private int _Id;
        [Column(Storage = "_Id", DbType = "Int NOT NULL IDENTITY", CanBeNull = false, IsPrimaryKey = true, IsDbGenerated = true, /*Identity Specification = true,*/ AutoSync = AutoSync.OnInsert)]
        public int Id
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value;
            }

        }
        private string _Mac;
        [Column(Storage = "_Mac")]
        public string Mac
        {
            get
            {
                return _Mac;
            }
            set
            {
                _Mac = value;
            }
        }
        private string _UpdatedAt;
        [Column(Storage = "_UpdatedAt")]
        public string UpdatedAt
        {
            get
            {
                return _UpdatedAt;
            }
            set
            {
                _UpdatedAt = value;
            }
        }
        private string _Type;
        [Column(Storage = "_Type")]
        public string Type
        {
            get
            {
                return _Type;
            }
            set
            {
                _Type = value;
            }
        }
        private string _Temp;
        [Column(Storage = "_Temp")]
        public string Temp
        {
            get
            {
                return _Temp;
            }
            set
            {
                _Temp = value;
            }
        }
        private string _Hum;
        [Column(Storage = "_Hum")]
        public string Hum
        {
            get
            {
                return _Hum;
            }
            set
            {
                _Hum = value;
            }
        }
        private string _Lux;
        [Column(Storage = "_Lux")]
        public string Lux
        {
            get
            {
                return _Lux;
            }
            set
            {
                _Lux = value;
            }
        }
        private string _Rssi;
        [Column(Storage = "_Rssi")]
        public string Rssi
        {
            get
            {
                return _Rssi;
            }
            set
            {
                _Rssi = value;
            }
        }
        private string _Linea;
        [Column(Storage = "_Linea")]
        public string Linea
        {
            get
            {
                return _Linea;
            }
            set
            {
                _Linea = value;
            }
        }
        private string _Zona;
        [Column(Storage = "_Zona")]
        public string Zona
        {
            get
            {
                return _Zona;
            }
            set
            {
                _Zona = value;
            }
        }
        private DateTime _EventDate;
        [Column(Storage = "_EventDate")]
        public DateTime EventDate
        {
            get
            {
                return _EventDate;
            }
            set
            {
                _EventDate = value;
            }
        }
        private string _Turno;
        [Column(Storage = "_Turno")]
        public string Turno
        {
            get
            {
                return _Turno;
            }
            set
            {
                _Turno = value;
            }
        }

    }
}
