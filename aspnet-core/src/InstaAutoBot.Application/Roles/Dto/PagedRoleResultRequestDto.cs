using Abp.Application.Services.Dto;

namespace InstaAutoBot.Roles.Dto
{
    public class PagedRoleResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}

