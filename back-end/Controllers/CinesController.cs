using AutoMapper;
using back_end.Data;
using back_end.DTO;
using back_end.Models;
using back_end.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Controllers
{
    [ApiController]
    [Route("api/cines")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class CinesController: ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public CinesController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<CineDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var queryable = _db.Cines.AsQueryable();
            await HttpContext.InsertParametersInHeader(queryable);
            var cines = await queryable.OrderBy(x => x.Name).Pagination(paginationDTO).ToListAsync();
            return _mapper.Map<List<CineDTO>>(cines);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CineDTO>> Get(int id)
        {
            var cine = await _db.Cines.FirstOrDefaultAsync(x => x.Id == id);

            if (cine == null)
            {
                return NotFound();
            }
            else
            {
                return _mapper.Map<CineDTO>(cine);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int Id, [FromBody] CineCreationDTO cineCreationDTO)
        {
            var cine = await _db.Cines.FirstOrDefaultAsync(x => x.Id == Id);

            if (cine == null)
            {
                return NotFound();
            }
            cine = _mapper.Map(cineCreationDTO, cine);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CineCreationDTO cineCreationDTO)
        {
            var cine = _mapper.Map<Cines>(cineCreationDTO);
            _db.Add(cine);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await _db.Cines.AnyAsync(x => x.Id == id);

            if (!exists)
            {
                return NotFound();
            }
            _db.Remove(new Cines() { Id = id });
            await _db.SaveChangesAsync();
            return NoContent();
        }



    }
}
