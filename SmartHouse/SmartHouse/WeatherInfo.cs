using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse
{
    public class WeatherInfo
    {
        string _cityName {  get; set; }
        DateTime _time {  get; set; }
        int _temperature { get; set; }

        public WeatherInfo(string cityname, DateTime time, int temperature) 
            {
                this._cityName =cityname;
                this._time = time;
                this._temperature = temperature;
            }
    }
}
