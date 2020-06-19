module Client.View.Footer

open Fulma
open Fable.React
open Fable.React.Props

let safeComponents =
    let components =
        span []
            [ a [ Href "https://github.com/SAFE-Stack/SAFE-template" ] [ str "SAFE  "; str Version.template ]
              str ", "
              a [ Href "https://saturnframework.github.io" ] [ str "Saturn" ]
              str ", "
              a [ Href "http://fable.io" ] [ str "Fable" ]
              str ", "
              a [ Href "https://elmish.github.io" ] [ str "Elmish" ]
              str ", "
              a [ Href "https://fulma.github.io/Fulma" ] [ str "Fulma" ] ]

    span []
        [ str "Version "
          strong [] [ str Version.app ]
          str " powered by: "
          components ]

let footer =
    Footer.footer []
        [ Content.content [ Content.Modifiers [ Modifier.TextAlignment(Screen.All, TextAlignment.Centered) ] ]
              [ safeComponents ] ]
