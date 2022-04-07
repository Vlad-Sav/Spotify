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
    public class ListenerController : Controller
    {
        private SongsRepo _songsRepo = new SongsRepo();
        private ArtistsSongsRepo _artistsSongsRepo = new ArtistsSongsRepo();
        private ListenersRepo _listenersRepo = new ListenersRepo();
        private ArtistsRepo _artistsRepo = new ArtistsRepo();
        private PlaylistsRepo _playlistsRepo = new PlaylistsRepo();
        private PlaylistsSongsRepo _playlistsSongsRepo = new PlaylistsSongsRepo();
        private UserManager<AppUser> userManager;
        public ListenerController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }
        // GET: ArtistsController
        public ActionResult Index()
        {
            var identity = HttpContext.User.Identity;
            AppUser user = userManager.FindByNameAsync(identity.Name).Result;

            var userName = identity.Name;
            var listenerId = _listenersRepo.GetAll().Where(a => a.ListenerName == userName).First().Id;
            var playlistId = _playlistsRepo.GetAll()
                .Where(p => p.ListenerId == listenerId && p.PlaylistName == "general")
                .First()
                .Id;
            var songsIds = _playlistsSongsRepo.GetAll().Where(s => s.PlaylistId == playlistId).Select(s => s.SongId);
            List<SongItemViewModel> list = new List<SongItemViewModel>();

            foreach (var i in songsIds)
            {
                string authorName = _artistsRepo.GetOne(_artistsSongsRepo.GetAll().Where(s => s.SongId == i).OrderBy(s => s.Id).First().ArtistId).ArtistName;
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
                    SongId = i ?? 0
                });;
            }
            ViewBag.Songs = list.OrderByDescending(s => s.SongId).Select(s => s).ToList();
            return View(user);
        }
        
    }
}
