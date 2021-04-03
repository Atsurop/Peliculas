using back_end.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.DTO
{
    public class GenderCreationDTO
    {
        [Required]
        [StringLength(maximumLength: 10)]
        [FirstLetterMayus]
        public string Name { get; set; }
    }
}
