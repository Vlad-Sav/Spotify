using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spotify.Models.ViewModels
{
    public class SongItemViewModel
    {
        public string Artists { get; set; }
        public string SongName { get; set; }
        public string Path { get; set; }
        public int SongId { get; set; }
        public bool IsAdded { get; set; }
        public int AlbumId { get; set; }
        
    }
}
