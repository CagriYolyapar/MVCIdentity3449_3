using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCIdentity_3.Areas.Administrator.Models.AppRoles.ResponseModels;
using MVCIdentity_3.Areas.Administrator.Models.AppUsers.RequestModels;
using MVCIdentity_3.Areas.Administrator.Models.PageVms;
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


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserRequestModel model)
        {
            ValidationResult validationResult = _createUserValidator.Validate(model);
            if (validationResult.IsValid)
            {
                AppUser appUser = new()
                {
                    UserName = model.UserName,
                    Email = model.Email
                };

                IdentityResult identityResult = await _userManager.CreateAsync(appUser,$"{appUser}123");
                if(identityResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(appUser, "Member");
                    return RedirectToAction("Index");
                }

                foreach(IdentityError identityError in identityResult.Errors)
                {
                    ModelState.AddModelError("", identityError.Description);
                }
            }
            else
            {
                foreach(ValidationFailure validationFailure in validationResult.Errors)
                {
                    ModelState.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);
                }
            }

            return View(model);
        }


        public async Task<IActionResult> AssignRole(int id)
        {
            AppUser appUser = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == id);

            //Öncelikle user'in rollerini bir ele geciriyoruz
            IList<string> userRoles = await _userManager.GetRolesAsync(appUser);

            //Sonra tüm rolleri ele geciriyoruz
            List<AppRole> allRoles = await _roleManager.Roles.ToListAsync();

            //Sonra yeni bir liste acıyoruz ki User'da hangi roller varsa allRoles'daki roller checkbox'ta checkli gelsin

            List<AppRoleResponseModel> responseRoles = new();

            foreach (AppRole item in allRoles)
            {
                responseRoles.Add(new()
                {
                    RoleName = item.Name,
                    IsChecked = userRoles.Contains(item.Name)
                });
            }

            AssignRolePageVm aRPVm = new()
            {
                Roles = responseRoles,
                UserId = id
            };

            return View(aRPVm);

        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(AssignRolePageVm model)
        {
            AppUser appUser = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == model.UserId);
            IList<string> userRoles = await _userManager.GetRolesAsync(appUser);

            foreach (var item in model.Roles)
            {
                if (!item.IsChecked && userRoles.Contains(item.RoleName)) await _userManager.RemoveFromRoleAsync(appUser, item.RoleName);
                else if(item.IsChecked && !userRoles.Contains(item.RoleName)) await _userManager.AddToRoleAsync(appUser, item.RoleName);
            }

            return RedirectToAction("Index");
        }
    }
}
