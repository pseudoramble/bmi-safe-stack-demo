open System.IO

open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Giraffe
open Saturn

open Api

let tryGetEnv = System.Environment.GetEnvironmentVariable >> function null | "" -> None | x -> Some x

let publicPath = Path.GetFullPath "../Client/public"

let port =
    "SERVER_PORT"
    |> tryGetEnv |> Option.map uint16 |> Option.defaultValue 8085us

let webApp : HttpHandler =
  Remoting.createApi ()
  |> Remoting.withRouteBuilder (fun typeName methodName -> sprintf "/api/%s/%s" typeName methodName)
  |> Remoting.fromValue bmiApi
  |> Remoting.buildHttpHandler

let app = application {
    url ("http://127.0.0.1:" + port.ToString() + "/")
    use_router webApp
    memory_cache
    use_static publicPath
    use_json_serializer(Thoth.Json.Giraffe.ThothSerializer())
    use_gzip
}

run app
