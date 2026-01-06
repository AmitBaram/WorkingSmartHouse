using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse
{
    public class DeviceHandler<T>
    {
        protected readonly IDataBase<T> _itemDB;

        public DeviceHandler(IDataBase<T> dataDevice)
        {
            _itemDB = dataDevice;
        }
        public async Task  AddToDB(T device)
        {
           await _itemDB.SaveToDB(device);
        }
        public async Task GetDeviceById(string id)
        {
           await _itemDB.GetItemInfo(id);
        }
        public virtual async Task RemoveItemByID(string id)
        {
            await _itemDB.RemoveItem(id);
        }
    }   
}
