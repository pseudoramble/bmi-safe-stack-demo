namespace Shared

type BodyDescription = {
  height: int
  weight: int
}

type Bmi = {
  value: float
}

type IBmiProtocol = {
  calculate : BodyDescription -> Async<Result<Bmi, string>>
}