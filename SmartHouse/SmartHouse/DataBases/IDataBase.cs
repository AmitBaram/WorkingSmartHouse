using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse
{
    public interface IDataBase<T>
    {
        Task SaveToDB(T item);
        Task<T> GetItemInfo(string id);
        Task UpdateDB(T item);
        Task RemoveItem(string id);
        Task<List<T>> GetAllItems();
        
    }
}
