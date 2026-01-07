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

            // 1. Create the Database 
            // We use ISchedualDevice so it can hold Boilers, ACs, and Lights together.
            var database = new JsonHomeDataBase<ISchedualDevice>();

            // 2. Create the Handler
            var deviceHandler = new SchedualDeviceHandler<ISchedualDevice>(database);

            // 3. Create Services
            var clock = new ClockManager();
            var weatherService = new WeatherAPIHandler(); // Assuming this implements IExternalDataService

            // 4. Initialize the App
            // Injecting all the parts we just created
            App smartHouseApp = new App(clock, deviceHandler, weatherService);

            // 5. Start the System
            // This checks the DB, creates factory devices if empty, and starts the clock
            await smartHouseApp.Start();

            // 6. Keep the Console Open
            // Since the clock runs on a background timer, we need to stop the main thread from exiting.
            Console.WriteLine("\n[System] System is running. Press [Enter] to exit.");
            Console.ReadLine();
        }
    }
}
