module Client

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props
open Fable.Core.JsInterop
open Fetch.Types
open Thoth.Fetch
open Fulma
open Thoth.Json

open Messages
open Shared

type Model = { description: BodyDescription option }

// defines the initial state and initial command (= side-effect) of the application
let init () : Model * Cmd<Msg> =
  let initialModel = { description = Some { height = 0; weight = 0 } }
  (initialModel, Cmd.none)

// The update function computes the next state of the application based on the
//  current state and the incoming events/messages
// It can also run side-effects (encoded as commands) like calling the server via Http.
// these commands in turn, can dispatch messages to which the update function will react.
let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> =
  match currentModel.description, msg with
    | Some desc, UpdateHeight h ->
      let nextModel = {
        currentModel with description = Some { height = h; weight = desc.weight } }
      (nextModel, Cmd.none)
    | Some desc, UpdateWeight w ->
      let nextModel = {
        currentModel with description = Some { height = desc.height; weight = w } }
      (nextModel, Cmd.none)
    | Some desc, Calculate ->
      (currentModel, Api.calculate desc)
    | _ -> (currentModel, Cmd.none)

let view (model : Model) (dispatch : Msg -> unit) =
  div [] [
    Navbar.navbar [ Navbar.Color IsPrimary ] [
      Navbar.Item.div [ ] [
        Heading.h2 [ ] [ str "BMI Calculator" ]
      ]
    ]

    Container.container [] [
      Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ] [
        Heading.h3 [] [ str ("Determine your BMI" ) ]
      ]
      BmiForm.form {
        onCalculate = (fun _ -> dispatch Calculate)
        onHeightChange = (fun e -> dispatch (UpdateHeight (int32 !!e.target?value)))
        onWeightChange = (fun e -> dispatch (UpdateWeight (int32 !!e.target?value)))
      }
    ]

    Footer.footer [ ] [
      Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ] [
        str "lol thx <3"
      ]
    ]
  ]

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram init update view
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactBatched "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
