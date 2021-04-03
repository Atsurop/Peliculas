using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Models
{
    public class FilmsActors
    {
        public int FilmId { get; set; }
        public int ActorId { get; set; }
        public Film Film { get; set; }
        public Actors Actor { get; set; }
        [StringLength(maximumLength:100)]
        public string Character { get; set; }
        public int Order { get; set; }
    }
}
