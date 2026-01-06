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
            
            Dictionary<DateTime, bool> schedule = new Dictionary<DateTime, bool>();

            // Adding items (Year, Month, Day, Hour, Minute)
            schedule.Add(new DateTime(2026, 1, 6, 8, 0, 0), true);  // Turn ON at 8:00 AM
            schedule.Add(new DateTime(2026, 1, 6, 10, 30, 0), false); // Turn OFF at 10:30 AM

            var jsonHomeDataBase = new JsonHomeDataBase<Boiler>();

            // 2. Initialize the Handler - types now match perfectly
            var boilerHandler = new SchedualDeviceHandler<Boiler>(jsonHomeDataBase);

            // 3. Create and Save your test boiler
            Boiler testBoiler = new Boiler(schedule, false, "TestBoiler");

            // IMPORTANT: Add it to DB first so it exists when you try to remove schedule
            await boilerHandler.AddToDB(testBoiler);

            // 4. Test schedule removal
            await boilerHandler.RemoveFromSchedual(testBoiler._id, new DateTime(2026, 1, 6, 8, 0, 0));

            Console.WriteLine("Process complete without casting errors!");
        }
    }
}
