using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse
{
    public class SchedualDeviceHandler<T> : DeviceHandler<T> where T : ISchedualDevice
    {
        
        public SchedualDeviceHandler(IJsonDataBase<T> dataDevice) : base(dataDevice)
        {
        }
        public async Task<Dictionary<DateTime, bool>> GetSchedual(string id)
        {
            ISchedualDevice device = await _deviceDB.GetItemInfo(id);

            if (device == null)
                throw new Exception($"Device with id {id} not found");

            return device.SchedualTime;
        }
        public async Task RemoveFromSchedual(string id,DateTime datetime )
        {

        }
        

    }
}
