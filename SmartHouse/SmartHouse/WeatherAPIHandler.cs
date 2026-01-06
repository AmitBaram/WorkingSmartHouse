using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartHouse
{
    public class WeatherAPIHandler : IExternalDataService<WeatherInfo>
    {
        private static readonly HttpClient client = new HttpClient();
        private const string ApiKey = "b367de2e84e925c930ae0476fcb995b0";
       

        public List<WeatherInfo> GetData(string key)
        {
            throw new NotImplementedException();
        }
        public async Task<WeatherInfo> GetBasicData(string cityName)
        {
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={ApiKey}&units=metric";

            try
            {
                // 1. Call the API
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode(); // Throws error if city not found

                string responseBody = await response.Content.ReadAsStringAsync();

                // 2. Parse the JSON
                // We use JsonDocument to jump straight to the "main" -> "temp" section
                using (JsonDocument doc = JsonDocument.Parse(responseBody))
                {
                    JsonElement root = doc.RootElement;

                    // OpenWeatherMap returns temp as a float, we cast to int as per your class
                    double temp = root.GetProperty("main").GetProperty("temp").GetDouble();
                    string name = root.GetProperty("name").GetString();

                    // 3. Return your custom class
                    return new WeatherInfo(name, DateTime.Now, (int)Math.Round(temp));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching weather: {ex.Message}");
                return null;
            }
        }
    }
}
