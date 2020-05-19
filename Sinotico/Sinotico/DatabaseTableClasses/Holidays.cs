using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinotico.DatabaseTableClasses
{
    [Table(Name = "holidays")]
    public class Holidays
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
        private string _LineName;
        [Column(Storage = "_LineName")]
        public string LineName
        {
            get
            {
                return _LineName;
            }
            set
            {
                _LineName = value;
            }

        }
        private DateTime _Holiday;
        [Column(Storage = "_Holiday")]
        public DateTime Holiday
        {
            get
            {
                return _Holiday;
            }
            set
            {
                _Holiday = value;
            }

        }
    }
}
