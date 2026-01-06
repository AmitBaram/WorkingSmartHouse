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
    public class JsonHomeDataBase : IJsonDataBase<IDevice> 
    {
        private readonly string  _jsonPath= @"C:\Users\yeric\Documents\Amit_Pitoch\GitHub\SamertHouseWorking1\WorkingSmartHouse\SmartHouse\SmartHouse\DataBases\HomeDB\homejsonDB.json";
        List<IDevice> _devices;
        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Formatting.Indented
        };
        public JsonHomeDataBase()
        {
            LoadFromFile();
        }

        public async Task<IDevice> GetItemInfo(string id)
        {
            // Search the local list for the device with matching ID
            return await Task.Run(() => _devices.FirstOrDefault(d => d._id == id));
        }

        public async Task SaveToDB(IDevice item)
        {
            if (_devices.Any(d => d._id == item._id))
                throw new Exception("Device with this ID already exists.");

            _devices.Add(item);
            await SaveToFile();
        }
        private async Task SaveToFile()
        {
            string json = JsonConvert.SerializeObject(_devices, _settings);
            // Use the standard synchronous method
            File.WriteAllText(_jsonPath, json);
            await Task.CompletedTask;

        }

        public async Task UpdateDB(IDevice item)
        {
            var index = _devices.FindIndex(d => d._id == item._id);

            if (index == -1)
                throw new Exception("Device not found.");

            _devices[index] = item;
            await SaveToFile();
        }
        private void LoadFromFile()
        {
            if (!File.Exists(_jsonPath))
            {
                _devices = new List<IDevice>();
                return;
            }

            try
            {
                string json = File.ReadAllText(_jsonPath);
                _devices = JsonConvert.DeserializeObject<List<IDevice>>(json, _settings) ?? new List<IDevice>();
            }
            catch
            {
                _devices = new List<IDevice>();
            }
        }

    }
}
