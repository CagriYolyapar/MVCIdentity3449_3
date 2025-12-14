using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCIdentity_3.Areas.Administrator.Models.AppUsers.RequestModels;
using MVCIdentity_3.Models.Entities;

namespace MVCIdentity_3.Areas.Administrator.Controllers
{
    [Authorize(Roles ="Admin")]
    [Area("Administrator")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IValidator<CreateUserRequestModel> _createUserValidator;

        public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IValidator<CreateUserRequestModel> createUserValidator)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _createUserValidator = createUserValidator;
        }

        public IActionResult Index()
        {
            List<AppUser> allUsers = _userManager.Users.ToList();
            return View(allUsers);
        }
    }
}
