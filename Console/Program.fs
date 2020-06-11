open System
open Game

let print indexedGuess =
    let i, guess = indexedGuess
    printfn "%i. Guess: %s" (i + 1) guess

let conclude game =
    printfn "Guesses:"
    game.Guesses
    |> List.rev
    |> List.indexed
    |> List.iter print

let rec play game =
        printfn ""
        printfn "Make a guess:"
        let guessString = Console.ReadLine()
        let updatedGame = {game with Guesses = guessString :: game.Guesses}
        let guessResult = Game.evaluate updatedGame
        match guessResult with
            | Correct ->
                printfn "Winner!"
                conclude updatedGame
                updatedGame
            | TooHigh ->
                printfn "Too High!"
                play updatedGame
            | TooLow ->
                printfn "Too Low!"
                play updatedGame
            | InvalidGuess ->
                printfn "Please type a number!"
                play updatedGame

let rec doYouWantToPlayAgain() =
    printfn ""
    printfn "Want to play again? (Y)es/(N)o"
    let key = Console.ReadKey().Key.ToString().ToLower()
    match key with
        | "y" -> true
        | "n" -> false
        | _ -> doYouWantToPlayAgain()

let rec gameLoop (game) =
    play game |> ignore
    if doYouWantToPlayAgain()
    then
        let newGame = Game.init()
        gameLoop newGame

[<EntryPoint>]
let main argv =
    let game = Game.init()
    gameLoop game |> ignore
    0 // return an integer exit code
