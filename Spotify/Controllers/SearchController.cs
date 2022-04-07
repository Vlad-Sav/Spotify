using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Spotify.Data.Repos;
using Spotify.Models;
using Spotify.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spotify.Controllers
{
    public class SearchController : Controller
    {
        private SongsRepo _songsRepo = new SongsRepo();
        private ArtistsSongsRepo _artistsSongsRepo = new ArtistsSongsRepo();
        private ArtistsRepo _artistsRepo = new ArtistsRepo();
        private ListenersRepo _listenersRepo = new ListenersRepo();
        private PlaylistsSongsRepo _playlistsSongsRepo = new PlaylistsSongsRepo();
        private PlaylistsRepo _playlistsRepo = new PlaylistsRepo();
        private AlbumsRepo _albumsRepo = new AlbumsRepo();
        private AlbumsSongsRepo _albumsSongsRepo = new AlbumsSongsRepo();
        IWebHostEnvironment _appEnvironment;
        
        private UserManager<AppUser> userManager;
        public SearchController(IWebHostEnvironment appEnvironment, UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
            _appEnvironment = appEnvironment;
        }
        public IActionResult Index()
        {
            var arr = new Mode[]
            {
                new Mode{ Id = 1, Name = "By Songs"},
                new Mode { Id = 2, Name = "By Artists"},
                new Mode { Id = 3, Name = "By Albums"}
            };
            ViewBag.Modes = new SelectList(arr, "Id", "Name");;
            return View();
        }
        public IActionResult SearchGet(SerachViewModel model)
        {
            var mode = model.Mode;
            if (mode == 1)
            {
                var songsIds = _songsRepo.GetAll().Where(s => s.SongName.ToLower().Contains(model.SearchText.ToLower())).Select(s => s.Id);
                ViewBag.Songs = CreateSongViewModels(songsIds);
                
                return View("SearchSongs", model);
            }
            else if (mode == 2)
            {
                var artists = _artistsRepo.GetAll().Where(a => a.ArtistName.ToLower().Contains(model.SearchText.ToLower()));
                
                return View("SearchArtists", artists);
            }
            else
            {
                var albums = _albumsRepo.GetAll().Where(a => a.AlbumName.ToLower().Contains(model.SearchText.ToLower()));
                ViewBag.Albums = CreateAlbumViewModels(albums);
                return View("SearchAlbums");
            }
        }
        [HttpPost]
        public IActionResult Search(SerachViewModel model)
        {
            return RedirectToAction("SearchGet", model);
            
        }
        public IActionResult AddToGeneralPlaylistFromSongsSearch(int songId)
        {
            var listenerId = GetCurrentId();
            var playlistId = _playlistsRepo.GetAll().Where(p => p.ListenerId == listenerId && p.PlaylistName == "general")?.First().Id;
            _playlistsSongsRepo.Add(new PlaylistsSongs
            {
                SongId = songId,
                PlaylistId = playlistId
            });
            return RedirectToAction("Index", "Listener");
        }
        public IActionResult ArtistsSongs(int artistId)
        {
            var songsIds = _artistsSongsRepo.GetAll().Where(a => a.ArtistId == artistId).Select(a => a.SongId).Distinct().ToList();
            ViewBag.Songs = CreateSongViewModels(songsIds);
            var albumsNames = _albumsRepo.GetAll().Where(a => a.ArtistId == artistId).Select(a => a.AlbumName);
            ViewBag.Albums = albumsNames;
            return View();
        }
        public IActionResult AlbumsSongs(int albumId)
        {
            var songsIds = _albumsSongsRepo.GetAll().Where(s => s.AlbumId == albumId).Select(s => s.SongId);
            var songs = CreateSongViewModels(songsIds);
            foreach (var item in songs)
            {
                item.AlbumId = albumId;
            }
            ViewBag.Songs = songs;
            return View();
        }
        public IActionResult AddToGeneralPlaylistFromSearchAlbums(int songId, int albumId)
        {
            var listenerId = GetCurrentId();
            var playlistId = _playlistsRepo.GetAll().Where(p => p.ListenerId == listenerId && p.PlaylistName == "general")?.First().Id;
            _playlistsSongsRepo.Add(new PlaylistsSongs
            {
                SongId = songId,
                PlaylistId = playlistId
            });
            return RedirectToAction("AlbumsSongs", new { albumId = albumId});
        }
        public List<AlbumItemViewModel> CreateAlbumViewModels(IEnumerable<Albums> albums)
        {
            List<AlbumItemViewModel> list = new List<AlbumItemViewModel>();
            foreach (var item in albums)
            {
                list.Add(new AlbumItemViewModel
                {
                    AlbumId = item.Id,
                    AlbumName = item.AlbumName,
                    ArtistName = _artistsRepo.GetOne(item.ArtistId).ArtistName
                });

            }
            return list;
        }
        public List<SongItemViewModel> CreateSongViewModels(IEnumerable<int> songsIds)
        {
            List<SongItemViewModel> list = new List<SongItemViewModel>();
            var listenerId= GetCurrentId();
            var playlistId = _playlistsRepo.GetAll()
                .Where(p => p.ListenerId == listenerId && p.PlaylistName == "general")
                .First()
                .Id;
            var listenersSongsIds = _playlistsSongsRepo.GetAll().Where(s => s.PlaylistId == playlistId).Select(s => s.SongId);
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
                    SongId = i,
                    IsAdded = listenersSongsIds.Contains(i) ? true : false
                }); ;
            }
            return list.OrderByDescending(s => s.SongId).Select(s => s).ToList();
        }
        public List<SongItemViewModel> CreateSongViewModels(IEnumerable<int?> songsIds)
        {
            List<SongItemViewModel> list = new List<SongItemViewModel>();
            var listenerId = GetCurrentId();
            var playlistId = _playlistsRepo.GetAll()
                .Where(p => p.ListenerId == listenerId && p.PlaylistName == "general")
                .First()
                .Id;
            var listenersSongsIds = _playlistsSongsRepo.GetAll().Where(s => s.PlaylistId == playlistId).Select(s => s.SongId);
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
                    SongId = i ?? 0,
                    IsAdded = listenersSongsIds.Contains(i) ? true : false
                }); ;
            }
            return list.OrderByDescending(s => s.SongId).Select(s => s).ToList();
        }
        public int GetCurrentId()
        {
            var identity = HttpContext.User.Identity;
            AppUser user = userManager.FindByNameAsync(identity.Name).Result;
            var userName = identity.Name;
            var listenerId = _listenersRepo.GetAll().Where(a => a.ListenerName == userName).First().Id;
            return listenerId;
        }
        
    }

    
}
