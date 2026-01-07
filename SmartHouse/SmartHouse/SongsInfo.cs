using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse
{
    public class SongsInfo
    {
        public string _singer {  get; set; }
        public string Song { get; set; }
       public  DateTime _singerDate {  get; set; }

        public SongsInfo(string singer, string song, DateTime singerDate)
        {
            this._singer = singer;
            this.Song = song;
            this._singerDate = singerDate;
        }
    }
}
