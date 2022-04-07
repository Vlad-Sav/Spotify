using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Spotify.Models
{
    public partial class Artists: BaseEntity
    {
        public Artists()
        {
            Albums = new HashSet<Albums>();
            ArtistsSongs = new HashSet<ArtistsSongs>();
        }
        [Column("ArtistId")]
        public override int Id { get; set; }
        public string ArtistName { get; set; }

        public string Password { get; set; }
        public virtual ICollection<Albums> Albums { get; set; }
        public virtual ICollection<ArtistsSongs> ArtistsSongs { get; set; }
    }
}
