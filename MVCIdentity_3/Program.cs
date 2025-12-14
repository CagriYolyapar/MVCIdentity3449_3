using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MVCIdentity_3.Areas.Administrator.Models.AppRoles.RequestModels;
using MVCIdentity_3.Areas.Administrator.Models.AppUsers.RequestModels;
using MVCIdentity_3.Areas.Administrator.Models.FluentValidation.AppRoles;
using MVCIdentity_3.Areas.Administrator.Models.FluentValidation.AppUsers;
using MVCIdentity_3.Models.ContextClasses;
using MVCIdentity_3.Models.Entities;
using MVCIdentity_3.Models.FluentValidation.AppUsers;
using MVCIdentity_3.Models.ViewModels.AppUsers.RequestModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddIdentity<AppUser, AppRole>(opt =>
{
    opt.Password.RequiredLength = 3;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireDigit = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.User.RequireUniqueEmail = true; //Aynı Email'e izin verilmez

}).AddEntityFrameworkStores<MyContext>(); //EntityFrameworkStores'a eklemeyi unutmayın...

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.Cookie.HttpOnly = true;
    opt.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    opt.SlidingExpiration = true;
    opt.Cookie.Name = "CyberSelf";
    opt.Cookie.SameSite = SameSiteMode.Strict;
    opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    opt.LoginPath = new PathString("/Home/SignIn");
    opt.AccessDeniedPath = new PathString("/Home/AccessDenied");
    
});

//Fluent Validation'da Validator IOC Container icin Transient metodunu saglıklı görürüz...

builder.Services.AddTransient<IValidator<UserRegisterRequestModel>, UserRegisterRequestModelValidator>();
builder.Services.AddTransient<IValidator<UserSignInRequestModel>, UserSignInRequestModelValidator>();
builder.Services.AddTransient<IValidator<CreateUserRequestModel>, CreateUserRequestModelValidator>();
builder.Services.AddTransient<IValidator<CreateRoleRequestModel>, CreateRoleRequestModelValidator>();



builder.Services.AddDbContext<MyContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")).UseLazyLoadingProxies());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "Administrator",
    pattern: "{area}/{controller}/{action}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Register}/{id?}");

app.Run();
