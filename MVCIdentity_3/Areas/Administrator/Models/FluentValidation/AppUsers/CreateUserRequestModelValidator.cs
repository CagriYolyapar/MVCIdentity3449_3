using FluentValidation;
using MVCIdentity_3.Areas.Administrator.Models.AppUsers.RequestModels;

namespace MVCIdentity_3.Areas.Administrator.Models.FluentValidation.AppUsers
{
    public class CreateUserRequestModelValidator : AbstractValidator<CreateUserRequestModel>
    {
        public CreateUserRequestModelValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Kullanıcı ismi bos gecilemez");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email alanı bos gecilemez").EmailAddress().WithMessage("Lütfen email formatını dogru giriniz");

        }
    }
}
