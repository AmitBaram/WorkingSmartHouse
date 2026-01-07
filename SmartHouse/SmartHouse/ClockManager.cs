using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SmartHouse
{
    public class ClockManager
    {
        private Timer _timer;
        private int _lastMinute;
        private int _lastHour;
        private int _lastDay;

        

        public event Action<DateTime> OnMinuteTick;
        public event Action<DateTime> OnHourTick;
        public event Func<DateTime, Task<List<WeatherInfo>>> OnMidNight;

        public ClockManager()
        {
            DateTime now = DateTime.Now;
            _lastMinute = now.Minute;
            _lastHour = now.Hour;

            // Check the time every 1 second (1000ms) 
            _timer = new Timer(1000);
            _timer.Elapsed += CheckTime;
            _timer.AutoReset = true;
        }

        public void OnStart()
        {
            _timer.Start();
        }

        private void CheckTime(object sender, ElapsedEventArgs e)
        {
            DateTime now = DateTime.Now;

            CheckMinute(now);
            CheckHour(now);
        }

        private void CheckMinute(DateTime now)
        {
            if (now.Minute != _lastMinute)
            {
                _lastMinute = now.Minute;
                
                OnMinuteTick?.Invoke(now);
            }
        }

        private void CheckHour(DateTime now)
        {
            if (now.Hour != _lastHour)
            {
                _lastHour = now.Hour;
                OnHourTick?.Invoke(now);
            }
        }
        private async Task CheckMidNight(DateTime now)
        {
            if (now.Hour == 0 && now.Day != _lastDay)
            {
                _lastDay = now.Day;

                if (OnMidNight != null)
                {
                    Console.WriteLine($"[Clock] Midnight detected! Triggering daily weather update...");
                    
                     await OnMidNight.Invoke(now);

                    
                }
            }
        }
    }
}