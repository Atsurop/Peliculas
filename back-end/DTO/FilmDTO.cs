using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.DTO
{
    public class FilmDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Resume { get; set; }
        public string Trailer { get; set; }
        public bool OnCine { get; set; }
        public DateTime LaunchDate { get; set; }
        public string Poster { get; set; }
        public List<GenderDTO> Genders { get; set; }
        public List<FilmActorDTO> Actors { get; set; }
        public List<CineDTO> Cines { get; set; }
        public int UserVote { get; set; }
        public double VoteProm { get; set; }
    }
}
