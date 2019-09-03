module Api

open Elmish
open Fable.Remoting.Client

open Messages
open Shared

let api =
  Remoting.createApi ()
  |> Remoting.withBaseUrl "/api"
  |> Remoting.buildProxy<IBmiProtocol>

let calculate desc =
  let success = fun res ->
    match res with
      Ok value -> ShowCalcuationReuslt value
      | Error msg -> ShowCalculationError msg

  let failure = fun (err: exn) -> ShowCalculationError err.Message

  Cmd.OfAsync.either api.calculate desc success failure