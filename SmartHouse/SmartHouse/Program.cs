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
            Console.WriteLine("--- Initializing Smart House System ---");

            // 1. Create Database & Handler
            var database = new JsonHomeDataBase<IDevice>();
            var deviceHandler = new SchedualDeviceHandler<IDevice>(database);

            // 2. Services
            var clock = new ClockManager();
            var weatherService = new WeatherAPIHandler();

            // --- VERIFICATION STEP: Print what is currently in the JSON ---
            Console.WriteLine("\n[System] Loading devices from JSON...");
            List<IDevice> currentDevices = await database.GetAllItems();

            if (currentDevices.Count > 0)
            {
                Console.WriteLine($"[System] Found {currentDevices.Count} devices in DB:");
                foreach (var device in currentDevices)
                {
                    Console.WriteLine($"   - Name: {device._name,-15} | ID: {device._id,-10} | Type: {device.GetType().Name}");
                }
            }
            else
            {
                Console.WriteLine("[System] Database is empty. Factory devices will be created.");
            }
            Console.WriteLine("--------------------------------------------\n");

            // --- MANUAL CHECK: Only add AC if it's missing ---
            // We check if "AC" is already in the list we just loaded
            bool acExists = currentDevices.Any(d => d._name == "AC" || d is AC);

            if (!acExists)
            {
                Console.WriteLine("[Program] AC missing. Adding manual AC...");
                try
                {
                    AC livingRoomAc = new AC(false, "AC", 24);
                    // Use a specific ID if you want to be sure
                    livingRoomAc._id = "AC_MANUAL_01";
                    await deviceHandler.AddToDB(livingRoomAc);
                    Console.WriteLine($"[Success] Added {livingRoomAc._name} to database.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Info] Could not add AC: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("[Program] AC already exists in JSON. Skipping manual add.");
            }

            // 3. Initialize & Start App
            App smartHouseApp = new App(clock, deviceHandler, weatherService);
            await smartHouseApp.Start();

            Console.WriteLine("\n[System] System is running. Press [Enter] to exit.");
            Console.ReadLine();
        }
    }
}
