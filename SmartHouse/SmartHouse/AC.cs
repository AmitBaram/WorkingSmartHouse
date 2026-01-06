using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse
{
    public class AC : IDevice
    {
        public bool _isOn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string _name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string _id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int _Temperature { get; set; }

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
