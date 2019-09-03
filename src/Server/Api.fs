module Api

open Shared

let isInvalidHeight bodyDesc =
  let height = float bodyDesc.height
  let isInvalid =
    System.Double.IsNaN height ||
    System.Double.IsInfinity height ||
    height <= 0.0

  if isInvalid
  then Error "Height is invalid"
  else Ok bodyDesc

let isInvalidWeight bodyDesc =
  let weight = float bodyDesc.weight
  let isInvalid =
    System.Double.IsNaN weight ||
     System.Double.IsInfinity weight ||
     weight <= 0.0

  if isInvalid
  then Error "Weight is invalid"
  else Ok bodyDesc

let isInvalidResp bmi =
  let isInvalid =
    System.Double.IsNaN bmi.value ||
      System.Double.IsInfinity bmi.value ||
      bmi.value < 0.0

  if isInvalid
  then Error "Result is invalid"
  else Ok bmi

let calculateBmi bodyDesc =
  // See formula found here:
  // https://www.cdc.gov/healthyweight/assessing/bmi/adult_bmi/index.html
  let weight = float bodyDesc.weight
  let height = float bodyDesc.height
  let result = 703.0 * weight / (height ** 2.0)
  Ok <| { value = System.Math.Round(result, 1) }

let determineBmi bodyDesc =
  async {
    let combinedBmiCalc =
      isInvalidHeight
      >> Result.bind isInvalidWeight
      >> Result.bind calculateBmi
      >> Result.bind isInvalidResp

    return combinedBmiCalc bodyDesc
  }

let bmiApi: IBmiProtocol = {
  calculate = determineBmi
}