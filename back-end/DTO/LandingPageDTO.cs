using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.DTO
{
    public class LandingPageDTO
    {
        public List<FilmDTO> OnCine { get; set; }
        public List<FilmDTO> NextPremiere { get; set; }
    }
}
