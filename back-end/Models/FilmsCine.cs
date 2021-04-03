using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Models
{
    public class FilmsCine
    {
        public int FilmId { get; set; }
        public int CineId { get; set; }
        public Film Film { get; set; }
        public Cines Cine { get; set; }
    }
}
