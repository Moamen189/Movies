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


        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
           var Movies =  await context.Movies.OrderByDescending(x => x.Rate).Include(g => g.Genre).Select(m => new MovieDetailsDTO
           {
               Id= m.Id,
               GenreId= m.GenreId,
               GenreName  = m.Genre.Name,
               Poster = m.Poster,
               Rate = m.Rate,
               Storeline = m.Storeline,
               Title = m.Title,
               Year = m.Year

           }).ToListAsync();

            return Ok(Movies);
        }


        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var Movie = await context.Movies.Include(g => g.Genre).SingleOrDefaultAsync(x => x.Id == id);

            if(Movie == null )
            {
                return NotFound();
            }
            var IdValidation = context.Movies.Any(x => x.Id == id);

            if (!IdValidation)
            {
                return BadRequest("Id is not Found");
            }

            var dto = new MovieDetailsDTO
            {
                Id = Movie.Id,
                GenreId = Movie.GenreId,
                GenreName = Movie.Genre.Name,
                Poster = Movie.Poster,
                Rate = Movie.Rate,
                Storeline = Movie.Storeline,
                Title = Movie.Title,
                Year = Movie.Year

            };
            return Ok(dto);
        }
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

            context.SaveChanges();

            return Ok(Movie);
        }
    }
}
