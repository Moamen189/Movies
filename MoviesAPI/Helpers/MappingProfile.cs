using AutoMapper;

namespace MoviesAPI.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie , MovieDetailsDTO>();
            CreateMap<MovieDetailsDTO, Movie>().ForMember(src => src.Poster , opt => opt.Ignore());

        }
    }
}
