namespace MoviesAPI.DTOs
{
    public class GenersDto
    {
        [MaxLength(100)]
        [Required]
        public string ? Name { get; set; } 
    }
}
