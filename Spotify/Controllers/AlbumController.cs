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
    public class AlbumController : Controller
    {
        private SongsRepo _songsRepo = new SongsRepo();
        private ArtistsSongsRepo _artistsSongsRepo = new ArtistsSongsRepo();
        private ArtistsRepo _artistsRepo = new ArtistsRepo();
        private AlbumsRepo _albumsRepo = new AlbumsRepo();
        private AlbumsSongsRepo _albumsSongsRepo = new AlbumsSongsRepo();
        private UserManager<AppUser> userManager;

        public AlbumController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }
        public IActionResult Index()
        {

            var artistId = GetCurrentId();
            
            return View(_albumsRepo.GetAll().Where(m => m.ArtistId == artistId));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Albums album)
        {
          
            album.ArtistId = GetCurrentId();
            _albumsRepo.Add(album);
            return RedirectToAction("Index");
        }
        public int GetCurrentId()
        {
            var identity = HttpContext.User.Identity;
            AppUser user = userManager.FindByNameAsync(identity.Name).Result;
            var userName = identity.Name;
            var artistId = _artistsRepo.GetAll().Where(a => a.ArtistName == userName).First().Id;
            return artistId;
        }
        public IActionResult Delete(int id)
        {
            var model = _albumsRepo.GetOne(id);
            return View(model);
        }
        [HttpPost]
        public IActionResult Delete(int id, Songs song)
        {
            var aSIds = _albumsSongsRepo.GetAll().Where(m => m.AlbumId == id);
            foreach (var item in aSIds)
            {
                _albumsSongsRepo.Delete(item);
            }
            var model = _albumsRepo.GetOne(id);
            _albumsRepo.Delete(model);
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var identity = HttpContext.User.Identity;
            AppUser user = userManager.FindByNameAsync(identity.Name).Result;
            List<SongItemViewModel> list = new List<SongItemViewModel>();
            //get all songs by artist
            var userName = identity.Name;
            var songsIds =_albumsSongsRepo.GetAll().Where(m => m.AlbumId == id).Select(m => m.SongId);
            foreach (var i in songsIds)
            {
                string artists = userName;
                string songName = _songsRepo.GetOne(i).SongName;
                foreach (var j in _artistsSongsRepo.GetAll().Where(a => a.SongId == i))
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
            return View(_albumsRepo.GetOne(id));
        }
        public IActionResult ChooseSong(int albumId)
        {
            var identity = HttpContext.User.Identity;
            AppUser user = userManager.FindByNameAsync(identity.Name).Result;
            List<SongItemViewModel> list = new List<SongItemViewModel>();
            //get all songs by artist
            var userName = identity.Name;
            var songsIds = _artistsSongsRepo.GetAll().Where(m => m.ArtistId == GetCurrentId()).Select(m => m.SongId);
            var songsInCurrentAlbum = _albumsSongsRepo.GetAll().Where(m => m.AlbumId == albumId).Select(m => m.SongId);
            songsIds = songsIds.Where(m => !songsInCurrentAlbum.Contains(m));
            foreach (var i in songsIds)
            {
                string artists = userName;
                string songName = _songsRepo.GetOne(i).SongName;
                foreach (var j in _artistsSongsRepo.GetAll().Where(a => a.SongId == i))
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
         
            return View(_albumsRepo.GetOne(albumId));
        }
        public IActionResult AddToAlbum(int songId, int albumId)
        {
            _albumsSongsRepo.Add(new AlbumsSongs
            {
                SongId = songId,
                AlbumId = albumId
            });
            return RedirectToAction("ChooseSong",new { albumId = albumId });
        }
        public IActionResult DeleteFromAlbum(int songId, int albumId)
        {
            var m = _albumsSongsRepo.GetAll().Where(m => m.SongId == songId && m.AlbumId == albumId).FirstOrDefault();
            _albumsSongsRepo.Delete(m.Id);
            return RedirectToAction("Edit", new { id = albumId });
        }
    }
}
