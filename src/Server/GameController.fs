module GameController

open System
open Game
open Saturn

type GameId = GameId of Guid

type PersistedGame =
    {Game: Game
     Id: GameId}

type GameResponse =
    {Id: GameId}

let toPersistedGame game =
    {Game = game
     Id = GameId (Guid.NewGuid())}

let toResponse (game: PersistedGame) =
    {Id = game.Id}

let gameController = controller {
    index (fun ctx -> Game.init() |> toPersistedGame |> toResponse |> Controller.json ctx)
}
