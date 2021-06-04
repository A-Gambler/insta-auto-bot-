using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace InstaAutoBot.Controllers
{
    public abstract class InstaAutoBotControllerBase: AbpController
    {
        protected InstaAutoBotControllerBase()
        {
            LocalizationSourceName = InstaAutoBotConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
