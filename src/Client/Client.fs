module Client

open Elmish
open Elmish.React
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.Recharts
open Fable.Recharts.Props
open Fable.PowerPack.Fetch
open Fulma
open Shared
open Thoth.Json

module P = Fable.Helpers.React.Props

type Model = { Counter: Counter option }

type Msg =
| Increment
| Decrement
| InitialCountLoaded of Result<Counter, exn>

let initialCounter = fetchAs<Counter> "/api/init" (Decode.Auto.generateDecoder())

let init () : Model * Cmd<Msg> =
    let initialModel = { Counter = None }
    let loadCountCmd = Cmd.ofPromise initialCounter [] (Ok >> InitialCountLoaded) (Error >> InitialCountLoaded)
    initialModel, loadCountCmd

let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> =
    match currentModel.Counter, msg with
    | Some counter, Increment ->
        let nextModel = { currentModel with Counter = Some { Value = counter.Value + 1 } }
        nextModel, Cmd.none
    | Some counter, Decrement ->
        let nextModel = { currentModel with Counter = Some { Value = counter.Value - 1 } }
        nextModel, Cmd.none
    | _, InitialCountLoaded (Ok initialCount)->
        let nextModel = { Counter = Some initialCount }
        nextModel, Cmd.none
    | _ ->
        currentModel, Cmd.none

let show = function
| { Counter = Some counter } -> string counter.Value
| { Counter = None   } -> "Loading..."

let button txt onClick =
    Button.button
        [ Button.IsFullWidth
          Button.Color IsPrimary
          Button.OnClick onClick ]
        [ str txt ]

type Data = { name: string; uv: int; pv: int; amt: int }
let data =
    [| { name = "Page A"; uv = 4000; pv = 2400; amt = 2400 }
       { name = "Page B"; uv = 3000; pv = 1398; amt = 2210 }
       { name = "Page C"; uv = 2000; pv = 9800; amt = 2290 }
       { name = "Page D"; uv = 2780; pv = 3908; amt = 2000 }
       { name = "Page E"; uv = 1890; pv = 4800; amt = 2181 }
       { name = "Page F"; uv = 2390; pv = 3800; amt = 2500 }
       { name = "Page G"; uv = 3490; pv = 4300; amt = 2100 } |]
let view (model : Model) (dispatch : Msg -> unit) =
    div [] [
        Navbar.navbar [
            Navbar.Color IsPrimary
        ] [
            Navbar.Item.div [ ] [
                Heading.h2 [ ] [
                    str "SAFE Template"
                ]
            ]
        ]

        Container.container [] [
            Content.content [
                Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ]
                ] [
                Heading.h3 [] [
                    str ("Press buttons to manipulate counter: " + show model)
                ]
            ]

            Columns.columns [] [
                Column.column [] [ button "-" (fun _ -> dispatch Decrement) ]
                Column.column [] [ button "+" (fun _ -> dispatch Increment) ]
            ]

            Content.content [] [
                lineChart [
                    Chart.Width 600.
                    Chart.Height 300.
                    Chart.Data data
                ] [
                    line [
                        Cartesian.Type Monotone
                        Cartesian.DataKey "uv"
                        P.Stroke "#8884d8"
                        P.StrokeWidth 2.
                    ] []
                    cartesianGrid [
                        P.Stroke "#ccc"
                        P.StrokeDasharray "5 5"
                    ] []
                    xaxis [ Cartesian.DataKey "name" ] []
                    yaxis [] []
                    tooltip [] []
                ]

            ]
        ]

        Footer.footer [ ] [
            Content.content [
                Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ]
            ] [

            ]
        ]
    ]

open Elmish.Debug
open Elmish.HMR

Program.mkProgram init update view
|> Program.withConsoleTrace
|> Program.withHMR
|> Program.withReact "elmish-app"
|> Program.withDebugger
|> Program.run
