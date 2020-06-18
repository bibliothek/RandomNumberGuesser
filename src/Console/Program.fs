open System
open Game

let print indexedGuess =
    let i, guess = indexedGuess
    printfn "%i. Guess: %A" (i + 1) guess

let conclude game =
    printfn "Guesses:"
    game.Guesses
    |> List.rev
    |> List.indexed
    |> List.iter print

let rec getGuess() =
    printfn ""
    printfn "Make a guess:"
    let guessString = Console.ReadLine()
    match Int32.TryParse guessString with
    | true, number ->
        Guess number
    | false, _ ->
        printfn "Please type a number!"
        getGuess()

let rec play game =
        let guess = getGuess()
        let updatedGame = Game.guess game guess
        match updatedGame.State with
            | Finished ->
                printfn "Winner!"
                conclude updatedGame
                updatedGame
            | Guessing TooHigh ->
                printfn "Too High!"
                play updatedGame
            | Guessing TooLow ->
                printfn "Too Low!"
                play updatedGame
            | _ -> play updatedGame

let rec doYouWantToPlayAgain() =
    printfn ""
    printfn "Want to play again? (Y)es/(N)o"
    let key = Console.ReadKey().Key.ToString().ToLower()
    match key with
        | "y" -> true
        | "n" -> false
        | _ -> doYouWantToPlayAgain()

let rec gameLoop game =
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
