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
            // --- 1. SETUP: Create the Database & Handler ---
            var jsonHomeDataBase = new JsonHomeDataBase<Boiler>();
            var boilerHandler = new SchedualDeviceHandler<Boiler>(jsonHomeDataBase);

            // --- 2. PREPARE TEST DATA: Schedule for RIGHT NOW ---
            // We get the current time and strip the seconds (to match your logic)
            DateTime now = DateTime.Now;
            DateTime testingTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);

            Console.WriteLine($"[Test] Setting schedule for: {testingTime}");

            Dictionary<DateTime, bool> schedule = new Dictionary<DateTime, bool>();

            // Add a rule: Turn ON (true) at this exact minute
            schedule.Add(testingTime, true);

            // --- 3. CREATE DEVICE ---
            // We start with isOn = false so we can see it change to true
            Boiler testBoiler = new Boiler(schedule, false, "AutoBoiler");

            // Add to DB (Wait for it to finish)
            await boilerHandler.AddToDB(testBoiler);

            Console.WriteLine($"[Test] Device '{testBoiler._name}' created. IsOn: {testBoiler._isOn}");

            // --- 4. EXECUTE THE CHECK ---
            Console.WriteLine("[Test] Running CheckForSchedual()...");

            // This method should find the boiler, see the time matches, turn it ON, and save.
            await boilerHandler.CheckForSchedual();

            // --- 5. VERIFY RESULT ---
            // Fetch the latest version from the DB to be sure
            var updatedBoiler = await jsonHomeDataBase.GetItemInfo(testBoiler._id);

            Console.WriteLine("------------------------------------------------");
            if (updatedBoiler._isOn)
            {
                Console.WriteLine("SUCCESS: The Scheduler turned the Boiler ON!");
            }
            else
            {
                Console.WriteLine("FAILED: The Boiler is still OFF.");
            }
            Console.WriteLine("------------------------------------------------");

            // Prevent console from closing immediately
            Console.ReadLine();
        }
    }
}
