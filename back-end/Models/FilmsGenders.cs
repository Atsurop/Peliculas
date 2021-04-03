using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Models
{
    public class FilmsGenders
    {
        public int FilmId { get; set; }
        public int GenderId { get; set; }
        public Film Film { get; set; }
        public Genders Gender { get; set; }
    }
}
