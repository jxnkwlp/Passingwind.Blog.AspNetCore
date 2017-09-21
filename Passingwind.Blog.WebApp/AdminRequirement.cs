using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Passingwind.Blog.WebApp
{
    public class AdminRequirement : IAuthorizationRequirement
    {
    }

    public class AdminAuthorizationHandler : AuthorizationHandler<AdminRequirement>, IAuthorizationHandler
    {
        private readonly UserManager _userManager;

        public AdminAuthorizationHandler(UserManager userManager)
        {
            this._userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRequirement requirement)
        {
            if (context.User == null || !context.User.Identity.IsAuthenticated)
            {
                // context.Fail();
                return;
            }

            var user = await _userManager.GetUserAsync(context.User);

            if (user == null)
            {
                // context.Fail();
                return;
            }

            var roles = await _userManager.GetRolesAsync(user);

            if (roles == null || roles.Count == 0)
            {
                // context.Fail();
                return;
            }

            if (roles.Contains(Role.Anonymous))
            {
                // context.Fail();
                return;
            }

            context.Succeed(requirement);
        }
    }
}
