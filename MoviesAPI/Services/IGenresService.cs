namespace MoviesAPI.Services
{
    public interface IGenresService
    {
        Task<IEnumerable<Genre>> GetAll();

        Task<Genre> GetById(byte id);
        Task<Genre> Add(Genre Genre);
        Task<bool> IsValidGenre(byte id);


        Genre Update(Genre Genre);

        Genre Delete(Genre Genre);
    }
}
