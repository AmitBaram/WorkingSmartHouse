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
            ISchedualDevice device = await _itemDB.GetItemInfo(id);

            if (device == null)
                throw new Exception($"Device with id {id} not found");

            return device.SchedualTime;
        }
        public async Task RemoveFromSchedual(string id,DateTime datetime )
        {
            T device = await _itemDB.GetItemInfo(id);

            if (device == null)
            {
                throw new Exception($"Device with id {id} not found");
            }

            // 2. Check if the specific schedule time exists in the dictionary
            if (device.SchedualTime.ContainsKey(datetime))
            {
                // Remove the entry from the dictionary in memory
                device.SchedualTime.Remove(datetime);

                // 3. Save the modified device back to the database
                // This triggers the SaveToFile method in your JsonHomeDataBase
                await _itemDB.UpdateDB(device);
            }
            else
            {
                // Optional: you can choose to throw an error or just do nothing if time isn't found
                throw new Exception($"No schedule found for {datetime} on device {id}");
            }
        }
        

    }
}
