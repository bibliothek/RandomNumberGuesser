module Client.View

open Fable.React
open Fable.React.Props
open Client.Model
open Client.Update
open Client.View.Footer
open Client.View.Navbar
open Client.View.Content
open Browser
open Browser.Types

let keyHandler (event: Event) dispatch =
    let keyboardEvent = event :?> KeyboardEvent
    if keyboardEvent.key = "Enter" then SubmitGuess |> dispatch


let view (model: Model) (dispatch: Msg -> unit) =
    if model.Initialization then document.addEventListener ("keydown", fun e -> keyHandler e dispatch)
    div [ ClassName "height-100" ]
        [ navbar
          content model dispatch
          footer ]
