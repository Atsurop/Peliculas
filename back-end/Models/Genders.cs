using back_end.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Models
{
    public class Genders
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 10)]
        [FirstLetterMayus]
        public string Name { get; set; }
        public List<FilmsGenders> FilmsGender { get; set; }
    }
}
