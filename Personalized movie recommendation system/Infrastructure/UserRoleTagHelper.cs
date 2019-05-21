using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Personalized_movie_recommendation_system.Models;

namespace Personalized_movie_recommendation_system.Infrastructure
{
    [HtmlTargetElement("td", Attributes = "identity-role")]
    public class UserRoleTagHelper : TagHelper
    {
        private UserManager<User> userManager;

        public UserRoleTagHelper(UserManager<User> usermgr)
        {
            userManager = usermgr;
        }

        [HtmlAttributeName("identity-role")]
        public string UserId { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            User user = await userManager.FindByIdAsync(UserId);

            List<string> roles = new List<string>();
            if (user != null)
            {
                roles = (List<string>)await userManager.GetRolesAsync(user);
            }

            output.Content.SetContent(roles.Count == 0 ? "No Roles" : string.Join(", ", roles));

        }
    }
}
