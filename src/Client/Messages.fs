module Messages

open Shared

type Msg =
  | UpdateHeight of int
  | UpdateWeight of int
  | Calculate
  | ShowCalcuationReuslt of Bmi
  | ShowCalculationError of string