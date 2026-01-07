using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse
{
    public class DeviceHandler<T>
    {
        protected readonly IDataBase<T> _itemDB;
        private readonly SongsInfoAPIHandler _songInfoAPI;
       

        public DeviceHandler(IDataBase<T> dataDevice)
        {
            _itemDB = dataDevice;
            _songInfoAPI = new SongsInfoAPIHandler();
        }
        public async Task<List<T>> GetAllDevices()
        {
            return await _itemDB.GetAllItems();
        }

        // 2. Update Device (For "Turn On/Off")
        public async Task UpdateDevice(T device)
        {
            await _itemDB.UpdateDB(device);
        }
        public bool CheckIfDBExist()
        {
            if (_itemDB == null) return false;
            try
            {
                List<T> items = _itemDB.GetAllItems().GetAwaiter().GetResult();
                return items != null && items.Count > 0;
            }
            catch { return false; }
        }
        public async Task  AddToDB(T device)
        {
           await _itemDB.SaveToDB(device);
        }
        public async Task<T> GetDeviceById(string id)
        {
            return await _itemDB.GetItemInfo(id);
        }
        public virtual async Task RemoveItemByID(string id)
        {
            await _itemDB.RemoveItem(id);
        }
        public virtual async Task GetInfoFromApi(string singerName)
        {
            

            Console.WriteLine($"\n[System] Searching for top song by '{singerName}'...");

            
            SongsInfo song = await _songInfoAPI.GetBasicData(singerName);

           
            if (song != null)
            {
                Console.WriteLine("------------------------------------------------");
                Console.WriteLine("🎵  SONG FOUND  🎵");
                Console.WriteLine($"   Artist:   {song._singer}");
                Console.WriteLine($"   Track:    {song.Song}");
                
                Console.WriteLine($"   Released: {song._singerDate:yyyy-MM-dd}");
                Console.WriteLine("------------------------------------------------");
            }
            else
            {
                Console.WriteLine($"[System] No songs found for artist: {singerName}");
            }

            
        }

        public async Task FactoryDevices()
        {
            Console.WriteLine("[Factory] checking database...");

            DateTime date = DateTime.Now;
            DateTime cleanDate = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0);

            Dictionary<DateTime, bool> schedule = new Dictionary<DateTime, bool>();
            schedule.Add(cleanDate, true);
            schedule.Add(cleanDate.AddHours(5), false);

            // We create an array of "Actions" or simply handle them one by one 
            // to ensure the 'new' keyword is called AFTER a delay.
            string[] deviceNames = { "Alexa_Home", "Boiler", "smartLight", "AC" };

            foreach (var name in deviceNames)
            {
                try
                {
                    // 1. WAIT first to ensure the system clock moves forward
                    await Task.Delay(200);

                    IDevice newDevice = null;

                   
                    switch (name)
                    {
                        case "Alexa_Home":
                            newDevice = new Alexa(true, name);
                            break;
                        case "Boiler":
                            newDevice = new Boiler(schedule, false, name);
                            break;
                        case "smartLight":
                            newDevice = new SmartLight(true, schedule, name);
                            break;
                        case "AC":
                            newDevice = new AC(true, name, 24);
                            break;
                    }

                    if (newDevice is T typedDevice)
                    {
                        await _itemDB.SaveToDB(typedDevice);
                        Console.WriteLine($"[Factory] Created: {name} (ID: {newDevice._id})");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Error] Could not save {name}: {ex.Message}");
                }
            }
        }
    }
     
}
