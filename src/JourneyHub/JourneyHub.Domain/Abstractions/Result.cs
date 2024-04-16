using JourneyHub.Application.DTOs;
using JourneyHub.Domain.Abstractions;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace JourneyHub.Application.DTOs;

public class Result
{
    protected internal Result(bool isSuccess, ErrorType error)
    {
        if (isSuccess && error != ErrorType.None)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && error == ErrorType.None)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public ErrorType Error { get; }

    public static Result Success() => new(true, ErrorType.None);

    public static Result Failure(ErrorType error) => new(false, error);

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, ErrorType.None);

    public static Result<TValue> Failure<TValue>(ErrorType error) => new(default, false, error);

    public static Result<TValue> Create<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(ErrorType.NullValue);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    public Result(TValue? value, bool isSuccess, ErrorType error) : base(isSuccess, error)
    {
        _value = value;
    }

    [NotNull]
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    public static implicit operator Result<TValue>(TValue? value) => Create(value);
}
