using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.DTO
{
    public class CineCreationDTO
    {
        [Required]
        [StringLength(maximumLength:75)]
        public string Name { get; set; }
        [Range(-90,90)]
        public double Latitude { get; set; }
        [Range(-180, 180)]
        public double Length { get; set; }
    }
}
