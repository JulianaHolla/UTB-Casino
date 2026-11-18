namespace UTB.Casino.Application.Common;

public class Result<T> : Result
{
    public T? Value { get; init; }
    public static Result<T> Ok(T value) => new() { Succeeded = true, Value = value };
    public new static Result<T> Fail(params string[] errors) => new() { Errors = errors };
}
