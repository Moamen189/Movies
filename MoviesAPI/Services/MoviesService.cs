using Microsoft.EntityFrameworkCore;

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
            await context.Movies.AddAsync(Movie);

            context.SaveChanges();

            return movie;
        }

        public Movie Delete(Movie movie)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Movie>> GetAll()
        {
          return  await context.Movies.OrderByDescending(x => x.Rate).Include(g => g.Genre).ToListAsync();
        }

        public async Task<Movie> Getbyid(int id)
        {
            return await context.Movies.Include(g => g.Genre).SingleOrDefaultAsync(x => x.Id == id);
        }

        public Movie Update(Movie movie)
        {
            throw new NotImplementedException();
        }
    }
}
