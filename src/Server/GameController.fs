module GameController

open System
open FSharp.Control.Tasks
open Game
open Microsoft.AspNetCore.Http
open Giraffe
open GameRepository.InMemory

type GameResponse =
    { Id: Guid
      PastGuesses: Guess list
      State: GameState }

[<CLIMutable>]
type GuessRequest = { Guess: int }

let toResponse (game: PersistedGame) =
    { Id = game.Id
      PastGuesses = game.Game.Guesses
      State = game.Game.State }

let handlers: HttpFunc -> HttpContext -> HttpFuncResult =
    choose
        [ GET
          >=> route "/"
          >=> fun next ctx ->
              let response =
                  ctx.GetService<AllGames> () ()
                  |> List.map toResponse

              json response next ctx
          POST
          >=> route "/"
          >=> fun next ctx ->
              let game =
                  Game.init ()
                  |> toPersistedGame <| Guid.NewGuid()
                  |> ctx.GetService<SaveGame>()
                  |> toResponse

              json game next ctx
          GET
          >=> routef "/%s" (fun id next ctx ->
                  let game =
                      id
                      |> Guid.Parse
                      |> ctx.GetService<FindGame>()
                      |> toResponse

                  json game next ctx)
          POST
          >=> routef "/%s/guess" (fun id next ctx ->
                  task {
                      let! guess = ctx.BindJsonAsync<GuessRequest>()
                      let guid = id |> Guid.Parse

                      let response =
                          guid
                          |> ctx.GetService<FindGame>()
                          |> fun game -> game.Game
                          |> Game.guess <| Guess guess.Guess
                          |> toPersistedGame <| guid
                          |> ctx.GetService<SaveGame>()
                          |> toResponse

                      return! json response next ctx
                  }) ]
