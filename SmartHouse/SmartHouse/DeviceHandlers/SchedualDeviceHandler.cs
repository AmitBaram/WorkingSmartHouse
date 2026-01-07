using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse
{
    public class SchedualDeviceHandler<T> : DeviceHandler<T> where T : IDevice
    {
        public SchedualDeviceHandler(IDataBase<T> dataDevice) : base(dataDevice)
        {
        }

        public async Task<Dictionary<DateTime, bool>> GetSchedual(string id)
        {
            T device = await _itemDB.GetItemInfo(id);

            if (device == null)
                throw new Exception($"Device with id {id} not found");

            // Check if this device actually supports scheduling
            if (device is ISchedualDevice sd)
            {
                return sd.SchedualTime;
            }
            else
            {
                throw new Exception($"Device {id} does not support scheduling.");
            }
        }

        public async Task RemoveFromSchedual(string id, DateTime datetime)
        {
            T device = await _itemDB.GetItemInfo(id);

            if (device == null)
                throw new Exception($"Device with id {id} not found");

            if (device is ISchedualDevice sd)
            {
                // Normalize time to remove seconds/milliseconds mismatch
                DateTime cleanTime = new DateTime(datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, 0);

                if (sd.SchedualTime.ContainsKey(cleanTime))
                {
                    sd.SchedualTime.Remove(cleanTime);
                    await _itemDB.UpdateDB(device); // Save T back to DB
                    Console.WriteLine($"[Success] Removed schedule for {cleanTime}");
                }
                else
                {
                    throw new Exception($"No schedule found for {cleanTime} on device {id}");
                }
            }
            else
            {
                throw new Exception("This device does not support schedules.");
            }
        }

        

        // This method handles adding/updating a schedule via Console Input
        public async Task ChangeSchedual(string id)
        {
            T device = await _itemDB.GetItemInfo(id);

            if (device == null)
            {
                Console.WriteLine($"[Error] Device {id} not found.");
                return;
            }

            if (device is ISchedualDevice sd)
            {
                Console.WriteLine($"\n--- Set Schedule for {sd._name} ---");
                Console.Write("Enter Date/Time (yyyy-MM-dd HH:mm): ");
                string input = Console.ReadLine();

                if (DateTime.TryParse(input, out DateTime dt))
                {
                    DateTime cleanTime = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);

                    Console.Write("Turn ON? (y/n): ");
                    string stateInput = Console.ReadLine();
                    bool turnOn = (stateInput?.ToLower() == "y");

                    // Update or Add
                    sd.SchedualTime[cleanTime] = turnOn;

                    await _itemDB.UpdateDB(device);
                    Console.WriteLine("[Success] Schedule updated.");
                }
                else
                {
                    Console.WriteLine("[Error] Invalid Date Format.");
                }
            }
            else
            {
                Console.WriteLine($"[Error] Device {device._name} does not support schedules.");
            }
        }

        public async Task CheckForSchedual(DateTime now)
        {
            // Normalize current time to match dictionary keys (00 seconds)
            DateTime currentHourMinute = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);

            List<T> devices = await _itemDB.GetAllItems();

            foreach (T d in devices.ToList())
            {
                // Filter: Is this device capable of having a schedule?
                if (d is ISchedualDevice sd)
                {
                    if (sd.SchedualTime != null && sd.SchedualTime.ContainsKey(currentHourMinute))
                    {
                        bool shouldBeOn = sd.SchedualTime[currentHourMinute];

                        // Logic: Only update DB if the state actually changes
                        // We check 'd._isOn' (from IDevice) vs 'shouldBeOn'
                        if (d._isOn != shouldBeOn)
                        {
                            if (shouldBeOn)
                            {
                                sd.TurnOn();
                                Console.WriteLine($"[Schedule] {currentHourMinute}: Turning ON {sd._name}");
                            }
                            else
                            {
                                sd.TurnOff();
                                Console.WriteLine($"[Schedule] {currentHourMinute}: Turning OFF {sd._name}");
                            }

                            await _itemDB.UpdateDB(d);
                        }
                    }
                }
            }
        }
    }
}

    