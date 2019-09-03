module Client

open Elmish
open Elmish.React
open Fable.React
open Fable.Core.JsInterop
open Fulma

open Messages
open Shared

type Model = {
  description: BodyDescription option
  result: Result<Bmi, string>
  }

let init () : Model * Cmd<Msg> =
  let initialModel = {
    description = Some { height = 0; weight = 0 }
    result = Error ""
    }
  (initialModel, Cmd.none)

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
    | Some desc, ShowCalcuationReuslt bmi ->
      let nextModel = { currentModel with result = Ok bmi }
      (nextModel, Cmd.none)
    | _, ShowCalculationError msg ->
      let nextModel = { currentModel with result = Error msg  }
      (nextModel, Cmd.none)
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

    Container.container [] [
      Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ] [
        Heading.h4 [] [
          str <|
            match model.result with
              Ok res -> sprintf "Your BMI is %f" res.value
              | Error msg when msg <> "" -> sprintf "ERROR: %s. Enter values and press calculate to determine BMI" msg
              | Error msg when msg = "" -> sprintf "Enter values and press calculate to determine BMI"
        ]
      ]
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
