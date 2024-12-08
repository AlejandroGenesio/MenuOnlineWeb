using FluentValidation;
using MenuOnlineUdemy.DTOs;

namespace MenuOnlineUdemy.Validations
{
    public class UsersCredentialDTOValidation: AbstractValidator<UsersCredentialDTO>
    {
        public UsersCredentialDTOValidation()
        {
                RuleFor(x => x.Email).NotEmpty().WithMessage("Please insert an email")
                .MaximumLength(150).WithMessage("This email is too long")
                .EmailAddress().WithMessage("This is not a valid format email");
        }
    }
}
