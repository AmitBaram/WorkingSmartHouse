using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse
{
    public static  class Utils
    {
        public static  DateTime GetTodayAtSpecificTime(int hour)
        {
            return DateTime.Today + new TimeSpan(hour, 0, 0);
        }

    }
}
