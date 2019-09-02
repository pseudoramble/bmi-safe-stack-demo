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

open Shared

// The model holds data that you want to keep track of while the application is running
// in this case, we are keeping track of a counter
// we mark it as optional, because initially it will not be available from the client
// the initial value will be requested from server
type Model = { Counter: Counter option }

// The Msg type defines what events/actions can occur while the application is running
// the state of the application changes *only* in reaction to these events
type Msg =
  | Increment
  | Decrement
  | InitialCountLoaded of Counter

let initialCounter () = Fetch.fetchAs<Counter> "/api/init"

// defines the initial state and initial command (= side-effect) of the application
let init () : Model * Cmd<Msg> =
  let initialModel = { Counter = None }
  let loadCountCmd =
    Cmd.OfPromise.perform initialCounter () InitialCountLoaded
  (initialModel, loadCountCmd)

// The update function computes the next state of the application based on the current state and the incoming events/messages
// It can also run side-effects (encoded as commands) like calling the server via Http.
// these commands in turn, can dispatch messages to which the update function will react.
let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> =
  match currentModel.Counter, msg with
    | Some counter, Increment ->
      let nextModel = { currentModel with Counter = Some { Value = counter.Value + 1 } }
      (nextModel, Cmd.none)
    | Some counter, Decrement ->
      let nextModel = { currentModel with Counter = Some { Value = counter.Value - 1 } }
      (nextModel, Cmd.none)
    | _, InitialCountLoaded initialCount->
      let nextModel = { Counter = Some initialCount }
      (nextModel, Cmd.none)
    | _ ->
      (currentModel, Cmd.none)


let show = function
  | { Counter = Some counter } -> string counter.Value
  | { Counter = None   } -> "Loading..."


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
        onChange = (fun e -> printfn "%A" (!!e.target?value));
        onCalculate = (fun _ -> printfn "lol")
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
