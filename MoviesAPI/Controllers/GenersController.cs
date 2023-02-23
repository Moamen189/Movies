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


        [HttpPut("{id}")]

        public async Task<IActionResult> Update(int id, [FromBody] GenersDto dto)
        {
            var genre = await context.Genres.SingleOrDefaultAsync(g => g.Id == id);

            if(genre == null)
            {
                return NotFound($"Id is not found : {id}");
            }

            genre.Name = dto.Name;

            context.SaveChanges();

            return Ok(genre);

        }



        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)
        {
             var genre = await context.Genres.SingleOrDefaultAsync(gen => gen.Id == id);

            if(genre == null)
            {
                return NotFound($"Id Number {id} is Not Found");
            }

            context.Genres.Remove(genre);
            context.SaveChanges();

            return Ok(genre);

        }
    }
}
