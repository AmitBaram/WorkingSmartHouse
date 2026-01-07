using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse
{
    public class App
    {
        // Dependencies
        private readonly ClockManager _clock;
        private readonly SchedualDeviceHandler<IDevice> _deviceHandler;
        private readonly IExternalDataService<WeatherInfo> _externalDataService;



        public App(ClockManager clock, SchedualDeviceHandler<IDevice> deviceHandler, IExternalDataService<WeatherInfo> externalDataService)
        {
            _clock = clock;
            _deviceHandler = deviceHandler;
            _externalDataService = externalDataService;
        }

        public async Task Start()
        {
            
            if ( !_deviceHandler.CheckIfDBExist())
            {
                // FactoryDevices is async, so we wait for it to finish
                await _deviceHandler.FactoryDevices();
            }

            
            _clock.OnMinuteTick += async (time) => await _deviceHandler.CheckForSchedual(time);

            
            _clock.OnHourTick += async (time) => await _deviceHandler.AutoControlAC("Hiafa");

            
            _clock.OnStart();
        }
        
        
    }
}
