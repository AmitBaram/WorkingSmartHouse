using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse
{
    public class SchedualDeviceHandler : DeviceHandler
    {
        public SchedualDeviceHandler(IJsonDataBase<IDevice> dataDevice) : base(dataDevice)
        {
        }


    }
}
