namespace UTB.Casino.Application.Common;

/// <summary>Outcome of an application service call (success or a list of error messages).</summary>
public class Result
{
    public bool Succeeded { get; init; }
    public IReadOnlyList<string> Errors { get; init; } = Array.Empty<string>();
    public string Error => string.Join(" ", Errors);

    public static Result Ok() => new() { Succeeded = true };
    public static Result Fail(params string[] errors) => new() { Errors = errors };
    public static Result Fail(IEnumerable<string> errors) => new() { Errors = errors.ToArray() };
}
