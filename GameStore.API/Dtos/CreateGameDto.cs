using System.ComponentModel.DataAnnotations;

namespace GameStore.API.Dtos;

public record class CreateGameDto(
   [Required][StringLength(50)] string Name,
   [Required][Range(1, 50)] int GenreId,
   [Required][Range(1, 100)] decimal Price,
   DateOnly ReleaseDate
);
