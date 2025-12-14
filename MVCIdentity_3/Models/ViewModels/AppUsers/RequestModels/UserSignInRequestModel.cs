namespace MVCIdentity_3.Models.ViewModels.AppUsers.RequestModels
{
    public class UserSignInRequestModel : BaseRegisterSignIn
    {
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }

    }
}
