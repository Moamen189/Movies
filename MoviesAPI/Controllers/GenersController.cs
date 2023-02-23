using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenersController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public GenersController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            var geners = await context.Genres.OrderBy(g => g.Name).ToListAsync();

            return Ok(geners);
        }

        [HttpPost]

        public async Task<IActionResult> Create([FromBody]GenersDto dto)
        {
            var genre = new Genre { Name = dto.Name };

            await context.Genres.AddAsync(genre);
            context.SaveChanges();  

            return Ok(genre);
        }
    }
}
