using System;
using GameStore.API.Dtos;

namespace GameStore.API.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";
    private static List<GameDto> games = [
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

    public static void MapGamesEndpoint(this WebApplication app)
    {

        var group = app.MapGroup("/games");


        // GET /games
        group.MapGet("/", () => games);


        // get /games/{id}
        group.MapGet("/{id}", (int id) =>
        {

            var game = games.Find(game => game.Id == id);

            return game is null ? Results.NotFound() : Results.Ok();

        })
           .WithName(GetGameEndpointName);

        // Post /games
        group.MapPost("/", (CreateGameDto newGame) =>
        {
            GameDto game = new
          (
             games.Count + 1,
             newGame.Name,
             newGame.Genre,
             newGame.Price,
             newGame.ReleaseDate
          );

            games.Add(game);

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        });

        //PUT /games/{id}
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            games[index] = new GameDto(
            id,
            updatedGame.Name,
            updatedGame.Genre,
            updatedGame.Price,
            updatedGame.ReleaseDate
            );

            return Results.Ok();



        });

        // DELETE /games/1
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);

            return Results.NoContent();
        });
    }

}
