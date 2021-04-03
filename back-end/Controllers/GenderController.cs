using AutoMapper;
using back_end.Data;
using back_end.DTO;
using back_end.Models;
using back_end.Repositories;
using back_end.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Controllers
{
    [Route("api/generos")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class GenderController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public GenderController(ILogger<GenderController> logger, ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GenderDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var queryable = _db.Genders.AsQueryable();
            await HttpContext.InsertParametersInHeader(queryable);
            var genders = await queryable.OrderBy(x => x.Name).Pagination(paginationDTO).ToListAsync();
            return _mapper.Map<List<GenderDTO>>(genders);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GenderDTO>> Get(int id)
        {
            var gender = await _db.Genders.FirstOrDefaultAsync(x => x.Id == id);

            if(gender == null)
            {
                return NotFound();
            }
            else
            {
                return _mapper.Map<GenderDTO>(gender);
            }
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult<List<GenderDTO>>> All()
        {
            var genders = await _db.Genders.ToListAsync();
            return _mapper.Map<List<GenderDTO>>(genders);
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int Id,[FromBody] GenderCreationDTO genderCreationDTO)
        {
            var gender = await _db.Genders.FirstOrDefaultAsync(x => x.Id == Id);

            if(gender == null)
            {
                return NotFound();
            }
            gender = _mapper.Map(genderCreationDTO, gender);
            await _db.SaveChangesAsync();
            return NoContent();                
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await _db.Genders.AnyAsync(x => x.Id == id);

            if (!exists)
            {
                return NotFound();
            }
            _db.Remove(new Genders() { Id = id });
            await _db.SaveChangesAsync();
            return NoContent();
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GenderCreationDTO genderCreationDTO)
        {
            var gender = _mapper.Map<Genders>(genderCreationDTO);
            _db.Add(gender);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
