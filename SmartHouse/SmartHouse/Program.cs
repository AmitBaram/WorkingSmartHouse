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
            Console.Title = "Smart House Control System";
            Console.WriteLine("--- Initializing Smart House System ---");

            // 1. Create the Database (Stores all IDevices: AC, Boiler, Alexa, Light)
            var database = new JsonHomeDataBase<IDevice>();

            // 2. Create the Handler (Logic for devices)
            var deviceHandler = new SchedualDeviceHandler<IDevice>(database);

            // 3. Create Services
            var clock = new ClockManager();
            var weatherService = new WeatherAPIHandler();

            // 4. Initialize the App
            App smartHouseApp = new App(clock, deviceHandler, weatherService);

            // 5. Start the System
            // This will:
            //   a. Check/Create DB
            //   b. Subscribe to Clock Events (Minute/Hour ticks)
            //   c. Start the Clock
            //   d. Launch the interactive Menu loop3
            try
            {
                await smartHouseApp.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CRITICAL ERROR] Application crashed: {ex.Message}");
            }

            // 6. Shutdown
            Console.WriteLine("\n[System] Shutting down...");
        }
    }
}