namespace MVCIdentity_3.Areas.Administrator.Models.AppRoles.ResponseModels
{
    public class AppRoleResponseModel
    {

        //Rol ataması istedigimizde kimlerin hangi rolde oldugunu görelim ki onları cıkartıp ekleyebilelim...

        public string RoleName { get; set; }
        public bool IsChecked { get; set; }

    }
}
