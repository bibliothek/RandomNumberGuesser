module Client.View.Navbar

open Fulma
open Fable.React

let navbar =
    Navbar.navbar [ Navbar.Color IsPrimary ] [ Navbar.Item.div [] [ Heading.h2 [] [ str "Random Number Guesser" ] ] ]
