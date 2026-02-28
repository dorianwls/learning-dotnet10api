using System;
using GameStore.API.Data;
using GameStore.API.Dtos;
using GameStore.API.Models;

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
      group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) =>
      {
         Game game = new()
         {
            Name = newGame.Name,
            GenreId = newGame.GenreId,
            Price = newGame.Price,
            ReleaseDate = newGame.ReleaseDate
         };

         dbContext.Games.Add(game);
         dbContext.SaveChanges();

         GameDetailsDto gameDto = new(
            game.Id,
            game.Name,
            game.GenreId,
            game.Price,
            game.ReleaseDate
         );

         return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, gameDto);
      });

      //PUT /games/{id}
      group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
      {
         var index = games.FindIndex(game => game.Id == id);

         if (index == -1)
         {
            return Results.NotFound();
         }


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
