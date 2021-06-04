using Abp.Application.Services;
using InstaAutoBot.MultiTenancy.Dto;

namespace InstaAutoBot.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

