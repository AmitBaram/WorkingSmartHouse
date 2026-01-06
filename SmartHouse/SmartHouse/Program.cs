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
            // --- 1. SETUP ---
            var jsonHomeDataBase = new JsonHomeDataBase<Boiler>();
            var boilerHandler = new SchedualDeviceHandler<Boiler>(jsonHomeDataBase);

            // Calculate "Now" for the test
            DateTime now = DateTime.Now;
            //DateTime testingTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
            DateTime testingTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0).AddMinutes(5);

            Console.WriteLine($"[Test] Target Time: {testingTime}");

            // --- 2. FIND OR CREATE LOGIC ---
            // Get all existing boilers
            List<Boiler> allBoilers = await jsonHomeDataBase.GetAllItems();

            // Try to find one with the specific name we want
            Boiler targetBoiler = allBoilers.FirstOrDefault(b => b._name == "AutoBoiler");

            if (targetBoiler != null)
            {
                // CASE A: FOUND EXISTING
                Console.WriteLine($"[Info] Found existing boiler: {targetBoiler._id}");

                // We must ensure it is currently OFF to verify the test works
                if (targetBoiler._isOn)
                {
                    Console.WriteLine("[Setup] Device was ON. Resetting to OFF for test...");
                    targetBoiler.TurnOff();
                }

                // Add/Update the schedule to match RIGHT NOW so the check passes
                if (targetBoiler.SchedualTime.ContainsKey(testingTime))
                {
                    targetBoiler.SchedualTime[testingTime] = true; // Ensure it's set to ON
                }
                else
                {
                    targetBoiler.SchedualTime.Add(testingTime, true);
                }

                // Save these setup changes to the DB
                await jsonHomeDataBase.UpdateDB(targetBoiler);
            }
            else
            {
                // CASE B: CREATE NEW (First Run)
                Console.WriteLine("[Info] 'AutoBoiler' not found. Creating new one...");

                Dictionary<DateTime, bool> schedule = new Dictionary<DateTime, bool>();
                schedule.Add(testingTime, true);

                targetBoiler = new Boiler(schedule, false, "AutoBoiler");
                await boilerHandler.AddToDB(targetBoiler);
            }

            // --- 3. EXECUTE CHECK ---
            Console.WriteLine("[Test] Running CheckForSchedual()...");

            await boilerHandler.CheckForSchedual();

            // --- 4. VERIFY ---
            // Re-fetch from DB to confirm the change happened on disk
            var updatedBoiler = await jsonHomeDataBase.GetItemInfo(targetBoiler._id);

            Console.WriteLine("------------------------------------------------");
            if (updatedBoiler._isOn)
            {
                Console.WriteLine($"SUCCESS: The Scheduler found the device{targetBoiler._id} and turned it ON.");
            }
            else
            {
                Console.WriteLine("FAILED: The device remained OFF.");
            }
            Console.WriteLine("------------------------------------------------");

            Console.ReadLine();
        }
    }
}
