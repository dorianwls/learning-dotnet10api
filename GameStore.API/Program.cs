using GameStore.API.DTOs;

const string GetGameEndpointName = "GetGame";

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();


List<GameDto> games = [
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
   .WithName(GetGameEndpointName);

// Post /games
app.MapPost("/games", (CreateGameDto newGame) =>
{
   GameDto game = new(
      games.Count + 1,
      newGame.Name,
      newGame.Genre,
      newGame.Price,
      newGame.ReleaseDate
   );

   games.Add(game);

   return Results.CreatedAtRoute(GetGameEndpointName, new {id = game.Id}, game);
});

app.Run();
