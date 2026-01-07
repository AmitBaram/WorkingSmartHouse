using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;
using System.IO;

namespace SmartHouse
{
    public class JsonHomeDataBase<T> : IJsonDataBase<T> where T : IDevice
    {
        private readonly string _jsonPath = @"C:\Users\yeric\Documents\Amit_Pitoch\GitHub\SamertHouseWorking1\WorkingSmartHouse\SmartHouse\SmartHouse\DataBases\HomeDB\DeviceHomejsonDB.json";

        // Use List<T> instead of List<IDevice>
        List<T> _devices;

        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Formatting.Indented
        };

        public JsonHomeDataBase()
        {
            LoadFromFile();
        }

        public async Task RemoveItem(string id)
        {
            var deviceToRemove = _devices.FirstOrDefault(d => d._id == id);
            if (deviceToRemove != null)
            {
                _devices.Remove(deviceToRemove);
                await SaveToFile();
            }
            else
            {
                throw new Exception($"Device with ID {id} not found.");
            }
        }

        public async Task<T> GetItemInfo(string id)
        {
            return await Task.Run(() => _devices.FirstOrDefault(d => d._id == id));
        }

        public async Task SaveToDB(T item)
        {
            if (_devices.Any(d => d._id == item._id))
                throw new Exception("Device already exists.");

            _devices.Add(item);
            await SaveToFile();
        }

        public async Task UpdateDB(T item)
        {
            var index = _devices.FindIndex(d => d._id == item._id);
            if (index == -1) throw new Exception("Device not found.");

            _devices[index] = item;
            await SaveToFile();
        }

        private async Task SaveToFile()
        {
            string json = JsonConvert.SerializeObject(_devices, _settings);
            File.WriteAllText(_jsonPath, json);
            await Task.CompletedTask;
        }
        public async Task<List<T>> GetAllItems() 
        {
            // 1. Check if the file exists physically on the disk
            if (!File.Exists(_jsonPath))
            {
                return new List<T>();
            }

            // 2. Perform the read operation on a background thread to avoid freezing UI
            return await Task.Run(() =>
            {
                try
                {
                    // Read the raw text from the file
                    string json = File.ReadAllText(_jsonPath);

                    // Convert it back into the list of objects
                    List<T> freshList = JsonConvert.DeserializeObject<List<T>>(json, _settings);

                    // Update the local list to match (optional, but good for consistency)
                    _devices = freshList ?? new List<T>();

                    return _devices;
                }
                catch
                {
                    // If the file is corrupted or locked, return an empty list
                    return new List<T>();
                }
            });
        }

        private void LoadFromFile()
        {
            if (!File.Exists(_jsonPath))
            {
                Console.WriteLine("[DB] File not found. Starting fresh.");
                // CRITICAL: Ensure the list is empty so Factory can add new ones
                _devices = new List<T>();
                return;
            }

            try
            {
                string json = File.ReadAllText(_jsonPath);
                _devices = JsonConvert.DeserializeObject<List<T>>(json, _settings);

                if (_devices == null)
                {
                    Console.WriteLine("[DB] Warning: JSON file existed but was empty/null.");
                    _devices = new List<T>();
                }
                else
                {
                    Console.WriteLine($"[DB] Loaded {_devices.Count} devices from file.");
                }
            }
            catch (Exception ex)
            {
                // THIS IS THE CRITICAL FIX: Print the error!
                Console.WriteLine($"\n[CRITICAL DB ERROR] Failed to load JSON file!");
                Console.WriteLine($"Error Message: {ex.Message}");
                Console.WriteLine($"Make sure all device classes (AC, Boiler, etc.) have a 'public ClassName() {{ }}' constructor.\n");

                // We initialize it to empty so the app doesn't crash, 
                // BUT now you will see the error message in the console.
                _devices = new List<T>();
            }
        }
    }
}
