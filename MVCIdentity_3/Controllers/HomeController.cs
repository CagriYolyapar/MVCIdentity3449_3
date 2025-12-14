using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCIdentity_3.Models.Entities;
using MVCIdentity_3.Models.ViewModels.AppUsers.RequestModels;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace MVCIdentity_3.Controllers
{
    //!!Identity sistemi aynı kullanıcı ismi ile kayıda izin vermez ama varsayılan olarak aynı email'e sahip baska bir kullanıcıya kayda izin verebiliyor...Onu engellemek istiyorsanız middleware'daki identity ayarlarına otomatik yazmanız lazım...


    //Adminlerimiz
    //cgr , cgr123 
    //adm , adm123
    //kutlay123 , ktl123

    //Memberimiz
    //cagri, 123
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IValidator<UserRegisterRequestModel> _userRegisterRequestValidator;
        private readonly IValidator<UserSignInRequestModel> _userSignInRequestValidator;

        public HomeController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager, IValidator<UserRegisterRequestModel> userRegisterRequestValidator, IValidator<UserSignInRequestModel> userSignInRequestValidator)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userRegisterRequestValidator = userRegisterRequestValidator;
            _userSignInRequestValidator = userSignInRequestValidator;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterRequestModel model)
        {
            ValidationResult validationResult = await _userRegisterRequestValidator.ValidateAsync(model);

            if (validationResult.IsValid)
            {
                AppUser appUser = new()
                {
                    UserName = model.UserName,
                    Email = model.Email
                };

                //Identity'e bu şekilde password verdiginiz takdirde artık gönderilen şifre onun kurallarına göre hashlenir

                IdentityResult identityResult = await _userManager.CreateAsync(appUser, model.Password);

                if (identityResult.Succeeded)
                {
                    #region AdminEklemek
                    //if (await _roleManager.FindByNameAsync("Admin") == null) await _roleManager.CreateAsync(new() { Name = "Admin" });

                    //await _userManager.AddToRoleAsync(appUser, "Admin");
                    #endregion

                    #region MemberEklemek
                    if (await _roleManager.FindByNameAsync("Member") == null) await _roleManager.CreateAsync(new()
                    {
                        Name = "Member"
                    });

                    await _userManager.AddToRoleAsync(appUser, "Member");
                    #endregion

                    TempData["message"]= $"{appUser.UserName} isimli kullanıcı basarıyla kayıt oldu";
                    return RedirectToAction("Register");
                }

                foreach (IdentityError identityError in identityResult.Errors)
                {
                    ModelState.AddModelError("", identityError.Description);
                }
            }
            else
            {
                foreach(ValidationFailure validationError in validationResult.Errors)
                {
                    ModelState.AddModelError(validationError.PropertyName,validationError.ErrorMessage);
                }
            }

            return View(model);
        }

        public IActionResult SignIn(string? returnUrl)
        {
            UserSignInRequestModel usModel = new()
            {
                ReturnUrl = returnUrl
            };

            return View(usModel);
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(UserSignInRequestModel model)
        {
            ValidationResult validationResult = _userSignInRequestValidator.Validate(model);
            if (validationResult.IsValid)
            {
                AppUser appUser = await _userManager.FindByNameAsync(model.UserName);
                if(appUser == null)
                {
                    TempData["message"] = "Kullanıcı bulunamadı";
                    return RedirectToAction("SignIn", new { returnUrl = model.ReturnUrl });
                }

                SignInResult signInResult = await _signInManager.PasswordSignInAsync(appUser, model.Password, model.RememberMe, true);
                if (signInResult.Succeeded) 
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    IList<string> userRoles = await _userManager.GetRolesAsync(appUser);
                    if (userRoles.Contains("Admin")) return RedirectToAction("AdminPanel", "Auth");
                    else if (userRoles.Contains("Member")) return RedirectToAction("MemberPanel");
                    return RedirectToAction("Panel");
                }
                else if (signInResult.IsLockedOut)
                {
                    DateTimeOffset? lockOutEndDate = await _userManager.GetLockoutEndDateAsync(appUser);
                    ModelState.AddModelError("", $"Hesabınız {(lockOutEndDate.Value.UtcDateTime - DateTimeOffset.UtcNow).Minutes} dakika süreyle askıya alınmıstır");

                }
                else
                {
                    int maxFailedAttempts = _userManager.Options.Lockout.MaxFailedAccessAttempts; //kullanıcının kac kez yanlıs giriş yapabilecegini söyler
                    string message = $"Eger {maxFailedAttempts - await _userManager.GetAccessFailedCountAsync(appUser)} kez daha yanlıs yaparsanız hesabınız gecici olarak askıya alınacaktır";
                    ModelState.AddModelError("", message);
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
    }
}