using Microsoft.EntityFrameworkCore;

namespace MoviesAPI.Services
{
    public class GenresService : IGenresService
    {

        private readonly ApplicationDbContext context;

        public GenresService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Genre> Add(Genre Genre)
        {
            

            await context.Genres.AddAsync(Genre);
            context.SaveChanges();

            return Genre;
        }

        public Task<Genre> Delete(Genre Genre)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Genre>> GetAll()
        {
            var genres = await context.Genres.OrderBy(g => g.Name).ToListAsync();

            return genres;
        }

        public Task<Genre> Update(Genre Genre)
        {
            throw new NotImplementedException();
        }
    }
}
