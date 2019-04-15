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

type Data = { name: string; uv: int; pv: int; amt: int }
open Fable.AgGrid

let init() = (), Cmd.none
let update _ _ = (), Cmd.none

let data =
    [| { name = "Page A"; uv = 4000; pv = 2400; amt = 2400 }
       { name = "Page B"; uv = 3000; pv = 1398; amt = 2210 }
       { name = "Page C"; uv = 2000; pv = 9800; amt = 2290 }
       { name = "Page D"; uv = 2780; pv = 3908; amt = 2000 }
       { name = "Page E"; uv = 1890; pv = 4800; amt = 2181 }
       { name = "Page F"; uv = 2390; pv = 3800; amt = 2500 }
       { name = "Page G"; uv = 3490; pv = 4300; amt = 2100 } |]
let view (model : _) (dispatch : _ -> unit) =
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
            Columns.columns [ ] [
                Column.column [ ] [
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
                Column.column [] [
                    div [ Id "myGrid"; Class "ag-theme-balham"; Style [ Height "200px"; Width "500px"  ] ] [
                        grid [
                            Props.ColumnDefs [|
                                { ColumnDef.Create "Name" "name" with filter = true; checkboxSelection = true }
                                { ColumnDef.Create "UV" "uv" with sortable = true }
                            |]
                            Props.RowData (data |> Array.map box)
                        ] [

                        ]
                    ]
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
