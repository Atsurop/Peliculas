using back_end.Data;
using back_end.DTO;
using back_end.Models;
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
    [Route("api/rating")]
    public class RatingsController: ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _db;
        public RatingsController(UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }
       [HttpPost]
       [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post([FromBody] RatingDTO ratingDTO)
        {
            var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email").Value;
            var user = await _userManager.FindByEmailAsync(email);
            var userId = user.Id;

            var actualRating = await _db.Ratings.FirstOrDefaultAsync(x => x.FilmId == ratingDTO.FilmId && x.UserId == userId);

            if(actualRating == null)
            {
                var rating = new Rating();
                rating.FilmId = ratingDTO.FilmId;
                rating.Score = ratingDTO.Score;
                rating.UserId = userId;
                _db.Add(rating);
            }
            else
            {
                actualRating.Score = ratingDTO.Score;
            }
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
