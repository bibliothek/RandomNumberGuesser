module Client.BackendCalls

open Fable.Core
open System
open Thoth
open Thoth.Fetch
open Game

let newGame (): JS.Promise<ClientGame> =
    promise { return! Fetch.post "/api/games/" }

let makeGuess (id: Guid) (guessRequest: GuessRequest): JS.Promise<ClientGame> =
    promise { return! Fetch.post ((sprintf "/api/games/%A/guess" id), guessRequest) }
