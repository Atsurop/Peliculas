using back_end.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.DTO
{
    public class FilmCreationDTO
    {
        public string Title { get; set; }
        public string Resume { get; set; }
        public string Trailer { get; set; }
        public bool OnCine { get; set; }
        public DateTime LaunchDate { get; set; }
        public IFormFile Poster { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> GendersIds { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> CinesIds { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder<List<ActorFilmCreationDTO>>))]
        public List<ActorFilmCreationDTO> Actors { get; set; }
    }
}
