using AutoMapper;
using back_end.Data;
using back_end.DTO;
using back_end.Models;
using back_end.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Controllers
{
    [ApiController]
    [Route("api/peliculas")] 
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class FilmsController: ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IStoreFiles _storeFiles;
        private readonly string container = "peliculas";
        private readonly UserManager<IdentityUser> _userManager;

        public FilmsController(ApplicationDbContext db, IMapper mapper, IStoreFiles storeFiles,  UserManager<IdentityUser> userManager)
        {
            _db = db;
            _mapper = mapper;
            _storeFiles = storeFiles;
            _userManager = userManager;
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<FilmDTO>> Get(int id)
        {
            var film = await _db.Films
                .Include(x => x.FilmsGender).ThenInclude(x => x.Gender)
                .Include(x => x.FilmsActors).ThenInclude(x => x.Actor)
                .Include(x => x.FilmsCines).ThenInclude(x => x.Cine)
                .FirstOrDefaultAsync(x => x.Id == id);

            if(film == null) { return NotFound(); }

            var voteProm = 0.0;
            var userVote = 0;

            if(await _db.Ratings.AnyAsync(x=> x.FilmId == id))
            {
                voteProm = await _db.Ratings.Where(x => x.FilmId == id).AverageAsync(x => x.Score);
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email").Value;
                    var user = await _userManager.FindByEmailAsync(email);
                    var userId = user.Id;
                    var ratingDB = await _db.Ratings.FirstOrDefaultAsync(x=> x.UserId == userId && x.FilmId == id);

                    if(ratingDB != null)
                    {
                        userVote = ratingDB.Score;
                    }
                }
            }

            var dto = _mapper.Map<FilmDTO>(film);
            dto.UserVote = userVote;
            dto.VoteProm = voteProm;
            dto.Actors = dto.Actors.OrderBy(x => x.Order).ToList();
            return dto;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<LandingPageDTO>> Get()
        {
            var top = 6;
            var today = DateTime.Today;

            var nextPremiere = await _db.Films
                .Where(x => x.LaunchDate > today && x.OnCine == false)
                .OrderBy(x => x.LaunchDate)
                .Take(top)
                .ToListAsync();

            var onCine = await _db.Films
                .Where(x => x.OnCine)
                .OrderBy(x => x.LaunchDate)
                .Take(top)
                .ToListAsync();

            var result = new LandingPageDTO();
            result.NextPremiere = _mapper.Map<List<FilmDTO>>(nextPremiere);
            result.OnCine = _mapper.Map<List<FilmDTO>>(onCine);
            return result;
        }

        [HttpGet("PutGet/{id:int}")]
        public async Task<ActionResult<FilmsPutGetDTO>> PutGet(int id)
        {
            var filmActionResult = await Get(id);
            if(filmActionResult.Result is NotFoundResult) { return NotFound(); }

            var film = filmActionResult.Value;

            var gendersSelectedIds = film.Genders.Select(x => x.Id).ToList();
            var gendersNotSelected = await _db.Genders
                .Where(x => !gendersSelectedIds.Contains(x.Id))
                .ToListAsync();

            var cinesSelectedIds = film.Cines.Select(x => x.Id).ToList();
            var cinesNotSelected = await _db.Cines
                .Where(x => !cinesSelectedIds.Contains(x.Id))
                .ToListAsync();

            var gendersNotSelectedDTO = _mapper.Map<List<GenderDTO>>(gendersNotSelected);
            var cinesNotSelectedDTO = _mapper.Map<List<CineDTO>>(cinesNotSelected);

            var response = new FilmsPutGetDTO();
            response.Film = film;
            response.GendersSelected = film.Genders;
            response.GendersNotSelected = gendersNotSelectedDTO;
            response.CinesSelected = film.Cines;
            response.CinesNotSelected = cinesNotSelectedDTO;
            response.Actors = film.Actors;
            return response;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] FilmCreationDTO filmCreationDTO)
        {
            var film = await _db.Films
                .Include(x => x.FilmsGender)
                .Include(x => x.FilmsCines)
                .Include(x => x.FilmsActors)
                .FirstOrDefaultAsync(x => x.Id == id);

            if(film == null)
            {
                return NotFound();
            }

            film = _mapper.Map(filmCreationDTO, film);

            if(filmCreationDTO.Poster != null)
            {
                film.Poster = await _storeFiles.UpdateFile(container, filmCreationDTO.Poster, film.Poster);
            }

            WriteActorsOrder(film);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("PostGet")]
        public async Task<ActionResult<FilmsPostGetDTO>> PostGet()
        {
            var cines = await _db.Cines.ToListAsync();
            var genders = await _db.Genders.ToListAsync();

            var cinesDTO = _mapper.Map<List<CineDTO>>(cines);
            var gendersDTO = _mapper.Map<List<GenderDTO>>(genders);

            return new FilmsPostGetDTO() { Cines = cinesDTO, Genders = gendersDTO };
        }
        
        [HttpGet("filter")]
        [AllowAnonymous]
        public async Task<ActionResult<List<FilmDTO>>> Filter([FromQuery] FilmsFilterDTO filmsFilterDTO)
        {
            var filmsQueryable = _db.Films.AsQueryable();

            if (!string.IsNullOrEmpty(filmsFilterDTO.Title))
            {
                filmsQueryable = filmsQueryable.Where(x => x.Title.Contains(filmsFilterDTO.Title));
            }

            if (filmsFilterDTO.OnCine)
            {
                filmsQueryable = filmsQueryable.Where(x => x.OnCine);
            }

            if (filmsFilterDTO.NextPremiere)
            {
                var today = DateTime.Today;
                filmsQueryable = filmsQueryable.Where(x => x.LaunchDate > today);
            }

            if (filmsFilterDTO.GenderId != 0)
            {
                filmsQueryable = filmsQueryable.Where(x => x.FilmsGender.Select(y => y.GenderId).Contains(filmsFilterDTO.GenderId));
            }

            await HttpContext.InsertParametersInHeader(filmsQueryable);

            var films = await filmsQueryable.Pagination(filmsFilterDTO.PaginationDTO).ToListAsync();
            return _mapper.Map<List<FilmDTO>>(films);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromForm] FilmCreationDTO filmCreationDTO)
        {
            var film = _mapper.Map<Film>(filmCreationDTO);

            if(filmCreationDTO.Poster != null)
            {
                film.Poster = await _storeFiles.SaveFile(container, filmCreationDTO.Poster);
            }
            WriteActorsOrder(film);

            _db.Add(film);
            await _db.SaveChangesAsync();
            return film.Id;
        }

        private void WriteActorsOrder(Film film)
        {
            if(film.FilmsActors != null)
            {
                for(int i = 0; i < film.FilmsActors.Count; i++)
                {
                    film.FilmsActors[i].Order = i;
                }
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var film = await _db.Films.FirstOrDefaultAsync(x => x.Id == id);

            if(film == null)
            {
                return NoContent();
            }

            _db.Remove(film);
            await _db.SaveChangesAsync();

            await _storeFiles.DeleteFile(film.Poster, container);

            return NoContent();

        }
        
    }
}
