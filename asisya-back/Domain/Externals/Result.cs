using Domain.Errors;

namespace Domain.Externals
{
    public class Result<T>(bool isSuccess, T data, BaseError error)
    {
        public bool IsSuccess { get; } = isSuccess;
        public T Data { get; } = data;
        public BaseError Error { get; } = error;

        public static Result<T> Success(T data)
            => new(true, data, default!);

        public static Result<T> Failure(BaseError error)
            => new(false, default!, error);
    }
}
