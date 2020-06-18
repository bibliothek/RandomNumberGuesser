namespace Game

open System

type Guess = Guess of int

type GuessResult =
    | Correct
    | TooHigh
    | TooLow

type GameState =
    | Started
    | Guessing of GuessResult
    | Finished

type Game =
    { Guesses: Guess list
      NumberToGuess: int
      State: GameState }


module Game =
    let private evaluate game =
        let (Guess guessAsInt) = List.head game.Guesses
        if guessAsInt > game.NumberToGuess then TooHigh
        elif guessAsInt < game.NumberToGuess then TooLow
        else Correct

    let guess game guess =
        let updatedGame = { game with Guesses = guess :: game.Guesses }
        let result = evaluate updatedGame
        match result with
        | TooHigh | TooLow -> { updatedGame with State = Guessing result  }
        | Correct -> {updatedGame with State = Finished}

    let init () =
        { Guesses = List.empty
          NumberToGuess = Random().Next(1, 101)
          State = Started }
