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
    [Route("api/actores")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class ActorController: ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IStoreFiles _storefiles;
        private readonly string container = "actores";
        public ActorController(ApplicationDbContext db, IMapper mapper, IStoreFiles storefiles)
        {
            _db = db;
            _mapper = mapper;
            _storefiles = storefiles;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var queryable = _db.Actors.AsQueryable();
            await HttpContext.InsertParametersInHeader(queryable);
            var actors = await queryable.OrderBy(x => x.Name).Pagination(paginationDTO).ToListAsync();
            return _mapper.Map<List<ActorDTO>>(actors);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ActorDTO>> Get(int id)
        {
            var actor = await _db.Actors.FirstOrDefaultAsync(x => x.Id == id);

            if (actor == null)
            {
                return NotFound();
            }

            return _mapper.Map<ActorDTO>(actor);
        }
        [HttpPost("buscarPorNombre")]
        public async Task<ActionResult<List<FilmActorDTO>>> BuscarPorNombre([FromBody]string name)
        {
            if(string.IsNullOrWhiteSpace(name)) { return new List<FilmActorDTO>(); }
            return await _db.Actors
                .Where(x => x.Name.Contains(name))
                .Select(x => new FilmActorDTO { Id = x.Id, Name = x.Name, Picture = x.Picture })
                .Take(5)
                .ToListAsync();
        } 

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCreationDTO actorCreationDTO)
        {
            var actor = await _db.Actors.FirstOrDefaultAsync(x => x.Id == id);

            if(actor == null)
            {
                return NotFound();
            }

            actor = _mapper.Map(actorCreationDTO, actor);

            if(actorCreationDTO.picture != null)
            {
                actor.Picture = await _storefiles.UpdateFile(container, actorCreationDTO.picture, actor.Picture);
            }
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var actor = await _db.Actors.FirstOrDefaultAsync(x => x.Id == id);

            if (actor == null)
            {
                return NotFound();
            }
            _db.Remove(new Actors() { Id = id });
            await _storefiles.DeleteFile(actor.Picture, container);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreationDTO actorCreationDTO)
        {
            var actor = _mapper.Map<Actors>(actorCreationDTO);

            if (actorCreationDTO.picture != null)
            {
                actor.Picture = await _storefiles.SaveFile(container, actorCreationDTO.picture);
            }
            _db.Add(actor);
            await _db.SaveChangesAsync();
            return NoContent();

        }
    }
}
