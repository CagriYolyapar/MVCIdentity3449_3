using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MVCIdentity_3.Models.Entities;

namespace MVCIdentity_3.Areas.Administrator.CustomTagHelpers
{
    [HtmlTargetElement("getUserRoles")] //tag helper'inizin html'de nasıl cagrılacagına dair olusturdugunuz bir Html elementi olacaktır...
    public class AdminRolesResponseTagHelper : TagHelper
    {
        private readonly UserManager<AppUser> _userManager;

        public AdminRolesResponseTagHelper(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        //Eger bir CustomTagHelper'i cagırırken parametre verecekseniz onun öyle bir Property'si olması gerekir
        public int UserId { get; set; }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) //Bu TagHelper cagrıldıgı zaman onun görevini uygulayacak burasıdır
        {
            string html = "";
            IList<string> userRoles = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(UserId.ToString()));

            foreach (string role in userRoles)
            {
                html += $"{role},";
            }

            html = html.TrimEnd(',');

            output.Content.SetHtmlContent(html);
        }
    }
}
