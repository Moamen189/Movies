using Microsoft.EntityFrameworkCore;
using MoviesAPI.Models;

namespace MoviesAPI.Services
{
    public class MoviesService : IMoviesService
    {
        private readonly ApplicationDbContext context;

        private new List<string> _allowedExtenstions = new List<string> { ".jpg", ".png" };

        private long _maxAllowedSize = 1048576;
        public MoviesService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Movie> Add(Movie movie)
        {
            await context.Movies.AddAsync(movie);

            context.SaveChanges();

            return movie;
        }

        public Movie Delete(Movie movie)
        {
            context.Remove(movie);
            context.SaveChanges();

            return movie;
        }

        public async Task<IEnumerable<Movie>> GetAll(byte genreId = 0)
        {
          return  await context.Movies.Where(m => m.GenreId == genreId || genreId ==0)
                .OrderByDescending(x => x.Rate).Include(g => g.Genre).ToListAsync();
        }

        public async Task<Movie> Getbyid(int id)
        {
            return await context.Movies.Include(g => g.Genre).SingleOrDefaultAsync(x => x.Id == id);
        }

        public Movie Update(Movie movie)
        {
            context.Update(movie);
            context.SaveChanges();

            return movie;
        }
    }
}
