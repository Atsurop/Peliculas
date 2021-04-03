using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.DTO
{
    public class ActorCreationDTO
    {
        [Required]
        [StringLength(maximumLength: 100)]
        public string Name { get; set; }
        public string Biography { get; set; }
        public DateTime BirthDate { get; set; }
        public IFormFile picture { get; set; }
    }
}
