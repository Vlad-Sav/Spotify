using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Spotify.Data.Repos;
using Spotify.Models;
using Spotify.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Spotify.Controllers
{
    public class HomeController : Controller
    {
        private SongsRepo _songsRepo = new SongsRepo();
        private ArtistsSongsRepo _artistsSongsRepo = new ArtistsSongsRepo();
        private ArtistsRepo _artistsRepo = new ArtistsRepo();
        private ListenersRepo _listenersRepo = new ListenersRepo();
        private PlaylistsSongsRepo _playlistsSongsRepo = new PlaylistsSongsRepo();
        private PlaylistsRepo _playlistsRepo = new PlaylistsRepo();
        private readonly ILogger<HomeController> _logger;
        private RoleManager<IdentityRole> roleManager;
        private UserManager<AppUser> userManager;
        public HomeController(ILogger<HomeController> logger, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _logger = logger;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (await roleManager.FindByNameAsync("Artist") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Artist"));
            }
            if (await roleManager.FindByNameAsync("Listener") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Listener"));
            }
           
            if (HttpContext.User.IsInRole("Artist"))
                return RedirectToAction("Index", "Artists") ;
            else if (HttpContext.User.IsInRole("Listener"))
                return RedirectToAction("Index", "Listener");
            ViewBag.Songs = CreateSongViewModels(_songsRepo.GetAll().Select(s => s.Id));
            return View("~/Views/Song/RecentlyAdded.cshtml");
        }
        public List<SongItemViewModel> CreateSongViewModels(IEnumerable<int> songsIds)
        {
            List<SongItemViewModel> list = new List<SongItemViewModel>();
            
            foreach (var i in songsIds)
            {
                string authorName = _artistsRepo.GetOne(_artistsSongsRepo.GetAll().OrderBy(s => s.Id).First().ArtistId).ArtistName;
                string artists = authorName;
                string songName = _songsRepo.GetOne(i).SongName;
                foreach (var j in _artistsSongsRepo.GetAll().Where(a => a.SongId == i))
                {
                    var n = _artistsRepo.GetOne(j.ArtistId).ArtistName;
                    if (n != authorName)
                        artists += ", " + n;
                }
                list.Add(new SongItemViewModel
                {
                    Artists = artists,
                    SongName = songName,
                    Path = _songsRepo.GetOne(i).Path,
                    SongId = i
                }); ;
            }
            return list.OrderByDescending(s => s.SongId).Select(s => s).ToList();
        }
        public IActionResult Privacy()
        {
            return View();
          
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
       
    }
}
