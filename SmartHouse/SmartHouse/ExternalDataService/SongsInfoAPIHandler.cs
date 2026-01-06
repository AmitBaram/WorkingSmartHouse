using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SmartHouse
{
    public class SongsInfoAPIHandler : IExternalDataService<SongsInfo>
    {
        private readonly HttpClient _client = new HttpClient();
        public async Task<SongsInfo> GetBasicData(string singerName)
        {
           
            // 1. Construct the iTunes Search URL
            // term: the singer, entity: song, limit: 1 (to get the top result)
            
            string url = $"https://itunes.apple.com/search?term={Uri.EscapeDataString(singerName)}&entity=song&limit=1";


            try
            {
                // 2. Call the API
                string response = await _client.GetStringAsync(url);

                // 3. Parse the JSON
                // Using dynamic is a quick way to access JSON properties without a separate model class
                dynamic data = JsonConvert.DeserializeObject(response);

                // 4. Check if we have at least one result
                if (data.resultCount > 0)
                {
                    var firstResult = data.results[0];

                    string artist = firstResult.artistName;
                    string track = firstResult.trackName;
                    DateTime release = firstResult.releaseDate;

                    // 5. Return the new SongsInfo object using your constructor
                    return new SongsInfo(artist, track, release);
                }

                return null; // No results found
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API Error: {ex.Message}");
                return null;
            }
        }
        

        public Task <List<SongsInfo>> GetData(string key)
        {
            throw new NotImplementedException();
        }
    }
}
