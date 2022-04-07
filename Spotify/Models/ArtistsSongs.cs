using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Spotify.Models
{
    public partial class ArtistsSongs: BaseEntity
    {
        [Column("Asid")]
        public override int Id { get; set; }
        public int? ArtistId { get; set; }
        public int? SongId { get; set; }

        public virtual Artists Artist { get; set; }
        public virtual Songs Song { get; set; }
    }
}
