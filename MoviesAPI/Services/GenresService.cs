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

        public Genre Delete(Genre Genre)
        {
           
            context.Genres.Remove(Genre);

            context.SaveChanges();

            return Genre;
        }

        public async Task<IEnumerable<Genre>> GetAll()
        {
            var genres = await context.Genres.OrderBy(g => g.Name).ToListAsync();

            return genres;
        }

        public async Task<Genre> GetById(byte id)
        {
            var genre = await context.Genres.SingleOrDefaultAsync(gen => gen.Id == id);

            return genre;
        }

        public async Task<bool> IsValidGenre(byte id)
        {
            return await context.Genres.AnyAsync(g => g.Id == id);
        }

        public Genre Update(Genre Genre)
        {
            context.Genres.Update(Genre);

            context.SaveChanges();

            return Genre;
        }
    }
}
