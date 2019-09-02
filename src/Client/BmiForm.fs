module BmiForm

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props
open Fulma

type OnCalculate = (Browser.Types.MouseEvent -> unit)
type OnChange = (Browser.Types.Event -> unit)

type BmiFormProps = {
    onCalculate: OnCalculate;
    onChange: OnChange
  }

let form (props: BmiFormProps) = 
  div [ ] [
    Field.div [ ] [
      Label.label [] [ str "Height (in.)" ]
      Control.div [] [
        Input.number [ 
          Input.Placeholder "ex: 70";
          Input.OnChange props.onChange;
        ]
      ]
    ]

    Field.div [ ] [
      Label.label [] [ str "Weight (lbs.)" ]
      Control.div [] [
        Input.number [ Input.Placeholder "ex: 200" ]
      ]
    ]

    Field.div [] [
      Control.div [] [
        Button.button [ Button.Color IsPrimary; Button.OnClick props.onCalculate ] [
          str "Caclulate"
        ]
      ]
    ]
  ]