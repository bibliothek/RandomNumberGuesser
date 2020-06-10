namespace Game

open System

type Game =
    { Guesses: string list
      NumberToGuess: int }

type GuessResult =
    | Correct
    | TooHigh
    | TooLow
    | NoGuess
    | InvalidGuess


module Game =
    let private getNumberToGuess () =
        let r = Random()
        r.Next(1, 101)

    let private tryParseInt s =
        try
            s |> int |> Some
        with :? FormatException -> None

    let evaluate game =
        match game.Guesses with
        | [] -> NoGuess
        | x :: _ ->
            match tryParseInt x with
            | Some i ->
                if i > game.NumberToGuess then TooHigh
                elif i < game.NumberToGuess then TooLow
                else Correct
            | None -> InvalidGuess

    let newGame () =
        { Guesses = List.empty
          NumberToGuess = getNumberToGuess () }
