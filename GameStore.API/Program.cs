using GameStore.API.DTOs;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


List<GameDTO> games = [
   new (1,
   "Street fighter 2",
   "Fighting",
   19.99M,
   new DateOnly(1992, 7, 15)),
new (2,
   "Final Fantasy VII",
   "RPG",
   59.99M,
   new DateOnly(2004, 10, 15)),
new (3,
   "Astro bot",
   "Plataformer",
   49.99M,
   new DateOnly(2025, 7, 15)),  
];

// GET /games
app.MapGet("/games", () => games);

// get /games/{id}
app.MapGet("/games/{id}", (int id) => games.Find(game => game.Id == id))
   .WithName("GetGame");

// Post /games
app.MapPost("/games", (CreateGameDto newGame) =>
{
   GameDTO game = new(
      games.Count + 1,
      newGame.Name,
      newGame.Genre,
      newGame.Price,
      newGame.ReleaseDate
   );

   games.Add(game);

   return Results.CreatedAtRoute("GetGame", new {id = game.Id});
});

app.Run();
