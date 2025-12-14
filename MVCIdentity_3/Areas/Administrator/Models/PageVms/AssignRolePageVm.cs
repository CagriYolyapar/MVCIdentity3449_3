using MVCIdentity_3.Areas.Administrator.Models.AppRoles.ResponseModels;

namespace MVCIdentity_3.Areas.Administrator.Models.PageVms
{
    public class AssignRolePageVm
    {
        public int UserId { get; set; }
        public List<AppRoleResponseModel> Roles { get; set; }
    }
}
