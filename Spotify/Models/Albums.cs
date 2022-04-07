using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Spotify.Models
{
    public partial class Albums: BaseEntity
    {
        public Albums()
        {
            AlbumsSongs = new HashSet<AlbumsSongs>();
        }
        [Column("AlbumId")]
        public override int Id { get; set; }
        public string AlbumName { get; set; }
        public int? ArtistId { get; set; }

        public virtual Artists Artist { get; set; }
        public virtual ICollection<AlbumsSongs> AlbumsSongs { get; set; }
    }
}
