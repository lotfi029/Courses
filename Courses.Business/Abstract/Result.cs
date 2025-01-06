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
        isSuccess 
        ? _value 
        : throw new InvalidOperationException();
}
//public class Result<TValue1, TValue2>(TValue1? value1, TValue2? value2, bool isSuccess, Error error) 
//    : Result<TValue1>(value1, isSuccess, error)
//{
//    private readonly TValue1? _value1 = value1;
//    private readonly TValue2? _value2 = value2;

//    public object GetValue()
//    {
//        if (_value1 is not null && _value2 is null)    
//            return _value1;
//        else if (_value2 is not null && _value1 is null)
//            return _value2;
//        else 
//            throw new InvalidOperationException();
//    }
//}