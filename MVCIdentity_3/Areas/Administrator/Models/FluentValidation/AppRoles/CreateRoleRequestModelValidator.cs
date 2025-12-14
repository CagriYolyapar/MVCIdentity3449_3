using FluentValidation;
using MVCIdentity_3.Areas.Administrator.Models.AppRoles.RequestModels;

namespace MVCIdentity_3.Areas.Administrator.Models.FluentValidation.AppRoles
{
    public class CreateRoleRequestModelValidator : AbstractValidator<CreateRoleRequestModel>
    {
        public CreateRoleRequestModelValidator()
        {
            RuleFor(x => x.RoleName).NotEmpty().WithMessage("Rol ismi bos gecilemez");
        }
    }
}
