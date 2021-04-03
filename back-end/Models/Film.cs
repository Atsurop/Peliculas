using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Models
{
    public class Film
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength:300)]
        public string Title { get; set; }
        public string Resume { get; set; }
        public string Trailer { get; set; }
        public bool OnCine { get; set; }
        public DateTime LaunchDate { get; set; }
        public string Poster { get; set; }
        public List<FilmsActors> FilmsActors { get; set; }
        public List<FilmsCine> FilmsCines { get; set; }
        public List<FilmsGenders> FilmsGender { get; set; }
    }
}
