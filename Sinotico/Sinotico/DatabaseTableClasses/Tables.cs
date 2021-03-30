using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;

namespace Sinotico.DatabaseTableClasses
{
    public class Tables
    {
        public static Table<Holidays> TblHolidays => FrmHolidays.dc.GetTable<Holidays>();
        public static Table<EfficiencyBounds> TblEfficiencyBounds => FrmHolidays.dc.GetTable<EfficiencyBounds>();
        public static Table<MonthTrash> TblMonthTrash => FrmHolidays.dc.GetTable<MonthTrash>();
        public static Table<IntensityBounds> TblIntensity => FrmHolidays.dc.GetTable<IntensityBounds>();
    }
}
