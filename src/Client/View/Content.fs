module Client.View.Content

open System
open Fable.React
open Game
open Fulma
open Client.Model
open Client.Update

let show model =
    match model with
    | { Game = None } | { Loading = true } -> "Loading..."
    | { Game = Some game } ->
        match game.State with
        | Started -> "Make a guess!"
        | Guessing TooHigh -> "The last guess was too high. Try again!"
        | Guessing TooLow -> "The last guess was too low. Try again!"
        | Finished -> "You did it!"
        | _ -> game.State |> string

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


let guesses model =
    match model.Game with
    | None -> div [] []
    | Some game ->
        ol [] (game.PastGuesses
                |> List.rev
                |> List.map (fun guess ->
                                let (Guess guessInt) = guess
                                li [] [str (string guessInt)]
                            ))

let inputValue model =
    match model.GuessInput with
    | None -> ""
    | Some i -> string i

let numberInput model dispatch =
    let parseInput (event: Browser.Types.Event) =
        match Int32.TryParse event.Value with
        | true, number -> UpdateGuess (Some number)
        | false, _ -> UpdateGuess None
    Input.number [
        Input.Placeholder "1-100"
        Input.Value (inputValue model)
        Input.OnChange (parseInput >> dispatch)
    ]

let content model dispatch =
    Container.container []
      [ Content.content [ Content.Modifiers [ Modifier.TextAlignment(Screen.All, TextAlignment.Centered) ] ]
            [ Heading.h3 [] [ str "Guess the number between 1 and 100"]
              Heading.h4 [] [show model |> str]
            ]
        numberInput model dispatch
        getButton model dispatch
        Content.content [ Content.Modifiers [ Modifier.TextAlignment(Screen.All, TextAlignment.Left) ]] [
            h5 [] [str "Your guesses:"]
            guesses model
        ]
      ]