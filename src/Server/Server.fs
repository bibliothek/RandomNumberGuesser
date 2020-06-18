module Server

open System.Collections.Generic
open System.IO

open System
open Microsoft.Extensions.DependencyInjection
open Saturn
open GameRepository.InMemory

let tryGetEnv key =
    match Environment.GetEnvironmentVariable key with
    | x when String.IsNullOrWhiteSpace x -> None
    | x -> Some x

let publicPath = Path.GetFullPath "../Client/public"

let port =
    "SERVER_PORT"
    |> tryGetEnv
    |> Option.map uint16
    |> Option.defaultValue 8085us


let serviceConfig (serviceCollection: IServiceCollection) =
    let inMemory = Dictionary<Guid, PersistedGame>()
    serviceCollection.AddInMemoryGameStore(inMemory) |> ignore
    serviceCollection

let apiRouter =
    router { forward "/api/games" GameController.handlers }

let app =
    application {
        url ("http://0.0.0.0:" + port.ToString() + "/")
        memory_cache
        use_router apiRouter
        service_config serviceConfig
        use_static publicPath
        use_json_serializer (Thoth.Json.Giraffe.ThothSerializer())
        use_gzip
    }

run app
