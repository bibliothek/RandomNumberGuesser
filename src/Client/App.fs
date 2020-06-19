module App

open Elmish
open Elmish.React
open Client.View
open Client.Update

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram (fun () -> init true) update view
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactBatched "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
