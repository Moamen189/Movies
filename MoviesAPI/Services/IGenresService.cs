namespace MoviesAPI.Services
{
    public interface IGenresService
    {
        Task<IEnumerable<Genre>> GetAll();

        Task<Genre> Add(Genre Genre);

        Task<Genre> Update(Genre Genre);

        Task<Genre> Delete(Genre Genre);
    }
}
