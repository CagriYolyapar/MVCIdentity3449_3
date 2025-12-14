using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCIdentity_3.Areas.Administrator.Models.AppRoles.RequestModels;
using MVCIdentity_3.Models.Entities;

namespace MVCIdentity_3.Areas.Administrator.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Administrator")]
    public class RoleController : Controller
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IValidator<CreateRoleRequestModel> _createRoleValidator;

        public RoleController(RoleManager<AppRole> roleManager, IValidator<CreateRoleRequestModel> createRoleValidator)
        {
            _roleManager = roleManager;
            _createRoleValidator = createRoleValidator;
        }

        public IActionResult Index()
        {
            return View(_roleManager.Roles.ToList());
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleRequestModel model)
        {
            ValidationResult validationResult = _createRoleValidator.Validate(model);
            if (validationResult.IsValid)
            {
                IdentityResult identityResult = await _roleManager.CreateAsync(new() { Name = model.RoleName });
                if (identityResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (IdentityError identityError in identityResult.Errors)
                {
                    ModelState.AddModelError("", identityError.Description);
                }
            }

            else
            {
                foreach (ValidationFailure validationFailure in validationResult.Errors)
                
                {
                    ModelState.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);
                }
            }
            return View(model);
        }
    }
}
