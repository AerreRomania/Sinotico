using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinotico.DatabaseTableClasses
{
    [Table(Name = "efficiency_bounds")]
    public class EfficiencyBounds
    {
        private int _Id;
        [Column(Storage = "_Id", DbType = "INT NOT NULL IDENTITY", CanBeNull = false, IsPrimaryKey = true, IsDbGenerated = true)]
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
        private string _Red;
        [Column(Storage = "_Red")]
        public string Red
        {
            get
            {
                return _Red;
            }
            set
            {
                _Red = value;
            }
        }
        private string _Yellow;
        [Column(Storage = "_Yellow")]
        public string Yellow
        {
            get
            {
                return _Yellow;
            }
            set
            {
                _Yellow = value;
            }
        }
        private string _Green;
        [Column(Storage = "_Green")]
        public string Green
        {
            get
            {
                return _Green;
            }
            set
            {
                _Green = value;
            }
        }
    }
}
