namespace Game

open System

type Guess = Guess of int

type Game =
    { Guesses: Guess list
      NumberToGuess: int }

type GuessResult =
    | Correct
    | TooHigh
    | TooLow

module Game =
    let evaluate game =
        let (Guess guessAsInt) = List.head game.Guesses
        if guessAsInt > game.NumberToGuess then TooHigh
        elif guessAsInt < game.NumberToGuess then TooLow
        else Correct

    let init () =
        { Guesses = List.empty
          NumberToGuess = Random().Next(1, 101) }
