using Microsoft.AspNetCore.Identity;
using Spotify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spotify
{
    public class RolesInitializer
    {
        public static async Task InitializeAsync( RoleManager<IdentityRole> roleManager)
        {
         
            if (await roleManager.FindByNameAsync("Artist") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Artist"));
            }
            if (await roleManager.FindByNameAsync("Listener") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Listener"));
            }
          
        }
    }
}
