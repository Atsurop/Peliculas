using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using back_end.Models;

namespace back_end.Repositories{
    public class memoryRepositories: IRepository{

        private List<Genders> _genders;
        public memoryRepositories(){
            _genders = new List<Genders>(){
            new Genders(){Id=1, Name="comedia"},
            new Genders(){Id=2, Name="accion"}
            };
        }

        public List<Genders> obtainingAllGenders(){
            return _genders;
        }
    }
}