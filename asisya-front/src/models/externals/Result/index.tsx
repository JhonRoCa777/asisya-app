export interface Result<T> {
  isSuccess: boolean,
  data: T,
  error: number
}