module Client.Model

open Game

type Model =
    { Game: ClientGame option
      GuessInput: int option
      Loading: bool }
