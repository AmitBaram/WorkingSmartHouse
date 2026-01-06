using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse
{
    public class SongsInfoAPIHandler : IExternalDataService<SongsInfo>
    {
        public Task<SongsInfo> GetBasicData(string key)
        {
            throw new NotImplementedException();
        }

        public Task <List<SongsInfo>> GetData(string key)
        {
            throw new NotImplementedException();
        }
    }
}
