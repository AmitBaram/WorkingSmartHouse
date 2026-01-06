using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse
{
    public class DeviceHandler
    {
        private readonly IJsonDataBase<IDevice> _deviceDB;

        public DeviceHandler(IJsonDataBase<IDevice> dataDevice)
        {
            _deviceDB = dataDevice;
        }
        public async Task  AddToDB(IDevice device)
        {
           await  _deviceDB.SaveToDB(device);
        }
        public async Task GetDeviceById(string id)
        {
           await _deviceDB.GetItemInfo(id);
        }
    }   
}
