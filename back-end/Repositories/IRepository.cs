using back_end.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Repositories
{
    interface IRepository
    {
        List<Genders> obtainingAllGenders();
    }
}
