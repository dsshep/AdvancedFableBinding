namespace App

open Fable.Core
open Feliz
open Fetch

type User = {
  Username: string
}

type Components =
  [<ReactComponent>]
  static member MswComponent() =
    let apiDataOpt, setApiData = React.useState None

    let loadData() = async {
      do! Async.Sleep 1_000
      let! response = 
        fetch "/api/user" [] 
        |> Async.AwaitPromise

      let! json = response.json<User>() |> Async.AwaitPromise
      Some json |> setApiData
    }

    React.useEffect(loadData >> Async.StartImmediate, [| |])

    Html.div [
      Html.p [
        match apiDataOpt with
        | Some user -> prop.text ($"The user is {user.Username}")
        | None -> prop.text "Loading data..."
      ]
    ]