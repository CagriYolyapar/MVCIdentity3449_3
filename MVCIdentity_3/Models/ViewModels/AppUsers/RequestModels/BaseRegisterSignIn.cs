namespace MVCIdentity_3.Models.ViewModels.AppUsers.RequestModels
{
    public abstract class BaseRegisterSignIn
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}
