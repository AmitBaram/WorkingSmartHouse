using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace SmartHouse
{
    public class JsonContactsDB : IJsonDataBase<Dictionary<string, string>>
    {
        private readonly string _jsonPath = @"C:\Users\yeric\Documents\Amit_Pitoch\GitHub\SamertHouseWorking1\WorkingSmartHouse\SmartHouse\SmartHouse\DataBases\ContactsDB\contacts.json";
        private Dictionary<string, string> _contacts;

        public JsonContactsDB()
        {
            LoadFromFile();
        }

        public async Task RemoveItem(string id)
        {
            if (_contacts.ContainsKey(id))
            {
                // 2. Remove from the in-memory dictionary
                _contacts.Remove(id);

                // 3. Save the updated dictionary back to the JSON file
                await SaveToFile();
            }
            else
            {
                // Optional: Throw an exception if you want to know if a removal failed
                throw new Exception($"Contact with name '{id}' not found.");
            }
        }


        public async Task<Dictionary<string, string>> GetItemInfo(string id)
        {
            // Returns the entire contact list
            return await Task.Run(() => _contacts);
        }
        public async Task<List<Dictionary<string, string>>> GetAllItems()
        {
            // Create a new list and put the single dictionary inside it
            var list = new List<Dictionary<string, string>>();
            list.Add(_contacts);

            return await Task.FromResult(list);
        }

        public async Task SaveToDB(Dictionary<string, string> newContacts)
        {
            foreach (var contact in newContacts)
            {
                if (!_contacts.ContainsKey(contact.Key))
                {
                    _contacts.Add(contact.Key, contact.Value);
                }
                else
                {
                    throw new Exception($"Contact with name '{contact.Key}' already exists. Use UpdateDB instead.");
                }
            }
            await SaveToFile();
        }

        // Added UpdateDB Method
        public async Task UpdateDB(Dictionary<string, string> itemToUpdate)
        {
            // If the item passed in is a dictionary of updates, apply them
            foreach (var contact in itemToUpdate)
            {
                if (_contacts.ContainsKey(contact.Key))
                {
                    _contacts[contact.Key] = contact.Value;
                }
                else
                {
                    // Optionally add it if it doesn't exist, or throw error
                    _contacts.Add(contact.Key, contact.Value);
                }
            }

            await SaveToFile();
        }

        private async Task SaveToFile()
        {
            string json = JsonConvert.SerializeObject(_contacts, Formatting.Indented);

            // Fix for 'File' does not contain 'WriteAllTextAsync'
            File.WriteAllText(_jsonPath, json);
            await Task.CompletedTask;
        }

        private void LoadFromFile()
        {
            if (!File.Exists(_jsonPath))
            {
                _contacts = new Dictionary<string, string>();
                return;
            }

            try
            {
                string json = File.ReadAllText(_jsonPath);
                _contacts = JsonConvert.DeserializeObject<Dictionary<string, string>>(json)
                            ?? new Dictionary<string, string>();
            }
            catch
            {
                _contacts = new Dictionary<string, string>();
            }
        }

    }
}

