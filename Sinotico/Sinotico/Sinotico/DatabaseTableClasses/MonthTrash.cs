using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinotico.DatabaseTableClasses
{
    [Table(Name = "month_trash")]
    public class MonthTrash
    {
        private int _Id;
        [Column(Storage = "_Id", DbType = "BIGINT NOT NULL IDENTITY", CanBeNull = false, IsPrimaryKey = true, IsDbGenerated = true)]
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

        private DateTime _Date;
        [Column(Storage = "_Date")]
        public DateTime Date
        {
            get
            {
                return _Date;
            }
            set
            {
                _Date = value;
            }
        }

        private int _Scarti;
        [Column(Storage = "_Scarti")]
        public int Scarti
        {
            get
            {
                return _Scarti;
            }
            set
            {
                _Scarti = value;
            }
        }

        private int _Rammendi;
        [Column(Storage = "_Rammendi")]
        public int Rammendi
        {
            get
            {
                return _Rammendi;
            }
            set
            {
                _Rammendi = value;
            }
        }

        private int _Produtivo;
        [Column(Storage = "_Produtivo")]
        public int Produtivo
        {
            get
            {
                return _Produtivo;
            }
            set
            {
                _Produtivo = value;
            }
        }

        private double _ConsiderateMac;
        [Column(Storage = "_ConsiderateMac")]
        public double ConsiderateMac
        {
            get
            {
                return _ConsiderateMac;
            }
            set
            {
                _ConsiderateMac = value;
            }
        }

        private int _Finezza;
        [Column(Storage = "_Finezza")]
        public int Finezza
        {
            get
            {
                return _Finezza;
            }
            set
            {
                _Finezza = value;
            }
        }

        private int _MancanzaLavoro;
        [Column(Storage = "_MancanzaLavoro")]
        public int MancanzaLavoro
        {
            get
            {
                return _MancanzaLavoro;
            }
            set
            {
                _MancanzaLavoro = value;
            }
        }

        private int _FermataStraordinaria;
        [Column(Storage = "_FermataStraordinaria")]
        public int FermataStraordinaria
        {
            get
            {
                return _FermataStraordinaria;
            }
            set
            {
                _FermataStraordinaria = value;
            }
        }

        private int _CalendarioTEEP;
        [Column(Storage = "_CalendarioTEEP")]
        public int CalendarioTEEP
        {
            get
            {
                return _CalendarioTEEP;
            }
            set
            {
                _CalendarioTEEP = value;
            }
        }
    }
}
