using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Spotify.Models
{
    public class BaseEntity
    {
        [Key]
        public virtual int Id { get; set; }
    }
}
