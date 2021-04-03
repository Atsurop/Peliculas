using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.DTO
{
    public class PaginationDTO
    {
        public int Page { get; set; } = 1;

        private int Records = 10;
        private readonly int MaxRecords = 50;

        public int RecordsPerPage
        {
            get
            {
                return Records;
            }
            set
            {
                Records = (value > MaxRecords) ? MaxRecords : value;
            }
        }

    }
}
