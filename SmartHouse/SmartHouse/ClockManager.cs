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

        // REMOVED: private readonly SchedualDeviceHandler... 
        // The ClockManager should not know about the DeviceHandler. 
        // The App class connects them.

        public event Action<DateTime> OnMinuteTick;
        public event Action<DateTime> OnHourTick;

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
                // Using ?.Invoke prevents crashes if no one is subscribed
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
    }
}