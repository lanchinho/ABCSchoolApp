using ABCShared.Library.Models.Requests.Schools;
using FluentValidation;

namespace App.Infrastructure.Validators;

public class CreateSchoolRequestValidator : AbstractValidator<CreateSchoolRequest>
{
    public CreateSchoolRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()            
            .WithMessage("School Name is Required!");
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue
        => async (requestModel, propertyName) =>
        {
            var validationResult = await ValidateAsync(ValidationContext<CreateSchoolRequest>
                .CreateWithOptions((CreateSchoolRequest)requestModel, x => x.IncludeProperties(propertyName)));

            if (validationResult.IsValid)
                return [];

            return validationResult.Errors.Select(e => e.ErrorMessage);
        };
}
