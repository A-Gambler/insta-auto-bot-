using System.Threading.Tasks;
using Abp.Application.Services;
using InstaAutoBot.Authorization.Accounts.Dto;

namespace InstaAutoBot.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
