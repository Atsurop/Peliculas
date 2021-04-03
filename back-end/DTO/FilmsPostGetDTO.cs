using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.DTO
{
    public class FilmsPostGetDTO
    {
        public List<GenderDTO> Genders { get; set; }
        public List<CineDTO> Cines { get; set; }
    }
}
