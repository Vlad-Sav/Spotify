using Microsoft.AspNetCore.Hosting;
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
    public class PlaylistController : Controller
    {
        private SongsRepo _songsRepo = new SongsRepo();
        private ArtistsSongsRepo _artistsSongsRepo = new ArtistsSongsRepo();
        private ArtistsRepo _artistsRepo = new ArtistsRepo();
        private ListenersRepo _listenersRepo = new ListenersRepo();
        private PlaylistsSongsRepo _playlistsSongsRepo = new PlaylistsSongsRepo();
        private PlaylistsRepo _playlistsRepo = new PlaylistsRepo();
        IWebHostEnvironment _appEnvironment;
        private UserManager<AppUser> userManager;
        public PlaylistController(IWebHostEnvironment appEnvironment, UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
            _appEnvironment = appEnvironment;
        }
        public IActionResult Index()
        {
            int userId = GetCurrentId();
            var playlists = _playlistsRepo.GetAll()
                .Where(p => p.ListenerId == userId && p.PlaylistName != "general");
            return View(playlists);
        }

        public int GetCurrentId()
        {
            var identity = HttpContext.User.Identity;
            AppUser user = userManager.FindByNameAsync(identity.Name).Result;
            var userName = identity.Name;
            var listenerId = _listenersRepo.GetAll().Where(a => a.ListenerName == userName).First().Id;
            return listenerId;
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Playlists playlist)
        {
            playlist.ListenerId = GetCurrentId();
            _playlistsRepo.Add(playlist);
            return RedirectToAction("Index");
        }
        public IActionResult DeleteFromGeneralPlaylisy(int id)
        {
            var listenerId = GetCurrentId();
            var playlistId = _playlistsRepo.GetAll().Where(p => p.ListenerId == listenerId && p.PlaylistName == "general")?.First().Id;
            var songId = _playlistsSongsRepo.GetAll().Where(p => p.SongId == id && p.PlaylistId == playlistId).First().Id;
            _playlistsSongsRepo.Delete(songId);
            return RedirectToAction("Index", "Listener");
        }
        public IActionResult Delete(int id)
        {
            var model = _playlistsRepo.GetOne(id);
            return View(model);
        }
        [HttpPost]
        public IActionResult Delete(int id, Songs song)
        {
            var aSIds = _playlistsSongsRepo.GetAll().Where(m => m.PlaylistId == id);
            foreach (var item in aSIds)
            {
                _playlistsSongsRepo.Delete(item);
            }
            var model = _playlistsRepo.GetOne(id);
            _playlistsRepo.Delete(model);
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var identity = HttpContext.User.Identity;
            AppUser user = userManager.FindByNameAsync(identity.Name).Result;
            List<SongItemViewModel> list = new List<SongItemViewModel>();
            
            var userName = identity.Name;
            var songsIds = _playlistsSongsRepo.GetAll().Where(m => m.PlaylistId == id).Select(m => m.SongId);
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
                });
            }
            ViewBag.Songs = list.OrderByDescending(s => s.SongId).Select(s => s).ToList();
            return View(_playlistsRepo.GetOne(id));
        }

        public IActionResult AddToGeneralPlaylistFromRecent(int songId)
        {
            var listenerId = GetCurrentId();
            var playlistId = _playlistsRepo.GetAll().Where(p => p.ListenerId == listenerId && p.PlaylistName == "general")?.First().Id;
            _playlistsSongsRepo.Add(new PlaylistsSongs
            {
                SongId = songId,
                PlaylistId = playlistId
            });
            return RedirectToAction("RecentlyAdded", "Song");
        }
        public IActionResult ChooseSong(int playlistId)
        {
            
            List<SongItemViewModel> list = new List<SongItemViewModel>();
            var listenerId = GetCurrentId();
            var generalPlaylistId = _playlistsRepo.GetAll().Where(p => p.ListenerId == listenerId && p.PlaylistName == "general")?.First().Id;
            var songIdsFromGeneral = _playlistsSongsRepo.GetAll().Where(s => s.PlaylistId == generalPlaylistId).Select(s => s.SongId);
            var songIdsFromAlbum = _playlistsSongsRepo.GetAll().Where(s => s.PlaylistId == playlistId )?.Select(s => s.SongId);
            var songsIds = songIdsFromGeneral.Where(s => !songIdsFromAlbum.Contains(s));

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
                });
            }
            ViewBag.Songs = list.OrderByDescending(s => s.SongId).Select(s => s).ToList();

            return View(_playlistsRepo.GetOne(playlistId));
        }
        public IActionResult AddToPlaylist(int songId, int playlistId)
        {
            _playlistsSongsRepo.Add(new PlaylistsSongs
            {
                SongId = songId,
                PlaylistId = playlistId
            });
            return RedirectToAction("ChooseSong", new { playlistId = playlistId });
        }
        public IActionResult DeleteFromPlaylist(int songId, int playlistId)
        {
            var m = _playlistsSongsRepo.GetAll().Where(m => m.SongId == songId && m.PlaylistId == playlistId).FirstOrDefault();
            _playlistsSongsRepo.Delete(m.Id);
            return RedirectToAction("Edit", new { id = playlistId });
        }
    }
}
