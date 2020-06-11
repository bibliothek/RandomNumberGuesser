namespace Game

open System

type Game =
    { Guesses: string list
      NumberToGuess: int }

type GuessResult =
    | Correct
    | TooHigh
    | TooLow
    | InvalidGuess

module Game =
    let evaluate game =
        let guess = List.head game.Guesses
        match Int32.TryParse guess with
        | true, number ->
            if number > game.NumberToGuess then TooHigh
            elif number < game.NumberToGuess then TooLow
            else Correct
        | false, _ -> InvalidGuess

    let init () =
        { Guesses = List.empty
          NumberToGuess = Random().Next(1, 101) }
