using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.DTO
{
    public class FilmsPutGetDTO
    {
        public FilmDTO Film { get; set; }
        public List<GenderDTO> GendersSelected { get; set; }
        public List<GenderDTO> GendersNotSelected { get; set; }
        public List<CineDTO> CinesSelected { get; set; }
        public List<CineDTO> CinesNotSelected { get; set; }
        public List<FilmActorDTO> Actors { get; set; }

    }
}
