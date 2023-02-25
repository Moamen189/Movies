using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Services;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenersController : ControllerBase
    {
     
        private readonly IGenresService genresService;

        public GenersController( IGenresService genresService)
        {
          
            this.genresService = genresService;
        }
        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            var geners = await genresService.GetAll();

            return Ok(geners);
        }

        [HttpPost]

        public async Task<IActionResult> Create([FromBody]GenersDto dto)
        {
            var genre = new Genre { Name = dto.Name };

            await genresService.Add(genre); 

            return Ok(genre);
        }


        [HttpPut("{id}")]

        public async Task<IActionResult> Update(byte id, [FromBody] GenersDto dto)
        {
            var genre = await genresService.GetById(id);

            if(genre == null)
            {
                return NotFound($"Id is not found : {id}");
            }

            genre.Name = dto.Name;

            genresService.Update(genre);

            return Ok(genre);

        }



        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(byte id)
        {
            var genre = await genresService.GetById(id);

            if(genre == null)
            {
                return NotFound($"Id Number {id} is Not Found");
            }

            genresService.Delete(genre);

            return Ok(genre);

        }
    }
}
