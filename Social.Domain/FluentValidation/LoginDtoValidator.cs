using FluentValidation;
using Social.Domain.Dto.Users;

namespace Social.Domain.FluentValidation;

public class LoginDtoValidator: AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email).EmailAddress().NotNull().NotEmpty();
        RuleFor(x => x.Password).NotNull().NotEmpty();
    }
}