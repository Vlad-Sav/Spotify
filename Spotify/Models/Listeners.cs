using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Spotify.Models
{
    public partial class Listeners: BaseEntity
    {
        public Listeners()
        {
            Playlists = new HashSet<Playlists>();
        }
        [Column("ListenerId")]
        public override int Id { get; set; }
        public string ListenerName { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Playlists> Playlists { get; set; }
    }
}
