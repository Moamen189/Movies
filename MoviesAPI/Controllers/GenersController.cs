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
            var geners = await context.Genres.ToListAsync();

            return Ok(geners);
        }
    }
}
