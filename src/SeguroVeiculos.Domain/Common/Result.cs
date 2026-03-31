namespace SeguroVeiculos.Domain.Common;

public enum ResultErrorType
{
    Validation,
    NotFound,
    BusinessRule
}

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? Error { get; }
    public ResultErrorType ErrorType { get; }

    private Result(T value)
    {
        IsSuccess = true;
        Value = value;
    }

    private Result(string error, ResultErrorType errorType)
    {
        IsSuccess = false;
        Error = error;
        ErrorType = errorType;
    }

    public static Result<T> Ok(T value) => new(value);

    public static Result<T> Fail(string error, ResultErrorType errorType = ResultErrorType.BusinessRule)
        => new(error, errorType);

    public static Result<T> NotFound(string error)
        => new(error, ResultErrorType.NotFound);

    public static Result<T> Validation(string error)
        => new(error, ResultErrorType.Validation);
}

