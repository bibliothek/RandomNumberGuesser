module Client.View

open Fable.React
open Fable.React.Props
open Client.Model
open Client.Update
open Client.View.Footer
open Client.View.Navbar
open Client.View.Content

let view (model: Model) (dispatch: Msg -> unit) =
    div [ ClassName "height-100" ]
        [ navbar
          content model dispatch
          footer ]
