using AutoMapper;
using back_end.DTO;
using back_end.Models;
using Microsoft.AspNetCore.Identity;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory)
        {
            CreateMap<Genders, GenderDTO>().ReverseMap();
            CreateMap<GenderCreationDTO, Genders>();
            CreateMap<Actors, ActorDTO>().ReverseMap();
            CreateMap<ActorCreationDTO, Actors>().ForMember(x => x.Picture, options => options.Ignore());
            CreateMap<CineCreationDTO, Cines>().ForMember(x => x.Location, x => x.MapFrom(dto => geometryFactory.CreatePoint(new Coordinate(dto.Length, dto.Latitude))));
            CreateMap<Cines, CineDTO>().ForMember(x => x.Latitude, dto => dto.MapFrom(field => field.Location.Y)).ForMember(x => x.Length, dto => dto.MapFrom(field => field.Location.X));
            CreateMap<FilmCreationDTO, Film>()
                .ForMember(x => x.Poster, options => options.Ignore())
                .ForMember(x => x.FilmsGender, options => options.MapFrom(MappFilmsGenders))
                .ForMember(x => x.FilmsCines, options => options.MapFrom(MappFilmsCines))
                .ForMember(x => x.FilmsActors, options => options.MapFrom(MappFilmsActors));
            CreateMap<Film, FilmDTO>()
                .ForMember(x => x.Genders, options => options.MapFrom(MappFilmsGenders))
                .ForMember(x => x.Actors, options => options.MapFrom(MappFilmsActors))
                .ForMember(x => x.Cines, options => options.MapFrom(MappFilmsCine));
            CreateMap<IdentityUser, UserDTO>();



        }

        private List<GenderDTO> MappFilmsGenders(Film film, FilmDTO filmDTO)
        {
            var result = new List<GenderDTO>();

            if (film.FilmsGender != null)
            {
                foreach (var gender in film.FilmsGender)
                {
                    result.Add(new GenderDTO() { Id = gender.GenderId, Name = gender.Gender.Name });
                }
            }
            return result;
        }

        private List<FilmActorDTO> MappFilmsActors(Film film, FilmDTO filmDTO)
        {
            var result = new List<FilmActorDTO>();

            if (film.FilmsActors != null)
            {
                foreach (var filmActor in film.FilmsActors)
                {
                    result.Add(new FilmActorDTO()
                    {
                        Id = filmActor.ActorId,
                        Name = filmActor.Actor.Name,
                        Picture = filmActor.Actor.Picture,
                        Order = filmActor.Order,
                        Character = filmActor.Character
                    });

                }
            }
            return result;
        }

        private List<CineDTO> MappFilmsCine(Film film, FilmDTO filmDTO)
        {
            var result = new List<CineDTO>();

            if(film.FilmsCines != null)
            {
                foreach(var cines in film.FilmsCines)
                {
                    result.Add(new CineDTO() { Id = cines.CineId, Name = cines.Cine.Name, Latitude = cines.Cine.Location.Y, Length = cines.Cine.Location.X });
                }
            }
            return result;
        }

        private List<FilmsGenders> MappFilmsGenders(FilmCreationDTO filmCreationDTO, Film film)
        {
            var result = new List<FilmsGenders>();

            if(filmCreationDTO.GendersIds == null) { return result; }

            foreach(var id in filmCreationDTO.GendersIds)
            {
                result.Add(new FilmsGenders() { GenderId = id });
            }
            return result;
        }


        private List<FilmsCine> MappFilmsCines(FilmCreationDTO filmCreationDTO, Film film)
        {
            var result = new List<FilmsCine>();

            if (filmCreationDTO.CinesIds == null) { return result; }

            foreach (var id in filmCreationDTO.CinesIds)
            {
                result.Add(new FilmsCine() { CineId = id });
            }
            return result;
        }

        private List<FilmsActors> MappFilmsActors(FilmCreationDTO filmCreationDTO, Film film)
        {
            var result = new List<FilmsActors>();

            if (filmCreationDTO.Actors == null) { return result; }

            foreach (var actor in filmCreationDTO.Actors)
            {
                result.Add(new FilmsActors() { ActorId = actor.Id, Character = actor.Character });
            }
            return result;
        }
    }
}
