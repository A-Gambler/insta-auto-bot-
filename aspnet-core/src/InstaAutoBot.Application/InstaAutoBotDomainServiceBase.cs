using Abp.Domain.Services;

namespace InstaAutoBot
{
    public abstract class InstaAutoBotDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected InstaAutoBotDomainServiceBase()
        {
            LocalizationSourceName = InstaAutoBotConsts.LocalizationSourceName;
        }
    }
}