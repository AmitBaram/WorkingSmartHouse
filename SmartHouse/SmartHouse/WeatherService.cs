using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHouse
{
    public class WeatherService
    {
        private readonly SchedualDeviceHandler<IDevice> _deviceHandler;
        private readonly IExternalDataService<WeatherInfo> _weatherService;

        public WeatherService(SchedualDeviceHandler<IDevice> deviceHandler, IExternalDataService<WeatherInfo> weatherService)
        {
            _deviceHandler = deviceHandler;
            _weatherService = weatherService;
        }

        /// <summary>
        /// Logic for Hourly Automation: Controls ACs based on current weather.
        /// </summary>
        public async Task AutoControlACByWeather(string cityName)
        {
            // Uses GetBasicData from your interface
            WeatherInfo weather = await _weatherService.GetBasicData(cityName);

            if (weather == null)
            {
                Console.WriteLine($"[Scheduler] Failed to get weather for {cityName}. Skipping automation.");
                return;
            }

            List<IDevice> allDevices = await _deviceHandler.GetAllDevices();
            var acUnits = allDevices.OfType<AC>().ToList();

            Console.WriteLine($"[Auto-AC] Temp: {weather._temperature}°C. Checking {acUnits.Count} units...");

            foreach (var ac in acUnits)
            {
                bool stateChanged = false;

                // Heat logic
                if (weather._temperature > 30 && !ac._isOn)
                {
                    Console.WriteLine($" -> {ac._name}: Hot! Turning ON (16°C)");
                    ac.TurnOn();
                    ac._Temperature = 16;
                    stateChanged = true;
                }
                // Cold logic
                else if (weather._temperature < 20 && !ac._isOn)
                {
                    Console.WriteLine($" -> {ac._name}: Cold! Turning ON (30°C)");
                    ac.TurnOn();
                    ac._Temperature = 30;
                    stateChanged = true;
                }
                // Eco/Off logic
                else if (weather._temperature >= 20 && weather._temperature <= 30 && ac._isOn)
                {
                    Console.WriteLine($" -> {ac._name}: Comfortable. Turning OFF.");
                    ac.TurnOff();
                    stateChanged = true;
                }

                if (stateChanged)
                {
                    await _deviceHandler.UpdateDevice(ac);
                }
            }
        }

        /// <summary>
        /// Logic for Midnight Event: Returns the 24-hour forecast list.
        /// </summary>
        public async Task<List<WeatherInfo>> GetDailyWeatherReport(string cityName)
        {
            Console.WriteLine($"[Scheduler] Fetching daily forecast list for {cityName}...");

            // Uses GetData from your interface (OpenMeteo 24h list)
            return await _weatherService.GetData(cityName);
        }
    }
}