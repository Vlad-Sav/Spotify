using Spotify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spotify.Data.Repos
{
    public class ListenersRepo: BaseRepo<Listeners>
    {
        public override int Delete(int id)
        {
            var a = GetAll().Where(m => m.Id == id).First();
            _db.Listeners.Remove(a);
            return _db.SaveChanges();
        }
    }
}
