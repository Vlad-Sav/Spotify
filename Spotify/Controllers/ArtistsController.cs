using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Spotify.Data.Repos;
using Spotify.Models;
using Spotify.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spotify.Controllers
{
    public class ArtistsController : Controller
    {
        private SongsRepo _songsRepo = new SongsRepo();
        private ArtistsSongsRepo _artistsSongsRepo = new ArtistsSongsRepo();
        private ArtistsRepo _artistsRepo = new ArtistsRepo();
        private UserManager<AppUser> userManager;
        public ArtistsController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }
        // GET: ArtistsController
        public ActionResult Index()
        {
            var identity = HttpContext.User.Identity;
            AppUser user = userManager.FindByNameAsync(identity.Name).Result;

            //get all songs by artist
            var userName = identity.Name;
            var artistId = _artistsRepo.GetAll().Where(a => a.ArtistName == userName).First().Id;
            var songsIds = _artistsSongsRepo.GetAll()?.Where(a => a.ArtistId == artistId).Select(a => a.SongId)?.Distinct();
            List<SongItemViewModel> list = new List<SongItemViewModel>();
            
            foreach(var i in songsIds)
            {
                string artists = userName;
                string songName = _songsRepo.GetOne(i).SongName;
                foreach(var j in _artistsSongsRepo.GetAll().Where(a => a.SongId == i))
                {
                    var n = _artistsRepo.GetOne(j.ArtistId).ArtistName;
                    if (n != userName)
                        artists += ", " + n;
                }
                list.Add(new SongItemViewModel
                {
                    Artists = artists,
                    SongName = songName,
                    Path = _songsRepo.GetOne(i).Path,
                    SongId = i ?? 0
                });
            }



            ViewBag.Songs = list.OrderByDescending(s => s.SongId).Select(s => s).ToList(); 
            return View(user);
        }

    
    }
}
