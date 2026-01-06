using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse
{
    public class Boiler : ISchedualDevice
    {
        public Dictionary<DateTime, bool> SchedualTime { get  ; set ; }
        public bool _isOn { get ; set ; }
        public string _name { get ; set ; }
        public string _id { get ; set; }

        public Boiler(Dictionary<DateTime, bool> schedualTime, bool isOn, string name)
        {
            SchedualTime = schedualTime;
            _isOn = isOn;
            _name = name;
            _id = GenerateRandomID();
        }

        public string GenerateRandomID()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();

            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        
        }

        public void TurnOff()
        {
            throw new NotImplementedException();
        }

        public void TurnOn()
        {
            throw new NotImplementedException();
        }
    }
}
