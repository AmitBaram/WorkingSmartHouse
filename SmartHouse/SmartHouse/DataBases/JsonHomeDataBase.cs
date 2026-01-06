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
        private readonly string _jsonPath = @"C:\Users\yeric\Documents\Amit_Pitoch\GitHub\SamertHouseWorking1\WorkingSmartHouse\SmartHouse\SmartHouse\DataBases\HomeDB\homejsonDB.json";

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

        private void LoadFromFile()
        {
            if (!File.Exists(_jsonPath))
            {
                _devices = new List<T>();
                return;
            }
            try
            {
                string json = File.ReadAllText(_jsonPath);
                _devices = JsonConvert.DeserializeObject<List<T>>(json, _settings) ?? new List<T>();
            }
            catch
            {
                _devices = new List<T>();
            }
        }
    }
}
