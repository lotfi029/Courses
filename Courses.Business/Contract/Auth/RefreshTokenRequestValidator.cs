namespace Courses.Business.Contract.Auth;
public sealed class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(e => e.RefreshToken)
            .NotEmpty()
            .Length(1, int.MaxValue);
        
        RuleFor(e => e.Token)
            .NotEmpty()
            .Length(1, int.MaxValue);
    }
}
