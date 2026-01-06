using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse
{
    public class DeviceHandler<T>
    {
        protected readonly IJsonDataBase<T> _deviceDB;

        public DeviceHandler(IJsonDataBase<T> dataDevice)
        {
            _deviceDB = dataDevice;
        }
        public async Task  AddToDB(T device)
        {
           await  _deviceDB.SaveToDB(device);
        }
        public async Task GetDeviceById(string id)
        {
           await _deviceDB.GetItemInfo(id);
        }
    }   
}
