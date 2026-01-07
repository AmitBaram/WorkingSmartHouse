using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse
{
    public class AC : IDevice
    {
        public bool _isOn { get ; set ; }
        public string _name { get; set; }
        public string _id { get; set; }
        public double _Temperature { get; set; }

        public AC()
        {

        }
       
        public AC(bool isOn, string name, int temperature)
        {
            _isOn = isOn;
            _name = name;
            _id = GenerateRandomID();
            _Temperature = temperature;
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
            this._isOn = false;
        }

        public void TurnOn()
        {
            this._isOn = true;
        }
    }
}
