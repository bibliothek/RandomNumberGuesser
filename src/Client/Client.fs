module Client

open System
open Elmish
open Elmish.React
open Fable.Core
open Fable.React
open Fable.React.Props
open Game
open Fulma
open Thoth
open Thoth.Fetch

type Counter = { Value: int }

type Model = {
    Game: ClientGame option
    GuessInput: int option
    Loading: bool
}

type Msg =
    | UpdateGuess of int
    | SubmitGuess
    | GuessResult of ClientGame
    | NewGameStarted of ClientGame
    | StartNewGame


let newGame (): JS.Promise<ClientGame> =
    promise { return! Fetch.post "/api/games/" }

let makeGuess (id: Guid) (guessRequest: GuessRequest): JS.Promise<ClientGame> =
    promise { return! Fetch.post ((sprintf "/api/games/%A/guess" id), guessRequest)}

let init (): Model * Cmd<Msg> =
    let initialModel = { Game = None; GuessInput = None; Loading = true }
    let initGameCommand =
        Cmd.OfPromise.perform newGame () NewGameStarted
    initialModel, initGameCommand

let guess currentModel game guessInput =
    let guessRequest = {Guess = guessInput}
    let promise = fun () -> makeGuess game.Id guessRequest
    let makeGuessCmd = Cmd.OfPromise.perform promise () GuessResult
    {currentModel with Loading = true}, makeGuessCmd


let update (msg: Msg) (currentModel: Model): Model * Cmd<Msg> =
    match currentModel.Game, msg with
    | Some game, SubmitGuess ->
        match currentModel.GuessInput with
        | Some guessInput ->
            guess currentModel game guessInput
        | None -> currentModel, Cmd.none
    | _, GuessResult result ->
        {currentModel with Game = Some result; Loading = false}, Cmd.none
    | _, UpdateGuess guess -> {currentModel with GuessInput = Some guess}, Cmd.none
    | _, NewGameStarted game ->
        let nextModel = { Game = Some game; GuessInput = None; Loading = false }
        nextModel, Cmd.none
    | _, StartNewGame -> init ()
    | _ -> currentModel, Cmd.none


let safeComponents =
    let components =
        span []
            [ a [ Href "https://github.com/SAFE-Stack/SAFE-template" ] [ str "SAFE  "; str Version.template ]
              str ", "
              a [ Href "https://saturnframework.github.io" ] [ str "Saturn" ]
              str ", "
              a [ Href "http://fable.io" ] [ str "Fable" ]
              str ", "
              a [ Href "https://elmish.github.io" ] [ str "Elmish" ]
              str ", "
              a [ Href "https://fulma.github.io/Fulma" ] [ str "Fulma" ] ]

    span []
        [ str "Version "
          strong [] [ str Version.app ]
          str " powered by: "
          components ]

let show =
    function
    | { Game = None } | { Loading = true } -> "Loading..."
    | { Game = Some game } ->
        match game.State with
        | Started -> "Make a guess!"
        | Guessing TooHigh -> "The last guess was too high. Try again!"
        | Guessing TooLow -> "The last guess was too low. Try again!"
        | Finished -> "You did it!"
        | _ -> game.State |> string

let button txt onClick =
    Button.button
        [ Button.IsFullWidth
          Button.Color IsPrimary
          Button.OnClick onClick ] [ str txt ]

let textOrLoading text model =
    if model.Loading then "Loading" else text

let newGameButton model dispatch : ReactElement =
    Button.button [
        Button.OnClick (fun _ -> dispatch  StartNewGame)
        Button.Disabled model.Loading
        Button.CustomClass "is-primary is-fullwidth"
    ] [str (textOrLoading "New Game" model)]

let guessButton (model: Model) dispatch : ReactElement =
    Button.button [
        Button.OnClick (fun _ -> dispatch SubmitGuess)
        Button.Disabled (model.GuessInput.IsNone || model.Loading)
        Button.CustomClass "is-primary is-fullwidth"
    ] [str (textOrLoading "Guess" model)]

let getButton model dispatch =
    match model.Game with
    | Some game ->
        match game.State with
        | Started | Guessing _ -> guessButton model dispatch
        | Finished -> newGameButton model dispatch
    | None -> newGameButton model dispatch

let inputValue model =
    match model.GuessInput with
    | None -> ""
    | Some i -> string i

let guesses model =
    match model.Game with
    | None -> div [] []
    | Some game ->
        ul [] (game.PastGuesses
                |> List.map (fun guess ->
                                let (Guess guessInt) = guess
                                li [] [str (string guessInt)]
                            ))

let view (model: Model) (dispatch: Msg -> unit) =
    div [ ClassName "height-100" ]
        [ Navbar.navbar [ Navbar.Color IsPrimary ]
              [ Navbar.Item.div [] [ Heading.h2 [] [ str "Random Number Guesser" ] ] ]

          Container.container []
              [ Content.content [ Content.Modifiers [ Modifier.TextAlignment(Screen.All, TextAlignment.Centered) ] ]
                    [ Heading.h3 [] [ str "Guess the number between 1 and 100"]
                      Heading.h4 [] [show model |> str]
                    ]
                Input.number [
                    Input.Placeholder "1-100"
                    Input.Id "number-input"
                    Input.OnChange (fun e -> e.Value |> int |> UpdateGuess |> dispatch )
                    Input.Value (inputValue model)
                ]
                getButton model dispatch
                h5 [] [str "Previous guesses:"]
                guesses model
              ]

          Footer.footer []
              [ Content.content [ Content.Modifiers [ Modifier.TextAlignment(Screen.All, TextAlignment.Centered) ] ]
                    [ safeComponents ] ] ]

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram init update view
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactBatched "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
