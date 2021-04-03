using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.DTO
{
    public class FilmsFilterDTO
    {
        public int Page { get; set; }
        public int RecordsPerPage { get; set; }
        public PaginationDTO PaginationDTO 
        {
            get { return new PaginationDTO() { Page = Page, RecordsPerPage = RecordsPerPage };  }
        }
        public string Title { get; set; }
        public int GenderId { get; set; }
        public bool OnCine { get; set; }
        public bool NextPremiere { get; set; }
    }
}
