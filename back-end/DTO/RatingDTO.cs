using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.DTO
{
    public class RatingDTO
    {
        public int FilmId { get; set; }
        [Range(1,5)]
        public int Score { get; set; }
    }
}
