using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace Spotify.Models.ViewModels
{
    public class CreateSongViewModel
    {
        [Required(ErrorMessage = "Enter song`s name")]
        public string SongName { get; set; }
        public int[] ArtistsId { get; set; }
        [Required(ErrorMessage = "Upload audio file")]
        public IFormFile File { get; set; }
    }
}
