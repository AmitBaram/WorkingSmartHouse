using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse
{
    public class SchedualDeviceHandler<T> : DeviceHandler<T> where T : ISchedualDevice
    {
        
        public SchedualDeviceHandler(IJsonDataBase<T> dataDevice) : base(dataDevice)
        {
        }
        public async Task<Dictionary<DateTime, bool>> GetSchedual(string id)
        {
            ISchedualDevice device = await _itemDB.GetItemInfo(id);

            if (device == null)
                throw new Exception($"Device with id {id} not found");

            return device.SchedualTime;
        }
        public async Task RemoveFromSchedual(string id,DateTime datetime )
        {
            T device = await _itemDB.GetItemInfo(id);

            if (device == null)
            {
                throw new Exception($"Device with id {id} not found");
            }

            // 2. Check if the specific schedule time exists in the dictionary
            if (device.SchedualTime.ContainsKey(datetime))
            {
                // Remove the entry from the dictionary in memory
                device.SchedualTime.Remove(datetime);

                // 3. Save the modified device back to the database
                // This triggers the SaveToFile method in your JsonHomeDataBase
                await _itemDB.UpdateDB(device);
            }
            else
            {
                // Optional: you can choose to throw an error or just do nothing if time isn't found
                throw new Exception($"No schedule found for {datetime} on device {id}");
            }
        }
        public async Task AutoControlAC(WeatherInfo weather, T device)
        {
            
            if (device is AC airConditioner)
            {
                Console.WriteLine($"Current Temp in {weather._cityName}: {weather._temperature}°C");

                bool stateChanged = false;

                
                if (weather._temperature > 30)
                {
                    if (!airConditioner._isOn)
                    {
                        Console.WriteLine("It's very hot. Turning AC ON (16°C).");
                        airConditioner.TurnOn();
                        airConditioner._Temperature = 16;
                        stateChanged = true;
                    }
                }
                
                else if (weather._temperature < 20)
                {
                    if (!airConditioner._isOn)
                    {
                        Console.WriteLine("It's cold. Turning AC ON (30°C).");
                        airConditioner.TurnOn();
                        airConditioner._Temperature = 30;
                        stateChanged = true;
                    }
                }
                
                else
                {
                    if (airConditioner._isOn)
                    {
                        Console.WriteLine("Temperature is comfortable (20-30). Turning AC OFF.");
                        airConditioner.TurnOff();
                        stateChanged = true;
                    }
                }

                
                if (stateChanged)
                {
                    
                    await _itemDB.UpdateDB(device);
                }
            }
            else
            {
                Console.WriteLine("The device provided is not an AC.");
            }
        }

        public async Task CheckForSchedual()
        {
            DateTime now = DateTime.Now;
            
            DateTime currentHourMinute = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);

            
            List<T> devices = await _itemDB.GetAllItems();

            foreach (T d in devices.ToList())
            {
                
                if (d is ISchedualDevice sd)
                {
                    
                    if (sd.SchedualTime != null && sd.SchedualTime.ContainsKey(currentHourMinute))
                    {
                        bool shouldBeOn = sd.SchedualTime[currentHourMinute];

                        
                        if (d is IDevice deviceBase && deviceBase._isOn != shouldBeOn)
                        {
                            if (shouldBeOn)
                            {
                                sd.TurnOn();
                                Console.WriteLine($" schedule found for {now} on device {sd._id}, device is turnd on");

                            }
                                
                                
                            else
                            {
                                sd.TurnOff();
                                Console.WriteLine($"No schedule found for {now} on device {sd._id} device is turnd on");
                            }
                               

                            
                            await _itemDB.UpdateDB(d);
                        }
                    }
                }
            }
        }



    }
}
