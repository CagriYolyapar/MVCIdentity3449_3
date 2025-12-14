namespace MVCIdentity_3.Models.ViewModels.AppUsers.RequestModels
{
    public class UserRegisterRequestModel : BaseRegisterSignIn
    {
        public string? Email { get; set; }
        public string? ConfirmPassword { get; set; }

    }
}
