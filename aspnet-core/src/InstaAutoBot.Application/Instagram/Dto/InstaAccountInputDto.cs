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
    public class InstaAccountInputDto : PagedUserResultRequestKeywordDto
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
