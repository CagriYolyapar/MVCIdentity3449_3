using FluentValidation;
using MVCIdentity_3.Models.ViewModels.AppUsers.RequestModels;

namespace MVCIdentity_3.Models.FluentValidation.AppUsers
{
    public class UserRegisterRequestModelValidator : RegisterSignInSharedValidator<UserRegisterRequestModel>
    {
        public UserRegisterRequestModelValidator()
        {
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Parolalar uyusmuyor").NotEmpty().WithMessage("Sifre tekrar alanı bos gecilemez");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Email formatında giriş yapınız").NotEmpty().WithMessage("Email alanı bos gecilemez").When(x => x.UserName == null);

            //When metodu sadece kendisinden önceki gelen validation kuralları icin gecerlidir... Sonraki Validation kurallarını etkilemez. Yani son kod satırımızda When metodu sadece UserName null oldugu zaman önceki kurallar calısır..
        }
    }
}
