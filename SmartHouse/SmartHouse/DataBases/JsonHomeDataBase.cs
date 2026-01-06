using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse
{
    public class JsonHomeDataBase : IJsonDataBase<IDevice>
    {
        private readonly string  _jsonPath="homejsonDB.json";
        List<IDevice> _devices;
        public async Task<IDevice> GetItemInfo(string id)
        {
            throw new NotImplementedException();
        }

        public async Task SaveToDB(IDevice item)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateDB(IDevice item)
        {
            throw new NotImplementedException();
        }
    }
}
