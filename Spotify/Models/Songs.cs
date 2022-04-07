using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Spotify.Models
{
    public partial class Songs: BaseEntity
    {
        public Songs()
        {
            AlbumsSongs = new HashSet<AlbumsSongs>();
            ArtistsSongs = new HashSet<ArtistsSongs>();
            PlaylistsSongs = new HashSet<PlaylistsSongs>();
        }
        [Column("SongId")]
        public override int Id { get; set; }
        public string SongName { get; set; }
        public string Path { get; set; }

        public virtual ICollection<AlbumsSongs> AlbumsSongs { get; set; }
        public virtual ICollection<ArtistsSongs> ArtistsSongs { get; set; }
        public virtual ICollection<PlaylistsSongs> PlaylistsSongs { get; set; }

    }
}
