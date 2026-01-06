using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse
{
    public interface IDataBase<T>
    {
        void SaveToDB(T item);
        T GetItemInfo(string id);

        void UpdateDB();

    }
}
