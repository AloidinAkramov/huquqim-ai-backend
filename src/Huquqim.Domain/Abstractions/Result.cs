namespace Huquqim.Domain.Abstractions;

/// <summary>
/// Operatsiya natijasi. Istisno (exception) o'rniga ishlatiladi.
/// </summary>
public record Result
{
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
            throw new InvalidOperationException("Muvaffaqiyatli natijada xatolik bo'lishi mumkin emas.");

        if (!isSuccess && error == Error.None)
            throw new InvalidOperationException("Muvaffaqiyatsiz natijada xatolik ko'rsatilishi shart.");

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public static Result Success() => new(true, Error.None);

    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

    public static implicit operator Result(Error error) => Failure(error);
}

/// <summary>
/// Ma'lumot qaytaruvchi operatsiya natijasi.
/// </summary>
public record Result<TValue> : Result
{
    private readonly TValue? _value;

    public Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    public TValue Data => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Muvaffaqiyatsiz natijaning ma'lumotini olib bo'lmaydi.");

    public static implicit operator Result<TValue>(TValue value) => Success(value);

    public static implicit operator Result<TValue>(Error error) => Failure<TValue>(error);
}
