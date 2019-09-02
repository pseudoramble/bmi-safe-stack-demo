module Api

open Shared

let calculateBmi weight height =
  let result = 703.0 * weight / (height ** 2.0)
  System.Math.Round(result, 1)

let determineBmi bodyDesc =
  async {
    return {
      value = calculateBmi (float bodyDesc.weight) (float bodyDesc.height)
    }
  }

let bmiApi: IBmiProtocol = {
  calculate = determineBmi
}