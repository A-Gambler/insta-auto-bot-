using System.Threading.Tasks;
using Abp.Application.Services;
using InstaAutoBot.Sessions.Dto;

namespace InstaAutoBot.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
