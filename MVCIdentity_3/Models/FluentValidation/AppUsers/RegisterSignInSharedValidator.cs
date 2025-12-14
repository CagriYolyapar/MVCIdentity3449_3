using FluentValidation;
using MVCIdentity_3.Models.ViewModels.AppUsers.RequestModels;

namespace MVCIdentity_3.Models.FluentValidation.AppUsers
{
    public  class RegisterSignInSharedValidator<T> : AbstractValidator<T> where T:BaseRegisterSignIn
    {
        public RegisterSignInSharedValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Kullanıcı ismi bos gecilemez");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Sifre alanı bos gecilemez").MinimumLength(3).WithMessage("Parola alanı minimum 3 karakterde olusmalıdır");
        }
    }
}
