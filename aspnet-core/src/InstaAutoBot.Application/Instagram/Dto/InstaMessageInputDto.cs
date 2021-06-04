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
    public class InstaMessageInputDto
    {
        public long InstaAccountId { get; set; }
        public string TextMessage { get; set; }
        public List<string> MessageTags { get; set; }
    }
    public class InstaMessageUpdateInputDto : InstaMessageInputDto
    {
        public long Id { get; set; }
    }
}
