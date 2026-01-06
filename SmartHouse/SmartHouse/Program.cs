using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            WeatherAPIHandler weatherAPIHandler = new WeatherAPIHandler();

            // Added 'await' and the missing semicolon ';'
            WeatherInfo i= await weatherAPIHandler.GetBasicData("Haifa");
          List<WeatherInfo> test=  await weatherAPIHandler.GetData("Haifa");
            //foreach (WeatherInfo testInfo in test)
            //{
            //    Console.WriteLine(testInfo._temperature );
            //    Console.WriteLine(testInfo._time);
            //}
            Console.WriteLine(Utils.GetTodayAtSpecificTime(19)); 
            
        }
    }
}
