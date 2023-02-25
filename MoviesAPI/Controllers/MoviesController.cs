using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MoviesAPI.Models;
using MoviesAPI.Services;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {

        private readonly IMoviesService moviesService;
        private readonly IGenresService genresService;
        private new List<string> _allowedExtenstions = new List<string> { ".jpg" , ".png"};

        private long _maxAllowedSize = 1048576;
        public MoviesController( IMoviesService moviesService , IGenresService genresService)
        {
            
            this.moviesService = moviesService;
            this.genresService = genresService;
        }


        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
           var Movies =  await moviesService.GetAll();

            return Ok(Movies);
        }
         

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var Movie = await moviesService.Getbyid(id);

            if(Movie == null )
            {
                return NotFound();
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

        [HttpGet("GetByGenreId")]
        public async Task<IActionResult> GetByGenreId(byte GenreId)
        {
            var Movies = await moviesService.GetAll(GenreId);

            return Ok(Movies);

        }

        [HttpPost]
        
        public async Task<IActionResult> Create([FromForm] MoviesDto dto)
        {
            if(dto.Poster == null)
            {
                return BadRequest("Posters is Required");
            }
            if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
            {
                return BadRequest("Only .png and .jpg are Allowed");
            }

            if(dto.Poster.Length> _maxAllowedSize)
            {
                return BadRequest("The Maximuim Size is 1 MB");
            }

            var isValidId = await genresService.IsValidGenre(dto.GenreId);

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

             moviesService.Add(Movie);

           
            return Ok(Movie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] MoviesDto dto)
        {

            var Movie = await moviesService.Getbyid(id);

            if (Movie == null)
            {
                return BadRequest("Not Found");
            }

            var isValidId = await genresService.IsValidGenre(dto.GenreId);

            if (!isValidId)
            {
                return BadRequest("This id is not Allowed");

            }

            if(dto.Poster != null)
            {
                if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                {
                    return BadRequest("Only .png and .jpg are Allowed");
                }


                if (dto.Poster.Length > _maxAllowedSize)
                {
                    return BadRequest("The Maximuim Size is 1 MB");
                }
                using var DataStream = new MemoryStream();

                await dto.Poster.CopyToAsync(DataStream);

                Movie.Poster = DataStream.ToArray();
            }


            Movie.Title = dto.Title;
            Movie.Year = dto.Year;
            Movie.Storeline = dto.Storeline;
            Movie.Rate = dto.Rate;
        
            Movie.GenreId = dto.GenreId;

            moviesService.Update(Movie);

            return Ok(Movie);


        }
        [HttpDelete("{Id}")]

        public async Task<IActionResult> Delete(int id)
        {
            var Movie =  await moviesService.Getbyid(id);

            if (Movie == null)
            {
                return BadRequest("Not Found"); 
            }

           moviesService.Delete(Movie);

            return Ok(Movie);
        }



    }
}
