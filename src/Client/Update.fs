module Client.Update

open Game
open Elmish
open Client.BackendCalls
open Client.Model

type Msg =
    | UpdateGuess of int option
    | SubmitGuess
    | GuessResult of ClientGame
    | NewGameStarted of ClientGame
    | StartNewGame

let init firstTime: Model * Cmd<Msg> =
    let initialModel =
        { Game = None
          GuessInput = None
          Loading = true
          Initialization = firstTime}

    let initGameCommand =
        Cmd.OfPromise.perform newGame () NewGameStarted

    initialModel, initGameCommand

let guess currentModel game guessInput =
    let guessRequest = { Guess = guessInput }
    let promise = fun () -> makeGuess game.Id guessRequest

    let makeGuessCmd =
        Cmd.OfPromise.perform promise () GuessResult

    { currentModel with Loading = true }, makeGuessCmd


let update (msg: Msg) (currentModel: Model): Model * Cmd<Msg> =
    match currentModel.Game, msg with
    | Some game, SubmitGuess ->
        match currentModel.GuessInput with
        | Some guessInput -> guess currentModel game guessInput
        | None -> currentModel, Cmd.none
    | _, GuessResult result ->
        { currentModel with
              Game = Some result
              Loading = false
              GuessInput = None },
        Cmd.none
    | _, UpdateGuess guess -> { currentModel with GuessInput = guess }, Cmd.none
    | _, NewGameStarted game ->
        { Game = Some game
          GuessInput = None
          Loading = false
          Initialization = false},
        Cmd.none
    | _, StartNewGame -> init false
    | _ -> currentModel, Cmd.none
