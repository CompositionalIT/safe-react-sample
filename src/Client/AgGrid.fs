module Fable.AgGrid

open Fable.Core
open Fable.Core.JsInterop
open Fable.Helpers.React
open Fable.Import.React

type ColumnDef =
    { headerName : string
      field : string
      sortable : bool
      filter : bool;
      checkboxSelection : bool }
    static member Create headerName field = { headerName = headerName; field = field; sortable = false; filter = false; checkboxSelection = false }

type Props =
    | ColumnDefs of ColumnDef array
    | RowData of obj array

let inline grid (props : Props list) (elems : ReactElement list) : ReactElement =
    ofImport "AgGridReact" "ag-grid-react" (keyValueList CaseRules.LowerFirst props) elems