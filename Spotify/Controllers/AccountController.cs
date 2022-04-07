using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Spotify.Data.Repos;
using Spotify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spotify.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        private ArtistsRepo _artistsRepo = new ArtistsRepo();
        private PlaylistsRepo _playlistsRepo = new PlaylistsRepo();
        private ListenersRepo _listenersRepo = new ListenersRepo();
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(User model)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser
                {
                    UserName = model.UserName,
                    Email = model.Email
                };
                // добавляем пользователя
                var result = await userManager.CreateAsync(appUser, model.Password);
              
                if (result.Succeeded)
                {
                    AppUser user = await userManager.FindByEmailAsync(model.Email);
                    if (user != null)
                    {
                        if (model.isArtist) { 
                            await userManager.AddToRoleAsync(user, "Artist");
                            _artistsRepo.Add(new Artists { ArtistName = model.UserName, Password = model.Password });
                        }
                        else { await userManager.AddToRoleAsync(user, "Listener");
                            await userManager.AddToRoleAsync(user, "Listener");
                            _listenersRepo.Add(new Listeners { ListenerName = model.UserName, Password = model.Password });
                            var id = _listenersRepo.GetAll().Where(m => m.ListenerName == model.UserName).First().Id;
                            _playlistsRepo.Add(new Playlists { ListenerId = id, PlaylistName = "general" });
                        }

                    }
                    await signInManager.SignInAsync(appUser, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user)
        {
            AppUser model = userManager.FindByNameAsync(user.UserName).Result;
        
            {
                var result =
                    await signInManager.PasswordSignInAsync(model, user.Password, true, true);
                if (result.Succeeded)
                {
                    
                   
                    return RedirectToAction("Index", "Home");
                   
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect username or password");
                }
            }
            return RedirectToAction("Index","Home");
        }
    }
}
