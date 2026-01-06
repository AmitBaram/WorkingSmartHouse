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
        private readonly SchedualDeviceHandler<ISchedualDevice> _sDeviseHandler;

        public event Action<DateTime> OnMinuteTick;
        public event Action<DateTime> OnHourTick;

        public ClockManager()
        {
            DateTime now = DateTime.Now;
            _lastMinute = now.Minute;
            _lastHour = now.Hour;

            // Check the time every 1 second (1000ms) 
            // This ensures we catch the exact moment the minute flips (e.g. 10:00:59 -> 10:01:00)
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

            // Call the two separate methods
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

        // Method 2: Handles Hour Logic
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
