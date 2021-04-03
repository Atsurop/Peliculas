using back_end.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Data
{
    public class ApplicationDbContext: IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FilmsActors>().HasKey(x => new {  x.FilmId, x.ActorId, });
            modelBuilder.Entity<FilmsGenders>().HasKey(x => new { x.FilmId, x.GenderId });
            modelBuilder.Entity<FilmsCine>().HasKey(x => new { x.FilmId, x.CineId });
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Genders> Genders { get; set; }

        public DbSet<Actors> Actors { get; set; }

        public DbSet<Cines> Cines { get; set; }
        public DbSet<Film> Films  { get; set; }
        public DbSet<FilmsActors> FilmsActors { get; set; }
        public DbSet<FilmsGenders> FilmsGenders { get; set; }
        public DbSet<FilmsCine> FilmsCines { get; set; }
        public DbSet<Rating> Ratings { get; set; }
    }
}
