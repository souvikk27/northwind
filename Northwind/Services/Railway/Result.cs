namespace Northwind.Services.Railway
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T Value { get; }
        public string Error { get; }

        private Result(bool isSuccess, T value, string error)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        public static Result<T> Success(T value) => new Result<T>(true, value, null!);
        public static Result<T> Failure(string error) => new Result<T>(false, default(T)!, error);

        public Result<TNew> Map<TNew>(Func<T, TNew> func) =>
            IsSuccess ? Result<TNew>.Success(func(Value)) : Result<TNew>.Failure(Error);

        public Result<TNew> Bind<TNew>(Func<T, Result<TNew>> func) =>
            IsSuccess ? func(Value) : Result<TNew>.Failure(Error);
        
        public async Task<Result<TNew>> MapAsync<TNew>(Func<T, Task<TNew>> func) =>
            IsSuccess ? Result<TNew>.Success(await func(Value)) : Result<TNew>.Failure(Error);
        public async Task<Result<TNew>> BindAsync<TNew>(Func<T, Task<Result<TNew>>> func) =>
            IsSuccess ? await func(Value) : Result<TNew>.Failure(Error);
    }
}