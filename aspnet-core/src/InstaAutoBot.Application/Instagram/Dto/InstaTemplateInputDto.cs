using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using InstaAutoBot.Users.Dto;

namespace InstaAutoBot.Instagram.Dto
{
    public class InstaTemplateInputDto
    {
        public long InstaAccountId { get; set; }
        public string Name { get; set; }
        public string ZipFileName { get; set; }
        public int PostsIntervalInHours { get; set; }

        public int StoriesIntervalInHours { get; set; }

        public List<string> TemplateTags { get; set; }
    }
    public class InstaTemplateUpdateInputDto: InstaTemplateInputDto
    {
        public long Id { get; set; }
    }

    public class InstaTemplateOutputDto : InstaTemplateUpdateInputDto
    {
    }

    public class InstaSummaryDto
    {
        public int TotalInstaAccounts { get; set; }
        public int TotalMessages { get; set; }
    }
}
