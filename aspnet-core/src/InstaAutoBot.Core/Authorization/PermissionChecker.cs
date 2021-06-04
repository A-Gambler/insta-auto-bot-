using Abp.Authorization;
using InstaAutoBot.Authorization.Roles;
using InstaAutoBot.Authorization.Users;

namespace InstaAutoBot.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
