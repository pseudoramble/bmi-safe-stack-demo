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
  let success = fun res -> ShowCalcuationReuslt res

  let failure = fun _ -> ShowCalculationError

  Cmd.OfAsync.either api.calculate desc success failure