using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse
{
    public class App
    {
        // Dependencies
        private readonly ClockManager _clock;
        private readonly SchedualDeviceHandler<IDevice> _deviceHandler;
        private readonly IExternalDataService<WeatherInfo> _externalDataService;



        public App(ClockManager clock, SchedualDeviceHandler<IDevice> deviceHandler, IExternalDataService<WeatherInfo> externalDataService)
        {
            _clock = clock;
            _deviceHandler = deviceHandler;
            _externalDataService = externalDataService;
        }

        public async Task Start()
        {
            
            if ( !_deviceHandler.CheckIfDBExist())
            {
                // FactoryDevices is async, so we wait for it to finish
                await _deviceHandler.FactoryDevices();
            }


            _clock.OnMinuteTick += async (time) =>
            {
                // VISUAL PROOF: Print a dot or message every minute
                Console.WriteLine($"\n[Event] Minute Tick: {time:HH:mm}");

                await _deviceHandler.CheckForSchedual(time);
            };

            // 2. Hour Tick (Auto AC)
            _clock.OnHourTick += async (time) =>
            {
                // VISUAL PROOF: Print message every hour
                Console.WriteLine($"\n[Event] Hour Tick: {time:HH:mm} - Checking AC...");

                await _deviceHandler.AutoControlAC("Haifa");
            };


            _clock.OnStart();
            await ShowMenu();
        }
        public async Task ShowMenu()
        {
            while (true)
            {
                Console.WriteLine("\n================ SMART HOUSE MENU ================");
                Console.WriteLine("1. 🏠 Show All Devices Status");
                Console.WriteLine("2. 💡 Turn Device On/Off");
                Console.WriteLine("3. 📅 Manage Schedule (Add/Update)");
                Console.WriteLine("4. ❌ Remove Schedule");
                Console.WriteLine("5. 🎵 Search Song Info");
                Console.WriteLine("6. 🚪 Exit");
                Console.WriteLine("==================================================");
                Console.Write("Select an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await PrintAllDevices();
                        break;
                    case "2":
                        await ControlDeviceState();
                        break;
                    case "3":
                        await ManageSchedule();
                        break;
                    case "4":
                        await RemoveSchedule();
                        break;
                    case "5":
                        Console.Write("Enter Artist Name: ");
                        string artist = Console.ReadLine();
                        await _deviceHandler.GetInfoFromApi(artist);
                        break;
                    case "6":
                        Console.WriteLine("Exiting Menu...");
                        return; // Exits the method (and the app if loops ends)
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }

        // --- HELPER METHODS FOR MENU ---

        private async Task PrintAllDevices()
        {
            var devices = await _deviceHandler.GetAllDevices();
            Console.WriteLine("\n--- Device Status List ---");
            foreach (var d in devices)
            {
                string status = d._isOn ? "[ON] " : "[OFF]";
                Console.WriteLine($"{status} {d._name} (ID: {d._id})");

                // If it has a schedule, show count
                if (d is ISchedualDevice sd && sd.SchedualTime.Count > 0)
                {
                    Console.WriteLine($"      -> Has {sd.SchedualTime.Count} scheduled events.");
                }
            }
        }

        private async Task ControlDeviceState()
        {
            Console.Write("Enter Device ID: ");
            string id = Console.ReadLine();

            var device = await _deviceHandler.GetDeviceById(id);
            if (device != null)
            {
                Console.WriteLine($"Current State: {(device._isOn ? "ON" : "OFF")}");
                Console.Write("Turn (on/off): ");
                string action = Console.ReadLine().ToLower();

                if (action == "on") device.TurnOn();
                else if (action == "off") device.TurnOff();
                else { Console.WriteLine("Invalid action."); return; }

                await _deviceHandler.UpdateDevice(device);
                Console.WriteLine("Device updated successfully.");
            }
            else
            {
                Console.WriteLine("Device not found.");
            }
        }

        private async Task ManageSchedule()
        {
            Console.Write("Enter Device ID to schedule: ");
            string id = Console.ReadLine();
            // Calls the interactive method you already wrote in SchedualDeviceHandler
            await _deviceHandler.ChangeSchedual(id);
        }

        private async Task RemoveSchedule()
        {
            Console.Write("Enter Device ID: ");
            string id = Console.ReadLine();
            Console.Write("Enter Date/Time to remove (yyyy-MM-dd HH:mm): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime dt))
            {
                try
                {
                    await _deviceHandler.RemoveFromSchedual(id, dt);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid Date format.");
            }
        }



    }

}
