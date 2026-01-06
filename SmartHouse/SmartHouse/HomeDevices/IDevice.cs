using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse
{
    public interface IDevice
    {
        bool _isOn { get; set; }
        string _name { get; set; }
        string _id { get; set; }


        void TurnOn();
        void TurnOff();
    }
}
