using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHouse
{
    public class MultimediaController
    {
        // Field for the external API service
        private readonly IExternalDataService<SongsInfo> _songInfoAPI;
        private readonly SchedualDeviceHandler<IDevice> _deviceHandler;

        public MultimediaController(IExternalDataService<SongsInfo> songInfoAPI)
        {
            _songInfoAPI = songInfoAPI;
        }

        /// <summary>
        /// Logic for searching and displaying song information.
        /// </summary>
        public async Task SearchAndDisplaySong(string singerName)
        {
            Console.WriteLine($"\n[Multimedia] Searching for top song by '{singerName}'...");

            // Use the field to get data
            SongsInfo song = await _songInfoAPI.GetBasicData(singerName);

            if (song != null)
            {
                Console.WriteLine("------------------------------------------------");
                Console.WriteLine("🎵  SONG FOUND  🎵");
                Console.WriteLine($"   Artist:   {song._singer}");
                Console.WriteLine($"   Track:    {song.Song}");
                Console.WriteLine($"   Released: {song._singerDate:yyyy-MM-dd}");
                Console.WriteLine("------------------------------------------------");
            }
            else
            {
                Console.WriteLine($"[Multimedia] No songs found for artist: {singerName}");
            }
        }
    }
}