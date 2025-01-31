namespace Courses.Business.Abstract;
public class Result
{
    public Result(bool isSuccess, Error error)
    {
        if ((isSuccess && error != Error.Non) || (!isSuccess && error == Error.Non))
            throw new InvalidOperationException();

        IsSuccess = isSuccess;
        Error = error;
    }
    public bool IsSuccess {  get; set; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; set; }

    public static Result Success() => new(true, Error.Non);
    public static Result Failure(Error error)=> new(false,error);
    
    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.Non);
    public static Result<TValue> Failure<TValue>(Error error)=> new(default,false,error);
}
public class Result<TValue>(TValue? value, bool isSuccess, Error error) : Result(isSuccess, error)
{
    private readonly TValue? _value = value;

    public TValue? Value => 
        IsSuccess 
        ? _value 
        : throw new InvalidOperationException();
}
public class Result<TValue1, TValue2>(TValue1? value1, TValue2? value2, bool isSuccess, Error error) 
   : Result<TValue1>(value1, isSuccess, error)
{
    private readonly TValue1? _value1 = value1;
    private readonly TValue2? _value2 = value2;


    public static Result<TValue1, TValue2> Success(TValue1 value1, TValue2 value2) =>
        new(value1, value2, true, Error.Non);

    public new static Result<TValue1, TValue2> Failure(Error error) => new(default, default, false, error);
    public TValue2? Value2 => 
        IsSuccess 
        ? _value2 
        : throw new InvalidOperationException();
    
    public TValue1? Value1 => 
        IsSuccess 
        ? _value1 
        : throw new InvalidOperationException();
}