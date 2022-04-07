using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Spotify.Models
{
    public partial class Playlists: BaseEntity
    {
        public Playlists()
        {
            PlaylistsSongs = new HashSet<PlaylistsSongs>();
        }
        [Column("PlaylistId")]
        public override int Id { get; set; }
        public string PlaylistName { get; set; }
        public int? ListenerId { get; set; }

        public virtual Listeners Listener { get; set; }
        public virtual ICollection<PlaylistsSongs> PlaylistsSongs { get; set; }
    }
}
