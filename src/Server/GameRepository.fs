module GameRepository.InMemory

open System
open System.Collections.Generic
open Microsoft.Extensions.DependencyInjection
open Game

type PersistedGame = { Game: Game; Id: Guid }

type FindGame = Guid -> PersistedGame
type SaveGame = PersistedGame -> PersistedGame
type AllGames = unit -> PersistedGame list

let toPersistedGame game id = { Game = game; Id = id }

let find (inMemory: Dictionary<Guid, PersistedGame>) (id: Guid): PersistedGame = inMemory.GetValueOrDefault id

let save (inMemory: Dictionary<Guid, PersistedGame>) (game: PersistedGame) =
    inMemory.[game.Id] <- game
    game

let all (inMemory: Dictionary<Guid, PersistedGame>): PersistedGame list =
    inMemory.Values |> Seq.cast |> List.ofSeq

type IServiceCollection with
    member this.AddInMemoryGameStore(dict: Dictionary<Guid, PersistedGame>) =
        this.AddSingleton<FindGame>(find dict) |> ignore
        this.AddSingleton<SaveGame>(save dict) |> ignore
        let allPointer = fun () -> all dict
        this.AddSingleton<AllGames>(allPointer) |> ignore
