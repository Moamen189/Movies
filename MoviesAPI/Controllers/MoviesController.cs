using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public MoviesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpPost]
        
        public async Task<IActionResult> Create([FromForm] MoviesDto dto)
        {
            using var DataStream = new MemoryStream();

            await dto.Poster.CopyToAsync(DataStream);
            var Movie = new Movie() { 
                Title = dto.Title , 
                Year = dto.Year ,
                Storeline = dto.Storeline ,
                Rate= dto.Rate ,
                Poster= DataStream.ToArray() ,
                GenreId = dto.GenreId ,
                    };

            await context.Movies.AddAsync(Movie); 
               
            context.SaveChangesAsync();

            return Ok(Movie);
        }
    }
}
