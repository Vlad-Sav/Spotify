using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Spotify.Models.ViewModels
{
    public class SerachViewModel 
    {
        [Required(ErrorMessage = "Enter search text")]
        public string SearchText { get; set; }
        public int Mode { get; set; }
    }
}
