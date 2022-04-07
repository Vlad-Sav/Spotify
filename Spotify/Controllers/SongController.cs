using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Spotify.Models;
using Spotify.Models.ViewModels;
using Spotify.Data.Repos;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;

namespace Spotify.Controllers
{
    public class SongController : Controller
    {
        private SongsRepo _songsRepo = new SongsRepo();
        private ArtistsSongsRepo _artistsSongsRepo = new ArtistsSongsRepo();
        private ArtistsRepo _artistsRepo = new ArtistsRepo();
        private ListenersRepo _listenersRepo = new ListenersRepo();
        private PlaylistsSongsRepo _playlistsSongsRepo = new PlaylistsSongsRepo();
        private PlaylistsRepo _playlistsRepo = new PlaylistsRepo();
        IWebHostEnvironment _appEnvironment;
        private UserManager<AppUser> userManager;
        public SongController(IWebHostEnvironment appEnvironment, UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
            _appEnvironment = appEnvironment;
        }
        public int GetCurrentId()
        {
            var identity = HttpContext.User.Identity;
            AppUser user = userManager.FindByNameAsync(identity.Name).Result;
            var userName = identity.Name;
            var artistId = _artistsRepo.GetAll().Where(a => a.ArtistName == userName).First().Id;
            return artistId;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            var curId = GetCurrentId();
            var artists = _artistsRepo.GetAll().Where(a => a.Id != curId);
            
            ViewBag.Artists = new SelectList(artists, "Id", "ArtistName");
            return View();
        }
        public string SHA1Crypt(string source)
        {
            using (SHA1 sha1Hash = SHA1.Create())
            {
                
                byte[] sourceBytes = Encoding.UTF8.GetBytes(source);
                byte[] hashBytes = sha1Hash.ComputeHash(sourceBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);

                return hash;
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSongViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            if (model != null)
            {
                var songNameHash = SHA1Crypt(model.SongName);
                string path = "/Songs/" + songNameHash + ".mp3";
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await model.File.CopyToAsync(fileStream);
                }

                _songsRepo.Add(new Songs
                {
                    Path = path,
                    SongName = model.SongName
                }) ;

                var songId = _songsRepo.GetAll().Where(s => s.SongName == model.SongName).First().Id;
                var artistIds = _artistsRepo.GetAll().Where(a => a.ArtistName == userName);
                var artistId = artistIds.First().Id;
                _artistsSongsRepo.Add(new ArtistsSongs
                {

                    SongId = songId,
                    ArtistId = artistId
                });
                if(model.ArtistsId != null)
                {
                    foreach (var item in model.ArtistsId)
                    {
                        _artistsSongsRepo.Add(new ArtistsSongs
                        {
                            ArtistId = item,
                            SongId = songId
                        });
                    }
                }
             

            }

            return RedirectToAction("Index", "Home");
        }
        public ActionResult Delete(int id)
        {
            Songs song = _songsRepo.GetOne(id);
            if (song == null)
            {
                return View();
            }
            return View(song);
        }

        // POST: DishTypesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Songs song)
        {
           
            try
            {
                var songs = _artistsSongsRepo.GetAll().Where(s => s.SongId == song.Id);
                foreach(var s in songs)
                {
                    _artistsSongsRepo.Delete(s.Id);
                }
                var path = _songsRepo.GetOne(id).Path;
                _songsRepo.Delete(song.Id);
                
                System.IO.File.Delete(_appEnvironment.WebRootPath + path);
                return RedirectToAction(nameof(Index),"Artist");
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, $@"Unable to create record: {e.Message}");
                return View();
            }
        }
        public ActionResult RecentlyAdded()
        {

            var identity = HttpContext.User.Identity;
            AppUser user = userManager.FindByNameAsync(identity.Name).Result;

            
            var userName = identity.Name;
            var listenerId = _listenersRepo.GetAll().Where(a => a.ListenerName == userName).First().Id;
            var playlistId = _playlistsRepo.GetAll()
                .Where(p => p.ListenerId == listenerId && p.PlaylistName == "general")
                .First()
                .Id;
            var listenersSongsIds = _playlistsSongsRepo.GetAll().Where(s => s.PlaylistId == playlistId).Select(s => s.SongId);

            var songsIds = _artistsSongsRepo.GetAll()?.Select(a => a.SongId)?.Distinct();
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
                    SongId = i ?? 0,
                    IsAdded = listenersSongsIds.Contains(i ?? 0)? true : false
                }); ;
            }
            ViewBag.Songs = list.OrderByDescending(s => s.SongId).Select(s => s).ToList();
            
            return View();
        }
    }
}
