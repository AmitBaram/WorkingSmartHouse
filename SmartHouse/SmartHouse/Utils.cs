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
        public static DateTime IntToHour(int hour)
        {
            
            if (hour < 0 || hour > 23)
            {
                throw new ArgumentOutOfRangeException("hour", "Hour must be between 0 and 23.");
            }

           
            return DateTime.Today.AddHours(hour);
        }
        public static DateTime IntHourToDateTime(int hour)
        {
            return DateTime.Today.AddHours(hour);
        }
    }
}
