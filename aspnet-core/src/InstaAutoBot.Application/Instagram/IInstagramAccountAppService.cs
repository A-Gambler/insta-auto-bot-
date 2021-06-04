using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InstaAutoBot.Instagram.Dto;
using InstaAutoBot.Instagram.Storage.FileManagement;
using InstaAutoBot.Sessions.Dto;

namespace InstaAutoBot.Sessions
{
    public interface IInstagramAccountAppService : IApplicationService
    {
        Task CreateInstagramBulkAccounts(int limit);

        Task<PagedResultDto<InstaAccountOutputDto>> GetInstaAccounts(InstaAccountInputDto input);
        Task TestInstaDataFileType(InstaDataFileType input);

        Task UpdateInstaTemplate(InstaTemplateUpdateInputDto input);

        Task CreateInstaTemplate(InstaTemplateInputDto input);

        Task DeleteInstaTemplate(long id);

        Task SendInstaMessage(InstaMessageInputDto input);

        Task<InstaTemplateUpdateInputDto> GetInstaTemplateByInstaAccountId(long instaAccountId);

        Task<InstaSummaryDto> GetInstaDashboardSummary();

        Task CreateOrUpdateInstaSettings(InstaSettingUpdateInputDto input);

        Task<InstaSettingUpdateInputDto> GetInstaSetting();
    }
}
