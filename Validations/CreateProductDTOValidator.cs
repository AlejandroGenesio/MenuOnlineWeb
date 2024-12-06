using FluentValidation;
using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Repositories;

namespace MenuOnlineUdemy.Validations
{
    public class CreateProductDTOValidator: AbstractValidator<CreateProductDTO>
    {
        public CreateProductDTOValidator(IRepositoryProducts repositoryProducts, IHttpContextAccessor httpContextAccessor)
        {
            var routeValueId = httpContextAccessor.HttpContext?.Request.RouteValues["id"];
            var id = 0;

            if (routeValueId is string stringValue)
            {
                int.TryParse(stringValue, out id);
            }

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Error, please add {PropertyName}.")
                .MaximumLength(50).WithMessage("Error, the value for {PropertyName} cannot be more than {MaxLength} characters.")
                .Must(FirstLetterUpperCase).WithMessage("First letter must be in upper case.")
                .MustAsync(async (name, _) =>
                {
                    var duplicationExists = await repositoryProducts.DuplicationExists(id, name);
                    return !duplicationExists;
                }).WithMessage("The value for {PropertyName} already exists.");
        }

        private bool FirstLetterUpperCase(string value)
        {
            if(string.IsNullOrEmpty(value)) return true;

            var firstLetter = value[0].ToString();
            return firstLetter == firstLetter.ToUpper();
        }
    }
}
