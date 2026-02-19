namespace Lumium.Application.Common.Models;

/// <summary>
/// Generički Result wrapper za Command/Query rezultate
/// </summary>
public class Result
{
    public bool IsSuccess { get; }
    public string Message { get; }
    public List<string> Errors { get; }

    protected Result(bool isSuccess, string message, List<string>? errors = null)
    {
        IsSuccess = isSuccess;
        Message = message;
        Errors = errors ?? [];
    }

    public static Result Success(string message = "Operacija uspešno izvršena")
        => new(true, message);

    public static Result Failure(string error)
        => new(false, error, [error]);

    public static Result Failure(List<string> errors)
        => new(false, "Operacija nije uspela", errors);

    public static Result Failure(string message, List<string> errors)
        => new(false, message, errors);
}

/// <summary>
/// Generički Result sa payload-om (data)
/// </summary>
public class Result<T> : Result
{
    public T? Data { get; }

    protected Result(bool isSuccess, string message, T? data = default, List<string>? errors = null)
        : base(isSuccess, message, errors)
    {
        Data = data;
    }

    public static Result<T> Success(T data, string message = "Operacija uspešno izvršena")
        => new(true, message, data);

    public new static Result<T> Failure(string error)
        => new(false, error, default, [error]);

    public new static Result<T> Failure(List<string> errors)
        => new(false, "Operacija nije uspela", default, errors);

    public new static Result<T> Failure(string message, List<string> errors)
        => new(false, message, default, errors);
}