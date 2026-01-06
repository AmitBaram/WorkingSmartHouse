using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse
{
    public interface IExternalDataService<T>
    {
        Task<T> GetBasicData(string key);
        Task <List<T>> GetData(string key);
    }
}
