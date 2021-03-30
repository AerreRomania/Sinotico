using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinotico.DatabaseTableClasses
{
    [Table(Name = "intensity_bounds")]
    public class IntensityBounds
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
        private int _value;
        [Column(Storage = "_value")]
        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
    }
}
