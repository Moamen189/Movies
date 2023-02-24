using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        private new List<string> _allowedExtenstions = new List<string> { ".jpg" , ".png"};

        private long _maxAllowedSize = 1048576;
        public MoviesController(ApplicationDbContext context)
        {
            this.context = context;
        }


        //[HttpGet]

        //public async Task<IActionResult> Get()
        //{

        //}
        [HttpPost]
        
        public async Task<IActionResult> Create([FromForm] MoviesDto dto)
        {
            if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
            {
                return BadRequest("Only .png and .jpg are Allowed");
            }

            if(dto.Poster.Length> _maxAllowedSize)
            {
                return BadRequest("The Maximuim Size is 1 MB");
            }

            var isValidId = await context.Genres.AnyAsync(g => g.Id == dto.GenreId);

           if (!isValidId)
            {
                return BadRequest("This id is not Allowed");

            }
               
           
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
